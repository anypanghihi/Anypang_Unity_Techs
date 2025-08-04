# Network Scheduler Sample

## 프로젝트 소개
이 샘플은 Unity에서 네트워크 요청(REST API 등)을 주기적으로 자동 실행하고, 요청의 등록/해제, 상태 추적, 우선순위, 재시도, 에러 처리, 디버깅 등 실무에서 요구되는 네트워크 스케줄링 패턴을 구조적으로 구현한 데모입니다.  
실제 서비스에서 대량의 네트워크 요청을 효율적으로 관리하고, 유지보수성과 확장성을 높이기 위한 설계 경험을 포트폴리오용으로 정리하였습니다.

## 주요 구현 및 구조

- **네트워크 요청 객체화 및 스케줄링**
  - [`NetworkRequest`](NetworkRequest.cs): URL, HTTP 메서드, 요청 간격(Interval), 타임아웃, 우선순위, 재시도, 콜백 등 다양한 속성을 가진 네트워크 요청 객체
  - 요청별로 등록/해제, 상태 추적, 자동 재시도, 취소, 응답 핸들러 등 지원

- **스케줄러 및 매니저 구조**
  - [`NetworkManager`](NetworkManager.cs): 등록된 모든 네트워크 요청을 주기적으로 검사하여, 각 요청의 Interval에 맞춰 자동 실행
  - 요청 실행 결과에 따라 상태(`RequestStatus`)를 갱신하고, 실패 시 재시도/에러 콜백 처리
  - 요청 완료/오류/취소 등 다양한 상태 관리

- **서비스 및 인터페이스 분리**
  - [`INetworkService`](Interfaces/INetworkService.cs): 네트워크 서비스 인터페이스로, 테스트/확장에 용이
  - [`NetworkService`](NetworkService.cs): 실제 네트워크 요청 등록/해제/관리 기능 구현
  - [`NetworkServiceProvider`](NetworkServiceProvider.cs): 싱글톤 패턴으로 네트워크 서비스 인스턴스 관리

- **응답 및 상태 관리**
  - [`NetworkResult`](NetworkResult.cs): HTTP 상태 코드, 응답 본문, 헤더, 성공 여부 등 응답 데이터 구조화
  - [`RequestStatus`](RequestStatus.cs): Idle, Waiting, Processing, Completed, Failed 등 요청 상태 Enum

- **핸들러/디버깅/테스트**
  - [`INetworkResponseHandler`](Handlers/INetworkResponseHandler.cs): 응답 핸들러 인터페이스로, 요청별 커스텀 처리 지원
  - [`NetworkDebugger`](NetworkDebugger.cs): 요청 로그 기록, 최근 요청 내역 조회 등 네트워크 디버깅 유틸리티
  - [`HttpbinTestURLs`](HttpbinTestURLs.cs): 테스트용 HTTPBin API URL 상수 제공

- **확장성/유지보수성**
  - 요청 객체, 서비스, 스케줄러, 핸들러, 디버거 등 각 역할별로 코드 분리
  - 인터페이스 기반 설계로 테스트 및 커스텀 확장 용이
  - 요청 우선순위, 재시도, 취소, 콜백 등 실무에서 요구되는 다양한 네트워크 패턴 지원

## 기대 효과

- 대규모 네트워크 요청의 효율적 관리 및 자동화 경험 어필
- 상태 기반 네트워크 요청 관리, 재시도, 우선순위, 에러 처리 등 실무 패턴 경험 강조
- 구조화된 코드, 인터페이스 기반 설계, 유지보수성/확장성 역량 강조

## 주요 코드 예시

```csharp
// 네트워크 요청 객체 생성 및 등록
var request = new NetworkRequest("https://httpbin.org/json", 5f, HttpMethod.Get,
    onComplete: result => Debug.Log(result.Response),
    onError: ex => Debug.LogError(ex.Message));
NetworkServiceProvider.GetService().RegisterRequest(request);

// NetworkManager에서 주기적으로 요청 실행
private void Update()
{
    // 등록된 요청들의 Interval을 체크하여 자동 실행
    // 실패 시 재시도, 완료/에러 콜백 처리
}
```

```csharp
// 요청 상태 Enum
public enum RequestStatus
{
    Idle, Waiting, Processing, Cancelled, Completed, Failed, HandleFailed
}
```

```csharp
// 네트워크 디버깅 로그 기록
NetworkDebugger.LogRequest(request, "Send", "요청 전송됨");
var logs = NetworkDebugger.GetRecentLogs();
```

## 폴더 구조 및 주요 파일

- `NetworkRequest.cs` : 네트워크 요청 객체
- `NetworkResult.cs` : 네트워크 응답 데이터 구조
- `RequestStatus.cs` : 요청 상태 Enum
- `NetworkManager.cs` : 요청 스케줄링 및 실행 매니저
- `NetworkService.cs`, `INetworkService.cs` : 네트워크 서비스 및 인터페이스
- `NetworkServiceProvider.cs` : 서비스 인스턴스 관리
- `Handlers/` : 응답 핸들러 인터페이스 및 구현
- `NetworkDebugger.cs` : 네트워크 요청 디버깅 유틸리티
- `HttpbinTestURLs.cs` : 테스트용 URL 상수

## 기타

- 실무에서 반복적/주기적 네트워크 작업, 대량 요청 관리, 상태 기반 처리, 확장성 높은 네트워크 구조 설계 경험을 효과적으로 어필할 수 있는