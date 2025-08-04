using Gpm.Ui;
using System;

namespace FlowLineUIEditor.LineList
{
    [Serializable]
    public class FlowLineData : InfiniteScrollData
    {
        public ScrollViewType viewType = ScrollViewType.Item;

        // 필요한 Data 여기에 추가

        public int sId;
        public string area1Id;  //EnumAreaSO 의 AreaId
        public string area2Id;  //EnumAreaSO 의 AreaId

        public string area1Text;
        public string area2Text;

        public void SetData(int sId, string area1Text, string area2Text, string area1Id = "", string area2Id = "")
        {
            this.sId = sId;
            this.area1Id = area1Id;
            this.area2Id = area2Id;

            this.area1Text = area1Text;
            this.area2Text = area2Text;
        }

        public void SetData(int sId, string area1Text, string area1Id = "")
        {
            this.sId = sId;
            this.area1Id = area1Id;
            this.area1Text = area1Text;
        }

        public void SetAreaText(int sId, string area1Text, string area2Text)
        {
            this.sId = sId;
            this.area1Text = area1Text;
            this.area2Text = area2Text;
        }

        public void SetIndex(int sId) { this.sId = sId; }
        public void SetArea1(string area1Text, string area1Id = "")
        {
            this.area1Id = area1Id;
            this.area1Text = area1Text;
        }
        public void SetArea2(string area2Text, string area2Id = "")
        {
            this.area2Id = area2Id;
            this.area2Text = area2Text;
        }
    }
}