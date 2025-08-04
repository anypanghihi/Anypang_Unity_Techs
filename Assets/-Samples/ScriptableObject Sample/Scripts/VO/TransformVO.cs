#if UNITY_EDITOR
using UnityEditor;
#endif
using Structs;
using UnityEngine;
using ScriptableObjects;

public class TransformVO : MonoBehaviour  // ISerializationCallbackReceiver
{
    [SerializeField] private TransformSO DataSO;
    
    public TransformSO Data { get { return DataSO; } }


    void OnEnable()
    {
        SetValue();
    }


    private void OnDisable()
    {
    }


    public void SetValue()
    {
        transform.position   = DataSO.Value.position;
        transform.rotation   = DataSO.Value.rotation;
        transform.localScale = DataSO.Value.scale;
    }

}


//------------------------------------------------------------------------------------------------------


#if UNITY_EDITOR

[CustomEditor(typeof(TransformVO))]
public class TransformVOEditor : Editor
{
    TransformVO vo;

    private void OnEnable()
    {
        vo = target as TransformVO;

        vo.SetValue();
    }

    private void OnDisable()
    {
        vo.Data.Save(vo.transform);
    }
}
#endif