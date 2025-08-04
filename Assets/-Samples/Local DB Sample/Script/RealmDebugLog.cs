global using Debug = Realms.Debug;
using System;

namespace Realms
{
    public static class Debug
    {        
        public static void Log(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#else
            // Realms Debug.LogDB �� ����
#endif

        }
        public static void LogError(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#else
            // Realms Debug.LogDB �� ����
#endif        
        }
        public static void LogError(Exception message) 
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#else
            // Realms Debug.LogDB �� ����
#endif        
        }
        public static void LogException(Exception message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(message);
#else
            // Realms Debug.LogDB �� ����
#endif        
        }
        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#else
            // Realms Debug.LogDB �� ����
#endif        
        }


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogPing(string message, UnityEngine.MonoBehaviour monoComponent)
        {
            /// ����: �� �޽��� ��� ������ �ڵ�� ������� �ʴ´�.            
            throw new Exception($"{message} from  [{monoComponent.gameObject.name}]�� [{monoComponent.GetType().Name}] \n");            
        }
    }



}
