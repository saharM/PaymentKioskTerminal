using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices;
using PS.Kiosk.Framework;

namespace Epp
{
    public enum KeyInfo
    {
        TimeOut = -1,
        Numbers = 0,
        Clear = 1,
        Enter = 2,
        Cancel = 3,
        Other = 4
    }

    internal class EPPReader : IDisposable
    {
        public byte PinLength = 0;
        public string PinPlanData = "";
        public int MaxLen = 0;

        public enum eEPPOutputMode
        {
            PlanText = 0x30,
            Des = 0x31,
            TDes = 0x32,
        }
        public event KeyPressedEventHandler KeyPressed;
        public delegate void KeyPressedEventHandler(KeyInfo KeyStatus);

        public byte PortNumber = 1;
        public bool IsOnGetPin = false;

        private eEPPOutputMode _EppOutputMode = eEPPOutputMode.PlanText;
        public eEPPOutputMode EppOutputMode
        {
            set
            {
                _EppOutputMode = value;
                SetEppOutputMode(value);
            }
            get
            {
                return _EppOutputMode;
            }
        }

        private byte[,] _MasterKey = new byte[16, 16];
        public byte[] GetMasterKey(int Index)
        {
            byte[] b = null;
            if (EppOutputMode == eEPPOutputMode.Des)
            {
                b = new byte[8];
            }
            else
            {
                b = new byte[16];
            }
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = _MasterKey[Index, i];
            }
            return b;
        }
        private void SetMasterKey(int Index, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                _MasterKey[Index, i] = value[i];
            }
        }

        private Timer ReadEPP;
        private DateTime ReadEPPTime;

        private bool _Beep = false;
        public bool Beep
        {
            set
            {
                _Beep = value;
                SetBeep();
            }
            get
            {
                return _Beep;
            }
        }

        public bool ISConnected;

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_EnablePort(int n);

        public bool Connect()
        {
            try
            {
                if (DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day > 20131130)
                {
                    KioskLogger.Instance.LogMessage("Return False in EppReader Connect");
                    return false;
                }

                ISConnected = PinPad_EnablePort(PortNumber % 10) == 0;
                if (ISConnected)
                {
                    FirstInit();
                }
                return ISConnected;
            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX, "Exception in EppReader Connect");
                throw;
            }
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DisablePort();
        public bool Disconnect()
        {
            try
            {
                try
                {
                    ReadEPP.Enabled = false;
                }
                catch (Exception ex) { }

                ISConnected = PinPad_DisablePort() != 0;
            }
            catch (Exception ex) { }

            return !ISConnected;
        }

        private void FirstInit()
        {
            SetBeep();
            SetEppOutputMode(eEPPOutputMode.Des);
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_SetBeep(byte n);
        private void SetBeep()
        {
            if (!ISConnected)
            {
                return;
            }
            PinPad_SetBeep((byte)((Beep) ? 0x32 : 0x33));
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_SetParameters(byte n);
        private void SetEppOutputMode(eEPPOutputMode ePPOutputMode)
        {
            try
            {
                if (!ISConnected)
                {
                    return;
                }

                PinPad_SetParameters((byte)ePPOutputMode);
                _EppOutputMode = ePPOutputMode;
            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX,"Error in Epp Change Mode");
                
            }

        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_InitKey();
        public bool ResetKey()
        {
            if (!ISConnected)
            {
                return false;
            }
            return PinPad_InitKey() == 0;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DownloadMKey(byte M, byte[] MK1, byte[] MK2, byte Length);
        public bool SetMasterKey(byte Index, byte[] OldKey, byte[] NewKey)
        {
            bool Ret = false;
            if (!ISConnected)
            {
                return false;
            }
            Ret = PinPad_DownloadMKey(Index, OldKey, NewKey, (byte)((EppOutputMode == eEPPOutputMode.Des) ? 8 : 16)) == 0;
            if (Ret)
            {
                SetMasterKey(Index, NewKey);
            }
            return Ret;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DownloadEncryWKey(byte M, byte N, byte[] WP, byte Length);
        public bool SetWorkingKey(byte MIndex, byte WIndex, byte[] NewKey)
        {
            bool Ret = false;
            if (!ISConnected)
            {
                return false;
            }
            Ret = PinPad_DownloadEncryWKey(MIndex, WIndex, NewKey, (byte)((EppOutputMode == eEPPOutputMode.Des) ? 8 : 16)) == 0;
            return Ret;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DownloadEncryplainWKey(byte M, byte N, byte[] WP1, byte[] WP2, byte Length);
        public bool SetPlainWorkingKey(byte MIndex, byte WIndex, byte[] NewKey, byte[] MKey)
        {
            bool Ret = false;
            if (!ISConnected)
            {
                return false;
            }
            Ret = PinPad_DownloadEncryplainWKey(MIndex, WIndex, NewKey, MKey, (byte)((EppOutputMode == eEPPOutputMode.Des) ? 8 : 16)) == 0;
            return Ret;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_ActiveWKey(byte M, byte N);
        public bool SetActiveWKey(byte MasterKeyIndex, byte WKeyIndex)
        {
            bool Ret = false;
            if (!ISConnected)
            {
                return false;
            }
            Ret = PinPad_ActiveWKey(MasterKeyIndex, WKeyIndex) == 0;
            return Ret;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DownloadCNo(string CardNumber, byte Length);
        public bool SetCardNumber(string CD, byte Length = 12)
        {
            try
            {
                bool Ret = false;
                if (!ISConnected)
                {
                    return false;
                }
                CD = System.Text.ASCIIEncoding.ASCII.GetString(System.Text.ASCIIEncoding.ASCII.GetBytes(CD));
                CD = CD.Substring(0, CD.Length - 1);
                CD = CD.Substring(CD.Length - Length);
                //PAN = "0000" + PAN.PadLeft(19, '0').Substring(6, 12);
                Ret = PinPad_DownloadCNo(CD, Length) == 0;
                return Ret;
            }
            catch (Exception EX)
            {
                
               StateManager.Instance.Current.Error(EX);
            }

            return false;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_MakeMac(byte[] Str1, byte[] Str2, int Length);
        public byte[] MakeMAC(byte[] InputData, int Length)
        {
            byte[] Ret = new byte[8];
            if (!ISConnected)
            {
                KioskLogger.Instance.LogMessage("MakeMAC In EppReader return Null , Not Connected");
                return null;
            }            
            if (PinPad_MakeMac(InputData, Ret, Length) == 0)
            {
                KioskLogger.Instance.LogMessage("MakeMAC In EppReader " + ASCIIEncoding.ASCII.GetString(Ret));
                return Ret;
            }
            else
            {
                KioskLogger.Instance.LogMessage("MakeMAC In EppReader return Null " );
                return null;
            }
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_EncryData(byte[] Str1, byte[] Str2, int Length);
        public byte[] EncryData(byte[] InputData, int Length)
        {
            byte[] Ret = new byte[8];
            if (!ISConnected)
            {
                return null;
            }
            if (PinPad_EncryData(InputData, Ret, Length) == 0)
            {
                return Ret;
            }
            else
            {
                return null;
            }
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_DecryData(byte[] Str1, byte[] Str2, int Length);
        public byte[] DecrypyData(byte[] InputData, int Length)
        {
            byte[] Ret = new byte[Length];
            if (!ISConnected)
            {
                return null;
            }
            if (PinPad_DecryData(InputData, Ret, Length) == 0)
            {
                return Ret;
            }
            else
            {
                return null;
            }
        }

        public enum PinEncryptionMode
        {
            ASSCII = 0,
            ISO9564_1_Format_0 = 1,
            ISO9564_1_Format_1 = 2,
            ISO9564_1_Format_3 = 3,
            IBM3624 = 4,
            NCR = 5
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_StartEPP(byte Length, byte EncryptionMode);
        public bool StartEpp(byte Length, byte EncryptionMode)
        {
            bool Ret = false;
            if (!ISConnected)
            {
                return false;
            }
            Ret = PinPad_StartEPP(Length, EncryptionMode) == 0;            
            return Ret;
        }

        public void StartGetPin(byte Length, PinEncryptionMode EncryptionMode, byte TimeOut = 30)
        {
            PinLength = 0;
            PinPlanData = "";
            ReadEPPTime = DateTime.Now.AddSeconds(TimeOut);
            ReadEPP.Interval = 100;
            ReadEPP.Enabled = true;
            ReadEPP.Start();
            if (Length < 4) { Length = 4; }
            if (Length > 13) { Length = 13; }
            
            PinPad_StartEPP(4, (byte)EncryptionMode);
            IsOnGetPin = true;
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_ExitEPP();
        public void ExitEpp()
        {
            int Ret = 0;
            if (!ISConnected)
            {
                return;
            }
            ReadEPP.Stop();
            Ret = PinPad_ExitEPP();
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_ReadOneByte(byte[] ReadByte);
        public void StartGetNumber(int TimeOut = 120)
        {
            PinLength = 0;
            PinPlanData = "";

            byte[] ReceiveData = new byte[20];
            for (int i = 0; i <= 10; i++)
            {
                PinPad_ReadOneByte(ReceiveData);
            }
            ReadEPPTime = DateTime.Now.AddSeconds(TimeOut);
            ReadEPP.Interval = 100;
            ReadEPP.Enabled = true;
        }

        private void ReadEPP_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > ReadEPPTime)
            {
                ReadEPP.Enabled = false;
                IsOnGetPin = false;
                if (KeyPressed != null)
                {
                    KeyPressed(KeyInfo.TimeOut);
                }
                return;
            }
            int i = 0;
            byte[] ReceiveData = new byte[20];
            i = PinPad_ReadOneByte(ReceiveData);
            if (i > 0)
            {
                for (int j = 0; j < i; j++)
                {
                    switch (ReceiveData[j])
                    {
                        case 0x42:
                            PinLength += 1;
                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.Numbers);
                            }
                            break;
                        case 0x80://'Time Out
                            ReadEPP.Enabled = false;
                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.TimeOut);
                            }
                            break;
                        case 0x8: //'Clear
                            if (PinLength > 0)
                            {
                                PinLength -= 1;
                                if (StateManager.Instance.Current.CurrentState == States.GettingPinState)
                                    PinPlanData = PinPlanData.Substring(0, PinLength);
                            }
                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.Clear);
                            }
                            
                            break;
                        case 0xD: //'Enter
                            //'ReadEPP.Enabled = False
                            //'IsOnGetPin = False
                           
                            
                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.Enter);
                            }
                            break;
                        case 0x1B: //'Cancel
                            ReadEPP.Enabled = false;
                            IsOnGetPin = false;
                            PinLength = 0;
                            PinPlanData = "";
                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.Cancel);
                            }
                            break;
                        default:
                            if (StateManager.Instance.Current.CurrentState == States.GettingPinState)
                            {
                                if (PinLength < MaxLen)
                                {
                                    PinLength += 1;
                                    PinPlanData += (char)ReceiveData[j];

                                }
                            }
                            else
                            {
                                //ی جاری اضافه می کند به متغییر طول اضافه نمی کنیم،جای ديگه هم از این متغییر استفاده نمی شود  Property چون داده را به  
                                PinPlanData = Convert.ToString((char)ReceiveData[j]);
                            }
                           

                            if (KeyPressed != null)
                            {
                                KeyPressed(KeyInfo.Numbers);
                            }
                            break;
                    }
                }
            }
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_OutProcess();
        public void StopGetPinOrNumber()
        {
            ReadEPP.Enabled = false;
            PinPad_OutProcess();
        }

        [DllImport("KTAPI.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int PinPad_ReadPin(byte[] ReadByte);
        public byte[] PinEncryptData
        {
            get
            {
                int i;
                //byte[] PIN_Data = new byte[8];
                byte[] Ret = new byte[8];
                SetActiveWKey(0, 1);
                i = PinPad_ReadPin(Ret);
                
                //if (i != 0)
                //{
                    //Ret = new byte[i - 1];
                    //for (int j = 0; j < i; j++)
                    //{
                    //    Ret[j] = PIN_Data[j];
                    //}
                //}
                return Ret;
            }
        }

        public string GetHex(byte[] B)
        {
            string Ret = "";
            for (int i = 0; i < B.Length; i++)
            {
                string BiHex = "0" + B[i].ToString("X");
                Ret += BiHex.Substring(BiHex.Length - 2);
            }
            return Ret;
        }
        public EPPReader()
        {
            ReadEPP = new Timer();
            ReadEPP.Elapsed += new ElapsedEventHandler(ReadEPP_Tick);
        }

        public void StopEppTimer()
        {
            ReadEPP.Stop();
        }

        #region IDisposable Support

        public void Dispose()
        {
            ReadEPP.Enabled = false;
            ReadEPP.Dispose();
            ReadEPP = null;
            Disconnect();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
