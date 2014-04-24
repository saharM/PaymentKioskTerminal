using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PS.Kiosk.Framework.StateManagerBase
{
    /// <summary>
    /// Generic StateContext interface
    /// </summary>
    /// <typeparam name="T">Enum of states</typeparam>
    public interface IStateContext<T>
    {
        /// <summary>
        /// Current state, readonly
        /// </summary>
        T State { get; }

        /// <summary>
        /// Event that will be fired upon state changes
        /// </summary>
        event StateChangeEventHandler<T> OnStateChange;

        /// <summary>
        /// Returns true while the last/exit state is not reached yet
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// State handler, to be called repeatedly while statecontext is active
        /// </summary>
        void Handle();
    }

    /// <summary>
    /// Generic StateContext 
    /// </summary>
    /// <typeparam name="T">Enum of states</typeparam>
    public abstract class StateContext<T> : IStateContext<T>
    {
        private StateBase<T> m_StateObject = null;
        private StateBase<T> m_NextStateObject = null;
        private Queue<TriggerEventArgs> m_TriggerEvents = new Queue<TriggerEventArgs>();
        private DateTime m_Deadline = DateTime.MaxValue;
        private Dictionary<T, StateBase<T>> m_States = new Dictionary<T, StateBase<T>>();
        private T m_State;
        private T m_NextState;

        /// <summary>
        /// Constructs a StateContext instance 
        /// Locates and instantiates all associated state classes
        /// </summary>
        /// <param name="initState">Initial state</param>
        /// <param name="exitState">Last state</param>
        public StateContext(T initState, T exitState)
        {
            Assembly a = Assembly.GetCallingAssembly();
            foreach (Type t in a.GetTypes())
            {
                if (t.IsSubclassOf(typeof(StateBase<T>)) && !t.IsAbstract)
                {
                    T id = (T)Enum.Parse(typeof(T), t.Name);
                    StateBase<T> st = (StateBase<T>)a.CreateInstance(t.FullName);
                    st.SetContext(this);
                    m_States.Add(id, st);
                }
            }
            if (m_States.ContainsKey(exitState))
            {
                throw new ArgumentException(string.Format("No implementation allowed for exitstate '{0}'", exitState));
            }
            m_States.Add(exitState, null);
            foreach (T vt in Enum.GetValues(typeof(T)))
            {
                if (!m_States.ContainsKey(vt))
                {
                    throw new ArgumentException(string.Format("State '{0}' implementation not found", vt));
                }
            }
            m_State = exitState;
            m_NextState = initState;
            NextStateObject = GetState(initState);
        }

        private StateBase<T> GetState(T state)
        {
            if (m_States.ContainsKey(state))
            {
                return m_States[state];
            }
            else
            {
                throw new Exception(string.Format("GetState() State not found: {0}", state));
            }
        }

        private StateBase<T> StateObject
        {
            get
            {
                return m_StateObject;
            }
            set
            {
                m_StateObject = value;
            }
        }

        private StateBase<T> NextStateObject
        {
            get
            {
                return m_NextStateObject;
            }
            set
            {
                m_NextStateObject = value;
            }
        }

        /// <summary>
        /// Current state, readonly
        /// </summary>
        public T State
        {
            get
            {
                return m_State;
            }
            private set
            {
                m_State = value;
            }
        }

        /// <summary>
        /// Next state, writeonly
        /// </summary>
        public T Next
        {
            private get
            {
                return m_NextState;
            }
            set
            {
                NextStateObject = GetState(value);
                m_NextState = value;
            }
        }

        /// <summary>
        /// Returns true while the last/exit state is not reached yet
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (NextStateObject != null) || (StateObject != null);
            }
        }

        /// <summary>
        /// Set a timeout on the current state (writeonly).
        /// Typically set from a states EntryAction.
        /// When timeout elapses the TimeoutHandler of the current state will be called
        /// Timeout is cancelled after a state change.
        /// </summary>
        public TimeSpan Timeout
        {
            set
            {
                m_Deadline = DateTime.Now + value;
            }
        }

        /// <summary>
        /// EventHandler to fill the trigger events queue.
        /// May be called from another thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnqueueTriggerEventHandler(object sender, TriggerEventArgs e)
        {
            m_TriggerEvents.Enqueue(e);
        }

        /// <summary>
        /// Event that will be fired upon state changes
        /// </summary>
        public event StateChangeEventHandler<T> OnStateChange;

        /// <summary>
        /// State handler, to be called repeatedly while statecontext is active
        /// </summary>
        public void Handle()
        {
            if (NextStateObject != StateObject)
            {
                if (StateObject != null)
                {
                    StateObject.ExitAction();
                }
                if (null != OnStateChange)
                {
                    OnStateChange(this, new StateChangeEventArgs<T>(State, Next));
                }
                StateObject = NextStateObject;
                State = Next;
                if (StateObject != null)
                {
                    m_Deadline = DateTime.MaxValue;
                    StateObject.EntryAction();
                }
            }
            else
            {
                if (null != StateObject)
                {
                    if (DateTime.Now > m_Deadline)
                    {
                        StateObject.TimeoutHandler();
                        m_Deadline = DateTime.MaxValue;
                    }
                    else if (m_TriggerEvents.Count > 0)
                    {
                        StateObject.TriggerHandler(m_TriggerEvents.Dequeue());
                    }
                    else
                    {
                        StateObject.DoAction();
                    }
                }
            }
        }
    }
}
