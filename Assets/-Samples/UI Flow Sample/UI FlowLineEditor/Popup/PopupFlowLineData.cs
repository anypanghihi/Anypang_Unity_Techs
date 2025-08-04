using Gpm.Ui;
using System;

namespace FlowLineUIEditor.LineList
{
    [Serializable]
    public class PopupFlowLineData : InfiniteScrollData
    {
        // 필요한 Data 여기에 추가

        public int    sId;
        public bool   isOn;
        public string areaId;  //EnumAreaSO 의 AreaId        
        public string areaText;
        
        
        public void SetData(int sId, bool isOn, string areaText, string areaId = "")
        {
            this.sId = sId;
            this.isOn = isOn;
            this.areaId = areaId;
            this.areaText = areaText;
        }

    }
} 