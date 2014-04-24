using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Business
{
    public class TriggerEventArgs : EventArgs
    {
        public TriggerEventArgs(string eventName)
        {
            this.EventName = eventName;
        }
        public string EventName;

    }

    public delegate void TriggerEventHandler(object sender, TriggerEventArgs te);
}
