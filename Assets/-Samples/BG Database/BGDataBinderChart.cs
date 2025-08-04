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
        Debug.LogWarning("데이타 읽기 완료 ==> " + json);
    }


 
}
