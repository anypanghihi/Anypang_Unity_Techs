using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class UIDateTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtDate;

    private void Start()
    {
        updateTimer().Forget();
    }

    private void OnDestroy()
    {
        
    }

    async UniTaskVoid updateTimer()
    {
        while(true)
        {
            txtDate.text = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
            await UniTask.Delay(1000, false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        }
    }

}
