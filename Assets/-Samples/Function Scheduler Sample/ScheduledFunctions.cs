using System.Collections.Generic;
using UnityEngine;

public class ScheduledFunctions : ScriptableObject
{
    private readonly HashSet<string> functionNames = new();

    public void AddFunction(string functionName) => functionNames.Add(functionName);
    public void RemoveFunction(string functionName) => functionNames.Remove(functionName);
    public void ClearFunctions() => functionNames.Clear();
}