#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using ScriptableObjects;

public class LineOverViewVO : MonoBehaviour  // ISerializationCallbackReceiver
{
    [SerializeField] private LineOverViewSO DataSO;
    
    public LineOverViewSO Data { get { return DataSO; } }


    void OnEnable()
    {
        SetValue();
    }


    private void OnDisable()
    {
    }


    public void SetValue()
    {
        transform.position = DataSO.position;
        transform.rotation = DataSO.rotation;
        transform.localScale = DataSO.scale;
    }

}


//------------------------------------------------------------------------------------------------------


#if UNITY_EDITOR

[CustomEditor(typeof(LineOverViewVO))]
public class LineOverViewVOEditor : Editor
{
    LineOverViewVO vo;

    private void OnEnable()
    {
        vo = target as LineOverViewVO;

        vo.SetValue();
    }

    private void OnDisable()
    {
        vo.Data.Save(vo.transform);
    }
}
#endif