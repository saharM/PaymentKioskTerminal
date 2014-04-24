using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Data;
using PS.Kiosk.Framework;
using System.Timers;
using PS.Kiosk.Data.DataAccessObjects;

namespace PS.Kiosk.DeviceController.Services
{
    public class CardReaderService
    {
        private static CardReaderService _instance;
        public static CardReaderService Instance
        {
            get { return GetInstance(); }
        }
        private static CardReader _reader;
        private static int _portNo;
        private CardReaderService()
        {
            _portNo = ParametersDataAccess.Instance.CardReaderPort;
        }
        private static CardReaderService GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock ("CsReaderGetInstance")
                    {
                        if (_instance == null)
                        {
                            _instance = new CardReaderService();
                            _reader = new CardReader("COM" + _portNo);
                            _reader.Connect();
                        }
                    }
                }
                return _instance;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);

                return null;
            }
        }
        public string CardNumber
        {
            get
            {
                if (_reader.Track2 != string.Empty)
                {
                    return _reader.Track2.Split('=')[0];
                }
                else
                {
                    StateManager.Instance.Current.Error("No CardNumber!");
                    return null;
                }
            }
        }
        public string IsoTrack
        {
            get
            {
                if (_reader.Track2 != string.Empty)
                {
                    return _reader.Track2;
                }
                else
                {
                    StateManager.Instance.Current.Error("No IsoTrack!");
                    return null;
                }
            }
        }
        public bool CheckReader()
        {
            //return true;

            try
            {
                if (!(bool)_reader.ISConnected)
                    _reader.Connect();
                _reader.ReadStatus();
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage("check reader exception.");

                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);

                _reader = null;
                
            }

            if (_reader == null || _reader.Track2 == "" || _reader.Track2 == null || _reader.CurrentCardPosition == CardPosition.No_Card_In_The_Reader)
            {
                try
                {
                    _reader.ReadStatus();
                    if ((_reader == null || _reader.Track2 == "" || _reader.Track2 == null) && _reader.CurrentCardPosition != CardPosition.No_Card_In_The_Reader)
                        EjectCard();
                }
                catch
                {
                }
                return false;
            }
            // SignOnParameters.isoTrack = _reader.Track2;//"6280231300591749=92045061757277300001";//
            // SignOnParameters.PAN = SignOnParameters.isoTrack.Substring(0, SignOnParameters.isoTrack.IndexOf('='));
            return true;
        }
        /// <summary>
        /// پس دادن کارت: کارت داخل ریدر را از سمت جلو ریدر خارج می کند
        /// </summary>
        public void EjectCard()
        {
            try
            {
                if (!(bool)_reader.ISConnected)
                    _reader.Connect();
                _reader.CardEject();
                _reader.Track2 = "";
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
            }
        }
        /// <summary>
        /// ضبط کارت: کارت داخل ریدر را از سمت پشت ریدر خارج می کند
        /// </summary>
        public void CaptureCard()
        {
            try
            {
                if (!(bool)_reader.ISConnected)
                    _reader.Connect();
                _reader.CaptureCard();
                _reader.Track2 = "";
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
            }
        }
        public void Close()
        {
            try
            {
                _reader.Disconnect();
                _reader = null;
                KioskLogger.Instance.LogMessage("reader closed.");
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
            }
        }
        public CardPosition IsCard()
        {
            try
            {
                if (!(bool)_reader.ISConnected)
                    _reader.Connect();
                return _reader.CurrentCardPosition;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
                return CardPosition.No_Card_In_The_Reader;
            }
        }
        public void NoAcceptCard()
        {
            _reader.EnabledShutter(false);
        }
        public void AcceptCard()
        {
            _reader.EnabledShutter(true);
        }

        public void WaitForCard()
        {
            ThreadManager.DoRepeatedly(CardWaiterTimer_Elapsed, 500);
        }
        void CardWaiterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (StateManager.Instance.Current.CurrentState == States.WaitingForCardState && CheckReader())
            {
                (sender as Timer).Stop();
                (sender as Timer).Dispose();
                StateManager.Instance.Current.ReceivedCard();
            }
            else
            {
                (sender as Timer).Start();
            }
        }
    }
}
