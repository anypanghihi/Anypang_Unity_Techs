# Network Scheduler Sample

## 프로젝트 소개
이 샘플은 기존의 **Function Scheduler Sample** 구조를 네트워크 요청(REST API 등) 스케줄링에 특화하여 확장한 버전입니다.  
여러 네트워크 요청(혹은 네트워크 관련 함수)을 주기적으로 자동 실행하고, 요청 등록/해제, 에디터 시각화 등 실무에서 자주 쓰이는 네트워크 스케줄링 패턴을 포트폴리오용으로 구현하였습니다.

## 기존 Function Scheduler Sample과의 주요 차이점

- **스케줄 대상의 명확한 네트워크 특화**
  - 기존 샘플은 GameObject의 임의 함수 호출에 초점을 맞췄다면,  
    본 샘플은 네트워크 요청(예: API 호출, 데이터 동기화 등) 객체(`OpItemBundleData`)를 주기적으로 실행하는 데 초점을 맞춤

- **네트워크 요청 스케줄링 구조**
  - `RequestScheduler`(ScriptableObject)에서 네트워크 요청 객체를 등록/해제 및 리스트 관리
  - `NetworkManagerSample`에서 각 요청별 실행 주기(Interval) 관리 및 자동 실행

- **코드 구조 및 네이밍 개선**
  - 함수명, 변수명, 클래스명이 네트워크 요청 스케줄링에 맞게 리네이밍 및 구조화
  - `RequestHandler`, `RequestScheduler`, `NetworkManagerSample` 등 네트워크 중심 네이밍

- **에디터 확장 및 시각화**
  - `RequestSchedulerEditor`에서 등록된 네트워크 요청 리스트를 Foldout UI로 시각화
  - 각 요청 객체를 클릭하면 에디터에서 Ping 기능 제공

- **실제 네트워크 요청 실행 구조로 확장 가능**
  - `ExecuteFunction`에서 실제 네트워크 요청을 호출하도록 확장 가능(현재는 샘플 구조)
  - 실무에서는 REST API 호출, 데이터 동기화, 상태 체크 등 다양한 네트워크 작업에 적용 가능

## 기대 효과

- 네트워크 요청의 주기적/자동 실행 관리 경험 어필
- 네트워크 요청 등록/해제, 에디터 시각화 등 실무 패턴 경험 강조
- 네트워크 중심 구조 설계 및 유지보수성, 확장성 역량 강조

## 주요 코드 예시

```csharp
// 네트워크 요청 등록 및 실행 주기 설정
RegisterFunction(TestObject, 5f);
RegisterFunction(TestObject, 10f);
RegisterFunction(TestObject2, 15f);

// NetworkManagerSample에서 주기적으로 요청 실행
private void Update()
{
    float currentTime = Time.time;
    foreach (var key in lastExecutionTimes.Keys)
    {
        if (currentTime - lastExecutionTimes[key] >= intervals[key])
        {
            ExecuteFunction(key);
            lastExecutionTimes[key] = currentTime;
        }
    }
}
```

```csharp
// RequestSchedulerEditor에서 등록 요청 시각화
showScheduledFunctions = EditorGUILayout.Foldout(showScheduledFunctions, "Scheduled Network Request");
if (showScheduledFunctions)
{
    DrawScheduledFunctions();
}
```

## 폴더 구조 및 주요 파일

- `RequestScheduler.cs` : 네트워크 요청 등록/해제 및 관리용 ScriptableObject
- `NetworkManagerSample.cs` : 네트워크 요청 실행 주기 관리 및 자동 호출
- `RequestSchedulerEditor.cs` : 에디터 확장(등록 요청 시각화)
- `Network Scheduler Sample.unity` : 샘플 Scene

## 기타

- 실무에서 반복적/주기적 네트워크 작업이 필요한 다양한 상황에 적용 가능한 구조로, 유지보수성과 확장성을 효과적으로 어필 할 수 있는 포트폴리오용 데모입니다.