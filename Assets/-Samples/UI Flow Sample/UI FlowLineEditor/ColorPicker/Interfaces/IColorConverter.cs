namespace FlowLineUIEditor.ColorPicker.Interfaces
{
    public interface IColorConverter
    {
        string ColorToHex(ColorData colorData);
        ColorData HexToColor(string hexCode);
        bool IsValidHex(string hexCode);
    }
}