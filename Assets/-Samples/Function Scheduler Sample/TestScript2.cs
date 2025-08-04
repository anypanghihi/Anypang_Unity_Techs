using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    public void PrintMessage3()
    {
        Debug.Log($"{gameObject.name}: PrintMessage3() 실행됨! {Time.time}");
    }
}
