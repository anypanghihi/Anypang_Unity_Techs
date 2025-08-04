using Gpm.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample.Dashboard
{
    [System.Serializable]
    public class SomeDataClass
    {
        public SomeDataClass(string data1, string data2, string data3) 
        {
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;
        }

        public string data1;
        public string data2;
        public string data3;
    }

    public class SampleListViewer : ListViewerBase<SomeDataClass>
    {
        public SampleListViewer(InfiniteScroll scrollRect, int maxCount, OrderBy orderby = OrderBy.DESC) : base(scrollRect, maxCount)
        {
            this.orderBy = orderby;

            if (orderBy.Equals(OrderBy.ASC))
            {
                printAction = LoadDataOrderByASC;
            }
            else
            {
                printAction = LoadDataOrderByDESC;
            }
        }

        private Action<List<SomeDataClass>> printAction;
        public override void PrintData(List<SomeDataClass> data)
        {
            printAction?.Invoke(data);
        }
    }
}
