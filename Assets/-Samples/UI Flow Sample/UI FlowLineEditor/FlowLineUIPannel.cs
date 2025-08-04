using UnityEngine;
using FlowLineUIEditor.PointEdit.View;
using FlowLineUIEditor.LineEdit.View;
using FlowLineUIEditor.LineList;
using FlowLineUIEditor.LineList.View;

namespace FlowLineUIEditor.Pannel
{
    public partial class FlowLineUIPannel : MonoBehaviour
    {
        [Header("UI 토글")]
        [SerializeField] private UIToggleTransitor subToggle;
        [SerializeField] private CanvasGroup[] subCanvas;

        [Header("UI 컴포넌트")]
        [SerializeField] private FlowLineListView lineListView;
        [SerializeField] private SelectLineView selectLineView;
        [SerializeField] private PointEditView pointEditView;
        [SerializeField] private LineEditView lineEditView;

        //[ReadOnly] public EnumAreaSO targetArea;

        public int viewType = -1;

        private void Start()
        {
            InitializeComponents();
            InitializeLineList();
            InitializeUIEvent();
            InitializeMenu();
        }
        private void OnDestroy()
        {
            UnInitializeComponents();
            UnInitializeLineList();
            UnInitializeUIEvent();
            UnInitializeMenu();
        }


        private void InitializeComponents()
        {

        }

        private void UnInitializeComponents()
        {
        }



        // To. Rasan (선택한 설비에 들어갈때 첫 한번 호출)
        public void EnterFlowLine()
        {
            ClearScrollView();

            //targetArea = RuntimeManager.instance.targetUIWorldActor.GetAreaSO();
            //SetMainLine(targetArea.name);
            SetupScrollView();

            ShowLineList();
            viewType = -1;
            gameObject.SetActive(true);
        }

        // To. Rasan (선택한 설비를 나갈때 첫 한번 호출)
        public void ExitFlowLine()
        {
            ClearScrollView();
            viewType = -1;
            gameObject.SetActive(false);
        }


        // To. Rasan
        public void ShowLineList()
        {
            AllHideEditor();
            subToggle.SetToggleID(0);

        }

        // To. Rasan
        public void ShowLineEditor()
        {
            AllHideEditor();
            subCanvas[0].alpha = subCanvas[2].alpha = 1.0f;
            subCanvas[0].interactable = subCanvas[2].interactable = true;
            subCanvas[0].blocksRaycasts = subCanvas[2].blocksRaycasts = true;
            subToggle.SetToggleID(0);
            viewType = 0;
        }

        // To. Rasan
        public void ShowPointEditor()
        {
            AllHideEditor();
            subCanvas[1].alpha = subCanvas[2].alpha = 1.0f;
            subCanvas[1].interactable = subCanvas[2].interactable = true;
            subCanvas[1].blocksRaycasts = subCanvas[2].blocksRaycasts = true;
            subToggle.SetToggleID(1);
            viewType = 1;
        }

        public void AllHideEditor()
        {
            foreach (CanvasGroup each in subCanvas)
            {
                each.alpha = 0.0f;
                each.interactable = false;
                each.blocksRaycasts = false;
            }
        }

        public PointEditView GetPointEditorUI() { return pointEditView; }
        public LineEditView GetLineEditorUI() { return lineEditView; }
        public SelectLineView GetSelectLineUI() { return selectLineView; }
    }

}