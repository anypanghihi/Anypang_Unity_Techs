using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void PrintMessage()
    {
        Debug.Log($"{gameObject.name}: PrintMessage() 실행됨! {Time.time}");
    }

    public void PrintMessage2()
    {
        Debug.Log($"{gameObject.name}: PrintMessage2() 실행됨! {Time.time}");
    }
}