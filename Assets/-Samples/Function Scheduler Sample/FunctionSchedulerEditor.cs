using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(FunctionScheduler))]
public class FunctionSchedulerEditor : Editor
{
    private FunctionScheduler scheduler;
    private bool showScheduledFunctions = true; // 🔹 Foldout UI 추가

    private void OnEnable()
    {
        scheduler = (FunctionScheduler)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        showScheduledFunctions = EditorGUILayout.Foldout(showScheduledFunctions, "Scheduled Functions");

        if (showScheduledFunctions)
        {
            DrawScheduledFunctions();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawScheduledFunctions()
    {
        Dictionary<GameObject, List<string>> scheduledFunctions = GetScheduledFunctions();

        if (scheduledFunctions.Count == 0)
        {
            EditorGUILayout.LabelField("No functions scheduled.");
            return;
        }

        foreach (var kvp in scheduledFunctions)
        {
            GameObject targetObject = kvp.Key;
            List<string> functions = kvp.Value;

            EditorGUILayout.BeginHorizontal();

            // 🔹 Ping GameObject 기능 추가
            if (GUILayout.Button(targetObject.name, GUILayout.Width(150)))
            {
                EditorGUIUtility.PingObject(targetObject);
            }

            EditorGUILayout.LabelField($"({functions.Count} functions)");

            EditorGUILayout.EndHorizontal();

            foreach (string functionName in functions)
            {
                EditorGUILayout.LabelField($"  • {functionName}");
            }

            GUILayout.Space(5);
        }
    }

    // 🔹 Reflection을 활용하여 FunctionScheduler 내부 Dictionary 가져오기
    private Dictionary<GameObject, List<string>> GetScheduledFunctions()
    {
        var field = typeof(FunctionScheduler).GetField("scheduledFunctions",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return field?.GetValue(scheduler) as Dictionary<GameObject, List<string>> ?? new Dictionary<GameObject, List<string>>();
    }
}