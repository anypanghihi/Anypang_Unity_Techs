using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class UITabTransitor : MonoBehaviour
{
    [SerializeField] UnityEvent<int> SelectEvent;

    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] float              duration = 0.2f;
    [SerializeField] RectTransform      Selector;    
    [SerializeField] Vector2[]          SelectPos;
    [SerializeField] TextMeshProUGUI[]  SelectText;
    [SerializeField] Image[]            SelectImage;
    [SerializeField] Color[]            tabImageColor;
    [SerializeField] Color[]            tabTextColor;
    [SerializeField] GameObject[]       tabViews;


    public int SelectId { get; private set; }

    void Start()
    {
        PostSelected(0);
    }

    public void SetToggleID(int id)
    {
        Selector.DOLocalMove(SelectPos[id], duration).SetEase(Ease.InOutQuad);

        PostSelected(id);
    }

    void PostSelected(int id)
    {
        SelectId = id;
        SelectEvent.Invoke(id);

        if (SelectText.Length > 0)
        {
            foreach (var item in SelectText)
                item.color = tabTextColor[0];

            SelectText[id].color = tabTextColor[1];
        }
        if (SelectImage.Length > 0)
        {
            foreach (var item in SelectImage)
                item.color = tabImageColor[0];

            SelectImage[id].color = tabImageColor[1];
        }


        foreach (var view in tabViews)
            view.SetActive(false);

        tabViews[id].SetActive(true);
    }


}
