using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using PS.Kiosk.Common.Model;
using System.Timers;
using System.IO;
using PS.Kiosk.Data;
using PS.Kiosk.Framework;
using PS.Kiosk.DeviceController.Services;
using PS.Kiosk.Data.DataAccessObjects;
using PS.Kiosk.Framework.ExceptionManagement;
using System.Windows;

namespace PS.Kiosk.Business
{
    public class KioskBusiness : IStatable, IDataErrorInfo
    {
        #region Creation

        public KioskBusiness()
        {

        }

        #endregion // Creation

        #region Properties

        private States _CurrentState;
        /// <summary>
        /// تعيين وضعيت کنونی
        /// </summary>
        public States CurrentState
        {
            get { return _CurrentState; }
            set { _CurrentState = value; NotifyPropertyChanged("CurrentState"); }
        }

        private States _PreviousState;
        /// <summary>
        /// فقط جهت نگداری وضعيت قبلی
        /// </summary>
        public States PreviousState
        {
            get { return _PreviousState; }

            set { _PreviousState = value; }

        }

        private States _NextState;
        /// <summary>
        /// فقط جهت نگداری وضعيت بعدی
        /// </summary>
        public States NextState
        {
            get
            {
                return _NextState;
            }
            set
            {
                _NextState = value;
            }
        }

        private States _ReturnState;
        /// <summary>
        /// فقط جهت نگهداری وضعیت بازگشت از وضعيت جاری
        /// </summary>
        public States ReturnsState
        {
            get
            {
                return _ReturnState;
            }
            set
            {
                _ReturnState = value;
            }
        }

        private string _BindedPropertyName;
        /// <summary>
        ///باید داده را بفرستد Property می فهمد به کدام  Epp
        /// </summary>
        public string BindedPropertyName
        {
            get
            {
                if (string.IsNullOrEmpty(_BindedPropertyName))
                    _BindedPropertyName = "Message";
                return _BindedPropertyName;
            }
            set
            {
                if (value == null)
                    value = "Message";
                _BindedPropertyName = value;
            }
        }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get
            {
                return _ErrorMsg;
            }
            set
            {
                _ErrorMsg = value;
                NotifyPropertyChanged("ErrorMsg");
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; NotifyPropertyChanged("Message"); }
        }

        private string _Message2;
        public string Message2
        {
            get
            {
                return _Message2;
            }
            set
            {
                _Message2 = value;
                NotifyPropertyChanged("Message2");
            }
        }

        private string _Message3;
        public string Message3
        {
            get
            {
                return _Message3;
            }
            set
            {
                _Message3 = value;
                NotifyPropertyChanged("Message3");
            }
        }

        private bool _ShouldDetectForCardCapturing;
        /// <summary>
        /// اگر ترو بود یعنی خطاهایی که باید کارت ضبط شود کارت را ضبط کند
        /// </summary>
        public bool ShouldDetectForCardCapturing
        {
            get
            {
                return _ShouldDetectForCardCapturing;
            }
            set
            {
                _ShouldDetectForCardCapturing = value;
            }
        }

        private bool _ShouldCaptureCard;
        /// <summary>
        /// کارت ضبط شود
        /// </summary>
        public bool ShouldCaptureCard
        {
            get
            {
                return _ShouldCaptureCard;
            }
            set
            {
                _ShouldCaptureCard = value;
            }
        }

        private bool _EnableControl;
        public bool EnableControl
        {
            get
            {
                return _EnableControl;
            }
            set
            {
                _EnableControl = value; NotifyPropertyChanged("EnableControl");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Properties

        #region State Properties

        public string KioskState { get; set; }

        #endregion // State Properties

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        #endregion // IDataErrorInfo Members

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        static readonly string[] ValidatedProperties = 
        { 
            "State"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                default:
                    Debug.Fail("Unexpected property being validated on Kiosk: " + propertyName);
                    break;
            }

            return error;
        }

        static bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }

        #endregion // Validation

        #region Implement IKiosk

        private Dictionary<object, object> _PropertyDic;
        public Dictionary<object, object> PropertyDic
        {
            get
            {
                if (_PropertyDic == null)
                    _PropertyDic = new Dictionary<object, object>();
                return _PropertyDic;
            }
            set
            {
                _PropertyDic = value;
            }
        }

        public event TriggerEventHandler OnConnected;
        private void Connect()
        {
            if (null != OnConnected)
            {
                OnConnected(this, new TriggerEventArgs(TriggerEvents.OnConnected));
            }
        }
        public void Connecting()
        {
            // اگر کارت داخل دستگاه باشد باید بگیرد اما در زمان برنامه نویسی آن را به بیرون می اندازد 
            //CardReaderService.Instance.CaptureCard();


            CardReaderService.Instance.EjectCard();
            CardReaderService.Instance.NoAcceptCard();

            
            
            if (SwitchBusiness.SignOnToSwitch(this))
            {
                this.Message = string.Empty;
                this.Connect();

                //برای انجام برگشت و پرداخت به صورت دوره ای
                //ControlReversal();
                //ControlSettle();

               
            }

        }

        public event TriggerEventHandler OnError;
        public void Error(string ErrorMessage)
        {
            this.Error(new CustomException(ErrorMessage));
        }
        public void Error(Exception ErrorEx)
        {

            //CardReaderService.Instance.EjectCard();
            //CardReaderService.Instance.NoAcceptCard();


            KioskLogger.Instance.LogMessage(ErrorEx, "");


            if (PropertyDic.ContainsKey("ErrorMsg"))
                PropertyDic.Remove("ErrorMsg");
            PropertyDic.Add("ErrorMsg", ErrorEx.Message);

            if (null != OnError)
            {
                OnError(this, new TriggerEventArgs(TriggerEvents.OnError));
            }


        }
        public void ErrorMsgShow()
        {
            if (PropertyDic.Count > 0)
                if (PropertyDic.ContainsKey("ErrorMsg") && PropertyDic["ErrorMsg"] != null)
                    this.ErrorMsg = Convert.ToString(PropertyDic["ErrorMsg"]);
        }

        public event TriggerEventHandler OnReturn;
        public void Return()
        {
            if (OnReturn != null)
            {
                OnReturn(this, new TriggerEventArgs(TriggerEvents.OnReturn));
            }
        }

        public event TriggerEventHandler OnShowWaiting;
        private void ShowWaiting(States NextState)
        {



            this.NextState = NextState;
            if (OnShowWaiting != null)
            {
                OnShowWaiting(this, new TriggerEventArgs(TriggerEvents.OnShowWaiting));
            }

        }
        public void ShowWaiting(States NextState, params object[] Parameters)
        {
            try
            {


                if (NextState == States.GetChargeState)
                {
                    if (Parameters != null)
                    {
                        ChargeType Type = (ChargeType)Parameters[0];
                        this.AddToDictionary("ChargeType", (int)Type);
                        this.AddToDictionary("ChargeAmount", Parameters[1].ToString());
                    }
                }

                //if (NextState == States.GetHamrahavalChargeState)
                //{
                //    if (Parameters != null)
                //    {
                //        //ChargeType Type = (ChargeType)Parameters[0];
                //        string key = "HamrahAval";

                //        if (PropertyDic.ContainsKey(key))
                //            PropertyDic.Remove(key);
                //        PropertyDic.Add(key, Parameters[1].ToString());
                //    }
                //}

                //if (NextState == States.GetTaliaChargeState)
                //{
                //    if (Parameters != null)
                //    {
                //        ChargeType Type = (ChargeType)Parameters[0];
                //        this.AddToDictionary("Talia", Parameters[1].ToString());
                //    }
                //}

                if (NextState == States.PayBillState)
                {
                    if (Parameters != null)
                    {

                        this.AddToDictionary("BillID", Convert.ToString(Parameters[0]));
                        this.AddToDictionary("PayID", Convert.ToString(Parameters[1]));
                        this.AddToDictionary("PayAmount", Convert.ToString(Parameters[2]));
                    }
                }

                //if (NextState == States.PayJiringState)
                //{
                //    if (Parameters != null)
                //    {
                //        this.AddToDictionary("MobileNumber", Convert.ToString(Parameters[0]));
                //        this.AddToDictionary("JiringPayAmount", Convert.ToString(Parameters[1]));
                //    }
                //}

                if (NextState == States.PaySpeciallServicesState)
                {
                    string ServiceType = Convert.ToString(PropertyDic["SpecialServiceType"]);
                    if ((Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)) != Enums.SpecialServiceType.Financial)
                    {
                        StopEppTimer();

                        if (Parameters != null)
                        {
                            if (Parameters[0] != null)
                                if (!string.IsNullOrEmpty(Convert.ToString(Parameters[0])))
                                    this.AddToDictionary("MobileNumber", Convert.ToString(Parameters[0]));

                            this.AddToDictionary("PayAmount", Convert.ToString(Parameters[1]));

                            if (CurrentState == States.ShowMobinetPaymentState)
                                this.AddToDictionary("ServiceId", Convert.ToString(Parameters[2]));
                        }
                    }
                }

                if (NextState == States.ShowHamrahAvalBillInfoState)
                {
                    StopEppTimer();
                    if (Parameters != null)
                    {
                        this.AddToDictionary("MobileNumber", Convert.ToString(Parameters[0]));
                    }
                }

                this.ShowWaiting(NextState);
            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }

        public event TriggerEventHandler OnRestart;
        public void Restart()
        {
            if (OnRestart != null)
                OnRestart(this, new TriggerEventArgs(TriggerEvents.OnRestart));
        }

        public event TriggerEventHandler OnShowBusy;
        public void ShowBusy(States NextState)
        {
            this.NextState = NextState;
            if (OnShowBusy != null)
            {
                OnShowBusy(this, new TriggerEventArgs(TriggerEvents.OnShowBusy));
            }
        }

        public void WaitForCard()
        {
            CardReaderService.Instance.AcceptCard();
            CardReaderService.Instance.WaitForCard();

            //در زمان بیکاری سیستم که منتظر دریافت کار است چک می کند که به زمان لاگ آن مجدد رسیده یا خیر
            ControlForRestart();
        }

        public event TriggerEventHandler OnReceivedCard;
        public void ReceivedCard()
        {
            //زمانی که کارت را دریافت کرد دیگر نباید برای لاگ آن مجدد چک کند
            SharedValue.checkForRestart = false;

            if (null != OnReceivedCard)
            {
                OnReceivedCard(this, new TriggerEventArgs(TriggerEvents.OnReceivedCard));
            }
        }

        public void GetPin()
        {
            this.Message = string.Empty;
            BindedPropertyName = "Message";
            EppService.Instance.GetPin();

        }

        public void GetNumber()
        {
            this.Message = string.Empty;

            EppService.Instance.GetNumber();


        }

        public event TriggerEventHandler OnFinishingSession;
        public void FinishSession()
        {
            if (null != OnFinishingSession)
            {
                OnFinishingSession(this, new TriggerEventArgs(TriggerEvents.OnFinishingSession));
            }
        }

        public void FinishingSession()
        {
            if (StateManager.Instance.Current.ShouldCaptureCard)
            {
                CardReaderService.Instance.CaptureCard();
                StateManager.Instance.Current.ShouldCaptureCard = false;
            }
            else
                CardReaderService.Instance.EjectCard();

            ParametersDataAccess.Instance.Clear();
        }

        public event TriggerEventHandler OnAchievedPin;
        public void AchievedPin()
        {
            if (null != OnAchievedPin)
            {
                OnAchievedPin(this, new TriggerEventArgs(TriggerEvents.OnAchievedPin));
            }
        }

        public event TriggerEventHandler OnGetBalanceInquiryAction;
        public void GetBalanceInquiryAction()
        {
            if (null != OnGetBalanceInquiryAction)
            {
                OnGetBalanceInquiryAction(this, new TriggerEventArgs(TriggerEvents.OnGetBalanceInquiryAction));
            }

        }

        public void GettingBalanceInquiryAction()
        {
            try
            {

                #region 87Format
                //decimal res = SwitchBusiness.GetBalanceInquiry();
                #endregion 87Format

                SpecialServiceReplyParameters reply = SwitchBusiness.SpecialServicePay(string.Empty, 0, Enums.SpecialServiceType.Financial);
                decimal res = reply.LedgerBalance;

                if (res >= 0)
                {
                    string result = Utility.UtilMethods.GetMoneyFormat(res.ToString());
                    this.AddToDictionary("CardBalance", result);
                    this.EnableControl = true;
                    this.GetBalanceInquiryAction();
                    this.Message = result;

                }


            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }

        public event TriggerEventHandler OnShowChargeType;
        public void ShowChargeType()
        {
            if (OnShowChargeType != null)
            {
                OnShowChargeType(this, new TriggerEventArgs(TriggerEvents.OnShowChargeType));
            }
        }


        public event TriggerEventHandler OnShowIrancellChargeType;
        public void ShowIrancellChargeType()
        {
            if (OnShowIrancellChargeType != null)
            {
                OnShowIrancellChargeType(this, new TriggerEventArgs(TriggerEvents.OnShowIrancellChargeType));
            }
        }

        public event TriggerEventHandler OnGetCharge;
        public void GetChargeAction()
        {
            if (OnGetCharge != null)
            {
                OnGetCharge(this, new TriggerEventArgs(TriggerEvents.OnGetCharge));
            }
        }

        public void GettingChargeAction()
        {
            try
            {
                Int64 value = 0;
                if (Int64.TryParse(Convert.ToString(PropertyDic["ChargeAmount"]), out value))
                {
                    int ChargeType = Convert.ToInt32(PropertyDic["ChargeType"]);

                    #region 87
                    //PurchaseChargeReplyParameters reply = SwitchBusiness.PurchaceCharge87(Enums.ChargeType.Irancell, Convert.ToInt64(value));
                    #endregion 87

                    SpecialServiceReplyParameters reply = SwitchBusiness.SpecialServicePay(string.Empty, Convert.ToInt64(value), Enums.SpecialServiceType.Financial, string.Empty, ChargeType);


                    if (reply != null)
                        this.GetChargeAction();
                }
                else
                    this.Error("Invalid Reply Object");

            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }

        }

        public event TriggerEventHandler OnShowHamrahAvalChargeType;
        public void ShowHamrahAvalChargeType()
        {
            if (OnShowHamrahAvalChargeType != null)
            {
                OnShowHamrahAvalChargeType(this, new TriggerEventArgs(TriggerEvents.OnShowHamrahAvalChargeType));
            }
        }

        //public event TriggerEventHandler OnGetHamrahAvalCharge;
        //public void GetHamrahAvalChargeAction()
        //{
        //    if (OnGetHamrahAvalCharge != null)
        //        OnGetHamrahAvalCharge(this, new TriggerEventArgs(TriggerEvents.OnGetHamrahAvalCharge));
        //}
        //public void GettingHamrahAvalChargeAction()
        //{
        //    try
        //    {
        //        ChargeType type = ChargeType.None;
        //        string value = PropertyDic["HamrahAval"].ToString();

                

        //        //PurchaseChargeReplyParameters reply;
        //        //reply = SwitchBusiness.PurchaceCharge87(Enums.ChargeType.HamrahAval, Convert.ToInt64(value));

        //        SpecialServiceReplyParameters reply = SwitchBusiness.SpecialServicePay(string.Empty, Convert.ToInt64(value), Enums.SpecialServiceType.Financial, string.Empty, (int)Enums.ChargeType.HamrahAval);

        //        if (reply != null)
        //            this.GetHamrahAvalChargeAction();

        //    }
        //    catch (Exception EX)
        //    {

        //        Error(EX.Message);
        //    }
        //}


        public event TriggerEventHandler OnShowTaliaChargeType;
        public void ShowTaliaChargeType()
        {
            if (OnShowTaliaChargeType != null)
                OnShowTaliaChargeType(this, new TriggerEventArgs(TriggerEvents.OnShowTaliaChargeType));
        }

        //public event TriggerEventHandler OnGetTaliaCharge;
        //public void GetTaliaChargeAction()
        //{
        //    if (OnGetTaliaCharge != null)
        //        OnGetTaliaCharge(this, new TriggerEventArgs(TriggerEvents.OnGetTaliaCharge));
        //}

        //public void GettingTaliaChargeAction()
        //{
        //    try
        //    {
        //        ChargeType type = ChargeType.None;
        //        string value = PropertyDic["Talia"].ToString();

        //        //PurchaseChargeReplyParameters reply;
        //        //reply = SwitchBusiness.PurchaceCharge87(Enums.ChargeType.Talia, Convert.ToInt64(value));

        //        SpecialServiceReplyParameters reply = SwitchBusiness.SpecialServicePay(string.Empty, Convert.ToInt64(value), Enums.SpecialServiceType.Financial, string.Empty, (int)Enums.ChargeType.Talia);


        //        if (reply != null)
        //            this.GetTaliaChargeAction();

        //    }
        //    catch (Exception EX)
        //    {

        //        Error(EX.Message);
        //    }
        //}

        public event TriggerEventHandler OnShowRightelChargeType;
        public void ShowRightelChargeType()
        {
            if (OnShowRightelChargeType != null)
            {
                OnShowRightelChargeType(this,new TriggerEventArgs(TriggerEvents.OnShowRightelChargeType));
            }
        }

        public event TriggerEventHandler OnExitAction;
        public void Exit()
        {
            if (OnExitAction != null)
            {
                OnExitAction(this, new TriggerEventArgs(TriggerEvents.OnExitAction));
            }
        }

        public event TriggerEventHandler OnPrinting;
        public void Printing()
        {
            if (OnPrinting != null)
            {
                OnPrinting(this, new TriggerEventArgs(TriggerEvents.OnPrinting));
            }
        }

        public event TriggerEventHandler OnPayBill;
        public void PayBill()
        {

            if (OnPayBill != null)
                OnPayBill(this, new TriggerEventArgs(TriggerEvents.OnPayBill));


        }

        public void PayBillAction()
        {
            try
            {
                StopEppTimer();
                string BillId = Convert.ToString(PropertyDic["BillID"]);
                string PayId = Convert.ToString(PropertyDic["PayID"]);
                Int64 PayAmount = Convert.ToInt64(PropertyDic["PayAmount"]);
                //BillPaymentReplyParameters reply = SwitchBusiness.BillPay87(BillId, PayId, PayAmount);
                BillPaymentReplyParameters reply = SwitchBusiness.BillPay93(BillId, PayId, PayAmount);
                if (reply != null)
                    PayBill();
                else
                    this.Error("Invalid Reply Object");


            }
            catch (Exception EX)
            {

                Error(EX);
            }
        }

        public event TriggerEventHandler OnShowPayBill;
        public void ShowPayBill()
        {
            if (OnShowPayBill != null)
            {
                OnShowPayBill(this, new TriggerEventArgs(TriggerEvents.OnShowPayBill));
            }
        }

        public event TriggerEventHandler OnShowJiring;
        public void ShowJiring()
        {
            if (OnShowJiring != null)
            {
                OnShowJiring(this, new TriggerEventArgs(TriggerEvents.OnShowJiring));
            }
        }

        public event TriggerEventHandler OnShowHamrahAvalServices;
        public void ShowHamrahAvalServices()
        {
            if (OnShowHamrahAvalServices != null)
                OnShowHamrahAvalServices(this, new TriggerEventArgs(TriggerEvents.OnShowHamrahAvalServices));
        }

        public event TriggerEventHandler OnShowIrancellServices;
        public void ShowIrancellServices()
        {
            if (OnShowIrancellServices != null)
            {
                OnShowIrancellServices(this, new TriggerEventArgs(TriggerEvents.OnShowIrancellServices));
            }
        }

        public event TriggerEventHandler OnShowIrancellServicePayment;
        public void ShowIrancellServicePayment()
        {
            if (OnShowIrancellServicePayment != null)
            {
                OnShowIrancellServicePayment(this, new TriggerEventArgs(TriggerEvents.OnShowIrancellServicePayment));
            }
        }

        public event TriggerEventHandler OnPaySpeciallServices;
        public void PaySpecialServices()
        {
            if (OnPaySpeciallServices != null)
            {
                OnPaySpeciallServices(this, new TriggerEventArgs(TriggerEvents.OnPaySpeciallServices));
            }
        }

        public void PaySpecialServiceAction()
        {
            string ServiceType = Convert.ToString(PropertyDic["SpecialServiceType"]);

            SpecialServiceReplyParameters SpecialServiceReply = null;
            try
            {
                if ((Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)) != Enums.SpecialServiceType.Financial)
                {

                    string MobileNumber = Convert.ToString(PropertyDic["MobileNumber"]);
                    Int64 PayAmount;

                    if (Int64.TryParse(Convert.ToString(PropertyDic["PayAmount"]), out PayAmount) && PayAmount > 0)
                    {


                        if (Convert.ToInt32(ServiceType) == (int)Enums.SpecialServiceType.Mobinnet)
                        {
                            string ServiceId = Convert.ToString(PropertyDic["ServiceId"]);
                            SpecialServiceReply = SwitchBusiness.SpecialServicePay(MobileNumber, PayAmount, Enums.SpecialServiceType.Mobinnet, ServiceId);
                        }
                        else
                            SpecialServiceReply = SwitchBusiness.SpecialServicePay(MobileNumber, PayAmount, (Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)));
                    }
                    else
                        throw new Exception("Invalid Parameters");

                    if (SpecialServiceReply != null)
                        PaySpecialServices();
                    else
                        this.Error("Invalid Reply Object");
                }
                //else
                //{
                //    SpecialServiceReply = SwitchBusiness.SpecialServicePay(string.Empty, 0, (Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)));

                //    decimal res = 0;
                //    if (res >= 0)
                //    {
                //        string result = Utility.UtilMethods.GetMoneyFormat(res.ToString());
                //        this.AddToDictionary("CardBalance", result);
                //        this.EnableControl = true;
                //        this.GetBalanceInquiryAction();
                //        this.Message = result;

                //    }
                //}


            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }

        public event TriggerEventHandler OnShowHamarahAvalBill;
        public void ShowHamarahAvalBill()
        {
            if (OnShowHamarahAvalBill != null)
                OnShowHamarahAvalBill(this, new TriggerEventArgs(TriggerEvents.OnShowHamrahAvalBill));
        }

        public event TriggerEventHandler OnShowHamrahAvalBillInfo;
        public void ShowHamrahAvalBillInfo()
        {
            if (OnShowHamrahAvalBillInfo != null)
                OnShowHamarahAvalBill(this, new TriggerEventArgs(TriggerEvents.OnShowHamrahAvalBillInfo));
        }

        /// <summary>
        /// دریافت اطلاعات قبض 93 ای
        /// </summary>
        public void GetSpecialServiceBillInfo()
        {
            string ServiceType = Convert.ToString(PropertyDic["SpecialServiceType"]);

            SpecialServiceReplyParameters SpecialServiceReply1 = null;
            //SpecialServiceReplyParameters SpecialServiceReply2 = null;
            try
            {


                string MobileNumber = Convert.ToString(PropertyDic["MobileNumber"]);


                if (Convert.ToInt32(ServiceType) == (int)Enums.SpecialServiceType.HamrahAvalFinalTermInfo ||
                        Convert.ToInt32(ServiceType) == (int)Enums.SpecialServiceType.HamrahAvalMidTermInfo)
                {
                    //
                    SpecialServiceReply1 = SwitchBusiness.SpecialServicePay(MobileNumber, 0, Enums.SpecialServiceType.HamrahAvalFinalTermInfo);
                    //if (SpecialServiceReply1 != null)
                    //    SpecialServiceReply2 = SwitchBusiness.SpecialServicePay(MobileNumber, 0, Enums.SpecialServiceType.HamrahAvalMidTermInfo);
                }

                if (SpecialServiceReply1 != null )//&& SpecialServiceReply2 != null)
                {
                    string[] Data = SpecialServiceReply1.ExtraData.Split(new string[] { ";" }, StringSplitOptions.None);
                    if (Data.Count() >= 2)
                    {
                        this.AddToDictionary("FinalTermAmount", Utility.UtilMethods.GetMoneyFormat(Data[0]));
                        this.AddToDictionary("MidTermAmount", Utility.UtilMethods.GetMoneyFormat(Data[1]));

                        ShowHamrahAvalBillInfo();
                    }
                    else
                        this.Error("Invalid Reply Object");
                }
                else
                    this.Error("Invalid Reply Object");
                

            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }


        public event TriggerEventHandler OnShowMobinetPayment;
        public void ShowMobinetPayment()
        {
            if (OnShowMobinetPayment != null)
                OnShowMobinetPayment(this, new TriggerEventArgs(TriggerEvents.OnShowMobinetPayment));
        }

        public event TriggerEventHandler OnShowHamrahAvalTopUp;
        public void ShowHamrahAvalTopUp()
        {
            if (OnShowHamrahAvalTopUp != null)
            {
                OnShowHamrahAvalTopUp(this, new TriggerEventArgs(TriggerEvents.OnShowHamrahAvalTopUp));
            }
        }

        #endregion


        /// <summary>
        /// با تغيير وضعيت اين متغيير ها مقداردهی اوليه(خالی) می شوند
        /// </summary>
        public void ClearStateProperties()
        {
            this.ErrorMsg = string.Empty;
            this.Message = string.Empty;
            this.Message2 = string.Empty;
            this.Message3 = string.Empty;
            this.EnableControl = false;
            this.BindedPropertyName = string.Empty;
            this.ShouldCaptureCard = false;

            if (this.CurrentState == States.FinishingSessionState)
            {
                //با پايان پذیرفتن کارلازم است این متغییرها خالی شوند
                PrintParameters.PrintData = null;
                PrintParameters.PrintDate = string.Empty;
                ReversalParameters.ReversalObject = null;
                SettleReplyParameters.LastSettleObject = null;
                PropertyDic.Clear();

                PreviousState = States.EmptyState;
                NextState = States.EmptyState;
                ReturnsState = States.EmptyState;
            }

        }

        public bool CheckPrinterStatus()
        {
            try
            {
                return PrinterService.IsAvailable();
            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }

            return false;
        }

        #region CustomPrivateMethod

        /// <summary>
        /// در هر 10 ثانيه چک می کند که آيا تراکنشی برای برگشت وجود دارد یا نه
        /// اگر بود درخواست برگشت را می فرستد
        /// </summary>
        private void ControlDynamicReversal()
        {
            ThreadManager.DoRepeatedly(DoReversalTimer_Elapsed, 2000);
        }

        /// <summary>
        ///ديگر اجرا می شود Thread چون از تایمر استفاده می شود خود به خود در یک
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoReversalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ((Timer)sender).Stop();
                DoReversal();
                ((Timer)sender).Start();
            }
            catch (Exception EX)
            {
                (sender as Timer).Start();
                KioskLogger.Instance.LogMessage(EX, "");
            }

        }

        public void DoReversal(bool IsInBackground = false)
        {
            try
            {
                Console.WriteLine("Enter For Reversal Before Login");
                KioskLogger.Instance.LogMessage("Enter For Reversal Before Login");

                List<tblSettleReversTrans> reversalList = GetReversalList93();

                KioskLogger.Instance.LogMessage("reversalList Count = " + reversalList.Count);

                if (reversalList != null && reversalList.Count > 0)
                {
                    //foreach (tblReversalTrans item in reversalList)
                    //{

                    //اخرین تراکنش برگشتی که ترد دیگری مشغول ارسال آن نیست
                    IEnumerable<tblSettleReversTrans> tbl = reversalList.Where(i => i.IsInTry == false);
                    if (tbl.Count() > 0)
                    {
                        tblSettleReversTrans item = tbl.Last();
                        //SwitchBusiness.SettleReversal93(item, IsInBackground, false);
                        SwitchBusiness.SettleReversal93(item, IsInBackground,false);
                        KioskLogger.Instance.LogMessage("Reverese Successfully");
                    }
                    //}

                    
                    
                }

            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }

        public void DoSettle(bool IsInBackground = false)
        {
            try
            {
                
                KioskLogger.Instance.LogMessage("Enter For Settle Before Trans");

                List<tblSettleTrans> settlelList = GetSettleList();
                if (settlelList != null && settlelList.Count > 0)
                {
                    //foreach (tblReversalTrans item in reversalList)
                    //{

                    //اخرین تراکنش برگشتی که ترد دیگری مشغول ارسال آن نیست
                    IEnumerable<tblSettleTrans> tbl = settlelList.Where(i => i.IsInTry == false);
                    if (tbl.Count() > 0)
                    {
                        tblSettleTrans item = tbl.Last();
                        SwitchBusiness.Settle(item, IsInBackground, false);
                        KioskLogger.Instance.LogMessage("Reverese Successfully");
                    }
                    //}

                   
                    
                }

            }
            catch (Exception EX)
            {

                Error(EX.Message);
            }
        }

        /// <summary>
        /// با فاصله زمانی مشخصی چک می کند که به زمان لاگ آن مجدد رسیدیم یا خیر
        /// </summary>
        public void ControlForRestart()
        {
            SharedValue.checkForRestart = true;
            ThreadManager.DoRepeatedly(DoRestartTimer_Elapsed, 1000);
        }

        private void DoRestartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ((Timer)sender).Stop();
                
                StateManager.Instance.Current.DoRestart();
                //مهم
                if (SharedValue.checkForRestart == false)
                    ((Timer)sender).Stop();
                else
                    ((Timer)sender).Start();
            }
            catch (Exception EX)
            {
                (sender as Timer).Start();
                KioskLogger.Instance.LogMessage(EX, "");
            }
        }

        void IStatable.DoRestart()
        {
            string restartTime = ParametersDataAccess.RestartTime;
            DateTime restartdateTime = new DateTime(Convert.ToInt32( restartTime.Substring(0,4)),
                Convert.ToInt32(restartTime.Substring(4, 2)), Convert.ToInt32(restartTime.Substring(6, 2)),
                Convert.ToInt32(restartTime.Substring(8, 2)), Convert.ToInt32(restartTime.Substring(10, 2)), Convert.ToInt32(restartTime.Substring(12, 2)));

            //ساعت به زمان مورد نظر رسیده باشد 
            if ( (DateTime.Now.Year == restartdateTime.Year &&
                DateTime.Now.Month == restartdateTime.Month &&
                DateTime.Now.Day == restartdateTime.Day  &&
                DateTime.Now.ToString("HH:mm:ss").Replace(":", "") == restartTime.Substring(restartTime.Length - 6, 6)) ||

                //یا یک روز تاریخ جلو رفته باشد که در ساعت 12 شب اتفاق می افتد
                (DateTime.Now.Year == restartdateTime.Year &&
                DateTime.Now.Month == restartdateTime.Month &&
                ( DateTime.Now.Day == restartdateTime.Day + 1))
                
            )
            {
                
               
                if (SharedValue.isAppRestart == false)
                {
                    KioskLogger.Instance.LogMessage("Restart App At :" + DateTime.Now.ToString()); 
                    SharedValue.isAppRestart = true;
                    StateManager.Instance.Current.ShowBusy(States.RestartState);
                }
            }
        }

        private void ControlDynamicSettle()
        {
            ThreadManager.DoRepeatedly(DoSettleTimer_Elapsed, 10000);
        }

        private void DoSettleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                List<tblSettleTrans> SettleList = GetSettleList();
                if (SettleList != null && SettleList.Count > 0)
                {
                    ((Timer)sender).Stop();

                    foreach (tblSettleTrans item in SettleList)
                    {
                        SwitchBusiness.Settle(item);
                    }

                    ((Timer)sender).Start();
                }
            }
            catch (Exception EX)
            {
                (sender as Timer).Start();
                KioskLogger.Instance.LogMessage(EX, "");
            }
        }

        private List<tblReversalTrans> GetReversalList()
        {
            List<tblReversalTrans> list = null;
            ParametersDataAccess p = null;
            p = ParametersDataAccess.Instance;

            if (p != null)
            {
                //if (this.PreviousState == States.ConnectingState)
                //    list = p.ReversalListBeforConnect;
                //else
                //    list = p.ReversalList;

                list = p.ReversalList;
                if (list == null)
                    return new List<tblReversalTrans>();
                else
                    return list;
            }
            else
                return null;
        }

        private List<tblSettleReversTrans> GetReversalList93()
        {
            List<tblSettleReversTrans> list = null;
            ParametersDataAccess p = null;
            p = ParametersDataAccess.Instance;

            if (p != null)
            {
                list = p.ReversalSettleList93;
                if (list == null)
                    return new List<tblSettleReversTrans>();
                else
                    return list;
            }
            else
                return null;
        }

        private List<tblSettleTrans> GetSettleList()
        {
            List<tblSettleTrans> list = null;

            list = ParametersDataAccess.Instance.SettleList;
            if (list == null)
                return new List<tblSettleTrans>();
            else
                return list;
        }

        /// <summary>
        /// انتظار برای انجام تراکنش برگشت قبلی
        /// </summary>
        /// <returns></returns>
        public bool WaitingForReversal()
        {
            // در سناریوی پیاده سازی شده همواره یک ریورس و یا یک ستل باید در گلوی سیستم بماند بیشتر از یک رکورد یعنی به هر دلیلی رکورد نتوانسته پاک شود پس همیشه آخرین رکورد مد نظر است

           
            KioskLogger.Instance.LogChangeStates("Check For Reversal");

            List<tblSettleReversTrans> resList = GetReversalList93();
            while (resList.Count  > 0)
            {
                //ترد دیگری مشغول است
                if (resList.Last().IsInTry)
                    System.Threading.Thread.Sleep(2000);
                else
                    break;

                resList = GetReversalList93();
            }

            return true;
        }

        /// <summary>
        /// انتظار برای انجام تراکنش پرداخت قبلی
        /// </summary>
        /// <returns></returns>
        public bool WaitingForSettle()
        {
            
            KioskLogger.Instance.LogChangeStates("Check for Settle");

            List<tblSettleTrans> resList = GetSettleList();
            while (resList.Count > 0)
            {
                
                //ترد دیگری مشغول است
                if (resList.Last().IsInTry)
                    System.Threading.Thread.Sleep(2000);
                else
                    break;

                resList = GetSettleList();
            }

            return true;
        }



        #endregion CustomPrivateMethod


        #region CustomPublicMethod

        public void Reversal()
        {
            try
            {
                if (ReversalParameters.ReversalObject != null)
                {
                    SwitchBusiness.Reversal(ReversalParameters.ReversalObject);
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX.Message);
            }
        }

        public void Settle()
        {
            try
            {
                
                if (SettleReplyParameters.LastSettleObject != null)
                    SwitchBusiness.Settle(SettleReplyParameters.LastSettleObject);
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX.Message);
            }
        }

        public void SettleReversal93(int serviceType)
        {
            try
            {

                if (ReversalParameters.ReversalObject != null)
                {
                    SwitchBusiness.SettleReversal93(ReversalParameters.ReversalObject, (Enums.SpecialServiceType)serviceType);
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX.Message);
            }
        }

        public Int64 GetBillPayment(string BillId, string PayId, out string ErrorMessage)
        {
            return Utility.UtilMethods.GetBillPayment(BillId, PayId, out  ErrorMessage);
        }

        public string GetBillType(string BillId, out string ErrorMessage)
        {
            return Utility.UtilMethods.GetBillType(BillId, out  ErrorMessage);
        }

        public void AddToDictionary(string KeyName, object val)
        {

            if (PropertyDic.ContainsKey(KeyName))
                PropertyDic.Remove(KeyName);
            PropertyDic.Add(KeyName, val);
        }

        public void StopEppTimer()
        {
            EppService.GetInstance().StopEppTimer();
        }

        #endregion CustomPublicMethod













       
    }
}

