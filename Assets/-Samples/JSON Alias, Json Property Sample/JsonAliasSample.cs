using Newtonsoft.Json;
using UnityEngine;

namespace Sample.JsonAliasing
{
    /// <summary>
    // JsonAlias(직접 구현 필요)
    // 직렬화(객체 → JSON) : 적용 안 됨
    // 역직렬화 (JSON → 객체) : 적용됨
    // 여러 개의 JSON 키 매핑 : 가능(여러 개의 별칭 매핑 가능)
    // 기본 제공 여부 : 직접 구현해야 함 
    /// </summary>
    public class JsonAliasSample : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerData
        {
            [JsonAlias("username", "player_name")]
            public string Name { get; set; }

            [JsonAlias("score", "player_score")]
            public int Score { get; set; }
        }

        void Start()
        {
            string json1 = @"{ ""username"": ""John"", ""score"": 100 }";
            string json2 = @"{ ""player_name"": ""Alice"", ""player_score"": 200 }";

            PlayerData player1 = JsonHelper.DeserializeWithAlias<PlayerData>(json1);
            PlayerData player2 = JsonHelper.DeserializeWithAlias<PlayerData>(json2);

            Debug.Log($"Player 1 - Name: {player1.Name}, Score: {player1.Score}");
            Debug.Log($"Player 2 - Name: {player2.Name}, Score: {player2.Score}");
        }
    }
}