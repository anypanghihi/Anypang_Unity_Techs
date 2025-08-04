using Doozy.Runtime.Bindy;
using UnityEngine;

namespace Korens.Autonomous
{
    public abstract class OpPanelInfoDataBase : MonoBehaviour
    {
        protected string dataFormat = string.Empty;
        protected string dataValue = string.Empty;
        public string Value
        {
            set
            {
                SetValue(value);
            }
            get
            {
                return dataValue;
            }
        }

        public abstract void SetItemData(OpItem item, string category);

        public abstract void SetValue(string value);

        [SerializeField] private Binder binder;
        public void SetCategory(string category, string key)
        {
            binder.bindId.Category = category;
            binder.bindId.Name = key;
        }
    }
}