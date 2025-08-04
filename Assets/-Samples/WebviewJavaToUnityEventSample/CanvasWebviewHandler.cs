using System;
using System.IO;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView;

public class CanvasWebViewHandler : MonoBehaviour
{

    [SerializeField] private CanvasWebViewPrefab _canvasWebViewPrefab;

    void Start()
    {
        // WebView가 준비될 때까지 기다렸다가 로드
        //_canvasWebViewPrefab.Initialized += OnWebViewInitialized;

        SetWebViewDownloadAsync().Forget();

        Debug.Log("start");
    }

    private void OnWebViewInitialized(object sender, System.EventArgs e)
    {
        _canvasWebViewPrefab.WebView.MessageEmitted += WebView_MessageEmitted;
        //_canvasWebViewPrefab.WebView.LoadUrl("http://192.168.66.1:5500/Assets/-Samples/WebviewJavaToUnityEventSample/TestPage.html"); // HTML 파일 경로
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
        _canvasWebViewPrefab.WebView.PostMessage("Message from Unity to WebView!");
    }

    async UniTaskVoid SetWebViewDownloadAsync()
    {
        await _canvasWebViewPrefab.WaitUntilInitialized();
        LoadHTML();

        // var webViewWithDownloads = _canvasWebViewPrefab.WebView as IWithDownloads;
        // if (webViewWithDownloads == null)
        // {
        //     Debug.Log("This 3D WebView plugin doesn't yet support IWithDownloads: " + _canvasWebViewPrefab.WebView.PluginType);
        //     return;
        // }

        // //string parentFolder = Path.GetDirectoryName(Application.dataPath);
        // //string destinationFolder = Path.Combine(parentFolder, "DownloadFiles");
        // string destinationFolder = GetDownloadPath();


        // webViewWithDownloads.SetDownloadsEnabled(true);
        // webViewWithDownloads.DownloadProgressChanged += (sender, eventArgs) =>
        // {
        //     Debug.Log($@"DownloadProgressChanged:
        //                Type: {eventArgs.Type},
        //                Url: {eventArgs.Url},
        //                Progress: {eventArgs.Progress},
        //                Id: {eventArgs.Id},
        //                FilePath: {eventArgs.FilePath},
        //                ContentType: {eventArgs.ContentType}"
        //             );

        //     if (eventArgs.Type == ProgressChangeType.Finished)
        //     {
        //         Debug.Log("Download finished");

        //         string destinationFile = ZString.Concat(destinationFolder, Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(eventArgs.FilePath), Path.GetExtension(eventArgs.FilePath));

        //         if (!Directory.Exists(destinationFolder))
        //         {
        //             Directory.CreateDirectory(destinationFolder);
        //         }

        //         Debug.Log(destinationFile);

        //         File.Move(eventArgs.FilePath, destinationFile);
        //     }
        // };
    }

    string GetDownloadPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";
    }

    public void LoadHTML()
    {
        _canvasWebViewPrefab.WebView.LoadHtml("<iframe src=\"http://youchelin.ycs.com/\" width=\"100%\" height=\"100%\" frameborder=\"0\"></iframe>");
    }
}
