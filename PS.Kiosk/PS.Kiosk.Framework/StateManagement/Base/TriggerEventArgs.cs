using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Framework
{
    public class TriggerEventArgs : EventArgs
    {
        public TriggerEventArgs(TriggerEvents raisedEvent)
        {
            this.RaisedEvent = raisedEvent;
        }
        public TriggerEvents RaisedEvent;

    }

    public delegate void TriggerEventHandler(object sender, TriggerEventArgs te);
}
