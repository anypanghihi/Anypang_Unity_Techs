# Network Sample

## 프로젝트 소개
이 샘플은 Unity에서 네트워크 통신(REST API, HTTP 요청/응답, 데이터 파싱 등)과 관련된 실무 패턴을 포트폴리오용으로 정리한 데모입니다.  
실제 현업에서 API 연동, 데이터 파싱, 네트워크 메모리 관리, 비동기 처리, 네트워크 요청 큐 관리 등 다양한 요구사항을 효과적으로 대응할 수 있는 구조를 구현하였습니다.

## 주요 기술 및 구현 포인트

- **네트워크 요청/응답 구조화**
  - `NetworkRequest` : 요청 URL, 메서드, 헤더, 바디, 재시도, 우선순위 등 다양한 옵션을 지원하는 네트워크 요청 객체
  - `NetworkResult` : HTTP 상태 코드, 응답 본문, 헤더, NativeArray<byte> 등 다양한 응답 데이터 구조화

- **비동기/병렬 네트워크 처리**
  - `Cysharp.Threading.Tasks`(UniTask) 기반의 비동기 네트워크 처리
  - CancellationToken, Timeout, 재시도, 딜레이 등 실무에서 필요한 네트워크 안정성 패턴 적용

- **네트워크 메모리 관리 및 Zero-Allocation**
  - `NetworkMemoryTest`에서 NativeArray<byte>를 직접 활용하여 대용량 데이터 다운로드 시 GC 최소화 및 메모리 효율성 테스트

- **실제 업무 패턴 반영**
  - 요청 큐 관리, 우선순위, 재시도, 에러 핸들링, 콜백 구조 등 실무에서 자주 쓰이는 네트워크 패턴 적용
  - 다양한 API 응답 포맷 대응 및 데이터 바인딩 구조

## 기대 효과

- 실무에서 요구되는 네트워크 통신, 데이터 파싱, 메모리 관리 등 다양한 경험 어필
- 비동기/병렬 처리, 재시도, 우선순위 등 네트워크 안정성 패턴 경험 강조
- 구조화된 네트워크 요청/응답 관리 및 유지보수성 높은 코드 설계 역량 강조

## 주요 코드 예시

```csharp
// 네트워크 요청 객체 생성 및 사용
NetworkRequest request = new NetworkRequest("https://httpbin.org/bytes/1048576", 1.0f, System.Net.Http.HttpMethod.Get);
StartNetworkMemoryTest(request);

// 비동기 네트워크 요청 및 응답 처리
private async UniTask<NetworkResult> SendWebRequestAsync(NetworkRequest request)
{
    using var cancellationTokenSource = new CancellationTokenSource();
    cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(request.Timeout));
    UnityWebRequest uwr = UnityWebRequest.Get(request.Url);
    var operation = await uwr.SendWebRequest().WithCancellation(cancellationTokenSource.Token);
    int statusCode = (int)uwr.responseCode;
    string responseText = uwr.downloadHandler?.text ?? "";
    return new NetworkResult(statusCode, responseText);
}
```

## 폴더 구조 및 주요 파일

- `Network Memory Test Scene.unity` : 대용량 데이터 다운로드 및 메모리 테스트 씬
- `NetworkRequest.cs`, `NetworkResult.cs` : 네트워크 요청/응답 구조화 클래스
- `NetworyMemoryTest.cs` : 대용량 데이터 다운로드 및 메모리 테스트

## 기타

- 유니티 네트워크 통신에서 메모리를 할당하지 않고 데이터 파싱 관리