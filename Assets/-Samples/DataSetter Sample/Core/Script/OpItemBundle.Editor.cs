#if UNITY_EDITOR

using System.Collections.Generic;
using Korens.Autonomous.Ford;
using Korens.Autonomous.Gamma;
using UnityEditor;
using UnityEngine;

namespace Korens.Autonomous
{

    [CustomEditor(typeof(OpItemBundle))]

    public class OpItemBundleEditor : Editor
    {
        OpItemBundle Bundle;
        List<bool> DataItemFold = new List<bool>(20);
        bool isActiveUnFold = true;
        bool isUnFold = true;

        private SerializedProperty itemsProperty;

        public void OnEnable()
        {
            Bundle = target as OpItemBundle;

            //List<OpItem> list = Bundle.GetItems();
            itemsProperty = serializedObject.FindProperty("Items");

            DataItemFold.Clear();

            for (int i = 0; i < itemsProperty.arraySize; i++)
            {
                DataItemFold.Add(false);
            }

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OpItemBundle Bundle = (OpItemBundle)target;

            EditorGUILayout.BeginVertical();
            Bundle.line = (Line)EditorGUILayout.EnumPopup("라인 종류", Bundle.line);
            EditorGUILayout.Space(20);

            Bundle.id = EditorGUILayout.IntField("Id", Bundle.id);
            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField($"----- Id {Bundle.id}, {Bundle.Name} 관리항목수: 현재 {Bundle.GetItemsSize()} 개 -----");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            // 추가 및 삭제 버튼
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("추가-Last"))
            {
                AddItem(Bundle.line);
            }
            if (GUILayout.Button("삭제-Last"))
            {
                RemoveItem();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            // 모든 항목 Foldout 적용
            isActiveUnFold = EditorGUILayout.ToggleLeft("모든 항목 Foldout 적용", isActiveUnFold);
            if (isActiveUnFold)
            {
                isUnFold = EditorGUILayout.ToggleLeft("항목 펼치기", isUnFold);
                for (int i = 0; i < itemsProperty.arraySize; i++)
                {
                    SerializedProperty item = itemsProperty.GetArrayElementAtIndex(i);
                    item.isExpanded = isUnFold;
                }
            }

            EditorGUILayout.Space(10);

            // 리스트 렌더링
            for (int i = 0; i < itemsProperty.arraySize; i++)
            {
                SerializedProperty item = itemsProperty.GetArrayElementAtIndex(i);
                if (item.isExpanded = EditorGUILayout.Foldout(item.isExpanded, $"Item {i + 1}"))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    DrawItemFields(item);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(5);
                }
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        private void DrawItemFields(SerializedProperty item)
        {
            SerializedProperty taskID = item.FindPropertyRelative("TaskID");
            SerializedProperty opID = item.FindPropertyRelative("OPID");
            SerializedProperty show = item.FindPropertyRelative("Show");

            EditorGUILayout.PropertyField(taskID, new GUIContent("네트워크 Response ID"));
            EditorGUILayout.PropertyField(opID, new GUIContent("공정 번호"));
            EditorGUILayout.PropertyField(show, new GUIContent("해당 항목 표시 유무"));

            if (item.managedReferenceValue is GammaOpItem)
            {
                DrawGammaFields(item);
            }
            else if (item.managedReferenceValue is FordOpItem)
            {
                DrawFordFields(item);
            }
        }

        private void DrawGammaFields(SerializedProperty item)
        {
            SerializedProperty boxType = item.FindPropertyRelative("BoxType");
            SerializedProperty processName = item.FindPropertyRelative("ProcessName");
            SerializedProperty hasSPEC = item.FindPropertyRelative("HasSPEC");
            SerializedProperty minSPEC = item.FindPropertyRelative("MinSPECValue");
            SerializedProperty maxSPEC = item.FindPropertyRelative("MaxSPECValue");
            SerializedProperty isBindingItem = item.FindPropertyRelative("isBindingItem");
            SerializedProperty processFormat = item.FindPropertyRelative("ProcessFormat");
            SerializedProperty processUnit = item.FindPropertyRelative("ProcessUnit");
            SerializedProperty processScale = item.FindPropertyRelative("ProcessScale");
            SerializedProperty hasDashboard = item.FindPropertyRelative("HasDashboard");
            SerializedProperty dashboardCategory = item.FindPropertyRelative("DashboardCategory");
            SerializedProperty spcLink = item.FindPropertyRelative("SPCLink");

            EditorGUILayout.PropertyField(boxType, new GUIContent("표시 형태"));
            EditorGUILayout.PropertyField(processName, new GUIContent("공정 이름"));

            EditorGUILayout.PropertyField(hasSPEC, new GUIContent("SPEC 값 존재 유무"));
            if (hasSPEC.boolValue)
            {
                EditorGUILayout.PropertyField(minSPEC, new GUIContent("SPEC의 하한치"));
                EditorGUILayout.PropertyField(maxSPEC, new GUIContent("SPEC의 상한치"));
            }

            EditorGUILayout.PropertyField(isBindingItem, new GUIContent("바인딩 유무"));
            if (isBindingItem.boolValue)
            {
                EditorGUILayout.PropertyField(processFormat, new GUIContent("공정 데이터 형식"));
                EditorGUILayout.PropertyField(processUnit, new GUIContent("공정 데이터 단위"));
                EditorGUILayout.PropertyField(processScale, new GUIContent("공정 데이터에 곱해질 값"));
            }

            EditorGUILayout.PropertyField(hasDashboard, new GUIContent("대시보드 유무"));
            if (hasDashboard.boolValue)
            {
                EditorGUILayout.PropertyField(dashboardCategory, new GUIContent("대시보드 종류"));
                if (dashboardCategory.enumValueIndex == (int)DashboardCategory.SPCGraph)
                {
                    EditorGUILayout.PropertyField(spcLink, new GUIContent("SPC Link (URL)"));
                }
            }
        }

        private void DrawFordFields(SerializedProperty item)
        {
            SerializedProperty isGroupTask = item.FindPropertyRelative("isGroupTask");
            SerializedProperty processName = item.FindPropertyRelative("ProcessName");
            SerializedProperty groupTaskID = item.FindPropertyRelative("GroupTaskID");
            SerializedProperty minSPEC = item.FindPropertyRelative("MinSPECValue");
            SerializedProperty maxSPEC = item.FindPropertyRelative("MaxSPECValue");
            SerializedProperty spcLink = item.FindPropertyRelative("SPCLink");

            EditorGUILayout.PropertyField(isGroupTask, new GUIContent("그룹 Task인지"));
            if (isGroupTask.boolValue)
            {
                EditorGUILayout.PropertyField(groupTaskID, new GUIContent("그룹 Task의 ID"));
            }
            EditorGUILayout.PropertyField(processName, new GUIContent("공정 이름"));
            EditorGUILayout.PropertyField(minSPEC, new GUIContent("SPEC의 하한치"));
            EditorGUILayout.PropertyField(maxSPEC, new GUIContent("SPEC의 상한치"));
            EditorGUILayout.PropertyField(spcLink, new GUIContent("SPC Link (URL)"));
        }

        private void AddItem(Line line)
        {
            itemsProperty.serializedObject.Update();
            int index = itemsProperty.arraySize;
            itemsProperty.InsertArrayElementAtIndex(index);

            SerializedProperty newItem = itemsProperty.GetArrayElementAtIndex(index);
            if (line == Line.Gamma)
                newItem.managedReferenceValue = new GammaOpItem();
            else if (line == Line.Ford)
                newItem.managedReferenceValue = new FordOpItem();

            itemsProperty.serializedObject.ApplyModifiedProperties();
        }

        private void RemoveItem()
        {
            if (itemsProperty.arraySize > 0)
            {
                itemsProperty.serializedObject.Update();
                itemsProperty.DeleteArrayElementAtIndex(itemsProperty.arraySize - 1);
                itemsProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif