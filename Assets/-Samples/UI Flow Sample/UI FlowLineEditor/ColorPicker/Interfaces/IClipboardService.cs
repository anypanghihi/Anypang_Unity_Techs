namespace FlowLineUIEditor.ColorPicker.Interfaces
{
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
        string GetFromClipboard();
    }
}