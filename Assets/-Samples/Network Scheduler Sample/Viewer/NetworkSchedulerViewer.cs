#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class NetworkSchedulerViewer : EditorWindow
{
    [MenuItem("Tools/Network/Scheduler Viewer")]
    public static void ShowWindow()
    {
        GetWindow<NetworkSchedulerViewer>("Network Scheduler Viewer");
    }

    private Vector2 scroll;

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("플레이 모드에서만 동작합니다.", MessageType.Info);
            return;
        }

        if (NetworkManager.Instance == null)
        {
            EditorGUILayout.LabelField("NetworkManager 없음");
            return;
        }

        var scheduler = NetworkManager.Instance.scheduler;
        if (scheduler == null)
        {
            EditorGUILayout.LabelField("NetworkScheduler 없음");
            return;
        }

        var requests = scheduler.GetAll();
        if (requests == null)
        {
            EditorGUILayout.LabelField("등록된 요청 없음");
            return;
        }

        int idle = 0, waiting = 0, processing = 0, completed = 0, failed = 0, cancelled = 0;
        foreach (var req in requests)
        {
            switch (req.Status)
            {
                case RequestStatus.Idle: idle++; break;
                case RequestStatus.Waiting: waiting++; break;
                case RequestStatus.Processing: processing++; break;
                case RequestStatus.Completed: completed++; break;
                case RequestStatus.Failed: failed++; break;
                case RequestStatus.Cancelled: cancelled++; break;
            }
        }

        EditorGUILayout.LabelField("=== 네트워크 스케줄러 통계 ===", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Idle: {idle}");
        EditorGUILayout.LabelField($"Waiting: {waiting}");
        EditorGUILayout.LabelField($"Processing: {processing}");
        EditorGUILayout.LabelField($"Completed: {completed}");
        EditorGUILayout.LabelField($"Failed: {failed}");
        EditorGUILayout.LabelField($"Cancelled: {cancelled}");

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var req in requests)
        {
            if (req == null) continue;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"[{req.Method}] {req.Url}");
            EditorGUILayout.LabelField($"상태: {req.Status}");
            EditorGUILayout.LabelField($"Interval: {req.Interval}s");
            EditorGUILayout.LabelField($"Retry: {req.RetryCount}/{req.MaxRetries}");
            EditorGUILayout.LabelField($"마지막시도: {req.LastAttemptTime:F2}");
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }
}
#endif