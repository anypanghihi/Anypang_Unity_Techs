# JSON Alias, Json Property Sample

## 프로젝트 소개
이 샘플은 Unity에서 다양한 JSON 데이터 포맷을 유연하게 역직렬화(파싱)할 수 있도록,  
`JsonProperty`(Newtonsoft 기본)와 직접 구현한 `JsonAlias` 어트리뷰트의 차이점과 활용법을 비교·시연하는 데모입니다.  
실제 현업에서 외부 API, 레거시 시스템 등 다양한 JSON 키 네이밍을 통합 처리해야 할 때 유용한 패턴을 포트폴리오용으로 정리하였습니다.

## 주요 기술 및 구현 포인트

- **JsonProperty (Newtonsoft 기본)**
  - C# 속성과 JSON 키를 1:1로 매핑
  - 직렬화(객체→JSON), 역직렬화(JSON→객체) 모두 지원
  - 단일 키만 매핑 가능(여러 별칭 불가)
  - 예시:  
    ```csharp
    [JsonProperty("player_name")]
    public string Name { get; set; }
    ```

- **JsonAlias (직접 구현)**
  - 여러 개의 JSON 키(별칭)를 하나의 속성에 매핑 가능
  - 역직렬화(JSON→객체)만 지원(직렬화는 기본 속성명 사용)
  - 다양한 외부 데이터 포맷을 하나의 C# 모델로 통합 처리 가능
  - 예시:  
    ```csharp
    [JsonAlias("username", "player_name")]
    public string Name { get; set; }
    ```

- **JsonHelper 유틸리티**
  - `DeserializeWithAlias<T>()` 메서드로 JsonAlias 어트리뷰트가 붙은 속성에 대해 여러 키를 순차적으로 검사하여 값 매핑
  - 리플렉션을 활용한 범용적 구조

- **실제 업무 패턴 반영**
  - 외부 API/서드파티/레거시 시스템 등에서 키 네이밍이 다를 때, 하나의 모델로 통합 파싱
  - 유지보수성과 확장성이 높은 데이터 파싱 구조 경험 어필

## 기대 효과

- 다양한 JSON 포맷 대응 및 데이터 통합 파싱 역량 강조
- 어트리뷰트, 리플렉션, 커스텀 역직렬화 등 고급 C# 활용 경험 어필
- 실무에서의 데이터 호환성, 유지보수성, 확장성 경험 강조

## 주요 코드 예시

```csharp
// JsonProperty 사용 예시 (Newtonsoft 기본)
[JsonProperty("player_name")]
public string Name { get; set; }

// JsonAlias 사용 예시 (직접 구현)
[JsonAlias("username", "player_name")]
public string Name { get; set; }

// JsonHelper로 여러 키 매핑 지원 역직렬화
PlayerData player = JsonHelper.DeserializeWithAlias<PlayerData>(jsonString);
```

## 폴더 구조 및 주요 파일

- `JsonPropertySample.cs` : JsonProperty 어트리뷰트 사용 예제
- `JsonAliasSample.cs` : JsonAlias 어트리뷰트 및 JsonHelper 사용 예제
- `JsonAliasAttribute.cs` : JsonAlias 어트리뷰트 직접 구현
- `JsonHelpler.cs` : JsonAlias 기반 역직렬화 유틸리티
- `Json Alias Example.unity`, `Json Property Example.unity` : 샘플 Scene

## 기타

- 실무에서 다양한 외부 데이터 포맷을 통합 처리해야 하는 상황에 최적화된 구조로, 확장성과 유지보수성을 효과적으로 어필할 수 있는 포트폴리오용 데모입니다.