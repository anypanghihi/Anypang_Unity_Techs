using System;

/// <summary>
/// 네트워크 서비스 제공자 - 애플리케이션 전체에서 네트워크 서비스 인스턴스를 관리합니다.
/// </summary>
public static class NetworkServiceProvider
{
    private static INetworkService defaultService;
    private static INetworkService currentService;

    /// <summary>
    /// 현재 네트워크 서비스 인스턴스를 가져옵니다.
    /// </summary>
    public static INetworkService GetService()
    {
        if (currentService == null)
        {
            if (defaultService == null)
            {
                defaultService = CreateDefaultService();
            }
            currentService = defaultService;
        }
        return currentService;
    }

    /// <summary>
    /// 네트워크 서비스 인스턴스를 설정합니다. 주로 테스트나 특수 상황에서 사용됩니다.
    /// </summary>
    public static void SetService(INetworkService service)
    {
        currentService = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// 기본 서비스로 되돌립니다.
    /// </summary>
    public static void ResetToDefault()
    {
        currentService = defaultService;
    }

    /// <summary>
    /// 기본 네트워크 서비스 인스턴스를 생성합니다.
    /// </summary>
    private static INetworkService CreateDefaultService()
    {
        var scheduler = NetworkManager.Instance.GetScheduler();
        return new NetworkService(scheduler);
    }
}