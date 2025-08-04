using Doozy.Runtime.UIManager.Containers;
using System.Collections.Generic;
using UnityEngine;

namespace Korens.Autonomous
{
    public class OpItemBundleData : MonoBehaviour
    {
        [SerializeField] List<OpItemData> OpItemDatas = new List<OpItemData>();
        public string Category;


        public List<OpItemData> Itemlist { get { return OpItemDatas; } }
        public int GetItemsSize { get { return Itemlist.Count; } }


        private void Start()
        {
        }


        #region using Editor
        [SerializeField] OpItemBundle OpItemBundle;
        [SerializeField] OpItemData OpItemPrefab;

        [Space(10)]
        [Header("해당 ItemData가 보여질 UI의 위치")]
        [Space(5)]
        [SerializeField] UIContainer OpUIView;



        public Transform TM { get { return this.transform; } }
        public OpItemData Prefab { get { return OpItemPrefab; } }
        public OpItemBundle Bundle { set { OpItemBundle = value; } get { return OpItemBundle; } }
        public int BundleId { set { OpItemBundle.id = value; } get { return OpItemBundle.id; } }
        public UIContainer UIView { set { OpUIView = value; } get { return OpUIView; } }


        public void CreateItemByEditor()
        {
            foreach (OpItem each in Bundle.Items)
            {
                OpItemData data = GameObject.Instantiate<OpItemData>(Prefab, Vector3.zero, Quaternion.identity, TM);

                data.ItemData = each;
                data.SetCategory(Category, each.TaskID);
                data.gameObject.name = each.TaskID;

                Itemlist.Add(data);
            }
        }
    }
    #endregion

}