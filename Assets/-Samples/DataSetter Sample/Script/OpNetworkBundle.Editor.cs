#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sample.OPDataSetter
{
    [CustomEditor(typeof(OpNetworkBundle))]

    public class OpNetworkBundleEditor : Editor
    {
        OpNetworkBundle Bundle;
        List<bool> DataItemFold = new List<bool>(20);
        bool isActiveUnFold = false;
        bool isUnFold = false;


        public void OnEnable()
        {
            Bundle = target as OpNetworkBundle;

            List<OpNetwork> list = Bundle.GetItems();

            DataItemFold.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                DataItemFold.Add(false);
            }

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            Bundle.id = EditorGUILayout.IntField("Id", Bundle.id);
            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(string.Format("----- Id {1}, {0} 관리항목수: 현재 {2} 개 -----", Bundle.Name, Bundle.id, Bundle.GetItemsSize()));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("추가-Last"))
            {
                Bundle.AddItem();
                DataItemFold.Add(false);
            }
            if (GUILayout.Button("삭제-Last"))
            {
                Bundle.RemoveItem();
                DataItemFold.RemoveAt(Bundle.GetItemsSize() - 1);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            {
                isActiveUnFold = EditorGUILayout.ToggleLeft("모든 항목 Foldout 적용", isActiveUnFold);
                if (isActiveUnFold)
                {
                    isUnFold = EditorGUILayout.ToggleLeft("항목 펼치기", isUnFold);

                    if (isUnFold)
                    {
                        List<OpNetwork> items = Bundle.GetItems();
                        for (int i = 0; i < items.Count; i++)
                        {
                            DataItemFold[i] = true;
                        }
                    }
                    else
                    {
                        List<OpNetwork> items = Bundle.GetItems();
                        for (int i = 0; i < items.Count; i++)
                        {
                            DataItemFold[i] = false;
                        }
                    }
                }
            }
            EditorGUILayout.Space(10);


            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.EndHorizontal();

            List<OpNetwork> list = Bundle.GetItems();
            for (int i = 0; i < list.Count; i++)
            {
                if (DataItemFold[i] = EditorGUILayout.Foldout(DataItemFold[i], list[i].reqName))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.Space(3);

                    list[i].reqName = EditorGUILayout.TextField("요청 ID 이름", list[i].reqName);

                    EditorGUILayout.Space(3);

                    list[i].reqUrl = EditorGUILayout.TextField("네트워크에 요청할 정보", list[i].reqUrl);

                    EditorGUILayout.Space(3);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Label("네트워크 Response 에서 사용될 ID 목록");

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("키 추가-Last"))
                    {
                        Bundle.AddItemKey(i);
                    }
                    if (GUILayout.Button("키 삭제-Last"))
                    {
                        Bundle.RemoveItemKey(i);
                    }
                    EditorGUILayout.EndHorizontal();


                    for (int j = 0; j < list[i].respKeys.Count; j++)
                    {
                        list[i].respKeys[j] = EditorGUILayout.TextField("Response Key ID (" + j + ")", list[i].respKeys[j]);
                    }

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space(3);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(5);
                }
            }

            EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
#endif