using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Korens.Autonomous;

[CustomEditor(typeof(RequestScheduler))]
public class RequestSchedulerEditor : Editor
{
    private RequestScheduler scheduler;
    private bool showScheduledFunctions = true; // Foldout UI 추가

    private void OnEnable()
    {
        scheduler = (RequestScheduler)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);

        showScheduledFunctions = EditorGUILayout.Foldout(showScheduledFunctions, "Scheduled Network Request");

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
        List<OpItemBundleData> scheduledFunctions = GetScheduledFunctions();

        if (scheduledFunctions.Count == 0)
        {
            EditorGUILayout.LabelField("No requests scheduled.");
            return;
        }

        foreach (var each in scheduledFunctions)
        {
            EditorGUILayout.BeginHorizontal();

            // Ping GameObject 기능 추가
            if (GUILayout.Button(each.name, GUILayout.Width(150)))
            {
                EditorGUIUtility.PingObject(each);
            }

            // EditorGUILayout.LabelField($"({each.Count} functions)");

            EditorGUILayout.EndHorizontal();

            // foreach (string functionName in functions)
            // {
            //     EditorGUILayout.LabelField($"  • {functionName}");
            // }

            GUILayout.Space(5);
        }
    }

    private List<OpItemBundleData> GetScheduledFunctions()
    {
        return scheduler.GetScheduledFunctions();
    }
}