using System;
using UnityEngine;


namespace Structs
{
    [Serializable]
    public struct PRS
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private Quaternion rot;
        [SerializeField] private Vector3 scl;

        public Vector3    position { set { pos = value; } get { return pos; } }
        public Quaternion rotation { set { rot = value; } get { return rot; } }
        public Vector3    scale    { set { scl = value; } get { return scl; } }

        public PRS(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            pos = position;
            rot = rotation;
            scl = scale;            
        }

        //public PRS()
        //{
        //    pos = Vector3.zero;
        //    rot = Quaternion.identity;
        //    scl = Vector3.one;
        //}
    }
}