using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PS.Kiosk.Framework
{
    public static class ThreadManager
    {
        public delegate void ThreadStarterCallback();
        public static void RunOnNewThread(ThreadStarterCallback threadStarter)
        {
            KioskLogger.Instance.LogMessage("new tread:" + threadStarter.Method.Name);
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            Thread oThread = new Thread(new ThreadStart(threadStarter));
            oThread.IsBackground = true;
            // Start the thread
            oThread.Start();
        }

        public static void DoRepeatedly(System.Timers.ElapsedEventHandler repeatingMethod, int interval)
        {
            KioskLogger.Instance.LogMessage("new timer tread:" + repeatingMethod.Method.Name);
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(repeatingMethod);
            timer.AutoReset = false;
            timer.Start();
        }
    }
}
