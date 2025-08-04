#if UNITY_EDITOR
using UnityEditor;
#endif
using Structs;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PRSData", menuName = "SO/Transform/PRS", order = 0)]
    public class TransformSO : ScriptableObject, IAssetSO
    {
        [SerializeField] private PRS data;
        public PRS Value => data; 

        public void Save<T>(T src)
        {
            Transform tm = src as Transform;
             
            data.position = tm.position;
            data.rotation = tm.rotation;
            data.scale = tm.localScale;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
#endif
        }

    }  
}