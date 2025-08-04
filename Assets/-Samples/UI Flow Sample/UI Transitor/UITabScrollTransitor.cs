using DG.Tweening;
using FlowLineUIEditor.LineList;
using Gpm.Ui;
using ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UITabScrollTransitor : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> SelectEvent;

    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] float duration = 0.2f;
    [SerializeField] RectTransform Selector;
    [SerializeField] Vector2[] SelectPos;
    [SerializeField] TextMeshProUGUI[] SelectText;
    [SerializeField] Image[] SelectImage;
    [SerializeField] Color[] tabImageColor;
    [SerializeField] Color[] tabTextColor;
    [SerializeField] InfiniteScroll scrollView;


    private PopupFlowLineData[][] dataList;


    public int SelectId { get; private set; }

    private void Awake()
    {
    }

    public void AddSelectEvent(UnityAction<int> action) { SelectEvent.AddListener(action); }
    public void RemoveSelectEvent(UnityAction<int> action) { SelectEvent.RemoveListener(action); }


    void Start()
    {
        scrollView.layout.padding.y = 2f;
        scrollView.layout.space.y = 0f;

        PostSelected(0);
    }

    public void SetToggleID(int id)
    {
        Selector.DOAnchorPos(SelectPos[id], duration).SetEase(Ease.InOutQuad);

        PostSelected(id);
    }

    void PostSelected(int id)
    {
        SelectId = id;
        SelectEvent.Invoke(id);
        ActiveScrollView(id);

        if (SelectText.Length > 0)
        {
            foreach (var item in SelectText)
                item.color = tabTextColor[0];

            SelectText[id].color = tabTextColor[1];
        }
        if (SelectImage.Length > 0)
        {
            foreach (var item in SelectImage)
                item.color = tabImageColor[0];

            SelectImage[id].color = tabImageColor[1];
        }


    }


    // public void CreateScrollView(List<EnumAreaArraySO> srcItems)
    // {
    //     int i=0;

    //     dataList = new PopupFlowLineData[srcItems.Count][];
    //     foreach (var srcItem in srcItems)
    //     {
    //         int j=0;
    //         dataList[i] = new PopupFlowLineData[srcItem.Arealist.Length];
    //         foreach (var item in srcItem.Arealist)
    //         {
    //             dataList[i][j] = new PopupFlowLineData();
    //             dataList[i][j].SetData(++j, false, item.name, item.ID);
    //         }

    //         i++;
    //     }

    // }

    public void ActiveScrollView(int id)
    {
        ClearToggle();

        scrollView.Clear();
        scrollView.InsertData(dataList[id]);
    }

    public void ClearScrollView()
    {
        ClearToggle();

        scrollView.Clear();
        scrollView.InsertData(dataList[SelectId]);
    }

    public void ClearToggle()
    {
        foreach (var item in dataList[SelectId])
        {
            item.isOn = false;
        }
    }

}
