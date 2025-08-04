using UnityEngine;
using System;
using TMPro;

namespace FlowLineUIEditor.LineEdit.View
{
    [Serializable]
    public class NumberControl
    {
        [Header("UI 컴포넌트")]
        [SerializeField] private UIButtonTransitor plusButton;
        [SerializeField] private UIButtonTransitor minusButton;
        [SerializeField] private TextMeshProUGUI numberText;

        [Header("설정")]
        [SerializeField] private int currentValue = 10;
        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 100;
        [SerializeField] private int step = 1;

        // 이벤트
        public event System.Action<int> OnValueChanged;
        public event System.Action OnPlusClicked;
        public event System.Action OnMinusClicked;

        private bool isInitialized = false;

        public void Initialize()
        {
            if (isInitialized) return;

            if (plusButton != null)
            {
                plusButton.AddEvent(OnPlusButtonClicked);
            }

            if (minusButton != null)
            {
                minusButton.AddEvent(OnMinusButtonClicked);
            }

            UpdateDisplay();
            isInitialized = true;
        }

        public void Cleanup()
        {
            if (!isInitialized) return;

            if (plusButton != null)
            {
                plusButton.RemoveEvent(OnPlusButtonClicked);
            }

            if (minusButton != null)
            {
                minusButton.RemoveEvent(OnMinusButtonClicked);
            }

            isInitialized = false;
        }

        private void OnPlusButtonClicked()
        {
            SetValue(currentValue + step);
            OnPlusClicked?.Invoke();
        }

        private void OnMinusButtonClicked()
        {
            SetValue(currentValue - step);
            OnMinusClicked?.Invoke();
        }

        public void SetValue(int value)
        {
            int newValue = Mathf.Clamp(value, minValue, maxValue);
            if (newValue != currentValue)
            {
                currentValue = newValue;
                UpdateDisplay();
                OnValueChanged?.Invoke(currentValue);
            }
        }

        public int GetValue()
        {
            return currentValue;
        }

        public void SetRange(int min, int max)
        {
            minValue = min;
            maxValue = max;
            SetValue(currentValue); // 현재 값이 범위를 벗어났다면 조정
        }

        public void SetStep(int stepValue)
        {
            step = Mathf.Max(1, stepValue);
        }

        private void UpdateDisplay()
        {
            if (numberText != null)
            {
                numberText.text = currentValue.ToString();
            }

            // 버튼 상태 업데이트
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            if (plusButton != null)
            {
                plusButton.SetInteractable(currentValue < maxValue);
            }

            if (minusButton != null)
            {
                minusButton.SetInteractable(currentValue > minValue);
            }
        }

        // 에디터에서 값 변경 시 호출 (Inspector 전용)
        public void OnValidate()
        {
            currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
            if (Application.isPlaying && isInitialized)
            {
                UpdateDisplay();
            }
        }
    }
} 