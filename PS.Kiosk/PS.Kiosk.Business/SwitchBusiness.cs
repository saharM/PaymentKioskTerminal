using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data;
using PS.Kiosk.Messaging.MsgCall;
using PS.Kiosk.Framework;
using PS.Kiosk.DeviceController.Services;
using PS.Kiosk.Data.DataAccessObjects;
using PS.Kiosk.Framework.ExceptionManagement;
using System.Threading;
using PS.Kiosk.Messaging.Operations;
using System.Reflection;
using System.Net.NetworkInformation;

namespace PS.Kiosk.Business
{
    //برقص ، نترس ، بمون ، بخون
    public class SwitchBusiness
    {
        #region GetParameters

        private static BaseParameters GetBaseParameters(ParametersFactory factory)
        {
            BaseParameters ParamObject = factory.GetParameters();

            ParamObject.AppVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

            ParamObject.MsgFormat = Enums.SwitchMsgFormat.Shetab87;
            ParamObject.TranDate = DateTime.Now;
            ParamObject.TranTime = DateTime.Now.TimeOfDay;
            ParamObject.TranDate = DateTime.Now;
            //فیلد 11 و37 تمامی پيغام های درخواست می تواند یکی باشد
            ParamObject.Stan = ParametersDataAccess.Instance.NewStan;
            ParamObject.TranRefNumber = ParametersDataAccess.Instance.NewTranRefNumber;
            ParamObject.LastSuccedStan = ParametersDataAccess.Instance.LastSuceedStan;
            ParamObject.BankAcceptorId = ParametersDataAccess.Instance.BankAcceptorId;
            ParamObject.IP = ParametersDataAccess.Instance.IP;
            ParamObject.Port = ParametersDataAccess.Instance.Port;
            ParamObject.TerminalAcceptorId = ParametersDataAccess.Instance.TerminalAcceptorId;
            ParamObject.TerminalAcceptorName = ParametersDataAccess.Instance.TerminalAcceptorName;
            ParamObject.CardAcceptorId = ParametersDataAccess.Instance.CardAcceptorId;
            ParamObject.CardAcceptorBinCode = ParametersDataAccess.Instance.CardAcceptorBinCode;
            ParamObject.TimeOut = ParametersDataAccess.Instance.TimeOut;
            return ParamObject;
        }

        private static SignOnParameters GetSignOnParameters()
        {
            GenericFactory<SignOnParameters> factory = new GenericFactory<SignOnParameters>();
            SignOnParameters signOnParameters = (SignOnParameters)GetBaseParameters(factory);

            signOnParameters.MsgType = Enums.MsgType.SignOn;
            signOnParameters.ProcessCode = Enums.ProcessCode.SignOn;
            signOnParameters.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            signOnParameters.TerminalSerialNumberP48 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            return signOnParameters;
        }

        private static BalanceInquiryParameteres GetBalanceInquiryParameteres()
        {
            GenericFactory<BalanceInquiryParameteres> factory = new GenericFactory<BalanceInquiryParameteres>();
            BalanceInquiryParameteres balanceInquiryParameteres = (BalanceInquiryParameteres)GetBaseParameters(factory);

            balanceInquiryParameteres.MsgType = Enums.MsgType.Financial;
            balanceInquiryParameteres.ProcessCode = Enums.ProcessCode.Remain;
            balanceInquiryParameteres.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            balanceInquiryParameteres.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            balanceInquiryParameteres.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            balanceInquiryParameteres.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;
            balanceInquiryParameteres.CardNumberP2 = CardReaderService.Instance.CardNumber;
            balanceInquiryParameteres.IsoTrack = CardReaderService.Instance.IsoTrack;
            return balanceInquiryParameteres;
        }

        private static PurchaseChargeParameters GetPurchaseChargeParameters()
        {
            GenericFactory<PurchaseChargeParameters> factory = new GenericFactory<PurchaseChargeParameters>();
            PurchaseChargeParameters purchaseChargeParameteres = (PurchaseChargeParameters)GetBaseParameters(factory);

            purchaseChargeParameteres.MsgType = Enums.MsgType.Financial;
            purchaseChargeParameteres.ProcessCode = Enums.ProcessCode.Charg;
            purchaseChargeParameteres.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            purchaseChargeParameteres.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            purchaseChargeParameteres.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            purchaseChargeParameteres.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;
            purchaseChargeParameteres.CardNumberP2 = CardReaderService.Instance.CardNumber;
            purchaseChargeParameteres.IsoTrack = CardReaderService.Instance.IsoTrack;

            return purchaseChargeParameteres;
        }

        private static ReversalParameters GetReversalParameters(FinancialParameters PrimaryTran)
        {
            GenericFactory<ReversalParameters> factory = new GenericFactory<ReversalParameters>();
            ReversalParameters reversalParameters = (ReversalParameters)GetBaseParameters(factory);

            reversalParameters.MsgType = Enums.MsgType.Reverse;
            reversalParameters.ProcessCode = PrimaryTran.ProcessCode;
            reversalParameters.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            reversalParameters.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            reversalParameters.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            reversalParameters.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;

            if (!string.IsNullOrEmpty(PrimaryTran.CardNumberP2))
                reversalParameters.CardNumberP2 = PrimaryTran.CardNumberP2;
            else
                reversalParameters.CardNumberP2 = CardReaderService.Instance.CardNumber;

            if (!string.IsNullOrEmpty(PrimaryTran.IsoTrack))
                reversalParameters.IsoTrack = PrimaryTran.IsoTrack;
            else
                reversalParameters.IsoTrack = CardReaderService.Instance.IsoTrack;

            if (PrimaryTran.PinBlockP52 != null)
                reversalParameters.PinBlockP52 = PrimaryTran.PinBlockP52;
            else
                reversalParameters.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;

            reversalParameters.PrimaryAmount = PrimaryTran.TranAmountP4;
            reversalParameters.PrimaryNewAmount = PrimaryTran.TranAmountP4;
            reversalParameters.PrimaryDateTime = PrimaryTran.TranDate.Date.ToString("yyyyMMdd").Trim() + PrimaryTran.TranDate.TimeOfDay.ToString().Substring(0, 8).Replace(":", "").Trim();
            reversalParameters.PrimaryProcessCode = PrimaryTran.ProcessCode;
            reversalParameters.PrimaryStan = PrimaryTran.Stan;
            reversalParameters.PrimaryTranRefNumber = PrimaryTran.TranRefNumber;

            return reversalParameters;
        }

        private static SettleParameters GetSettleParam()
        {
            GenericFactory<SettleParameters> factoty = new GenericFactory<SettleParameters>();
            SettleParameters SettleParameters = (SettleParameters)GetBaseParameters(factoty);

            SettleParameters.MsgType = Enums.MsgType.Settle;
            SettleParameters.ProcessCode = Enums.ProcessCode.Settle;
            SettleParameters.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            SettleParameters.TerminalSerialNumber53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            

            return SettleParameters;
        }

        private static BillPaymentParameters GetBillPaymentParam(Enums.SwitchMsgFormat MsgFormat)
        {
            GenericFactory<BillPaymentParameters> factory = new GenericFactory<BillPaymentParameters>();
            BillPaymentParameters BillPaymentParam = (BillPaymentParameters)GetBaseParameters(factory);

            if (MsgFormat == Enums.SwitchMsgFormat.Shetab87)
            {
                BillPaymentParam.MsgType = Enums.MsgType.Financial;
                BillPaymentParam.ProcessCode = Enums.ProcessCode.BillPay;
            }
            else
            {
                BillPaymentParam.MsgType = Enums.MsgType.SpecialService;
                BillPaymentParam.ProcessCode = Enums.ProcessCode.BillPay93;
            }

            BillPaymentParam.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            BillPaymentParam.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            BillPaymentParam.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            BillPaymentParam.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;
            BillPaymentParam.CardNumberP2 = CardReaderService.Instance.CardNumber;
            BillPaymentParam.IsoTrack = CardReaderService.Instance.IsoTrack;
            BillPaymentParam.ExpireDate = BillPaymentParam.IsoTrack.Substring(BillPaymentParam.IsoTrack.IndexOf("=") + 1, 4);

            return BillPaymentParam;
 
        }

        private static SpecialServiceParameters GetSpecialServiceParameters()
        {
            GenericFactory<SpecialServiceParameters> factory = new GenericFactory<SpecialServiceParameters>();
            SpecialServiceParameters ServiceParam = (SpecialServiceParameters)GetBaseParameters(factory);

            ServiceParam.MsgFormat = Enums.SwitchMsgFormat.Shetab93;
            ServiceParam.MsgType = Enums.MsgType.SpecialService;
            ServiceParam.ProcessCode = Enums.ProcessCode.SpecialService;
            ServiceParam.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            ServiceParam.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            ServiceParam.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            ServiceParam.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;
            ServiceParam.CardNumberP2 = CardReaderService.Instance.CardNumber;
            ServiceParam.IsoTrack = CardReaderService.Instance.IsoTrack;
            ServiceParam.ExpireDate = ServiceParam.IsoTrack.Substring(ServiceParam.IsoTrack.IndexOf("=") + 1, 4);
            
            ServiceParam.ServiceUseCode = "210101213144";
            return ServiceParam;
        }

        private static SettleReverse93Parameters GetSettleReverseParameters(FinancialParameters PrimaryTran, Enums.SpecialServiceType serviceType)
        {


            GenericFactory<SettleReverse93Parameters> factory = new GenericFactory<SettleReverse93Parameters>();
            SettleReverse93Parameters reversalParameters = (SettleReverse93Parameters)GetBaseParameters(factory);

            if (serviceType == Enums.SpecialServiceType.Reversal)
                reversalParameters.MsgType = Enums.MsgType.Reverse93;
            else
                reversalParameters.MsgType = Enums.MsgType.Settle93;

            reversalParameters.ProcessCode = PrimaryTran.ProcessCode;
            reversalParameters.DeviceCodeP25 = ParametersDataAccess.Instance.DeviceCodeP25;
            reversalParameters.TerminalSerialNumberP53 = ParametersDataAccess.Instance.TerminalSerialNumberP53;
            reversalParameters.TranCurrencyCodeP49 = ParametersDataAccess.Instance.CurrencyCode;
            reversalParameters.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;

            if (!string.IsNullOrEmpty(PrimaryTran.CardNumberP2))
                reversalParameters.CardNumberP2 = PrimaryTran.CardNumberP2;
            else
                reversalParameters.CardNumberP2 = CardReaderService.Instance.CardNumber;

            if (!string.IsNullOrEmpty(PrimaryTran.IsoTrack))
                reversalParameters.IsoTrack = PrimaryTran.IsoTrack;
            else
                reversalParameters.IsoTrack = CardReaderService.Instance.IsoTrack;

            if (PrimaryTran.PinBlockP52 != null)
                reversalParameters.PinBlockP52 = PrimaryTran.PinBlockP52;
            else
                reversalParameters.PinBlockP52 = ParametersDataAccess.Instance.PinBlock;

            reversalParameters.PrimaryAmount = PrimaryTran.TranAmountP4;
            reversalParameters.PrimaryNewAmount = PrimaryTran.TranAmountP4;
            reversalParameters.PrimaryDateTime = PrimaryTran.TranDate.Date.ToString("yyyyMMdd").Trim() + PrimaryTran.TranDate.TimeOfDay.ToString().Substring(0, 8).Replace(":", "").Trim();
            reversalParameters.PrimaryProcessCode = PrimaryTran.ProcessCode;
            reversalParameters.PrimaryStan = PrimaryTran.Stan;
            reversalParameters.PrimaryTranRefNumber = PrimaryTran.TranRefNumber;

            reversalParameters.ServiceType = serviceType;
            return reversalParameters;

        }

        
        #endregion GetParameters

        #region Transactions

        public static bool SignOnToSwitch(KioskBusiness thisKiosk)
        {
            try
            {
                byte[] MacKey; byte[] PinKey;
                CsUtil csutil = new CsUtil();
                csutil.ExtractPinMacKeys(out MacKey, out PinKey, ParametersDataAccess.Instance.TerminalSerialNumberP53);
                //byte[] key = new byte[8] {0xf2, 0x63, 0xb2, 0x72, 0x83, 0xc2, 0x70, 0xb4 };
                EppService.Instance.SetKeysForLogin(MacKey);

                SignOnParameters signonParams = GetSignOnParameters();
                //ثبت تراکنش درخواست
                Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(signonParams);

                SignOnReplyParameters signOnReplyParameters = Transactions.TransactionsInstance.SignOn(signonParams);

                //ثبت تراکنش پاسخ
                TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(signOnReplyParameters, TranReqId);


                SwitchValidation.IsValid39(Convert.ToInt32(signOnReplyParameters.ReplyCodeP39));

                string[] ParamKeys = new string[] { "CardAcceptorId", "TerminalAcceptorId", "ApplicationRestartHour" };
                string strRestartTime = SignOnReplyParameters.SwitchSigonOnDateTime.Substring(0, 8);
                string[] ParamValues = new string[] { signOnReplyParameters.CardAcceptorId, signOnReplyParameters.TerminalAcceptorId, strRestartTime };
                Parameters.Instance.Update(ParamKeys, ParamValues);

                //Parameters.Instance.Update(signOnReplyParameters.TerminalAcceptorId, signOnReplyParameters.CardAcceptorId);
               
                return signOnReplyParameters.IsAuthenticated;
            }
            catch (CustomException custEx)
            {
                StateManager.Instance.Current.Error(custEx);

            }
            catch (Exception ex)
            {
                StateManager.Instance.Current.Error(ex.Message);
            }
            return false;
        }

        public static decimal GetBalanceInquiry87()
        {
            
            BalanceInquiryParameteres balanceReq;
            BalanceInquiryReplyParameteres balanceRep;
            try
            {
                TimeSpan t1 = DateTime.Now.TimeOfDay;

                balanceReq = GetBalanceInquiryParameteres();

                //ثبت تراکنش درخواست
                Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(balanceReq);

                TimeSpan t2 = DateTime.Now.TimeOfDay;
                TimeSpan MakeParamreq = t2 - t1;
                Console.WriteLine( "\n" + "MakeBalanceParamSwitch = " + MakeParamreq.ToString() + "\n");

                balanceRep = Transactions.TransactionsInstance.GetBalanceInquery(balanceReq);
                

                if (balanceRep != null)
                {
                    //ثبت تراکنش پاسخ
                    TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(balanceRep, TranReqId);

                    SwitchValidation.IsValidTran(balanceReq, balanceRep);

                    PrintParameters.PrintData = balanceRep;
                    //تاريخ روی رسيد باید تاریخ درخواست تراکنش باشد
                    PrintParameters.PrintDate = PS.Kiosk.Messaging.Utilities.UtilityMethods.ChangeToShamsi(balanceReq.TranDate);
                    //شماره پيگيری هم باید از پيغام درخواست باشد
                    (PrintParameters.PrintData as BalanceInquiryReplyParameteres).Stan = balanceReq.Stan;

                }
                else
                    throw new CustomException("Invalid Reply Object");

               return balanceRep.LedgerBalance;
                
            }
            catch (CustomException custEx)
            {
                StateManager.Instance.Current.Error(custEx);
            }
            catch (Exception ex)
            {
                StateManager.Instance.Current.Error(ex.Message);
            }
            return -1;
        }

        public static PurchaseChargeReplyParameters PurchaceCharge87(Enums.ChargeType type , Int64 Amount)
        {
            PurchaseChargeReplyParameters PurchaceRep = null;
            PurchaseChargeParameters PurchaceReq = null;
            try
            {

                PurchaceReq = GetPurchaseChargeParameters();


                PurchaceReq.Chargetype98 = type;


                PurchaceReq.TranAmountP4 = Amount;

                //شارژ گروهی فعلا نداریم
                PurchaceReq.ChargeCountP48 = 1;

                //ثبت تراکنش درخواست
                Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(PurchaceReq);


                PurchaceRep = Transactions.TransactionsInstance.PurchaseCharge(PurchaceReq);

                if (PurchaceRep != null)
                {
                    //ثبت تراکنش پاسخ
                    TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(PurchaceRep, TranReqId);

                    PurchaceRep.MsgFormat = PurchaceReq.MsgFormat;
                    SwitchValidation.IsValidTran(PurchaceReq, PurchaceRep);



                    #region SetForPrint

                    PurchaceRep.CardNumber = PurchaceReq.CardNumberP2;
                    PrintParameters.PrintData = PurchaceRep;
                    //تاريخ روی رسيد باید تاریخ درخواست تراکنش باشد
                    PrintParameters.PrintDate = PS.Kiosk.Messaging.Utilities.UtilityMethods.ChangeToShamsi(PurchaceReq.TranDate);
                    //شماره پيگيری هم باید از پيغام درخواست باشد
                    PrintParameters.PrintData.Stan = PurchaceReq.Stan;

                    #endregion SetForPrint

                    #region SetForReversal/Settle

                    //لازم است ذخیره شود که در صورت بروز خطا در وضعيت های بعدی همين کاربر بتوانیم تراکنش برگشت را بسازیم
                    ReversalParameters.ReversalObject = PurchaceReq;

                    //لازم است برای پرداخت ذخيره شود
                    SettleReplyParameters.LastSettleObject = PurchaceRep;

                    #endregion SetForReversal/Settle

                }
                else
                    throw new CustomException("Invalid Reply Object");

                return PurchaceRep;

            }
            catch (CustomException custEx)
            {
                if (PurchaceReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (PurchaceRep != null && PurchaceRep.ReplyCodeP39 != null && Convert.ToInt32(PurchaceRep.ReplyCodeP39) == 0)
                    {
                        Reversal(PurchaceReq);//بسته به نوع خطا
                        //return PurchaceRep; //برای چاپ شارژ
                    }
                    else
                    {
                        //Time Out => Recersal
                        if (PurchaceRep != null && PurchaceRep.ReplyCodeP39 != null && Convert.ToInt32(PurchaceRep.ReplyCodeP39) == 777)
                            Reversal(PurchaceReq);
                    }

                }

                StateManager.Instance.Current.Error(custEx);
            }
            catch (Exception ex)
            {
                if (PurchaceReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (PurchaceRep != null && PurchaceRep.ReplyCodeP39 != null && Convert.ToInt32(PurchaceRep.ReplyCodeP39) == 0)
                    {
                        Reversal(PurchaceReq);//بسته به نوع خطا
                        //return PurchaceRep; //برای چاپ شارژ
                    }
                    else
                    {
                        //Time Out => Recersal
                        if (PurchaceRep != null && PurchaceRep.ReplyCodeP39 != null && Convert.ToInt32(PurchaceRep.ReplyCodeP39) == 777)
                            Reversal(PurchaceReq);
                    }
                }

                StateManager.Instance.Current.Error(ex.Message);
            }


            return null;
        }

        public static BillPaymentReplyParameters BillPay87(string BillId, string PayId, Int64 Amount)
        {
            BillPaymentParameters BillpayReq = null;
            BillPaymentReplyParameters BillpayRep = null;
            try
            {
                BillpayReq = GetBillPaymentParam(Enums.SwitchMsgFormat.Shetab87);
                BillpayReq.TranAmountP4 = Amount;
                BillpayReq.BillID = BillId;
                BillpayReq.PayID = PayId;

                //ثبت درخواست
                Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(BillpayReq);

                BillpayRep = Transactions.TransactionsInstance.BillPay(BillpayReq);

                
                if (BillpayRep != null)
                {
                    //ثبت پاسخ
                    TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(BillpayRep, TranReqId);
                    BillpayRep.MsgFormat = BillpayReq.MsgFormat;
                    SwitchValidation.IsValidTran(BillpayReq, BillpayRep);
                    #region SetForPrint

                    BillpayRep.TranAmountP4 = BillpayReq.TranAmountP4;
                    BillpayRep.BillId = BillpayReq.BillID;
                    BillpayRep.PayId = BillpayReq.PayID;
                    BillpayRep.CardNumber = BillpayReq.CardNumberP2;
                    PrintParameters.PrintData = BillpayRep;

                    //تاريخ روی رسيد باید تاریخ درخواست تراکنش باشد
                    PrintParameters.PrintDate = PS.Kiosk.Messaging.Utilities.UtilityMethods.ChangeToShamsi(BillpayReq.TranDate);
                    //شماره پيگيری هم باید از پيغام درخواست باشد
                    PrintParameters.PrintData.Stan = BillpayReq.Stan;

                    #endregion SetForPrint

                    #region SetForReversal/Settle

                    //لازم است ذخیره شود که در صورت بروز خطا در وضعيت های بعدی همين کاربر بتوانیم تراکنش برگشت را بسازیم
                    ReversalParameters.ReversalObject = BillpayReq;

                    //لازم است برای پرداخت ذخيره شود
                    SettleReplyParameters.LastSettleObject = BillpayRep;

                    #endregion SetForReversal/Settle
                }
                else
                    throw new CustomException("");

                return BillpayRep;
            }
            catch (CustomException custEx)
            {
                if (BillpayReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 0)
                    {
                        Reversal(BillpayReq);//بسته به نوع خطا

                    }
                    else
                    {
                        //Time Out => Recersal
                        if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 777)
                            Reversal(BillpayReq);
                    }

                }

                StateManager.Instance.Current.Error(custEx);
            }

            catch (Exception Ex)
            {
                if (BillpayReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 0)
                    {
                        Reversal(BillpayReq);//بسته به نوع خطا

                    }
                    else
                    {
                        //Time Out => Recersal
                        if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 777)
                            Reversal(BillpayReq);
                    }

                }

                StateManager.Instance.Current.Error(Ex.Message);
            }

            return null;
        }

        public static SpecialServiceReplyParameters SpecialServicePay(string MobileNumber, Int64 Amount, Enums.SpecialServiceType ServiceType, string ServiceId = "",params object[] OtherParam )
        {
            SpecialServiceParameters ServiceReq = null;
            SpecialServiceReplyParameters ServiceRep = null;
            Int64 TranId = 0;

            try
            {
                ServiceReq = GetSpecialServiceParameters();
                ServiceReq.MobileNumber = MobileNumber;
                ServiceReq.ServiceType = ServiceType;

                if (ServiceReq.ServiceType == Enums.SpecialServiceType.HamrahAvalFinalTermInfo ||
                    ServiceReq.ServiceType == Enums.SpecialServiceType.HamrahAvalMidTermInfo)
                {
                    ServiceReq.ProcessCode = Enums.ProcessCode.SpecialServiceBillPay;
                    ServiceReq.MsgType = Enums.MsgType.BillInfo;
                }
                else
                    ServiceReq.TranAmountP4 = Amount;

                if (ServiceReq.ServiceType == Enums.SpecialServiceType.Financial)
                {
                    //خرید شارژ
                    if (OtherParam.Count() > 0 &&
                        (Convert.ToInt32(OtherParam[0]) == (int)Enums.ChargeType.Irancell ||
                        Convert.ToInt32(OtherParam[0]) == (int)Enums.ChargeType.HamrahAval ||
                        Convert.ToInt32(OtherParam[0]) == (int)Enums.ChargeType.Rightel ||
                        Convert.ToInt32(OtherParam[0]) == (int)Enums.ChargeType.Talia))
                    {
                        ServiceReq.ProcessCode = Enums.ProcessCode.Charg;
                        ServiceReq.ChargeParam = new PurchaseChargeParameters();
                        ServiceReq.ChargeParam.ChargeAmount = Amount;
                        ServiceReq.ChargeParam.Chargetype98 = (Enums.ChargeType)OtherParam[0];
                        ServiceReq.ChargeParam.ChargeCountP48 = 1;
                    }
                    else
                        ServiceReq.ProcessCode = Enums.ProcessCode.Remain;
                }

                if (ServiceType == Enums.SpecialServiceType.Mobinnet)
                    ServiceReq.ServiceId = ServiceId;

                TranId = TransactionsDataAccess.Instance.InsertNewTransaction(ServiceReq);

                
                ServiceRep = Transactions.TransactionsInstance.SpecialServicePay(ServiceReq);

                if (ServiceRep != null)
                {
                    TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(ServiceRep, TranId);
                    SwitchValidation.IsValidTran(ServiceReq, ServiceRep);

                    #region SetForPrint

                    ServiceRep.ServiceType = ServiceReq.ServiceType;
                    ServiceRep.BankNam48 = ServiceRep.ExtraData;
                    ServiceRep.CardNumber = ServiceReq.CardNumberP2;
                    //تاريخ روی رسيد باید تاریخ درخواست تراکنش باشد
                    PrintParameters.PrintDate = PS.Kiosk.Messaging.Utilities.UtilityMethods.ChangeToShamsi(ServiceReq.TranDate);

                   
                    if (ServiceReq.ServiceType != Enums.SpecialServiceType.Financial)
                    {
                        if (ServiceReq.ServiceType != Enums.SpecialServiceType.HamrahAvalFinalTermInfo && ServiceReq.ServiceType != Enums.SpecialServiceType.HamrahAvalMidTermInfo)
                            ServiceRep.TranAmountP4 = ServiceReq.TranAmountP4;
                        ServiceRep.MobileNumber = MobileNumber;
                        
                        PrintParameters.PrintData = ServiceRep;
                        //شماره پيگيری هم باید از پيغام درخواست باشد
                        PrintParameters.PrintData.Stan = ServiceReq.Stan;
                    }
                    else
                    {
                        //موجودی
                        if (ServiceReq.ProcessCode == Enums.ProcessCode.Remain)
                        {
                            BalanceInquiryReplyParameteres PrintObj = new BalanceInquiryReplyParameteres();
                            PrintObj.CardNumber = ServiceReq.CardNumberP2;
                            PrintObj.BankNam48 = ServiceRep.BankNam48;
                            PrintObj.Stan = ServiceReq.Stan;
                            PrintObj.TranRefNumber = ServiceRep.TranRefNumber;
                            PrintObj.LedgerBalance = ServiceRep.LedgerBalance;
                            PrintObj.AvailableBalance = ServiceRep.AvailableBalance;
                            PrintObj.SpecialMsg = ServiceRep.SpecialMsg;
                            PrintObj.Wage = ServiceRep.Wage;

                            PrintParameters.PrintData = PrintObj;
                        }
                        else //خرید کارت شارژ
                            if (ServiceReq.ProcessCode == Enums.ProcessCode.Charg)
                            {
                                ServiceRep.ChargeParam.ChargeTypeP98 = Convert.ToString(OtherParam[0]);
                                ServiceRep.ChargeParam.BankNam48 = ServiceRep.BankNam48;
                                ServiceRep.ChargeParam.CardNumber = ServiceRep.CardNumber;
                                ServiceRep.ChargeParam.TranRefNumber = ServiceRep.TranRefNumber;
                                ServiceRep.ChargeParam.SpecialMsg = ServiceRep.SpecialMsg;
                                ServiceRep.ChargeParam.Wage = ServiceRep.Wage;
                                PrintParameters.PrintData = ServiceRep.ChargeParam;
                                //شماره پيگيری هم باید از پيغام درخواست باشد
                                PrintParameters.PrintData.Stan = ServiceReq.Stan;

                                #region SetForReversal/Settle

                                //لازم است ذخیره شود که در صورت بروز خطا در وضعيت های بعدی همين کاربر بتوانیم تراکنش برگشت را بسازیم
                                ReversalParameters.ReversalObject = ServiceReq;

                                //لازم است برای پرداخت ذخيره شود
                                //SettleReplyParameters.LastSettleObject = ServiceRep;

                                #endregion SetForReversal/Settle
                            }
                    }

                    #endregion SetForPrint


                }
                else
                    throw new CustomException();


                return ServiceRep;
            }
            catch (CustomException CustEX)
            {
                if (ServiceReq != null)
                {
                    if (ServiceReq.ProcessCode == Enums.ProcessCode.Charg)
                    {
                        //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                        if (ServiceRep != null && ServiceRep.ReplyCodeP39 != null && Convert.ToInt32(ServiceRep.ReplyCodeP39) == 0)
                        {
                            SettleReversal93(ServiceReq, Enums.SpecialServiceType.Reversal);//بسته به نوع خطا

                        }
                        else
                        {
                            //Time Out => Recersal
                            if (ServiceRep != null && ServiceRep.ReplyCodeP39 != null && Convert.ToInt32(ServiceRep.ReplyCodeP39) == 777)
                                SettleReversal93(ServiceReq, Enums.SpecialServiceType.Reversal);
                        }
                    }

                }

                StateManager.Instance.Current.Error(CustEX);
            }
            catch (Exception EX)
            {
                if (ServiceReq != null)
                {
                    if (ServiceReq.ProcessCode == Enums.ProcessCode.Charg)
                    {
                        //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                        if (ServiceRep != null && ServiceRep.ReplyCodeP39 != null && Convert.ToInt32(ServiceRep.ReplyCodeP39) == 0)
                        {
                            SettleReversal93(ServiceReq,Enums.SpecialServiceType.Reversal);//بسته به نوع خطا

                        }
                        else
                        {
                            //Time Out => Recersal
                            if (ServiceRep != null && ServiceRep.ReplyCodeP39 != null && Convert.ToInt32(ServiceRep.ReplyCodeP39) == 777)
                                SettleReversal93(ServiceReq, Enums.SpecialServiceType.Reversal);
                        }
                    }

                }
                StateManager.Instance.Current.Error(EX.Message);
            }

            return ServiceRep;
        }

        public static BillPaymentReplyParameters BillPay93(string BillId, string PayId, Int64 Amount)
        {
            BillPaymentParameters BillpayReq = null;
            BillPaymentReplyParameters BillpayRep = null;
            try
            {
                BillpayReq = GetBillPaymentParam(Enums.SwitchMsgFormat.Shetab93);
                BillpayReq.TranAmountP4 = Amount;
                BillpayReq.BillID = BillId;
                BillpayReq.PayID = PayId;
                BillpayReq.ServiceUseCode = "210101213144";
                BillpayReq.ServiceType = Enums.SpecialServiceType.Financial;
                //ثبت درخواست
                Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(BillpayReq);

                BillpayRep = Transactions.TransactionsInstance.BillPay93(BillpayReq);


                if (BillpayRep != null)
                {
                    //ثبت پاسخ
                    TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(BillpayRep, TranReqId);
                    
                    SwitchValidation.IsValidTran(BillpayReq, BillpayRep);
                    #region SetForPrint

                    BillpayRep.TranAmountP4 = BillpayReq.TranAmountP4;
                    BillpayRep.BillId = BillpayReq.BillID;
                    BillpayRep.PayId = BillpayReq.PayID;
                    BillpayRep.CardNumber = BillpayReq.CardNumberP2;
                    PrintParameters.PrintData = BillpayRep;

                    //تاريخ روی رسيد باید تاریخ درخواست تراکنش باشد
                    PrintParameters.PrintDate = PS.Kiosk.Messaging.Utilities.UtilityMethods.ChangeToShamsi(BillpayReq.TranDate);
                    //شماره پيگيری هم باید از پيغام درخواست باشد
                    PrintParameters.PrintData.Stan = BillpayReq.Stan;

                    #endregion SetForPrint

                    #region SetForReversal/Settle

                    //لازم است ذخیره شود که در صورت بروز خطا در وضعيت های بعدی همين کاربر بتوانیم تراکنش برگشت را بسازیم
                    ReversalParameters.ReversalObject = BillpayReq;

                    //لازم است برای پرداخت ذخيره شود
                    SettleReplyParameters.LastSettleObject = BillpayRep;

                    #endregion SetForReversal/Settle
                }
                else
                    throw new CustomException("");

                

                return BillpayRep;
            }
            catch (CustomException custEx)
            {
                if (BillpayReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 0)
                    {
                        SettleReversal93(BillpayReq,Enums.SpecialServiceType.Reversal);//بسته به نوع خطا

                    }
                    else
                    {
                        //Time Out => Recersal
                        if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 777)
                            SettleReversal93(BillpayReq, Enums.SpecialServiceType.Reversal);
                    }

                }

                StateManager.Instance.Current.Error(custEx);
            }

            catch (Exception Ex)
            {
                if (BillpayReq != null)
                {
                    //از سوييچ جواب درست گرفتیم اما خطا هم گرفتیم و به چاپ نرسیدیم
                    if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 0)
                    {
                        SettleReversal93(BillpayReq, Enums.SpecialServiceType.Reversal);//بسته به نوع خطا

                    }
                    else
                    {
                        //Time Out => Recersal
                        if (BillpayRep != null && BillpayRep.ReplyCodeP39 != null && Convert.ToInt32(BillpayRep.ReplyCodeP39) == 777)
                            SettleReversal93(BillpayReq, Enums.SpecialServiceType.Reversal);
                    }

                }

                StateManager.Instance.Current.Error(Ex.Message);
            }

            return null;
        }

        #region Settle/Reverse

        /// <summary>
        /// درخواست برگشت تراکنش
        /// </summary>
        /// <param name="PrimaryTran"></param>
        public static void Reversal(FinancialParameters PrimaryTran, bool IsInBackground = true, tblReversalTrans revereseRecord = null)
        {
            if (IsInBackground)
            {
                Thread NewThread = new Thread(() => { DoReversal(PrimaryTran, revereseRecord); });
                NewThread.IsBackground = true;
                NewThread.Start();
            }
            else
                DoReversal(PrimaryTran, revereseRecord );
                
            
        }

        /// <summary>
        /// درخواست برگشت تراکنش از جدول برگشت 
        /// </summary>
        /// <param name="PrimaryTran"></param>
        public static void Reversal(tblReversalTrans PrimaryTran, bool IsInBackground = true , bool MultipleTry = true)
        {
            FinancialParameters financeReversal = new FinancialParameters();
            
            financeReversal.CardNumberP2 = PrimaryTran.CardNumber;
            financeReversal.TranAmountP4 = Convert.ToInt64( PrimaryTran.PrimaryNewAmount);
            financeReversal.TranDate = Utility.UtilMethods.GetDateFormat( PrimaryTran.PrimaryDateTime);
            financeReversal.ProcessCode = (Enums.ProcessCode)Convert.ToInt32( PrimaryTran.PrimaryProcessCode);
            financeReversal.Stan = Convert.ToInt32( PrimaryTran.PrimaryStan);
            financeReversal.TranRefNumber = Convert.ToInt64(PrimaryTran.PrimaryRefNumber);
            financeReversal.IsoTrack = PrimaryTran.IsoTrack;
            Reversal(financeReversal, IsInBackground, PrimaryTran);
            
            
        }

        private static void DoReversal(FinancialParameters PrimaryTran, tblReversalTrans revereseRecord = null, bool MultipleTry = true )
        {

            int i;
            int TryCount = 0;
            bool ShouldDelete = true;

            if (MultipleTry)
                TryCount = 10;
            else
                TryCount = 0;

            Int64 ReversalTranID = 0;

            if (PrimaryTran != null)
                KioskLogger.Instance.LogMessage("Enter Reversal For Stan =" + Convert.ToString(PrimaryTran.Stan));

            ReversalParameters reversalParam = null;
            ReversalReplyParameters reversalReplyParameters = null;

            //تا 10 بار تلاش می کنيم
            for (i = 0; i <= TryCount; i++)
            {
                try
                {
                    //ثبت می کنیم که مشخص باشد درحال انجام عملیات برگشت هستیم و تراکنش های بعدی تا انجام 10 بار تلاش منتظر می مانند
                    if (i == 0)
                    {
                        reversalParam = GetReversalParameters(PrimaryTran);

                        if (reversalParam == null)
                            break;

                        if (revereseRecord == null)
                            ReversalTranID = ReversalDataAccess.Instance.InsertNewTransaction(reversalParam);
                        else
                            ReversalTranID = revereseRecord.ID;

                        //اگر شبکه قطع بود درخواست را نمی فرستیم و به بعد موکول می کنیم
                        if (NetworkInterface.GetIsNetworkAvailable() == false)
                        {
                            KioskLogger.Instance.LogMessage("Stop Sending Reversal Becuase of Network Unavalibility");
                            ReversalDataAccess.Instance.UpdateIsIntry(ReversalTranID, false);
                            return;
                        }

                    }

                    //if (i < 11)
                    //    throw new Exception("Exception Test For  Reversal. i =" + i.ToString());

                    //ثبت تراکنش درخواست
                    Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(reversalParam);

                    //انجام تراکنش برگشت
                    reversalReplyParameters = Transactions.TransactionsInstance.Reversal(reversalParam);
                    

                    //ثبت تراکنش پاسخ
                    if (reversalReplyParameters != null)
                        TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(reversalReplyParameters, TranReqId);
                    else
                        throw new Exception("reversalReplyParameters Is Null");


                    if (!string.IsNullOrEmpty(reversalReplyParameters.ReplyCodeP39) && Convert.ToInt32(reversalReplyParameters.ReplyCodeP39) != 0)
                    {
                        //در شرایط زير نباید ادامه دهیم
                        if (SwitchValidation.ContinueTran(reversalReplyParameters) == false)
                        {
                            KioskLogger.Instance.LogMessage("Stop Reversal For ReplyStan = " + Convert.ToString(reversalReplyParameters.Stan) + "-ReplyCode =" + Convert.ToString(reversalReplyParameters.ReplyCodeP39));
                            DeleteContinues(Enums.Transactions.REVERSAL, ReversalTranID);
                            break;
                        }
                        throw new Exception("Unsuccessful Reversal");

                    }


                    //اگر موفق بود از حلقه خارج شود و از جدول حذف می کند
                    KioskLogger.Instance.LogMessage("Successful Reversal For Stan=" + PrimaryTran.Stan.ToString());
                    //برای پاک کردن داخل حلقه بیوفتد
                    DeleteContinues(Enums.Transactions.REVERSAL, ReversalTranID);
                    break;

                }
                catch (Exception EX)
                {
                    if (reversalReplyParameters != null)
                        KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Reversal Try-PrimaryStan =" + Convert.ToString(PrimaryTran.Stan) + "-ReplyCode =" + Convert.ToString(reversalReplyParameters.ReplyCodeP39));
                    else
                        KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Reversal Try-PrimaryStan =" + Convert.ToString(PrimaryTran.Stan));

                    if (MultipleTry == false && TryCount == 0 && i == 0)
                    {//یعنی در بار اول به خطا خردیم
                        //چون تراکنش بعدی هم احتمالا به مشکل برمی خورم پس تراکنش برگشت را پاک نمی کنیم و به بعد موکول می کنیم
                        ShouldDelete = false;
                    }

                }


            }

            if (TryCount == 0)
            {
                //برای پاک کردن داخل حلقه بیوفتد
                if (ShouldDelete)
                    DeleteContinues(Enums.Transactions.REVERSAL, ReversalTranID);
                else
                    ReversalDataAccess.Instance.UpdateIsIntry(ReversalTranID, false);
            }
            else
            //تا 10 بار تلاش کرد نشد
            if (i == TryCount + 1)
            { 

                //اگر تلاشها تمام شد لازم است به بعد موکول شود
                ReversalDataAccess.Instance.UpdateIsIntry(ReversalTranID, false);
            }
        }

        /// <summary>
        /// درخواست پرداخت
        /// </summary>
        public static void Settle(FinancialReplyParameters SettleParam, tblSettleTrans settleRecord = null, bool IsInBackground = true, bool MultipleTry = true)
        {
            if (IsInBackground)
            {
                Thread NewThread = new Thread(() => { DoSettle(SettleParam, settleRecord, MultipleTry); });
                NewThread.IsBackground = true;
                NewThread.Start();
            }
            else
                DoSettle(SettleParam, settleRecord, MultipleTry);
        }

        /// <summary>
        /// درخواست پرداخت برای پرداخت های نا موفقی که در جدول مربوطه ذخيره شده
        /// </summary>
        /// <param name="item"></param>
        public static void Settle(tblSettleTrans item, bool IsInBackground = true, bool MultipleTry = true)
        {
            FinancialReplyParameters financeParam = new FinancialReplyParameters();
            financeParam.Stan = Convert.ToInt32( item.Stan);
            financeParam.TranRefNumber = Convert.ToInt64(item.TranRefNumber);
            financeParam.TranDate = item.SendDateTime;
            Settle(financeParam,item,IsInBackground,MultipleTry);
            SettleDataAccess.Instance.DeleteTransaction(item);
        }

        private static void DoSettle(FinancialReplyParameters SettleParam,tblSettleTrans settleRecord = null, bool MultipleTry = true)
        {
            int i;
            int TryCount = 0;
            bool ShouldDelete = true;

            if (MultipleTry)
                TryCount = 10;
            else
                TryCount = 0;

            Int64 SettleTranID = 0;

            SettleParameters SettleReq = null;
            SettleReplyParameters SettleReply = null;
            SettleTranID = 0;
             //تا 10 بار تلاش می کنيم
            for (i = 0; i <= TryCount; i++)
            {
                
                try
                {


                    if (i == 0)
                    {
                        SettleReq = GetSettleParam();
                        if (SettleReq == null)
                            break;

                        SettleReq.LastSuccedStan = SettleParam.Stan;


                        if (settleRecord == null)
                            SettleTranID = SettleDataAccess.Instance.InsertNewSettleTran(SettleReq);
                        else
                            SettleTranID = settleRecord.ID;

                        //اگر شبکه قطع بود درخواست را نمی فرستیم و به بعد موکول می کنیم
                        if (NetworkInterface.GetIsNetworkAvailable() == false)
                        {
                            KioskLogger.Instance.LogMessage("Stop Sending Settle Becuase of Network Unavalibility");
                            SettleDataAccess.Instance.UpdateIsIntry(SettleTranID, false);
                            return;
                        }
                    }
                   

                    //ثبت درخواست
                    Int64 TranReqID = TransactionsDataAccess.Instance.InsertNewTransaction(SettleReq);

                    //انجام تراکنش پرداخت
                    SettleReply = Transactions.TransactionsInstance.Settle(SettleReq);

                    //ثبت پاسخ
                    if (SettleReply != null)
                        TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(SettleReply, TranReqID);

                    if (!string.IsNullOrEmpty(SettleReply.ReplyCodeP39) && Convert.ToInt32(SettleReply.ReplyCodeP39) != 0)
                    {
                        if (SwitchValidation.ContinueTran(SettleReply) == false)
                        {
                            DeleteContinues(Enums.Transactions.Settele, SettleTranID);
                            KioskLogger.Instance.LogMessage("Stop Settle For ReplyStan" + SettleReply.Stan.ToString() + "-ReplyCode =" + SettleReply.ReplyCodeP39.ToString());
                            break;
                        }
                        else
                            throw new Exception("Unsuccessful Settle");
                    }

                    //اگر موفق بود از حلقه خارج شود و از جدول حذف می کند
                    KioskLogger.Instance.LogMessage("Successful Settle For Stan=" + SettleReq.Stan);
                    DeleteContinues(Enums.Transactions.Settele, SettleTranID);
                    break;
                }
                catch (Exception EX)
                {
                    if (SettleReply != null)
                    KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Settle Try-PrimaryStan =" + SettleReq.Stan.ToString() + "-ReplyCode =" + SettleReply.ReplyCodeP39.ToString());
                    else
                        KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Settle Try-PrimaryStan =" + SettleReq.Stan.ToString()) ;

                    if (MultipleTry == false && TryCount == 0 && i == 0)
                    {
                        ShouldDelete = false;
                    }
                   
                }
            }

            if (TryCount == 0)
            {
                //باید در حلقه بیوفتد
                if (ShouldDelete)
                    DeleteContinues(Enums.Transactions.Settele, SettleTranID);
                else
                    SettleDataAccess.Instance.UpdateIsIntry(SettleTranID, false);
            }
            else
                if (i == TryCount + 1)
                {//اگر تلاشها تمام شد لازم است به بعد موکول شود
                    SettleDataAccess.Instance.UpdateIsIntry(SettleTranID, false);
                }
        }

        /// <summary>
        /// تا 10 بار برای پاک کردن در حلقه می ماند
        /// </summary>
        /// <param name="TranType"></param>
        /// <param name="ID"></param>
        private static void DeleteContinues(Enums.Transactions TranType, Int64 ID)
        {
            if (ID <= 0)
                return;

            bool ShouldDelete = true;
            int TryCount = 0;
            while (ShouldDelete && TryCount <= 10 )
            {
                TryCount++;
                if (TranType == Enums.Transactions.REVERSAL)
                {
                    ShouldDelete = !ReversalDataAccess.Instance.DeleteTransaction(ID);
                    if (ShouldDelete == false)
                        KioskLogger.Instance.LogMessage("Delete after reversal Successfully");
                }
                if (TranType == Enums.Transactions.Settele)
                {
                    ShouldDelete = !SettleDataAccess.Instance.DeleteTransaction(ID);
                    if (ShouldDelete == false)
                        KioskLogger.Instance.LogMessage("Delete after Settle Successfully");
                }

                
            }
 
        }

        #region Version93

        /// <summary>
        /// درخواست برگشت تراکنش
        /// </summary>
        /// <param name="PrimaryTran"></param>
        public static void SettleReversal93(FinancialParameters PrimaryTran, Enums.SpecialServiceType serviceType, bool IsInBackground = true, tblSettleReversTrans revereseRecord = null)
        {
            if (IsInBackground)
            {
                Thread NewThread = new Thread(() => { DoSettleReversal93(PrimaryTran,serviceType,revereseRecord); });
                NewThread.IsBackground = true;
                NewThread.Start();
            }
            else
                DoSettleReversal93(PrimaryTran, serviceType, revereseRecord);


        }

        /// <summary>
        /// درخواست برگشت تراکنش از جدول برگشت 
        /// </summary>
        /// <param name="PrimaryTran"></param>
        public static void SettleReversal93(tblSettleReversTrans PrimaryTran, bool IsInBackground = true, bool MultipleTry = true)
        {
            FinancialParameters financeReversal = new FinancialParameters();

            financeReversal.CardNumberP2 = PrimaryTran.CardNumber;
            financeReversal.TranAmountP4 = Convert.ToInt64(PrimaryTran.PrimaryNewAmount);
            financeReversal.TranDate = Utility.UtilMethods.GetDateFormat(PrimaryTran.PrimaryDateTime);
            financeReversal.ProcessCode = (Enums.ProcessCode)Convert.ToInt32(PrimaryTran.PrimaryProcessCode);
            financeReversal.Stan = Convert.ToInt32(PrimaryTran.PrimaryStan);
            financeReversal.TranRefNumber = Convert.ToInt64(PrimaryTran.PrimaryRefNumber);
            financeReversal.IsoTrack = PrimaryTran.IsoTrack;
            //(financeReversal as SettleReverse93Parameters).ServiceType = (Enums.SpecialServiceType)PrimaryTran.ServiceType;
            SettleReversal93(financeReversal, (Enums.SpecialServiceType)PrimaryTran.ServiceType, IsInBackground, PrimaryTran);

        }

        private static void DoSettleReversal93(FinancialParameters PrimaryTran, Enums.SpecialServiceType serviceType, tblSettleReversTrans revereseRecord = null, bool MultipleTry = true)
        {

            int i;
            int TryCount = 0;
            bool ShouldDelete = true;

            if (MultipleTry)
                TryCount = 10;
            else
                TryCount = 0;

            Int64 ReversalTranID = 0;

            if (PrimaryTran != null)
                KioskLogger.Instance.LogMessage("Enter Reversal For Stan =" + Convert.ToString(PrimaryTran.Stan));

            SettleReverse93Parameters reversalParam = null;
            SettleReverse93ReplyParameters reversalReplyParameters = null;

            //تا 10 بار تلاش می کنيم
            for (i = 0; i <= TryCount; i++)
            {
                try
                {
                    //ثبت می کنیم که مشخص باشد درحال انجام عملیات برگشت هستیم و تراکنش های بعدی تا انجام 10 بار تلاش منتظر می مانند
                    if (i == 0)
                    {
                        reversalParam = GetSettleReverseParameters(PrimaryTran, serviceType);

                        if (reversalParam == null)
                            break;

                        if (revereseRecord == null)
                            ReversalTranID = ReversalDataAccess.Instance.InsertNewTransaction93(reversalParam);
                        else
                            ReversalTranID = revereseRecord.ID;

                        //اگر شبکه قطع بود درخواست را نمی فرستیم و به بعد موکول می کنیم
                        if (NetworkInterface.GetIsNetworkAvailable() == false)
                        {
                            KioskLogger.Instance.LogMessage("Stop Sending Reversal Becuase of Network Unavalibility");
                            ReversalDataAccess.Instance.UpdateIsIntry(ReversalTranID, false);
                            return;
                        }

                    }

                    //if (i < 11)
                    //    throw new Exception("Exception Test For  Reversal. i =" + i.ToString());

                    //ثبت تراکنش درخواست
                    Int64 TranReqId = TransactionsDataAccess.Instance.InsertNewTransaction(reversalParam);

                    //انجام تراکنش برگشت
                    reversalReplyParameters = Transactions.TransactionsInstance.SettleReverse93(reversalParam);


                    //ثبت تراکنش پاسخ
                    if (reversalReplyParameters != null)
                        TransactionsReplyDataAccess.Instance.InsertNewReplyTransaction(reversalReplyParameters, TranReqId);
                    else
                        throw new Exception("reversalReplyParameters Is Null");


                    if (!string.IsNullOrEmpty(reversalReplyParameters.ReplyCodeP39) && Convert.ToInt32(reversalReplyParameters.ReplyCodeP39) != 0)
                    {
                        

                        //در شرایط زير نباید ادامه دهیم
                        if (SwitchValidation.ContinueTran(reversalReplyParameters) == false)
                        {
                            KioskLogger.Instance.LogMessage("Stop Reversal For ReplyStan = " + Convert.ToString(reversalReplyParameters.Stan) + "-ReplyCode =" + Convert.ToString(reversalReplyParameters.ReplyCodeP39));
                            DeleteContinues93(ReversalTranID);
                            break;
                        }
                        throw new Exception("Unsuccessful Reversal");

                    }


                    //اگر موفق بود از حلقه خارج شود و از جدول حذف می کند
                    KioskLogger.Instance.LogMessage("Successful Reversal For Stan=" + PrimaryTran.Stan.ToString());
                    //برای پاک کردن داخل حلقه بیوفتد
                    DeleteContinues93(ReversalTranID);
                    break;

                } 
                catch (Exception EX)
                {
                    if (reversalReplyParameters != null)
                        KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Reversal Try-PrimaryStan =" + Convert.ToString(PrimaryTran.Stan) + "-ReplyCode =" + Convert.ToString(reversalReplyParameters.ReplyCodeP39));
                    else
                        KioskLogger.Instance.LogMessage(EX, i.ToString() + "th Time in Reversal Try-PrimaryStan =" + Convert.ToString(PrimaryTran.Stan));

                    if (MultipleTry == false && TryCount == 0 && i == 0)
                    {//یعنی در بار اول به خطا خردیم
                        //چون تراکنش بعدی هم احتمالا به مشکل برمی خورم پس تراکنش برگشت را پاک نمی کنیم و به بعد موکول می کنیم
                        ShouldDelete = false;
                    }

                }


            }

            if (TryCount == 0)
            {
                //برای پاک کردن داخل حلقه بیوفتد
                if (ShouldDelete)
                    DeleteContinues93(ReversalTranID);
                else
                    ReversalDataAccess.Instance.UpdateIsIntry93(ReversalTranID, false);
            }
            else
                //تا 10 بار تلاش کرد نشد
                if (i == TryCount + 1)
                {
                    //اگر تلاشها تمام شد لازم است به بعد موکول شود
                    ReversalDataAccess.Instance.UpdateIsIntry93(ReversalTranID, false);
                }
        }

        private static void DeleteContinues93(Int64 ID)
        {
            if (ID <= 0)
                return;

            bool ShouldDelete = true;
            int TryCount = 0;
            while (ShouldDelete && TryCount <= 10)
            {
                TryCount++;
                
                    ShouldDelete = !ReversalDataAccess.Instance.DeleteTransaction93(ID);
                    if (ShouldDelete == false)
                        KioskLogger.Instance.LogMessage("Delete after reversal/Settle Successfully");
               


            }
        }

        #endregion Version93

        #endregion Settle/Reverse

        #endregion Transactions


    }
}
