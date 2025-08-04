using System.Collections.Generic;
using Korens.Autonomous;
using UnityEngine;

public class RequestHandler
{
    public string url;
    public float duration;
    //등등
}

[CreateAssetMenu(fileName = "RequestScheduler", menuName = "Scheduler/RequestScheduler")]
public class RequestScheduler : ScriptableObject
{
    private readonly List<OpItemBundleData> scheduledFunctions = new();

    public List<OpItemBundleData> GetScheduledFunctions()
    {
        return scheduledFunctions;
    }

    public void RegisterFunction(OpItemBundleData target)
    {
        if (!scheduledFunctions.Contains(target))
        {
            scheduledFunctions.Add(target);
        }
    }

    public void UnregisterFunction(OpItemBundleData target)
    {
        scheduledFunctions.Remove(target);
        if (scheduledFunctions.Count == 0) scheduledFunctions.Remove(target);
    }

    public void ClearFunctions() => scheduledFunctions.Clear();
}