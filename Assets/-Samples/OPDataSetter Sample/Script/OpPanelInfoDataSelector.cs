using UnityEngine;
using UnityEngine.Events;

namespace Sample.OPDataSetter
{
    public class OpPanelInfoDataSelector : MonoBehaviour
    {
        [SerializeField] OpPanelInfoData[] panelDatas;

        OpPanelInfoData currentInfoData = null;
        public void SetItemData(OpItem item, string category)
        {
            if (currentInfoData != null)
            {
                currentInfoData.RemoveBind();
            }

            foreach (var box in panelDatas)
            {
                box.gameObject.SetActive(false);
            }

            currentInfoData = panelDatas[item.BoxType];

            currentInfoData.SetItemData(item, category);
        }

        void SetHourChartEvent(UnityAction<string> call, string key)
        {
            currentInfoData.SetHourChartEvent(call, key);
        }

        void SetSPCViewerEvent(UnityAction<string> call, string key)
        {
            currentInfoData.SetSPCViewerEvent(call, key);
        }

        void SetBarcodeViewerEvent(UnityAction<string> call, string key)
        {
            currentInfoData.SetBarcodeViewerEvent(call, key);
        }
    }
}