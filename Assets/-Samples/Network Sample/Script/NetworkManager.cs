using Sample.OPDataSetter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sample.Network
{
    [System.Serializable]
    public class OPNormalData
    {
        public string VALUE;

        public override string ToString()
        {
            return VALUE;
        }
    }

    [System.Serializable]
    public class OPNgCountData
    {
        public string SUM_CNT;

        public override string ToString()
        {
            return SUM_CNT;
        }
    }

    public partial class NetworkManager : MonoBehaviour
    {
        public static NetworkManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GetOPDataAsync().Forget();
        }

        [SerializeField] private TextMeshProUGUI apiText;
    }
}