using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace FlowLineUIEditor.LineList
{
    public class FlowLineTabScrollUnit : InfiniteScrollItem
    {
        [SerializeField] private UIToggleGroupTransitor selectToggle;

        private PopupFlowLineData currentItem;

        private void Awake()
        {
            InitializeUI();
        }

        private void Start()
        {
            AddEventListeners();

            selectToggle.SetToggleGroup(this.GetComponentInParent<ToggleGroup>());
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void InitializeUI()
        {
            
        }

        private void AddEventListeners()
        {
            selectToggle.AddEvent(OnSelectButtonClicked);
        }

        private void RemoveEventListeners()
        {
            selectToggle.RemoveEvent(OnSelectButtonClicked);
        }

        private void OnSelectButtonClicked()
        {            
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PopupTabScrollAreaSelect, currentItem);            
            currentItem.isOn = true;
        }


        public override void UpdateData(InfiniteScrollData scrollData)
        {
            base.UpdateData(scrollData);


            currentItem = (PopupFlowLineData)scrollData;           
            UpdateUI();
        }

        public PopupFlowLineData GetFlowLineData()
        {
            return currentItem;
        }

        private void UpdateUI()
        {
            selectToggle.SetToggleOn(currentItem.isOn);
            selectToggle.SetText(currentItem.areaText);
        }
    }
} 