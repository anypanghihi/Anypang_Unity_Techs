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
            // Realms Debug.LogDB 에 저장
#endif

        }
        public static void LogError(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#else
            // Realms Debug.LogDB 에 저장
#endif        
        }
        public static void LogError(Exception message) 
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#else
            // Realms Debug.LogDB 에 저장
#endif        
        }
        public static void LogException(Exception message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogException(message);
#else
            // Realms Debug.LogDB 에 저장
#endif        
        }
        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#else
            // Realms Debug.LogDB 에 저장
#endif        
        }


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogPing(string message, UnityEngine.MonoBehaviour monoComponent)
        {
            /// 주의: 이 메시지 출력 이후의 코드는 실행되지 않는다.            
            throw new Exception($"{message} from  [{monoComponent.gameObject.name}]의 [{monoComponent.GetType().Name}] \n");            
        }
    }



}
