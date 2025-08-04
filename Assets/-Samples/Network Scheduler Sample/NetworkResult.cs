using System;
using System.Collections.Generic;
using Unity.Collections;

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

    /// <summary>
    /// 응답 헤더
    /// </summary>
    public Dictionary<string, string> Headers { get; }

    /// <summary>
    /// Ford Data 처리용 임시 ID
    /// </summary>
    public string TID;

    /// <summary>
    /// 응답이 성공적인지 여부 (200~299 상태 코드)
    /// </summary>
    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

    /// <summary>
    /// 네트워크 응답 결과를 초기화합니다.
    /// </summary>
    /// <param name="statusCode">HTTP 상태 코드</param>
    /// <param name="nativeData">응답 본문을 바이트 배열로 변환한 것</param>
    /// <param name="response">응답 본문</param>
    /// <param name="headers">응답 헤더</param>
    public NetworkResult(int statusCode, string response = null, Dictionary<string, string> headers = null)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Ford Data 처리용 임시 NetworkResult 생성자
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="TID"></param>
    /// <param name="response"></param>
    public NetworkResult(int statusCode, string TID, string response = null)
    {
        StatusCode = statusCode;
        this.TID = TID;
        Response = response;
    }
}