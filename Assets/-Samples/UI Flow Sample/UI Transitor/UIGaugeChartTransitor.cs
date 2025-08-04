using TMPro;
using UnityEngine;
using DG.Tweening;


public class UIGaugeChartTransitor : MonoBehaviour
{
    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] float              duration = 0.2f;
    [SerializeField] TextMeshProUGUI    Title;
    [SerializeField] RectTransform      Rotator;


    float prevGauge = 0.0f;
    float maxGauge  = 1.0f;

    void Start()
    {
        //SetMaxValue("100");
    }

    public void SetMaxValue(string gauge)
    {
        maxGauge = System.Convert.ToSingle(gauge);
    }
    public void SetChartValue(string gauge)
    {
        float dest = System.Convert.ToSingle(gauge);
        dest = dest / maxGauge;
        dest = Mathf.Clamp01(dest);
        

        float currGauge = prevGauge;
        DOTween.To(() => currGauge, x => currGauge = x, dest, duration)
            .SetEase(Ease.OutElastic, 0.0f, 0.5f )
            .OnUpdate(() =>
            {
                Rotator.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(0f, -180f, currGauge));
            });


        prevGauge = dest;
    }

    /*
    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetChartValue("34.2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetChartValue("49.9");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetChartValue("79.9");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetChartValue("0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetChartValue("100");
        }
    }
    */
}
