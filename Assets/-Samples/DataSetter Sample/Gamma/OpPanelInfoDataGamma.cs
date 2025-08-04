using Cysharp.Text;
using Doozy.Runtime.UIManager.Components;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Korens.Autonomous.Gamma
{
    public class OpPanelInfoDataGamma : OpPanelInfoDataBase
    {
        private void Start()
        {
            dataFormat = itemdata.ProcessUnit == string.Empty ? itemdata.ProcessFormat : ZString.Concat(itemdata.ProcessFormat, " ", itemdata.ProcessUnit);
        }

        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] TextMeshProUGUI specText;
        [SerializeField] TextMeshProUGUI valueText;

        [SerializeField] Image valueInputBox;
        [SerializeField] Color[] clrInputBox;
        [SerializeField] GammaOpItem itemdata;


        public override void SetItemData(OpItem item, string category)
        {
            itemdata = item as GammaOpItem;

            if (itemdata.isBindingItem)
            {
                //AddBind(OpItemDataContainer.instance.Category, item.TaskID);
                SetCategory(category, itemdata.TaskID);
            }

            valueInputBox.gameObject.SetActive(itemdata.isBindingItem);

            SetDescription("dummy description");
            SetSpecData(itemdata);

            if (itemdata.HasDashboard)
            {
                SetDashboardButton(itemdata.DashboardCategory);
            }
        }

        public override void SetValue(string value)
        {
            string processedValue = "-";

            if (value != null)
            {
                if (itemdata.ProcessFormat.Equals("1:양품,2:불량"))
                {
                    //SetBoxColor(value);
                    processedValue = value == "1.0" ? "양품" : "불량";
                }
                else
                if (float.TryParse(value, out var fValue))
                {
                    //SetBoxColor(fValue);
                    processedValue = string.Format(dataFormat, fValue);
                }
            }

            dataValue = processedValue;
            valueText.text = processedValue;
        }

        void SetDescription(string text)
        {
            descriptionText.text = text;
        }

        public void SetSpecData(OpItem item)
        {
            specText.gameObject.SetActive(item.HasSPEC);

            if (item.HasSPEC)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SPEC ");
                sb.Append(item.MinSPECValue);
                sb.Append("~");
                sb.Append(item.MaxSPECValue);
                sb.Append(item.ProcessUnit);

                specText.text = sb.ToString();
            }
        }

        [SerializeField] private UIButton[] dashboardButtons;
        void SetDashboardButton(DashboardCategory dashboard)
        {
            dashboardButtons[(int)dashboard - 1].gameObject.SetActive(true);

            switch (dashboard)
            {
                case DashboardCategory.TimeGraph:
                    //SetHourChartEvent();
                    break;

                case DashboardCategory.SPCGraph:
                    //SetSPCViewerEvent();
                    break;

                case DashboardCategory.BarcodeChart:
                    //SetBarcodeViewerEvent();
                    break;
            }
        }

        void SetBoxColor(string value)
        {
            if (value.Equals("1.0")) // ???
            {
                valueInputBox.color = clrInputBox[1];
            }
            else
            if (value.Equals("2.0")) // ???
            {
                valueInputBox.color = clrInputBox[2];
            }
            else
            {
                valueInputBox.color = clrInputBox[0];
            }
        }

        void SetBoxColor(float value)
        {
            if (itemdata.BoxType.Equals(0))
            {
                if (value.Equals(0.0f))
                {
                    valueInputBox.color = clrInputBox[3];
                }
                else
                {
                    if (IsDataNG(value))
                    {
                        valueInputBox.color = clrInputBox[2];
                    }
                    else
                    {
                        valueInputBox.color = clrInputBox[1];
                    }
                }
            }
            else
            {
                valueInputBox.color = clrInputBox[0];
            }
        }

        bool IsDataNG(float value)
        {
            return value < itemdata.MinSPECValue || value > itemdata.MaxSPECValue;
        }
    }
}