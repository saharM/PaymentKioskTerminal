using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Framework.StateManagerBase;
using System.Reflection;

namespace PS.Kiosk.Framework
{
    public enum TriggerEvents
    {
        OnConnected,
        OnError,
        OnReturn,
        OnReceivedCard,
        OnFinishingSession,
        OnAchievedPin,
        OnGetBalanceInquiryAction,
        OnExitAction,
        /// <summary>
        /// فرم انتخاب شارژ
        /// </summary>
        OnShowChargeType,
        /// <summary>
        /// انتخاب نوع شارژ ایرانسل
        /// </summary>
        OnShowIrancellChargeType,
        /// <summary>
        /// خرید شارژ 
        /// </summary>
        OnGetCharge,
        /// <summary>
        /// انتخاب نوع شارژ همراه اول
        /// </summary>
        OnShowHamrahAvalChargeType,
        /// <summary>
        /// خرید شارژ همراه اول
        /// </summary>
        //OnGetHamrahAvalCharge,
        /// <summary>
        /// انتخاب نوع شارژ تالیا
        /// </summary>
        OnShowTaliaChargeType,
        /// <summary>
        /// خرید شارژ تالیا
        /// </summary>
        //OnGetTaliaCharge,
        OnShowRightelChargeType,
        /// <summary>
        /// نمايش صفحه انتظار
        /// </summary>
        OnShowWaiting,
        /// <summary>
        /// جهت لاگ آن مجدد
        /// </summary>
        OnRestart,
        /// <summary>
        /// نمایش مشغوليت سیستم
        /// </summary>
        OnShowBusy,
        /// <summary>
        /// چاپ
        /// </summary>
        OnPrinting,
        /// <summary>
        /// نمايش فرم پرداخت قبض
        /// </summary>
        OnShowPayBill,
        /// <summary>
        /// پرداخت قبض
        /// </summary>
        OnPayBill,
        /// <summary>
        /// نمایش فرم شارژ جيرينگ
        /// </summary>
        OnShowJiring,
       
        /// <summary>
        /// نمايش فرم سرويسهای ويژه ايرانسل
        /// </summary>
        OnShowIrancellServices,
        /// <summary>
        /// نمایش فرم ورودی اطلاعات کاربر
        /// </summary>
        OnShowIrancellServicePayment,
        /// <summary>
        /// پرداخت سرویس های ايرانسل
        /// </summary>
        OnPaySpeciallServices,
        /// <summary>
        /// نمایش فرم مبین نت
        /// </summary>
        OnShowMobinetPayment,
        /// <summary>
        /// نمایش فرم سرویس های همراه اول
        /// </summary>
        OnShowHamrahAvalServices,
        /// <summary>
        /// نمایش فرم همراه اول - شارژ مستقیم
        /// </summary>
        OnShowHamrahAvalTopUp,
        /// <summary>
        /// نمایش فرم پرداخت قبض پایان دوره و میان دوره همراه اول
        /// </summary>
        OnShowHamrahAvalBill,
        /// <summary>
        /// نمایش اطلاعات قبض
        /// </summary>
        OnShowHamrahAvalBillInfo

        
    }
    abstract class StateBase : State<StateContext, States>
    {
        public override void TriggerHandler(TriggerEventArgs e)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(e.RaisedEvent.ToString());
            theMethod.Invoke(this, new object[] {});
        }

        public virtual void OnConnected() { }
        
        public virtual void OnError() 
        {
            Context.Next = States.ErrorState;
        }
        public virtual void OnReturn()
        {
            if (Context.Statable.ReturnsState != States.EmptyState)
                Context.Next = Context.Statable.ReturnsState;
            else
                Context.Next = States.FinishingSessionState;
        }
        public virtual void OnReceivedCard() { }
        public virtual void OnFinishingSession() 
        {
            Context.Next = States.FinishingSessionState;
        }
        public virtual void OnAchievedPin() { }
        public virtual void OnGetBalanceInquiryAction() { }
        public virtual void OnExitAction() { Context.Next = States.FinishingSessionState; }
        public virtual void OnShowChargeType() { }
        public virtual void OnShowIrancellChargeType() { }
        public virtual void OnGetCharge() {  }
        public virtual void OnShowHamrahAvalChargeType() { }
        //public virtual void OnGetHamrahAvalCharge() { }
        public virtual void OnShowTaliaChargeType() { }
        //public virtual void OnGetTaliaCharge() { }
        public virtual void OnShowRightelChargeType() { }
        public virtual void OnShowWaiting() { }
        public virtual void OnRestart() { }
        public virtual void OnShowBusy() { }
        public virtual void OnPrinting() { }
        public virtual void OnShowPayBill() { }
        public virtual void OnPayBill() { }
        public virtual void OnShowJiring() { }
        public virtual void OnShowIrancellServices() { }
        public virtual void OnShowIrancellServicePayment() { }
        public virtual void OnPaySpeciallServices() { }
        public virtual void OnShowMobinetPayment() { }
        public virtual void OnShowHamrahAvalServices() { }
        public virtual void OnShowHamrahAvalTopUp() { }
        public virtual void OnShowHamrahAvalBill(){}
        public virtual void OnShowHamrahAvalBillInfo(){}


    }

}
