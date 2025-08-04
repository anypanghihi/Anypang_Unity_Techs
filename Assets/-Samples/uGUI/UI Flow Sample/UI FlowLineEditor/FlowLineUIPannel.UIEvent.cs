using System;
using System.Collections.Generic;
using UnityEngine;
using FlowLineUIEditor.ColorPicker;
using FlowLineUIEditor.LineList;

namespace FlowLineUIEditor.Pannel
{
    public partial class FlowLineUIPannel : IFlowLineUIEventHandler// UIEvent
    {
        private Dictionary<FlowLineUIEventType, Action<FlowLineUIEventData>> eventHandlers;

        private void InitializeUIEvent()
        {
            InitializeEventHandlers();

            // 이벤트 시스템 등록
            FlowLineUIEventSystem.AddEvent(this,

                // Popup 이벤트
                FlowLineUIEventType.PopupCancel,
                FlowLineUIEventType.PopupConfirm,
                FlowLineUIEventType.PopupTabScrollAreaConfirm,
                // Menu 이벤트
                FlowLineUIEventType.MenuUpdate,
                // SelectLine 이벤트
                FlowLineUIEventType.SelectLineRemove,
                FlowLineUIEventType.SelectLineCreate,
                FlowLineUIEventType.SelectLineSelect,
                FlowLineUIEventType.SelectLineDeSelect,
                FlowLineUIEventType.SelectLineDeparture,
                FlowLineUIEventType.SelectLineDestination,
                // PointEditor 이벤트
                FlowLineUIEventType.PointEditorPrev,
                FlowLineUIEventType.PointEditorNext,
                FlowLineUIEventType.PointEditorRemove,
                FlowLineUIEventType.PointEditorUndo,
                FlowLineUIEventType.PointEditorRedo,
                // LineEditor 이벤트
                FlowLineUIEventType.LineEditorPointCountPlus,
                FlowLineUIEventType.LineEditorPointCountMinus,
                FlowLineUIEventType.LineEditorPointCountValueChanged,
                FlowLineUIEventType.LineEditorLineWidthPlus,
                FlowLineUIEventType.LineEditorLineWidthMinus,
                FlowLineUIEventType.LineEditorLineWidthValueChanged,
                FlowLineUIEventType.LineEditorDirectionReverse,
                FlowLineUIEventType.LineEditorUndo,
                FlowLineUIEventType.LineEditorRedo,
                // ColorPicker 이벤트
                FlowLineUIEventType.ColorPickerLineColorChanged,
                FlowLineUIEventType.ColorPickerLineHexCodeChanged,
                FlowLineUIEventType.ColorPickerLineColorReleased,
                FlowLineUIEventType.ColorPickerPointColorChanged,
                FlowLineUIEventType.ColorPickerPointHexCodeChanged,
                FlowLineUIEventType.ColorPickerPointColorReleased
                );
        }

        private void UnInitializeUIEvent()
        {
            // 이벤트 시스템 해제
            FlowLineUIEventSystem.RemoveEvent(this,

                // Popup 이벤트
                FlowLineUIEventType.PopupCancel,
                FlowLineUIEventType.PopupConfirm,
                FlowLineUIEventType.PopupTabScrollAreaConfirm,
                // Menu 이벤트
                FlowLineUIEventType.MenuUpdate,
                // SelectLine 이벤트
                FlowLineUIEventType.SelectLineRemove,
                FlowLineUIEventType.SelectLineCreate,
                FlowLineUIEventType.SelectLineSelect,
                FlowLineUIEventType.SelectLineDeSelect,
                FlowLineUIEventType.SelectLineDeparture,
                FlowLineUIEventType.SelectLineDestination,
                // PointEditor 이벤트
                FlowLineUIEventType.PointEditorPrev,
                FlowLineUIEventType.PointEditorNext,
                FlowLineUIEventType.PointEditorRemove,
                FlowLineUIEventType.PointEditorUndo,
                FlowLineUIEventType.PointEditorRedo,
                // LineEditor 이벤트
                FlowLineUIEventType.LineEditorPointCountPlus,
                FlowLineUIEventType.LineEditorPointCountMinus,
                FlowLineUIEventType.LineEditorPointCountValueChanged,
                FlowLineUIEventType.LineEditorLineWidthPlus,
                FlowLineUIEventType.LineEditorLineWidthMinus,
                FlowLineUIEventType.LineEditorLineWidthValueChanged,
                FlowLineUIEventType.LineEditorDirectionReverse,
                FlowLineUIEventType.LineEditorUndo,
                FlowLineUIEventType.LineEditorRedo,
                // ColorPicker 이벤트
                FlowLineUIEventType.ColorPickerLineColorChanged,
                FlowLineUIEventType.ColorPickerLineHexCodeChanged,
                FlowLineUIEventType.ColorPickerLineColorReleased,
                FlowLineUIEventType.ColorPickerPointColorChanged,
                FlowLineUIEventType.ColorPickerPointHexCodeChanged,
                FlowLineUIEventType.ColorPickerPointColorReleased
                );


            // Dictionary 정리
            eventHandlers?.Clear();
        }

        private void InitializeEventHandlers()
        {
            eventHandlers = new Dictionary<FlowLineUIEventType, Action<FlowLineUIEventData>>
            {
                // Popup 이벤트
                { FlowLineUIEventType.PopupCancel,      (data) => { OnPopupCancel(); HidePopup(); } },
                { FlowLineUIEventType.PopupConfirm,     (data) => { OnPopupConfirm(); HidePopup(); } },
                { FlowLineUIEventType.PopupTabScrollAreaConfirm, (data) =>
                    {
                        if (data.IsDataOfType<PopupFlowLineData>())
                        {
                            var flowData = data.GetDataAs<PopupFlowLineData>();
                            // 예시: SelectLineView에 데이터 반영                            

                            selectLineView.SetFlowLineData(flowData.sId, flowData.areaText, flowData.areaText);
                        }
                    }
                },
               

                // Menu 이벤트
                { FlowLineUIEventType.MenuUpdate,   (data) =>
                    {
                        if (data.IsDataOfType<int>())
                            ShowUpdatePopup(data.GetDataAs<int>());
                        else
                            Debug.LogWarning($"[MenuUpdate] 잘못된 데이터 타입: {data}");
                    }
                },
                
                // SelectLine 이벤트
                { FlowLineUIEventType.SelectLineCreate,   (data) => lineListView.AddScrollItem() },
                { FlowLineUIEventType.SelectLineSelect,   (data) =>
                    {
                        if (data.IsDataOfType<int>())
                        {
                            SelectScrollItem(data.GetDataAs<int>());
                        }
                        else
                            Debug.LogWarning($"[SelectLineSelect] 잘못된 데이터 타입: {data}");
                    }
                },
                { FlowLineUIEventType.SelectLineDeSelect, (data) => DeSelectScrollItem() },
                { FlowLineUIEventType.SelectLineRemove,   (data) =>
                    {
                        if (data.IsDataOfType<int>())
                            ShowDeletePopup(data.GetDataAs<int>());
                        else
                            Debug.LogWarning($"[SelectLineRemove] 잘못된 데이터 타입: {data}");
                    }
                },
                { FlowLineUIEventType.SelectLineDeparture, (data) =>
                    {
                        popupPanel.ShowAreaListScroll();
                    }
                },
                { FlowLineUIEventType.SelectLineDestination, (data) =>
                    {
                        popupPanel.ShowAreaListScroll();
                    }
                },
                
                // PointEditor 이벤트
                { FlowLineUIEventType.PointEditorPrev,    (data) => Debug.Log("[PointEditor] Prev 버튼 클릭됨") },
                { FlowLineUIEventType.PointEditorNext,    (data) => Debug.Log("[PointEditor] Next 버튼 클릭됨") },
                { FlowLineUIEventType.PointEditorRemove,  (data) => Debug.Log("[PointEditor] Remove 버튼 클릭됨") },
                { FlowLineUIEventType.PointEditorUndo,    (data) => Debug.Log("[PointEditor] Undo 버튼 클릭됨 - 실제 Undo 처리는 다른 모듈에서 구현") },
                { FlowLineUIEventType.PointEditorRedo,    (data) => Debug.Log("[PointEditor] Redo 버튼 클릭됨 - 실제 Redo 처리는 다른 모듈에서 구현") },
                
                // LineEditor 이벤트
                { FlowLineUIEventType.LineEditorPointCountPlus, (data) => Debug.Log("[LineEditor] PointCount Plus 버튼 클릭됨") },
                { FlowLineUIEventType.LineEditorPointCountMinus, (data) => Debug.Log("[LineEditor] PointCount Minus 버튼 클릭됨") },
                { FlowLineUIEventType.LineEditorPointCountValueChanged, (data) =>
                    {
                        if (data.IsDataOfType<int>())
                        {
                            var pointCountValue = data.GetDataAs<int>();
                            Debug.Log($"[LineEditor] PointCount 값 변경됨: {pointCountValue}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[LineEditorPointCountValueChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.LineEditorLineWidthPlus, (data) => Debug.Log("[LineEditor] LineWidth Plus 버튼 클릭됨") },
                { FlowLineUIEventType.LineEditorLineWidthMinus, (data) => Debug.Log("[LineEditor] LineWidth Minus 버튼 클릭됨") },
                { FlowLineUIEventType.LineEditorLineWidthValueChanged, (data) =>
                    {
                        if (data.IsDataOfType<int>())
                        {
                            var lineWidthValue = data.GetDataAs<int>();
                            Debug.Log($"[LineEditor] LineWidth 값 변경됨: {lineWidthValue}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[LineEditorLineWidthValueChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.LineEditorDirectionReverse, (data) => Debug.Log("[LineEditor] Direction Reverse 버튼 클릭됨") },
                { FlowLineUIEventType.LineEditorUndo, (data) => Debug.Log("[LineEditor] Undo 버튼 클릭됨 - 실제 Undo 처리는 다른 모듈에서 구현") },
                { FlowLineUIEventType.LineEditorRedo, (data) => Debug.Log("[LineEditor] Redo 버튼 클릭됨 - 실제 Redo 처리는 다른 모듈에서 구현") },
                
                // ColorPicker 이벤트
                { FlowLineUIEventType.ColorPickerLineColorChanged, (data) =>
                    {
                        if (data.IsDataOfType<ColorData>())
                        {
                            var colorData = data.GetDataAs<ColorData>();
                            Debug.Log($"[ColorPicker] Line Color 변경됨: {colorData}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerLineColorChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },

                { FlowLineUIEventType.ColorPickerLineHexCodeChanged, (data) =>
                    {
                        if (data.IsDataOfType<string>())
                        {
                            var hexCode = data.GetDataAs<string>();
                            Debug.Log($"[ColorPicker] Line Hex Code 변경됨: {hexCode}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerLineHexCodeChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.ColorPickerLineColorReleased, (data) =>
                    {
                        if (data.IsDataOfType<ColorData>())
                        {
                            var colorData = data.GetDataAs<ColorData>();
                            Debug.Log($"[ColorPicker] Line Color 세팅됨: {colorData}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerLineColorReleased] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.ColorPickerPointColorChanged, (data) =>
                    {
                        if (data.IsDataOfType<ColorData>())
                        {
                            var colorData = data.GetDataAs<ColorData>();
                            Debug.Log($"[ColorPicker] Point Color 변경됨: {colorData}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerPointColorChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.ColorPickerPointHexCodeChanged, (data) =>
                    {
                        if (data.IsDataOfType<string>())
                        {
                            var hexCode = data.GetDataAs<string>();
                            Debug.Log($"[ColorPicker] Point Hex Code 변경됨: {hexCode}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerPointHexCodeChanged] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
                { FlowLineUIEventType.ColorPickerPointColorReleased, (data) =>
                    {
                        if (data.IsDataOfType<ColorData>())
                        {
                            var colorData = data.GetDataAs<ColorData>();
                            Debug.Log($"[ColorPicker] Point Color 세팅됨: {colorData}");
                            // 실제 처리 로직 추가
                        }
                        else
                        {
                            Debug.LogWarning($"[ColorPickerPointColorReleased] 잘못된 데이터 타입: {data}");
                        }
                    }
                },
            };
        }

        public void HandleEvent(FlowLineUIEventData eventData)
        {
            if (eventHandlers != null && eventHandlers.TryGetValue(eventData.eventType, out var handler))
            {
                handler(eventData);
            }
            else
            {
                Debug.LogWarning($"[FlowLineUIPannel] 처리되지 않은 이벤트: {eventData.eventType}");
            }
        }
    }

}