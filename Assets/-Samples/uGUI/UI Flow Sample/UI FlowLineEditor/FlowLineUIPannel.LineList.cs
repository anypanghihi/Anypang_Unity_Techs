using FlowLineUIEditor.LineList;

namespace FlowLineUIEditor.Pannel
{
    public partial class FlowLineUIPannel // LineList
    {
        private void InitializeLineList()
        {
            // 이벤트 등록은 FlowLineUIPannel.Popup에서 통합 처리됨
        }

        private void UnInitializeLineList()
        {
            // 이벤트 해제는 FlowLineUIPannel.Popup에서 통합 처리됨
        }

        // To. Rasan
        public void SetupScrollView()
        {
            lineListView.DefaultCreateItem();
        }

        // To. Rasan
        public void ClearScrollView()
        {
            lineListView.ClearScrollView();
        }

        // To. Rasan
        public void RemoveScrollItem(int sId)
        {
            lineListView.RemoveScrollItem(sId);
        }

        // To. Rasan
        public void SelectScrollItem(int sId)
        {
            FlowLineData found = lineListView.FindDataItem(sId);

            selectLineView.SetFlowLineData(sId, found.area1Text, found.area2Text);

            ShowLineEditor();
        }

        public void DeSelectScrollItem()
        {
            ShowLineList();
        }

        public void RemoveScrollItemPopup(int sId) { ShowDeletePopup(sId); }
        public void UpdateScrollItemPopup(int mId) { ShowUpdatePopup(mId); }
    }
}