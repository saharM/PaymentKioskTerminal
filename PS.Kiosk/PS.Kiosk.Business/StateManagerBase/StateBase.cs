using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common;

namespace PS.Kiosk.Business.StateManagerBase
{
    /// <summary>
    /// Base class of State
    /// </summary>
    /// <typeparam name="T">Enum of states</typeparam>
    public abstract class StateBase<T>
    {
        /// <summary>
        /// Used by StateContext to set context after construction
        /// </summary>
        /// <param name="context"></param>
        internal abstract void SetContext(StateContext<T> context);

        /// <summary>
        /// Constructs a StateBase instance
        /// </summary>
        public StateBase() { }

        /// <summary>
        /// Custom ToString
        /// </summary>
        /// <returns>State name</returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Override this function to define an Entry action
        /// </summary>
        public virtual void EntryAction() { }

        /// <summary>
        /// Override this function to define a Do action
        /// </summary>
        public virtual void DoAction() { }

        /// <summary>
        /// Override this function to define a Exit action
        /// </summary>
        public virtual void ExitAction() { }

        /// <summary>
        /// Override this function to handle triggers.
        /// </summary>
        /// <param name="e">Dequeued eventargs</param>
        public virtual void TriggerHandler(TriggerEventArgs e) { }

        /// <summary>
        /// Override this function to handle timeouts
        /// Unhandled timeouts throw a NotImplementedException ("Unhandled timout")
        /// </summary>
        public virtual void TimeoutHandler()
        {
            throw new NotImplementedException("Unhandled timeout");
        }
    }
}
