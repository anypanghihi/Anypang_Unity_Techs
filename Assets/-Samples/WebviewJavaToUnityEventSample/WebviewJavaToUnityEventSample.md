# WebviewJavaToUnityEventSample

## 프로젝트 소개
이 샘플은 Unity에서 WebView를 활용하여 JavaScript와 Unity(C#) 간의 양방향 통신을 구현한 데모입니다.  
실제 서비스에서 웹 기반 UI와 네이티브 게임 로직을 연동할 때 활용할 수 있는 기술을 포트폴리오용으로 정리하였습니다.

## 주요 기술 및 구현 포인트
- **Unity WebView**: Unity에서 웹 페이지를 임베드하고, JavaScript와의 인터페이스를 제공합니다.
- **JavaScript ↔ Unity 통신**: JavaScript에서 Unity로 메시지를 보내고, Unity에서 JavaScript 함수를 호출하는 구조를 구현했습니다.
- **이벤트 시스템**: WebView에서 발생한 이벤트를 Unity의 C# 이벤트로 변환하여 처리합니다.
- **실제 적용 예시**: 버튼 클릭, 데이터 전달, 상태 동기화 등 실무에서 자주 쓰이는 패턴을 샘플로 구현하였습니다.

## 기대 효과
- 웹과 네이티브 게임 로직의 유연한 연동
- 유지보수성과 확장성이 높은 구조 설계 경험
- 실무에서의 WebView 활용 노하우 어필

## 주요 코드 예시
```csharp
// JavaScript에서 Unity로 메시지 전송
window.Unity.call('OnWebEvent', JSON.stringify({ type: 'login', userId: 123 }));

// Unity에서 메시지 수신 및 처리
void OnWebEvent(string message) {
    var data = JsonUtility.FromJson<WebEventData>(message);
    // 이벤트 타입별 분기 처리
}
```

## 기타
- 실제 프로젝트에서는 보안, 데이터 검증, 에러 핸들링 등 추가 고려사항이 있습니다.