using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Korens.Autonomous.Gamma
{
    [System.Serializable]
    public enum DashboardCategory
    {
        None,
        TimeGraph,
        SPCGraph,
        BarcodeChart,
        Count = 3
    }

    [Serializable]
    public class GammaOpItem : OpItem
    {
        /// 해당 공정이 표시될 오브젝트의 타입.
        /// 0 : Spec 있음 (Type 1), 1 : Spec 없음 (Type 2), 2 : Value 없음, Spec 있음 (Type 3)
        public int BoxType;


        /// 해당 공정의 대시보드 종류.
        public DashboardCategory DashboardCategory;
    }

    [System.Serializable]
    public class OPTask
    {
        public int Id;
        public int Count;
        public int[] BoxTypes;
        public string[] Tasks;
        public string[] Descriptions;
        public string[] Types;
        public float[] Scales;
        public string[] Specs;
    }

    public class OPTaskUnit
    {
        public int BoxType;
        public string Task;
        public string Description;
        public string Type;
        public float Scale;
        public string Spec;

        public OPTaskUnit(int boxType, string task, string description, string type, float scale, string spec)
        {
            BoxType = boxType;
            Task = task;
            Description = description;
            Type = type;
            Scale = scale;
            Spec = spec;
        }
    }

    [System.Serializable]
    public class TagFile
    {
        public string id;
        public string param;
        public int opId;
    }

    public class OPGammaTaskJsonReader : MonoBehaviour
    {
        List<OPTask> opTask;

        public Dictionary<int, List<OPTaskUnit>> opTaskDict;
        public Dictionary<string, string> SPCTags;

        public void Init()
        {
            opTask = new List<OPTask>();
            opTaskDict = new Dictionary<int, List<OPTaskUnit>>();
            SPCTags = new Dictionary<string, string>();

            ParseTaskData();
            ParseSPCLinkData();

            //DebugOPTaskDict(opTaskDict);
            //Debug.Log(SPCTags.Count);
        }

        private void ParseTaskData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "KRS_TASK_DATA_JSON.json");

            var taskData = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<JObject>(taskData);
            opTask = JsonConvert.DeserializeObject<List<OPTask>>(data["processNames"].ToString());

            foreach (OPTask opTask in opTask)
            {
                List<OPTaskUnit> taskList = new List<OPTaskUnit>();

                for (int i = 0; i < opTask.Count; i++)
                {
                    OPTaskUnit task = new OPTaskUnit
                   (
                        opTask.BoxTypes[i],
                        opTask.Tasks[i],
                        opTask.Descriptions[i],
                        opTask.Types[i],
                        opTask.Scales[i],
                        opTask.Specs[i]
                    );

                    taskList.Add(task);
                }

                opTaskDict.Add(opTask.Id, taskList);
            }
        }

        private void ParseSPCLinkData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "KRS_SPC_LINK.json");

            var taskData = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<JObject>(taskData);
            var dataList = JsonConvert.DeserializeObject<List<TagFile>>(data["Gamma"].ToString());

            foreach (var each in dataList)
            {
                SPCTags.Add(each.id, each.param);
            }
        }

        public void DebugOPTaskDict(Dictionary<int, List<OPTaskUnit>> opTaskDict)
        {
            foreach (var idEntry in opTaskDict)
            {
                Debug.Log($"Task ID: {idEntry.Key}");
                foreach (var taskUnit in idEntry.Value)
                {
                    Debug.Log($"  BoxType: {taskUnit.BoxType}, Task: {taskUnit.Task}, Description: {taskUnit.Description}, Type: {taskUnit.Type}, Scale: {taskUnit.Scale}, Spec: {taskUnit.Spec}");
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OPGammaTaskJsonReader))]
    public class OPGammaTaskJsonReaderEditor : Editor
    {
        OPGammaTaskJsonReader container = null;

        private void OnEnable()
        {
            container = target as OPGammaTaskJsonReader;
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
                    asset.line = Line.Gamma;
                    asset.id = ID;

                    foreach (var each in container.opTaskDict[ID])
                    {
                        SetAssetData(asset, each, container.SPCTags);
                    }


                    string fileName = ZString.Concat("OP", asset.id, ".asset");
                    string path = Path.Combine("Assets/Resources/Gamma", fileName);
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

        public static void SetAssetData(OpItemBundle asset, OPTaskUnit taskUnit, Dictionary<string, string> spcDict)
        {
            GammaOpItem item = new GammaOpItem();

            item.TaskID = taskUnit.Task;
            item.OPID = asset.id;
            item.HasDashboard = !(taskUnit.BoxType == 0 || taskUnit.BoxType == 3);
            if (item.HasDashboard)
            {
                if (taskUnit.BoxType == 2 || taskUnit.BoxType == 5)
                {
                    item.DashboardCategory = DashboardCategory.TimeGraph;
                }
                else
                if (taskUnit.BoxType == 1 || taskUnit.BoxType == 4 || taskUnit.BoxType == 6)
                {
                    item.DashboardCategory = DashboardCategory.SPCGraph;

                    if (spcDict.ContainsKey(item.TaskID))
                    {
                        item.SPCLink = spcDict[item.TaskID];
                    }
                }
                else
                if (taskUnit.BoxType == 7)
                {
                    item.DashboardCategory = DashboardCategory.BarcodeChart;
                }
            }
            item.ProcessName = taskUnit.Description;

            if (taskUnit.Type.Contains(" "))
            {
                string[] splits = taskUnit.Type.Split(" ");

                item.ProcessFormat = splits[0];
                item.ProcessUnit = splits[1];
            }
            else
            {
                item.ProcessFormat = taskUnit.Type;
            }

            item.ProcessScale = taskUnit.Scale;

            item.HasSPEC = taskUnit.Spec != string.Empty;
            if (item.HasSPEC)
            {
                string[] specs = taskUnit.Spec.Split(" ");
                string[] minmax = specs[1].Split("~");
                item.MinSPECValue = float.Parse(minmax[0]);
                item.MaxSPECValue = float.Parse(minmax[1]);
            }

            item.isBindingItem = taskUnit.Type != "";

            asset.AddItem(item);
        }
    }
#endif
}