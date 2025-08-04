# Function Scheduler Sample

## 프로젝트 소개
이 샘플은 Unity에서 다양한 GameObject의 특정 함수를 일정 주기로 자동 실행할 수 있도록 설계된 "함수 스케줄러" 시스템의 데모입니다.  
실제 게임/앱에서 반복적으로 호출해야 하는 함수(예: 상태 체크, 자동 저장, 주기적 이펙트 등)를 효율적으로 관리할 수 있는 구조를 포트폴리오용으로 구현하였습니다.

## 주요 기술 및 구현 포인트

- **함수 스케줄링 시스템**
  - `FunctionScheduler`(ScriptableObject)에서 GameObject와 함수명을 등록/해제하여 관리
  - `SchedulerManager`에서 각 함수별 실행 주기(Interval)를 설정하고, 주기적으로 해당 함수를 자동 호출

- **실행 주기 관리 및 동적 등록/해제**
  - 각 함수별로 실행 주기를 개별적으로 지정 가능
  - 런타임 중에도 함수 등록/해제 및 주기 변경이 가능하여 유연한 스케줄링 지원

- **SendMessage 기반 동적 함수 호출**
  - GameObject에 지정된 함수명을 SendMessage로 호출하여, 다양한 컴포넌트의 메서드를 동적으로 실행

- **에디터 확장 및 시각화**
  - `FunctionSchedulerEditor`를 통해 에디터에서 현재 등록된 함수 목록을 Foldout UI로 확인 가능
  - GameObject 이름 클릭 시 해당 오브젝트를 Ping, 함수별 등록 현황 시각화

- **테스트 및 샘플 스크립트 제공**
  - `TestScript`, `TestScript2` 등에서 PrintMessage 계열 함수 구현
  - `SchedulerManager`에서 샘플 오브젝트와 함수 등록 예시 제공

## 기대 효과

- 반복적/주기적 함수 호출 관리의 효율성 및 유지보수성 향상
- 런타임 동적 등록/해제, 에디터 시각화 등 실무에서 요구되는 다양한 패턴 경험 어필
- ScriptableObject, 에디터 확장, SendMessage 등 Unity 고급 기능 활용 역량 강조

## 주요 코드 예시

```csharp
// 함수 등록 및 실행 주기 설정
RegisterFunction(TestObject, "PrintMessage", 5f);
RegisterFunction(TestObject, "PrintMessage2", 10f);
RegisterFunction(TestObject2, "PrintMessage3", 15f);
```

```csharp
// SchedulerManager에서 주기적으로 함수 실행
private void Update()
{
    float currentTime = Time.time;
    foreach (var key in lastExecutionTimes.Keys)
    {
        if (currentTime - lastExecutionTimes[key] >= intervals[key])
        {
            ExecuteFunction(key.Item1, key.Item2);
            lastExecutionTimes[key] = currentTime;
        }
    }
}
```

```csharp
// FunctionSchedulerEditor에서 등록 함수 목록 시각화
showScheduledFunctions = EditorGUILayout.Foldout(showScheduledFunctions, "Scheduled Functions");
if (showScheduledFunctions)
{
    DrawScheduledFunctions();
}
```

## 폴더 구조 및 주요 파일

- `FunctionScheduler.cs` : 함수 등록/해제 및 관리용 ScriptableObject
- `SchedulerManager.cs` : 함수 실행 주기 관리 및 자동 호출
- `FunctionSchedulerEditor.cs` : 에디터 확장(등록 함수 목록 시각화)
- `TestScript.cs`, `TestScript2.cs` : 테스트용 함수 구현
- `Function Scheduler Sample.unity` : 샘플 Scene

## 기타

- 실무에서 반복적/주기적 작업이 필요한 다양한 상황에 적용 가능한 구조로, 유지보수성과 확장성을 효과적으로 어필할