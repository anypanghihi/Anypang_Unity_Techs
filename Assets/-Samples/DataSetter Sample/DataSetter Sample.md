# DataSetter Sample

## 프로젝트 소개
이 샘플은 Unity에서 다양한 공정(Operation) 데이터를 실시간으로 받아와 UI에 바인딩하고, 네트워크 연동, 데이터 컨테이너 구조, 에디터 자동화 등 실무에서 자주 쓰이는 데이터 관리 및 UI 연동 패턴을 포트폴리오용으로 정리한 데모입니다.

## 주요 기술 및 구현 포인트

- **데이터 컨테이너 및 바인딩 구조**
  - 각 공정(OP)의 데이터 구조를 ScriptableObject(`OpItemBundle`, `OpNetworkBundle`)로 관리
  - `OpItemDataContainer`에서 전체 데이터와 뷰를 통합 관리하며, 각 데이터는 `OpItemData`, `OpPanelInfoData` 등으로 UI에 바인딩

- **네트워크 연동 및 데이터 파싱**
  - `NetworkManager`에서 API 호출 및 JSON 파싱, 데이터 Dictionary 관리
  - 실시간으로 받아온 데이터를 각 공정별로 매핑하여 UI에 자동 반영

- **유연한 UI/Prefab 구조**
  - 각 공정별 버튼, 컨테이너, 패널 등 UI 요소를 Prefab으로 분리하여 재사용성 강화
  - `OpPanelInfoDataSelector`를 통해 BoxType에 따라 동적으로 UI 패널 선택 및 데이터 바인딩

- **에디터 자동화 및 커스텀 인스펙터**
  - 각 데이터/컨테이너/네트워크 번들에 대해 커스텀 에디터 스크립트 제공
  - 버튼 클릭 한 번으로 데이터/뷰 생성 및 삭제, 항목 추가/삭제 등 반복 작업 자동화

- **실제 업무 패턴 반영**
  - 공정별 SPEC, 대시보드, API 연동, 데이터 포맷, 컬러링 등 실무에서 요구되는 다양한 데이터 처리 및 UI 연동 패턴 구현

## 기대 효과

- 대규모 데이터 기반 UI/UX 설계 및 유지보수 경험 어필
- 네트워크 연동, 데이터 파싱, 실시간 바인딩 등 실무에서 요구되는 핵심 기술 경험 강조
- 에디터 자동화 및 구조화된 데이터 관리 역량 어필

## 주요 코드 예시

```csharp
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

```csharp
// 데이터 컨테이너에서 패널 뷰 전환
public void ShowOpPanel(int id)
{
    if (prevView != null)
        prevView.Hide();

    BundleDatas[id].UIView.Show();
    prevView = BundleDatas[id].UIView;
}
```

```csharp
// 에디터에서 데이터/뷰 자동 생성
if (GUILayout.Button("Create OpItemBundle Data"))
{
    foreach (OpItemBundle each in container.Bundles)
    {
        OpItemBundleData data = GameObject.Instantiate<OpItemBundleData>(container.BundlePrefab, ...);
        // ... 데이터 및 뷰 생성 ...
    }
}
```

## 폴더 구조 및 주요 파일

- `Prefab/` : 각종 버튼, 컨테이너, 데이터 패널 등 UI Prefab
- `Scene/` : 샘플 Scene
- `Script/`
  - `OpItemBundle.cs`, `OpNetworkBundle.cs` : ScriptableObject 기반 데이터 구조
  - `OpItemDataContainer.cs`, `OpItemBundleData.cs`, `OpItemData.cs` : 데이터 컨테이너 및 바인딩
  - `OpPanelInfoData.cs`, `OpPanelInfoDataSelector.cs` : UI 패널 및 데이터 선택/바인딩
  - `NetworkManager.cs` : 네트워크 연동 및 데이터 파싱
  - `*.Editor.cs` : 커스텀 에디터 스크립트

## 기타

- 실무에서의 데이터 기반 UI/UX, 네트워크 연동, 에디터 자동화 등 다양한 경험을 효과적으로 어필할