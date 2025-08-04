using UnityEngine;

namespace FlowLineUIEditor.Popup
{
    public enum PopupType
    {
        Update,
        Delete
    }

    /// <summary>
    /// PopupPanel - CanvasGroup을 이용한 팝업 관리 클래스
    /// PopupUI의 표시/숨김을 관리하고 버튼 이벤트를 처리합니다.
    /// Update와 Delete 두 가지 타입의 팝업을 지원합니다.
    /// 
    /// 사용 예시:
    /// popupPanel.ShowPopup(PopupType.Delete, "NO. 1", "삭제 하시겠습니까?");
    /// popupPanel.ShowPopup(PopupType.Update, "NO. 1", "업데이트 하시겠습니까?");
    /// </summary>
    public class PopupPanel : MonoBehaviour
    {
        [Header("Popup Components")]
        [SerializeField] private BasePopup[] popups;
        [SerializeField] private PopupUI popupDialog;
        [SerializeField] private PopupUI popupToast;
        [SerializeField] private PopupFlowLineScroll areaListScroll;

        
        private PopupType currentPopupType;

        private void Awake()
        {
            InitializePopup();
        }

        private void Start()
        {
            // 기존 PopupUI에서 직접 이벤트 처리
        }

        private void OnDestroy()
        {
        }

        private void InitializePopup()
        {
            HideAllPopups();
        }


        public void ShowPopup(PopupType popupType, string title, string content)
        {
            HideAllPopups();
            currentPopupType = popupType;
            popupDialog.SetPopupData(title, content);
            popupDialog.SetPopupVisible(true);
        }

        public void ShowAreaListScroll()
        {
            HideAllPopups();
            areaListScroll.SetPopupVisible(true);
            areaListScroll.ClearVisible();
        }


        public void HidePopup()
        {
            HideAllPopups();
        }

        private void HideAllPopups()
        {
            foreach (var popup in popups)
                popup.SetPopupVisible(false);
        }

        public PopupType GetCurrentPopupType()
        {
            return currentPopupType;
        }

        // 외부에서 사용할 수 있는 Static 래퍼 메서드들
        private static PopupPanel currentInstance;

        public static void SetCurrentInstance(PopupPanel instance)
        {
            currentInstance = instance;
        }

        public static void ShowUpdatePopup(string title, string content)
        {
            if (currentInstance != null)
            {
                currentInstance.ShowPopup(PopupType.Update, title, content);
            }
            else
            {
                Debug.LogWarning("[PopupPanel] PopupPanel 인스턴스가 설정되지 않았습니다.");
            }
        }

        public static void ShowDeletePopup(string title, string content)
        {
            if (currentInstance != null)
            {
                currentInstance.ShowPopup(PopupType.Delete, title, content);
            }
            else
            {
                Debug.LogWarning("[PopupPanel] PopupPanel 인스턴스가 설정되지 않았습니다.");
            }
        }

        public static void HidePopupStatic()
        {
            if (currentInstance != null)
            {
                currentInstance.HidePopup();
            }
        }
    }
} 