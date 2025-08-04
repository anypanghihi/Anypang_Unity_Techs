using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("NetworkManager instance is null! Make sure it exists in the scene.");
            }

            return instance;
        }
    }

    public NetworkScheduler scheduler;
    private readonly Dictionary<NetworkRequest, float> lastExecution = new();
    private readonly List<NetworkRequest> toSend = new();
    private float checkTimer = 0f;
    private const float CHECK_INTERVAL = 0.1f;
    private const float MAX_REQUEST_AGE = 60f; // 1분

    /// <summary>
    /// 네트워크 스케줄러를 가져옵니다.
    /// </summary>
    /// <returns>네트워크 스케줄러 인스턴스</returns>
    public NetworkScheduler GetScheduler()
    {
        return scheduler;
    }

    private void Awake()
    {
        instance = this;
        scheduler = new NetworkScheduler();
    }

    private void Update()
    {
        if (!Application.isPlaying) return;

        checkTimer += Time.deltaTime;
        if (checkTimer < CHECK_INTERVAL) return;
        checkTimer = 0f;

        float now = Time.time;
        toSend.Clear();

        try
        {
            var requests = scheduler.GetAll();
            if (requests == null)
            {
                Debug.LogError("[Network] 스케줄러가 null 요청 목록을 반환했습니다.");
                return;
            }

            foreach (var req in requests)
            {
                if (req == null) continue; // null 요청 건너뛰기

                // if (req.Status == RequestStatus.Completed)
                //     continue;

                if (!lastExecution.TryGetValue(req, out float lastTime))
                {
                    // 최초 등록 시 바로 송신되도록 Interval만큼 이전으로 설정
                    lastExecution[req] = now - req.Interval;
                    lastTime = now - req.Interval;
                }

                if (now - lastTime >= req.Interval)
                {
                    toSend.Add(req);
                    lastExecution[req] = now;
                    req.Status = RequestStatus.Waiting;
                }
                // else if (req.Status == RequestStatus.Wating)
                // {
                //     // 대기 상태 유지
                // }
            }

            foreach (var req in toSend)
            {
                if (req != null) // null 체크 추가
                {
                    ExecuteRequestAsync(req).Forget();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] Update 중 오류 발생: {ex.Message}");
        }

        //CleanupTimer();
    }

    private float cleanupTimer = 0f;
    private const float CLEANUP_INTERVAL = 5f; // 5초
    private void CleanupTimer()
    {
        cleanupTimer += Time.deltaTime;
        if (cleanupTimer >= CLEANUP_INTERVAL)
        {
            CleanupCompletedRequests();
            cleanupTimer = 0f;
        }
    }

    private void CleanupCompletedRequests()
    {
        var keysToRemove = new List<NetworkRequest>();

        foreach (var kvp in lastExecution)
        {
            if (kvp.Key.Status == RequestStatus.Completed ||
                Time.time - kvp.Value > MAX_REQUEST_AGE)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            lastExecution.Remove(key);
        }
    }

    private async UniTask ExecuteRequestAsync(NetworkRequest request)
    {
        if (request == null)
        {
            Debug.LogError("[Network] 요청 객체가 null입니다.");
            return;
        }

        request.Status = RequestStatus.Processing;

        try
        {
            var result = await SendRequestAsync(request);

            if (result == null)
            {
                Debug.LogError($"[Network] {request.Url}에 대한 응답이 null입니다.");
                request.Status = RequestStatus.Failed;
                request.OnError?.Invoke(new NullReferenceException("응답 결과가 null입니다."));
                return;
            }

            if (result.IsSuccess)
            {
                request.Status = RequestStatus.Completed;
                request.ProcessResponse(result);
            }
            else
            {
                request.Status = RequestStatus.Failed;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] 요청 실행 중 오류 발생: {ex.Message}");
            request.Status = RequestStatus.Failed;
            request.OnError?.Invoke(ex);
        }
    }

    public async UniTask<NetworkResult> SendRequestAsync(NetworkRequest request)
    {
        if (request == null)
        {
            Debug.LogError("[Network] 요청 객체가 null입니다.");
            return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError);//, "{\"error\":\"Request object is null\"}");
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

            await uwr.SendWebRequest().WithCancellation(cancellationTokenSource.Token);

#if UNITY_2020_1_OR_NEWER
            bool isNetworkError = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError;
#else
            bool isNetworkError = uwr.isNetworkError || uwr.isHttpError;
#endif

            int statusCode = (int)uwr.responseCode;
            string responseText = uwr.downloadHandler?.text ?? "";

            // 핵심: NativeArray<byte>를 직접 반환
            // NativeArray<byte>.ReadOnly nativeData = default;
            // if (uwr.downloadHandler != null)
            // {
            //     nativeData = uwr.downloadHandler.nativeData;
            // }

            if (isNetworkError)
            {
                Debug.LogError($"[Network] 네트워크 오류: {uwr.error}");
                return new NetworkResult(statusCode);
            }

            return new NetworkResult(statusCode, request.TID, responseText);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Network] 요청 실행 중 오류 발생: {ex.Message}");
            return new NetworkResult((int)System.Net.HttpStatusCode.InternalServerError);//, $"{{\"error\":\"{ex.Message}\"}}");
        }
    }
}
