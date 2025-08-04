using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.UI.Shaders.Sample;

public class UIPieChartTransitor : MonoBehaviour
{
    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] float duration = 0.2f;
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI Percent;
    [SerializeField] Meter FillGauge;
    [SerializeField] Vector2 RemapVal = new Vector2(0.75f, 0.125f);


    float prevPercent = 0.0f;

    void Start()
    {
        SetChartValue("0.0");
    }

    public void SetChartValue(string percent)
    {
        // 0 ~ 1 사이의 값으로 변경해야 함. (일단 pecent 값으로 온다고 가정)
        float val100 = System.Convert.ToSingle(percent);


        float currPercent = prevPercent;
        DOTween.To(() => currPercent, x => currPercent = x, val100, duration)
            .OnUpdate(() =>
            {
                Percent.text = currPercent.ToString("F1");
            })
            .OnComplete(() =>
            {
                if (currPercent.Equals(100.0f))
                {
                    currPercent = 100.0f;
                    Percent.text = currPercent.ToString("F1");
                }
            });


        float val01 = Mathf.Clamp01(val100 * 0.01f);
        val01 = RemapVal.x * val01 + RemapVal.y;

        DOTween.To(() => FillGauge.Value, x => FillGauge.Value = x, val01, duration);

        prevPercent = val100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetChartValue("1.2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetChartValue("43.9");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetChartValue("99.9");
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
}
