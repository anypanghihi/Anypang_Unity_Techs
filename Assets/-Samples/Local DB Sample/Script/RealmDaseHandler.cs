using UnityEngine;
using Realms;
using System;
using Cysharp.Threading.Tasks;
using System.Collections.Concurrent;

public partial class RealmDaseHandler : MonoBehaviour
{
    public static RealmDaseHandler instance;

    private Realm realmL;
    private Realm realmN;

    private RmLocalLog localLog;
    private RmNetworkLog networkLog;

    RealmConfiguration configL;
    RealmConfiguration configN;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreateRealmsBase();
    }

    private void OnDestroy()
    {
        realmL.Dispose();
        realmN.Dispose();
    }

    void CreateRealmsBase()
    {
        try
        {
            configL = new RealmConfiguration(Application.streamingAssetsPath + "/Realms/local.realm");
            configN = new RealmConfiguration(Application.streamingAssetsPath + "/Realms/network.realm");

            configL.Schema = new[] { typeof(RmLocalData), typeof(RmLocalLog) };
            configN.Schema = new[] { typeof(RmNetData), typeof(RmNetDatas), typeof(RmNetworkLog) };


#if UNITY_EDITOR
            Realm.DeleteRealm(configL);
            Realm.DeleteRealm(configN);
# endif
            realmL = Realm.GetInstance(configL);
            realmN = Realm.GetInstance(configN);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            CreateRealmsBase();
        }
    }

    public void InsertLocalDBDataForTest(string Tid, DateTime dateTime)
    {
        string TDate = dateTime.ToString("yyyy.MM.dd");

        int rand = UnityEngine.Random.Range(0, 9999);

        RmLocalData data = new RmLocalData();
        {
            data.Message = rand.ToString();

            data.TDATE = TDate;
        }

        localLog = realmL.Find<RmLocalLog>(Tid);
        if (localLog == null)
        {
            realmL.Write(() =>
            {
                realmL.Add(new RmLocalLog(Tid, data));
            });
        }
        else
        {
            realmL.Write(() =>
            {
                localLog.Add(data);
            });
        }
    }

    public void InsertLocalDBData(string Tid, RmLocalData data, DateTime dateTime)
    {
        string TDate = dateTime.ToString("yyyy.MM.dd");

        data.TDATE = TDate;

        localLog = realmL.Find<RmLocalLog>(Tid);
        if (localLog == null)
        {
            realmL.Write(() =>
            {
                realmL.Add(new RmLocalLog(Tid, data));
            });
        }
        else
        {
            realmL.Write(() =>
            {
                localLog.Add(data);
            });
        }
    }

    private ConcurrentQueue<LocalLogUnit> localCQ = new ConcurrentQueue<LocalLogUnit>();

    public void EnqueueLocalData(LocalLogUnit data)
    {
        localCQ.Enqueue(data);

        InsertLocalDBDataAsync().Forget();
    }

    public async UniTaskVoid InsertLocalDBDataAsync()
    {
        UniTask.RunOnThreadPool(() => InsertLocalDBDataProcess(), true, this.GetCancellationTokenOnDestroy()).Forget();

        await UniTask.Yield();

        realmL.Refresh();
    }

    async UniTaskVoid InsertLocalDBDataProcess()
    {
        try
        {
            while (localCQ.Count > 0)
            {
                Debug.Log(localCQ.Count.ToString());

                if (localCQ.TryDequeue(out LocalLogUnit result))
                {
                    Debug.Log($"{result.TID} : Dequeue Entered");

                    RmLocalData data = new RmLocalData();
                    {
                        data.TDATE = result.TDATE.ToString("yyyy.MM.dd");
                        data.Message = result.Message;
                    }

                    await UniTask.Yield();

                    using (var taskRealm = Realm.GetInstance(configL))
                    {
                        localLog = taskRealm.Find<RmLocalLog>(result.TID);

                        using (var trans = taskRealm.BeginWrite())
                        {
                            if (localLog == null)
                            {
                                taskRealm.Add(new RmLocalLog(result.TID, data));
                            }
                            else
                            {
                                localLog.Add(data);
                            }

                            trans.Commit();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Realm Write Exception : " + ex);
        }
    }

    public void InsertNetworkDBDataForTest(string Tid, DateTimeOffset dateTimeOffset)
    {
        string NDate = dateTimeOffset.ToString("yyyy.MM.dd");

        int rand = UnityEngine.Random.Range(0, 9999);

        RmNetData data = new RmNetData();
        {
            string rData = rand.ToString();

            data.NAME = rData;
            data.VALUE = rData;
            data.STATUS = rand;
            data.MIN = rData;
            data.MAX = rData;
            data.SDATE = DateTimeOffset.Now.ToString("HH:mm:ss");
        }

        networkLog = realmN.Find<RmNetworkLog>(Tid);
        if (networkLog == null)
        {
            realmL.Write(() =>
            {
                RmNetDatas datas = new RmNetDatas(NDate, data);

                realmN.Add(new RmNetworkLog(Tid, datas));
            });
        }
        else
        {
            realmL.Write(() =>
            {
                RmNetDatas found = networkLog.Find(NDate);
                if (found != null)
                {
                    found.AddData(data);
                }
                else
                {
                    networkLog.Add(new RmNetDatas(NDate, data));
                }
            });
        }
    }

    public void InsertNetworkDBData(string Tid, RmNetData data, DateTimeOffset dateTimeOffset)
    {
        string NDate = dateTimeOffset.ToString("yyyy.MM.dd");

        int rand = UnityEngine.Random.Range(0, 9999);

        data.SDATE = DateTimeOffset.Now.ToString("HH:mm:ss");

        networkLog = realmN.Find<RmNetworkLog>(Tid);
        if (networkLog == null)
        {
            realmL.Write(() =>
            {
                RmNetDatas datas = new RmNetDatas(NDate, data);

                realmN.Add(new RmNetworkLog(Tid, datas));
            });
        }
        else
        {
            realmL.Write(() =>
            {
                RmNetDatas found = networkLog.Find(NDate);
                if (found != null)
                {
                    found.AddData(data);
                }
                else
                {
                    networkLog.Add(new RmNetDatas(NDate, data));
                }
            });
        }
    }

    public async UniTaskVoid InsertNetworkDBDataAsync()
    {
        UniTask.RunOnThreadPool(() => InsertNetworkDBDataProcess(), true, this.GetCancellationTokenOnDestroy()).Forget();

        await UniTask.Yield();

        realmN.Refresh();
    }

    async UniTaskVoid InsertNetworkDBDataProcess()
    {
        await UniTask.Yield();
    }
}
