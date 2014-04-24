using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Framework.StateManagerBase;

namespace PS.Kiosk.Framework
{
    interface IStateContext : IStateContext<States> { }

    class StateContext : StateContext<States>, IStateContext
    {
        private IStatable m_Statable;

        public StateContext(IStatable statable)
            : base(States.ConnectingState, States.EmptyState)
        {
            statable.OnConnected += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnError += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnReceivedCard += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnFinishingSession += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnAchievedPin += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnGetBalanceInquiryAction += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnExitAction += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowChargeType += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowIrancellChargeType += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnGetCharge += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowHamrahAvalChargeType += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowTaliaChargeType += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowRightelChargeType += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowWaiting += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowBusy += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnPrinting += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnPayBill += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowPayBill += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowJiring += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowIrancellServices += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowIrancellServicePayment += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnPaySpeciallServices += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowMobinetPayment += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowHamrahAvalServices += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnReturn += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowHamrahAvalTopUp += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowHamarahAvalBill += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnShowHamrahAvalBillInfo += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            statable.OnRestart += new TriggerEventHandler(this.EnqueueTriggerEventHandler);
            m_Statable = statable;
        }

        public IStatable Statable
        {
            get { return m_Statable; }
        }
    }
}
