using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Messaging.Operations;
using PS.Kiosk.Common.Model;
using Fanap.Messaging.Iso8583;
using Fanap.Messaging;
using PS.Kiosk.DeviceController.Services;
using PS.Kiosk.Framework.ExceptionManagement;
using PS.Kiosk.Framework;
using Fanap.Utilities;

namespace PS.Kiosk.Messaging.MsgCall
{
    public class Transactions : CsAgent
    {
        private Transactions()
        {

        }

        private static Transactions _TransactionsInstance;

        public static Transactions TransactionsInstance
        {
            get
            {
                if (Transactions._TransactionsInstance == null)
                {
                    Transactions._TransactionsInstance = new Transactions();
                   
                }
                return Transactions._TransactionsInstance;
            }
            
        }
        #region Property

        CsSender _Sender;
        private CsSender Sender
        {
            get
            {
                if (_Sender == null)
                {
                   
                    _Sender = new CsSender(this._ip, this._port);
                }
                return _Sender;
            }

        }

        private Fanap.Messaging.Message outMessage;

        private Iso8583Message inMessage = new Iso8583Message();

        private Enums.Transactions _TranType;
        /// <summary>
        /// تراکنش جاری در حال اجرا
        /// </summary>
        public Enums.Transactions TranType
        {
            get { return _TranType; }
            set { _TranType = value; }
        }

        #endregion Property

        

        #region Transaction Implementation

        public SignOnReplyParameters SignOn(SignOnParameters signonParam)
        {
            SignOnReplyParameters signonReply = new SignOnReplyParameters();
            TranType = Enums.Transactions.Signon;

            byte[] bte2 = { 0x00, 0x85, 0x60, 0x00, 0x13, 0x00, 0x01 };

            this._ip = signonParam.IP;
            this._port = signonParam.Port;
            CsSender c = Sender;

            
            this.CsParam.messageType = (int)signonParam.MsgType;
            this.CsParam.Language = 1;
            this.CsParam.trxType = (int)Enums.Transactions.Signon;
            this.CsParam.AuditNumber = signonParam.Stan;
            this.CsParam.RefrenceNo = Convert.ToString(signonParam.TranRefNumber);


            this._csParam.trxType = (int)Enums.Transactions.Signon;
            try
            {
                TimeSpan t1 = DateTime.Now.TimeOfDay;
                //Create Request Msg
                if (MakeSignOnMsg(signonParam))
                {
                    TimeSpan t2 = DateTime.Now.TimeOfDay;
                    TimeSpan Makereq = t2 - t1;
                    Console.WriteLine("Makereq = " + Makereq.ToString());

                    Trx2Do();

                    TimeSpan t33 = DateTime.Now.TimeOfDay;
                    if (this.outMessage.Fields[39] != null)
                        signonReply.ReplyCodeP39 = this.outMessage.Fields[39].ToString();

                    //Agar Filed39 dorost nabashad ., mack va Pink ra nemitavanad estekhraj konad va khata midahad
                    if (this.outMessage.Fields[39] == null || Convert.ToInt32(this.outMessage.Fields[39].Value.ToString()) != 0)
                    {
                        signonReply.TranSuccess = false;
                        signonReply.IsAuthenticated = false;
                        return signonReply;
                    }

                    //در فیلد 48 پاسخ 
                    string[] sss = this.outMessage.Fields[48].ToString().Split(';');
                    

                    string MACK = sss[14];
                    string PINK = sss[15];

                    KioskLogger.Instance.LogMessage("MAC = " + MACK);
                    KioskLogger.Instance.LogMessage("PINK = " + PINK);

                    //Call Business and send MACK AND PINK For injecting to the Masterkey
                    if (!EppService.Instance.SetKeyFromLoginReply(0, 0, 1, csUtil.HexToBin(MACK), csUtil.HexToBin(PINK)))
                    {
                        signonReply.IsAuthenticated = false;
                        throw new Exception("Could not Login");
                    }
                    KioskLogger.Instance.LogMessage("keys injected.");

                    signonParam.MsgFormat = Enums.SwitchMsgFormat.Shetab93;
                    signonReply.MsgType = Enums.MsgType.SignOnReply;

                    DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
                    signonReply.DateTimeP7 = datetime;
                    signonReply.TranTime = datetime.TimeOfDay;
                    signonReply.TranDate = datetime.Date;

                    signonReply.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);


                    signonReply.TerminalAcceptorId = (string)this.outMessage.Fields[41].Value;
                    signonReply.CardAcceptorId = (string)this.outMessage.Fields[42].Value;
                    signonReply.ExtraData = (string)this.outMessage.Fields[48].Value;
                    signonReply.MACP64 = (string)this.outMessage.Fields[64].Value;
                    signonReply.TranSuccess = true;

                    //اطلاعات پايه که معمولا برای چاپ استفاده می شود
                    BaseReplyParameters signonBase = null;
                    Set93Field48(signonReply.ExtraData, ref signonBase);

                    //تنظيم ساعت سیستم با سوييچ
                    if (!Utilities.UtilityMethods.SetLclTime(SignOnReplyParameters.SwitchSigonOnDateTime))
                        throw new CustomException("Could Not Set System Local Time");

                   
                    TimeSpan t4 = DateTime.Now.TimeOfDay;
                    TimeSpan total = t4 - t33;
                    Console.WriteLine("Total = " + total.ToString());
                    signonReply.IsAuthenticated = true;
                    KioskLogger.Instance.LogMessage("signon TRUE.");


                }
                else
                {
                    KioskLogger.Instance.LogMessage("signon FALSE.");
                    signonReply.TranSuccess = false;
                    signonReply.IsAuthenticated = false;
                }

            }
            catch (Exception EX)
            {
                signonReply.TranSuccess = false;
                signonReply.IsAuthenticated = false;

                if ((EX is CustomException) == false)
                    throw new CustomException("Could not Login", EX.Message);
                else
                    throw EX;
            }

            return signonReply;
        }

        /// <summary>
        /// دریافت مانده حساب(تراکنش مالی)
        /// </summary>
        /// <param name="InquiryParam">یک پیغام از این نوع</param>
        /// <returns>پاسخ</returns>
        public BalanceInquiryReplyParameteres GetBalanceInquery(BalanceInquiryParameteres InquiryParam)
        {
            
            //CsSender c = Sender;
            BalanceInquiryReplyParameteres BalanceInqueryReply = new BalanceInquiryReplyParameteres();
            this.CsParam.Language = 1;
            this.CsParam.trxType = (int)Enums.Transactions.REMAIN;
            this.CsParam.PAN = InquiryParam.CardNumberP2;

            TranType = Enums.Transactions.REMAIN;

            #region OLd
            //this.CsParam.Amount = Convert.ToString(InquiryParam.TranAmountP4);
            //this.CsParam.isoTrack = InquiryParam.IsoTrack;
            //this.CsParam.messageType = (int)InquiryParam.MsgType;
            //this.CsParam.PAN = InquiryParam.CardNumberP2;
            
            //this.CsParam.AuditNumber = InquiryParam.Stan;
            //this.CsParam.RefrenceNo = Convert.ToString(InquiryParam.TranRefNumber);
            //this.CsParam.PINBlock = InquiryParam.PinBlockP52;

            #endregion OLd

            try
            {
                if (MakeFinancial87Msg(InquiryParam))
                {
                    this.Trx2Do();
                    BalanceInqueryReply = this.MakeFinancial87ReplyMsg<BalanceInquiryReplyParameteres>(this.outMessage);

                   
                }
                else
                    throw new CustomException("Internal Exception", "Unsuccessfull Make Msg");
                


            }
            catch (Exception EX)
            {
                BalanceInqueryReply.TranSuccess = false;

                if ((EX is CustomException) == false)
                    throw new CustomException(EX.Message);
                else
                    throw EX;
            }

            return BalanceInqueryReply;
        }

        /// <summary>
        /// خرید (تراکنش مالی)
        /// </summary>
        /// <param name="PurchaseParams"></param>
        /// <returns></returns>
        public PurchaseReplyParameters Purchase(PurchaseParameters PurchaseParams)
        {
            PurchaseReplyParameters PurchaseReply = new PurchaseReplyParameters();

            this.CsParam.Amount = Convert.ToString(PurchaseParams.TranAmountP4);
            this.CsParam.isoTrack = PurchaseParams.IsoTrack;
            this.CsParam.messageType = (int)Enums.MsgType.Financial;
            this.CsParam.PAN = PurchaseParams.CardNumberP2;
            this.CsParam.Language = 1;
            this.CsParam.trxType = (int)Enums.Transactions.PAYMENT;
            this.CsParam.AuditNumber = PurchaseParams.Stan;
            this.CsParam.RefrenceNo = Convert.ToString(PurchaseParams.TranRefNumber);

            try
            {
                if (MakeFinancial87Msg(PurchaseParams))
                {
                    Trx2Do();
                    //PurchaseReply = (PurchaseReplyParameters)this.MakeFinancial87ReplyMsg(this.outMessage);
                    PurchaseReply.TranSuccess = true;
                }
                else
                    throw new Exception("Unsuccessfull Make Msg");
                


            }
            catch (Exception EX)
            {

                PurchaseReply.TranSuccess = false;
                PurchaseReply.ErrorCode = -1;
                PurchaseReply.ErrorMsg = EX.Message;
            }



            return PurchaseReply;
        }

        /// <summary>
        /// خرید شارژ
        /// </summary>
        /// <param name="PurchaseParams"></param>
        /// <returns></returns>
        public PurchaseChargeReplyParameters PurchaseCharge(PurchaseChargeParameters ChargeParams)
        {
            //CsSender c = Sender;
            PurchaseChargeReplyParameters chargeReply = null;

            this.CsParam.Language = 1;
            this.CsParam.trxType = (int)Enums.Transactions.Charge;

            TranType = Enums.Transactions.Charge;

            try
            {
                if (MakeFinancial87Msg <PurchaseChargeParameters>(ChargeParams))
                {
                    this.Trx2Do();

                    TimeSpan t3 = DateTime.Now.TimeOfDay;
                    chargeReply = this.MakeFinancial87ReplyMsg<PurchaseChargeReplyParameters>(this.outMessage);
                    TimeSpan t4 = DateTime.Now.TimeOfDay;
                    TimeSpan FinanceReply = t4 - t3;
                    Console.WriteLine("FinanceReply = " + FinanceReply.ToString());

                }
                else
                    throw new Exception("Unsuccessfull Make Msg");



            }
            catch (Exception EX)
            {
                if (chargeReply != null)
                    chargeReply.TranSuccess = false;
                throw EX;
                
            }

            
            return chargeReply;
 
        }

        /// <summary>
        /// اصلاحيه
        /// </summary>
        /// <param name="ReversalParam"></param>
        /// <returns></returns>
        public ReversalReplyParameters Reversal(ReversalParameters ReversalParam)
        {
            this._ip = ReversalParam.IP;
            this._port = ReversalParam.Port;

            CsSender c = Sender;

            ReversalReplyParameters ReversalReply = null; //= new ReversalReplyParameters();
            try
            {
                TranType = Enums.Transactions.REVERSAL;

                this.CsParam.Language = 1;
                this.CsParam.trxType = (int)Enums.Transactions.REVERSAL;

                if (MakeReversalMsg(ReversalParam))
                {
                    Trx2Do();
                    ReversalReply = MakeFinancial87ReplyMsg<ReversalReplyParameters>(this.outMessage);
                }

                return ReversalReply;
            }
            catch (Exception EX)
            {
                if (ReversalReply != null)
                    ReversalReply.TranSuccess = false;
                throw EX;
            }
        }

        public SettleReplyParameters Settle(SettleParameters SettleParam)
        {

            this._ip = SettleParam.IP;
            this._port = SettleParam.Port;

            CsSender c = Sender;

            SettleReplyParameters SettleReply = null; //= new ReversalReplyParameters();
            try
            {
                TranType = Enums.Transactions.Settele;

                this.CsParam.Language = 1;
                this.CsParam.trxType = (int)Enums.Transactions.Settele;

                if (MakeSettleMsg(SettleParam))
                {
                    Trx2Do();
                    SettleReply = MakeSettleReplyMsg(this.outMessage);
                }

                return SettleReply;
            }
            catch (Exception EX)
            {
                if (SettleReply != null)
                    SettleReply.TranSuccess = false;
                throw EX;
            }
        }

        public BillPaymentReplyParameters BillPay(BillPaymentParameters BillParam)
        {
            BillPaymentReplyParameters BillReply = null; //= new BillPaymentReplyParameters();
            try
            {
                TranType = Enums.Transactions.BILLPAY;

                this.CsParam.Language = 1;
                this.CsParam.trxType = (int)Enums.Transactions.BILLPAY;

                if (MakeFinancial87Msg <BillPaymentParameters>(BillParam))
                {
                    Trx2Do();
                    BillReply = MakeFinancial87ReplyMsg<BillPaymentReplyParameters>(this.outMessage);
                }

                return BillReply;
            }
            catch (Exception EX)
            {
                if (BillReply != null)
                    BillReply.TranSuccess = false;
                throw EX;
            }
        }

        public BillPaymentReplyParameters BillPay93(BillPaymentParameters BillParam)
        {
            BillPaymentReplyParameters BillReply = null; //= new BillPaymentReplyParameters();
            try
            {
                TranType = Enums.Transactions.BILLPAY;

                this.CsParam.Language = 1;
                this.CsParam.trxType = (int)Enums.Transactions.BILLPAY;

                if (MakeBillpay93Msg(BillParam))
                {
                    Trx2Do();
                    BillReply = MakeBillpay93ReplyMsg(this.outMessage);
                }

                return BillReply;
            }
            catch (Exception EX)
            {
                if (BillReply != null)
                    BillReply.TranSuccess = false;
                throw EX;
            }
        }

        public SpecialServiceReplyParameters SpecialServicePay(SpecialServiceParameters ServiceParam)
        {
             SpecialServiceReplyParameters SpecialServiceReply = null;
            try
            {
                TranType = Enums.Transactions.Jiring;

                this.CsParam.Language = 1;
                //this.CsParam.trxType = (int)Enums.Transactions.Jiring;

                if (MakeSpecialService<SpecialServiceParameters>(ServiceParam))
                {
                    
                    Trx2Do();
                    SpecialServiceReply = MakeSpecialService93Reply<SpecialServiceReplyParameters>(this.outMessage);

                    if (ServiceParam.MsgType == Enums.MsgType.BillInfo)
                    {
                        SpecialServiceReply.MsgType = Enums.MsgType.BillInfo;
                        SpecialServiceReply.TranAmountP4 =  (Convert.ToInt64(this.outMessage.Fields[4].Value.ToString().Trim()));
                    }
                }

                return SpecialServiceReply;

            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX, "Exception Came in SpecialServicePay");
                if (SpecialServiceReply != null)
                    SpecialServiceReply.TranSuccess = false;

                throw EX;
            }
        }

        public SettleReverse93ReplyParameters SettleReverse93(SettleReverse93Parameters ServiceParam)
        {
            this._ip = ServiceParam.IP;
            this._port = ServiceParam.Port;

            CsSender c = Sender;
            SettleReverse93ReplyParameters ServiceReply = null;
            try
            {
               this.CsParam.Language = 1;
                if (MakeSettleReversal93Msg(ServiceParam))
                {

                    Trx2Do();
                    ServiceReply = MakeSettleReversal93Reply(this.outMessage);

                    
                }

                return ServiceReply;

            }
            catch (Exception EX)
            {
                if (ServiceReply != null)
                    ServiceReply.TranSuccess = false;

                throw EX;
            }
        }

        #endregion Transaction Implementation

        #region PrivateMethods

        private void Trx2Do()
        {
            #region comment

            //bool hasResponse = false;
            //switch (_csParam.trxType)
            //{
            //    case 3:
            //        if (!AuthTrx())
            //            return false;
            //        break;
            //    case 1://مانده گيري
            //    case 4://پراخت قبض
            //    case 22://خريد
            //    case 5:
            //    case 6:
            //    case 7:
            //        if (!FinancialTrx())
            //            return false;
            //        hasResponse = true;
            //        break;
            //    case 10://  درخواست واریز
            //        if (!FinancialConfirmTrx())
            //            return false;
            //        hasResponse = true;
            //        break;
            //    case 15:
            //        if (!ReversalTrx())
            //            return false;
            //        hasResponse = true;
            //        break;
            //    case 26:
            //        if (!SignOnOpr(Params))
            //            return false;
            //        hasResponse = true;
            //        break;

            //    default:
            //        return false;
            //}


            #endregion comment

            _csParam.messageType = ((Iso8583Message)inMessage).MessageTypeIdentifier;
            _csParam.isresponse = true;



            if (!Sender.SendPacketSync(inMessage))
                throw new CustomException("Internal Exception", "Unsuccessfull Packet Send");



            outMessage = Sender.TrxMessage;
            if (this.outMessage == null)
                throw new CustomException("Internal Exception", "Invalid Reply Message");


            TimeSpan t33 = DateTime.Now.TimeOfDay;

            if (!CheckMAC((Iso8583Message)outMessage)) //اگر فیلد 39 معتبر بود لازم است خطای مک برگردانده شود
                if (this.outMessage.Fields[39] != null && Convert.ToInt32(this.outMessage.Fields[39].Value.ToString()) == 0)
                    throw new CustomException("Internal Exception", "Invalid MAC");
        }

        //*** Make Request Msg ****

        /// <summary>
        /// پیغام ارسالی را می سازد
        /// </summary>
        /// <param name="signonParam"></param>
        /// <returns></returns>
        private bool MakeSignOnMsg(SignOnParameters signonParam)
        {
            inMessage.Fields.Clear();

            inMessage.MessageTypeIdentifier = (int)signonParam.MsgType;
            inMessage.Formatter = (signonParam.MsgType == Enums.MsgType.SignOn) ? (IMessageFormatter)new Iso8583Ascii1993MessageFormatter() : new Iso8583Ascii1987MessageFormatter();
            

            inMessage.Fields.Add(11, signonParam.Stan.ToString().PadLeft(6, '0'));
            inMessage.Fields.Add(12, string.Concat(signonParam.TranDate.Date.ToString("yyMMdd").Trim(), signonParam.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim()));
            inMessage.Fields.Add(24, Convert.ToString(821));

            inMessage.Fields.Add(41, signonParam.TerminalAcceptorId.PadLeft(8, '0'));
            inMessage.Fields.Add(42, signonParam.CardAcceptorId.PadLeft(15, '0'));
            //inMessage.Fields.Add(48, string.Format("{0};{1};{2}","1.1.2", signonParam.Stan.ToString().PadLeft(6, '0'), signonParam.TerminalSerialNumberP48));
            inMessage.Fields.Add(48, string.Format("{0};{1};{2}", signonParam.AppVersion, signonParam.LastSuccedStan.ToString().PadLeft(6, '0'), signonParam.TerminalSerialNumberP48));
            inMessage.Fields.Add(63, string.Empty);


            byte[] bte = { 0x1c };
            string str = System.Text.Encoding.ASCII.GetString(bte);
            //inMessage.Fields.Add(48, "000000" + str + "03.00.05" + str);
            inMessage.Fields.Add(64, "0000000000000000");

            TimeSpan t33 = DateTime.Now.TimeOfDay;

            
            KioskLogger.Instance.LogMessage("inMessage before set MAC in SignOn : " + inMessage.ToString());
            bool res = false;
            res = SetMAC(ref inMessage, Utilities.UtilityMethods.GetKeys(KeyType.MACKey, inMessage));
            KioskLogger.Instance.LogMessage("inMessage After set MAC in SignOn : " + inMessage.ToString());

            TimeSpan t3 = DateTime.Now.TimeOfDay;
            TimeSpan SetMACTime = t3 - t33;
            Console.WriteLine("SetMACTime = " + SetMACTime.ToString());

            return res;

        }

        /// <summary>
        /// ساختن پيغام در خواست تراکنش مالی با فرمت 87
        /// </summary>
        /// <typeparam name="T">نوع کلاس تراکنش مالی</typeparam>
        /// <param name="financeParam"></param>
        /// <returns></returns>
        private bool MakeFinancial87Msg<T>(T financeParam) where T : class, new()
        {
            // ...ای جونم ، من اين حس قشنگو به تو مدیونم ، می دونم تا دنيا باشه عاشق تو می مونم

            inMessage.Fields.Clear();
            FinancialParameters Obj = new FinancialParameters();
            if (financeParam is FinancialParameters)
            {
                Obj = financeParam as FinancialParameters;


                inMessage.MessageTypeIdentifier = (int)Obj.MsgType;
                inMessage.Formatter = (Obj.MsgType == Enums.MsgType.Financial) ? (IMessageFormatter)new Iso8583Ascii1987MessageFormatter() : new Iso8583Ascii1993MessageFormatter();
                inMessage.Fields.Add(2, Obj.CardNumberP2);

                switch (this.CsParam.trxType)
                {

                    case 1://موجودی
                        inMessage.Fields.Add(3, Convert.ToString((int)Enums.ProcessCode.Remain));
                        break;
                    case 4://پرداخت قبض
                        inMessage.Fields.Add(3, Convert.ToString((int)Enums.ProcessCode.BillPay));
                        inMessage.Fields.Add(4, Obj.TranAmountP4.ToString().PadLeft(12, '0'));
                        break;
                    case 27: //خرید شارژ
                        inMessage.Fields.Add(3, Convert.ToString((int)Enums.ProcessCode.Charg));
                        inMessage.Fields.Add(4, Obj.TranAmountP4.ToString().PadLeft(12, '0'));
                        break;

                    default:
                        return false;
                }

                inMessage.Fields.Add(11, Convert.ToString(Obj.Stan).PadLeft(6, '0'));
                inMessage.Fields.Add(12, Obj.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim());
                inMessage.Fields.Add(13, Obj.TranDate.Date.ToString("yyyyMMdd").Trim());
                inMessage.Fields.Add(25, Convert.ToString(Obj.DeviceCodeP25));

                inMessage.Fields.Add(32, Obj.BankAcceptorId);
                inMessage.Fields.Add(35, Obj.IsoTrack);
                inMessage.Fields.Add(37, Convert.ToString(Obj.TranRefNumber).PadLeft(12, '0'));
                inMessage.Fields.Add(41, Obj.TerminalAcceptorId.PadLeft(8, '0'));
                inMessage.Fields.Add(42, Obj.CardAcceptorId.PadLeft(15, '0'));
            }

            if (Obj == null)
                return false;

            string str = Utilities.UtilityMethods.FieldSepratorStr;
            string Field48 = Obj.LastSuccedStan.ToString().PadLeft(6, '0') + str + "03.00.05" + str;
            
            if (Obj is PurchaseChargeParameters)
                Field48 += ((PurchaseChargeParameters)Obj).ChargeCountP48.ToString() + str;
            else
            if(Obj is BillPaymentParameters)
                Field48 = Field48 + ((BillPaymentParameters)Obj).BillID.ToString() + str + ((BillPaymentParameters)Obj).PayID.ToString() + str;

            inMessage.Fields.Add(48, Field48);

            inMessage.Fields.Add(49, Convert.ToString(Obj.TranCurrencyCodeP49));
            inMessage.Fields.Add(53, Obj.TerminalSerialNumberP53);
            inMessage.Fields.Add(52, csUtil.BinToHex(Obj.PinBlockP52));
            
            if (_csParam.trxType == 27)//خرید شارژ
            {
                if (Obj is PurchaseChargeParameters)
                    inMessage.Fields.Add(98, ((int)((PurchaseChargeParameters)Obj).Chargetype98).ToString().PadLeft(25, '0'));
            }
            else
                inMessage.Fields.Add(98, "0000000000000000000000000");

            inMessage.Fields.Add(128, this.DefMAC);

            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
            byte[] key = csUtil.HexToBin(keyStr);


            KioskLogger.Instance.LogMessage("inMessage before set MAC : " + inMessage.ToString());
            bool result = SetMAC(ref inMessage, key);
            KioskLogger.Instance.LogMessage("inMessage After set MAC in SignOn : " + inMessage.ToString());

            return result;

        }
    
        /// <summary>
        /// ساختن پیغام درخواست تراکنش برگشت
        /// </summary>
        /// <param name="ReversalParam"></param>
        /// <returns></returns>
        private bool MakeReversalMsg(ReversalParameters ReversalParam)
        {
            inMessage.Fields.Clear();
            inMessage.Formatter = (ReversalParam.MsgType == Enums.MsgType.Reverse) ? (IMessageFormatter)new Iso8583Ascii1987MessageFormatter() : new Iso8583Ascii1993MessageFormatter();
            inMessage.MessageTypeIdentifier = (int)ReversalParam.MsgType;
            inMessage.Fields.Add(2, ReversalParam.CardNumberP2);
            inMessage.Fields.Add(3, Convert.ToString((int)ReversalParam.PrimaryProcessCode));
            inMessage.Fields.Add(4, ReversalParam.PrimaryAmount.ToString().PadLeft(12, '0'));
            inMessage.Fields.Add(11, Convert.ToString(ReversalParam.Stan).PadLeft(6, '0'));
            inMessage.Fields.Add(12, ReversalParam.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim());
            inMessage.Fields.Add(13, ReversalParam.TranDate.Date.ToString("yyyyMMdd").Trim());
            inMessage.Fields.Add(25, Convert.ToString( ReversalParam.DeviceCodeP25));

            inMessage.Fields.Add(32, ReversalParam.BankAcceptorId);
            inMessage.Fields.Add(35, ReversalParam.IsoTrack);
            inMessage.Fields.Add(37, Convert.ToString(ReversalParam.PrimaryTranRefNumber).PadLeft(12, '0'));
            inMessage.Fields.Add(41, ReversalParam.TerminalAcceptorId.PadLeft(8, '0'));
            inMessage.Fields.Add(42, ReversalParam.CardAcceptorId.PadLeft(15, '0'));


            inMessage.Fields.Add(48, ReversalParam.LastSuccedStan.ToString().PadLeft(6, '0') + Utilities.UtilityMethods.FieldSepratorStr + "03.00.05" + Utilities.UtilityMethods.FieldSepratorStr + ReversalParam.PrimaryStan.ToString() + Utilities.UtilityMethods.FieldSepratorStr);
            inMessage.Fields.Add(49, Convert.ToString( ReversalParam.TranCurrencyCodeP49));
            inMessage.Fields.Add(53, ReversalParam.TerminalSerialNumberP53);
            inMessage.Fields.Add(90, (ReversalParam.PrimaryStan.ToString().PadLeft(6,'0')+ReversalParam.PrimaryDateTime).PadRight(42,'0'));
            inMessage.Fields.Add(95, Convert.ToString(ReversalParam.PrimaryAmount));

            inMessage.Fields.Add(98, "0000000000000000000000000");
            inMessage.Fields.Add(128, DefMAC);
            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
            byte[] key = csUtil.HexToBin(keyStr);
            bool result = SetMAC(ref inMessage, key);
            KioskLogger.Instance.LogMessage(inMessage.ToString());
            return result;
        }

        private bool MakeSettleMsg(SettleParameters SettleParam)
        {
            inMessage.Fields.Clear();
            inMessage.Formatter = (SettleParam.MsgType == Enums.MsgType.Settle) ? (IMessageFormatter)new Iso8583Ascii1987MessageFormatter() : new Iso8583Ascii1993MessageFormatter();
            inMessage.MessageTypeIdentifier = (int)SettleParam.MsgType;
            inMessage.Fields.Add(3, Convert.ToString((int)SettleParam.ProcessCode));
            inMessage.Fields.Add(11, Convert.ToString(SettleParam.Stan).PadLeft(6, '0'));
            inMessage.Fields.Add(12, SettleParam.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim());
            inMessage.Fields.Add(13, SettleParam.TranDate.Date.ToString("yyyyMMdd").Trim());
            inMessage.Fields.Add(25, Convert.ToString(SettleParam.DeviceCodeP25));
            inMessage.Fields.Add(32, SettleParam.BankAcceptorId);
            inMessage.Fields.Add(41, SettleParam.TerminalAcceptorId.PadLeft(8, '0'));
            inMessage.Fields.Add(42, SettleParam.CardAcceptorId.PadLeft(15, '0'));
            inMessage.Fields.Add(48, SettleParam.LastSuccedStan.ToString().PadLeft(6, '0') + Utilities.UtilityMethods.FieldSepratorStr + "03.00.05" + Utilities.UtilityMethods.FieldSepratorStr );
            inMessage.Fields.Add(53, SettleParam.TerminalSerialNumber53);
            inMessage.Fields.Add(64, "0000000000000000");

            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
            byte[] key = csUtil.HexToBin(keyStr);
            bool result = SetMAC(ref inMessage, key);
            KioskLogger.Instance.LogMessage(inMessage.ToString());
            return result;
        }

        /// <summary>
        /// ساختن پیغام سرویس های ويژه
        /// </summary>
        /// <param name="ServiceParam"></param>
        /// <returns></returns>
        private bool MakeSpecialService<T>(T ServiceParam) where T : SpecialServiceParameters , new()
        {


            try
            {
                if (ServiceParam == null)
                    return false;

                inMessage.Fields.Clear();
                inMessage.MessageTypeIdentifier = (int)ServiceParam.MsgType;
                inMessage.Formatter = (IMessageFormatter)new Iso8583Ascii1993MessageFormatter();

                inMessage.Fields.Add(2, ServiceParam.CardNumberP2);
                inMessage.Fields.Add(3, Convert.ToString((int)ServiceParam.ProcessCode));

                if (ServiceParam.ProcessCode == Enums.ProcessCode.Charg)
                    inMessage.Fields.Add(6, Convert.ToString(ServiceParam.ChargeParam.ChargeAmount));

                inMessage.Fields.Add(11, ServiceParam.Stan.ToString().PadLeft(6, '0'));
                inMessage.Fields.Add(12, string.Concat(ServiceParam.TranDate.Date.ToString("yyMMdd").Trim(), ServiceParam.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim()));
                inMessage.Fields.Add(14, ServiceParam.ExpireDate);
                inMessage.Fields.Add(22, ServiceParam.ServiceUseCode);
                inMessage.Fields.Add(24, Convert.ToString(((int)ServiceParam.ServiceType)));
                inMessage.Fields.Add(35, ServiceParam.IsoTrack);

                inMessage.Fields.Add(41, ServiceParam.TerminalAcceptorId.PadLeft(8, '0'));
                inMessage.Fields.Add(42, ServiceParam.CardAcceptorId.PadLeft(15, '0'));



                if (ServiceParam.ServiceType != Enums.SpecialServiceType.HamrahAvalMidTermInfo && ServiceParam.ServiceType != Enums.SpecialServiceType.HamrahAvalFinalTermInfo)
                {
                    inMessage.Fields.Add(4, Convert.ToString(ServiceParam.TranAmountP4));

                    if (ServiceParam.ServiceType != Enums.SpecialServiceType.Financial)
                        inMessage.Fields.Add(37, Convert.ToString(ServiceParam.TranRefNumber).PadLeft(12, '0'));

                    inMessage.Fields.Add(52, csUtil.BinToHex(ServiceParam.PinBlockP52));
                }

                string Field48 = string.Empty;

                if (ServiceParam.ServiceType == Enums.SpecialServiceType.Financial)
                {
                    if (ServiceParam.ProcessCode == Enums.ProcessCode.Remain)
                        Field48 = ServiceParam.AppVersion;
                    if (ServiceParam.ProcessCode == Enums.ProcessCode.Charg)
                        Field48 = Convert.ToString(ServiceParam.ChargeParam.ChargeCountP48) + ";" + Convert.ToString((int)ServiceParam.ChargeParam.Chargetype98);
                }
                else
                    if (ServiceParam.ServiceType == Enums.SpecialServiceType.Mobinnet)
                    {
                        Field48 = ServiceParam.MobileNumber + ";" + ServiceParam.ServiceId;
                    }
                    else
                        if (ServiceParam.ServiceType == Enums.SpecialServiceType.HamrahAvalMidTermInfo || ServiceParam.ServiceType == Enums.SpecialServiceType.HamrahAvalFinalTermInfo)
                        {
                            Field48 = ServiceParam.MobileNumber;
                        }
                        else
                            Field48 = ServiceParam.MobileNumber + ";" + Convert.ToString(ServiceParam.TranAmountP4);


                inMessage.Fields.Add(48, Field48);

                inMessage.Fields.Add(63, Convert.ToString(0));


                byte[] bte = { 0x1c };
                string str = System.Text.Encoding.ASCII.GetString(bte);
                inMessage.Fields.Add(64, "0000000000000000");

                
                bool res = SetMAC(ref inMessage, Utilities.UtilityMethods.GetKeys(KeyType.MACKey, inMessage));
                KioskLogger.Instance.LogMessage(inMessage.ToString());
                return res;
            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX, "Exception Came in MakeSpecialService");
                throw EX;
            }
            
        }

        private bool MakeSettleReversal93Msg(SettleReverse93Parameters Param)
        {

            inMessage.Fields.Clear();
            inMessage.Formatter = (IMessageFormatter)new Iso8583Ascii1993MessageFormatter();
            inMessage.MessageTypeIdentifier = (int)Param.MsgType;
            inMessage.Fields.Add(2, Param.CardNumberP2);
            inMessage.Fields.Add(3, Convert.ToString((int)Param.PrimaryProcessCode));
            inMessage.Fields.Add(4, Param.PrimaryAmount.ToString().PadLeft(12, '0'));
            inMessage.Fields.Add(7, string.Concat(Param.TranDate.Date.ToString("yyMMdd").Trim().Substring(2,Param.TranDate.Date.ToString("yyMMdd").Length -2), Param.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim()));
            inMessage.Fields.Add(11, Convert.ToString(Param.PrimaryStan).PadLeft(6, '0'));
            inMessage.Fields.Add(12, Param.PrimaryDateTime.Substring(2,Param.PrimaryDateTime.Length - 2));
            inMessage.Fields.Add(24, Convert.ToString( (int)Param.ServiceType));
            inMessage.Fields.Add(41, Param.TerminalAcceptorId.PadLeft(8, '0'));
            inMessage.Fields.Add(42, Param.CardAcceptorId.PadLeft(15, '0'));
            inMessage.Fields.Add(64, "0000000000000000");
           
            int index = 0;
            string keyStr = CsSecurityKeys.getInstance().getMakKey(index);
            byte[] key = csUtil.HexToBin(keyStr);
            bool result = SetMAC(ref inMessage, key);
            KioskLogger.Instance.LogMessage(inMessage.ToString());
            return result;
        }

        private bool MakeBillpay93Msg(BillPaymentParameters BillParam)
        {
            if (BillParam == null)
                return false;

            inMessage.Fields.Clear();
            inMessage.MessageTypeIdentifier = (int)BillParam.MsgType;
            inMessage.Formatter = (IMessageFormatter)new Iso8583Ascii1993MessageFormatter();

            inMessage.Fields.Add(2, BillParam.CardNumberP2);
            inMessage.Fields.Add(3, Convert.ToString((int)BillParam.ProcessCode));
            inMessage.Fields.Add(4, Convert.ToString(BillParam.TranAmountP4));
            inMessage.Fields.Add(11, BillParam.Stan.ToString().PadLeft(6, '0'));
            inMessage.Fields.Add(12, string.Concat(BillParam.TranDate.Date.ToString("yyMMdd").Trim(), BillParam.TranTime.ToString().Substring(0, 8).Replace(":", "").Trim()));
            inMessage.Fields.Add(14, BillParam.ExpireDate);
            inMessage.Fields.Add(22, BillParam.ServiceUseCode);
            inMessage.Fields.Add(24, Convert.ToString(((int)BillParam.ServiceType)));
            inMessage.Fields.Add(35, BillParam.IsoTrack);
            inMessage.Fields.Add(41, BillParam.TerminalAcceptorId.PadLeft(8, '0'));
            inMessage.Fields.Add(42, BillParam.CardAcceptorId.PadLeft(15, '0'));
            inMessage.Fields.Add(52, csUtil.BinToHex(BillParam.PinBlockP52));

            string Field48 = BillParam.BillID + ";" + BillParam.PayID;
            inMessage.Fields.Add(48, Field48);

            inMessage.Fields.Add(63, Convert.ToString(0));


            byte[] bte = { 0x1c };
            string str = System.Text.Encoding.ASCII.GetString(bte);
            inMessage.Fields.Add(64, "0000000000000000");

            bool res = SetMAC(ref inMessage, Utilities.UtilityMethods.GetKeys(KeyType.MACKey, inMessage));
            KioskLogger.Instance.LogMessage(inMessage.ToString());
            return res;
            
        }

       

        //*** Make Reply Msg

        /// <summary>
        /// ساختن شی از پیغام پاسخ تراکنش مالی با فرمت 87
        /// </summary>
        /// <typeparam name="T">نوع کلاس تراکنش مالی</typeparam>
        /// <param name="ouMsg">يغام پاسخ دریافتی از سوييچ</param>
        /// <returns></returns>
        private T MakeFinancial87ReplyMsg<T>(Message ouMsg) where T : FinancialReplyParameters, new()
        {

            T Obj = new T();

            Obj.MsgType = Enums.MsgType.FinancialReply;
            if (this.outMessage.Fields[3] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[3].Value)))
                Obj.ProcessCode = (Enums.ProcessCode)(Convert.ToInt32(this.outMessage.Fields[3].Value.ToString().Trim()));

            Obj.MsgFormat = Enums.SwitchMsgFormat.Shetab87;
            DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
            Obj.DateTimeP7 = datetime;

            if (this.outMessage.Fields[11] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[11].Value)))
                Obj.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);

            Obj.TranTime = datetime.TimeOfDay;
            Obj.TranDate = datetime.Date;

            if (this.outMessage.Fields[32] != null)
            Obj.BankAcceptorId = Convert.ToString(this.outMessage.Fields[32].Value);

            if (this.outMessage.Fields[35] != null)
                Obj.IsoTrack = Convert.ToString(this.outMessage.Fields[35].Value);

            if (this.outMessage.Fields[37] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[37].Value)))
                Obj.TranRefNumber = Convert.ToInt64(this.outMessage.Fields[37].Value);

            if (this.outMessage.Fields[39] != null)
                Obj.ReplyCodeP39 = Convert.ToString(this.outMessage.Fields[39].Value);

            if (this.outMessage.Fields[41] != null)
                Obj.TerminalAcceptorId = Convert.ToString(this.outMessage.Fields[41].Value);

            if (this.outMessage.Fields[42] != null)
                Obj.CardAcceptorId = Convert.ToString(this.outMessage.Fields[42].Value);

            if (this.outMessage.Fields[48] != null)
                Obj.ExtraData = Convert.ToString(this.outMessage.Fields[48].Value);

            if (this.outMessage.Fields[49] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[49].Value)))
                Obj.TranCurrencyCodeP49 = Convert.ToInt32(this.outMessage.Fields[49].Value);

            if (this.outMessage.Fields[53] != null)
                Obj.TerminalSerialNumber53 = Convert.ToString(this.outMessage.Fields[53].Value);

            if (this.outMessage.Fields[128] != null)
                Obj.MACP128 = Convert.ToString(this.outMessage.Fields[128].Value);



            //اطلاعات موجودی در فیلد 54 با فرمت خاصی است
            if (Obj is BalanceInquiryReplyParameteres)
            {
                (Obj as BalanceInquiryReplyParameteres).CardNumber = this.CsParam.PAN;

                BalanceInquiryReplyParameteres balanceObj = Obj as BalanceInquiryReplyParameteres;
                if (this.outMessage.Fields[54] != null)
                    SetBalanceInfo(this.outMessage.Fields[54].Value.ToString(), ref balanceObj);
            }


            //اطلاعات شارژ در فیلد48 است
            if (Obj is PurchaseChargeReplyParameters)
            {
                if (this.outMessage.Fields[98] != null)
                    (Obj as PurchaseChargeReplyParameters).ChargeTypeP98 = Convert.ToString(this.outMessage.Fields[98].Value);

                //اگر شارژ درست نخريده باشد فيلد48 خالی است
                if (this.outMessage.Fields[39] != null && Convert.ToInt32(this.outMessage.Fields[39].Value.ToString()) == 0)
                {
                    if (!string.IsNullOrEmpty(Obj.ExtraData))
                    {
                        string chargeInfo = Obj.ExtraData;
                        string[] chargeInfoarray = chargeInfo.Split(new string[] { Utilities.UtilityMethods.FieldSepratorStr }, StringSplitOptions.None);

                        if (chargeInfoarray.Count() >= 12)
                            (Obj as PurchaseChargeReplyParameters).ChargeCount = chargeInfoarray[11];
                        if (chargeInfoarray.Count() >= 13)
                            (Obj as PurchaseChargeReplyParameters).ChargeSerial = chargeInfoarray[12];
                        if (chargeInfoarray.Count() >= 15)
                            (Obj as PurchaseChargeReplyParameters).ChargeAmount = chargeInfoarray[14];

                        string chargePass = chargeInfoarray[13];
                        EppService.Instance.SetActiveWKey(0, 1);
                        //string Asci2HexCharge = Utilities.UtilityMethods.Ascii2Hex(chargePass);
                        byte[] binCharge = csUtil.HexToBin(chargePass);
                        byte[] decrypycharge = EppService.Instance.DecrypyData(binCharge);
                        string chargeStr = Utilities.UtilityMethods.BytetoStr(decrypycharge).Replace("\0", "").Trim();


                        (Obj as PurchaseChargeReplyParameters).ChargePassword = chargeStr;
                    }
                }

            }

            if (Obj is ReversalReplyParameters)
            {
                (Obj as ReversalReplyParameters).PrimaryProcessCode = (Enums.ProcessCode)(Convert.ToInt32(this.outMessage.Fields[3].Value.ToString().Trim()));
                (Obj as ReversalReplyParameters).PrimaryAmount = Convert.ToInt32(this.outMessage.Fields[4].Value.ToString().Trim());
                (Obj as ReversalReplyParameters).PrimaryTranRefNumber = Convert.ToInt64(this.outMessage.Fields[37].Value);
                
                BalanceInquiryReplyParameteres balanceObj = (Obj as ReversalReplyParameters).BalanceInfo;
                if (this.outMessage.Fields[54] != null)
                    SetBalanceInfo(this.outMessage.Fields[54].Value.ToString(), ref balanceObj);
            }


            Obj.TranSuccess = true;

            Set87Field48(ref Obj);

            return Obj;
        }

        private SettleReplyParameters MakeSettleReplyMsg(Message message)
        {
            SettleReplyParameters ReplyParam = new SettleReplyParameters();

            if (this.outMessage.Fields[3] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[3].Value)))
                ReplyParam.ProcessCode = (Enums.ProcessCode)(Convert.ToInt32(this.outMessage.Fields[3].Value.ToString().Trim()));
            ReplyParam.MsgFormat = Enums.SwitchMsgFormat.Shetab87;
            DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
            ReplyParam.DateTimeP7 = datetime;

            if (this.outMessage.Fields[11] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[11].Value)))
                ReplyParam.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);

            ReplyParam.TranTime = datetime.TimeOfDay;
            ReplyParam.TranDate = datetime.Date;

            if (this.outMessage.Fields[32] != null)
                ReplyParam.BankAcceptorId = Convert.ToString(this.outMessage.Fields[32].Value);

            if (this.outMessage.Fields[39] != null)
                ReplyParam.ReplyCodeP39 = Convert.ToString(this.outMessage.Fields[39].Value);

            if (this.outMessage.Fields[41] != null)
                ReplyParam.TerminalAcceptorId = Convert.ToString(this.outMessage.Fields[41].Value);

            if (this.outMessage.Fields[42] != null)
                ReplyParam.CardAcceptorId = Convert.ToString(this.outMessage.Fields[42].Value);

            if (this.outMessage.Fields[48] != null)
                ReplyParam.ExtraData = Convert.ToString(this.outMessage.Fields[48].Value);

            if (this.outMessage.Fields[64] != null)
                ReplyParam.MACP64 = Convert.ToString(this.outMessage.Fields[64].Value);

            ReplyParam.TranSuccess = true;
            Set87Field48(ref ReplyParam);

            return ReplyParam;
            
        }

        private T MakeSpecialService93Reply<T>(Message OutMsg) where T : SpecialServiceReplyParameters ,new()
        {
            T Obj = new T();

            Obj.MsgType = Enums.MsgType.SpecialService;
            Obj.MsgFormat = Enums.SwitchMsgFormat.Shetab93;

            
            if (this.outMessage.Fields[3] != null && !string.IsNullOrEmpty(Convert.ToString( this.outMessage.Fields[3].Value)))
                Obj.ProcessCode = (Enums.ProcessCode)(Convert.ToInt32(this.outMessage.Fields[3].Value.ToString().Trim()));

            DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
            Obj.DateTimeP7 = datetime;
            Obj.TranTime = datetime.TimeOfDay;
            Obj.TranDate = datetime.Date;

            if (this.outMessage.Fields[11] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[11].Value)))
                Obj.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);

            if (this.outMessage.Fields[37] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[37].Value)))
                Obj.TranRefNumber = Convert.ToInt64(this.outMessage.Fields[37].Value);

            if (this.outMessage.Fields[39] != null)
                Obj.ReplyCodeP39 = Convert.ToString(this.outMessage.Fields[39].Value);

            if (this.outMessage.Fields[41] != null)
                Obj.TerminalAcceptorId = Convert.ToString( this.outMessage.Fields[41].Value);

            if (this.outMessage.Fields[42] != null)
                Obj.CardAcceptorId = Convert.ToString( this.outMessage.Fields[42].Value);

            if (this.outMessage.Fields[46] != null)
                Obj.Wage = Convert.ToString(this.outMessage.Fields[46].Value);

            if (this.outMessage.Fields[48] != null)
                Obj.ExtraData = Convert.ToString(this.outMessage.Fields[48].Value);


            if (this.outMessage.Fields[62] != null)
            {
                CsFanapEncoder fanapencoder = new CsFanapEncoder();
                Obj.SpecialMsg = fanapencoder.Decode( Convert.ToString(this.outMessage.Fields[62].Value));
            }

            if (this.outMessage.Fields[64] != null)
                Obj.MACP64 = Convert.ToString(this.outMessage.Fields[64].Value);

            Obj.TranSuccess = true;

          

            string Data = Obj.ExtraData;

            if (this.outMessage.Fields[24] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[24].Value)))
            if ((Enums.SpecialServiceType)( Convert.ToInt32(this.outMessage.Fields[24].Value)) == Enums.SpecialServiceType.Financial)
            {
                if (this.outMessage.Fields[2] != null)
                    Obj.CardNumber = Convert.ToString(this.outMessage.Fields[2].Value);
                //دریافت موجودی
                if (Obj.ProcessCode == Enums.ProcessCode.Remain)
                {
                    BalanceInquiryReplyParameteres balanceObj = new BalanceInquiryReplyParameteres();
                    if (this.outMessage.Fields[54] != null && !string.IsNullOrEmpty(Convert.ToString( this.outMessage.Fields[54].Value)))
                    {
                        SetBalanceInfo(Convert.ToString( this.outMessage.Fields[54].Value), ref balanceObj);
                        Obj.LedgerBalance = balanceObj.LedgerBalance;
                        Obj.AvailableBalance = balanceObj.AvailableBalance;
                    }
                }

                //خرید شارژ
                if (Obj.ProcessCode == Enums.ProcessCode.Charg)
                {
                    Obj.ChargeParam = new PurchaseChargeReplyParameters();
                    this.TranType = Enums.Transactions.Charge;

                    if (this.outMessage.Fields[4] != null && !string.IsNullOrEmpty(Convert.ToString( this.outMessage.Fields[4].Value)))
                        Obj.TranAmountP4 = Convert.ToInt64(this.outMessage.Fields[4].Value);
                    if (this.outMessage.Fields[6] != null)
                        Obj.ChargeParam.ChargeAmount = Convert.ToString(this.outMessage.Fields[6].Value);
                }
            }

            //اطلاعات پايه که معمولا برای چاپ استفاده می شود
            BaseReplyParameters BaseObj = (Obj as BaseReplyParameters);
            Set93Field48(Obj.ExtraData, ref BaseObj);

            return Obj;
        }

        private SettleReverse93ReplyParameters MakeSettleReversal93Reply(Message OutMsg)
        {
            SettleReverse93ReplyParameters ReplyParam = new SettleReverse93ReplyParameters();
            ReplyParam.MsgFormat = Enums.SwitchMsgFormat.Shetab93;
            DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
            ReplyParam.DateTimeP7 = datetime;

            if (this.outMessage.Fields[2] != null)
                ReplyParam.CardNumber = Convert.ToString(this.outMessage.Fields[2].Value);

            if (this.outMessage.Fields[11] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[11].Value)))
                ReplyParam.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);

            ReplyParam.TranTime = datetime.TimeOfDay;
            ReplyParam.TranDate = datetime.Date;

            if (this.outMessage.Fields[39] != null)
                ReplyParam.ReplyCodeP39 = Convert.ToString(this.outMessage.Fields[39].Value);

            if (this.outMessage.Fields[41] != null)
                ReplyParam.TerminalAcceptorId = Convert.ToString(this.outMessage.Fields[41].Value);

            if (this.outMessage.Fields[42] != null)
                ReplyParam.CardAcceptorId = Convert.ToString(this.outMessage.Fields[42].Value);

            if (this.outMessage.Fields[64] != null)
                ReplyParam.MACP128 = Convert.ToString(this.outMessage.Fields[64].Value);

            ReplyParam.TranSuccess = true;


            return ReplyParam;
        }

        private BillPaymentReplyParameters MakeBillpay93ReplyMsg(Message message)
        {
            BillPaymentReplyParameters Obj = new BillPaymentReplyParameters();

            Obj.MsgType = Enums.MsgType.SpecialServiceReply;

            if (this.outMessage.Fields[2] != null)
                Obj.CardNumber = Convert.ToString(this.outMessage.Fields[2].Value);

            Obj.MsgFormat = Enums.SwitchMsgFormat.Shetab93;

            if (this.outMessage.Fields[3] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[3].Value)))
                Obj.ProcessCode = (Enums.ProcessCode)(Convert.ToInt32(this.outMessage.Fields[3].Value));

            DateTime datetime = Utilities.UtilityMethods.GetDateTime(this.outMessage);
            Obj.DateTimeP7 = datetime;

            if (this.outMessage.Fields[11] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[11].Value)))
            Obj.Stan = Convert.ToInt32(this.outMessage.Fields[11].Value);

            Obj.TranTime = datetime.TimeOfDay;
            Obj.TranDate = datetime.Date;

            if (this.outMessage.Fields[37] != null && !string.IsNullOrEmpty(Convert.ToString(this.outMessage.Fields[37].Value)))
                Obj.TranRefNumber = Convert.ToInt64(this.outMessage.Fields[37].Value);

            if (this.outMessage.Fields[39] != null)
                Obj.ReplyCodeP39 = Convert.ToString(this.outMessage.Fields[39].Value);

            if (this.outMessage.Fields[41] != null)
                Obj.TerminalAcceptorId = Convert.ToString(this.outMessage.Fields[41].Value);

            if (this.outMessage.Fields[42] != null)
                Obj.CardAcceptorId = Convert.ToString(this.outMessage.Fields[42].Value);

            if (this.outMessage.Fields[46] != null)
                Obj.Wage = Convert.ToString(this.outMessage.Fields[46].Value);

            if (this.outMessage.Fields[48] != null)
                Obj.ExtraData = Convert.ToString(this.outMessage.Fields[48].Value);

            if (this.outMessage.Fields[62] != null)
            {
                CsFanapEncoder fanapencoder = new CsFanapEncoder();
                Obj.SpecialMsg = fanapencoder.Decode(Convert.ToString(this.outMessage.Fields[62].Value));
            }

            if (this.outMessage.Fields[64] != null)
                Obj.MACP128 = Convert.ToString(this.outMessage.Fields[64].Value);

            return Obj;
        }

        //***

        /// <summary>
        ///که به فرمت 93 است SionOn استخراج اطلاعات از فیلد 48 در 
        /// </summary>
        /// <param name="Data"></param>
        private void Set93Field48(string Data , ref BaseReplyParameters replyMsg)
        {
            //اطلاعات اضافی که در فیلد 48 به همراه پاسخ می آيد
            try
            {
                if (Data != null)
                {
                    //if (TranType != Enums.Transactions.Signon)
                    //    return;

                    CsFanapEncoder fanapencoder = new CsFanapEncoder();
                    string[] ExtraData = Data.Split(new string[] { ";" }, StringSplitOptions.None);
                    int ExtraDataCount = ExtraData.Count();
                    if (ExtraDataCount > 1)
                    {
                        if (TranType == Enums.Transactions.Charge)
                        {
                            string str = string.Empty;

                            if (ExtraDataCount >= 1)
                                replyMsg.ExtraData = fanapencoder.Decode(ExtraData[0]);
                            if(ExtraDataCount >= 2)
                                (replyMsg as SpecialServiceReplyParameters).ChargeParam.ChargeCount = ExtraData[1];
                            if (ExtraDataCount >= 4)
                                (replyMsg as SpecialServiceReplyParameters).ChargeParam.ChargePassword = ExtraData[3];
                            if (ExtraDataCount >= 5)
                                (replyMsg as SpecialServiceReplyParameters).ChargeParam.ChargeSerial = ExtraData[4];
                        }
                        else
                        {
                            //اطلاعات پايه فقط در لاگین پر شود
                            if (TranType == Enums.Transactions.Signon)
                            {
                                if (ExtraDataCount >= 1)
                                    SignOnReplyParameters.PosSN = ExtraData[0];
                                if (ExtraDataCount >= 2)
                                    SignOnReplyParameters.VahedTejariSN = ExtraData[1];
                                if (ExtraDataCount >= 3)
                                    SignOnReplyParameters.VahedeTejariDS = fanapencoder.Decode(ExtraData[2]);
                                if (ExtraDataCount >= 4)
                                    SignOnReplyParameters.SwitchSigonOnDateTime = ExtraData[3];
                                if (ExtraDataCount >= 5)
                                    SignOnReplyParameters.VahedeTejariTel = ExtraData[4];
                                if (ExtraDataCount >= 6)
                                    SignOnReplyParameters.Tran_TelNo = ExtraData[5];
                                if (ExtraDataCount >= 7)
                                    SignOnReplyParameters.Tran_TelNo1 = ExtraData[6];
                                if (ExtraDataCount >= 8)
                                    SignOnReplyParameters.TMS_TelNo = ExtraData[7];
                                if (ExtraDataCount >= 9)
                                    SignOnReplyParameters.TMS_TelNo1 = ExtraData[8];
                                if (ExtraDataCount >= 10)
                                    SignOnReplyParameters.Tran_IP = ExtraData[9];
                                if (ExtraDataCount >= 11)
                                    SignOnReplyParameters.TMS_IP = ExtraData[10];
                                if (ExtraDataCount >= 12)
                                    SignOnReplyParameters.ApplicationVer = ExtraData[11];
                                if (ExtraDataCount >= 13)
                                    SignOnReplyParameters.Account = ExtraData[12];
                                if (ExtraDataCount >= 14)
                                    SignOnReplyParameters.AppID = ExtraData[13];
                                if (ExtraDataCount >= 15)
                                    SignOnReplyParameters.MACKey = ExtraData[14];
                                if (ExtraDataCount >= 16)
                                    SignOnReplyParameters.PinKey = ExtraData[15];
                                if (ExtraDataCount >= 17)
                                    SignOnReplyParameters.LOYALTYCARDTRACK2 = ExtraData[16];
                            }
                        }

                    }
                    else
                    {
                        if (replyMsg != null)
                            replyMsg.ExtraData = fanapencoder.Decode(ExtraData[0]);
                    }
                    


                }
            }
            catch (Exception EX)
            {
                return;
                //throw new CustomException("Could Not Set Initial Parameters" , EX.Message);
            }
           
        }

        /// <summary>
        ///  استخراج اطلاعات فیلد48 پيغام های پاسخ با فرمت 87
        /// </summary>
        /// <param name="Data"></param>
        private void Set87Field48<T>(ref T ObjReply) where T :  BaseReplyParameters,new()
        {

            string Data = (ObjReply as T).ExtraData;
            CsFanapEncoder FanapDecoder = new CsFanapEncoder();
            string[] DataArray = Data.Split(new string[] { Utilities.UtilityMethods.FieldSepratorStr }, StringSplitOptions.None);

            (ObjReply as T).DayMessage48 = FanapDecoder.Decode(DataArray[0]);
            (ObjReply as T).BuyerMsg48 = FanapDecoder.Decode(DataArray[1]);
            (ObjReply as T).AcceptorMsg48 = FanapDecoder.Decode(DataArray[2]);
            (ObjReply as T).BankNam48 = FanapDecoder.Decode(DataArray[3]);
            (ObjReply as T).CardType48 = FanapDecoder.Decode(DataArray[4]);
            (ObjReply as T).AcceptorName48 = FanapDecoder.Decode(DataArray[5]);
            (ObjReply as T).AcceptorAddress48 = FanapDecoder.Decode(DataArray[6]);
            (ObjReply as T).AcceptorTelNumber48 = FanapDecoder.Decode(DataArray[7]);
            (ObjReply as T).AcceptorWebsite48 = FanapDecoder.Decode(DataArray[8]);
            (ObjReply as T).BankWebSite48 = FanapDecoder.Decode(DataArray[9]);
            (ObjReply as T).TerminalMsg48 = FanapDecoder.Decode(DataArray[10]);

            if (ObjReply is ReversalReplyParameters)
            {   //در تراکنش برگشت ، تاريخ تراکنش مورد برگشت در اینجا وجود دارد می شود
                (ObjReply as ReversalReplyParameters).PrimaryDateTime = FanapDecoder.Decode(DataArray[11]);
            }

            if (ObjReply is BillPaymentReplyParameters)
            {
                //در پرداخت قبض در فیلد 48 پرداخت نام سازمان صادر کننده قبض است
                (ObjReply as BillPaymentReplyParameters).BillOrganization = Convert.ToString(DataArray[11]);
            }
        }

        private void SetBalanceInfo(string strField54, ref BalanceInquiryReplyParameteres ObjBalance)
        {
            if (ObjBalance == null)
                return;
            if (string.IsNullOrEmpty(strField54))
                return;

            
            ObjBalance.Accouttype = (Enums.AccountType)(Convert.ToUInt32(strField54.Substring(0, 2)));

            //فیلد 2و3 نوع وجه
            Enums.AmountType Amounttype = (Enums.AmountType)(Convert.ToUInt32(strField54.Substring(2, 2)));

            //موجودی اول
            if (Amounttype == Enums.AmountType.Available)
                ObjBalance.AvailableBalance = Convert.ToInt64(strField54.Substring(8, 12));
            else
                ObjBalance.LedgerBalance = Convert.ToInt64(strField54.Substring(8, 12));

            //فیلد 4 تا 6 نوع ارز
            ObjBalance.CurrencyType = Convert.ToInt32(strField54.Substring(4, 3));

            //فیلد 7 ماهیت حساب
            if (Convert.ToString(strField54.Substring(7, 1)) == "C")
                ObjBalance.Accountnature = Enums.AccountNature.Credit;
            else
                ObjBalance.Accountnature = Enums.AccountNature.Debit;

            //موجودی دوم
            Enums.AmountType Amounttype2 = (Enums.AmountType)(Convert.ToUInt32(strField54.Substring(22, 2)));
            if (Amounttype2 == Enums.AmountType.Available)
               ObjBalance.AvailableBalance = Convert.ToInt64(strField54.Substring(28, 12));
            else
                ObjBalance.LedgerBalance = Convert.ToInt64(strField54.Substring(28, 12));
        }

        #endregion PrivateMethods
    }
}
