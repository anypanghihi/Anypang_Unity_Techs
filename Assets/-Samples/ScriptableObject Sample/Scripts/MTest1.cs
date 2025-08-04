using Unity.VisualScripting;
using UnityEngine;


public class MTest1 : ManagedBehaviour
{
    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("OnEnable " + this.name);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("OnDisable " + this.name);
    }


    public void Update()
    {
        Debug.Log("O update " + this.name);
    }

    public override void ManagedUpdate()
    {
        Debug.Log("M update " + this.name);
    }
}