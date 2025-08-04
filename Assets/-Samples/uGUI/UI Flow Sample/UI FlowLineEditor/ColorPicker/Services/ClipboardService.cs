using UnityEngine;
using FlowLineUIEditor.ColorPicker.Interfaces;

namespace FlowLineUIEditor.ColorPicker.Services
{
    public class ClipboardService : IClipboardService
    {
        public void CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning("[클립보드] 빈 텍스트는 복사할 수 없습니다.");
                return;
            }

            try
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                ClipboardUtil.Copy(text);
#else
                // 브라우저(WebGL)에서는 Unity 내장 클립보드 사용
                GUIUtility.systemCopyBuffer = text;
#endif
                Debug.Log($"[클립보드 복사 성공] {text}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[클립보드 오류] 복사 실패: {ex.Message}");
            }
        }

        public string GetFromClipboard()
        {
            try
            {
                return GUIUtility.systemCopyBuffer ?? string.Empty;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[클립보드 읽기 오류] {ex.Message}");
                return string.Empty;
            }
        }
    }
}