using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network.ZeroMemoryAllocSample
{
    public class NetworkMemoryTest : MonoBehaviour
    {
        private void Start()
        {
            NetworkRequest request = new NetworkRequest("https://httpbin.org/bytes/1048576", 1.0f, System.Net.Http.HttpMethod.Get);

            StartNetworkMemoryTest(request);
        }

        private async void StartNetworkMemoryTest(NetworkRequest request)
        {
            Debug.Log("NetworkMemoryTest Start");

            for (int i = 0; i < 10; i++)
            {
                Debug.Log($"{i}번째 Request");
                SendWebRequestAsync(request).Forget();

                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }

        private async UniTask<NetworkResult> SendWebRequestAsync(NetworkRequest request)
        {
            if (request == null)
            {
                Debug.LogError("[Network] 요청 객체가 null입니다.");
                return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError, "{\"error\":\"Request object is null\"}");
            }

            try
            {
                using var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(request.Timeout));

                UnityWebRequest uwr = null;
                if (request.Method == System.Net.Http.HttpMethod.Get)
                {
                    uwr = UnityWebRequest.Get(request.Url);
                }
                else if (request.Method == System.Net.Http.HttpMethod.Post)
                {
                    uwr = UnityWebRequest.Post(request.Url, request.Body ?? "", "application/json");
                }
                else
                {
                    throw new NotSupportedException($"지원하지 않는 HTTP 메서드: {request.Method}");
                }

                var operation = await uwr.SendWebRequest().WithCancellation(cancellationTokenSource.Token);

#if UNITY_2020_1_OR_NEWER
                bool isNetworkError = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError;
#else
        bool isNetworkError = uwr.isNetworkError || uwr.isHttpError;
#endif

                int statusCode = (int)uwr.responseCode;
                string responseText = uwr.downloadHandler?.text ?? "";

                if (isNetworkError)
                {
                    Debug.LogError($"[Network] 네트워크 오류: {uwr.error}");
                    return new NetworkResult(statusCode, responseText);
                }

                return new NetworkResult(statusCode, responseText);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Network] 요청 실행 중 오류 발생: {ex.Message}");
                return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError, $"{{\"error\":\"{ex.Message}\"}}");
            }
        }

        private async UniTask<NetworkResult> SendWebRequestAsync_With_ZeroAlloc(NetworkRequest request)
        {
            if (request == null)
            {
                Debug.LogError("[Network] 요청 객체가 null입니다.");
                return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError, "{\"error\":\"Request object is null\"}");
            }

            try
            {
                using var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(request.Timeout));

                UnityWebRequest uwr = null;
                if (request.Method == System.Net.Http.HttpMethod.Get)
                {
                    uwr = UnityWebRequest.Get(request.Url);
                }
                else if (request.Method == System.Net.Http.HttpMethod.Post)
                {
                    uwr = UnityWebRequest.Post(request.Url, request.Body ?? "", "application/json");
                }
                else
                {
                    throw new NotSupportedException($"지원하지 않는 HTTP 메서드: {request.Method}");
                }

                var operation = await uwr.SendWebRequest().WithCancellation(cancellationTokenSource.Token);

#if UNITY_2020_1_OR_NEWER
                bool isNetworkError = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError;
#else
        bool isNetworkError = uwr.isNetworkError || uwr.isHttpError;
#endif

                int statusCode = (int)uwr.responseCode;

                // 핵심: NativeArray<byte>를 직접 반환
                NativeArray<byte>.ReadOnly nativeData = default;
                if (uwr.downloadHandler != null)
                {
                    nativeData = uwr.downloadHandler.nativeData;
                }

                if (isNetworkError)
                {
                    Debug.LogError($"[Network] 네트워크 오류: {uwr.error}");
                    return new NetworkResult(statusCode, nativeData);
                }

                return new NetworkResult(statusCode, nativeData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Network] 요청 실행 중 오류 발생: {ex.Message}");
                return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError, $"{{\"error\":\"{ex.Message}\"}}");
            }
        }
    }
}