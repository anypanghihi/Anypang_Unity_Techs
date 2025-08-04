using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Sample.OPDataSetter
{
    public class OPNormalData
    {
        public string VALUE;
    }

    public class OPNgCountData
    {
        public string SUM_CNT;
    }

    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private OpNetworkBundle OpItemNetBundle;



        Dictionary<string, string> opDataDictionary = new Dictionary<string, string>();

        private void Start()
        {
            GetOPDataAsync().Forget();
        }

        public List<T> ParseData<T>(string apiData)
        {
            if (!IsValidAPIResponse(apiData))
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<JObject>(apiData);
            var opData = JsonConvert.DeserializeObject<List<T>>(data["resultData"].ToString());

            return opData;
        }

        bool IsValidAPIResponse(string apiData)
        {
            if (apiData != null)
            {
                return !(apiData.Equals(string.Empty) || apiData.Equals("{}"));
            }

            return false;
        }

        private async UniTask<string> SendWebRequest(string param, CancellationTokenSource cts)
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequest.Get(UnityWebRequest.UnEscapeURL(param)))
                {
                    request.timeout = 5;

                    await request.SendWebRequest().WithCancellation(cts.Token);

                    return request.downloadHandler.text;
                }
            }
            catch (UnityWebRequestException ex)
            {
                Debug.LogError($"[{DateTime.Now:MM.dd HH:mm:ss}] {param} {ex.Result} : {ex.Message}");

                return null;
            }
        }

        CancellationTokenSource cts = new CancellationTokenSource();
        public async UniTaskVoid GetOPDataAsync()
        {
            await UniTask.Yield();

            //while (true)
            //{
            //OP 10
            GetOpDataWithJSON("OP10_FaAirPress_Average", "{    \"resultData\": [    {        \"VALUE\": \"5.352\",        \"TID\": \"OP10_FaAirPress\"    }]}", cts);
            GetOpDataWithNgCountJSON("OP10_LineStateNgCount1_Real", "{    \"resultData\": [        {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }]}", cts);
            GetOpDataWithNgCountJSON("OP10_LineStateNgCount2_Real", "{    \"resultData\": [        {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }]}", cts);
            GetOpDataWithJSON("OP10_ProdJud1", "{    \"resultData\": [        {            \"TIMESTAMP\": \"2024-03-22 08:35:08 927:099:500\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.0\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP10_ProdJud1\",            \"SID\": \"OP10\"        }    ]}", cts);
            GetOpDataWithJSON("OP10_ProdJud2", "{    \"resultData\": [        {            \"TIMESTAMP\": \"2024-03-22 08:35:08 927:291:300\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.0\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP10_ProdJud2\",            \"SID\": \"OP10\"        }    ]}", cts);

            //OP20
            GetOpDataWithJSON("OP20_FaAirPress_Average", "{    \"resultData\": [    {        \"VALUE\": \"2.875\",        \"TID\": \"OP20_FaAirPress\"    }]}", cts);
            GetOpDataWithNgCountJSON("OP20_LineStateNgCount_Real", "{    \"resultData\": [        {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }]}", cts);
            GetOpDataWithJSON("OP20_ProdDist", "{    \"resultData\": [        {            \"TIMESTAMP\": \"2024-03-22 09:30:29 756:026:500\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.59\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP20_ProdPress\",            \"SID\": \"OP20\"        }    ]}", cts);
            GetOpDataWithJSON("OP20_ProdPress", "{    \"resultData\": [        {            \"TIMESTAMP\": \"2024-03-22 08:35:16 937:899:000\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"232.29\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP20_ProdDist\",            \"SID\": \"OP20\"        }    ]}", cts);

            //ezra
            Debug.Log("Set Dictionary finished : " + opDataDictionary.Keys.Count);


            // 요청한 정보에 응답이 와서 opDataDictionary 에 정보들이 세팅했다고 치고
            foreach (var kv in opDataDictionary)
            {
                if (OpItemDataContainer.OpDataList.ContainsKey(kv.Key))
                {
                    OpItemDataContainer.OpDataList[kv.Key].Value = kv.Value;
                }
            }

            foreach (var each in OpItemNetBundle.Items)
            {
                // 해당 url 로 호출한다
                // WebRequest( each.reqUrl ) () =>
                {
                    // 응답이 오면 해당 key 값으로 찾는다.

                    // string key = each.respKeys[0];
                    // value = jsonData[key];
                    // 
                    // if (OpItemDataContainer.OpDataList.ContainsKey( key ))
                    // {
                    //    OpItemDataContainer.OpDataList[ key ].Value = value;
                    // }

                }
            }

        }

        async void GetOpDataWithURL(string url, CancellationTokenSource cts)
        {
            string jsonData = await SendWebRequest(url, cts);

            if (jsonData != null)
            {

            }
        }

        void GetOpDataWithJSON(string key, string jsonData, CancellationTokenSource cts)
        {
            if (jsonData != null)
            {
                List<OPNormalData> opDataList = ParseData<OPNormalData>(jsonData);

                foreach (var each in opDataList)
                {
                    string value = each.VALUE;

                    if (opDataDictionary.ContainsKey(key))
                    {
                        opDataDictionary[key] = value;
                    }
                    else
                    {
                        opDataDictionary.Add(key, value);
                    }
                }
            }
        }

        void GetOpDataWithNgCountJSON(string key, string jsonData, CancellationTokenSource cts)
        {
            if (jsonData != null)
            {
                List<OPNgCountData> opDataList = ParseData<OPNgCountData>(jsonData);

                foreach (var each in opDataList)
                {
                    string value = each.SUM_CNT;

                    if (opDataDictionary.ContainsKey(key))
                    {
                        opDataDictionary[key] = value;
                    }
                    else
                    {
                        opDataDictionary.Add(key, value);
                    }
                }
            }
        }
    }
}
