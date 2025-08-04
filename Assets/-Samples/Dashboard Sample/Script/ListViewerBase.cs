using Doozy.Runtime.Common;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample.Dashboard
{
    public enum OrderBy
    {
        ASC,
        DESC
    }

    public abstract class ListViewerBase<T>
    {
        protected InfiniteScroll scrollRect;
        protected int maxCount;
        protected OrderBy orderBy;

        protected ListViewerBase(InfiniteScroll scrollRect, int maxCount)
        {
            this.scrollRect = scrollRect;
            this.maxCount = maxCount;
        }

        public abstract void PrintData(List<T> data);

        protected void LoadDataOrderByASC(List<T> data)
        {
            ClearData();

            OrderByASC(data);
        }

        protected void LoadDataOrderByDESC(List<T> data)
        {
            ClearData();

            OrderByDESC(data);
        }

        public void ClearData()
        {
            scrollRect.Clear();
        }

        private void OrderByDESC(List<T> list)
        {
            var dataList = new List<T>(list);

            if (dataList.Count > 0)
            {
                if (dataList.Count <= maxCount)
                {
                    for (int i = dataList.Count - 1; i >= 0; i--)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = dataList.Count - i - 1;
                        data.data = dataList[i];

                        scrollRect.InsertData(data);
                    }
                    for (int i = dataList.Count; i < maxCount; i++)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = i;

                        scrollRect.InsertData(data);
                    }
                }
                else
                {
                    for (int i = dataList.Count - 1; i >= 0; i--)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = dataList.Count - i - 1;
                        data.data = dataList[i];

                        scrollRect.InsertData(data);
                    }
                }
            }
            else
            {
                for (int i = 0; i < maxCount; i++)
                {
                    ListDataElement<T> data = new ListDataElement<T>();
                    data.index = i;

                    scrollRect.InsertData(data);
                }
            }

            scrollRect.UpdateAllData();
        }
        
        private void OrderByASC(List<T> list)
        {
            var dataList = new List<T>(list);

            if (dataList.Count > 0)
            {
                if (dataList.Count <= maxCount)
                {
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = i;
                        data.data = dataList[i];

                        scrollRect.InsertData(data);
                    }
                    for (int i = dataList.Count - 1; i < maxCount; i++)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = i;

                        scrollRect.InsertData(data);
                    }
                }
                else
                {
                    for (int i = 0; i <dataList.Count; i++)
                    {
                        ListDataElement<T> data = new ListDataElement<T>();
                        data.index = i;
                        data.data = dataList[i];

                        scrollRect.InsertData(data);
                    }
                }
            }
            else
            {
                for (int i = 0; i < maxCount; i++)
                {
                    ListDataElement<T> data = new ListDataElement<T>();
                    data.index = i;

                    scrollRect.InsertData(data);
                }
            }

            scrollRect.UpdateAllData();
        }
    }
}
