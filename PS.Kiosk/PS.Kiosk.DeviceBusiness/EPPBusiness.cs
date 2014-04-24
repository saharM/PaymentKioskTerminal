using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epp;
using PS.Kiosk.Data;

namespace PS.Kiosk.Common
{
    public class EPPBusiness
    {        
        private static EPPBusiness _instance = null;
        private EPPReader EppReader = null;
        private static byte _portNo;
        public int dataLen = 0;
        public string PinData = "";

        public EPPBusiness()
        {
            ParametersDataAccess pda = new ParametersDataAccess();
            _portNo = pda.EPPPort;
        }

        public static EPPBusiness GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock ("CsEppGetInstance")
                    {
                        if (_instance == null)
                        {
                            _instance = new EPPBusiness();

                        }
                    }
                }
                if (_instance.EppReader == null)
                {
                    _instance.EppReader = new Epp.EPPReader();
                    _instance.EppReader.KeyPressed += new Epp.EPPReader.KeyPressedEventHandler(_instance.EppReader_KeyPressed);
                    _instance.EppReader.PortNumber = _portNo;
                    if (!_instance.EppConnect())
                        _instance = null;
                }
                return _instance;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region CloseEvent
        //public event CloseFormEvent CloseForm;
        public event Epp.EPPReader.KeyPressedEventHandler KeyPress;
        #endregion


        public byte[] CalculateMac(byte[] key, byte[] inputData, int Length)
        {
            _instance.EppReader.SetWorkingKey(0, key);
            return _instance.EppReader.MakeMAC(inputData, Length);
        }

        public void EppReader_KeyPressed(Epp.EPPReader.KeyInfo KeyStatus)
        {
            try
            {
                if (KeyStatus == Epp.EPPReader.KeyInfo.Numbers)
                {
                    PinData = EppReader.PinPlanData;
                    dataLen = EppReader.PinLength;
                }
                KeyPress(KeyStatus);
            }
            catch (Exception ex)
            {
                EppDis();
                KeyPress(Epp.EPPReader.KeyInfo.Cancel);
            }
        }

        public bool EppDis()
        {
            try
            {
                EppReader.Disconnect();
                EppReader.Dispose();
                EppReader = null;
                return true;
            }
            catch
            {
                EppReader = null;
                return false;
            }
        }

        private bool EppConnect()
        {
            try
            {
                return EppReader.Connect();
            }
            catch (Exception a)
            {
                return false;
            }
        }

    }
}
