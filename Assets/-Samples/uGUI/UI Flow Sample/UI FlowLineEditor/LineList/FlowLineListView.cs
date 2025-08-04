using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;


namespace FlowLineUIEditor.LineList.View
{
    public class FlowLineListView : MonoBehaviour
    {
        [SerializeField] private InfiniteScroll scrollView;

        private List<FlowLineData> dataList = new List<FlowLineData>(10);


        private void Start()
        {
            scrollView.layout.padding.y = 4f;
            scrollView.layout.space.y = 8.8f;
        }

        private void OnDestroy()
        {
        }


        public void ClearScrollView()
        {
            scrollView.Clear();
            dataList.Clear();
        }

        // To. Rasan
        public void AddScrollItem()
        {
            FlowLineData data = new FlowLineData();
            data.viewType = ScrollViewType.Item;

            data.sId = dataList.Count + 1;
            dataList.Add(data);

            scrollView.InsertData(data, dataList.Count - 1);
        }

        public void RemoveScrollItem(int index)
        {
            if (dataList.Count <= 0) return;

            FlowLineData data = FindDataItem(index);

            if (data != null)
            {
                scrollView.RemoveData(data);
                dataList.Remove(data);

                // no(sId) 재갱신 처리
                int i = 0;
                foreach (var each in dataList)
                {
                    each.sId = ++i;
                }

                scrollView.UpdateAllData();
            }
        }

        public FlowLineData FindDataItem(int index)
        {
            return dataList.Find(x => x.sId.Equals(index));
        }

        public void SetScrollItemData(int index, FlowLineData data)
        {
            FlowLineData found = FindDataItem(index);

            if (found != null)
            {
                //found.SetData()
            }
        }

        public void DefaultCreateItem()
        {
            FlowLineData data = new FlowLineData();
            data.viewType = ScrollViewType.Create;
            data.sId = -1;

            scrollView.InsertData(data);
        }




    }
}