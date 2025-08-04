using System;
using UnityEngine;

namespace FlowLineUIEditor.ColorPicker
{
    [Serializable]
    public struct ColorData
    {
        [Range(0f, 1f)] public float r;
        [Range(0f, 1f)] public float g;
        [Range(0f, 1f)] public float b;
        [Range(0f, 1f)] public float a;

        public ColorData(float r, float g, float b, float a = 1f)
        {
            this.r = Mathf.Clamp01(r);
            this.g = Mathf.Clamp01(g);
            this.b = Mathf.Clamp01(b);
            this.a = Mathf.Clamp01(a);
        }

        public ColorData(Color color)
        {
            this.r = color.r;
            this.g = color.g;
            this.b = color.b;
            this.a = color.a;
        }

        public Color ToColor() => new Color(r, g, b, a);

        public static implicit operator Color(ColorData colorData) => colorData.ToColor();
        public static implicit operator ColorData(Color color) => new ColorData(color);

        public override string ToString()
        {
            return $"RGBA({r:F2}, {g:F2}, {b:F2}, {a:F2})";
        }
    }
}