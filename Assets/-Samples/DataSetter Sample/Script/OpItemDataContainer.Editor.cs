#if UNITY_EDITOR

using Cysharp.Text;
using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Sample.OPDataSetter
{
    [CustomEditor(typeof(OpItemDataContainer))]
    public class OpItemDataContainerEditor : Editor
    {
        OpItemDataContainer container = null;

        private void OnEnable()
        {
            container = target as OpItemDataContainer;
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            GUIStyle myStyle = new GUIStyle(GUI.skin.box);
            myStyle.normal.textColor = Color.yellow;

            GUILayout.Space(10);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Box("Edit - OpItem Data", myStyle);

            if (GUILayout.Button("Create OpItemBundle Data", GUILayout.Height(30)))
            {
                ClearAllBundle();

                foreach (OpItemBundle each in container.Bundles)
                {
                    OpItemBundleData data = GameObject.Instantiate<OpItemBundleData>(container.BundlePrefab, Vector3.zero, Quaternion.identity, container.DataTM);
                    data.gameObject.name = ZString.Concat("OP", each.id);
                    data.Bundle = each;
                    data.BundleId = each.id;
                    data.Category = container.Category;
                    data.CreateItemByEditor();

                    container.BundleDatas.Add(each.id, data);

                    UIContainer root = GameObject.Instantiate<UIContainer>(container.ViewTMPrefab, container.ViewTM);
                    root.name = ZString.Concat("ItemView ", data.gameObject.name);
                    data.UIView = root;
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Remove OpItemBundle Data", GUILayout.Height(30)))
            {
                ClearAllBundle();
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();


            GUILayout.Space(10);

            //serializedObject.Update();
            //DrawDefaultInspector();

            //serializedObject.ApplyModifiedProperties();

            GUILayout.Space(10);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Box("Edit - OpItem View", myStyle);

            {
                // 패널별로 가장 많은 Item 을 찾아서 그 갯수만큼 미리적재
                List<OpItemBundleData> list = container.transform.GetComponentsInChildren<OpItemBundleData>().ToList();


                List<OpItemBundleData> found = list.FindAll(x => x.UIView == null);

                if (found.Count > 0)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(5);

                    EditorGUILayout.HelpBox("OpItem Container/OpItemBundle Data 에 UIContainer 연결이 없습니다.", MessageType.Error);

                    GUILayout.BeginHorizontal();


                    foreach (OpItemBundleData each in found)
                    {
                        if (EditorGUILayout.LinkButton(each.name))
                        {
                            EditorGUIUtility.PingObject(each);
                        }
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                }
            }


            if (GUILayout.Button("Create OpItemView Panel", GUILayout.Height(30)))
            {
                ClearAllPanel();

                // 패널별로 가장 많은 Item 을 찾아서 그 갯수만큼 미리적재
                List<OpItemBundleData> list = container.transform.GetComponentsInChildren<OpItemBundleData>().ToList();


                foreach (OpItemBundleData item in list)
                {
                    // 이미 있으면 아래 계산을 다 한 녀석이다.
                    if (container.UIViewDatas.ContainsKey(item.UIView)) continue;


                    List<OpItemBundleData> found = list.FindAll(x => x.UIView.Equals(item.UIView));

                    int maxItems = 0;
                    foreach (OpItemBundleData each in found)
                    {
                        if (maxItems < each.GetItemsSize)
                            maxItems = each.GetItemsSize;
                    }

                    List<OpPanelInfoDataSelector> panelInfo = new List<OpPanelInfoDataSelector>();
                    for (int i = 0; i < maxItems; i++)
                    {
                        OpPanelInfoDataSelector data = GameObject.Instantiate<OpPanelInfoDataSelector>(container.OpPanelPrefab, Vector3.zero, Quaternion.identity, item.UIView.transform);
                        data.name = ZString.Concat("DATA - ", i);

                        data.SetItemData(item.Itemlist[i].ItemData, item.Category);
                        panelInfo.Add(data);
                    }

                    container.UIViewDatas.Add(item.UIView, panelInfo);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Remove OpItemView Panel", GUILayout.Height(30)))
            {
                ClearAllPanel();
            }

            GUILayout.EndVertical();
            GUILayout.Space(10);


            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        void ClearAllBundle()
        {
            List<OpItemBundleData> list = container.transform.GetComponentsInChildren<OpItemBundleData>().ToList();

            foreach (OpItemBundleData data in list)
            {
                Object.DestroyImmediate(data.UIView.gameObject);
            }

            container.BundleDatas.Clear();

            for (int i = container.DataTM.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(container.DataTM.GetChild(i).gameObject);
            }
        }

        void ClearAllPanel()
        {
            List<OpItemBundleData> list = container.transform.GetComponentsInChildren<OpItemBundleData>().ToList();

            foreach (OpItemBundleData data in list)
            {
                Transform tm = data.UIView.transform;
                if (tm.childCount > 0)
                {
                    for (int i = tm.childCount - 1; i >= 0; i--)
                        Object.DestroyImmediate(tm.GetChild(i).gameObject);
                }
            }

            container.UIViewDatas.Clear();
        }



    }
}

#endif