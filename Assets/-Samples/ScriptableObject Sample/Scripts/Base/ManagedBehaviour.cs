using System;
using UnityEngine;


public abstract class ManagedBehaviour : MonoBehaviour
{
    [SerializeField] ManagedBehaviourSO ManagedSO;
       

    public virtual void OnEnable()
    {     
        ManagedSO.Register(ManagedUpdate);
    }

    public virtual void OnDisable()
    {
        ManagedSO.UnRegister(ManagedUpdate);
    }

    public abstract void ManagedUpdate();
}

