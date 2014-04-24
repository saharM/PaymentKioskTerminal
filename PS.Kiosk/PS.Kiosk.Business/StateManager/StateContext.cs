using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Business.StateManagerBase;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Common;

namespace PS.Kiosk.Business
{
    interface IStateContext : IStateContext<KioskStates> { }

    class StateContext : StateContext<KioskStates>, IStateContext
    {
        private IKiosk m_Kiosk;

        public StateContext(IKiosk kiosk)
            : base(KioskStates.ConnectingState, KioskStates.EmptyState)
        {
            kiosk.OnConnect += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            kiosk.OnConnectingError += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            kiosk.OnGetCard += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            m_Kiosk = kiosk;
        }

        public IKiosk Kiosk
        {
            get { return m_Kiosk; }
        }
    }
}
