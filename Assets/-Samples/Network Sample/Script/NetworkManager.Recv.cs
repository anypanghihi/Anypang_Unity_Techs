using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Sample.OPDataSetter;

namespace Sample.Network
{
    public partial class NetworkManager //.Recv
    {
        [SerializeField] private OpNetworkBundle OpItemNetBundle;

        Dictionary<string, string> opDataDictionary = new Dictionary<string, string>();

        CancellationTokenSource cts = new CancellationTokenSource();
        private async UniTaskVoid GetOPDataAsync()
        {
            await UniTask.Yield();

            //while (true)
            //{
            //OP 10
            GetOpDataWithJSON<OPNormalData>("OP10_FaAirPress_Average", "{    \"resultData\":     {        \"VALUE\": \"5.352\",        \"TID\": \"OP10_FaAirPress\"    }}");
            GetOpDataWithJSON<OPNgCountData>("OP10_LineStateNgCount1_Real", "{    \"resultData\":         {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }}");
            GetOpDataWithJSON<OPNgCountData>("OP10_LineStateNgCount2_Real", "{    \"resultData\":         {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }}");
            GetOpDataWithJSON<OPNormalData>("OP10_ProdJud1", "{    \"resultData\":         {            \"TIMESTAMP\": \"2024-03-22 08:35:08 927:099:500\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.0\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP10_ProdJud1\",            \"SID\": \"OP10\"        }    }");
            GetOpDataWithJSON<OPNormalData>("OP10_ProdJud2", "{    \"resultData\":         {            \"TIMESTAMP\": \"2024-03-22 08:35:08 927:291:300\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.0\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP10_ProdJud2\",            \"SID\": \"OP10\"        }    }");

            //OP20
            GetOpDataWithJSON<OPNormalData>("OP20_FaAirPress_Average", "{    \"resultData\":     {        \"VALUE\": \"2.875\",        \"TID\": \"OP20_FaAirPress\"    }}");
            GetOpDataWithJSON<OPNgCountData>("OP20_LineStateNgCount_Real", "{    \"resultData\":         {            \"AB\" : \"B\",            \"MODEL\" : \"3\",            \"HOUR_TITLE\" : \"3\",            \"CNT\" : \"4\",            \"SUM_CNT\" : \"62\",            \"REAL_HOUR_TITLE\" : \"3\",        }}");
            GetOpDataWithJSON<OPNormalData>("OP20_ProdDist", "{    \"resultData\":         {            \"TIMESTAMP\": \"2024-03-22 09:30:29 756:026:500\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"1.59\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP20_ProdPress\",            \"SID\": \"OP20\"        }    }");
            GetOpDataWithJSON<OPNormalData>("OP20_ProdPress", "{    \"resultData\":         {            \"TIMESTAMP\": \"2024-03-22 08:35:16 937:899:000\",            \"TOTCNT\": null,            \"CNT\": null,            \"MAPPING_TABLE_NAME\": \"OWP_MACHBASE\",            \"HOUR_TITLE\": null,            \"VALUE\": \"232.29\",            \"STR_VALUE\": null,            \"TID\": \"GAMMA^OP20_ProdDist\",            \"SID\": \"OP20\"        }    }");

            Debug.Log("Set Dictionary finished : " + opDataDictionary.Keys.Count);


            // 요청한 정보에 응답이 와서 opDataDictionary 에 정보들이 세팅했다고 치고
            foreach (var kv in opDataDictionary)
            {
                //if (OpItemDataContainer.OpDataList.ContainsKey(kv.Key))
                //{
                //    OpItemDataContainer.OpDataList[kv.Key].Value = kv.Value;
                //}

                apiText.text += kv.Key + " : " + kv.Value + "\n";
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

        private async UniTaskVoid GetOpDataWithURL<T>(string key, string url, CancellationTokenSource cts)
        {
            string jsonData = await SendWebRequest(url, cts);

            if (jsonData != null)
            {
                GetOpDataWithJSON<T>(key, jsonData);
            }
        }

        private void GetOpDataWithJSON<T>(string key, string jsonData)
        {
            if (jsonData != null)
            {
                string value = ParseData<T>(jsonData);

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