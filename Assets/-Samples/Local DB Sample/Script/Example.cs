using Realms;
using System;
using System.Collections.Concurrent;
using UnityEngine;

public class Example : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        //Debug.LogPing("Start", this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < 5; i++)
            {
                RealmDaseHandler.instance.InsertLocalDBDataForTest("L1PBMX.L1PBMXW220", DateTime.Now);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < 5; i++)
            {
                RealmDaseHandler.instance.InsertLocalDBDataForTest("L1PBMX.L1PBMXW220", DateTime.Now);
            }

            for (int i = 0; i < 5; i++)
            {
                RealmDaseHandler.instance.InsertLocalDBDataForTest("L1PBMX.L1PBMXW220", DateTime.Now.AddDays(1));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < 10; i++)
            {
                RealmDaseHandler.instance.InsertLocalDBDataForTest("L1PBMX.L1PBMXW220", DateTime.Now.AddDays(i));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (int i = 1; i < 11; i++)
            {
                RealmDaseHandler.instance.EnqueueLocalData(new LocalLogUnit("L1PBMX.L1PBMXW220", DateTime.Now, "Enqueue Test " + i.ToString()));
            }
        }
    }

    // No other code directly references the "PreservedMethod" method, so when when stripping is enabled,
    // it will be removed unless the [Preserve] attribute is applied.
    // using UnityEngine.Scripting;
    // [UnityEngine.Scripting.Preserve]
    [Realms.Preserve]
    static void PreservedMethod()
    {
        Debug.Log("PreservedMethod");
    }
}
