using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sample.Dashboard
{
    public class ListDataElement<T> : InfiniteScrollData
    {
        public int index = 0;
        public T data;
    }

    public abstract class ListDataBase<T> : InfiniteScrollItem
    {
        [SerializeField] protected TextMeshProUGUI[] datas;
        [SerializeField] protected Image background;
        [SerializeField] private Color32[] backgroundColors;

        /// <summary>
        /// 스크롤 될 때 자동으로 호출되는 함수.
        /// </summary>
        public override void UpdateData(InfiniteScrollData scrollData)
        {
            base.UpdateData(scrollData);

            ListDataElement<T> itemData = (ListDataElement<T>)scrollData;

            var data = itemData.data;

            background.color = backgroundColors[itemData.index & 1];

            if (data == null)
            {
                // 빈 박스
                ClearData();

                return;
            }

            SetData(data);
        }

        private void ClearData()
        {
            foreach (var data in datas)
            {
                data.text = string.Empty;
            }
        }

        /// <summary>
        /// 데이터를 text에 넣는다. <br/>
        /// ex) <br/>
        /// datas[0].text = data.data1; <br/>
        /// datas[1].text = data.data2; <br/>
        /// datas[2].text = data.data3; <br/>
        /// </summary>
        protected abstract void SetData(T data);
    }
}
