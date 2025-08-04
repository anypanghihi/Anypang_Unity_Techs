using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine.Networking;

namespace Sample.Network
{
    public partial class NetworkManager //.Send
    {
        private string ParseData<T>(string apiData)
        {
            if (!IsValidAPIResponse(apiData))
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<JObject>(apiData);
            var opData = JsonConvert.DeserializeObject<T>(data["resultData"].ToString());

            return opData.ToString();
        }

        /// <summary>
        /// �迭 ������ JSON �����͸� �Ľ��Ѵ�.
        /// </summary>
        private List<T> ParseDataList<T>(string apiData)
        {
            if (!IsValidAPIResponse(apiData))
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<JObject>(apiData);
            var opData = JsonConvert.DeserializeObject<List<T>>(data["resultData"].ToString());

            return opData;
        }

        /// <summary>
        /// ��Ű���� ���� Case�� �����Ѵ�. <br/>
        /// �۽������� ���� ���� �ٸ�. <br/>
        /// "{}" �̷��� ���°� ������ �ƴ� ���� �ִٴ� ��.
        /// </summary>
        private bool IsValidAPIResponse(string apiData)
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
    }
}