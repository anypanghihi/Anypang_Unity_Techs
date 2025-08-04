using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIToggleGroupTransitor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Toggle toggle;
    [SerializeField] UnityEvent SelectEvent;

    [Space(5)]
    [Header("내부 속성")]
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Color[] TextColor;
    [SerializeField] Image Image;
    [SerializeField] Color[] ImageColor;

    private enum ButtonState
    {
        Normal = 0,
        Hover = 1,
        Pressed = 2,
        Released = 3,
        Disabled = 4
    }

    private void Start()
    {   
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
        SetColor(ButtonState.Normal);
    }


    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    public void SetToggleGroup(ToggleGroup group)
    {
        toggle.group = group;
    }
    public void SetToggleOn(bool isOn)
    {
        toggle.isOn = isOn;
        OnToggleValueChanged(isOn);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // 토글이 선택되었을 때만 SelectEvent 호출
        if (isOn)
        {
            InvokeEvent();

            int state = (int)ButtonState.Hover;
            Image.color = ImageColor[state];
            Text.color = TextColor[state];
        }
        else
        {
            SetColor(ButtonState.Normal);
        }
    }

    public void SetInteractable(bool val)
    {
        toggle.interactable = val;

        if (toggle.interactable != true)
        {
            int state = (int)ButtonState.Disabled;
            Image.color = ImageColor[state];
            Text.color = TextColor[state];
        }
    }

    public void InvokeEvent()
    {
        SelectEvent.Invoke();
    }

    /// <summary>
    /// 버튼 클릭 이벤트 추가 (Public Wrapper)
    /// </summary>
    /// <param name="action">추가할 액션</param>
    public void AddEvent(UnityAction action)
    {
        if (SelectEvent != null && action != null)
        {
            SelectEvent.AddListener(action);
        }
    }

    /// <summary>
    /// 버튼 클릭 이벤트 제거 (Public Wrapper)
    /// </summary>
    /// <param name="action">제거할 액션</param>
    public void RemoveEvent(UnityAction action)
    {
        if (SelectEvent != null && action != null)
        {
            SelectEvent.RemoveListener(action);
        }
    }

    /// <summary>
    /// 모든 버튼 클릭 이벤트 제거 (Public Wrapper)
    /// </summary>
    public void RemoveAllEvents()
    {
        if (SelectEvent != null)
        {
            SelectEvent.RemoveAllListeners();
        }
    }



    public void OnPointerEnter(PointerEventData eventData) { SetColor(ButtonState.Hover);   }
    public void OnPointerExit(PointerEventData eventData)  { SetColor(ButtonState.Normal);  }
    public void OnPointerDown(PointerEventData eventData)  { SetColor(ButtonState.Pressed); }
    public void OnPointerUp(PointerEventData eventData)    { SetColor(ButtonState.Normal);  }



    private void SetColor(ButtonState id)
    {
        if (toggle.interactable && !toggle.isOn)
        {
            int state = (int)id;
            Image.color = ImageColor[state];
            Text.color = TextColor[state];
        }
    }

    public void SetText(string val)
    {
        Text.text = val;
    }
}