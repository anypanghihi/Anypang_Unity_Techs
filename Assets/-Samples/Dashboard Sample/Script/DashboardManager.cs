using Doozy.Runtime.UIManager.Containers;
using E2C;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

namespace Sample.Dashboard
{
    public partial class DashboardManager : MonoBehaviour
    {
        public static DashboardManager instance;

        private void Awake()
        {
            instance = this;

            AwakeSignal();
        }

        private void Start()
        {
            // Â÷Æ® ºä¾î ¼¼ÆÃ.
            timeGraphDataDict.Add("ID-0001", new List<float>() { 17, 23, 25, 46, 102, 98, 80, 21, 0, 66, 45, 37 });
            sampleChartController = new SampleChartController(sampleChart);
            sampleChartController.AddCategory();

            // À¥ ºä¾î ¼¼ÆÃ.
            spcLinkDict.Add("ID-0002", "www.daum.net");

            // ¸®½ºÆ® ºä¾î ¼¼ÆÃ.
            barcodeDataDict.Add("ID-0003", new List<SomeDataClass>()
            {
                new SomeDataClass("data1", "data2", "data3"),
                new SomeDataClass("data4", "data5", "data6"),
                new SomeDataClass("data7", "data8", "data9")
            });
            sampleListViewer = new SampleListViewer(scrollRect, maxCount, orderBy);
        }

        private void OnEnable()
        {
            OnEnableSignal();
        }

        private void OnDisable() 
        {
            OnDisableSignal();
        }

        UIContainer currentViewer;
        void ShowViewer(UIContainer container)
        {
            if (currentViewer != null)
            {
                if (currentViewer == container)
                {
                    return;
                }
                else
                {
                    HideViewer();
                }
            }

            currentViewer = container;
            currentViewer.InstantShow();
        }

        public void HideViewer()
        {
            currentViewer.InstantHide();
            currentViewer = null;
        }

        #region "½Ã°£´ç Â÷Æ® ºä¾î"

        [SerializeField] private UIContainer timeGraphViewer;
        [SerializeField] private E2Chart sampleChart;
        private Dictionary<string, List<float>> timeGraphDataDict = new Dictionary<string, List<float>>();
        private SampleChartController sampleChartController;

        public void ShowTimeGraphViewer(string taskID)
        {
            ShowViewer(timeGraphViewer);
            SendTimeGraphSignal(taskID);
        }

        void SetTimeGraphViewer(string taskID)
        {
            if (timeGraphDataDict.TryGetValue(taskID, out List<float> dataList))
            {
                //Debug.Log("Time Graph Viewer " + value);

                sampleChartController.AddData(dataList);
                sampleChartController.UpdateChart();
            }
        }

        #endregion

        #region "SPC ºä¾î"

        [SerializeField] private UIContainer SPCViewer;
        [SerializeField] private CanvasWebViewPrefab webViewPrefab;
        private Dictionary<string, string> spcLinkDict = new Dictionary<string, string>();

        public void ShowSPCViewer(string taskID)
        {
            ShowViewer(SPCViewer);
            SendSPCSignal(taskID);
        }

        void SetSPCViewer(string taskID)
        {
            if (spcLinkDict.TryGetValue(taskID, out string spcLink))
            {
                //Debug.Log("SPC Viewer " + value);

                webViewPrefab.WebView.LoadUrl(spcLink);
            }
        }

        #endregion

        #region "¹ÙÄÚµå ºä¾î"

        [SerializeField] private UIContainer barcodeViewer;
        [SerializeField] private InfiniteScroll scrollRect;
        [SerializeField] private int maxCount;
        [SerializeField] private OrderBy orderBy = OrderBy.DESC;
        private Dictionary<string, List<SomeDataClass>> barcodeDataDict = new Dictionary<string, List<SomeDataClass>>();
        private SampleListViewer sampleListViewer;

        public void ShowBarcodeViewer(string taskID)
        {
            ShowViewer(barcodeViewer);
            SendBarcodeSignal(taskID);
        }

        void SetBarcodeViewer(string taskID)
        {
            if (barcodeDataDict.TryGetValue(taskID, out List<SomeDataClass> dataList))
            {
                //Debug.Log("BarcodeViewer " + value);

                //sampleListViewer.LoadListDataDESC(dataList);
                sampleListViewer.PrintData(dataList);
            } 
        }

        #endregion
    }
}
