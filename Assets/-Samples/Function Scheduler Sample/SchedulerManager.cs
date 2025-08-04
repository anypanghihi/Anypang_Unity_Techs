using System;
using System.Collections.Generic;
using UnityEngine;

public class SchedulerManager : MonoBehaviour
{
    [SerializeField] private FunctionScheduler functionScheduler;
    private readonly Dictionary<(GameObject, string), float> lastExecutionTimes = new();
    private readonly Dictionary<(GameObject, string), float> intervals = new();

    private void Update()
    {
        float currentTime = Time.time;

        var keys = new List<(GameObject, string)>(lastExecutionTimes.Keys);
        foreach (var key in keys)
        {
            if (!intervals.TryGetValue(key, out float interval)) continue;
            if (currentTime - lastExecutionTimes[key] < interval) continue;

            ExecuteFunction(key.Item1, key.Item2);
            lastExecutionTimes[key] = currentTime;
        }
    }

    public GameObject TestObject;
    public GameObject TestObject2;
    void Start()
    {
        RegisterFunction(TestObject, "PrintMessage", 5f);
        RegisterFunction(TestObject, "PrintMessage2", 10f);
        RegisterFunction(TestObject2, "PrintMessage3", 15f);
    }

    public void RegisterFunction(GameObject target, string functionName, float interval)
    {
        var key = (target, functionName);
        if (!lastExecutionTimes.ContainsKey(key))
        {
            lastExecutionTimes[key] = Time.time;
            intervals[key] = interval;
            functionScheduler.RegisterFunction(target, functionName);
        }
    }

    public void UnregisterFunction(GameObject target, string functionName)
    {
        var key = (target, functionName);
        if (lastExecutionTimes.Remove(key))
        {
            intervals.Remove(key);
            functionScheduler.UnregisterFunction(target, functionName);
        }
    }

    private void ExecuteFunction(GameObject target, string functionName)
    {
        target?.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
    }
}