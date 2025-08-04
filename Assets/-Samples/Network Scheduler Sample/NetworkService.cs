using System;

/// <summary>
/// 네트워크 서비스의 기본 구현입니다.
/// </summary>
public class NetworkService : INetworkService
{
    private readonly NetworkScheduler scheduler;
    private bool isDisposed;

    /// <summary>
    /// 네트워크 서비스를 초기화합니다.
    /// </summary>
    /// <param name="scheduler">네트워크 요청을 처리할 스케줄러</param>
    public NetworkService(IRequestScheduler<NetworkRequest> scheduler)
    {
        this.scheduler = (NetworkScheduler)scheduler ?? throw new ArgumentNullException(nameof(scheduler));
    }

    /// <summary>
    /// 네트워크 요청을 등록합니다.
    /// </summary>
    /// <param name="request">등록할 네트워크 요청</param>
    public void RegisterRequest(NetworkRequest request)
    {
        if (isDisposed) throw new ObjectDisposedException(nameof(NetworkService));
        scheduler.Register(request);
    }

    /// <summary>
    /// 네트워크 요청을 등록 해제합니다.
    /// </summary>
    /// <param name="request">등록 해제할 네트워크 요청</param>
    public void UnregisterRequest(NetworkRequest request)
    {
        if (isDisposed) throw new ObjectDisposedException(nameof(NetworkService));
        scheduler.Unregister(request);
    }

    /// <summary>
    /// 리소스를 해제합니다.
    /// </summary>
    public void Dispose()
    {
        if (!isDisposed)
        {
            // 정리 작업 수행
            isDisposed = true;
        }
    }
}
