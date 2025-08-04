using Newtonsoft.Json;
using UnityEngine;

namespace Sample.UniRX
{
    [System.Serializable]
    public class SampleData
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int AlternativeValue { get; set; }
    }

    public class MockSampleService
    {
        // 임의의 데이터를 가진 SampleData 인스턴스를 생성
        public SampleData GenerateMockData()
        {
            return new SampleData
            {
                Name = "Random Item",
                Value = Random.Range(10, 100),
                AlternativeValue = Random.Range(100, 200)
            };
        }

        // SampleData 객체를 JSON 형식으로 직렬화하여 반환
        public string GetMockJson()
        {
            SampleData mockData = GenerateMockData();
            return JsonConvert.SerializeObject(mockData);
        }

        // JSON을 SampleData 객체로 변환하여 반환
        public SampleData GetMockData()
        {
            string jsonString = GetMockJson();
            return JsonConvert.DeserializeObject<SampleData>(jsonString);
        }
    }
}
