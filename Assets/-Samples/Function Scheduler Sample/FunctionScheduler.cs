using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FunctionScheduler", menuName = "Scheduler/FunctionScheduler")]
public class FunctionScheduler : ScriptableObject
{
    private readonly Dictionary<GameObject, List<string>> scheduledFunctions = new();

    public void RegisterFunction(GameObject target, string functionName)
    {
        if (!scheduledFunctions.TryGetValue(target, out var functions))
        {
            functions = new List<string>();
            scheduledFunctions[target] = functions;
        }

        if (!functions.Contains(functionName))
        {
            functions.Add(functionName);
        }
    }

    public void UnregisterFunction(GameObject target, string functionName)
    {
        if (!scheduledFunctions.TryGetValue(target, out var functions)) return;

        functions.Remove(functionName);
        if (functions.Count == 0) scheduledFunctions.Remove(target);
    }

    public void ClearFunctions() => scheduledFunctions.Clear();
}