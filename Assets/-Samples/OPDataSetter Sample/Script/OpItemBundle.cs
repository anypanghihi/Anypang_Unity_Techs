using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sample.OPDataSetter
{
    public enum DashboardCategory
    {
        None,
        TimeGraph = 0,
        SPCGraph = 1,
        BarcodeChart = 2,
        Count = 3
    }

    [Serializable]
    public class OpItem
    {
        // 해당 항목을 보여줄건지..
        public bool Show = true;

        /// API 데이터베이스에서 해당 공정의 정보를 탐색할 때 사용될 ID.
        /// ex) OP10_ProdJud1
        public string TaskID;

        /// 해당 공정의 OP 번호.
        /// ex) 10
        public int OPID;

        /// 해당 공정이 표시될 오브젝트의 타입.
        /// 0 : Spec 있음 (Type 1), 1 : Spec 없음 (Type 2), 2 : Value 없음, Spec 있음 (Type 3)
        public int BoxType;
        //public DataBoxType BoxType;

        /// 해당 공정의 대시보드 보유 유무.
        public bool HasDashboard;

        /// 해당 공정의 대시보드 종류.
        public DashboardCategory DashboardCategory;

        /// 해당 공정의 API 주소.
        public string APILink;

        /// 해당 공정이 표시될 이름.
        /// ex) 비전검사(좌)
        public string ProcessName;

        /// 해당 공정의 데이터가 표시될 string format.
        /// ex) {0:N2} bar, 1:양품,2:불량
        public string ProcessFormat = "{0:N0}";

        /// 해당 공정의 데이터가 표시될 string format.
        /// ex) {0:N2} bar, 1:양품,2:불량
        public string ProcessUnit;

        /// 해당 공정에 곱해질 값.
        /// ex) 1, 0.01
        public float ProcessScale = 1.0f;

        /// SPEC 값 존재 유무
        public bool HasSPEC;

        /// SPEC의 하한치.
        public float MinSPECValue;

        /// SPEC의 상한치.
        public float MaxSPECValue;

    }



    [CreateAssetMenu(menuName = "Doozy/Bindy/OpItemBundle", order = 4)]
    public class OpItemBundle : ScriptableObject
    {
        public int id;

        public string Name
        {
            get { return this.name; }
        }

        [Header("관리데이타")]
        public List<OpItem> Items = new List<OpItem>();


        public int GetItemsSize() { return Items.Count; }


        public List<OpItem> GetItems()
        {
            return Items;
            //return (string[])preQuotes.Clone(); // return readonly array
        }


        public void AddItem() { OpItem each = new OpItem(); Items.Add(each); }

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

    }
}
