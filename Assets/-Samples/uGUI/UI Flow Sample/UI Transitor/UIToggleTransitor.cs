using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIToggleTransitor : MonoBehaviour
{
    [SerializeField] UnityEvent<int> SelectEvent;

    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] float              duration = 0.2f;
    [SerializeField] RectTransform      Selector;    
    [SerializeField] Vector2[]          SelectPos;
    [SerializeField] TextMeshProUGUI[]  SelectText;
    [SerializeField] Image[]            SelectImage;
    [SerializeField] Color[]            toggleColor;

    public int SelectId { get; private set; }
    protected Action<int> OnChildExCall;

    protected void Start()
    {
        PostSelected(0);
    }

    public void SetToggleID(int id)
    {
        Selector.DOLocalMove(SelectPos[id], duration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(()=> PostSelected(id));        
    }

    void PostSelected(int id)
    {
        SelectId = id;
        SelectEvent.Invoke(id);
        OnChildExCall?.Invoke(id);

        if (SelectText.Length > 0)
        {
            foreach (var item in SelectText)
                item.color = toggleColor[0];

            SelectText[id].color = toggleColor[1];
        }
        if (SelectImage.Length > 0)
        {
            foreach (var item in SelectImage)
                item.color = toggleColor[0];

            SelectImage[id].color = toggleColor[1];
        }        
    }


}
