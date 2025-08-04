# Dashboard Sample

## 프로젝트 소개
이 샘플은 Unity 기반의 대시보드 UI/UX를 구현한 데모 프로젝트입니다.  
실제 업무에서 데이터 시각화, 실시간 데이터 리스트, 웹 연동, 신호 기반 뷰 전환 등 다양한 대시보드 기능을 통합적으로 경험할 수 있도록 설계되었습니다.

## 주요 기술 및 구현 포인트

- **데이터 시각화(Chart)**
  - [E2Chart](https://assetstore.unity.com/packages/tools/gui/e2chart-187858) 플러그인을 활용한 실시간 그래프(BarChart 등) 구현
  - 데이터 추가/삭제, 축 자동 조정, 시리즈 컬러 커스터마이징 등 ChartBase/ChartController 구조로 확장성 있게 설계

- **리스트 뷰어 및 무한 스크롤**
  - `InfiniteScroll` 기반의 리스트 뷰어(ListViewerBase, SampleListViewer) 구현
  - 데이터 정렬(ASC/DESC), 동적 데이터 바인딩, 마우스 Hover 효과 등 실무 패턴 반영

- **프로그레스/게이지 UI**
  - `UIBarFill`, `UICircleFill`, `UIRoundBar` 등 다양한 형태의 Progress Bar/게이지 UI 컴포넌트 직접 구현
  - DOTween, UniTask 등 비동기 애니메이션 및 UI 업데이트 적용

- **웹 뷰어 연동**
  - [Vuplex WebView](https://assetstore.unity.com/packages/tools/gui/3d-webview-for-windows-mac-154378) 연동을 통한 외부 웹페이지 임베드 및 실시간 데이터 연동

- **Signal 기반 뷰 전환**
  - Doozy Signal 시스템을 활용한 뷰 전환 및 데이터 동기화 구조
  - 신호(Signal)로 각 뷰(그래프, 웹, 리스트) 간 데이터 전달 및 UI 상태 관리

- **Prefab/Scene 구조화**
  - 각 UI Frame, Item, Progress 등은 Prefab으로 분리하여 재사용성과 유지보수성 강화
  - 대시보드 샘플 Scene 제공

## 기대 효과

- 실시간 데이터 시각화 및 대시보드 UI/UX 설계 경험 어필
- 신호 기반의 뷰 전환, 데이터 바인딩, 비동기 UI 처리 등 실무에서 요구되는 다양한 패턴 경험
- 확장성과 유지보수성을 고려한 구조 설계 역량 강조

## 주요 코드 예시

```csharp
// Chart 데이터 추가 및 축 자동 조정
public override void AddData(List<float> dataList)
{
    float maxValue = GetMaxValue(dataList);
    SetChartAxisInterval(maxValue);
    series.dataY = dataList;
}
```

```csharp
// Signal 기반 뷰 전환 및 데이터 전달
void SendTimeGraphSignal(string taskID)
{
    SigStream[(int)DashboardCategory.TimeGraph].SendSignal<string>(taskID);
}
void OnTimeGraphSignal(Signal signal)
{
    if (signal.TryGetValue(out string signalValue))
    {
        SetTimeGraphViewer(signalValue);
    }
}
```

```csharp
// Progress Bar 비동기 애니메이션
public async UniTaskVoid FillValue(float value)
{
    float curAmount = slider.value;
    float nxtAmount = (value / 100.0f);
    float t = 0.0f;
    while (t <= 1.0f )
    {
        t += Time.deltaTime;
        slider.value = Mathf.Lerp(curAmount, nxtAmount, t);
        await UniTask.Yield(this.GetCancellationTokenOnDestroy());
    }
}
```

## 폴더 구조 및 주요 파일

- `Prefab/` : 각종 UI 프레임, 아이템, 프로그래스 바 등 Prefab
- `Scene/` : 대시보드 샘플 Scene
- `Script/`
  - `ChartBase.cs`, `SampleChartController.cs` : 차트 컨트롤러 및 베이스 클래스
  - `ListViewerBase.cs`, `SampleListViewer.cs`, `ListDataBase.cs` : 리스트 뷰어 및 데이터 바인딩
  - `Progress/` : 다양한 Progress Bar, 게이지 UI 컴포넌트
  - `DashboardManager.cs`, `DashboardManager.Signal.cs` : 전체 대시보드 관리 및 Signal 처리

## 기타

- 실무에서의 대시보드 UI/UX, 데이터 시각화, 신호 기반 구조, 웹 연동 등 다양한 경험을 효과적으로 어필할 수 있는