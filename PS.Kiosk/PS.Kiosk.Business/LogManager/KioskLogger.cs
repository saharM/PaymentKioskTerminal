using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PS.Kiosk.Business
{
    public class KioskLogger
    {
        private int counter = 1;
        private static KioskLogger _instance;

        #region Variable
        private StreamWriter _stmWriter = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public KioskLogger()
        {

        }

        #endregion

        #region Methods

        public void SendMessage(string Msg)
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
                    Msg = counter.ToString() + " >>> " + "<" + DateTime.Now.ToString().Substring(0, 20) + "> : " + Msg;
                    ++counter;
                    _stmWriter.WriteLine(Msg);
                    _stmWriter.Close();
                    _stmWriter = null;
                }
                catch (Exception ex)
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
        public static KioskLogger GetInstance()
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

    }
}
