using UnityEngine;
using UnityEngine.UI;
using FlowLineUIEditor.ColorPicker.Interfaces;
using FlowLineUIEditor.ColorPicker.Services;
using TMPro;
using System;
using System.Collections;

namespace FlowLineUIEditor.ColorPicker.UI
{
    public enum ColorPickerType
    {
        LineColor,
        PointColor
    }

    public class ColorPickerUI : MonoBehaviour
    {
        [Header("ColorPicker 타입")]
        [SerializeField] private ColorPickerType colorPickerType = ColorPickerType.LineColor;

        [Header("현재 색상")]
        [SerializeField] private ColorData currentColor = new ColorData(0.0f, 0.0f, 0.0f, 1f); // #BA4141FF

        [Header("RGBA 슬라이더")]
        [SerializeField] private Slider RSlider;
        [SerializeField] private Slider GSlider;
        [SerializeField] private Slider BSlider;
        [SerializeField] private Slider ASlider;

        [Header("Hex 코드")]
        [SerializeField] private TMP_InputField hexInputField;
        [SerializeField] private UIButtonTransitor copyButton;

        private bool isUpdatingFromCode = false;
        private IColorConverter colorConverter;
        private IClipboardService clipboardService;
        private Coroutine debounceCoroutine;
        private ColorData pressedColor;
        private ColorData beforeColor;
        private string beforeHexCode;

        private void Awake()
        {
            InitializeDependencies();
        }

        private void Start()
        {
            SetupColorPickerUI();
            InitializeSliders();
            InitializeHexInput();
            InitializeCopyButton();
            SubscribeToEvents();
            InitializeUI();
        }

        public void SetClipboardService(IClipboardService clipboardService)
        {
            this.clipboardService = clipboardService;
        }

        public void SetColorPickerType(ColorPickerType type)
        {
            colorPickerType = type;
        }

        public ColorPickerType GetColorPickerType()
        {
            return colorPickerType;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
            RemoveAllUIListeners();
        }

        private void InitializeDependencies()
        {
            colorConverter = new ColorConverter();
            clipboardService = new ClipboardService();
        }

        private void SetupColorPickerUI()
        {
            // 자체 설정이므로 별도 처리 없음
        }

        private void InitializeUI()
        {
            UpdateUI(currentColor);
            string hexCode = colorConverter.ColorToHex(currentColor);
            UpdateHexDisplay(hexCode);
        }

        private void InitializeSliders()
        {
            if (RSlider != null)
            {
                RSlider.minValue = 0f;
                RSlider.maxValue = 1f;
                RSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            }

            if (GSlider != null)
            {
                GSlider.minValue = 0f;
                GSlider.maxValue = 1f;
                GSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            }

            if (BSlider != null)
            {
                BSlider.minValue = 0f;
                BSlider.maxValue = 1f;
                BSlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            }

            if (ASlider != null)
            {
                ASlider.minValue = 0f;
                ASlider.maxValue = 1f;
                ASlider.onValueChanged.AddListener(_ => OnSliderValueChanged());
            }
        }

        private void InitializeHexInput()
        {
            if (hexInputField != null)
            {
                hexInputField.onEndEdit.AddListener(OnHexInputChanged);
                hexInputField.onValueChanged.AddListener(OnHexInputRealtime);
                hexInputField.characterLimit = 50; // Rich Text 태그를 고려하여 증가
                hexInputField.contentType = TMP_InputField.ContentType.Standard;

                // Rich Text 기능 활성화
                hexInputField.richText = true;
            }
        }

        private void InitializeCopyButton()
        {
            if (copyButton != null)
            {
                copyButton.AddEvent(OnCopyButtonClicked);
            }
        }

        private void SubscribeToEvents()
        {
            // FlowLineUIEventSystem을 사용하므로 별도 구독 불필요
            // 이벤트는 OnSliderValueChanged와 OnHexInputChanged에서 직접 TriggerEvent 호출
        }

        private void UnsubscribeFromEvents()
        {
            // FlowLineUIEventSystem을 사용하므로 별도 구독 해제 불필요
        }



        private void OnSliderValueChanged()
        {
            if (isUpdatingFromCode) return;

            beforeColor = currentColor;

            float r = RSlider != null ? RSlider.value : 0f;
            float g = GSlider != null ? GSlider.value : 0f;
            float b = BSlider != null ? BSlider.value : 0f;
            float a = ASlider != null ? ASlider.value : 1f;

            ColorData newColor = new ColorData(r, g, b, a);
            currentColor = newColor;
            // Hex 코드 업데이트
            string hexCode = colorConverter.ColorToHex(newColor);
            UpdateHexDisplay(hexCode);

            // FlowLineUIEventSystem 이벤트 발생
            if (colorPickerType == ColorPickerType.LineColor)
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerLineColorChanged, newColor);
            }
            else
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerPointColorChanged, newColor);
            }
        }
        private void OnUpdateHexInput(string hexValue)
        {
            beforeColor = currentColor;
            OnPointerDown();
            ProcessHexInput(hexValue);
            hexInputField.stringPosition = hexInputField.text.Length;
            hexInputField.caretPosition = hexInputField.text.Length;

            if (currentColor.ToColor() != beforeColor.ToColor())
            {
                if (colorPickerType == ColorPickerType.LineColor)
                {
                    FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerLineColorChanged, currentColor);
                }
                else
                {
                    FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerPointColorChanged, currentColor);
                }

                OnPointerUp();
            }
        }
        private void OnHexInputChanged(string hexValue)
        {
            if (isUpdatingFromCode) return;

            string cleanHexValue = ExtractPlainTextFromRichText(hexValue).Trim();
            if (!string.IsNullOrEmpty(cleanHexValue) && !cleanHexValue.StartsWith("#"))
            {
                cleanHexValue = "#" + cleanHexValue;
            }

            if (IsValidHexFormat(cleanHexValue))
            {
                OnUpdateHexInput(cleanHexValue);
            }
            else
            {
                ProcessHexInput(beforeHexCode);
            }
        }

        private void OnHexInputRealtime(string hexValue)
        {
            if (isUpdatingFromCode) return;

            // 기존 대기 중이던 처리 취소
            if (debounceCoroutine != null)
                StopCoroutine(debounceCoroutine);

            // 0.05초 후에 한 번만 실행
            debounceCoroutine = StartCoroutine(DelayedHandle(hexValue));
        }
        IEnumerator DelayedHandle(string hexValue)
        {
            yield return new WaitForSeconds(0.05f); // 붙여넣기 끝날 때까지 잠깐 대기
            string cleanHexValue = ExtractPlainTextFromRichText(hexValue).Trim();

            if (!string.IsNullOrEmpty(cleanHexValue) && !cleanHexValue.StartsWith("#"))
            {
                cleanHexValue = "#" + cleanHexValue;
            }

            if (IsValidHexFormat(cleanHexValue))
            {
                OnUpdateHexInput(cleanHexValue);
            }
        }

        private void ProcessHexInput(string hexValue)
        {
            // Rich Text 태그 제거하여 순수한 hex 값만 추출
            string cleanHexValue = ExtractPlainTextFromRichText(hexValue).Trim();

            if (!string.IsNullOrEmpty(cleanHexValue) && !cleanHexValue.StartsWith("#"))
            {
                cleanHexValue = "#" + cleanHexValue;
            }

            // 유효한 Hex 값인 경우 슬라이더와 색상 미리보기 업데이트
            if (IsValidHexFormat(cleanHexValue))
            {
                ColorData newColor = HexToColor(cleanHexValue);
                currentColor = newColor; // currentColor 업데이트 추가

                // 슬라이더 즉시 업데이트
                isUpdatingFromCode = true;
                if (RSlider != null) RSlider.value = newColor.r;
                if (GSlider != null) GSlider.value = newColor.g;
                if (BSlider != null) BSlider.value = newColor.b;
                if (ASlider != null) ASlider.value = newColor.a;
                isUpdatingFromCode = false;

                // Rich Text로 # 기호에만 색상 적용하여 다시 설정
                UpdateHexDisplayWithColor(cleanHexValue, newColor);
            }

            // FlowLineUIEventSystem 이벤트 발생
            if (colorPickerType == ColorPickerType.LineColor)
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerLineHexCodeChanged, cleanHexValue);
            }
            else
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerPointHexCodeChanged, cleanHexValue);
            }
        }

        private string ExtractPlainTextFromRichText(string richText)
        {
            if (string.IsNullOrEmpty(richText)) return "";

            // Rich Text 태그를 모두 제거하여 순수한 텍스트만 추출
            string result = System.Text.RegularExpressions.Regex.Replace(richText, @"<[^>]*>", "");
            return result;
        }

        private void UpdateHexDisplayWithColor(string hexCode, ColorData colorData)
        {
            if (hexInputField == null) return;
            isUpdatingFromCode = true;

            // # 기호에만 색상을 적용할 색상 계산
            Color textColor = colorData.ToColor();

            // Rich Text를 사용하여 # 기호에만 색상 적용
            string colorHex = ColorUtility.ToHtmlStringRGB(textColor);
            string hexOnly = hexCode.TrimStart('#');
            string richText = $"<color=#{colorHex}>#</color>{hexOnly}";
            beforeHexCode = hexOnly;

            hexInputField.text = richText.ToUpper();
            // 전체 텍스트 색상은 기본값으로 복원
            hexInputField.textComponent.color = Color.white;

            isUpdatingFromCode = false;
        }

        private bool IsValidHexFormat(string hexValue)
        {
            if (string.IsNullOrEmpty(hexValue)) return false;

            string cleanHex = hexValue.Replace("#", "");

            if (cleanHex.Length != 3 && cleanHex.Length != 4 && cleanHex.Length != 6 && cleanHex.Length != 8)
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(cleanHex, @"^[0-9A-Fa-f]+$");
        }

        private ColorData HexToColor(string hexCode)
        {
            if (!IsValidHexFormat(hexCode))
                return new ColorData(1f, 1f, 1f, 1f);

            hexCode = hexCode.Replace("#", "").ToUpper();

            try
            {
                if (hexCode.Length == 8)
                {
                    // 8자리 Hex 코드 (RRGGBBAA)
                    int r = System.Convert.ToInt32(hexCode.Substring(0, 2), 16);
                    int g = System.Convert.ToInt32(hexCode.Substring(2, 2), 16);
                    int b = System.Convert.ToInt32(hexCode.Substring(4, 2), 16);
                    int a = System.Convert.ToInt32(hexCode.Substring(6, 2), 16);

                    return new ColorData(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                else if (hexCode.Length == 6)
                {
                    // 6자리 Hex 코드 (RRGGBB)
                    int r = System.Convert.ToInt32(hexCode.Substring(0, 2), 16);
                    int g = System.Convert.ToInt32(hexCode.Substring(2, 2), 16);
                    int b = System.Convert.ToInt32(hexCode.Substring(4, 2), 16);

                    return new ColorData(r / 255f, g / 255f, b / 255f, 1f);
                }
                else if (hexCode.Length == 4)
                {
                    // 4자리 Hex 코드 (RGBA)
                    int r = System.Convert.ToInt32(hexCode.Substring(0, 1) + hexCode.Substring(0, 1), 16);
                    int g = System.Convert.ToInt32(hexCode.Substring(1, 1) + hexCode.Substring(1, 1), 16);
                    int b = System.Convert.ToInt32(hexCode.Substring(2, 1) + hexCode.Substring(2, 1), 16);
                    int a = System.Convert.ToInt32(hexCode.Substring(3, 1) + hexCode.Substring(3, 1), 16);

                    return new ColorData(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                else if (hexCode.Length == 3)
                {
                    // 3자리 Hex 코드 (RGB)
                    int r = System.Convert.ToInt32(hexCode.Substring(0, 1) + hexCode.Substring(0, 1), 16);
                    int g = System.Convert.ToInt32(hexCode.Substring(1, 1) + hexCode.Substring(1, 1), 16);
                    int b = System.Convert.ToInt32(hexCode.Substring(2, 1) + hexCode.Substring(2, 1), 16);

                    return new ColorData(r / 255f, g / 255f, b / 255f, 1f);
                }
            }
            catch
            {
                return new ColorData(1f, 1f, 1f, 1f);
            }

            return new ColorData(1f, 1f, 1f, 1f);
        }

        private void OnCopyButtonClicked()
        {
            if (hexInputField != null && clipboardService != null)
            {
                // Rich Text 태그 제거하여 순수한 hex 값만 복사
                string hexCode = ExtractPlainTextFromRichText(hexInputField.text);
                clipboardService.CopyToClipboard(hexCode);
                ShowCopyFeedback();
                Debug.Log($"[ColorPicker] 클립보드 복사 완료: {hexCode}");
            }
            else
            {
                Debug.LogWarning("[ColorPicker] ClipboardService가 없거나 InputField가 없습니다.");
            }
        }

        private void ShowCopyFeedback()
        {
            Debug.Log("[ColorPicker] 복사 완료! UIButtonTransitor 애니메이션 실행 중...");
        }

        public void UpdateUI(ColorData colorData)
        {
            isUpdatingFromCode = true;

            if (RSlider != null) RSlider.value = colorData.r;
            if (GSlider != null) GSlider.value = colorData.g;
            if (BSlider != null) BSlider.value = colorData.b;
            if (ASlider != null) ASlider.value = colorData.a;

            // Hex 값과 텍스트 색상 함께 업데이트
            if (hexInputField != null)
            {
                Color32 color32 = colorData.ToColor();
                string hexCode = $"{color32.r:X2}{color32.g:X2}{color32.b:X2}";

                // Alpha가 255가 아닌 경우에만 Alpha 포함
                if (color32.a != 255)
                {
                    hexCode += $"{color32.a:X2}";
                }

                // Rich Text를 사용하여 # 기호에만 색상 적용
                Color textColor = colorData.ToColor();

                // # 기호에만 색상을 적용하고 나머지는 기본 색상
                string colorHex = ColorUtility.ToHtmlStringRGB(textColor);
                string richText = $"<color=#{colorHex}>#</color>{hexCode}";

                hexInputField.text = richText.ToUpper();

                // 전체 텍스트 색상은 기본값으로 복원
                hexInputField.textComponent.color = Color.white;
            }

            isUpdatingFromCode = false;
        }

        public void UpdateHexDisplay(string hexCode)
        {
            if (isUpdatingFromCode) return;

            isUpdatingFromCode = true;
            if (hexInputField != null)
            {
                // Hex 코드에서 색상을 추출하여 # 기호에만 색상 적용
                if (IsValidHexFormat(hexCode))
                {
                    ColorData colorData = HexToColor(hexCode);

                    // # 기호에만 색상을 적용할 색상 계산
                    Color textColor = colorData.ToColor();

                    // Rich Text를 사용하여 # 기호에만 색상 적용
                    string colorHex = ColorUtility.ToHtmlStringRGB(textColor);
                    string hexOnly = hexCode.TrimStart('#');
                    string richText = $"<color=#{colorHex}>#</color>{hexOnly}";

                    hexInputField.text = richText.ToUpper();

                    // 전체 텍스트 색상은 기본값으로 복원
                    hexInputField.textComponent.color = Color.white;
                }
                else
                {
                    // 유효하지 않은 hex 코드는 그대로 표시
                    hexInputField.text = hexCode.ToUpper();
                }
            }
            isUpdatingFromCode = false;
        }

        public void OnPointerDown()
        {
            pressedColor = beforeColor;
        }
        public void OnPointerUp()
        {
            if (colorPickerType == ColorPickerType.LineColor)
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerLineColorReleased, pressedColor);
            }
            else
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerPointColorReleased, pressedColor);
            }
        }

        private void RemoveAllUIListeners()
        {
            if (RSlider != null)
                RSlider.onValueChanged.RemoveAllListeners();
            if (GSlider != null)
                GSlider.onValueChanged.RemoveAllListeners();
            if (BSlider != null)
                BSlider.onValueChanged.RemoveAllListeners();
            if (ASlider != null)
                ASlider.onValueChanged.RemoveAllListeners();

            if (hexInputField != null)
            {
                hexInputField.onEndEdit.RemoveAllListeners();
                hexInputField.onValueChanged.RemoveAllListeners();
            }

            if (copyButton != null)
            {
                copyButton.RemoveEvent(OnCopyButtonClicked);
            }

            Debug.Log("[ColorPicker] 모든 UI 리스너가 제거되었습니다.");
        }

        // ========================================
        // Public API
        // ========================================

        public void SetColor(ColorData color)
        {
            currentColor = color;
            beforeColor = currentColor;

            // UI 즉시 업데이트
            UpdateUI(color);

            // FlowLineUIEventSystem 이벤트 발생
            if (colorPickerType == ColorPickerType.LineColor)
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerLineColorChanged, color);
            }
            else
            {
                FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.ColorPickerPointColorChanged, color);
            }

        }

        public ColorData GetCurrentColor()
        {
            return currentColor;
        }

        public void InjectDependencies(IColorConverter converter, IClipboardService clipboard)
        {
            colorConverter = converter;
            clipboardService = clipboard;
        }
    }
}