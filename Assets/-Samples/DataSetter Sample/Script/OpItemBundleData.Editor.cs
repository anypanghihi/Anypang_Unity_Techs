#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Sample.OPDataSetter
{
    [CustomEditor(typeof(OpItemBundleData))]
    public class OpItemBundleDataEditor : Editor
    {
        OpItemBundleData container = null;


        public override void OnInspectorGUI()
        {
            container = target as OpItemBundleData;


            base.OnInspectorGUI();

            serializedObject.Update();



            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(20);
            if (GUILayout.Button("Create OpItem Data", GUILayout.Height(30)))
            {
                Create();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Remove OpItem Data", GUILayout.Height(25)))
            {
                ClearAll();
            }

            GUILayout.Space(10);
        }

        public void Create()
        {
            ClearAll();
            container.CreateItemByEditor();
        }

        void ClearAll()
        {
            container.Itemlist.Clear();

            for (int i = container.TM.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(container.TM.GetChild(i).gameObject);
        }



    }
}

#endif