using UnityEngine;
using Vuplex.WebView;

public class WebViewHandler : MonoBehaviour
{

    [SerializeField] private WebViewPrefab _webViewPrefab;

    void Start()
    {
        _webViewPrefab.WebView.MessageEmitted += WebView_MessageEmitted;
        _webViewPrefab.WebView.LoadUrl("C:/Users/ghwns/Desktop/projects/impix_dtframework2.0/Assets/-Samples/Webview Java To C# Event Sample/TestPage.html"); // HTML 파일 경로
    }

    private void WebView_MessageEmitted(object sender, EventArgs<string> e)
    {
        Debug.Log("Received message from JavaScript: " + e.Value);

        // 버튼 클릭 이벤트 감지
        if (e.Value.Contains("buttonClicked"))
        {
            OnButtonClicked();
        }
    }

    private void OnButtonClicked()
    {
        Debug.Log("Button was clicked in the web page!");
        _webViewPrefab.WebView.PostMessage("Message from Unity to WebView!");
    }
}
