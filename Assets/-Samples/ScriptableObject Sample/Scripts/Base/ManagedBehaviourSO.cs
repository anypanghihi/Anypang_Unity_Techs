#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ManagedBehaviourSO", menuName = "SO/Base/ManagedBehaviour")]
public class ManagedBehaviourSO : ScriptableObject
{
    const int Capacity = 10; // Memory 의 연속성 : Runtime 시 Capacity 를 넘어가는 숫자만큼 할당하자.

    public List<Action> ManagedUpdater = new List<Action>(Capacity);

    public void Register(Action action) { ManagedUpdater.Add(action); }
    public void UnRegister(Action action) { ManagedUpdater.Remove(action); }


    private void OnEnable()
    {
        UpdateAsync().Forget();
    }

    CancellationTokenSource disableCancellation = null;
    async UniTaskVoid UpdateAsync()
    {
        await UniTask.Yield();

        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }
        disableCancellation = new CancellationTokenSource();


        while (ManagedUpdater != null)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: disableCancellation.Token);

            foreach (Action update in ManagedUpdater)
            {
                update();
            }
        }
    }

    private void OnDisable()
    {
        disableCancellation.Cancel();
        disableCancellation.Dispose();
        disableCancellation = null;
    }
}











#if UNITY_EDITOR

namespace ScriptableObjects
{
    [CustomEditor(typeof(ManagedBehaviourSO))]

    public class ManagedBehaviourSOEditor : Editor
    {
        ManagedBehaviourSO Bundle;


        public void OnEnable()
        {
            Bundle = target as ManagedBehaviourSO;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            GUILayout.Space(10);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Managed Update Listeners");
            DrawLine();

            if (Bundle.ManagedUpdater != null && Bundle.ManagedUpdater.Count > 0)
            {
                foreach (var each in Bundle.ManagedUpdater)
                {
                    DrawButton(each.Target);
                }
            }
            else
            {
                EditorGUILayout.LabelField("List is empty (Runtime 시 등록됨)");
            }


            EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private void DrawLine()
        {
            // 실선 색상 설정
            Color originalColor = GUI.color;
            GUI.color = Color.gray;

            // Rect 설정 (x, y, width, height)
            Rect rect = GUILayoutUtility.GetRect(0, 3); // 높이를 2로 설정하여 선의 두께 지정
            EditorGUI.DrawRect(rect, GUI.color); // 실선 그리기

            // 색상 복원
            GUI.color = originalColor;

            GUILayout.Space(5);
        }

        private void DrawButton(object target)
        {
            GUIStyle leftAlignedStyle = new GUIStyle(GUI.skin.button);
            leftAlignedStyle.alignment = TextAnchor.MiddleLeft; // 텍스트 왼쪽 정렬            

            string name = target.ToString();
            if (GUILayout.Button(name, leftAlignedStyle, GUILayout.Width(300)))
            {
                int lastSpaceIndex = name.LastIndexOf(' ');

                // 마지막 빈칸이 있는 경우
                if (lastSpaceIndex != -1)
                {
                    // 빈칸 앞까지의 문자열 추출
                    string result = name.Substring(0, lastSpaceIndex);

                    GameObject foundObject = GameObject.Find(result);
                    if (foundObject != null)
                    {
                        EditorGUIUtility.PingObject(foundObject);
                    }
                }
            }
        }
    }
}

#endif