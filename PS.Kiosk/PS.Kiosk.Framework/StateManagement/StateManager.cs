using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Framework.StateManagerBase;
using System.ComponentModel;

namespace PS.Kiosk.Framework
{
    public class StateManager
    {
        private static StateManager _instance;
        public static StateManager Instance
        {
            get
            {
                return _instance;
            }
        }
        private IStatable _current;
        public IStatable Current
        {
            get
            {
                return _current;
            }
        }

        public static bool Start(IStatable statable)
        {
            if (_instance == null)
            {
                if (statable != null)
                {
                    _instance = new StateManager(statable);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private StateManager(IStatable current)
        {
            _current = current;
        }

        public void Run()
        {
            ThreadManager.RunOnNewThread(_instance.KioskManagerTheardStarter);
        }

        private void KioskManagerTheardStarter()
        {
            // Create StateContext
            IStateContext context = new StateContext(_current);

            // Subscribe to state change notifications
            context.OnStateChange += new StateChangeEventHandler<States>(context_OnStateChange);

            // Run the machine...                        
            while (context.IsActive)
            {
                context.Handle();
            }
        }

        void context_OnStateChange(object sender, StateChangeEventArgs<States> args)
        {
            KioskLogger.Instance.LogMessage(string.Format("State change: {0} -> {1}", args.OldState, args.NewState));
            _current.CurrentState = args.NewState;
            _current.PreviousState = args.OldState;
            
            _current.ClearStateProperties();
        }
    }
}
