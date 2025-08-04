using System;

namespace FlowLineUIEditor
{
    /// <summary>
    /// FlowLineUI 이벤트 데이터를 담는 구조체
    /// </summary>
    public struct FlowLineUIEventData
    {
        public FlowLineUIEventType eventType;
        public object data;

        public FlowLineUIEventData(FlowLineUIEventType eventType, object data = null)
        {
            this.eventType = eventType;
            this.data = data;
        }
        
        /// <summary>
        /// 데이터가 특정 타입인지 안전하게 확인합니다.
        /// </summary>
        public bool IsDataOfType<T>()
        {
            return data is T;
        }
        
        /// <summary>
        /// 데이터를 특정 타입으로 안전하게 캐스팅합니다.
        /// </summary>
        public T GetDataAs<T>()
        {
            if (data is T result)
                return result;
            return default(T);
        }
        
        /// <summary>
        /// 디버깅을 위한 문자열 표현
        /// </summary>
        public override string ToString()
        {
            return $"EventType: {eventType}, Data: {data?.ToString() ?? "null"} ({data?.GetType().Name ?? "null"})";
        }
    }
} 