using UnityEngine;
using FlowLineUIEditor.ColorPicker;
using FlowLineUIEditor.ColorPicker.UI;

namespace FlowLineUIEditor.LineEdit.View
{
    public class LineEditView : MonoBehaviour
    {
        [Header("Flow Point Count")]
        [SerializeField] private NumberControl pointCountControl = new NumberControl();

        [Header("Flow Line Width")]
        [SerializeField] private NumberControl lineWidthControl = new NumberControl();

        [Header("Flow Direction")]
        [SerializeField] private UIButtonTransitor directionReverseButton;

        [Header("Undo/Redo 버튼")]
        [SerializeField] private UIButtonTransitor undoButton;
        [SerializeField] private UIButtonTransitor redoButton;

        [Header("Flow Line Color")]
        [SerializeField] private ColorPickerUI lineColorPicker;

        [Header("Flow Point Color")]
        [SerializeField] private ColorPickerUI pointColorPicker;

        private void Start()
        {
            InitializeNumberControls();
            InitializeButtons();
            InitializeColorPickers();
        }

        private void OnDestroy()
        {
            CleanupNumberControls();
            RemoveAllUIListeners();
        }

        private void OnValidate()
        {
            // Inspector에서 값 변경 시 NumberControl 업데이트
            pointCountControl?.OnValidate();
            lineWidthControl?.OnValidate();
        }

        private void InitializeNumberControls()
        {
            // Point Count 초기화 (Inspector 설정값 사용)
            pointCountControl.Initialize();
            pointCountControl.OnPlusClicked += OnPointCountPlusClicked;
            pointCountControl.OnMinusClicked += OnPointCountMinusClicked;
            pointCountControl.OnValueChanged += OnPointCountValueChanged;

            // Line Width 초기화 (Inspector 설정값 사용)
            lineWidthControl.Initialize();
            lineWidthControl.OnPlusClicked += OnLineWidthPlusClicked;
            lineWidthControl.OnMinusClicked += OnLineWidthMinusClicked;
            lineWidthControl.OnValueChanged += OnLineWidthValueChanged;
        }

        private void CleanupNumberControls()
        {
            if (pointCountControl != null)
            {
                pointCountControl.OnPlusClicked -= OnPointCountPlusClicked;
                pointCountControl.OnMinusClicked -= OnPointCountMinusClicked;
                pointCountControl.OnValueChanged -= OnPointCountValueChanged;
                pointCountControl.Cleanup();
            }

            if (lineWidthControl != null)
            {
                lineWidthControl.OnPlusClicked -= OnLineWidthPlusClicked;
                lineWidthControl.OnMinusClicked -= OnLineWidthMinusClicked;
                lineWidthControl.OnValueChanged -= OnLineWidthValueChanged;
                lineWidthControl.Cleanup();
            }
        }

        private void InitializeColorPickers()
        {
            // Line Color Picker 초기화
            if (lineColorPicker != null)
            {
                lineColorPicker.SetColorPickerType(ColorPickerType.LineColor);
                lineColorPicker.SetColor(new ColorData(0.73f, 0.25f, 0.25f, 1f)); // #BA4141FF


                Debug.Log("[LineEditorUI] Line Color Picker 초기화 완료");
            }

            // Point Color Picker 초기화  
            if (pointColorPicker != null)
            {
                pointColorPicker.SetColorPickerType(ColorPickerType.PointColor);
                pointColorPicker.SetColor(new ColorData(0.25f, 0.73f, 0.25f, 1f)); // #41BA41FF
                Debug.Log("[LineEditorUI] Point Color Picker 초기화 완료");
            }
        }

        private void InitializeButtons()
        {
            // Direction 버튼
            if (directionReverseButton != null)
            {
                directionReverseButton.AddEvent(OnDirectionReverseClicked);
            }

            // Undo/Redo 버튼
            if (undoButton != null)
            {
                undoButton.AddEvent(OnUndoButtonClicked);
            }

            if (redoButton != null)
            {
                redoButton.AddEvent(OnRedoButtonClicked);
            }
        }

        // Point Count 이벤트
        private void OnPointCountPlusClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorPointCountPlus);
        }

        private void OnPointCountMinusClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorPointCountMinus);
        }

        private void OnPointCountValueChanged(int newValue)
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorPointCountValueChanged, newValue);
        }

        // Line Width 이벤트
        private void OnLineWidthPlusClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorLineWidthPlus);
        }

        private void OnLineWidthMinusClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorLineWidthMinus);
        }

        private void OnLineWidthValueChanged(int newValue)
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorLineWidthValueChanged, newValue);
        }

        // Direction 버튼 이벤트
        private void OnDirectionReverseClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorDirectionReverse);
        }

        // Undo/Redo 버튼 이벤트
        private void OnUndoButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorUndo);
        }

        private void OnRedoButtonClicked()
        {
            FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.LineEditorRedo);
        }

        // Public API - 값 가져오기/설정
        public int GetPointCount()
        {
            return pointCountControl.GetValue();
        }

        public void SetPointCount(int count)
        {
            pointCountControl.SetValue(count);
        }

        public int GetLineWidth()
        {
            return lineWidthControl.GetValue();
        }

        public void SetLineWidth(int width)
        {
            lineWidthControl.SetValue(width);
        }

        // Color API
        public ColorData GetLineColor()
        {
            return lineColorPicker != null ? lineColorPicker.GetCurrentColor() : new ColorData(1f, 1f, 1f, 1f);
        }

        public void SetLineColor(ColorData color)
        {
            if (lineColorPicker != null)
            {
                lineColorPicker.SetColor(color);
            }
        }

        public ColorData GetPointColor()
        {
            return pointColorPicker != null ? pointColorPicker.GetCurrentColor() : new ColorData(1f, 1f, 1f, 1f);
        }

        public void SetPointColor(ColorData color)
        {
            if (pointColorPicker != null)
            {
                pointColorPicker.SetColor(color);
            }
        }

        // 컴포넌트 참조 설정
        public void SetLineColorPicker(ColorPickerUI colorPicker)
        {
            lineColorPicker = colorPicker;
        }

        public ColorPickerUI GetLineColorPicker()
        {
            return lineColorPicker;
        }

        public void SetPointColorPicker(ColorPickerUI colorPicker)
        {
            pointColorPicker = colorPicker;
        }

        public ColorPickerUI GetPointColorPicker()
        {
            return pointColorPicker;
        }

        public void CheckUndoRedo(bool hasUndo, bool hasRedo)
        {
            undoButton.SetInteractable(hasUndo);
            redoButton.SetInteractable(hasRedo);
        }

        private void RemoveAllUIListeners()
        {
            // Direction 버튼
            if (directionReverseButton != null)
            {
                directionReverseButton.RemoveEvent(OnDirectionReverseClicked);
            }

            // Undo/Redo 버튼
            if (undoButton != null)
            {
                undoButton.RemoveEvent(OnUndoButtonClicked);
            }

            if (redoButton != null)
            {
                redoButton.RemoveEvent(OnRedoButtonClicked);
            }
            Debug.Log("[LineEditorUI] 모든 UI 리스너가 제거되었습니다.");
        }
    }
}