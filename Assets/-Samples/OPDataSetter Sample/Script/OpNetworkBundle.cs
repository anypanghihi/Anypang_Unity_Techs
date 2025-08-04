using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sample.OPDataSetter
{
    [Serializable]
    public class OpNetwork
    {
        [Header("요청 ID 이름")]
        public string reqName;

        [Header("요청할 정보")]
        public string reqUrl;

        [Header("응답시 사용할 키목록")]
        public List<string> respKeys = new List<string>();
    }



    [CreateAssetMenu(menuName = "Doozy/Bindy/OpNetworkBundle", order = 4)]
    public class OpNetworkBundle : ScriptableObject
    {
        public int id;

        public string Name
        {
            get { return this.name; }
        }

        [Header("요청 정보")]
        public List<OpNetwork> Items = new List<OpNetwork>();


        public int GetItemsSize() { return Items.Count; }


        public List<OpNetwork> GetItems()
        {
            return Items;
            //return (string[])preQuotes.Clone(); // return readonly array
        }

        private void OnEnable()
        {
            //Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Games/Gizmos/TestSO Icon.png");
            //EditorGUIUtility.SetIconForObject(this, icon);
        }

        public void AddItem() { OpNetwork each = new OpNetwork(); Items.Add(each); }

        public void RemoveItem()
        {
            if (Items.Count > 0)
            {
                int last = Items.Count - 1;

                Items[last] = null;
                Items.RemoveAt(last);
            }
        }

        public void RemoveItem(int index)
        {
            if (Items.Count > 0)
            {
                Items[index] = null;
                Items.RemoveAt(index);
            }
        }


        public void AddItemKey(int i) { Items[i].respKeys.Add(string.Empty); }

        public void RemoveItemKey(int i)
        {
            if (Items[i].respKeys.Count > 0)
            {
                int last = Items[i].respKeys.Count - 1;

                Items[i].respKeys[last] = null;
                Items[i].respKeys.RemoveAt(last);
            }
        }


    }
}
