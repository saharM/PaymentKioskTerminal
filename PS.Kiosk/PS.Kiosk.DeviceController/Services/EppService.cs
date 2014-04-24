using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epp;
using PS.Kiosk.Data;
using PS.Kiosk.Framework;
using System.Security.Cryptography;
using System.IO;
using PS.Kiosk.Data.DataAccessObjects;
using PS.Kiosk.Framework.ExceptionManagement;

namespace PS.Kiosk.DeviceController.Services
{
    public class EppService
    {
        private static EppService _instance = null;
        public static EppService Instance
        {
            get { return GetInstance(); }
        }
        private EPPReader EppReader = null;
        private static byte _portNo;
        public int dataLen = 0;
        public string PinData = "";

        

        public int MaxLen
        {
            get
            {
                return EppReader.MaxLen;
            }
            set
            {
                EppReader.MaxLen = value;
            }
        }

        private EppService()
        {
            _portNo = ParametersDataAccess.Instance.EPPPort;
        }

        public static EppService GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock ("CsEppGetInstance")
                    {
                        if (_instance == null)
                        {
                            _instance = new EppService();

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

        #region Event
        event Epp.EPPReader.KeyPressedEventHandler KeyPress;
        #endregion

        /// <summary>
        /// برای دریافت رمز کارت
        /// </summary>
        public void GetPin()
        {
            if (Instance.KeyPress == null)
                Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
            else
            {
                Delegate[] InvokList = Instance.KeyPress.GetInvocationList();

                if (InvokList == null)
                    Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
                else
                    if (InvokList.Select(i => i.Method.Name != "Instance_KeyPress").First())
                        Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
            }

            EppReader.EppOutputMode = EPPReader.eEPPOutputMode.Des;
            Instance.EppGetPin();
            //Instance.MaxLen = 4;
        }

        /// <summary>
        /// برای دریافت عددی که کاربر وارد کرده
        /// </summary>
        public void GetNumber()
        {
            if (Instance.KeyPress == null)
                Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
            else
            {
                Delegate[] InvokList = Instance.KeyPress.GetInvocationList();

                if (InvokList == null)
                    Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
                else
                    if (InvokList.Select(i => i.Method.Name != "Instance_KeyPress").First())
                        Instance.KeyPress += new EPPReader.KeyPressedEventHandler(Instance_KeyPress);
            }
            EppReader.EppOutputMode = EPPReader.eEPPOutputMode.PlanText;
            EppReader.StartGetNumber(1000);
            Instance.MaxLen = 15;
        }

        public void ChangeToDes()
        {
            EppReader.EppOutputMode = EPPReader.eEPPOutputMode.Des;
        }

        void Instance_KeyPress(KeyInfo KeyStatus)
        {
            try
            {
                if (KeyStatus == KeyInfo.Clear)
                {
                    //StateManager.Instance.Current.Message = StateManager.Instance.Current.Message.Substring(0, StateManager.Instance.Current.Message.Length - 1);
                    this.ClearText();
                }
                else
                    if (KeyStatus == KeyInfo.TimeOut || KeyStatus == KeyInfo.Cancel)
                    {
                        StateManager.Instance.Current.FinishSession();
                    }
                    else

                        if (KeyStatus == KeyInfo.Numbers)
                        {
                            if (StateManager.Instance.Current.CurrentState == States.GettingPinState)
                            {
                                if (StateManager.Instance.Current.Message.Length < 4)
                                    StateManager.Instance.Current.Message += "*";
                            }
                            else
                                SetProperty(EppReader.PinPlanData);
                        }
                        else
                            if (KeyStatus == KeyInfo.Enter)
                            {
                                if (StateManager.Instance.Current.CurrentState == States.GettingPinState)
                                {
                                    StateManager.Instance.Current.Message2 = string.Empty;

                                    if (StateManager.Instance.Current.Message.Length == 4)
                                    {

                                        EppReader.ExitEpp();
                                        ParametersDataAccess.Instance.PinBlock = EppReader.PinEncryptData;
                                        StateManager.Instance.Current.AchievedPin();
                                    }
                                    else
                                    {
                                        //هنوز چهار رقم را وارد نکرده و تایید زده
                                        this.StopEppTimer();
                                        StateManager.Instance.Current.GetPin();
                                       
                                    }
                                }
                                
                            }


            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage("EPP_KeyPress ::: " + ex.Message);
            }
        }

        private void EppGetPin()
        {
            EppReader.SetCardNumber(CardReaderService.Instance.CardNumber);
            //byte[] mkey = new byte[8] { 0xf2, 0x63, 0xb2, 0x72, 0x83, 0xc2, 0x70, 0xb4 };
            //byte[] wkey = new byte[8] { 0x73, 0x2c, 0x6b, 0xe5, 0x0e, 0x15, 0x16, 0x8a };
            //SetKeysForLogin(mkey);
            //EppReader.SetPlainWorkingKey(0,1,mkey,wkey);
            //EppReader.SetActiveWKey(0, 0);
            //byte[] encryptedWkey = EppReader.EncryData(wkey, 8);
            //EppReader.SetWorkingKey(0, 1, encryptedWkey);
            EppReader.SetActiveWKey(0, 1);
            EppReader.StartGetPin(4, EPPReader.PinEncryptionMode.ISO9564_1_Format_0, 60);
        }

        public void StopEppTimer()
        {
            EppReader.StopEppTimer();
        }

        public byte[] EncryptPIN(string PIN, string PAN, byte[] key)
        {
            byte[] pinBlock = null;
            try
            {
                PIN = PIN.Length.ToString().PadLeft(2, '0') + PIN.PadRight(14, 'F');
                PAN = "0000" + PAN.PadLeft(19, '0').Substring(6, 12);

                byte[] InPin = HexToBin(PIN);
                byte[] bPAN = HexToBin(PAN);
                for (int i = 0; i < 8; i++)
                    InPin[i] = (byte)((int)InPin[i] ^ (int)bPAN[i]);

                pinBlock = Des(InPin, key, EncryptDecrypt.Encrypt);

                //TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                //TDESAlgorithm.Mode = CipherMode.ECB;
                //TDESAlgorithm.Padding = PaddingMode.None;
                //TDESAlgorithm.Key = key;

                //ICryptoTransform encryptor = TDESAlgorithm.CreateEncryptor();

                //pinBlock = encryptor.TransformFinalBlock(InPin, 0, InPin.Length);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pinBlock;
        }

        public enum EncryptDecrypt
        {
            Encrypt,
            Decrypt
        }

        private byte[] PadRight(byte[] DataIn)
        {
            int num = ((DataIn.Length % 8) == 0) ? DataIn.Length : (((DataIn.Length + 8) / 8) * 8);
            byte[] array = new byte[num];
            DataIn.CopyTo(array, 0);
            return array;
        }

        private byte[] Des(byte[] _Data, byte[] _Key, EncryptDecrypt _Operation)
        {
            CryptoStream stream2;
            byte[] rgbKey = PadRight(_Key);
            byte[] buffer = PadRight(_Data);
            byte[] _Res = new byte[buffer.Length];
            if (_Key.Length == 0)
            {
                return null;
            }
            DES des = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            byte[] IV = new byte[8];
            if (_Operation == EncryptDecrypt.Encrypt)
            {
                stream2 = new CryptoStream(stream, des.CreateEncryptor(rgbKey, IV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, 8);
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(_Res, 0, 8);
                stream.Close();
            }
            else
            {
                stream2 = new CryptoStream(stream, des.CreateDecryptor(rgbKey, IV), CryptoStreamMode.Write);
                byte[] array = new byte[0x10];
                buffer.CopyTo(array, 0);
                stream2.Write(array, 0, array.Length);
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(_Res, 0, 8);
                stream.Close();
            }
            return _Res;
        }

        public byte[] HexToBin(string inputStr)
        {
            byte[] Res = new byte[inputStr.Length / 2];
            int Size = 0, Len = 0;
            if (inputStr.Length == 0)
                return null;
            while (Len < inputStr.Length)
            {
                int k;
                if (inputStr[Len] >= '0' && inputStr[Len] <= '9')
                    k = inputStr[Len] - 0x30;
                else if (inputStr[Len] >= 'A' && inputStr[Len] <= 'F')
                    k = inputStr[Len] - 'A' + 10;
                else if (inputStr[Len] >= 'a' && inputStr[Len] <= 'f')
                    k = inputStr[Len] - 'a' + 10;
                else
                    return null;
                if ((Size & (1 != 0 ? 1 : 0)) != 0)
                    Res[Size >> 1] += (byte)k;
                else
                    Res[Size >> 1] = (byte)(k << 4);
                Len++;
                Size++;
            }
            Size = Size >> 1;
            return Res;
        }


        public byte[] CalculateMac(byte[] inputData, int Length)
        {
            //if (key != null)
            //{
            //    SetKeysForLogin(key);
            //}
            //_instance.EppReader.SetActiveWKey(0, 0);
            //SetActiveWKey(0, 0);
            return _instance.EppReader.MakeMAC(inputData, Length);
        }

        public void SetKeysForLogin(byte[] key)
        {
            //_instance.EppReader.StartEpp(0x08, 0x00);
            bool res1 = _instance.EppReader.ResetKey();
            KioskLogger.Instance.LogMessage("Reset Keys = " + res1);

            bool res2 = _instance.EppReader.SetMasterKey(0, new byte[8] { 0x38, 0x38, 0x38, 0x38, 0x38, 0x38, 0x38, 0x38 }, key);
            KioskLogger.Instance.LogMessage("SetMasterKey = " + res2 + " Key =" + ASCIIEncoding.ASCII.GetString(key));

            bool res3 = _instance.EppReader.SetPlainWorkingKey(0, 0, key, key);
            KioskLogger.Instance.LogMessage("SetPlainWorkingKey = " + res3 + " Key =" + ASCIIEncoding.ASCII.GetString(key));

            //_instance.EppReader.SetWorkingKey(0, key);
            //_instance.EppReader.SetActiveWKey(0, 0);
        }

        public bool SetActiveWKey(byte MKIndex, byte WKIndex)
        {
           return _instance.EppReader.SetActiveWKey(MKIndex, WKIndex);
        }

        public byte[] EncryptData(byte[] inputData)
        {
            return _instance.EppReader.EncryData(inputData, inputData.Length);
        }

        public byte[] DecrypyData(byte[] inputData)
        {
            return _instance.EppReader.DecrypyData(inputData, inputData.Length);
        }

        public bool SetKeyFromLoginReply(byte MIndex, byte MKIndex, byte PKIndex, byte[] Mkey, byte[] Pkey)
        {
            return Instance.EppReader.SetWorkingKey(MIndex, MKIndex, Mkey) && _instance.EppReader.SetWorkingKey(MIndex, PKIndex, Pkey);
        }

        public void EppReader_KeyPressed(Epp.KeyInfo KeyStatus)
        {
            try
            {
                if (KeyStatus == Epp.KeyInfo.Numbers)
                {
                    PinData = EppReader.PinPlanData;
                    dataLen = EppReader.PinLength;
                }
                KeyPress(KeyStatus);
            }
            catch (Exception ex)
            {
                EppDis();
                KioskLogger.Instance.LogMessage(ex, "Exception in EppReader_KeyPressed");
                KeyPress(Epp.KeyInfo.Cancel);
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

        /// <summary>
        ///جاری می دهد Property اطلاعات گرفته شده از ای پی پی را با
        /// </summary>
        /// <param name="value"></param>
        private void SetProperty(string value)
        {
            string CurrentPropertyName = StateManager.Instance.Current.BindedPropertyName;

            if (CurrentPropertyName != null)
            {
                if (StateManager.Instance.Current.CurrentState == States.GettingPinState)
                {
                    StateManager.Instance.Current.GetType().GetProperty(CurrentPropertyName).SetValue(StateManager.Instance.Current, value, null);
                }
                else
                {
                    if (value.Trim() != "*")
                    {
                        string val = Convert.ToString(StateManager.Instance.Current.GetType().GetProperty(CurrentPropertyName).GetValue(StateManager.Instance.Current, null));
                        value = val + value;
                        StateManager.Instance.Current.GetType().GetProperty(CurrentPropertyName).SetValue(StateManager.Instance.Current, value, null);
                    }
                }
            }
            //else
            //    StateManager.Instance.Current.Message = value;
        }

        private void ClearText()
        {
            string CurrentPropertyName = StateManager.Instance.Current.BindedPropertyName;
            if (CurrentPropertyName != null)
            {
                string val = Convert.ToString( StateManager.Instance.Current.GetType().GetProperty(CurrentPropertyName).GetValue(StateManager.Instance.Current, null));
                if (string.IsNullOrEmpty(val) == false)
                {
                    val = val.Substring(0, val.Length - 1);
                    StateManager.Instance.Current.GetType().GetProperty(CurrentPropertyName).SetValue(StateManager.Instance.Current, val, null);
                }
            }
        }

    }
}
