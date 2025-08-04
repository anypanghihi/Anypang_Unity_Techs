using Doozy.Runtime.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sample.OPDataSetter;

namespace Sample.Dashboard
{
    public partial class DashboardManager //.Signal
    {
        SignalStream[] SigStream;
        SignalReceiver[] SigReceiver;

        // Start is called before the first frame update
        void AwakeSignal()
        {
            SigStream = new SignalStream[(int)DashboardCategory.Count];
            SigReceiver = new SignalReceiver[(int)DashboardCategory.Count];

            SigStream[(int)DashboardCategory.TimeGraph] = SignalStream.Get("DASHBOARD", "TimeGraph");
            SigStream[(int)DashboardCategory.SPCGraph] = SignalStream.Get("DASHBOARD", "SPCGraph");
            SigStream[(int)DashboardCategory.BarcodeChart] = SignalStream.Get("DASHBOARD", "BarcodeChart");

            SigReceiver[(int)DashboardCategory.TimeGraph] = new SignalReceiver().SetOnSignalCallback(OnTimeGraphSignal);
            SigReceiver[(int)DashboardCategory.SPCGraph] = new SignalReceiver().SetOnSignalCallback(OnSPCSignal);
            SigReceiver[(int)DashboardCategory.BarcodeChart] = new SignalReceiver().SetOnSignalCallback(OnBarcodeSignal);
        }

        void OnEnableSignal()
        {
            // 해당 Stream 에 대해서 연결 및 해제를 해야 한다.
            SigStream[(int)DashboardCategory.TimeGraph].ConnectReceiver(SigReceiver[(int)DashboardCategory.TimeGraph]);
            SigStream[(int)DashboardCategory.SPCGraph].ConnectReceiver(SigReceiver[(int)DashboardCategory.SPCGraph]);
            SigStream[(int)DashboardCategory.BarcodeChart].ConnectReceiver(SigReceiver[(int)DashboardCategory.BarcodeChart]);
        }

        void OnDisableSignal()
        {
            // 해당 Stream 에 대해서 연결 및 해제를 해야 한다.
            SigStream[(int)DashboardCategory.TimeGraph].DisconnectReceiver(SigReceiver[(int)DashboardCategory.TimeGraph]);
            SigStream[(int)DashboardCategory.SPCGraph].DisconnectReceiver(SigReceiver[(int)DashboardCategory.SPCGraph]);
            SigStream[(int)DashboardCategory.BarcodeChart].DisconnectReceiver(SigReceiver[(int)DashboardCategory.BarcodeChart]);
        }

        void SendTimeGraphSignal(string taskID)
        {
            SigStream[(int)DashboardCategory.TimeGraph].SendSignal<string>(taskID);
            //SignalsService.SendSignal(streamCategory, streamName, "simepleservice");
        }

        void SendSPCSignal(string taskID)
        {
            SigStream[(int)DashboardCategory.SPCGraph].SendSignal<string>(taskID);
        }

        void SendBarcodeSignal(string taskID)
        {
            SigStream[(int)DashboardCategory.BarcodeChart].SendSignal<string>(taskID);
        }


        void OnTimeGraphSignal(Signal signal)
        {
            if (signal.hasValue)
            {
                Type valueType = signal.valueType; //get the payload value type

                //depending on what the value type is (it can be anything) and your use-case below are some examples

                //Example - your value is an int (it can be anything)
                {
                    string signalValue;

                    //Option 1 - get it via a direct cast (unsafe method - fastest)
                    //signalValue = signal.GetValueUnsafe<string>();

                    if (signal.TryGetValue(out signalValue))
                    {
                        SetTimeGraphViewer(signalValue);
                    }
                }
            }
        }

        void OnSPCSignal(Signal signal)
        {
            if (signal.hasValue)
            {
                Type valueType = signal.valueType; //get the payload value type

                //depending on what the value type is (it can be anything) and your use-case below are some examples

                //Example - your value is an int (it can be anything)
                {
                    string signalValue;

                    //Option 1 - get it via a direct cast (unsafe method - fastest)
                    //signalValue = signal.GetValueUnsafe<string>();

                    if (signal.TryGetValue(out signalValue))
                    {
                        SetSPCViewer(signalValue);
                    }
                }
            }
        }

        void OnBarcodeSignal(Signal signal)
        {
            if (signal.hasValue)
            {
                Type valueType = signal.valueType; //get the payload value type

                //depending on what the value type is (it can be anything) and your use-case below are some examples

                //Example - your value is an int (it can be anything)
                {
                    string signalValue;

                    //Option 1 - get it via a direct cast (unsafe method - fastest)
                    //signalValue = signal.GetValueUnsafe<string>();

                    if (signal.TryGetValue(out signalValue))
                    {
                        SetBarcodeViewer(signalValue);
                    }
                }
            }
        }
    }
}
