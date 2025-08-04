using Cysharp.Text;
using UnityEngine;
using Binder = Doozy.Runtime.Bindy.Binder;

namespace Korens.Autonomous
{
    public class OpItemData : MonoBehaviour
    {
        [SerializeField] private OpItem item;
        public OpItem ItemData
        {
            set { item = value; }
            get { return item; }
        }

        public string Key { get { return item.TaskID; } }
        public string Value { get; set; }

        [SerializeField] Binder binder;
        public void SetCategory(string category, string name)
        {
            binder.bindId.Category = category;
            binder.bindId.Name = name;
        }
    }
}
