using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Dashboard
{
    public class SampleListData : ListDataBase<SomeDataClass>, IPointerEnterHandler, IPointerExitHandler
    {
        protected override void SetData(SomeDataClass data)
        {
            datas[0].text = data.data1;
            datas[1].text = data.data2;
            datas[2].text = data.data3;
        }

        Color32 originColor;
        [SerializeField] private Color32 backgroundHoverColor;
        public void OnPointerEnter(PointerEventData eventData)
        {
            originColor = background.color;
            background.color = backgroundHoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            background.color = originColor;
        }
    }
}