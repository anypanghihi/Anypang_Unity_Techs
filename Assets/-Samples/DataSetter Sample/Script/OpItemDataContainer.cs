using Doozy.Runtime.UIManager.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sample.OPDataSetter
{
    public class OpItemDataContainer : MonoBehaviour
    {
        public static OpItemDataContainer instance;

        public static Dictionary<string, OpItemData> OpDataList = new Dictionary<string, OpItemData>();
        [SerializeField] Dictionary<int, OpItemBundleData> OpBundleList = new Dictionary<int, OpItemBundleData>();

        public Dictionary<int, OpItemBundleData> BundleDatas { get { return OpBundleList; } }
        public Dictionary<UIContainer, List<OpPanelInfoDataSelector>> UIViewDatas { get { return OpViewUIList; } }


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            OpItemBundleData[] bundlelist = GetComponentsInChildren<OpItemBundleData>();


            foreach (OpItemBundleData item in bundlelist)
            {
                OpBundleList.Add(item.Bundle.id, item);

                foreach (OpItemData each in item.Itemlist)
                {
                    OpDataList.Add(each.Key, each);
                }

                List<OpPanelInfoDataSelector> list = item.UIView.transform.GetComponentsInChildren<OpPanelInfoDataSelector>(true).ToList();
                UIViewDatas.Add(item.UIView, list);
            }
        }

        #region using Editor

        [SerializeField] string OpBindyCategory = "OPLine";
        [SerializeField] OpItemBundleData OpBundleDataPrefab;
        [SerializeField] Transform OpViewRootTM;
        [SerializeField] OpPanelInfoDataSelector OpViewInfoPrefab;
        [SerializeField] UIContainer OpViewTMPrefab;

        [SerializeField] List<OpItemBundle> OpItemBundles = new List<OpItemBundle>();
        [SerializeField] Dictionary<UIContainer, List<OpPanelInfoDataSelector>> OpViewUIList = new Dictionary<UIContainer, List<OpPanelInfoDataSelector>>();


        //[SerializeReference, HideInInspector] Transform OpPanelViewTM;


        public string Category { get { return OpBindyCategory; } }
        public List<OpItemBundle> Bundles { get { return OpItemBundles; } }
        public OpItemBundleData BundlePrefab { get { return OpBundleDataPrefab; } }
        public Transform DataTM { get { return this.transform; } }
        public Transform ViewTM { get { return OpViewRootTM; } }
        public UIContainer ViewTMPrefab { get { return OpViewTMPrefab; } }

        public OpPanelInfoDataSelector OpPanelPrefab { get { return OpViewInfoPrefab; } }

        #endregion

        UIContainer prevView = null;
        public void ShowOpPanel(int id)
        {
            if (prevView != null)
            {
                // foreach (var info in UIViewDatas[prevView])
                // {
                //     info.gameObject.SetActive(false);
                // }

                prevView.Hide();
            }

            //List<OpPanelInfoDataSelector> PanelInfo = UIViewDatas[BundleDatas[id].UIView];
            // List<OpItemData> itemlist = BundleDatas[id].Itemlist;
            // for (int i = 0; i < itemlist.Count; i++)
            // {
            //     OpPanelInfoDataSelector selector = PanelInfo[i];

            //     selector.gameObject.SetActive(true);
            //     selector.SetItemData(itemlist[i].ItemData);
            // }
            BundleDatas[id].UIView.Show();

            prevView = BundleDatas[id].UIView;
        }

        public void ShowHomePanel()
        {

        }

    }
}