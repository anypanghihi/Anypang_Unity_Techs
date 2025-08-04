# UniRx Sample

## 프로젝트 소개
이 샘플은 Unity에서 [UniRx](https://github.com/neuecc/UniRx)를 활용한 MVVM 패턴, 데이터 바인딩, 리액티브 프로그래밍 구조를 포트폴리오용으로 정리한 데모입니다.  
실제 현업에서 UI와 데이터의 실시간 동기화, View/ViewModel 분리, 테스트 가능한 구조 설계 등 실무 패턴을 경험할 수 있도록 설계되었습니다.

## 주요 기술 및 구현 포인트

- **UniRx 기반 MVVM 패턴**
  - `SampleViewModel`에서 `ReactiveProperty<SampleData>`로 데이터 상태를 관리
  - `SampleView`에서 ViewModel의 ReactiveProperty를 Subscribe하여 데이터 변경 시 UI가 자동으로 갱신

- **Mock 데이터 서비스 및 직렬화**
  - `MockSampleService`에서 임의의 데이터를 생성하고, JSON 직렬화/역직렬화 과정을 통해 실제 API 연동과 유사한 구조 구현
  - `SampleData` 클래스는 Name, Value, AlternativeValue 등 다양한 필드를 포함

- **UI 자동 바인딩**
  - `SampleView`에서 UniRx의 Subscribe를 활용해 ViewModel의 데이터가 변경될 때마다 UI(TextMeshProUGUI) 텍스트가 자동으로 갱신
  - Start에서 Mock 데이터 로드 및 바인딩 자동화

- **테스트 및 확장성**
  - Mock 데이터 구조와 UniRx 패턴을 활용해 실제 네트워크/API 연동 없이도 테스트 및 구조 확장 가능

## 기대 효과

- UniRx를 활용한 리액티브 프로그래밍, MVVM 패턴, 데이터 바인딩 경험 어필
- 테스트 가능한 구조, View/ViewModel 분리, 유지보수성 높은 코드 설계 역량 강조
- 실무에서의 UI-데이터 동기화, Mock 서비스 활용 경험 강조

## 주요 코드 예시

```csharp
// ViewModel에서 ReactiveProperty로 데이터 관리
public ReactiveProperty<SampleData> SampleInfo { get; private set; }

// View에서 UniRx Subscribe로 UI 자동 갱신
_viewModel.SampleInfo.Subscribe(UpdateUI).AddTo(this);

// Mock 데이터 생성 및 JSON 직렬화/역직렬화
public SampleData GetMockData()
{
    string jsonString = GetMockJson();
    return JsonConvert.DeserializeObject<SampleData>(jsonString);
}
```

## 폴더 구조 및 주요 파일

- `SampleView.cs` : UniRx 기반 View, UI 자동 바인딩
- `SampleViewModel.cs` : ViewModel, ReactiveProperty 관리
- `MockSampleService.cs` : Mock 데이터 생성, JSON 직렬화/역직렬화
- `UniRx Example.unity` : 샘플 Scene

## 기타

- UniRx, MVVM, 데이터 바인딩, 테스트 가능한 구조 등 실무에서 요구되는 다양한 패턴을 효과적으로 어필 할 수 있는 포트폴리오용 데모입니다.