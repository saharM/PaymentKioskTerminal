using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PS.Kiosk.Common.Model
{
    public static class SharedValue
    {
        static object LockObj = new object();

        public static volatile  Mutex AppMutex;

        //private static volatile Mutex AppMutex
        //{
        //    get { return SharedValue._AppMutex; }
        //    set
        //    {
        //        lock (LockObj)
        //        {
        //            SharedValue._AppMutex = value;
        //        }
        //    }
        //}

        public static bool isAppRestart;

        public static bool checkForRestart;

    }
}
