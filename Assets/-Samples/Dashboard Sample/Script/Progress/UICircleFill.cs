using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICircleFill : MonoBehaviour
{
    [Range(0,100)]
    public float fillValue = 0;
    [SerializeField] Image circleFillImage;
    [SerializeField] TextMeshProUGUI textFill;

    CancellationTokenSource cts = null;

    // Start is called before the first frame update
    void Start()
    {
        cts = new CancellationTokenSource();

        textFill.text = "0.0%";
    }

    private void OnDestroy()
    {
        CancelFill();
    }

    public void CancelFill()
    {
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }

        cts = new CancellationTokenSource();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // FillValue(float.Parse(textFill.text));

    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        FillValue(0);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        FillValue(41.56f);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        FillValue(100);
    //    }
    //}

    public async UniTaskVoid FillValue(float value)
    {
        CancelFill();

        if (value < 0.0f) 
        {
            value = 0.0f;
        }
        else
        if (value > 100.0f)
        {
            value = 100.0f;
        }

        float curAmount = circleFillImage.fillAmount;
        float nxtAmount = (value / 100.0f);
        
        float amount = 0.0f;
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * 1.1f;
            amount = Mathf.Lerp(curAmount, nxtAmount, t);

            circleFillImage.fillAmount = amount;
            
            amount *= 100.0f;
            textFill.text = amount.ToString("F1") + "%";

            await UniTask.Yield(cts.Token);
        }

        if(amount.Equals(100.0f))
        {
            textFill.text = "100.0%";
        }
    }
}
