using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sample.OPDataSetter
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] OpItemDataContainer opItemContainer;

        #region "OP Default View"


        public void SetOpPanelInfo(int id) { opItemContainer.ShowOpPanel(id); }

        #endregion

        #region "Home View"

        #endregion
    }
}