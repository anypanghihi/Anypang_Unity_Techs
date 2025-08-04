using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Korens.Autonomous.Ford
{
    public class OpPanelInfoDataFordWrapper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject wrapperBackground;
        [SerializeField] private VerticalLayoutGroup wrapperLayout;
        [SerializeField] private VerticalLayoutGroup rootLayout;

        public void SetGroupTitleText(string title)
        {
            this.title.text = title;
        }

        public bool IsSameGroup(string compareText)
        {
            return title.text.Equals(compareText);
        }

        public void SetSoloTask(Transform unit)
        {
            title.gameObject.SetActive(false);
            wrapperBackground.SetActive(false);
            wrapperLayout.padding.top = 0;
            rootLayout.padding.bottom = 0;

            unit.SetParent(rootLayout.transform);
        }

        public void WrapPanelInfoData(Transform unit)
        {
            unit.SetParent(rootLayout.transform);
        }
    }
}