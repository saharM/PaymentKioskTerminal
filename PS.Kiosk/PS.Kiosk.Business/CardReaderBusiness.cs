using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.DeviceController;
using PS.Kiosk.Common;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data;
using System.Timers;

namespace PS.Kiosk.Business
{
    public class CardReaderBusiness
    {
        private static CardReaderBusiness _instance = null;
        private static CardReader _reader;
        private static int _portNo;
        public CardReaderBusiness()
        {
            ParametersDataAccess pda = new ParametersDataAccess();
            _portNo = pda.CardReaderPort;
        }
        public static CardReaderBusiness GetInstance()
        {            
            try
            {
                if (_instance == null)
                {
                    lock ("CsReaderGetInstance")
                    {
                        if (_instance == null)
                        {
                            _instance = new CardReaderBusiness();
                            _reader = new CardReader("COM" + _portNo);
                            _reader.Connect();
                        }
                    }
                }
                return _instance;
            }
            catch (Exception ex)
            {
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);

                return null;
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
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);

                _reader = null;
            }

            if (_reader == null || _reader.Track2 == "" || _reader.Track2 == null || _reader.CurrentCardPosition == CardReader.CardPosition.No_Card_In_The_Reader)
            {
                try
                {
                    _reader.ReadStatus();
                    if ((_reader == null || _reader.Track2 == "" || _reader.Track2 == null)&& _reader.CurrentCardPosition != CardReader.CardPosition.No_Card_In_The_Reader)
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
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
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
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
            }
        }
        public void Close()
        {
            try
            {
                _reader.Disconnect();
                _reader = null;
            }
            catch (Exception ex)
            {
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
            }
        }
        public CardReader.CardPosition IsCard()
        {
            try
            {
                if (!(bool)_reader.ISConnected)
                    _reader.Connect();
                return _reader.CurrentCardPosition;
            }
            catch (Exception ex)
            {
                KioskLogger.GetInstance().SendMessage(System.Reflection.MethodBase.GetCurrentMethod().Name + " ::: " + ex.Message);
                return  CardReader.CardPosition.No_Card_In_The_Reader;
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
            if (KioskStateManager.GetInstance().CurrentKiosk.CurrentState == KioskStates.WaitingState && CheckReader())
            {
                (sender as Timer).Stop();
                (sender as Timer).Dispose();
                KioskStateManager.GetInstance().CurrentKiosk.OnGetCardEvent();
            }
            else
            {
                (sender as Timer).Start();
            }
        }
    }
}
