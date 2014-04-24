using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace PS.Kiosk.Framework
{
    //توجه کارهای مربوط به اغلب وضعيت ها در وضعيت انتظار انجام می شود و وضعیت مورد نظر در غالب وضعيت بعدی برای وضعيت انتظار تلقی می شود

    public enum ChargeType
    {
        None = 1,
        Irancell1000 = 10000,
        Irancell2000 = 20000,
        Irancell5000 = 50000,
        Irancell20000 = 200000,
        HamrahAval1000 = 10000,
        HamrahAval2000 = 20000,
        HamrahAval5000 = 50000,
        HamrahAval10000 = 100000,
        HamrahAval20000 = 200000,
        HamrahAval10000Recharge = 100000,
        Talia2000 = 20000,
        Talia5000 = 50000,
        Talia10000 = 100000
    }

    public enum States
    {
        ConnectingState, ErrorState, WaitingForCardState, GettingPinState, FinishingSessionState, MainMenuState,ReturnState,

        /// <summary>
        /// قبل از ورود به هر صفحه ای که با ای پی پی کار می کند- وضعيت مشغول سیستم
        /// </summary>
        BusyState,
        /// <summary>
        /// جهت لاگ آن مجدد در پایان روز
        /// </summary>
        RestartState,

        /// <summary>
        /// انتظار
        /// </summary>
        WaitingState,

        /// <summary>
        /// دریافت موجودی
        /// </summary>
        GetBalanceInquiryActionState,

        EmptyState,

        /// <summary>
        /// نمایش فرم انتخاب نوع شارژ
        /// </summary>
        ShowChargeTypeState,

        /// <summary>
        /// نمایش فرم انتخاب شارژ ایرانسل
        /// </summary>
        ShowIrancellChargeTypeState,

        /// <summary>
        /// خرید شارژ ایرانسل 
        /// </summary>
        GetChargeState,

        /// <summary>
        /// نمایش فرم انتخاب شارژ همراه اول
        /// </summary>
        ShowHamrahAvalChargeTypeState,

        /// <summary>
        /// خرید شارژ همراه اول 
        /// </summary>
        //GetHamrahavalChargeState,

        /// <summary>
        /// نمایش فرم انتخاب شارژ تالیا
        /// </summary>
        ShowTaliaChargeTypeState,

        /// <summary>
        /// خرید شارژ تالیا 
        /// </summary>
        //GetTaliaChargeState,

        /// <summary>
        /// نمایش کارت شارژهای رایتل
        /// </summary>
        ShowRightelChargeTypeState,

        /// <summary>
        /// وضعيت چاپ
        /// </summary>
        PrintState,

        /// <summary>
        /// نمایش فرم پرداخت قبض
        /// </summary>
        ShowPayBillState,

        /// <summary>
        /// پرداخت قبض
        /// </summary>
        PayBillState,

        /// <summary>
        /// نمایش فرم جيرينگ
        /// </summary>
        ShowJiringState,
        
        /// <summary>
        /// نمایش سرويسهای ایرانسل
        /// </summary>
        ShowIrancellServicesState ,

        /// <summary>
        /// نمايش فرم دریافت ورودی از کاربر برای سرویس های ایرانسل
        /// </summary>
        ShowIrancellServicePaymentState,

        /// <summary>
        /// پرداخت سرويس های ايرانسل
        /// </summary>
        PaySpeciallServicesState,

        /// <summary>
        /// نمایش فرم مبین نت
        /// </summary>
        ShowMobinetPaymentState,

        /// <summary>
        /// نمایش فرم سرویس های همراه اول
        /// </summary>
        ShowHamrahAvalServicesState,

        /// <summary>
        /// نمایش فرم شارژ مستقیم همراه اول
        /// </summary>
        ShowHamrahAvalTopUpState,

        /// <summary>
        /// فرم قبض پایان دوره - میان دوره همراه اول
        /// </summary>
        ShowHamrahAvalBillState,

        /// <summary>
        /// نمایش اطلاعات قبض همراه اول
        /// </summary>
        ShowHamrahAvalBillInfoState
    }

    class ConnectingState : StateBase
    {
        public override void EntryAction()
        {
            
            Context.Statable.ShowWaiting(States.WaitingForCardState, null);
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }
    }

    class ReturnState : StateBase
    {
        
    }

    class ErrorState : StateBase
    {
        public override void EntryAction()
        {
            Context.Statable.ErrorMsgShow();
            Context.Timeout = TimeSpan.FromSeconds(5);
        }

        public override void TimeoutHandler()
        {
            //اگر وضعِت قبلی کانکت بود به جای رفتن به وضعيت جدید دوباره سعی می کند
            if (Context.Statable.PreviousState == States.WaitingState && Context.Statable.NextState == States.WaitingForCardState)
                Context.Next = States.ConnectingState;
            else
                if (Context.Statable.PreviousState == States.GetChargeState)
                    Context.Next = States.ShowChargeTypeState;
                else
                    Context.Next = States.FinishingSessionState;
        }
    }

    class WaitingForCardState : StateBase
    {
        public override void EntryAction()
        {
            Context.Statable.WaitForCard();
            
        }

        public override void OnReceivedCard()
        {
            Context.Statable.ShowBusy(States.GettingPinState);
           
        }

        public override void OnShowBusy()
        {
            Context.Next = States.BusyState;
        }

        public override void OnRestart()
        {
            Context.Next = States.RestartState;
        }
    }

    class GettingPinState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(30);
            Context.Statable.GetPin();
        }

        public override void OnAchievedPin()
        {
            Context.Next = States.MainMenuState;
        }

        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.FinishingSessionState;
            
        }
        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.ExitAction();
        }
       
    }

    class FinishingSessionState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMilliseconds(1000);
            Context.Statable.FinishingSession();
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.WaitingForCardState;
        }
    }

    class MainMenuState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(30);
        }

        public override void OnGetBalanceInquiryAction()
        {
            Context.Next = States.GetBalanceInquiryActionState;
        }

        public override void OnShowChargeType()
        {
            Context.Next = States.ShowChargeTypeState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.FinishingSessionState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        public override void OnShowPayBill()
        {
            
            Context.Next = States.ShowPayBillState;
        }

        public override void OnShowHamrahAvalServices()
        {
            Context.Next = States.ShowHamrahAvalServicesState;
        }

        public override void OnShowIrancellServices()
        {
            Context.Next = States.ShowIrancellServicesState;
        }

        public override void OnShowMobinetPayment()
        {
            
            Context.Next = States.ShowMobinetPaymentState;
        }

        public override void OnShowBusy()
        {
            Context.Next = States.BusyState;
        }
    }

    class BusyState : StateBase
    {
        public override void EntryAction()
        {
            //قبل از انجام هر تراکنش باید چک شود که تراکنش برگشت و یا پرداختی وجود نداشته باشد
            Context.Statable.WaitingForReversal();
            Context.Statable.WaitingForSettle();

            if (Context.Statable.NextState != States.EmptyState)
                Context.Next = Context.Statable.NextState;
            else
                Context.Next = States.FinishingSessionState;
        }

        public override void OnShowJiring()
        {
            Context.Next = States.ShowJiringState;
        }

        public override void OnShowHamrahAvalTopUp()
        {
            Context.Next = States.ShowHamrahAvalTopUpState;
        }

        public override void OnShowPayBill()
        {
            Context.Next = States.ShowPayBillState;
        }

        public override void OnShowIrancellServicePayment()
        {
            Context.Next = States.ShowIrancellServicePaymentState;
        }

        public override void OnShowMobinetPayment()
        {
            Context.Next = States.ShowMobinetPaymentState;
        }

        public override void OnShowHamrahAvalBill()
        {
            Context.Next = States.ShowHamrahAvalBillState;
        }

        
    }

    class RestartState : StateBase
    {
 
    }

    class WaitingState : StateBase
    {
        public override void EntryAction()
        {
            //Context.Timeout = TimeSpan.FromSeconds(30);

            if (NetworkInterface.GetIsNetworkAvailable() == false)
                Context.Statable.Error("Network Is Not Available");
            else
                if(!Context.Statable.CheckPrinterStatus())
                    Context.Statable.Error("Printer is UnAvailable2");
            else
            {
                if (Context.Statable.PreviousState == States.ConnectingState)
                {
                    //قبل از ریست شدن کلیدها در زمان ساین آن اگر تراکنش برگشتی مانده باید انجام شود
                    Context.Statable.DoReversal();
                    //Context.Statable.DoSettle();
                }
                else
                {
                    //قبل از انجام هر تراکنش باید چک شود که تراکنش برگشت و یا پرداختی وجود نداشته باشد
                    //اگر ترد دیگری مشغول باشد صبر می کند
                    Context.Statable.WaitingForReversal();
                    //Context.Statable.WaitingForSettle();

                    //اگر ترد دیگری مشغول نباشد تراکنش برگشت  ویا پرداخت را انجام می دهد
                    Context.Statable.DoReversal();
                    //Context.Statable.DoSettle();

                }

                if (Context.Statable.NextState == States.WaitingForCardState)
                    Context.Statable.Connecting();

                //دريافت موجودی
                if (Context.Statable.NextState == States.GetBalanceInquiryActionState)
                    Context.Statable.GettingBalanceInquiryAction();

                //خرید کارت شارژ 
                if (Context.Statable.NextState == States.GetChargeState)
                    Context.Statable.GettingChargeAction();

                ////خرید کارت شارژ همراه اول
                //if (Context.Statable.NextState == States.GetHamrahavalChargeState)
                //    Context.Statable.GettingHamrahAvalChargeAction();

                ////خرید کارت شارژ تاليا
                //if (Context.Statable.NextState == States.GetTaliaChargeState)
                //    Context.Statable.GettingTaliaChargeAction();

                //پرداخت قبض
                if (Context.Statable.NextState == States.PayBillState)
                    Context.Statable.PayBillAction();

                //سرویسهای ويژه
                if (Context.Statable.NextState == States.PaySpeciallServicesState)
                    Context.Statable.PaySpecialServiceAction();

                //قبض دو مرحله ای همراه اول
                if (Context.Statable.NextState == States.ShowHamrahAvalBillInfoState)
                    Context.Statable.GetSpecialServiceBillInfo();
            }
        }

       
        public override void OnGetBalanceInquiryAction()
        {
            Context.Next = States.GetBalanceInquiryActionState;
        }

        public override void OnGetCharge()
        {
            Context.Next = States.GetChargeState;
        }

        //public override void OnGetHamrahAvalCharge()
        //{
        //    Context.Next = States.GetHamrahavalChargeState;
        //}

        //public override void OnGetTaliaCharge()
        //{
        //    Context.Next = States.GetTaliaChargeState;
        //}

        public override void OnPrinting()
        {
            Context.Next = States.PrintState;
        }

        public override void OnPayBill()
        {
            Context.Next = States.PayBillState;
        }

        public override void OnPaySpeciallServices()
        {
            Context.Next = States.PaySpeciallServicesState;
        }

        public override void OnConnected()
        {
            Context.Next = States.WaitingForCardState;
        }

        public override void OnShowHamrahAvalBillInfo()
        {
            Context.Next = States.ShowHamrahAvalBillInfoState;
        }

        public override void OnFinishingSession()
        {
            Context.Next = States.FinishingSessionState;
        }
    }

    class PrintState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
        }
        public override void TimeoutHandler()
        {
            Context.Next = States.FinishingSessionState;
        }
    }

    class GetBalanceInquiryActionState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(30);
            Context.Statable.ReturnsState = States.MainMenuState;

            

            //Context.Statable.GettingBalanceInquiryAction(); 
            Context.Statable.Message = Convert.ToString(Context.Statable.PropertyDic["CardBalance"]);
            Context.Statable.PropertyDic.Remove("CardBalance");
        }

        public override void OnPrinting()
        {
            Context.Next = States.PrintState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }
       
    }

    class ShowChargeTypeState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(20);
            Context.Statable.ReturnsState = States.MainMenuState;
           
        }

        public override void OnShowIrancellChargeType()
        {
            Context.Next = States.ShowIrancellChargeTypeState;
        }

        public override void OnShowHamrahAvalChargeType()
        {
            Context.Next = States.ShowHamrahAvalChargeTypeState;
        }

        public override void OnShowTaliaChargeType()
        {
            Context.Next = States.ShowTaliaChargeTypeState;
        }

        public override void OnShowRightelChargeType()
        {
            Context.Next = States.ShowRightelChargeTypeState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }
    }

    class ShowIrancellChargeTypeState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(20);
            Context.Statable.ReturnsState = States.ShowChargeTypeState;
            
        }
       

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }
    }

    class GetChargeState : StateBase 
    {
        public override void EntryAction()
        {
            Context.Next = States.PrintState;
        }
       
    }

    class ShowHamrahAvalChargeTypeState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(20);
            Context.Statable.ReturnsState = States.ShowChargeTypeState;
            
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        
        
    }

    //class GetHamrahavalChargeState :StateBase
    //{
    //    public override void EntryAction()
    //    {
    //        Context.Next = States.PrintState;
    //    }
    //}

    class ShowTaliaChargeTypeState : StateBase
    {
        public override void EntryAction()
        {
            Context.Statable.ReturnsState = States.ShowChargeTypeState;
            Context.Timeout = TimeSpan.FromSeconds(20);
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        
    }

    //class GetTaliaChargeState : StateBase
    //{
    //    public override void EntryAction()
    //    {
    //        Context.Next = States.PrintState;
    //    }
    //}

    class ShowRightelChargeTypeState : StateBase
    {
        public override void EntryAction()
        {
            Context.Statable.ReturnsState = States.ShowChargeTypeState;
            Context.Timeout = TimeSpan.FromSeconds(20);
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }
    }

    class ShowPayBillState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
            Context.Statable.ReturnsState = States.MainMenuState;
            
            Context.Statable.GetNumber();
            
        }

        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }

        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }
    }

    class PayBillState : StateBase
    {
        public override void EntryAction()
        {
            Context.Next = States.PrintState;
        }
    }

    class ShowJiringState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
            Context.Statable.ReturnsState = States.ShowHamrahAvalServicesState;
            Context.Statable.GetNumber();
            
        }

        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }

       
        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }
    }

    class ShowIrancellServicesState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(20);
            Context.Statable.ReturnsState = States.MainMenuState;
            
        }

        public override void OnShowIrancellServicePayment()
        {
            Context.Next = States.ShowIrancellServicePaymentState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }

        public override void OnShowBusy()
        {
            Context.Next = States.BusyState;
        }
    }

    class ShowIrancellServicePaymentState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
            Context.Statable.ReturnsState = States.ShowIrancellServicesState;
            Context.Statable.GetNumber();
            
            
        }
        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }
        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }
    }

    class PaySpeciallServicesState : StateBase
    {
        public override void EntryAction()
        {
            Context.Next = States.PrintState;
        }
    }

    class ShowMobinetPaymentState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
            Context.Statable.ReturnsState = States.MainMenuState;
            Context.Statable.GetNumber();
            

        }
        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.WaitingState;
        }

        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }
    }

    class ShowHamrahAvalServicesState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(20);
            Context.Statable.ReturnsState = States.MainMenuState;
            
        }

        public override void OnShowJiring()
        {
            Context.Next = States.ShowJiringState;
        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }

        public override void OnShowBusy()
        {
            Context.Next = States.BusyState;
        }
    }

    class ShowHamrahAvalTopUpState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(3);
            Context.Statable.ReturnsState = States.ShowHamrahAvalServicesState;
            Context.Statable.GetNumber();

        }

        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }


        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }

        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }
    }

    class ShowHamrahAvalBillState : StateBase
    {

        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromMinutes(1);
            Context.Statable.ReturnsState = States.ShowHamrahAvalServicesState;
            Context.Statable.GetNumber();


        }
        public override void TimeoutHandler()
        {
            Context.Statable.StopEppTimer();
            Context.Next = States.MainMenuState;
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;
        }
        public override void OnExitAction()
        {
            Context.Statable.StopEppTimer();
            base.OnExitAction();
        }

        public override void OnReturn()
        {
            Context.Statable.StopEppTimer();
            base.OnReturn();
        }
    }

    class ShowHamrahAvalBillInfoState : StateBase
    {
        public override void EntryAction()
        {
            Context.Timeout = TimeSpan.FromSeconds(30);
            Context.Statable.ReturnsState = States.ShowHamrahAvalServicesState;

            Context.Statable.Message = Convert.ToString(Context.Statable.PropertyDic["MobileNumber"]);
            

            Context.Statable.Message2 = Convert.ToString(Context.Statable.PropertyDic["FinalTermAmount"]);
            Context.Statable.PropertyDic.Remove("FinalTermAmount");

            Context.Statable.Message3 = Convert.ToString(Context.Statable.PropertyDic["MidTermAmount"]);
            Context.Statable.PropertyDic.Remove("MidTermAmount");
        }

        public override void OnShowWaiting()
        {
            Context.Next = States.WaitingState;

        }

        public override void TimeoutHandler()
        {
            Context.Next = States.MainMenuState;
        }
    }

}
