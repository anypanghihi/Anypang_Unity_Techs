using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UIBarFill : MonoBehaviour
{
    [Range(0,100)]
    public float fillValue = 0;
    [SerializeField] Image fillImage;
    [SerializeField] RectTransform fillHandler;
    [SerializeField] RectTransform handlerEdgeImage;
    [SerializeField] GameObject[] objEdge;

    [SerializeField] TextMeshProUGUI texPercent;

    // Start is called before the first frame update
    void Start()
    {
        oldfillX = fillHandler.localPosition.x;
        oldedgeX = handlerEdgeImage.localPosition.x;

        foreach (GameObject each in objEdge) each.SetActive(false);
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

    float oldfillX;
    float oldedgeX;
    public async UniTaskVoid FillValue(float value)
    {
        float curAmount = fillImage.fillAmount;
        float nxtAmount = (value / 100.0f);

        foreach (GameObject each in objEdge) each.SetActive(true);

        float amount = 0.0f;
        float t = 0.0f;
        while (t <= 1.0f )
        {
            t += Time.deltaTime;
            amount = Mathf.Lerp(curAmount, nxtAmount, t);

            fillImage.fillAmount = amount;
            fillHandler.localPosition = new Vector3(oldfillX + fillHandler.rect.width * amount, 0, 0);
            handlerEdgeImage.localPosition = new Vector3(oldedgeX - fillHandler.rect.width * amount, 0, 0);


            // Percent 표시 : null 처리해야 됨.
            //int percent = (int)(amount * 100);
            //texPercent.text = percent.ToString();  

            await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        }

        if (nxtAmount <= 0.0f)
        {
            foreach (GameObject each in objEdge) each.SetActive(false);
        }
    }
}
