#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "OverViewData", menuName = "SO/Line/OverView", order = 0)]
    public class LineOverViewSO : ScriptableObject//, IAssetSO
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private Quaternion rot;
        [SerializeField] private Vector3 scl = Vector3.one;

        public Vector3 position { set { pos = value; } get { return pos; } }
        public Quaternion rotation { set { rot = value; } get { return rot; } }
        public Vector3 scale { set { scl = value; } get { return scl; } }


        public void Save<T>(T src)
        {
            Transform tm = src as Transform;



#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
#endif
        }

    }
}