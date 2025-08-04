using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FlowLineUIEditor.Popup
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Image       popupBg;

        public void SetPopupVisible(bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
            popupBg.enabled = visible;            
        }
    }

    public class PopupUI : BasePopup
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI   titleText;
        [SerializeField] private TextMeshProUGUI   contentText;
        [SerializeField] private UIButtonTransitor cancelButton;
        [SerializeField] private UIButtonTransitor confirmButton;


        private void Start()
        {
            SetupEventListeners();
        }

        private void OnDestroy()
        {
            RemoveEventListeners();
        }

        private void SetupEventListeners()
        {
            if (cancelButton != null)
            {
                cancelButton.AddEvent(OnCancelButtonClicked);
            }

            if (confirmButton != null)
            {
                confirmButton.AddEvent(OnConfirmButtonClicked);
            }
        }

        private void RemoveEventListeners()
        {
            if (cancelButton != null)
            {
                cancelButton.RemoveEvent(OnCancelButtonClicked);
            }

            if (confirmButton != null)
            {
                confirmButton.RemoveEvent(OnConfirmButtonClicked);
            }
        }

        private void OnCancelButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PopupCancel);
        }

        private void OnConfirmButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PopupConfirm);
        }



        public void SetPopupData(string title, string content)
        {
            titleText.text = title;
            contentText.text = content;
        }

        public void SetTitleText(string title)
        {
            titleText.text = title;
        }

        public void SetContentText(string content)
        {
            contentText.text = content;
        }
    }
} 