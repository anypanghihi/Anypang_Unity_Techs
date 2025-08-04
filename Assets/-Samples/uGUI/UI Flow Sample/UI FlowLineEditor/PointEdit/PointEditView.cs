using UnityEngine;

namespace FlowLineUIEditor.PointEdit.View
{
    public class PointEditView : MonoBehaviour
    {
        [Header("Add Point 버튼")]
        [SerializeField] private UIButtonTransitor prevButton;
        [SerializeField] private UIButtonTransitor nextButton;

        [Header("Remove Point 버튼")]
        [SerializeField] private UIButtonTransitor removeButton;

        [Header("Undo/Redo 버튼")]
        [SerializeField] private UIButtonTransitor undoButton;
        [SerializeField] private UIButtonTransitor redoButton;

        private void Start()
        {
            InitializeButtons();
        }

        private void OnDestroy()
        {
            RemoveAllUIListeners();
        }

        private void InitializeButtons()
        {
            if (prevButton != null)
            {
                prevButton.AddEvent(OnPreviousButtonClicked);
            }

            if (nextButton != null)
            {
                nextButton.AddEvent(OnNextButtonClicked);
            }

            if (removeButton != null)
            {
                removeButton.AddEvent(OnRemoveButtonClicked);
            }

            if (undoButton != null)
            {
                undoButton.AddEvent(OnUndoButtonClicked);
            }

            if (redoButton != null)
            {
                redoButton.AddEvent(OnRedoButtonClicked);
            }
        }

        private void OnPreviousButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorPrev);
        }

        private void OnNextButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorNext);
        }

        private void OnRemoveButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorRemove);
        }

        private void OnUndoButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorUndo);
        }

        private void OnRedoButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorRedo);
        }

        public void CheckUndoRedo(bool hasUndo, bool hasRedo)
        {
            undoButton.SetInteractable(hasUndo);
            redoButton.SetInteractable(hasRedo);
        }
        public void RemoveBtnCheck(bool b)
        {
            removeButton.SetInteractable(b);
        }

        private void RemoveAllUIListeners()
        {
            if (prevButton != null)
            {
                prevButton.RemoveEvent(OnPreviousButtonClicked);
            }

            if (nextButton != null)
            {
                nextButton.RemoveEvent(OnNextButtonClicked);
            }

            if (removeButton != null)
            {
                removeButton.RemoveEvent(OnRemoveButtonClicked);
            }

            if (undoButton != null)
            {
                undoButton.RemoveEvent(OnUndoButtonClicked);
            }

            if (redoButton != null)
            {
                redoButton.RemoveEvent(OnRedoButtonClicked);
            }

            //Debug.Log("[PointEditorUI] 모든 UI 리스너가 제거되었습니다.");
        }
    }
}