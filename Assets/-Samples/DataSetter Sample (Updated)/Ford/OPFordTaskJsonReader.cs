using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Korens.Autonomous.Ford
{
    [Serializable]
    public class FordOpItem : OpItem
    {
        // FORD 라인에서 해당 데이터가 그룹 데이터인지 체크하는 용도
        public bool isGroupTask;

        // Group Task일 경우 사용할 ID
        public string GroupTaskID;
    }

    [System.Serializable]
    public class OPTask
    {
        public int Id;
        public OPTaskProcess[] Process;
    }

    [System.Serializable]
    public class OPTaskProcess
    {
        public string Name;
        public string[] Tasks;
        public string[] Min;
        public string[] Max;
        public string[] SPC;
    }

    public class OPTaskUnit
    {
        public string Task;
        public string Min;
        public string Max;
        public string SPC;

        public OPTaskUnit(string task, string min, string max, string spc)
        {
            Task = task;
            Min = min;
            Max = max;
            SPC = spc;
        }
    }

    public class OPFordTaskJsonReader : MonoBehaviour
    {
        List<OPTask> opTask;
        public Dictionary<int, Dictionary<string, List<OPTaskUnit>>> opTaskDict;

        public void Init()
        {
            opTask = new List<OPTask>();
            opTaskDict = new Dictionary<int, Dictionary<string, List<OPTaskUnit>>>();

            ParseTaskData();

            DebugOPTaskDict(opTaskDict);
        }

        Dictionary<string, List<OPTaskUnit>> opTaskProcessDict;
        private void ParseTaskData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "KRS_FORD_TASK_DATA_JSON.json");

            var taskData = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<JObject>(taskData);
            opTask = JsonConvert.DeserializeObject<List<OPTask>>(data["processNames"].ToString());

            foreach (OPTask op in opTask)
            {
                opTaskProcessDict = new Dictionary<string, List<OPTaskUnit>>();

                foreach (OPTaskProcess process in op.Process)
                {
                    List<OPTaskUnit> taskList = new List<OPTaskUnit>();

                    for (int i = 0; i < process.Tasks.Length; i++)
                    {
                        OPTaskUnit task = new OPTaskUnit
                       (
                            process.Tasks[i],
                            process.Min[i],
                            process.Max[i],
                            process.SPC[i]
                        );

                        taskList.Add(task);
                    }

                    string key = string.Empty;
                    if (taskList.Count == 1 && process.Name == string.Empty)
                    {
                        key = taskList[0].Task;
                    }
                    else
                    {
                        key = process.Name;
                    }

                    opTaskProcessDict.Add(key, taskList);
                }

                opTaskDict.Add(op.Id, opTaskProcessDict);
            }
        }

        public void DebugOPTaskDict(Dictionary<int, Dictionary<string, List<OPTaskUnit>>> opTaskDict)
        {
            foreach (var idEntry in opTaskDict)
            {
                Debug.Log($"Task ID: {idEntry.Key}");
                foreach (var processEntry in idEntry.Value)
                {
                    Debug.Log($"  Process Name: {processEntry.Key}");
                    foreach (var taskUnit in processEntry.Value)
                    {
                        Debug.Log($"    Task: {taskUnit.Task}, Min: {taskUnit.Min}, Max: {taskUnit.Max}, SPC: {taskUnit.SPC}");
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OPFordTaskJsonReader))]
    public class OPFordTaskJsonReaderEditor : Editor
    {
        OPFordTaskJsonReader container = null;

        private void OnEnable()
        {
            container = target as OPFordTaskJsonReader;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle myStyle = new GUIStyle(GUI.skin.box);
            myStyle.normal.textColor = Color.yellow;

            GUILayout.Space(10);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Box("Edit - OpItem Data", myStyle);

            if (GUILayout.Button("Create OpItemBundle", GUILayout.Height(30)))
            {
                container.Init();

                foreach (var ID in container.opTaskDict.Keys)
                {
                    OpItemBundle asset = ScriptableObject.CreateInstance<OpItemBundle>();
                    asset.line = Line.Ford;
                    asset.id = ID;

                    // Set AssetData
                    foreach (var pair in container.opTaskDict[ID])
                    {
                        SetAssetData(asset, pair.Key, container.opTaskDict[ID][pair.Key]);
                    }

                    string fileName = ZString.Concat("OP", asset.id, ".asset");
                    string path = Path.Combine("Assets/Resources/Ford", fileName);
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();

                    Debug.Log($"Saved ScriptableObject at {path}");
                }
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        public static void SetAssetData(OpItemBundle asset, string groupTaskID, List<OPTaskUnit> taskUnitList)
        {
            bool isGroupTask = taskUnitList.Count > 1;
            foreach (var each in taskUnitList)
            {
                FordOpItem item = new FordOpItem();

                item.isGroupTask = isGroupTask;

                if (item.isGroupTask)
                {
                    item.GroupTaskID = groupTaskID;
                    item.TaskID = ZString.Concat(asset.id, "_", groupTaskID, "_", each.Task);
                }
                else
                {
                    item.TaskID = ZString.Concat(asset.id, "_", each.Task);
                }

                item.OPID = asset.id;
                item.ProcessName = each.Task;

                if (float.TryParse(each.Min, out float fMin))
                {
                    item.MinSPECValue = fMin;
                }

                if (float.TryParse(each.Max, out float fMax))
                {
                    item.MaxSPECValue = fMax;
                }

                item.SPCLink = each.SPC;

                asset.AddItem(item);
            }
        }
    }
#endif
}