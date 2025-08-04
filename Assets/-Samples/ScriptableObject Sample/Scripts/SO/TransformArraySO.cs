#if UNITY_EDITOR
using UnityEditor;
#endif
using Structs;
using UnityEngine;
using System.Collections.Generic;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PRSDatas", menuName = "SO/Transform/PRSArray", order = 0)]
    public class TransformArraySO : ScriptableObject, IAssetSO
    {
        [SerializeField] private List<PRS> data = new List<PRS>();
        private int index = 0;

        public PRS Value => data[index];
        public int Length => data.Count;
        public int Index => index;

        public void AddData()
        {
            data.Add(new PRS());
        }

        public void RemoveData()
        {
            if (data.Count > 0)
            {
                data.RemoveAt(data.Count - 1);
            }
        }

        public void SetIndexBinder(int idx)
        {
            if (idx >= 0)
            {
                this.index = idx;
            }
        }

        public void Save<T>(T src)
        {
            Transform tm = src as Transform;

            if (data.Count > 0 && index < data.Count)
            {
                PRS upd = data[index];

                upd.position = tm.position;
                upd.rotation = tm.rotation;
                upd.scale = tm.localScale;

                data[index] = upd;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
#endif
        }
    }

}