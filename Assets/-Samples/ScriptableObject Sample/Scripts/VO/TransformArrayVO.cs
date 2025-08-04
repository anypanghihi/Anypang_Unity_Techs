#if UNITY_EDITOR
using UnityEditor;
#endif
using Structs;
using UnityEngine;
using ScriptableObjects;
using System;

public class TransformArrayVO : MonoBehaviour  // ISerializationCallbackReceiver
{
    [SerializeField] private TransformArraySO DataSO;
    
    public TransformArraySO Data { get { return DataSO; } }


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

    public int  ArrayLength()           { return DataSO.Length;         }
    public void AddArrayLast()          { DataSO.AddData();             }
    public void RmvArrayLast()          { DataSO.RemoveData();          }
    public void SetIndexBinder(int idx) { DataSO.SetIndexBinder(idx);   }
    public int  SelectedIndex()         { return DataSO.Index;          }

}


//------------------------------------------------------------------------------------------------------


#if UNITY_EDITOR

[CustomEditor(typeof(TransformArrayVO))]
public class TransformVOArrayEditor : Editor
{
    TransformArrayVO vo;
    
    private void OnEnable()
    {
        vo = target as TransformArrayVO;        
    }

    private void OnDisable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIStyle myStyle = new GUIStyle(GUI.skin.box);
        myStyle.normal.textColor = Color.cyan;
        myStyle.fontSize = 14;

        GUILayout.Space(10);
        GUILayout.Box($"Array Length is {vo.ArrayLength()}",  myStyle);
        GUILayout.BeginHorizontal(EditorStyles.helpBox);        

        if (GUILayout.Button("Add Array Data", GUILayout.Height(30)))
        {
            vo.AddArrayLast();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Remove Array Data", GUILayout.Height(30)))
        {
            vo.RmvArrayLast();
        } 
        
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        myStyle.normal.textColor = Color.yellow;
        myStyle.fontSize = 14;

        GUILayout.Box($"Select Array Index is {vo.SelectedIndex()}", myStyle);
        GUILayout.BeginHorizontal(EditorStyles.helpBox);

        if (GUILayout.Button("<<", GUILayout.Height(30)))
        {
            int id = vo.SelectedIndex();

            if( --id < 0 )                { id = 0; }
            if(   id >= vo.ArrayLength()) { id = vo.ArrayLength() - 1; }

            vo.SetIndexBinder(id);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            vo.Data.Save(vo.transform);
        }

        GUILayout.Space(10);

        if (GUILayout.Button(">>", GUILayout.Height(30)))
        {
            int id = vo.SelectedIndex();

            if (++id >= vo.ArrayLength()) { id = vo.ArrayLength() - 1; }

            vo.SetIndexBinder(id);
        }        

        GUILayout.EndHorizontal();

        GUILayout.Space(3);

        if (GUILayout.Button("Apply Select Index Value", GUILayout.Height(30)))
        {
            vo.SetValue();
        }
    }
}
#endif