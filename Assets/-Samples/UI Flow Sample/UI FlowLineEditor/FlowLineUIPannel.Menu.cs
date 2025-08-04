using TMPro;
using UnityEngine;

namespace FlowLineUIEditor.Pannel
{
    public partial class FlowLineUIPannel // Menu
    {
        [Header("UI 메뉴")]
        [SerializeField] private TextMeshProUGUI   title;
        [SerializeField] private UIButtonTransitor update;


        private void InitializeMenu()
        {
            update.AddEvent(OnUpdateButtonClicked);
        }

        private void UnInitializeMenu()
        {
            update.RemoveEvent(OnUpdateButtonClicked);
        }

        // To. Rasan
        int mId = 0;
        public void SetMainLine(string mainArea)
        {
            mId = 1;

            SetTitle(mainArea);
        }


        void SetTitle(string name)
        {
            title.text = name;
        }


        private void OnUpdateButtonClicked()
        {            
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.MenuUpdate, mId);
        }

        
        
    }
}