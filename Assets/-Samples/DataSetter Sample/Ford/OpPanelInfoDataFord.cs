using System.Linq;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;

namespace Korens.Autonomous.Ford
{
    enum DataType
    {
        specMinMax,
        specMinOnly,
        specMaxOnly,
        noSpecData
    }

    public class OpPanelInfoDataFord : OpPanelInfoDataBase
    {
        [SerializeField] UIButton button;
        [SerializeField] GameObject dashboardIcon;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI specMinText;
        [SerializeField] TextMeshProUGUI specMaxText;
        [SerializeField] TextMeshProUGUI value;
        [SerializeField] FordOpItem itemdata;
        [SerializeField] OpPanelInfoDataFordWrapper wrapperPrefab;

        public override void SetItemData(OpItem item, string category)
        {
            itemdata = item as FordOpItem;

            SetCategory(category, itemdata.TaskID);

            SetDescription(itemdata);
        }

        public override void SetValue(string value)
        {
            throw new System.NotImplementedException();
        }

        DataType dataType;
        float specMin = 0.0f;
        float specMax = 0.0f;

        public void SetDescription(FordOpItem item)
        {
            title.text = item.ProcessName;

            specMinText.text = item.MinSPECValue.ToString();
            specMaxText.text = item.MaxSPECValue.ToString();

            SetPanelInfoForm(item);
        }

        // 생성되었을 때, 나와 같은 Group이 이미 있는지 확인
        // 있다면? 해당 Group의 자식 오브젝트로 들어간다.
        // 없다면? 새로운 Group을 생성
        private void SetPanelInfoForm(FordOpItem item)
        {
            Transform parent = transform.parent;
            if (parent == null) return;

            if (item.isGroupTask)
            {
                OpPanelInfoDataFordWrapper wrapper = FindExistingWrapper(parent, item.GroupTaskID);

                if (wrapper == null) // 기존 그룹이 없으면 새 그룹 생성
                {
                    wrapper = Instantiate(wrapperPrefab, parent);
                    wrapper.name = item.GroupTaskID;
                    wrapper.SetGroupTitleText(item.GroupTaskID);
                }

                transform.SetParent(wrapper.transform);
                wrapper.WrapPanelInfoData(this.transform);
            }
            else
            {
                OpPanelInfoDataFordWrapper newWrapper = Instantiate(wrapperPrefab, parent);
                newWrapper.SetSoloTask(this.transform);
                newWrapper.name = item.TaskID;
            }
        }

        // 기존 그룹을 찾는 함수
        private OpPanelInfoDataFordWrapper FindExistingWrapper(Transform parent, string groupTaskID)
        {
            foreach (Transform sibling in parent)
            {
                if (sibling.TryGetComponent<OpPanelInfoDataFordWrapper>(out var wrapper) && wrapper.IsSameGroup(groupTaskID))
                {
                    return wrapper;
                }
            }
            return null;
        }

        public void SetDescriptionNG(OPTaskUnit taskUnit)
        {
            title.text = taskUnit.Task;

            specMinText.text = "";
            specMaxText.text = "";
            dataType = DataType.noSpecData;

            button.interactable = true;
            dashboardIcon.SetActive(true);
        }

        bool IsDataNG(string strValue)
        {
            float value = float.Parse(strValue);

            switch (dataType)
            {
                case DataType.specMinMax:
                    return value < specMin || value > specMax;

                case DataType.specMinOnly:
                    return value < specMin;

                case DataType.specMaxOnly:
                    return value > specMax;

                case DataType.noSpecData:
                    return false;

                default:
                    return false;
            }
        }

    }
}
