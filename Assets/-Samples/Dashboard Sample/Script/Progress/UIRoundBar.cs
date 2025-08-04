using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;

public class UIRoundBar : MonoBehaviour
{
    [Range(0,100)]
    public float fillValue = 0;
    [SerializeField] Slider slider;
    [SerializeField] GameObject    fillArea;
    [SerializeField] RectTransform roundBar;
    [SerializeField] RectTransform fillHandler;    

    [SerializeField] TextMeshProUGUI texPercent;

    float BarWidth;
    float FillHeight;

    void Start()
    {
        fillArea.SetActive(false);

        BarWidth = roundBar.sizeDelta.x;
        FillHeight = fillHandler.sizeDelta.y;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        FillValue(0);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        FillValue(30);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        FillValue(100);
    //    }
    //}

    public async UniTaskVoid FillValue(float value)
    {
        float curAmount = slider.value;
        float nxtAmount = (value / 100.0f);

        if (value >= 0.1f)
        {
            fillArea.SetActive(true);
        }

        float amount = 0.0f;
        float t = 0.0f;
        while (t <= 1.0f )
        {
            t += Time.deltaTime;
            amount = Mathf.Lerp(curAmount, nxtAmount, t);

            slider.value = amount;
            fillHandler.offsetMin = new Vector2( 0.0f, fillHandler.offsetMin.y);
            fillHandler.offsetMax = new Vector2( BarWidth * (1 - amount), fillHandler.offsetMax.y);

            // Percent 표시 : null 처리해야 됨.            
            texPercent.text = string.Format("{0:N1}", amount * 100);

            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }

        if (nxtAmount <= 0.0f)
        {
            fillArea.SetActive(false);
        }
    }
}
