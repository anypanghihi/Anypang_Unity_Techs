using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Network.ZeroMemoryAllocSample
{
    /// <summary>
    /// 네트워크 요청의 응답 결과를 담는 클래스입니다.
    /// </summary>
    public class NetworkResult
    {
        /// <summary>
        /// HTTP 상태 코드
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// 응답 본문
        /// </summary>
        public string Response { get; }
        public NativeArray<byte>.ReadOnly? NativeData { get; }
        public ReadOnlySpan<byte> Span => NativeData.HasValue ? NativeData.Value.AsReadOnlySpan() : default;

        /// <summary>
        /// 응답 헤더
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// 응답이 성공적인지 여부 (200~299 상태 코드)
        /// </summary>
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

        /// <summary>
        /// 네트워크 응답 결과를 초기화합니다.
        /// </summary>
        /// <param name="statusCode">HTTP 상태 코드</param>
        /// <param name="response">응답 본문</param>
        /// <param name="headers">응답 헤더</param>
        public NetworkResult(int statusCode, string response, Dictionary<string, string> headers = null)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers ?? new Dictionary<string, string>();
        }

        public NetworkResult(int statusCode, NativeArray<byte>.ReadOnly? nativeData = null)
        {
            StatusCode = statusCode;
            NativeData = nativeData;
        }
    }
}