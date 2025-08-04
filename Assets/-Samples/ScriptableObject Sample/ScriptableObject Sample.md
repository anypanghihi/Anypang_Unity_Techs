# ScriptableObject Sample

## 프로젝트 소개
이 샘플은 Unity의 ScriptableObject를 활용한 데이터 관리, 이벤트/액션 시스템, 그리고 ScriptableObject 기반 런타임 매니지먼트 구조를 포트폴리오용으로 정리한 데모입니다.  
실제 현업에서 데이터의 직렬화, 에디터-런타임 데이터 동기화, ScriptableObject를 통한 코드 구조화, 이벤트/액션 패턴 등 다양한 실무 패턴을 경험할 수 있도록 설계되었습니다.

## 주요 기술 및 구현 포인트

- **ScriptableObject 기반 데이터 관리**
  - `TransformSO`, `TransformArraySO`, `LineOverViewSO` 등 다양한 데이터 구조를 ScriptableObject로 설계
  - PRS(Position, Rotation, Scale) 구조체를 활용한 위치/회전/스케일 데이터 직렬화 및 관리
  - ScriptableObject를 통한 데이터 저장/불러오기 및 에디터-런타임 동기화

- **ScriptableObject 기반 런타임 매니지먼트**
  - `ManagedBehaviourSO`에서 런타임 중 등록된 Action(업데이트 함수) 리스트를 관리
  - `ManagedBehaviour` 추상 클래스를 상속받아, 각 오브젝트의 ManagedUpdate를 ScriptableObject에서 일괄 관리 및 실행
  - UniTask를 활용한 비동기 업데이트 루프 및 메모리 관리

- **이벤트/액션 시스템**
  - `ScriptableEvent`, `ScriptableAction` 등 ScriptableObject 기반 이벤트/액션 구조 설계
  - 다양한 오브젝트 간의 느슨한 결합 및 이벤트 기반 통신 패턴 구현

- **에디터 확장 및 커스텀 인스펙터**
  - 각 VO(뷰 오브젝트) 클래스에 대해 커스텀 에디터 제공(TransformVO, LineOverViewVO, TransformArrayVO 등)
  - 배열 데이터 추가/삭제, 인덱스 선택, 값 저장 등 에디터에서 직관적으로 데이터 관리 가능
  - `ManagedBehaviourSOEditor`에서 런타임 등록된 업데이트 함수 리스트를 시각화 및 오브젝트 Ping 기능 제공

- **실제 업무 패턴 반영**
  - ScriptableObject를 통한 데이터/이벤트/액션/매니지먼트 구조의 통합적 활용
  - 에디터-런타임 데이터 동기화, 커스텀 에디터 자동화, 느슨한 결합 구조 등 실무에서 요구되는 다양한 패턴 구현

## 기대 효과

- ScriptableObject를 활용한 데이터 구조화, 런타임 매니지먼트, 이벤트/액션 시스템 경험 어필
- 에디터 확장, 커스텀 인스펙터, 비동기 업데이트 등 고급 Unity 기능 활용 역량 강조
- 유지보수성과 확장성이 높은 구조 설계 및 실무 패턴 경험 강조

## 주요 코드 예시

```csharp
// ScriptableObject 기반 데이터 저장/불러오기
public class TransformSO : ScriptableObject, IAssetSO
{
    [SerializeField] private PRS data;
    public PRS Value => data;

    public void Save<T>(T src)
    {
        Transform tm = src as Transform;
        data.position = tm.position;
        data.rotation = tm.rotation;
        data.scale = tm.localScale;
        // 에디터에서 변경사항 저장
    }
}
```

```csharp
// ScriptableObject 기반 런타임 매니지먼트
public abstract class ManagedBehaviour : MonoBehaviour
{
    [SerializeField] ManagedBehaviourSO ManagedSO;

    public virtual void OnEnable()
    {
        ManagedSO.Register(ManagedUpdate);
    }
    public virtual void OnDisable()
    {
        ManagedSO.UnRegister(ManagedUpdate);
    }
    public abstract void ManagedUpdate();
}
```

```csharp
// 커스텀 에디터에서 배열 데이터 관리
if (GUILayout.Button("Add Array Data"))
{
    vo.AddArrayLast();
}
if (GUILayout.Button("Remove Array Data"))
{
    vo.RmvArrayLast();
}
```

## 폴더 구조 및 주요 파일

- `Scripts/Base/`
  - `ManagedBehaviour.cs`, `ManagedBehaviourSO.cs` : 런타임 매니지먼트 구조
  - `ScriptableAction.cs`, `ScriptableEvent.cs` : ScriptableObject 기반 이벤트/액션 시스템
  - `Structs.cs` : PRS(Position, Rotation, Scale) 구조체
- `Scripts/SO/`
  - `TransformSO.cs`, `TransformArraySO.cs`, `LineOverViewSO.cs` : ScriptableObject 기반 데이터 구조
  - `IAssetSO.cs` : 데이터 저장 인터페이스
- `Scripts/VO/`
  - `TransformVO.cs`, `LineOverViewVO.cs`, `TransformArrayVO.cs` : ScriptableObject 데이터와 연동되는 뷰 오브젝트 및 커스텀 에디터
- `SO/` : ScriptableObject 에셋 리소스
- `ScriptableObject.unity` : 샘플 Scene

## 기타

- ScriptableObject를 활용한 데이터 관리, 런타임 매니지먼트, 이벤트/액션 시스템, 에디터 확장 등 다양한 실무 경험을 효과적으로 어필할 수 있는