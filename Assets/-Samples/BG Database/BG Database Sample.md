# BG Database 샘플

## 프로젝트 소개
이 샘플은 Unity에서 [BGDatabase](https://assetstore.unity.com/packages/tools/integration/bg-database-113957) 에셋을 활용하여, 게임 내 데이터 관리 및 동적 데이터 바인딩, 데이터 검증, 실시간 값 조정, 그리고 데이터 기반 그래프 시각화까지의 실무 패턴을 포트폴리오용으로 정리한 데모입니다.

## 주요 기술 및 구현 포인트

- **BGDatabase 활용**  
  - 테이블/엔티티/필드 구조를 코드로 자동 생성 및 접근
  - 실시간 데이터 수정, 검증, 바인딩 등 다양한 데이터 관리 기능 구현

- **데이터 검증 및 수정 인터페이스**  
  - `IBGTableValidator`, `IBGValueModifier` 인터페이스 설계 및 서비스 구현
  - 테이블/엔티티/필드의 유효성 체크 및 값 조정 로직 분리

- **동적 데이터 바인딩 및 차트 연동**  
  - `BGDataBinderChart`를 통한 외부 JSON 데이터 바인딩 및 차트 데이터 연동
  - 실시간 데이터 변경 시 차트 갱신 구조

- **그래프 컬러 커스터마이징**  
  - `BGGraph_MinMaxColor`(MinMaxColorUnit) 커스텀 유닛을 통해 값의 범위에 따라 그래프 색상 동적 변경
  - JSON 템플릿(`최대최소 칼라적용 graph Template.json`) 기반의 그래프 설정

- **실제 업무 패턴 반영**  
  - 실무에서 자주 사용하는 데이터 그룹화, 조건별 필터링, 코드 자동생성 활용 등 다양한 패턴 적용

## 기대 효과

- 대규모 게임/앱에서의 데이터 관리 자동화 및 유지보수성 향상
- 실시간 데이터 기반 UI/그래프 연동 경험 어필
- 인터페이스 기반 설계 및 확장성 높은 구조 설계 역량 강조

## 주요 코드 예시

```csharp
// 데이터 검증 및 값 조정 서비스 예시
public void ChangeValue_With_Comment(string tableName, int entityId)
{
    var table = tableValidator.GetTable(tableName);
    var entity = tableValidator.GetEntity(table, entityId);
    var field = tableValidator.GetFieldByComment(table, "Value");
    float adjustment = UnityEngine.Random.Range(-0.1f, 0.0f);
    valueModifier.ModifyFieldValue(entity, field.Name, adjustment);
}
```

```csharp
// 그래프 컬러 동적 변경 커스텀 유닛
public override void Definition()
{
    a = ValueInput<float>("min", "a");
    b = ValueInput<float>("max", "b");
    c = ValueInput<float>("cur", "c");
    d = ValueInput<Color>("min", "d");
    e = ValueInput<Color>("max", "e");
    ValueOutput<Color>("min < cur < max", "f", GetValue);
}
```

## 폴더 구조 및 주요 파일

- `BGDataBaseTest.cs` : 데이터 접근/수정/그룹화 테스트 및 실습 코드
- `BGDataBinderChart.cs` : 외부 데이터(JSON) 바인딩 및 차트 연동
- `BGGraph_MinMaxColor.cs` : 그래프 컬러 커스터마이징 커스텀 유닛
- `Services/`, `Interfaces/` : 데이터 검증/수정 서비스 및 인터페이스
- `최대최소 칼라적용 graph Template.json` : 그래프 컬러 설정 템플릿

## 기타

- 실무에서의 데이터 관리 자동화, 유지보수성, 확장성, 그리고 시각화 경험을 효과적으로 어필할 수