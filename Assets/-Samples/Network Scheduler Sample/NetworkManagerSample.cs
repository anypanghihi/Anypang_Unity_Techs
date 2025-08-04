using System;
using System.Collections.Generic;
using Korens.Autonomous;
using UnityEngine;

public class NetworkManagerSample : MonoBehaviour
{
    [SerializeField] private RequestScheduler functionScheduler;
    private readonly Dictionary<OpItemBundleData, float> lastExecutionTimes = new();
    private readonly Dictionary<OpItemBundleData, float> intervals = new();

    private void Update()
    {
        float currentTime = Time.time;

        foreach (var key in lastExecutionTimes.Keys)
        {
            if (!intervals.TryGetValue(key, out float interval)) continue;
            if (currentTime - lastExecutionTimes[key] < interval) continue;

            ExecuteFunction(key);
            lastExecutionTimes[key] = currentTime;
        }
    }

    public OpItemBundleData TestObject;
    public OpItemBundleData TestObject2;
    void Start()
    {
        RegisterFunction(TestObject, 5f);
        RegisterFunction(TestObject, 10f);
        RegisterFunction(TestObject2, 15f);
    }

    public void RegisterFunction(OpItemBundleData target, float interval)
    {
        if (!lastExecutionTimes.ContainsKey(target))
        {
            lastExecutionTimes[target] = Time.time;
            intervals[target] = interval;
            functionScheduler.RegisterFunction(target);
        }
    }

    public void UnregisterFunction(OpItemBundleData target)
    {
        if (lastExecutionTimes.Remove(target))
        {
            intervals.Remove(target);
            functionScheduler.UnregisterFunction(target);
        }
    }

    /// <summary>
    /// 스케쥴링 할 리퀘스트
    /// </summary>
    /// <param name="target"></param>
    private void ExecuteFunction(OpItemBundleData target)
    {
        //target?.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
    }
}