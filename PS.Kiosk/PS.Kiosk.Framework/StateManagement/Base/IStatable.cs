using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PS.Kiosk.Framework
{
    public interface IStatable : INotifyPropertyChanged
    {
        #region Property

        Dictionary<object, object> PropertyDic {get;set;}
       

        #endregion Property

        event TriggerEventHandler OnConnected;
        event TriggerEventHandler OnError;
        event TriggerEventHandler OnReturn;
        event TriggerEventHandler OnReceivedCard;
        event TriggerEventHandler OnFinishingSession;
        event TriggerEventHandler OnAchievedPin;
        event TriggerEventHandler OnGetBalanceInquiryAction;
        event TriggerEventHandler OnExitAction;
        event TriggerEventHandler OnShowChargeType;
        event TriggerEventHandler OnShowIrancellChargeType;
        event TriggerEventHandler OnGetCharge;
        event TriggerEventHandler OnShowHamrahAvalChargeType;
        //event TriggerEventHandler OnGetHamrahAvalCharge;
        event TriggerEventHandler OnShowTaliaChargeType;
        //event TriggerEventHandler OnGetTaliaCharge;
        event TriggerEventHandler OnShowRightelChargeType;
        event TriggerEventHandler OnRestart;
        event TriggerEventHandler OnShowWaiting;
        event TriggerEventHandler OnShowBusy;
        event TriggerEventHandler OnPrinting;
        event TriggerEventHandler OnShowPayBill;
        event TriggerEventHandler OnPayBill;
        event TriggerEventHandler OnShowJiring;
        event TriggerEventHandler OnShowIrancellServices;
        event TriggerEventHandler OnShowIrancellServicePayment;
        event TriggerEventHandler OnPaySpeciallServices;
        event TriggerEventHandler OnShowMobinetPayment;
        event TriggerEventHandler OnShowHamrahAvalServices;
        event TriggerEventHandler OnShowHamrahAvalTopUp;
        event TriggerEventHandler OnShowHamarahAvalBill;
        event TriggerEventHandler OnShowHamrahAvalBillInfo;

        

        void Connecting();
        void Error(string ErrorMessage);
        void Error(Exception ErrorEx);
        void Return();
        void ErrorMsgShow();
        void WaitForCard();
        void ReceivedCard();
        void GetPin();
        void GetNumber();
        void FinishingSession();
        void FinishSession();
        void AchievedPin();
        void StopEppTimer();
        void GetBalanceInquiryAction();
        void GettingBalanceInquiryAction();
        void Exit();
        void ShowChargeType();
        void ShowIrancellChargeType();

        void GetChargeAction();
        void GettingChargeAction();

        void ShowHamrahAvalChargeType();
        void ShowTaliaChargeType();
        void ShowRightelChargeType();       
        bool CheckPrinterStatus();
        void ShowWaiting(States NextState , params object[] Parameters);
        void ShowBusy(States NextState);
        void Restart();
        bool WaitingForReversal();
        bool WaitingForSettle();
        void Printing();
        void Reversal();
        void Settle();
        void SettleReversal93(int ServiceType);
        void PayBill();
        void ShowPayBill();
        void PayBillAction();
        Int64 GetBillPayment(string BillId, string PayId, out string ErrorMsg);
        string GetBillType(string BillId ,out string ErrorMsg);
        void ShowJiring();
        void ShowIrancellServices();
        void ShowIrancellServicePayment();
        void PaySpecialServices();
        void PaySpecialServiceAction();
        void ShowHamarahAvalBill();
        void ShowHamrahAvalBillInfo();
        void GetSpecialServiceBillInfo();
        void ShowMobinetPayment();
        void ShowHamrahAvalServices();
        void ShowHamrahAvalTopUp();
        void AddToDictionary(string KeyName, object val);
        void DoReversal(bool IsInBackground = false);
        void DoSettle(bool IsInBackground = false);
        void DoRestart();

        States CurrentState { get; set; }
        States PreviousState { get; set; }
        States NextState { get; set; }
        States ReturnsState { get; set; }

        string ErrorMsg { get; set; }
        string Message { get; set; }
        string Message2 { get; set; }
        string Message3 { get; set; }
        string BindedPropertyName { get; set; }
        bool EnableControl { get; set; }
        bool ShouldDetectForCardCapturing{ get; set; }
        bool ShouldCaptureCard { get; set; }

        void ClearStateProperties();
    }
}
