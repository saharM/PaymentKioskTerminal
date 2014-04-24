using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using PS.Kiosk.Framework.ExceptionManagement;

namespace PS.Kiosk.Framework
{
    public class KioskLogger
    {
        private int counter = 1;
        private static KioskLogger _instance;
        public static KioskLogger Instance
        {
            get { return GetInstance(); }
        }

        #region Variable
        private StreamWriter _stmWriter = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        private KioskLogger()
        {

        }

        #endregion

        #region Methods

        private void LogMessage2(string Msg)
        {
            //lock (this)
            {
               // Console.WriteLine(Msg);
                try
                {
                    string fname = "C:\\LOG\\Log-" + DateTime.Now.ToString().Substring(0, 10).Replace("/", "-") + ".txt";
                    if (!Directory.Exists("C:\\LOG"))
                        Directory.CreateDirectory("C:\\LOG");
                    if (_stmWriter == null)
                        _stmWriter = new StreamWriter(fname, true);
                    if (!File.Exists(fname))
                    {
                        if (fname != null)
                        {
                            _stmWriter.Close();
                        }
                        _stmWriter = new StreamWriter(fname, true);
                    }
                    Msg = counter.ToString() + " >>> " + "<" + DateTime.Now.ToString("yymmdd") + "> : " + Msg;
                    ++counter;
                    _stmWriter.WriteLine(Msg);
                    _stmWriter.Close();
                    _stmWriter = null;
                }
                catch (Exception)
                {
                    if (_stmWriter != null)
                        _stmWriter.Close();
                    _stmWriter = null;
                    return;
                }
            }

        }
        public void Stop()
        {            
            _stmWriter.Close();
            _stmWriter = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public void Dispose()
        {
        }
        private static KioskLogger GetInstance()
        {
            if (_instance == null)
            {
                lock ("CsLoggerGetInstance")
                {
                    if (_instance == null)
                    {
                        _instance = new KioskLogger();
                    }
                }
            }
            return _instance;
        }
        #endregion

        private string GetLogFileName()
        {

            try
            {
                //string FullPath = @"D:\mohanna\PS.Kiosk\PS.Kiosk.UI\bin\Debug";//Assembly.GetCallingAssembly().Location;

                string FullPath = Assembly.GetEntryAssembly().Location;
                string CurrentPath = FullPath.Replace(".exe", "-Log") + "\\" + "KioskErrorLog";

                if (!Directory.Exists(CurrentPath))
                    Directory.CreateDirectory(CurrentPath);

                return CurrentPath + "\\" + "KioskErrorLog_" + DateTime.Now.ToString("yymmdd") + ".txt";
            }
            catch (Exception EX)
            {


            }

            return "";

        }

        private string GetStatesLogFileName()
        {

            try
            {
                

                string FullPath = Assembly.GetEntryAssembly().Location;
                string CurrentPath = FullPath.Replace(".exe", "-Log") + "\\" + "KioskStatesLog";

                if (!Directory.Exists(CurrentPath))
                    Directory.CreateDirectory(CurrentPath);

                return CurrentPath + "\\" + "KioskStates_" + DateTime.Now.ToString("yymmdd") + ".txt";
            }
            catch (Exception EX)
            {


            }

            return "";

        }

        public void LogMessage(string message)
        {

            try
            {
                string vFileName = this.GetLogFileName();

                if (!string.IsNullOrEmpty(vFileName))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(vFileName, true);
                    sw.WriteLine(System.DateTime.Now.ToString() + "   " + (message));
                    sw.WriteLine("------------------------------------------------------------------------");
                    sw.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception EX)
            {
                //throw EX;
            }
        }

        public void LogMessage(Exception EX, string Extramessage)
        {

            try
            {
                string vFileName = this.GetLogFileName();

                if (!string.IsNullOrEmpty(vFileName))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(vFileName, true);

                    string Msg;
                    string ExMessage;
                    if (EX is CustomException)
                        ExMessage = " EX Message : " + (EX as CustomException).LogMessage;
                    else
                        ExMessage = " EX Message : " + EX.Message;
                   

                        if (EX.InnerException != null)
                            Msg = "User:" + Extramessage +  ExMessage + "-" + "StackTrace :" + EX.StackTrace + "-" + "InnerExep:" + EX.InnerException.Message;
                        else
                            Msg = "User:" + Extramessage +  ExMessage + "-" + "StackTrace :" + EX.StackTrace;
                   

                    sw.WriteLine(System.DateTime.Now.ToString() + "   " + (Msg));
                    sw.WriteLine("------------------------------------------------------------------------");
                    sw.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception EX2)
            {
                //throw EX2;
            }
        }

        public void LogChangeStates(string message)
        {

            try
            {
                string vFileName = this.GetStatesLogFileName();

                if (!string.IsNullOrEmpty(vFileName))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(vFileName, true);
                    sw.WriteLine(System.DateTime.Now.ToString() + "   " + (message));
                    sw.WriteLine("------------------------------------------------------------------------");
                    sw.Close();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception EX)
            {
                //throw EX;
            }
        }
    }
}
