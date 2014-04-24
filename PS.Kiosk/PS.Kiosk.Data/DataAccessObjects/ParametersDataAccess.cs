using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Framework;
using PS.Kiosk.Framework.ExceptionManagement;



namespace PS.Kiosk.Data.DataAccessObjects
{
    public class ParametersDataAccess
    {
        KioskDataEntities KioskDataEntities = new KioskDataEntities();
        //tblTransactions Trans;
        private static ParametersDataAccess _instance;
        public static ParametersDataAccess Instance
        {
            get { return GetInstance(); }
        }
        private byte[] _PinBlock;



        private ParametersDataAccess()
        {
            SetCutOffDateTime();
           
        }

        private static ParametersDataAccess GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock ("CsReaderGetInstance")
                    {
                        if (_instance == null)
                        {
                            _instance = new ParametersDataAccess();
                            
                        }
                    }
                }
                return _instance;
            }
            catch (Exception ex)
            {
                KioskLogger.Instance.LogMessage(ex, "Error in ParametersDataAccess GetInstance()");
            }

            return null;
        }

        public void Clear()
        {
            _PinBlock = null; 
        }

        private tblTransactions _transReq;
        private tblTransactions TransReq
        {
            get
            {
                try
                {
                   
                    IQueryable<tblTransactions> trans = KioskDataEntities.tblTransactions.OrderBy(t => t.ID);
                    if (trans!= null && trans.Count() > 0)
                    {
                        _transReq = trans.ToList().Last();

                    }

                    
                }
                catch (Exception EX)
                {

                    KioskLogger.Instance.LogMessage(EX,"");
                }

                return _transReq;
            }
            
        }

        private tblTransactionsReply _transReply;
        private tblTransactionsReply TransReply
        {
            get
            {
                List<tblTransactionsReply> trans = KioskDataEntities.tblTransactionsReply.OrderBy(t => t.ID).ToList();
                if (trans.Count > 0)
                {
                    _transReply = trans.Last();

                }

                return _transReply;
            }

        }

        private tblReversalTrans _ReversalTran;
        private tblReversalTrans ReversalTran
        {
            get
            {
                List<tblReversalTrans> trans = KioskDataEntities.tblReversalTrans.OrderBy(t => t.ID).ToList();
                if (trans.Count > 0)
                {
                    _ReversalTran = trans.Last();

                }

                return _ReversalTran;
            }

        }

        private tblSettleTrans _SettleTran;
        private tblSettleTrans SettleTran
        {
            get
            {
                List<tblSettleTrans> trans = KioskDataEntities.tblSettleTrans.OrderBy(t => t.ID).ToList();
                if (trans.Count > 0)
                    _SettleTran = trans.Last();
                return _SettleTran;
            }
            
        }

        private Int64 _LastSuceedStan;
        public Int64 LastSuceedStan
        {
            get
            {
                //باید یکی باشد پس فرقی نمی کند کدام را بفرستیم چون یکی است  Stan در تراکنش رفت و برگشت 
                List<tblTransactionsReply> trans = KioskDataEntities.tblTransactionsReply.Where(t => t.ReplyCode == 0).OrderBy(t => t.ID).ToList();
                if (trans.Count > 0)
                {
                    _LastSuceedStan = Convert.ToInt64(trans.Last().Stan);

                }
                else
                    _LastSuceedStan = 0;

                return _LastSuceedStan;
            }
            
        }

       

        public int NewStan
        {
            get
            {
                if (TransReq != null)
                {
                    int lastStan;
                    if (int.TryParse(TransReq.Stan, out lastStan))
                    {
                        _NewTranRefNumber = (lastStan >= 999999) ? 1 : lastStan + 1;
                        return _NewTranRefNumber;
                    }
                    else
                    {
                        throw new CustomException("Stan value is undefined.");
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        private int _NewTranRefNumber;
        public int NewTranRefNumber
        {
            get
            {
                return _NewTranRefNumber;
                //if (TransReq != null)
                //{
                //    Int64 lastTranRefNumber;
                //    if (Int64.TryParse(TransReq.RefrenceNumber, out lastTranRefNumber))
                //    {
                //        return (lastTranRefNumber >= 11199999999)? 1 : lastTranRefNumber + 1;
                //    }
                //    else
                //    {
                //        throw new CustomException("Stan value is undefined.");
                //    }
                //}
                //else
                //{
                //    return 11100000001;
                //}
            }
        }

        public Int64 NewReqID
        {
            get
            {
                if (TransReq != null)
                {
                    Int64 id;
                    if (Int64.TryParse( Convert.ToString( TransReq.ID) , out id))
                    {
                        return id + 1;
                    }
                    else
                    {
                        throw new CustomException("ID value is undefined.");
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        public Int64 NewReplyID
        {
            get
            {
                if (TransReply != null)
                {
                    Int64 id;
                    if (Int64.TryParse(Convert.ToString(TransReply.ID), out id))
                    {
                        return id + 1;
                    }
                    else
                    {
                        throw new CustomException("ID value is undefined.");
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        public Int64 NewReversalID
        {
            get
            {
                if (ReversalTran != null)
                {
                    Int64 id;
                    if (Int64.TryParse(Convert.ToString(ReversalTran.ID), out id))
                    {
                        return  id + 1;
                    }
                    else
                    {
                        throw new CustomException("ID value is undefined.");
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        public Int64 NewSettleID
        {
            get
            {
                if (SettleTran != null)
                {
                    Int64 id;
                    if (Int64.TryParse(Convert.ToString(SettleTran.ID), out id))
                    {
                        return id + 1;
                    }
                    else
                    {
                        throw new CustomException("ID value is undefined.");
                    }
                }
                else
                    return 1;
            }
        }

        private List<tblReversalTrans> _ReversalList;
        /// <summary>
        /// ليستی از آخرین تراکنشهای مانده در بانک برای برگشت
        /// </summary>
        public List<tblReversalTrans> ReversalList
        {
            get
            {
                KioskDataEntities.Dispose();
                _ReversalList = null;


                #region CuttOff

                //KioskDataEntities = new KioskDataEntities();
                //IEnumerable<tblReversalTrans> tran = KioskDataEntities.tblReversalTrans.ToList().Where
                //   (t => DateTime.Now.Year == (Convert.ToInt32(t.PrimaryDateTime.Substring(0, 4))) &&
                //        DateTime.Now.Month == (Convert.ToInt32(t.PrimaryDateTime.Substring(4, 2))) &&
                //DateTime.Now.Day <= (Convert.ToInt32(t.PrimaryDateTime.Substring(6, 2)) + ReversalCutOffDay) &&
                //    DateTime.Now.TimeOfDay.Hours <= ParametersDataAccess.ReversalCutOffTime);
                #endregion CuttOff

                #region Before4Min Left

                //KioskDataEntities = new KioskDataEntities();
                //IEnumerable<tblReversalTrans> tran = KioskDataEntities.tblReversalTrans.ToList().Where
                //   (t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                //        DateTime.Now.Month == t.SendDateTime.Date.Month &&
                //DateTime.Now.Day == t.SendDateTime.Date.Day &&
                //DateTime.Now.TimeOfDay - t.SendDateTime.TimeOfDay <= TimeSpan.FromMinutes(4));

                #endregion Before4Min Left

                KioskDataEntities = new KioskDataEntities();
                IEnumerable<tblReversalTrans> tran = KioskDataEntities.tblReversalTrans.ToList().Where
                   (t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                        DateTime.Now.Month == t.SendDateTime.Date.Month &&
                DateTime.Now.Day == t.SendDateTime.Date.Day);


                if (tran.Count() > 0)
                    _ReversalList = tran.OrderBy(t => t.ID).ToList<tblReversalTrans>();

                return _ReversalList;
            }
           
        }

        private List<tblSettleReversTrans> _ReversalSettleList93;
        /// <summary>
        /// ليستی از آخرین تراکنشهای مانده در بانک برای برگشت
        /// </summary>
        public List<tblSettleReversTrans> ReversalSettleList93
        {
            get
            {
                KioskDataEntities.Dispose();
                _ReversalSettleList93 = null;

                KioskDataEntities = new KioskDataEntities();
                IEnumerable<tblSettleReversTrans> tran = KioskDataEntities.tblSettleReversTrans.ToList().Where(t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                        DateTime.Now.Month == t.SendDateTime.Date.Month && DateTime.Now.Day == t.SendDateTime.Date.Day);


                if (tran.Count() > 0)
                    _ReversalSettleList93 = tran.OrderBy(t => t.ID).ToList<tblSettleReversTrans>();

                return _ReversalSettleList93;
            }

        }

        private List<tblReversalTrans> _ReversalListBeforConnect;
        /// <summary>
        /// ليستی از آخرین تراکنشهای مانده در بانک برای برگشت
        /// </summary>
        public List<tblReversalTrans> ReversalListBeforConnect
        {
            get
            {
                try
                {
                    KioskLogger.Instance.LogMessage("Enter ReversalListBeforConnect");
                    KioskDataEntities.Dispose();
                    _ReversalListBeforConnect = null;


                    #region CuttOff

                    KioskDataEntities = new KioskDataEntities();
                    KioskLogger.Instance.LogMessage("Create Instance of KioskDataEntities");

                    IEnumerable<tblReversalTrans> tran = KioskDataEntities.tblReversalTrans.ToList().Where
                       (t => DateTime.Now.Year == (Convert.ToInt32(t.PrimaryDateTime.Substring(0, 4))) &&
                            DateTime.Now.Month == (Convert.ToInt32(t.PrimaryDateTime.Substring(4, 2))) &&
                    DateTime.Now.Day <= (Convert.ToInt32(t.PrimaryDateTime.Substring(6, 2)) + ReversalCutOffDay) &&
                        DateTime.Now.TimeOfDay.Hours <= ParametersDataAccess.ReversalCutOffTime);

                    #endregion CuttOff



                    if (tran.Count() > 0)
                        _ReversalListBeforConnect = tran.OrderBy(t => t.ID).ToList<tblReversalTrans>();

                    return _ReversalListBeforConnect;
                }
                catch (Exception EX)
                {

                    throw EX;
                }
            }

        }

        private List<tblSettleTrans> _SettleList;
        /// <summary>
        /// ليستی از آخرین تراکنشهای مانده در بانک برای پرداخت
        /// </summary>
        public List<tblSettleTrans> SettleList
        {
            get
            {
                KioskDataEntities.Dispose();
                KioskDataEntities = new Data.KioskDataEntities();
                _SettleList = null;

                #region CuttOff

                //IEnumerable<tblSettleTrans> tran = KioskDataEntities.tblSettleTrans.ToList().Where
                //   (t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                //        DateTime.Now.Month == t.SendDateTime.Date.Month &&
                //DateTime.Now.Day <= (t.SendDateTime.Date.Day + ParametersDataAccess.SettleCutOffDay) &&
                //    DateTime.Now.TimeOfDay.Hours <= ParametersDataAccess.SettleCutOffTime);

                #endregion CuttOff

                #region Before4Min Left
                //IEnumerable<tblSettleTrans> tran = KioskDataEntities.tblSettleTrans.ToList().Where
                //   (t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                //        DateTime.Now.Month == t.SendDateTime.Date.Month &&
                //DateTime.Now.Day == t.SendDateTime.Date.Day &&
                //DateTime.Now.TimeOfDay - t.SendDateTime.TimeOfDay <= TimeSpan.FromMinutes(4));

                #endregion  Before4Min Left

                IEnumerable<tblSettleTrans> tran = KioskDataEntities.tblSettleTrans.ToList().Where
               (t => DateTime.Now.Year == t.SendDateTime.Date.Year &&
                    DateTime.Now.Month == t.SendDateTime.Date.Month &&
            DateTime.Now.Day == t.SendDateTime.Date.Day);

                if (tran.Count() > 0)
                    _SettleList = tran.OrderBy(t => t.ID).ToList<tblSettleTrans>();

                return _SettleList;
            }

        }
        
        public byte[] PinBlock
        {
            get
            {
                if (_PinBlock == null)
                {
                    //StateManager.Instance.Current.Error("NoPinBlock!");
                    KioskLogger.Instance.LogMessage("NoPinBlock");
                }
                return _PinBlock;
            }
            set { _PinBlock = value; }
        }

        public string BankAcceptorId
        {
            get
            {
                tblParameters BankAcceptorIdParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "BankAcceptorId").FirstOrDefault();
                if (BankAcceptorIdParam != null)
                {
                    if (BankAcceptorIdParam.ParamValue != string.Empty)
                    {
                        return BankAcceptorIdParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("BankAcceptorId value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no BankAcceptorId ParameterKey in database.");
                }
            }
        }

        public string IP
        {
            get
            {
                tblParameters IPParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "IP").FirstOrDefault();
                if (IPParam != null)
                {                    
                    if (IPParam.ParamValue != string.Empty)
                    {
                        return IPParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("IP value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no IP ParameterKey in database.");
                }
            }
        }

        public int Port
        {
            get
            {
                tblParameters PortParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "Port").FirstOrDefault();
                if (PortParam != null)
                {
                    int port;
                    if (int.TryParse(PortParam.ParamValue, out port))
                    {
                        return port;
                    }
                    else
                    {
                        throw new Exception("Port value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no Port ParameterKey in database.");
                }
            }
        }

        public string TerminalAcceptorId
        {
            get
            {
                tblParameters TerminalAcceptorIdParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "TerminalAcceptorId").FirstOrDefault();
                
                if (TerminalAcceptorIdParam != null)
                {
                    if (TerminalAcceptorIdParam.ParamValue != string.Empty)
                    {
                        return TerminalAcceptorIdParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("TerminalAcceptorId value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no TerminalAcceptorId ParameterKey in database.");
                }
            }
        }

        public string TerminalAcceptorName
        {
            get
            {
                tblParameters TerminalAcceptorNameParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "TerminalAcceptorName").FirstOrDefault();
                if (TerminalAcceptorNameParam != null)
                {
                    if (TerminalAcceptorNameParam.ParamValue != string.Empty)
                    {
                        return TerminalAcceptorNameParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("TerminalAcceptorName value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no TerminalAcceptorName ParameterKey in database.");
                }
            }
        }

        public string CardAcceptorId
        {
            get
            {
                tblParameters CardAcceptorIdParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "CardAcceptorId").FirstOrDefault();
                if (CardAcceptorIdParam != null)
                {
                    if (CardAcceptorIdParam.ParamValue != string.Empty)
                    {
                        return CardAcceptorIdParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("CardAcceptorId value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no CardAcceptorId ParameterKey in database.");
                }
            }
        }

        public string CardAcceptorBinCode
        {
            get
            {
                tblParameters CardAcceptorBinCodeParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "CardAcceptorBinCode").FirstOrDefault();
                if (CardAcceptorBinCodeParam != null)
                {
                    if (CardAcceptorBinCodeParam.ParamValue != string.Empty)
                    {
                        return CardAcceptorBinCodeParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("CardAcceptorBinCode value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no CardAcceptorBinCode ParameterKey in database.");
                }
            }
        }

        public int TimeOut
        {
            get
            {
                tblParameters TimeOutParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "TimeOut").FirstOrDefault();
                if (TimeOutParam != null)
                {
                    int timeOut;
                    if (int.TryParse(TimeOutParam.ParamValue, out timeOut))
                    {
                        return timeOut;
                    }
                    else
                    {
                        throw new Exception("TimeOut value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no TimeOut ParameterKey in database.");
                }
            }
        }

        public int DeviceCodeP25
        {
            get
            {
                tblParameters DeviceCodeP25Param = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "DeviceCodeP25").FirstOrDefault();
                if (DeviceCodeP25Param != null)
                {
                    int deviceCodeP25;
                    if (int.TryParse(DeviceCodeP25Param.ParamValue, out deviceCodeP25))
                    {
                        return deviceCodeP25;
                    }
                    else
                    {
                        throw new Exception("DeviceCodeP25 value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no DeviceCodeP25 ParameterKey in database.");
                }
            }
        }

        public string TerminalSerialNumberP53
        {
            get
            {
                tblParameters TerminalSerialNumberP53Param = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "TerminalSerialNumberP53").FirstOrDefault();
                if (TerminalSerialNumberP53Param != null)
                {
                    if (TerminalSerialNumberP53Param.ParamValue != string.Empty)
                    {
                        return TerminalSerialNumberP53Param.ParamValue;
                    }
                    else
                    {
                        throw new Exception("TerminalSerialNumberP53 value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no TerminalSerialNumberP53 ParameterKey in database.");
                }
            }
        }

        public int CardReaderPort
        {
            get
            {
                tblParameters CardReaderPortParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "CardReaderPort").FirstOrDefault();
                if (CardReaderPortParam != null)
                {
                    int cardReaderPort;
                    if (int.TryParse(CardReaderPortParam.ParamValue, out cardReaderPort))
                    {
                        return cardReaderPort;
                    }
                    else
                    {
                        throw new Exception("CardReaderPort value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no CardReaderPort ParameterKey in database.");
                }
            }
        }

        public byte EPPPort
        {
            get
            {
                tblParameters EPPPortParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "EPPPort").FirstOrDefault();
                if (EPPPortParam != null)
                {
                    byte eppPort;
                    if (byte.TryParse(EPPPortParam.ParamValue, out eppPort))
                    {
                        return eppPort;
                    }
                    else
                    {
                        throw new Exception("EPPPort value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no EPPPort ParameterKey in database.");
                }
            }
        }

        public string ReversalFileName
        {
            get
            {
                tblParameters ReversalFileNameParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "ReversalFileName").FirstOrDefault();
                if (ReversalFileNameParam != null)
                {
                    if (ReversalFileNameParam.ParamValue != string.Empty)
                    {
                        return ReversalFileNameParam.ParamValue;
                    }
                    else
                    {
                        throw new Exception("ReversalFileName value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no ReversalFileName ParameterKey in database.");
                }
            }
        }

        public int CurrencyCode
        {
            get
            {
                tblParameters CurrencyCodeParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "CurrencyCode").FirstOrDefault();
                if (CurrencyCodeParam != null)
                {
                    int currencyCode;
                    if (int.TryParse(CurrencyCodeParam.ParamValue, out currencyCode))
                    {
                        return currencyCode;
                    }
                    else
                    {
                        throw new Exception("CurrencyCode value is undefined.");
                    }
                }
                else
                {
                    throw new Exception("There is no CurrencyCode ParameterKey in database.");
                }
            }
        }

       
        /// <summary>
        /// تعداد روزهای مهلت انجام تراکنش برگشت
        /// </summary>
        public static int ReversalCutOffDay;

       
        /// <summary>
        /// مهلت زمانی انجام تراکنش برگشت 
        /// </summary>
        public static int ReversalCutOffTime;

       
        /// <summary>
        /// تعداد روزهای مهلت انجام تراکنش پرداخت
        /// </summary>
        public static int SettleCutOffDay;

        
        /// <summary>
        /// مهلت زمانی انجام تراکنش پرداخت 
        /// </summary>
        public static int SettleCutOffTime;

        private static string _RestartTime;
        /// <summary>
        /// زمان لاگ آن مجدد
        /// </summary>
        public static string RestartTime
        {
            get
            {
                tblParameters Param = new KioskDataEntities().tblParameters.Where(p => p.ParamKey == "ApplicationRestartHour").FirstOrDefault();
                if (Param != null)
                {
                    if (Param.ParamValue != string.Empty)
                    {
                        _RestartTime = Param.ParamValue;
                        return _RestartTime;
                    }
                    else
                    {
                        _RestartTime = "000000";
                    }
                }
                else
                {
                    _RestartTime = "000000";
                }

                return _RestartTime;
            }
            
        }
      
        private bool _DetectForCardCapturing;
        /// <summary>
        /// در صورت لزوم کارت ضبط شود یا خیر
        /// </summary>
        public bool DetectForCardCapturing
        {
            get
            {
                tblParameters cardCapturing = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "DetectForCardCapturing").FirstOrDefault();
                if (cardCapturing != null)
                {
                    int CardCapture;
                    if (int.TryParse(cardCapturing.ParamValue, out CardCapture))
                    {
                        _DetectForCardCapturing = CardCapture == 1 ? true : false;
                        
                    }
                    else
                    {
                        _DetectForCardCapturing = false;
                    }
                }
                else
                {
                    _DetectForCardCapturing = false;
                }
                return _DetectForCardCapturing;
            }
            
        }

        private void SetCutOffDateTime()
        {
            try
            {
                KioskLogger.Instance.LogMessage("Enter for SetCutOffDateTime");
                tblParameters Param = null;
                IQueryable<tblParameters> tblParam = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "ReversalCutOffTime");
                KioskLogger.Instance.LogMessage("get tblParam Successfully");

                if (tblParam.Count() > 0)
                    Param = tblParam.FirstOrDefault();

                if (Param != null)
                {
                    string[] val = Param.ParamValue.Split(new char[] { ',' });

                    if (int.TryParse(val[0], out ReversalCutOffDay) == false)
                        ReversalCutOffDay = 0;

                    if (int.TryParse(val[1], out ReversalCutOffTime) == false)
                        ReversalCutOffTime = 23;


                }
                else
                {
                    ReversalCutOffDay = 0;
                    ReversalCutOffTime = 23;
                }
                tblParameters Param2 = null;
                IQueryable<tblParameters> tblParam2 = KioskDataEntities.tblParameters.Where(p => p.ParamKey == "SettleCutOffTime");
                KioskLogger.Instance.LogMessage("get tblParam2 Successfully");
                if (tblParam2.Count() > 0)
                    Param2 = tblParam2.FirstOrDefault();

                if (Param2 != null)
                {

                    string[] val2 = Param2.ParamValue.Split(new char[] { ',' });
                    if (int.TryParse(val2[0], out SettleCutOffDay) == false)
                        SettleCutOffDay = 1;

                    if (int.TryParse(val2[1], out SettleCutOffTime) == false)
                        SettleCutOffTime = 23;


                }
                else
                {
                    SettleCutOffDay = 1;
                    SettleCutOffTime = 23;
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX , "Error in SetCutOffDateTime");
                throw EX;
            }
        }
       
         
    }
}
