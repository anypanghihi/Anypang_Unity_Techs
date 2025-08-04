using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 
/// Interceptor Filter ����
/// 
/// ���� ó�� Ȥ�� ���� ó���϶� (��ó��, ��ó��) ��� ���͸� ����ؾ� �ϴ� ��Ȳ�� ���� ��� ���
/// 
/// ����ó���� ���� �������� ���ǵ��� ��� ������� ���, Target �� �����ؾ� �Ǵ� ���
/// if...if ... �� �������� ������ ��� �˻����ڰ� � ������ �ٲ��� if...if �� �߰�/����/�����ؾ� �Ǵ� ��찡 ����
/// 
/// ������ if �� class (��ü) ȭ �Ͽ�, Filter �������� add filter �� if ...if �� ��ü�ϴ� ����
///
/// ��(��) ó�� ������ ��� �����Ǿ����� target �� �����ϴ� ���

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



// IPassFiter ������

public class LoginPassFilter : IPassFilter
{
    string passId = "ezra";

    public void Execute(ref PassInfo info)
    {
        // userId �� ������ id ���� �˻�
        if ( passId.Equals(info.userId))
        {
            Debug.Log("Login Pass");            
        }
        else
        {
            info.result = false;
            info.errmsg = "���� ���ڰ�";
        }
    }
}

public class OPRoomPassFilter : IPassFilter
{
    string passId = "ezra2";

    public void Execute(ref PassInfo info)
    {
        // userId �� ������ id ���� �˻�
        if (passId.Equals(info.userId))
        {
            Debug.Log("Room Pass");            
        }
        else
        {
            info.result = false;
            info.errmsg = "������ ���ڰ�";
        }

    }
}

public class OPLinePassFilter : IPassFilter
{
    string passId = "ezra";

    public void Execute(ref PassInfo info)
    {
        // userId �� ������ id ���� �˻�
        if (passId.Equals(info.userId))
        {
            Debug.Log("OPLine Pass");           

        }
        else
        {
            info.result = false;
            info.errmsg = "OP�ڰ� ���ڰ�";
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
        // �ܺο��� ���޹��� ������ 
        PassInfo info = new PassInfo()
        {
            userId = "ezra",
            result = true,
            errmsg = string.Empty,
        };
        

        // ���Ϳ� �ְ� ó���Ѵ�.
        FilterChain.DoFilter(ref info);

        Debug.Log("��� " + info.result);
        Debug.Log("�޼��� " + info.errmsg);
    }

    private void OnDestroy()
    {
        FilterChain = null;
    }
}
