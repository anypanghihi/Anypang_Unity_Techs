#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sample.OPDataSetter
{

    [CustomEditor(typeof(OpItemBundle))]

    public class OpItemBundleEditor : Editor
    {
        OpItemBundle Bundle;
        List<bool> DataItemFold = new List<bool>(20);
        bool isActiveUnFold = true;
        bool isUnFold = true;


        public void OnEnable()
        {
            Bundle = target as OpItemBundle;

            List<OpItem> list = Bundle.GetItems();

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
                        List<OpItem> items = Bundle.GetItems();
                        for (int i = 0; i < items.Count; i++)
                        {
                            DataItemFold[i] = true;
                        }
                    }
                    else
                    {
                        List<OpItem> items = Bundle.GetItems();
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

            List<OpItem> list = Bundle.GetItems();
            for (int i = 0; i < list.Count; i++)
            {
                if (DataItemFold[i] = EditorGUILayout.Foldout(DataItemFold[i], list[i].TaskID))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.Space(3);

                    list[i].TaskID = EditorGUILayout.TextField("네트워크 Response 에서 사용될 ID", list[i].TaskID);
                    list[i].OPID = EditorGUILayout.IntField("공정 번호", list[i].OPID);
                    list[i].BoxType = EditorGUILayout.IntField("표시 형태", list[i].BoxType);

                    list[i].HasDashboard = EditorGUILayout.Toggle("대시보드 유무", list[i].HasDashboard);

                    if (list[i].HasDashboard)
                    {
                        list[i].DashboardCategory = (DashboardCategory)EditorGUILayout.EnumPopup("대시보드 종류 : ", list[i].DashboardCategory);
                    }

                    list[i].APILink = EditorGUILayout.TextField("API Link (URL)", list[i].APILink);

                    list[i].ProcessName = EditorGUILayout.TextField("공정 이름 : ", list[i].ProcessName);
                    list[i].ProcessFormat = EditorGUILayout.TextField("공정 데이터 형식", list[i].ProcessFormat);
                    list[i].ProcessUnit = EditorGUILayout.TextField("공정 데이터 단위", list[i].ProcessUnit);

                    list[i].ProcessScale = EditorGUILayout.FloatField("공정 데이터에 곱해질 값", list[i].ProcessScale);
                    list[i].HasSPEC = EditorGUILayout.Toggle("SPEC 값 존재 유무", list[i].HasSPEC);

                    if (list[i].HasSPEC)
                    {
                        list[i].MinSPECValue = EditorGUILayout.FloatField("SPEC의 하한치", list[i].MinSPECValue);
                        list[i].MaxSPECValue = EditorGUILayout.FloatField("SPEC의 상한치", list[i].MaxSPECValue);
                    }

                    EditorGUILayout.Space(3);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    list[i].Show = EditorGUILayout.Toggle("해당 항목 표시 유무", list[i].Show);
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