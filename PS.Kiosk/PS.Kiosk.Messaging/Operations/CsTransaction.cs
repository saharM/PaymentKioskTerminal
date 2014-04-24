using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanap.Messaging;
using Fanap.Messaging.Iso8583;
using System.IO;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using PS.Kiosk.Messaging.Operations;



namespace PS.Kiosk.Messaging.Operations
{
    #region enum
    
    #endregion

    public enum KeyType
    {
        PINKey = 1,
        MACKey = 2,
        MASTERKey = 3
    }
    public class CsTransaction : CsAgent
    {
        public delegate void OnReceiveResponseDelegate(int ID);

        #region variable
        public Fanap.Messaging.Message _messageout;
        
        private Iso8583Message inMessage = new Iso8583Message();
        private Iso8583Message _inMessage = new Iso8583Message();
        private string _terminalID = "";
        private string _cardAcceptorID = "";
        private string _terminalName = "";
        private string _AcceptorBINCode;
        private int timeOut;
        private Iso8583Message response = null;
        public OnReceiveResponseDelegate onReceiveResponse;
        private int id;
        CsSender sender = new CsSender("192.168.7.235",16000);
    

        #endregion


        #region Construct
        public CsTransaction(string ip, int port, string TermCode, string CardAccID, string TerminalName, string AcceptorBINCode , int TimeOut)
        {
            _ip = ip;
            _port = port;
            timeOut = TimeOut;
            _terminalName = TerminalName;
            _terminalID = TermCode.PadLeft(8, '0');
            _cardAcceptorID = CardAccID;
            ///sender = new CsSender(_ip, _port);
            ///sender.TimeOut = TimeOut;
            _AcceptorBINCode = AcceptorBINCode;
            //id = IDGenerator.getInstance().getNextID("CsTransaction");
            
        }
        #endregion

        #region properties

        public int ID
        {
            get
            {
                return id;
            }
        }
        
        public Iso8583Message Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;
            }
        }

        #endregion

        #region methods   
      
        
        
        private string Hex2Str(string srIn)
        {
            return System.Text.Encoding.Unicode.GetString(csUtil.HexToBin(srIn));
        }

        private byte[] Str2Byte(string str)
        {
            byte[] res = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
                res[i] = (byte)str[i];
            return res;
        }

        public byte[] GetKeys(KeyType keyType, Iso8583Message MsgIn)
        {
            string POS;
            string serial;
            try
            {
                byte[] MACK, PINK;


                if (MsgIn.MessageTypeIdentifier == 0800)
                {
                    serial = MsgIn.Fields[53].Value.ToString();
                    csUtil.ExtractPinMacKeys(out MACK, out PINK, serial);
                }
                else
                {
                    if (MsgIn.Fields.Contains(41))
                        POS = MsgIn.Fields[41].Value.ToString();
                    else
                        POS = "";
                    return null;
                }

                if (keyType == KeyType.MACKey)
                    return MACK;
                else
                    return PINK;

            }
            catch (Exception ex)
            {
                
                return null;
            }

        }

        private string GetYYYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        private string GetYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        #region Transactions

        public void start()
        {
            Trx2Do();
        }

        public bool Trx2Do()
        {
            bool hasResponse = false;
            switch (_csParam.trxType)
            {
                case 3:
                    if (!AuthTrx())
                        return false;
                    break;
                case 1://مانده گيري
                case 4://پراخت قبض
                case 22://خريد
                case 5:
                case 6:
                case 7:
                    if (!FinancialTrx())
                        return false;
                    hasResponse = true;
                    break;
                case 10://  درخواست واریز
                    if (!FinancialConfirmTrx())
                        return false;
                    hasResponse = true;
                    break;
                case 15:
                    if (!ReversalTrx())
                        return false;
                    hasResponse = true;
                    break;
                case 26:
                    if (!SignOn())
                        return false;
                    hasResponse = true;
                    break;

                default:
                    return false;
            }



            _csParam.messageType = ((Iso8583Message)inMessage).MessageTypeIdentifier;
            _csParam.isresponse = true;



            bool result = false;
            sender.SendPacketSync(inMessage);
            _messageout = sender.TrxMessage;
            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getPinKey(index);
            byte[] key = csUtil.HexToBin(keyStr);

            byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
            string ss = csUtil.BinToHex( csUtil.DesBuffer(csUtil.HexToBin(_messageout.Fields[48].ToString().Split(str[0])[12]) , key , EncryptDecrypt.Decrypt));

            CheckMAC((Iso8583Message)_messageout);
            return result;
        }

        private bool SignOn()
        {
            inMessage.MessageTypeIdentifier = 0800;
            inMessage.Fields.Add(3, "900000");
            inMessage.Fields.Add(11, _csParam.AuditNumber.ToString().PadLeft(6, '0'));
            inMessage.Fields.Add(12, GetYYMMDDhhmmss().Substring(6, 6));
            inMessage.Fields.Add(13, GetYYYYMMDDhhmmss().Substring(0, 8));
            inMessage.Fields.Add(25, "14");
            inMessage.Fields.Add(32, "628023");

            inMessage.Fields.Add(41, "5537".PadLeft(8, '0'));
            inMessage.Fields.Add(42, "22869".PadLeft(15, '0'));
            inMessage.Fields.Add(53, "29533925");

            //inMessage.Fields.Add(41, "25991".PadLeft(8, '0'));
            //inMessage.Fields.Add(42, "77114".PadLeft(15, '0'));
            //inMessage.Fields.Add(53, "70393066");

            byte[] bte = { 0x1c };
            string str = System.Text.Encoding.ASCII.GetString(bte);
            inMessage.Fields.Add(48, "000000" + str + "03.00.05" + str);
            inMessage.Fields.Add(64, "0000000000000000");
            SetMAC(ref inMessage, GetKeys(KeyType.MACKey, inMessage));
            return true;
        }

        private bool FinancialTrx()
        {
            try
            {
                inMessage.Fields.Clear();
                inMessage.MessageTypeIdentifier = _csParam.messageType;
                inMessage.Fields.Add(2, _csParam.PAN);
                switch (_csParam.trxType)
                {
                    case 22:
                        inMessage.Fields.Add(3, PURCH_PROCESSCODE);
                        inMessage.Fields.Add(4, _csParam.Amount.PadLeft(12, '0'));
                        break;
                    case 1:
                        inMessage.Fields.Add(3, REM_PROCESSCODE);
                        break;            
                    case 4:
                    case 10:
                        inMessage.Fields.Add(3, BILLPAY_PROCESSCODE);
                        inMessage.Fields.Add(4, _csParam.Amount.PadLeft(12, '0'));
                        break;
                    case 5:
                        inMessage.Fields.Add(3, TRANSC2C_PROCESSCODE);
                        inMessage.Fields.Add(4, _csParam.Amount.PadLeft(12, '0'));
                        break;
                    case 6:
                        inMessage.Fields.Add(3, PIN_PROCESSCODE);
                        inMessage.Fields.Add(4, _csParam.Amount.PadLeft(12, '0'));
                        break;
                    case 7:
                        inMessage.Fields.Add(3, STATE_PROCESSCODE);
                        inMessage.Fields.Add(4, _csParam.Amount.PadLeft(12, '0'));
                        break;
                    default:
                        return false;
                }
                _csParam.DateTimeTrx = GetYYYYMMDDhhmmss();
                inMessage.Fields.Add(11, _csParam.AuditNumber.ToString().PadLeft(6, '0'));
                inMessage.Fields.Add(12, _csParam.DateTimeTrx.Substring(8, 6));
                inMessage.Fields.Add(13, _csParam.DateTimeTrx.Substring(0, 8));
                inMessage.Fields.Add(25, "14");
                
                inMessage.Fields.Add(32, "628023");
                inMessage.Fields.Add(35, _csParam.isoTrack);
                inMessage.Fields.Add(37, _csParam.RefrenceNo.PadLeft(12, '0'));
                inMessage.Fields.Add(41, "5537".PadLeft(8, '0'));
                inMessage.Fields.Add(42, "22869".PadLeft (15, '0'));
                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                switch (_csParam.trxType)
                {
                    case 5:
                        inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str + _csParam.DestinationPAN + str);
                        break;
                    case 4:
                        inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str + _csParam.BillID + str + _csParam.PayID + str);
                        break;
                    case 1:
                    case 22:
                    case 7:
                        inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str);
                        break;
                    case 6:
                        inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str + _csParam.DestinationPAN + str);
                        break;
                }

                inMessage.Fields.Add(49, IRAN_CURRENCY_CODE);
                inMessage.Fields.Add(53, "29533925");
                inMessage.Fields.Add(52, csUtil.BinToHex(_csParam.PINBlock));
                if(_csParam.trxType == 6)
                    inMessage.Fields.Add(98, CsParam.DestinationPAN.PadLeft(25,'0'));
                else
                    inMessage.Fields.Add(98, "0000000000000000000000000");

                inMessage.Fields.Add(128, DefMAC);
                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                byte[] key = csUtil.HexToBin(keyStr);
                bool result = SetMAC(ref inMessage, key);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool AuthTrx()
        {
            try
            {
                inMessage.Fields.Clear();
                inMessage.MessageTypeIdentifier = _csParam.messageType;
                inMessage.Fields.Add(2, _csParam.PAN);
                inMessage.Fields.Add(3, AUTH_PROCESSCODE);
                _csParam.DateTimeTrx = GetYYYYMMDDhhmmss();
                inMessage.Fields.Add(11, _csParam.AuditNumber.ToString().PadLeft(6, '0'));
                inMessage.Fields.Add(12, _csParam.DateTimeTrx.Substring(8, 6));
                inMessage.Fields.Add(13, _csParam.DateTimeTrx.Substring(0, 8));
                inMessage.Fields.Add(25, "14");

                inMessage.Fields.Add(32, "628023");
                inMessage.Fields.Add(35, _csParam.isoTrack);
                inMessage.Fields.Add(37, _csParam.RefrenceNo.PadLeft(12, '0'));
                inMessage.Fields.Add(41, "5537".PadLeft(8, '0'));
                inMessage.Fields.Add(42, "22869".PadLeft(15, '0'));
                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str + _csParam.DestinationPAN + str);

                inMessage.Fields.Add(49, IRAN_CURRENCY_CODE);
                inMessage.Fields.Add(53, "29533925");
                inMessage.Fields.Add(52, csUtil.BinToHex(_csParam.PINBlock));
                inMessage.Fields.Add(98, "0000000000000000000000000");
                inMessage.Fields.Add(128, DefMAC);
                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                byte[] key = csUtil.HexToBin(keyStr);
                bool result = SetMAC(ref inMessage, key);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool FinancialConfirmTrx()
        {
            try
            {
                ++_csParam.AuditNumber;
                _inMessage = (Iso8583Message)inMessage.Clone();
                inMessage.Fields.Clear();
                _csParam.DateTimeTrx = GetYYYYMMDDhhmmss();
                inMessage.MessageTypeIdentifier = 0500;
                inMessage.Fields.Add(3,"920000");
                inMessage.Fields.Add(11, _csParam.AuditNumber.ToString().PadLeft(6, '0'));
                inMessage.Fields.Add(12, _csParam.DateTimeTrx.Substring(8, 6));
                inMessage.Fields.Add(13, _csParam.DateTimeTrx.Substring(0, 8));
                inMessage.Fields.Add(25, "14");

                inMessage.Fields.Add(32, "628023");
                inMessage.Fields.Add(41, "5537".PadLeft(8, '0'));
                inMessage.Fields.Add(42, "22869".PadLeft(15, '0'));
                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                inMessage.Fields.Add(48, (_csParam.AuditNumber - 1).ToString().PadLeft(6, '0') + str + "03.00.05" + str);

                inMessage.Fields.Add(53, "29533925");

                inMessage.Fields.Add(64, DefMAC);
                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                byte[] key = csUtil.HexToBin(keyStr);
                bool result = SetMAC(ref inMessage, key);
                
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
        private bool ReversalTrx()
        {
            try
            {
                ++_csParam.AuditNumber;
                _inMessage = (Iso8583Message)inMessage.Clone(); 
                inMessage.Fields.Clear();
                inMessage.MessageTypeIdentifier = 0420;
                inMessage.Fields.Add(2, _csParam.PAN);
                inMessage.Fields.Add(3, _csParam.ProcessCode);
                inMessage.Fields.Add(4, _csParam.Amount.ToString().PadLeft(12, '0'));
                _csParam.DateTimeTrx = GetYYYYMMDDhhmmss();
                inMessage.Fields.Add(11, _csParam.AuditNumber.ToString().PadLeft(6, '0'));
                inMessage.Fields.Add(12, _csParam.DateTimeTrx.Substring(8, 6));
                inMessage.Fields.Add(13, _csParam.DateTimeTrx.Substring(0, 8));
                inMessage.Fields.Add(25, "14");

                inMessage.Fields.Add(32, "628023");
                inMessage.Fields.Add(37, _inMessage.Fields[37].ToString());
                inMessage.Fields.Add(41, "5537".PadLeft(8, '0'));
                inMessage.Fields.Add(42, "22869".PadLeft(15, '0'));
                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                inMessage.Fields.Add(48, _inMessage.Fields[11].ToString().PadLeft(6, '0') + str + "03.00.05" + str + _inMessage.Fields[11].ToString() + str);
                inMessage.Fields.Add(49, IRAN_CURRENCY_CODE);
                inMessage.Fields.Add(53, "29533925");
                inMessage.Fields.Add(95, "0".PadLeft(42,'0'));
                inMessage.Fields.Add(90, (_inMessage.Fields[11].ToString().PadLeft(6, '0') + _inMessage.Fields[13].ToString() + _inMessage.Fields[12].ToString()).PadRight(42,'0'));

                inMessage.Fields.Add(98, "0000000000000000000000000");
                inMessage.Fields.Add(128, DefMAC);
                int index = 0;
                string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
                byte[] key = csUtil.HexToBin(keyStr);
                bool result = SetMAC(ref inMessage, key);

                return result;
            }
            catch(Exception ex)
            {
                string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                System.IO.StreamWriter sw = new StreamWriter(@"c:\logerr" + date + ".txt", true);
                sw.WriteLine(ex.Message);
                sw.Close(); 
                return false;
            }
        }

        #endregion Transactions


        #endregion
    }
}

