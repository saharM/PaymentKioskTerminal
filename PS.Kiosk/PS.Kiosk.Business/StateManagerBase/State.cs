using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Business.StateManagerBase
{
    /// <summary>
    /// Base class for states
    /// </summary>
    /// <typeparam name="T">Derived from StateContext</typeparam>
    /// <typeparam name="U">Enum of states</typeparam>
    public abstract class State<T, U> : StateBase<U> where T : StateContext<U>
    {
        private T m_Context;

        /// <summary>
        /// Used by StateContext to set context after construction
        /// </summary>
        /// <param name="context"></param>
        internal override void SetContext(StateContext<U> context)
        {
            m_Context = (T)context;
        }

        /// <summary>
        /// Used to access StateContext
        /// </summary>
        protected T Context
        {
            get
            {
                return m_Context;
            }
        }
    }
}
