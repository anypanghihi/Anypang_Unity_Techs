using System.Runtime.InteropServices;
using UnityEngine;

public class ClipboardUtil : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    public static void Copy(string text)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        CopyToClipboard(text);
#else
        GUIUtility.systemCopyBuffer = text; // 에디터나 스탠드얼론에서
#endif
    }
}