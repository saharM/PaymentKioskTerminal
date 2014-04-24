using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Business.StateManagerBase;
using System.Reflection;
using PS.Kiosk.Common;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.Business
{
    abstract class StateBase : State<StateContext, KioskStates>
    {
        public override void TriggerHandler(TriggerEventArgs e)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(e.EventName);
            theMethod.Invoke(this, new object[] {});
        }

        public virtual void OnConnect() { }
        public virtual void OnConnectingError() { }
        public virtual void OnGetCard() { }
    }

}
