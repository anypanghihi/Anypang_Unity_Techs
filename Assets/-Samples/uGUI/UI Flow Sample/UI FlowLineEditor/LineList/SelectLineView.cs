using TMPro;
using UnityEngine;


namespace FlowLineUIEditor.LineList
{
    public class SelectLineView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private TextMeshProUGUI departureText;
        [SerializeField] private TextMeshProUGUI destinationText;
        [SerializeField] private UIButtonTransitor backButton;
        [SerializeField] private UIButtonTransitor removeButton;


        private FlowLineData currentFlowLineData = new FlowLineData();

        private void Awake()
        {
            InitializeUI();
        }

        private void Start()
        {
            AddEventListeners();
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void InitializeUI()
        {
            // 기본값 설정
            SetFlowLineData(0, "Empty", "Empty");
        }

        private void AddEventListeners()
        {
            backButton.AddEvent(OnDeSelectButtonClicked);
            removeButton.AddEvent(OnRemoveButtonClicked);
        }

        private void RemoveEventListeners()
        {
            backButton.RemoveEvent(OnDeSelectButtonClicked);
            removeButton.RemoveEvent(OnRemoveButtonClicked);
        }

        private void OnDeSelectButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineDeSelect);
        }

        private void OnRemoveButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.SelectLineRemove, currentFlowLineData.sId);
        }

        // To. Rasan
        public void SetFlowLineData(int index, string area1Text, string area2Text)
        {
            currentFlowLineData.SetAreaText(sId: index, area1Text, area2Text);
            UpdateUI();
        }

        public FlowLineData GetFlowLineData()
        {
            return currentFlowLineData;
        }



        private void UpdateUI()
        {
            noText.text = currentFlowLineData.sId.ToString();
            departureText.text = currentFlowLineData.area1Text;
            destinationText.text = currentFlowLineData.area2Text;
        }
    }
}