using System;
using UnityEngine;
using FlowLineUIEditor.ColorPicker.Interfaces;

namespace FlowLineUIEditor.ColorPicker.Services
{
    public class ColorConverter : IColorConverter
    {
        public string ColorToHex(ColorData colorData)
        {
            // RGBA 모두 포함해서 Hex로 변환 (#RRGGBBAA 형태)
            Color32 color32 = colorData.ToColor();
            return $"#{color32.r:X2}{color32.g:X2}{color32.b:X2}{color32.a:X2}";
        }

        public ColorData HexToColor(string hexCode)
        {
            if (!IsValidHex(hexCode))
                return new ColorData(1f, 1f, 1f, 1f);

            hexCode = hexCode.Replace("#", "").ToUpper();
            
            try
            {
                if (hexCode.Length == 8)
                {
                    // 8자리 Hex 코드 (RRGGBBAA)
                    int r = Convert.ToInt32(hexCode.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hexCode.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hexCode.Substring(4, 2), 16);
                    int a = Convert.ToInt32(hexCode.Substring(6, 2), 16);
                    
                    return new ColorData(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                else if (hexCode.Length == 6)
                {
                    // 6자리 Hex 코드 (RRGGBB) - Alpha는 1.0으로 설정
                    int r = Convert.ToInt32(hexCode.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hexCode.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hexCode.Substring(4, 2), 16);
                    
                    return new ColorData(r / 255f, g / 255f, b / 255f, 1f);
                }
                else if (hexCode.Length == 4)
                {
                    // 4자리 Hex 코드 (RGBA -> RRGGBBAA)
                    int r = Convert.ToInt32(hexCode.Substring(0, 1) + hexCode.Substring(0, 1), 16);
                    int g = Convert.ToInt32(hexCode.Substring(1, 1) + hexCode.Substring(1, 1), 16);
                    int b = Convert.ToInt32(hexCode.Substring(2, 1) + hexCode.Substring(2, 1), 16);
                    int a = Convert.ToInt32(hexCode.Substring(3, 1) + hexCode.Substring(3, 1), 16);
                    
                    return new ColorData(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                else if (hexCode.Length == 3)
                {
                    // 3자리 Hex 코드 (RGB -> RRGGBB) - Alpha는 1.0으로 설정
                    int r = Convert.ToInt32(hexCode.Substring(0, 1) + hexCode.Substring(0, 1), 16);
                    int g = Convert.ToInt32(hexCode.Substring(1, 1) + hexCode.Substring(1, 1), 16);
                    int b = Convert.ToInt32(hexCode.Substring(2, 1) + hexCode.Substring(2, 1), 16);
                    
                    return new ColorData(r / 255f, g / 255f, b / 255f, 1f);
                }
            }
            catch
            {
                return new ColorData(1f, 1f, 1f, 1f);
            }

            return new ColorData(1f, 1f, 1f, 1f);
        }

        public bool IsValidHex(string hexCode)
        {
            if (string.IsNullOrEmpty(hexCode))
                return false;

            hexCode = hexCode.Replace("#", "");
            
            // 3자리(RGB), 4자리(RGBA), 6자리(RRGGBB), 8자리(RRGGBBAA) Hex 코드 지원
            if (hexCode.Length != 3 && hexCode.Length != 4 && hexCode.Length != 6 && hexCode.Length != 8)
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(hexCode, @"^[0-9A-Fa-f]+$");
        }
    }
}