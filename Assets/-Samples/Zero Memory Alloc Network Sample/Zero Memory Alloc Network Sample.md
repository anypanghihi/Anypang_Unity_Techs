# Zero Memory Alloc Network Sample

## 프로젝트 소개
이 샘플은 Unity에서 대용량 네트워크 데이터 다운로드 시 **GC(가비지 컬렉션) 할당을 최소화**하는 네트워크 통신 구조를 구현한 데모입니다.  
실제 현업에서 REST API, HTTP 요청/응답, 데이터 파싱, 그리고 메모리 효율성(Zero-Allocation)이 중요한 상황에 대응할 수 있는 실무 패턴을 포트폴리오용으로 정리하였습니다.

## 주요 구현 및 기술 포인트

- **네트워크 요청/응답 구조화**
  - [`NetworkRequest`](Script/NetworkRequest.cs): URL, 메서드, 헤더, 바디, 재시도, 우선순위, 콜백 등 다양한 옵션을 지원하는 네트워크 요청 객체
  - [`NetworkResult`](Script/NetworkResult.cs): HTTP 상태 코드, 응답 본문, 헤더, 그리고 NativeArray<byte> 등 다양한 응답 데이터 구조화

- **Zero-Allocation 네트워크 처리**
  - [`NetworkMemoryTest`](Script/NetworyMemoryTest.cs):  
    - 일반적인 UnityWebRequest 처리와 NativeArray<byte>를 직접 반환하는 Zero-Allocation 방식 모두 지원  
    - 대용량 데이터 다운로드 시 GC 할당을 최소화하여 메모리 효율성 극대화  
    - `SendWebRequestAsync_With_ZeroAlloc()`에서 NativeArray<byte>를 직접 반환하여 불필요한 문자열 변환 및 임시 객체 생성을 방지

- **비동기/병렬 네트워크 처리**
  - [UniTask](https://github.com/Cysharp/UniTask) 기반의 비동기 네트워크 처리
  - CancellationToken, Timeout, 재시도, 딜레이 등 실무에서 필요한 네트워크 안정성 패턴 적용

- **실제 업무 패턴 반영**
  - 요청 큐 관리, 우선순위, 재시도, 에러 핸들링, 콜백 구조 등 실무에서 자주 쓰이는 네트워크 패턴 적용
  - 다양한 API 응답 포맷 대응 및 데이터 바인딩 구조

## 기대 효과

- 대용량 네트워크 데이터 처리 시 GC 최소화 및 메모리 효율성 경험 어필
- 비동기/병렬 처리, 재시도, 우선순위 등 네트워크 안정성 패턴 경험 강조
- 구조화된 네트워크 요청/응답 관리 및 유지보수성 높은 코드 설계 역량 강조

## 주요 코드 예시

```csharp
// 네트워크 요청 객체 생성 및 사용
NetworkRequest request = new NetworkRequest("https://httpbin.org/bytes/1048576", 1.0f, System.Net.Http.HttpMethod.Get);
StartNetworkMemoryTest(request);

// 일반적인 비동기 네트워크 요청 (메모리 할당 발생)
private async UniTask<NetworkResult> SendWebRequestAsync(NetworkRequest request)
{
    // ... UnityWebRequest 처리 및 string 반환 ...
}

// Zero-Allocation 방식 (NativeArray<byte> 직접 반환)
private async UniTask<NetworkResult> SendWebRequestAsync_With_ZeroAlloc(NetworkRequest request)
{
    // ... UnityWebRequest 처리 및 NativeArray<byte> 반환 ...
}
```

## 폴더 구조 및 주요 파일

- `Network Memory Test Scene.unity` : 대용량 데이터 다운로드 및 메모리 테스트 씬
- `Script/NetworkRequest.cs` : 네트워크 요청 객체
- `Script/NetworkResult.cs` : 네트워크 응답 데이터 구조
- `Script/NetworyMemoryTest.cs` : 대용량 데이터 다운로드 및 Zero-Allocation 테스트

## 기타

- Unity 네트워크 통신에서 메모리 할당 없이 대용량 데이터를 효율적으로 처리하는 구조를 효과적으로 어필할 수 있는 포트폴리오용 데모입니다.
