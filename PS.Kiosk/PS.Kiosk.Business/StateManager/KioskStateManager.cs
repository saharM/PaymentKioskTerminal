using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Business.StateManagerBase;
using System.ComponentModel;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Common;

namespace PS.Kiosk.Business
{
    public class KioskStateManager
    {
        private static KioskStateManager _instance;
        private static KioskModel _currentKiosk;
        public KioskModel CurrentKiosk
        {
            get
            {
                return _currentKiosk;
            }
        }
        public static KioskStateManager GetInstance(KioskModel currentKiosk = null)
        {
            if (_instance == null)
            {
                if (currentKiosk != null)
                {
                    _instance = new KioskStateManager(currentKiosk);
                }
                else
                {
                    throw new Exception("NO KIOSK!");
                }
            }
            return _instance;
        }

        private KioskStateManager(KioskModel currentKiosk)
        {
            _currentKiosk = currentKiosk;
        }

        public void Run()
        {
            ThreadManager.RunOnNewThread(_instance.KioskManagerTheardStarter);
        }

        private void KioskManagerTheardStarter()
        {
            // Create StateContext
            IStateContext context = new StateContext(_currentKiosk);

            // Subscribe to state change notifications
            context.OnStateChange += new StateChangeEventHandler<KioskStates>(context_OnStateChange);

            // Run the machine...                        
            while (context.IsActive)
            {
                context.Handle();
            }
        }

        void context_OnStateChange(object sender, StateChangeEventArgs<KioskStates> args)
        {
            KioskLogger.GetInstance().SendMessage(string.Format("State change: {0} -> {1}", args.OldState, args.NewState));
            _currentKiosk.CurrentState = args.NewState;
        }
    }
}
