using Newtonsoft.Json;
using UnityEngine;

namespace Sample.JsonAliasing
{
    /// <summary>
    // JsonProperty
    // 직렬화(객체 → JSON) : 적용됨
    // 역직렬화(JSON → 객체) : 적용됨
    // 여러 개의 JSON 키 매핑 : 불가능(단일 키만 사용 가능)
    // 기본 제공 여부 : Newtonsoft.Json에서 기본 지원
    /// </summary>
    public class JsonPropertySample : MonoBehaviour
    {
        [System.Serializable]
        public class Player
        {
            [JsonProperty("player_name")]  // JSON에서 "player_name" 키와 매핑됨
            public string Name { get; set; }

            [JsonProperty("player_score")]
            public int Score { get; set; }
        }

        void Start()
        {
            string json = @"{ ""player_name"": ""John"", ""player_score"": 100 }";

            // 역직렬화 (JSON → 객체)
            Player player = JsonConvert.DeserializeObject<Player>(json);
            Debug.Log($"Player Name: {player.Name}, Score: {player.Score}");

            // 직렬화 (객체 → JSON)
            string serializedJson = JsonConvert.SerializeObject(player, Formatting.Indented);
            Debug.Log($"Serialized JSON: {serializedJson}");
        }
    }
}