using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 네트워크 디버깅을 위한 유틸리티 클래스입니다.
/// </summary>
public static class NetworkDebugger
{
    public static bool EnableVerboseLogging = false;

    private static List<NetworkRequestLog> _requestLogs = new List<NetworkRequestLog>(100);
    private static readonly object _logLock = new object();

    /// <summary>
    /// 요청 로그를 기록합니다.
    /// </summary>
    public static void LogRequest(NetworkRequest request, string action, string details = null)
    {
        if (!EnableVerboseLogging) return;

        lock (_logLock)
        {
            _requestLogs.Add(new NetworkRequestLog
            {
                Timestamp = DateTime.Now,
                Url = request.Url,
                Action = action,
                Details = details,
                Status = request.Status
            });

            // 로그 크기 제한
            if (_requestLogs.Count > 1000)
            {
                _requestLogs.RemoveRange(0, 500);
            }
        }
    }

    /// <summary>
    /// 최근 요청 로그를 반환합니다.
    /// </summary>
    public static NetworkRequestLog[] GetRecentLogs(int count = 100)
    {
        lock (_logLock)
        {
            return _requestLogs.OrderByDescending(l => l.Timestamp)
                              .Take(count)
                              .ToArray();
        }
    }

    /// <summary>
    /// 요청 로그 정보를 담는 구조체입니다.
    /// </summary>
    public struct NetworkRequestLog
    {
        public DateTime Timestamp;
        public string Url;
        public string Action;
        public string Details;
        public RequestStatus Status;
    }
}