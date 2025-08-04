using FlowLineUIEditor.Popup;
using UnityEngine;

namespace FlowLineUIEditor.Pannel
{
    public partial class FlowLineUIPannel // Popup
    {
        [Header("UI 팝업")]
        [SerializeField] private PopupPanel popupPanel;

        private int currentId;
        


        public void ShowDeletePopup(int sId) // sub Line Id
        {            
            currentId = sId;
            popupPanel.ShowPopup(PopupType.Delete, sId.ToString(), "삭제 하시겠습니까?");
        }

        public void ShowUpdatePopup(int mId) // main Line Id
        {           
            currentId = mId;    
            popupPanel.ShowPopup(PopupType.Update, mId.ToString(), "서버 업로드 하시겠습니까?");
        }

        public void HidePopup()
        {
            popupPanel.HidePopup();
        }

        private void OnPopupCancel()
        {
            Debug.Log("[Popup Cancel] 취소를 선택했습니다.");
        }

        private void OnPopupConfirm()
        {
            // PopupPanel의 currentPopupType에 따라 처리
            var popupType = popupPanel.GetCurrentPopupType();
            
            if (popupType == PopupType.Update)
            {
                OnUpdateConfirm();
            }
            else if (popupType == PopupType.Delete)
            {
                OnDeleteConfirm();
            }
        }

        private void OnUpdateConfirm()
        {
            Debug.Log("[Popup Update 확인] - 업데이트 작업을 수행합니다.");
            // 여기서 실제 업데이트 작업 수행
            
            //currentId
            //실제 서버로 보내는 작업

            //Popup UpLoading 관련 UI 작업

            // 완료되면 Popup Uploading Hide 해줘야 함.
        }

        private void OnDeleteConfirm()
        {
            Debug.Log("[Popup Delete 확인] - 삭제 작업을 수행합니다.");

            // 여기서 실제 삭제 작업 수행
            RemoveScrollItem(currentId);
        }
    }
}