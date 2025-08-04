using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 
/// Interceptor Filter 패턴
/// 
/// 사전 처리 혹은 사후 처리일때 (전처리, 후처리) 모든 필터를 통과해야 하는 상황에 놓인 경우 사용
/// 
/// 인증처리와 같이 여러가지 조건들이 모두 통과했을 경우, Target 을 실행해야 되는 경우
/// if...if ... 의 연속적인 결합인 경우 검사대상자가 어떤 조건이 바뀔경우 if...if 를 추가/삭제/수정해야 되는 경우가 존재
/// 
/// 각각의 if 를 class (객체) 화 하여, Filter 개념으로 add filter 로 if ...if 를 대체하는 패턴
///
/// 전(후) 처리 조건을 모두 충족되었을때 target 을 실행하는 방식

/// </summary>

public interface IPassFilter
{
    public void Execute(ref PassInfo info);
}

public class PassFilterChain
{
    List<IPassFilter> filters = new List<IPassFilter>();


    public PassFilterChain AddFilter(IPassFilter item)
    {
        filters.Add(item);
        return this;
    }

    public void DoFilter(ref PassInfo info)
    {
        foreach( var each in filters )
            each.Execute(ref info);
    }
}



// IPassFiter 구현부

public class LoginPassFilter : IPassFilter
{
    string passId = "ezra";

    public void Execute(ref PassInfo info)
    {
        // userId 가 인증된 id 인지 검사
        if ( passId.Equals(info.userId))
        {
            Debug.Log("Login Pass");            
        }
        else
        {
            info.result = false;
            info.errmsg = "유저 미자격";
        }
    }
}

public class OPRoomPassFilter : IPassFilter
{
    string passId = "ezra2";

    public void Execute(ref PassInfo info)
    {
        // userId 가 인증된 id 인지 검사
        if (passId.Equals(info.userId))
        {
            Debug.Log("Room Pass");            
        }
        else
        {
            info.result = false;
            info.errmsg = "룸입장 미자격";
        }

    }
}

public class OPLinePassFilter : IPassFilter
{
    string passId = "ezra";

    public void Execute(ref PassInfo info)
    {
        // userId 가 인증된 id 인지 검사
        if (passId.Equals(info.userId))
        {
            Debug.Log("OPLine Pass");           

        }
        else
        {
            info.result = false;
            info.errmsg = "OP자격 미자격";
        }
    }
}



public struct PassInfo
{
    public string userId;

    public bool   result;
    public string errmsg; 
}

public class InterceptorFilterSample : MonoBehaviour
{
    PassFilterChain FilterChain = null;

    private void Awake()
    {
        FilterChain = new PassFilterChain();

        FilterChain.AddFilter( new LoginPassFilter() ).
                    AddFilter( new OPRoomPassFilter() ).
                    AddFilter( new OPLinePassFilter() );
    }

    private void Start()
    {
        // 외부에서 전달받은 정보를 
        PassInfo info = new PassInfo()
        {
            userId = "ezra",
            result = true,
            errmsg = string.Empty,
        };
        

        // 필터에 넣고 처리한다.
        FilterChain.DoFilter(ref info);

        Debug.Log("결과 " + info.result);
        Debug.Log("메세지 " + info.errmsg);
    }

    private void OnDestroy()
    {
        FilterChain = null;
    }
}
