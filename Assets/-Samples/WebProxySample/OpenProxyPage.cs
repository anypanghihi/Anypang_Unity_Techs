using System.Diagnostics;
using UnityEngine;

public class OpenProxyPage : MonoBehaviour
{
    public string url;

    public void OpenWebPage()
    {
        Process.Start(url); // Unity가 제공하는 프록시 주소
    }
}
