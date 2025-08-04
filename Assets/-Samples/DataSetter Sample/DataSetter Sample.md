# DataSetter Sample

## 프로젝트 소개
이 샘플은 Unity에서 다양한 공정(Operation)별 데이터를 실시간으로 받아와 UI에 바인딩하고, 네트워크 연동, 데이터 컨테이너 구조, 에디터 자동화 등 실무에서 자주 쓰이는 데이터 관리 및 UI 연동 패턴을 포트폴리오용으로 정리한 데모입니다.  
업데이트 버전에서는 공정별 폴더 구조 분리, 코드 모듈화, 커스텀 데이터 처리, 유지보수성 향상 등 확장성과 실무 적용성을 더욱 강화하였습니다.

## 주요 구현 및 업데이트 내용

- **공정별(코어/포드/감마) 폴더 구조 분리**
  - `Core`, `Ford`, `Gamma` 등 각 공정별로 폴더를 분리하여, 데이터/스크립트/프리팹/씬을 독립적으로 관리
  - 각 공정별로 별도의 Scene, Prefab, Script, UI 폴더를 구성하여 확장성과 유지보수성 강화

- **데이터 컨테이너 및 바인딩 구조**
  - 각 공정(OP)의 데이터 구조를 ScriptableObject(`OpItemBundle`, `OpNetworkBundle`)로 관리
  - `OpItemDataContainer`에서 전체 데이터와 뷰를 통합 관리하며, 각 데이터는 `OpItemData`, `OpPanelInfoData` 등으로 UI에 바인딩

- **공정별 커스텀 데이터 처리**
  - 각 공정에 특화된 데이터 파서(`OPFordTaskJsonReader.cs`, `OPGammaTaskJsonReader.cs` 등) 및 데이터 래퍼(`OpPanelInfoDataFordWrapper.cs` 등) 추가
  - 공정별로 상이한 데이터 포맷, UI 요구사항을 유연하게 대응할 수 있도록 구조 개선

- **네트워크 연동 및 데이터 파싱**
  - `NetworkManager`에서 API 호출 및 JSON 파싱, 데이터 Dictionary 관리
  - 실시간으로 받아온 데이터를 각 공정별로 매핑하여 UI에 자동 반영

- **유연한 UI/Prefab 구조**
  - 각 공정별 버튼, 컨테이너, 패널 등 UI 요소를 Prefab으로 분리하여 재사용성 강화
  - `OpPanelInfoDataSelector`를 통해 BoxType에 따라 동적으로 UI 패널 선택 및 데이터 바인딩

- **에디터 자동화 및 커스텀 인스펙터**
  - 각 데이터/컨테이너/네트워크 번들에 대해 커스텀 에디터 스크립트 제공
  - 버튼 클릭 한 번으로 데이터/뷰 생성 및 삭제, 항목 추가/삭제 등 반복 작업 자동화

- **씬(Scene) 분리 및 테스트 용이성 향상**
  - 각 공정별로 독립적인 샘플 Scene(`OPDataSetter Sample Scene.unity`) 제공
  - 공정별 기능 테스트 및 디버깅이 용이하도록 설계

## 기대 효과

- 대규모 데이터 기반 UI/UX 설계 및 유지보수 경험 어필
- 다양한 공정/프로젝트에 대한 확장성 및 유지보수성 대폭 향상
- 네트워크 연동, 데이터 파싱, 실시간 바인딩 등 실무에서 요구되는 핵심 기술 경험 강조
- 폴더/코드/리소스 구조화 및 모듈화 역량 강조
- 에디터 자동화 및 구조화된 데이터 관리 역량 어필

## 주요 코드/구조 예시

```csharp
// Ford 공정 전용 데이터 파서
public class OPFordTaskJsonReader : MonoBehaviour
{
    // Ford 전용 데이터 포맷 파싱 및 UI 연동
}

// 데이터 컨테이너에서 패널 뷰 전환
public void ShowOpPanel(int id)
{
    if (prevView != null)
        prevView.Hide();

    BundleDatas[id].UIView.Show();
    prevView = BundleDatas[id].UIView;
}

// 네트워크 데이터 파싱 및 UI 반영
public async UniTaskVoid GetOPDataAsync()
{
    // ... API 호출 및 데이터 파싱 ...
    foreach (var kv in opDataDictionary)
    {
        if (OpItemDataContainer.OpDataList.ContainsKey(kv.Key))
        {
            OpItemDataContainer.OpDataList[kv.Key].Value = kv.Value;
        }
    }
}
```

## 폴더 구조 및 주요 파일

- `Core/` : 공통 Prefab, Script 등
- `Ford/` : Ford 공정 전용 Scene, Script(`OPFordTaskJsonReader.cs`, `OpPanelInfoDataFord.cs` 등), Prefab, UI
- `Gamma/` : Gamma 공정 전용 Scene, Script(`OPGammaTaskJsonReader.cs` 등), Prefab, UI
- `OpItemBundle.cs`, `OpNetworkBundle.cs` : ScriptableObject 기반 데이터 구조
- `OpItemDataContainer.cs`, `OpItemBundleData.cs`, `OpItemData.cs` : 데이터 컨테이너 및 바인딩
- `OpPanelInfoData.cs`, `OpPanelInfoDataSelector.cs` : UI 패널 및 데이터 선택/바인딩
- `NetworkManager.cs` : 네트워크 연동 및 데이터 파싱
- `*.Editor.cs` : 커스텀 에디터 스크립트

## 기타

- 실무에서 여러 공정/제품군을 동시에 관리해야 하는 상황에 최적화된 구조로, 확장성과 유지보수성을 효과적으로 어필할 수 있는 포트폴리오용 데모입니다.