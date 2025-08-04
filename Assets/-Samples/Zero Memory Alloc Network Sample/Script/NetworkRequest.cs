using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Network.ZeroMemoryAllocSample
{
    /// <summary>
    /// 네트워크 요청 정보를 담는 클래스입니다.
    /// </summary>
    public class NetworkRequest : IComparer<NetworkRequest>, IDisposable
    {
        /// <summary>
        /// 요청 URL
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// 요청 간격(초)
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// 요청 타임아웃(초)
        /// </summary>
        public float Timeout { get; set; } = 10f;

        /// <summary>
        /// HTTP 메서드
        /// </summary>
        public System.Net.Http.HttpMethod Method { get; }

        /// <summary>
        /// 요청 헤더
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// 요청 본문
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// 재시도 횟수
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// 최대 재시도 횟수 (-1은 무한 재시도)
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// 재시도 지연 시간(초)
        /// </summary>
        public float RetryDelay { get; set; } = 1f;

        public int Priority { get; set; } = 0;

        /// <summary>
        /// 응답 완료 콜백
        /// </summary>
        public Action<NetworkResult> OnComplete { get; set; }

        /// <summary>
        /// 오류 발생 콜백
        /// </summary>
        public Action<Exception> OnError { get; set; }

        /// <summary>
        /// 추가 필요한 파라미터 ex) 쿼리 등
        /// </summary>
        //public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// 요청 생성 시간
        /// </summary>
        public float CreationTime { get; private set; }

        /// <summary>
        /// 마지막 시도 시간
        /// </summary>
        public float LastAttemptTime { get; private set; }

        /// <summary>
        /// 취소 토큰 소스
        /// </summary>
        private CancellationTokenSource _cancellationSource;

        /// <summary>
        /// 취소 토큰
        /// </summary>
        public CancellationToken CancellationToken => _cancellationSource?.Token ?? CancellationToken.None;

        /// <summary>
        /// 네트워크 요청을 초기화합니다.
        /// </summary>
        public NetworkRequest(string url, float interval, System.Net.Http.HttpMethod method, Action<NetworkResult> onComplete = null, Action<Exception> onError = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "URL은 null이거나 빈 문자열일 수 없습니다.");
            }

            Url = url;
            Interval = Mathf.Max(0.1f, interval); // 최소 간격 보장
            Method = method ?? System.Net.Http.HttpMethod.Get; // 기본값 설정
            Headers = new Dictionary<string, string>();
            OnComplete = onComplete;
            OnError = onError;
            CreationTime = Time.time;
            _cancellationSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 요청 시도를 기록합니다.
        /// </summary>
        public void RecordAttempt()
        {
            RetryCount++;
            LastAttemptTime = Time.time;
        }

        /// <summary>
        /// 요청 본문을 설정합니다.
        /// </summary>
        public NetworkRequest SetBody(string body)
        {
            Body = body;
            return this;
        }

        /// <summary>
        /// 헤더를 추가합니다.
        /// </summary>
        public NetworkRequest AddHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }


        /// <summary>
        /// 응답을 처리합니다.
        /// </summary>
        public void ProcessResponse(NetworkResult result)
        {
            if (result == null)
            {
                Debug.LogError("[Network] 응답 결과가 null입니다.");
                OnError?.Invoke(new NullReferenceException("응답 결과가 null입니다."));
                return;
            }
        }

        /// <summary>
        /// 요청의 해시 코드를 반환합니다.
        /// </summary>
        public override int GetHashCode()
        {
            return Url.GetHashCode() ^ Method.GetHashCode();
        }

        /// <summary>
        /// 요청이 다른 요청과 동일한지 비교합니다.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is NetworkRequest other)
            {
                return Url == other.Url && Method == other.Method;
            }
            return false;
        }

        /// <summary>
        /// 요청의 우선순위를 비교합니다.
        /// </summary>
        public int Compare(NetworkRequest x, NetworkRequest y)
        {
            return x.Priority.CompareTo(y.Priority);
        }

        /// <summary>
        /// 요청을 취소합니다.
        /// </summary>
        public void Cancel()
        {
            try
            {
                _cancellationSource?.Cancel();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Network] 요청 취소 중 오류 발생: {ex.Message}");
            }
        }

        /// <summary>
        /// 리소스를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            try
            {
                _cancellationSource?.Dispose();
                _cancellationSource = null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Network] 리소스 해제 중 오류 발생: {ex.Message}");
            }
        }
    }
}