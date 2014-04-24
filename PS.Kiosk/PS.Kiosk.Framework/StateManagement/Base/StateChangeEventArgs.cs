using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Framework.StateManagerBase
{
    /// <summary>
    /// EventArgs for the StateChange event. 
    /// Contains the old and new states.
    /// </summary>
    /// <typeparam name="T">Enum of states</typeparam>
    public class StateChangeEventArgs<T>
    {
        private T m_OldState;
        private T m_NewState;

        /// <summary>
        /// Previous active state
        /// </summary>
        public T OldState
        {
            get
            {
                return m_OldState;
            }
        }

        /// <summary>
        /// New active state
        /// </summary>
        public T NewState
        {
            get
            {
                return m_NewState;
            }
        }

        internal StateChangeEventArgs(T oldState, T newState)
        {
            m_OldState = oldState;
            m_NewState = newState;
        }
    }

    /// <summary>
    /// Delegate used for notifying StateChange events
    /// </summary>
    /// <typeparam name="T">Enum of states</typeparam>
    /// <param name="sender">StateContext</param>
    /// <param name="args">StateChangeEventArgs</param>
    public delegate void StateChangeEventHandler<T>(object sender, StateChangeEventArgs<T> args);
}
