using FlowLineUIEditor.LineList;
using FlowLineUIEditor.Popup;
using System.Collections.Generic;
using UnityEngine;
using FlowLineUIEditor;

public class PopupFlowLineScroll : BasePopup, IFlowLineUIEventHandler
{
    [SerializeField] UITabScrollTransitor tabScroll;
    [SerializeField] UIButtonTransitor confirmButton;
    [SerializeField] UIButtonTransitor cancelButton;

    //[SerializeField] List<EnumAreaArraySO> areaList;

    private PopupFlowLineData selectedFlowLineData = null;

    void Awake()
    {
        //tabScroll.CreateScrollView(areaList);
    }

    private void Start()
    {
        FlowLineUIEventSystem.AddEvent(FlowLineUIEventType.PopupTabScrollAreaSelect, this);
        tabScroll.AddSelectEvent(OnTabScrollSelectEvent);

        confirmButton.AddEvent(OnConfirmButtonClicked);
        cancelButton.AddEvent(OnCancelButtonClicked);
    }

    private void OnDestroy()
    {
        FlowLineUIEventSystem.RemoveEvent(FlowLineUIEventType.PopupTabScrollAreaSelect, this);
        tabScroll.RemoveSelectEvent(OnTabScrollSelectEvent);

        confirmButton.RemoveEvent(OnConfirmButtonClicked);
        cancelButton.RemoveEvent(OnCancelButtonClicked);
    }

    public void HandleEvent(FlowLineUIEventData eventData)
    {
        selectedFlowLineData = eventData.GetDataAs<PopupFlowLineData>();
    }

    // tabScroll의 SelectEvent 리스너
    private void OnTabScrollSelectEvent(int id)
    {
        ResetSelectedFlowLineData();
    }

    private void OnConfirmButtonClicked()
    {
        base.SetPopupVisible(false);

        FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PopupTabScrollAreaConfirm, selectedFlowLineData);
        ResetSelectedFlowLineData();
    }

    private void OnCancelButtonClicked()
    {
        base.SetPopupVisible(false);

        FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PopupTabScrollAreaConfirm, null);
        ResetSelectedFlowLineData();
    }

    public void ClearVisible()
    {
        tabScroll.ClearScrollView();
        ResetSelectedFlowLineData();
    }
    public void ResetSelectedFlowLineData() { selectedFlowLineData = null; }
}
