using Cysharp.Text;
using Doozy.Runtime.Bindy;
using Doozy.Runtime.UIManager.Components;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sample.OPDataSetter
{
    public class OpPanelInfoData : MonoBehaviour
    {
        string dataFormat = string.Empty;
        string dataValue = string.Empty;
        public string Value
        {
            set
            {
                if (value != null)
                {
                    if (value.Equals("-"))
                    {
                        dataValue = value;
                    }
                    else
                    if (itemdata.ProcessFormat.Equals("1:양품,2:불량"))
                    {
                        dataValue = value == "1.0" ? "양품" : "불량";

                        SetBoxColor(value);
                    }
                    else
                    {
                        //float temp = float.Parse(dataValue);
                        //temp *= item.ProcessScale;

                        dataValue = string.Format(dataFormat, value);

                        if (float.TryParse(value, out var fValue))
                        {
                            SetBoxColor(fValue);
                        }
                    }
                }
                else
                {
                    dataValue = "null";
                }

                // 여기에서 값이 들어온것을 알기때문에. 관련된 정보를 갱신한다.
                texts[2].text = dataValue;
            }
            get
            {
                return dataValue;
            }
        }

        private void Start()
        {
            dataFormat = itemdata.ProcessUnit == string.Empty ? itemdata.ProcessFormat : ZString.Concat(itemdata.ProcessFormat, " ", itemdata.ProcessUnit);
        }

        /// <summary>
        /// 0 : Description, 1 : SPEC, 2 : Value
        /// </summary>
        [SerializeField] TextMeshProUGUI[] texts;
        [SerializeField] TextMeshProUGUI timePopTitle;
        [SerializeField] TextMeshProUGUI spcPopTitle;
        [SerializeField] TextMeshProUGUI barcodPopTitle;

        [SerializeField] Image valuInputBox;
        [SerializeField] Color[] clrInputBox;
        [SerializeField] OpItem itemdata;


        public void SetItemData(OpItem item, string category)
        {
            itemdata = item;

            if (item.BoxType < 2)
            {
                //AddBind(OpItemDataContainer.instance.Category, item.TaskID);
                AddBind(category, item.TaskID);
            }

            gameObject.SetActive(true);
            SetDescription(item.ProcessName);

            SetSpecData(item);

            SetDashboardButton(item.DashboardCategory);
        }

        void SetDescription(string text)
        {
            texts[0].text = text;
        }

        public void SetSpecData(OpItem item)
        {
            if (item.HasSPEC)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SPEC ");
                sb.Append(item.MinSPECValue);
                sb.Append("~");
                sb.Append(item.MaxSPECValue);
                sb.Append(item.ProcessUnit);

                texts[1].text = sb.ToString();
            }
        }

        public void SetSpecText(string spec)
        {            
            texts[1].text = spec;
        }

        [SerializeField] private Binder binder;
        public void RemoveBind()
        {
            if (binder.bindables.Count > 0)
            {
                var receiver = binder.bindables.Find(x => x.connectionType.Equals(ConnectionType.Receiver));

                receiver?.SetOnBindBehavior(OnBindBehavior.DoNothing);
            }

            //binder.RemoveBind();
        }

        public void AddBind(string category, string key)
        {
            binder.bindId.Category = category;
            binder.bindId.Name = key;
            //binder.AddBind();

            if (binder.bindables.Count > 0)
            {
                var receiver = binder.bindables.Find(x => x.connectionType.Equals(ConnectionType.Receiver));

                receiver?.SetOnBindBehavior(OnBindBehavior.GetValue);
            }
        }

        [SerializeField] private UIButton[] dashboardButtons;
        UIButton showButton;
        void SetDashboardButton(DashboardCategory dashboard)
        {
            if (dashboard == DashboardCategory.None)
            {
                ClearDashboardButton();

                return;
            }

            showButton = dashboardButtons[(int)dashboard - 1];
            showButton.gameObject.SetActive(true);

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

        void ClearDashboardButton()
        {
            if (showButton != null)
            {
                showButton.gameObject.SetActive(false);
                showButton = null;
            }
        }

        public void SetHourChartEvent(UnityAction<string> call, string key)
        {
            if (showButton != null)
            {
                showButton.onClickEvent.AddListener(() =>
                {
                    call(key);

                    //SetTimeTitleText();
                });
            }
        }

        public void SetSPCViewerEvent(UnityAction<string> call, string key)
        {
            if (showButton != null)
            {
                showButton.onClickEvent.AddListener(() =>
                {
                    call(key);

                    //SetSPCTitleText();
                });
            }
        }

        public void SetBarcodeViewerEvent(UnityAction<string> call, string key)
        {
            if (showButton != null)
            {
                showButton.onClickEvent.AddListener(() =>
                {
                    call(key);

                    //SetBarcodeTitleText();
                });
            }
        }

        // binder로 대체
        void SetTimeTitleText()
        {
            if (timePopTitle != null)
                timePopTitle.text = texts[0].text;
        }

        // binder로 대체
        void SetSPCTitleText()
        {
            if (spcPopTitle != null)
            {
                var text = texts[0].text;
                if (text.Contains("\n"))
                {
                    text = text.Replace("\n", " ");
                }

                spcPopTitle.text = text;
            }
        }

        // binder로 대체
        void SetBarcodeTitleText()
        {
            if (barcodPopTitle != null)
            {
                barcodPopTitle.text = texts[0].text;
            }
        }

        // binder로 대체
        void SetBoxColor(string value)
        {
            if (value.Equals("1.0")) // 양품
            {
                valuInputBox.color = clrInputBox[1];
            }
            else
            if (value.Equals("2.0")) // 불량
            {
                valuInputBox.color = clrInputBox[2];
            }
            else
            {
                valuInputBox.color = clrInputBox[0];
            }
        }

        void SetBoxColor(float value)
        {
            if (itemdata.BoxType.Equals(0))
            {
                if (value.Equals(0.0f))
                {
                    valuInputBox.color = clrInputBox[3];
                }
                else
                {
                    if (IsDataNG(value))
                    {
                        valuInputBox.color = clrInputBox[2];
                    }
                    else
                    {
                        valuInputBox.color = clrInputBox[1];
                    }
                }
            }
            else
            {
                valuInputBox.color = clrInputBox[0];
            }
        }

        bool IsDataNG(float value)
        {
            return value < itemdata.MinSPECValue || value > itemdata.MaxSPECValue;
        }
    }
}