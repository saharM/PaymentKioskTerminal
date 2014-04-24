using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.Business
{    
    class ConnectingState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(30);
            Context.Kiosk.Connect();
        }

        public override void OnConnect()
        {
            Context.Next = KioskStates.WaitingState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = KioskStates.ConnectingErrorState;
        }

        public override void OnConnectingError()
        {
            Context.Next = KioskStates.ConnectingErrorState;
        }
    }
    
    class ConnectingErrorState : StateBase
    {
        public override void EntryAction()
        {
            Context.Kiosk.ConnectingError();
        }        
    }

    class WaitingState : StateBase
    {
        public override void EntryAction()
        {
            Context.Kiosk.Waiting();
        }

        public override void OnGetCard()
        {
            Context.Next = KioskStates.GettingPinState;
        }
    }

    class GettingPinState : StateBase
    {
        public override void EntryAction()
        {
            string s = string.Empty;
        }
    }

}
