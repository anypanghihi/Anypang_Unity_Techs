# DataSetter Sample (Updated)

## 프로젝트 소개
이 샘플은 기존 `DataSetter Sample`의 구조와 기능을 확장·개선한 업데이트 버전입니다.  
실제 현업에서 다양한 공정(Operation)별 데이터 구조와 UI/UX 요구사항이 늘어남에 따라, 폴더 구조 분리, 코드 모듈화, 공정별 커스텀 처리, 유지보수성 향상에 중점을 두고 리팩토링한 데모입니다.

## 주요 업데이트 내용

- **공정별(코어/포드/감마) 폴더 구조 분리**
  - `Core`, `Ford`, `Gamma` 등 각 공정별로 폴더를 분리하여, 데이터/스크립트/프리팹/씬을 독립적으로 관리
  - 각 공정별로 별도의 Scene, Prefab, Script, UI 폴더를 구성하여 확장성과 유지보수성 강화

- **공정별 커스텀 데이터 처리**
  - 각 공정에 특화된 데이터 파서(`OPFordTaskJsonReader.cs`, `OPGammaTaskJsonReader.cs` 등) 및 데이터 래퍼(`OpPanelInfoDataFordWrapper.cs` 등) 추가
  - 공정별로 상이한 데이터 포맷, UI 요구사항을 유연하게 대응할 수 있도록 구조 개선

- **모듈화 및 코드 분리**
  - 공통 기능은 `Core`에, 공정별 특화 기능은 각 폴더에 분리하여 코드 중복 최소화
  - Prefab, Script, UI 등 리소스도 공정별로 독립 관리

- **씬(Scene) 분리 및 테스트 용이성 향상**
  - 각 공정별로 독립적인 샘플 Scene(`OPDataSetter Sample Scene.unity`) 제공
  - 공정별 기능 테스트 및 디버깅이 용이하도록 설계

## 기대 효과

- 다양한 공정/프로젝트에 대한 확장성 및 유지보수성 대폭 향상
- 공정별 요구사항에 맞춘 커스텀 데이터 처리 및 UI/UX 대응 경험 어필
- 폴더/코드/리소스 구조화 및 모듈화 역량 강조

## 주요 코드/구조 예시

```csharp
// Ford 공정 전용 데이터 파서
public class OPFordTaskJsonReader : MonoBehaviour
{
    // Ford 전용 데이터 포맷 파싱 및 UI 연동
}
```

```csharp
// 공정별 데이터 래퍼 구조
public class OpPanelInfoDataFordWrapper : MonoBehaviour
{
    // Ford 공정에 특화된 데이터 래핑 및 바인딩
}
```

## 폴더 구조 및 주요 파일

- `Core/` : 공통 Prefab, Script 등
- `Ford/` : Ford 공정 전용 Scene, Script(`OPFordTaskJsonReader.cs`, `OpPanelInfoDataFord.cs` 등), Prefab, UI
- `Gamma/` : Gamma 공정 전용 Scene, Script(`OPGammaTaskJsonReader.cs` 등), Prefab, UI

## 기타

- 실무에서 여러 공정/제품군을 동시에 관리해야 하는 상황에 최적화된 구조로, 확장성과 유지보수성을 효과적으로