using System;
using System.Collections.Generic;

/// <summary>
/// 네트워크 요청을 스케줄링하고 관리하는 클래스입니다.
/// </summary>
public class NetworkScheduler : IRequestScheduler<NetworkRequest>
{
    private readonly List<NetworkRequest> requestList = new List<NetworkRequest>();

    /// <summary>
    /// 네트워크 요청을 등록합니다.
    /// </summary>
    /// <param name="request">등록할 네트워크 요청</param>
    public void Register(NetworkRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (!requestList.Contains(request))
        {
            requestList.Add(request);
            request.Status = RequestStatus.Waiting;
        }
    }

    /// <summary>
    /// 네트워크 요청을 등록 해제합니다.
    /// </summary>
    /// <param name="request">등록 해제할 네트워크 요청</param>
    public void Unregister(NetworkRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // 등록 리스트/딕셔너리에서 제거
        requestList.Remove(request);
        request.Dispose(); // 여기서만 Dispose!
    }

    /// <summary>
    /// 모든 등록된 요청을 가져옵니다.
    /// </summary>
    public IEnumerable<NetworkRequest> GetAll() => requestList;

    /// <summary>
    /// 모든 요청을 제거합니다.
    /// </summary>
    public void Clear() => requestList.Clear();
}
