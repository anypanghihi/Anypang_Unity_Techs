using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

public class UnityProxyServer : MonoBehaviour
{
    private HttpListener listener;
    private Thread serverThread;
    public string targetUrl;

    void Start()
    {
        StartProxyServer();
    }

    void OnApplicationQuit()
    {
        StopProxyServer();
    }

    public void StartProxyServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/"); // Unity가 제공하는 프록시 주소
        listener.Start();
        Debug.Log("✅ Unity 프록시 서버 시작됨: http://localhost:8080/");

        serverThread = new Thread(HandleRequests);
        serverThread.Start();
    }

    public void StopProxyServer()
    {
        listener.Stop();
        serverThread.Abort();
        Debug.Log("🛑 Unity 프록시 서버 중지됨.");
    }

    private void HandleRequests()
    {
        while (listener.IsListening)
        {
            try
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // 🔹 원본 사이트로 요청 보내기 (모든 요청 정보 포함)
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(targetUrl + request.Url.PathAndQuery);
                webRequest.Method = request.HttpMethod;
                webRequest.UserAgent = request.UserAgent;
                webRequest.Accept = request.Headers["Accept"];
                webRequest.Referer = request.Headers["Referer"];

                // 🔹 POST 요청이면 데이터 전달
                if (request.HttpMethod == "POST" && request.HasEntityBody)
                {
                    using (Stream requestStream = webRequest.GetRequestStream())
                    {
                        request.InputStream.CopyTo(requestStream);
                    }
                }

                // 🔹 원본 사이트에서 응답 받기
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                responseStream.CopyTo(memoryStream);
                byte[] responseData = memoryStream.ToArray();

                // 🔹 응답 헤더 설정 (원본과 동일하게 설정)
                response.ContentType = webResponse.ContentType + "; charset=UTF-8"; // 🔥 인코딩 문제 해결
                response.ContentLength64 = responseData.Length;
                response.StatusCode = (int)webResponse.StatusCode;

                // 🔹 CORS 문제 해결 (필요할 경우)
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

                // 🔹 데이터 전송
                response.OutputStream.Write(responseData, 0, responseData.Length);
                response.Close();
            }
            catch (Exception e)
            {
                Debug.LogError($"❌ 오류 발생: {e.Message}");
            }
        }
    }
}
