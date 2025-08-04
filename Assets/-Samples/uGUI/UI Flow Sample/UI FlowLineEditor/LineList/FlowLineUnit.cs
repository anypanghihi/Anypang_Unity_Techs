using Gpm.Ui;
using TMPro;
using UnityEngine;
using FlowLineUIEditor;

namespace FlowLineUIEditor.LineList
{
    public enum ScrollViewType
    {
        Item,
        Create
    }

    public class FlowLineUnit : InfiniteScrollItem, IFlowLineUIEventHandler
    {
        [Header("Item")]
        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private UIButtonTransitor departureButton;
        [SerializeField] private UIButtonTransitor destinationButton;
        [SerializeField] private UIButtonTransitor selectButton;
        [SerializeField] private UIButtonTransitor removeButton;

        [Header("Create")]
        [SerializeField] private CanvasGroup createCanvas;
        [SerializeField] private UIButtonTransitor createButton;

        private FlowLineData currentItem;
        private FlowLineUIEventType currentUIEvent = FlowLineUIEventType.None;

        private void Awake()
        {
            InitializeUI();
        }

        private void Start()
        {
            FlowLineUIEventSystem.AddEvent(FlowLineUIEventType.PopupTabScrollAreaConfirm, this);

            AddEventListeners();
        }

        private void OnDestroy()
        {
            FlowLineUIEventSystem.RemoveEvent(FlowLineUIEventType.PopupTabScrollAreaConfirm, this);

            RemoveEventListeners();
        }

        private void InitializeUI()
        {
        }

        private void AddEventListeners()
        {
            departureButton.AddEvent(OnDepartureClicked);
            destinationButton.AddEvent(OnDestinationClicked);
            selectButton.AddEvent(OnSelectButtonClicked);
            removeButton.AddEvent(OnRemoveButtonClicked);
            createButton.AddEvent(OnCreateClicked);
        }

        private void RemoveEventListeners()
        {
            departureButton.RemoveEvent(OnDepartureClicked);
            destinationButton.RemoveEvent(OnDestinationClicked);
            selectButton.RemoveEvent(OnSelectButtonClicked);
            removeButton.RemoveEvent(OnRemoveButtonClicked);
            createButton.RemoveEvent(OnCreateClicked);
        }

        private void OnDepartureClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineDeparture, currentItem.sId);
            currentUIEvent = FlowLineUIEventType.SelectLineDeparture;
        }
        private void OnDestinationClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineDestination, currentItem.sId);
            currentUIEvent = FlowLineUIEventType.SelectLineDestination;
        }
        private void OnSelectButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineSelect, currentItem.sId);
        }
        private void OnRemoveButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineRemove, currentItem.sId);
        }
        private void OnCreateClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineCreate);
        }

        public void HandleEvent(FlowLineUIEventData eventData)
        {
            var flowData = eventData.GetDataAs<PopupFlowLineData>();

            if (flowData != null)
            {
                if (currentUIEvent.Equals(FlowLineUIEventType.SelectLineDeparture))
                {
                    departureButton.SetText(flowData.areaText);
                    currentItem.SetArea1(flowData.areaText, flowData.areaId);
                }
                else
                if (currentUIEvent.Equals(FlowLineUIEventType.SelectLineDestination))
                {
                    destinationButton.SetText(flowData.areaText);
                    currentItem.SetArea2(flowData.areaText, flowData.areaId);
                }

                if (currentUIEvent != FlowLineUIEventType.None)
                {
                    if (string.IsNullOrEmpty(currentItem.area1Text) is false && string.IsNullOrEmpty(currentItem.area2Text) is false)
                    {

                    }
                }
            }

            currentUIEvent = FlowLineUIEventType.None;
        }

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            base.UpdateData(scrollData);


            currentItem = (FlowLineData)scrollData;

            SetFlowLineData(currentItem.sId, currentItem.area1Text, currentItem.area2Text);


            if (currentItem.viewType.Equals(ScrollViewType.Create))
            {
                createCanvas.alpha = 1.0f;
                createCanvas.interactable = true;
                createCanvas.blocksRaycasts = true;
            }
            else
            {
                createCanvas.alpha = 0.0f;
                createCanvas.interactable = false;
                createCanvas.blocksRaycasts = false;
            }
        }




        // Public API - 다른 개발자가 사용할 메서드들
        public void SetFlowLineData(int index, string area1Text, string area2Text)
        {
            currentItem.SetAreaText(sId: index, area1Text, area2Text);
            UpdateUI();
        }

        public FlowLineData GetFlowLineData()
        {
            return currentItem;
        }

        private void UpdateUI()
        {
            noText.text = currentItem.sId.ToString();
            departureButton.SetText(currentItem.area1Text);
            destinationButton.SetText(currentItem.area2Text);
        }
    }
}