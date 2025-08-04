using Doozy.Runtime.UIManager.Containers;
using System.Collections.Generic;
using UnityEngine;

namespace Korens.Autonomous
{
    public class OpItemDataContainer : MonoBehaviour
    {
        public static OpItemDataContainer instance;

        public static Dictionary<string, OpItemData> OpDataDict = new Dictionary<string, OpItemData>();
        [SerializeField] private Dictionary<int, OpItemBundleData> OpBundleDict = new Dictionary<int, OpItemBundleData>();

        public Dictionary<int, OpItemBundleData> BundleDatas { get { return OpBundleDict; } }
        //public Dictionary<UIContainer, List<OpPanelInfoDataBase>> UIViewDatas { get { return OpViewUIList; } }


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            OpItemBundleData[] bundlelist = GetComponentsInChildren<OpItemBundleData>();

            foreach (OpItemBundleData item in bundlelist)
            {
                OpBundleDict.Add(item.Bundle.id, item);

                foreach (OpItemData each in item.Itemlist)
                {
                    OpDataDict.Add(each.Key, each);
                }

                // List<OpPanelInfoDataBase> list = item.UIView.transform.GetComponentsInChildren<OpPanelInfoDataBase>(true).ToList();
                // UIViewDatas.Add(item.UIView, list);
            }
        }

        #region using Editor

        [SerializeField] string OpBindyCategory = "OPLine";
        [SerializeField] OpItemBundleData OpBundleDataPrefab;
        [SerializeField] Transform OpViewRootTM;
        [SerializeField] OpPanelInfoDataBase OpViewInfoPrefab;
        [SerializeField] UIContainer OpViewTMPrefab;

        [SerializeField] List<OpItemBundle> OpItemBundles = new List<OpItemBundle>();
        //[SerializeField] Dictionary<UIContainer, List<OpPanelInfoDataBase>> OpViewUIList = new Dictionary<UIContainer, List<OpPanelInfoDataBase>>();


        //[SerializeReference, HideInInspector] Transform OpPanelViewTM;


        public string Category { get { return OpBindyCategory; } }
        public List<OpItemBundle> Bundles { get { return OpItemBundles; } }
        public OpItemBundleData BundlePrefab { get { return OpBundleDataPrefab; } }
        public Transform DataTM { get { return this.transform; } }
        public Transform ViewTM { get { return OpViewRootTM; } }
        public UIContainer ViewTMPrefab { get { return OpViewTMPrefab; } }

        public OpPanelInfoDataBase OpPanelPrefab { get { return OpViewInfoPrefab; } }

        #endregion

        UIContainer prevView = null;
        public void ShowOpPanel(int id)
        {
            if (prevView != null)
            {
                prevView.Hide();
                // foreach (var info in UIViewDatas[prevView])
                // {
                //     info.gameObject.SetActive(false);
                // }
            }

            // List<OpPanelInfoDataSelector> PanelInfo = UIViewDatas[BundleDatas[id].UIView];
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

        /// <summary>
        ///  for debug
        /// </summary>
        /// <param name="opID"></param>
        public void ShowOpPanel(GameObject opID)
        {
            if (int.TryParse(opID.name, out int id))
            {
                if (prevView != null)
                {
                    prevView.Hide();
                    // foreach (var info in UIViewDatas[prevView])
                    // {
                    //     info.gameObject.SetActive(false);
                    // }
                }

                // List<OpPanelInfoDataSelector> PanelInfo = UIViewDatas[BundleDatas[id].UIView];
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
        }

        /// <summary>
        ///  for debug
        /// </summary>
        /// <param name="opID"></param>
        public void ShowOpPanelFord(GameObject opID)
        {
            if (int.TryParse(opID.name, out int id))
            {
                if (prevView != null)
                {
                    prevView.gameObject.SetActive(false);
                    // foreach (var info in UIViewDatas[prevView])
                    // {
                    //     info.gameObject.SetActive(false);
                    // }
                }

                // List<OpPanelInfoDataSelector> PanelInfo = UIViewDatas[BundleDatas[id].UIView];
                // List<OpItemData> itemlist = BundleDatas[id].Itemlist;
                // for (int i = 0; i < itemlist.Count; i++)
                // {
                //     OpPanelInfoDataSelector selector = PanelInfo[i];

                //     selector.gameObject.SetActive(true);
                //     selector.SetItemData(itemlist[i].ItemData);
                // }

                BundleDatas[id].UIView.gameObject.SetActive(true);

                prevView = BundleDatas[id].UIView;
            }
        }

        public void ShowHomePanel()
        {

        }

    }
}