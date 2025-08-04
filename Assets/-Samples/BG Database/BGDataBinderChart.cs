using UnityEngine;

public class BGDataBinderChart : MonoBehaviour
{
    string json;

    public string ChartDATA
    {
        get=> json;
        set
        {
            json = value;
            readData();
        }
    }

    void readData()
    {
        Debug.LogWarning("json ==> " + json);
    }


 
}
