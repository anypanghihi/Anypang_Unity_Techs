using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICircleFillR : MonoBehaviour
{
    [Range(0,100)]
    public float fillValue = 0;
    [SerializeField] Image circleFillImage;
    [SerializeField] RectTransform fillHandler;
    [SerializeField] RectTransform handlerEdgeImage;
    [SerializeField] GameObject[] objEdge;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject each in objEdge) each.SetActive(false);
    }

    // Update is called once per frame
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
        float curAmount = circleFillImage.fillAmount;
        float nxtAmount = (value / 100.0f);
        foreach (GameObject each in objEdge) each.SetActive(true);

        float amount = 0.0f;
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * 1.1f;
            amount = Mathf.Lerp(curAmount, nxtAmount, t);

            circleFillImage.fillAmount = amount;

            float angle = amount * 360;
            fillHandler.localEulerAngles = new Vector3(0, 0, -angle);
            handlerEdgeImage.localEulerAngles = new Vector3(0, 0, angle);

            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }        
        

        if (nxtAmount <= 0.0f)
        {
            foreach (GameObject each in objEdge) each.SetActive(false);
        }
    }
}
