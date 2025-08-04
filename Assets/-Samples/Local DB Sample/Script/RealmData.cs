using Realms;
using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Ŭ���̾�Ʈ ��ü���� ������ ������ ����
/// </summary>
 
public partial class RmLocalData : IRealmObject
{
    // MapTo�� ���� ��������� Realm Studio���� Ȯ�� ���� 
    [MapTo("TDate")]
    public string TDATE { get; set; } // �����

    [MapTo("Message")]
    public string Message { get; set; }


    public RmLocalData() { }
    public RmLocalData(string tDate, string msg)
    {
        this.TDATE   = tDate;
        this.Message = msg;
    }
}

public class LocalLogUnit
{
    public string TID;
    public DateTime TDATE;
    public string Message;

    public LocalLogUnit(string tID, DateTime tDate, string message) 
    {
        TID = tID;
        TDATE = tDate;
        Message = message;
    }
}

public partial class RmLocalLog : IRealmObject
{
    [PrimaryKey]    
    public string TID { get; set; } // �з� �׸�ID �����ؾ� ��. (HOMEUI, OPUI, RestAPI, CAMERA, Network line, Animation ���) 

    public IList<RmLocalData> data { get; }

    // SyncVar
    //public RealmInteger<int> RealmInteger { get; set; }


    public RmLocalLog(string tId, RmLocalData data)
    {
        this.TID = tId;
        this.data.Add(data);
    }

    public RmLocalData Find(string tDate)
    {
        var found = this.data.Where(x => x.TDATE.Equals(tDate));
        

        if (found.Count() > 0)
        {
            return found.First();
        }

        return null;
    }

    public void Add(RmLocalData newdata)
    {
        this.data.Add(newdata);
    }
}




/// <summary>
/// �ý��� Backend ���� ������ ����Ÿ�� ����
/// </summary>

public partial class RmNetData : IRealmObject
{
    [MapTo("Name")]
    public string NAME { get; set; }
    public string VALUE;
    public int    STATUS;
    public string MIN;
    public string MAX;
    public string SDATE; // ����ð�        
}


public partial class RmNetDatas : IRealmObject
{
    [MapTo("TDate")]
    public string NDATE { get; set; } // �������� ���� ����ð�

    public IList<RmNetData> data { get; }

    public RmNetDatas() { }
    public RmNetDatas(string nDate, RmNetData data)
    {
        this.NDATE = nDate;
        this.data.Add(data);
    }


    public void AddData(RmNetData data)
    {
        this.data.Add(data);
    }
}

public partial class RmNetworkLog : IRealmObject
{
    [PrimaryKey]
    public string TID { get; set; }  // �����׸� ID : MES ���

    public IList<RmNetDatas> data { get; }

    // SyncVar
    //public RealmInteger<int> RealmInteger { get; set; }


    public RmNetworkLog() { }
    public RmNetworkLog(string tId, RmNetDatas data)
    {
        this.TID = tId;
        this.data.Add(data);
    }

    public RmNetDatas Find(string tDate)
    {
        var found = this.data.Where(x => x.NDATE.Equals(tDate));


        if (found.Count() > 0)
        {
            return found.First();
        }

        return null;
    }

    public void Add(RmNetDatas newdatas)
    {
        this.data.Add(newdatas);
    }
}