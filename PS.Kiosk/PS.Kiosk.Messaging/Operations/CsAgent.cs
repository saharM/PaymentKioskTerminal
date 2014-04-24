using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanap.Messaging;
using System.Threading;
using System.Reflection;
using Fanap.Messaging.Iso8583;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.IO;
using PS.Kiosk.Messaging.Operations;
using PS.Kiosk.Common;
using PS.Kiosk.DeviceController.Services;
using PS.Kiosk.Framework;


namespace PS.Kiosk.Messaging.Operations
{
    public class CsAgent
    {
        #region Variable
        //protected EPP.EPPReader _Epp;

        private byte[] IV = new byte[8];
        protected CsUtil csUtil = new CsUtil();
        private const int MACLength = 16;
        protected string _ip = "";
        protected int _port;
        //protected IMessageFormatter _formatter; //= new Iso8583Ascii1987MessageFormatter();
        protected string _filename = "";
        protected FormatterContext formatterContext;
        protected CsParameters _csParam = new CsParameters();

        protected const int _PINLENGTH = 4;
        protected const int _MIN_PAN_LEN = 16;
        protected const int _MAX_PAN_LEN = 19;
        #endregion

        #region ConstVariable


        protected string FINANCIAL_FUNCTION_CODE = "200";
        protected string REVERSAL_FUNCTION_CODE = "400";
        protected string ENDDAY_FUNCTION_CODE = "821";
        protected string RECONCIL_FUNCTION_CODE = "500";
        protected string FILE_ACTION_FUNC_CODE = "300";
        protected string IRAN_CURRENCY_CODE = "364";

        protected string PURCH_PROCESSCODE = "000000";
        protected string REM_PROCESSCODE = "310000";
        protected string AUTH_PROCESSCODE = "330000";
        protected string GETACCOUNTS_PROCESSCODE = "330000";
        protected string STATE_PROCESSCODE = "370000";
        protected string BILLAUTH_PROCESSCODE = "190000";
        protected string BILLPAY_PROCESSCODE = "170000";
        protected string TRANSC2C_PROCESSCODE = "400000";
        protected string TRANA2A_PROCESSCODE = "401010";
        protected string TRANA2AO_PROCESSCODE = "401020";
        protected string TRANC2AO_PROCESSCODE = "402010";
        protected string PIN_PROCESSCODE = "190000";

        protected string MELLIBINCODE = "673020";

        //protected byte[] DefMAC = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };//"00000000";
        protected string DefMAC = "0000000000000000";
        #endregion

        #region Properties
        //public EPP.EPPReader Epp
        //{
        //    set
        //    {
        //        _Epp = value;
        //    }
        //    get
        //    { return _Epp; }
        //}
        public CsParameters CsParam
        {
            set
            {
                _csParam = value;
            }
            get
            {
                return _csParam;
            }
        }
        //public IMessageFormatter formatter
        //{
        //    set
        //    {
        //        _formatter = value;
        //    }
        //}

        #endregion

        #region Constructor
        public CsAgent()
        {
        }
        #endregion

        #region Methods

        protected bool CheckMAC(Iso8583Message message)
        {
            try
            {

                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                //byte[] key = csUtil.HexToBin(keyStr);

                IMessageFormatter fm = message.Formatter; //_formatter;
                formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
                fm.Format(message, ref formatterContext);

                //byte[] key = new byte[] { 0xf2,0x63,0xb2,0x72,0x83,0xc2,0x70,0xb4};
                //EppService.Instance.SetKeysForLogin(key);
                byte[] makArr = CalculateMAC(formatterContext.GetData(), MACLength);
                //byte[] inputData = formatterContext.GetData();
                //byte[] inputDataWithoutMAC = inputData.Take(inputData.Length - 16).ToArray();
                //byte[] makArr = EppService.Instance.CalculateMac(key, inputDataWithoutMAC, inputDataWithoutMAC.Length);
                string makStr = csUtil.BinToHex(makArr);

                bool result = false;
                if (message.Fields.Contains((int)ISOEl.P64))
                    result = (message.Fields[64].ToString() == makStr);
                else
                    result = (message.Fields[128].ToString() == makStr);
                return result;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckMAC2(Iso8583Message message)
        {
            try
            {

                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                byte[] key = csUtil.HexToBin("75D385BCAE62624398324345686438D3");//keyStr);


                IMessageFormatter fm = new Iso8583Ascii1993MessageFormatter();// _formatter;
                formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
                fm.Format(message, ref formatterContext);

                byte[] makArr = CalculateMAC2(key, formatterContext.GetData(), MACLength);
                string makStr = csUtil.BinToHex(makArr);

                bool result = false;
                if (message.Fields.Contains((int)ISOEl.P64))
                    result = (message.Fields[64].ToString() == makStr);
                else
                    result = (message.Fields[128].ToString() == makStr);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected byte[] CalculateMAC2(byte[] key, byte[] Packet, int MACLen)
        {
            int blockLength = 8;
            byte[] block = new byte[blockLength];
            byte zero = 0;
            byte[] Res = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            int PacketLength = Packet.Length - MACLen, idx = 0;
            bool first = true;

            byte[] key1 = new byte[8];
            byte[] key2 = new byte[8];

            Array.Copy(key, 0, key1, 0, 8);
            Array.Copy(key, 8, key2, 0, 8);

            try
            {
                while (idx < PacketLength)
                {
                    if (first)
                    {
                        if (PacketLength - idx < blockLength)
                            block = new byte[blockLength - Math.Abs(PacketLength - idx)];
                        for (int i = idx; i < (blockLength + idx) && i < PacketLength; i++)
                            block[i - idx] = (i < PacketLength ? Packet[i] : zero);
                        idx += blockLength;
                        Res = csUtil.DesBuffer(block, key1, EncryptDecrypt.Encrypt);
                        //Res = csUtil._3Des(block, key, EncryptDecrypt.Encrypt);
                    }
                    first = false;
                    for (int i = 0; i < blockLength; i++)
                        Res[i] = (byte)((int)Res[i] ^ (int)(idx + i < PacketLength ? Packet[i + idx] : zero));
                    idx += blockLength;
                    block = Res;
                    Res = csUtil.DesBuffer(block, key1, EncryptDecrypt.Encrypt);
                    //Res = csUtil._3Des(block, key, EncryptDecrypt.Encrypt);
                }
                Res = csUtil.DesBuffer(Res, key2, EncryptDecrypt.Decrypt);
                Res = csUtil.DesBuffer(Res, key1, EncryptDecrypt.Encrypt);

                //Array.Reverse(key, 8, 8);
                //Res = csUtil._3Des(Res, key, EncryptDecrypt.Encrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Res;
        }

        protected bool SetMAC(ref Iso8583Message message, byte[] key)
        {
            try
            {
                EppService.Instance.ChangeToDes();

                IMessageFormatter fm = message.Formatter; //_formatter;
                formatterContext = new FormatterContext(FormatterContext.DefaultBufferSize);
                fm.Format(message, ref formatterContext);

                byte[] result = CalculateMAC(formatterContext.GetData(), MACLength);
                //byte[] inputData = formatterContext.GetData();
                //byte[] inputDataWithoutMAC = inputData.Take(inputData.Length - 16).ToArray();
                //byte[] result = EppService.Instance.CalculateMac(key, inputDataWithoutMAC, inputDataWithoutMAC.Length);
                if (message.Fields.Contains((int)ISOEl.P64))
                    message.Fields.Add((int)ISOEl.P64, csUtil.BinToHex(result));
                else
                    message.Fields.Add((int)ISOEl.S128, csUtil.BinToHex(result));
                return true;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage("Exception In SetMAC");
                throw ex;
            }
        }

        public byte[] CalculateMAC(byte[] Packet, int MACLen)
        {
            
            //Array.Copy(Packet, 10, Packet, 0, Packet.Length - 10);
            int blockLength = 8;
            byte[] block = new byte[blockLength];
            byte zero = 0;
            byte[] Res = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            int PacketLength = Packet.Length - MACLen, idx = 0;//-10
            bool first = true;
            try
            {
                bool ActiveKeyRes = EppService.Instance.SetActiveWKey(0, 0);
                KioskLogger.Instance.LogMessage("Set Active Key in Calculate MAC = " + ActiveKeyRes.ToString());

                while (idx < PacketLength)
                {
                    //byte[] ResManual = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
                    if (first)
                    {
                        //if (PacketLength - idx < blockLength)
                        //    block = new byte[blockLength - Math.Abs(PacketLength - idx)];
                        //for (int i = idx; i < (blockLength + idx) && i < PacketLength; i++)
                        //    block[i - idx] = (i < PacketLength ? Packet[i] : zero);
                        //idx += blockLength;
                        if (PacketLength < 96)
                        {
                            block = new byte[PacketLength];
                            for (int i = 0; i < PacketLength; i++)
                                block[i] = Packet[i];
                            idx += PacketLength;
                            Res = EppService.Instance.CalculateMac(block, PacketLength); //csUtil.DesBuffer(block, key, EncryptDecrypt.Encrypt);
                        }
                        else
                        {
                            block = new byte[88];
                            for (int i = 0; i < 88; i++)
                                block[i] = Packet[i];
                            idx += 88;
                            Res = EppService.Instance.CalculateMac(block, 88); //csUtil.DesBuffer(block, key, EncryptDecrypt.Encrypt);
                        }
                        first = false;
                        //ResManual = csUtil.DesBuffer(block, key, EncryptDecrypt.Encrypt);
                    }
                    else
                    {                        
                        for (int i = 0; i < blockLength; i++)
                            Res[i] = (byte)((int)Res[i] ^ (int)(idx + i < PacketLength ? Packet[i + idx] : zero));
                        idx += blockLength;
                        block = Res;
                        Res = EppService.Instance.EncryptData(block); //csUtil.DesBuffer(block, key, EncryptDecrypt.Encrypt);
                        //ResManual = csUtil.DesBuffer(block, key, EncryptDecrypt.Encrypt);
                    }
                    //for (int i = 0; i < ResManual.Length; i++)
                    //{
                    //    if (Res[i] != ResManual[i])
                    //    {
                    //        throw new Exception("bad encryption!");
                    //    }   
                    //}                    
                }
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(ex ,"Exception In CalculateMAC");
                throw ex;
            }
            byte[] _res = new byte[4];
            Array.Copy(Res, _res, 4);

            KioskLogger.Instance.LogMessage("CalculateMAC Return = " + Res.ToString());

            return Res;
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


        public byte[] EncryptPIN(string PIN, string PAN, byte[] key)
        {
            byte[] pinBlock = null;
            try
            {
                PIN = PIN.Length.ToString().PadLeft(2, '0') + PIN.PadRight(14, 'F');
                PAN = "0000" + PAN.PadLeft(19, '0').Substring(6, 12);

                byte[] InPin = csUtil.HexToBin(PIN);
                byte[] bPAN = csUtil.HexToBin(PAN);
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

        #endregion
    }
}
