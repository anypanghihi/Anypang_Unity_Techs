using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowLineUIEditor
{
    public enum FlowLineUIEventType
    {
        // PointEditor
        PointEditorPrev,
        PointEditorNext,
        PointEditorRemove,
        PointEditorUndo,
        PointEditorRedo,

        // LineEditor
        LineEditorPointCountPlus,
        LineEditorPointCountMinus,
        LineEditorPointCountValueChanged,
        LineEditorLineWidthPlus,
        LineEditorLineWidthMinus,
        LineEditorLineWidthValueChanged,
        LineEditorDirectionReverse,
        LineEditorUndo,
        LineEditorRedo,

        // Popup
        PopupCancel,
        PopupConfirm,
        PopupTabScrollAreaSelect,
        PopupTabScrollAreaConfirm,

        // SelectLine
        SelectLineCreate,
        SelectLineSelect,
        SelectLineDeSelect,
        SelectLineRemove,
        SelectLineDeparture,
        SelectLineDestination,

        // Menu
        MenuUpdate,

        // ColorPicker
        ColorPickerLineColorChanged,
        ColorPickerLineHexCodeChanged,
        ColorPickerLineColorReleased,
        ColorPickerPointColorChanged,
        ColorPickerPointHexCodeChanged,
        ColorPickerPointColorReleased,

        None,
    }


    /// <summary>
    /// UI 이벤트 처리자 인터페이스
    /// </summary>
    public interface IFlowLineUIEventHandler
    {
        void HandleEvent(FlowLineUIEventData eventData);
    }

    /// <summary>
    /// FlowLine UI 이벤트 시스템
    /// 기존 static event 방식을 대체하는 interface 기반 시스템
    /// 
    /// 사용 예시:
    /// FlowLineUIEventSystem.RegisterHandler(FlowLineUIEventType.PointEditorPrev, this);
    /// FlowLineUIEventSystem.TriggerEvent(FlowLineUIEventType.PointEditorPrev);
    /// </summary>
    public static class FlowLineUIEventSystem
    {
        private static Dictionary<FlowLineUIEventType, List<IFlowLineUIEventHandler>> eventHandlers =
                   new Dictionary<FlowLineUIEventType, List<IFlowLineUIEventHandler>>();

        /// <summary>
        /// 특정 이벤트 타입에 핸들러를 등록합니다.
        /// </summary>
        public static void AddEvent(FlowLineUIEventType eventType, IFlowLineUIEventHandler handler)
        {
            if (!eventHandlers.ContainsKey(eventType))
                eventHandlers[eventType] = new List<IFlowLineUIEventHandler>();

            if (!eventHandlers[eventType].Contains(handler))
                eventHandlers[eventType].Add(handler);
        }

        /// <summary>
        /// 특정 이벤트 타입에서 핸들러를 제거합니다.
        /// </summary>
        public static void RemoveEvent(FlowLineUIEventType eventType, IFlowLineUIEventHandler handler)
        {
            if (eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType].Remove(handler);
                // 핸들러가 모두 제거되면 Dictionary 키도 삭제
                if (eventHandlers[eventType].Count == 0)
                {
                    eventHandlers.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 이벤트를 발생시킵니다.
        /// </summary>
        public static void TriggerEvent(FlowLineUIEventType eventType, object data = null)
        {
            if (eventHandlers.ContainsKey(eventType))
            {
                var eventData = new FlowLineUIEventData(eventType, data);
                var handlers = eventHandlers[eventType];

                // 핸들러 리스트를 복사하여 이벤트 처리 중 핸들러 변경으로 인한 문제 방지
                var handlersCopy = new List<IFlowLineUIEventHandler>(handlers);

                foreach (var handler in handlersCopy)
                {
                    try
                    {
                        handler.HandleEvent(eventData);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"[FlowLineUIEventSystem] 이벤트 핸들러에서 예외 발생: {eventType}\n{ex}");
                    }
                }
            }

            // 디버그 로그
            //Debug.Log($"[FlowLineUIEventSystem] {eventType} triggered with data: {data}");
        }

        /// <summary>
        /// 여러 이벤트를 한번에 등록합니다.
        /// </summary>
        public static void AddEvent(IFlowLineUIEventHandler handler, params FlowLineUIEventType[] eventTypes)
        {
            foreach (var eventType in eventTypes)
                AddEvent(eventType, handler);
        }

        /// <summary>
        /// 여러 이벤트를 한번에 제거합니다.
        /// </summary>
        public static void RemoveEvent(IFlowLineUIEventHandler handler, params FlowLineUIEventType[] eventTypes)
        {
            foreach (var eventType in eventTypes)
                RemoveEvent(eventType, handler);
        }

        /// <summary>
        /// 모든 핸들러를 제거합니다.
        /// </summary>
        public static void ClearAllHandlers()
        {
            eventHandlers.Clear();
        }

        /// <summary>
        /// 특정 이벤트 타입의 핸들러 수를 반환합니다.
        /// </summary>
        public static int GetHandlerCount(FlowLineUIEventType eventType)
        {
            return eventHandlers.ContainsKey(eventType) ? eventHandlers[eventType].Count : 0;
        }
    }
}