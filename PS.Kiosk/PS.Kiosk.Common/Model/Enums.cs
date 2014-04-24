using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class Enums
    {
        public enum MsgType
        {

            //Version 78
            Financial = 0200,
            FinancialReply=0210,
            Reverse = 0420,
            ReverseReply=0430,
            SignOn = 1804,
            SignOnReply = 1814,
            Settle = 500,
            SettleReply = 510,

            //version 93
            SpecialService = 1200,
             SpecialServiceReply = 1210,
            BillInfo = 1100,
            Reverse93 = 1420,
            ReverseReply93 = 1430,
            Settle93 = 1220,
            SettleReply93 = 1230
            
        }

        public enum SwitchMsgFormat
        {
            Shetab87 = 87,
            Shetab93 = 93
        }

        public enum ProcessCode
        {
            Remain = 310000,
            BillPay = 170000,
            Charg = 190000,
            Purchace = 000000,
            State = 370000,
            Transfer = 400000,
            SignOn = 900000,
            SignOnReply = 910000,
            Settle = 930000,
            SpecialService = 970000 ,
            SpecialServiceBillPay = 350000,
            BillPay93 = 500000
        }

        public enum Transactions
        {
            REMAIN = 1, BILLPAY = 4, Settele = 10, CUTOFF = 8, ECHOTEST1 = 16,
            ECHOTEST2 = 17, PINKEY = 18, MACKEY = 19, REVERSAL = 15, PAYMENT = 22 , Signon = 26 , Charge = 27 , Jiring = 28 , IrancellPayBill = 29
        }

        /// <summary>
        /// نوع حساب
        /// </summary>
        public enum AccountType
        {
            None = 0,
            Jari = 1,
            Pasandaz = 2
        }

        /// <summary>
        /// ماهیت حساب
        /// </summary>
        public enum AccountNature
        {
            /// <summary>
            /// بدهکار
            /// </summary>
            Debit = 1,
            /// <summary>
            /// بستانکار
            /// </summary>
            Credit= 2
        }


        /// <summary>
        /// نوع وجه
        /// </summary>
        public enum AmountType
        {
            None = 0,
            /// <summary>
            /// مانده واقعی
            /// </summary>
            Ledger = 1,
            /// <summary>
            /// مانده قابل دسترس
            /// </summary>
            Available = 2
            
        }

        public enum ChargeType
        {

            Irancell = 9935,
            HamrahAval = 9912,
            Talia = 9932,
            Rightel = 9920

        }

        public enum ServiceType
        {
            // آب
            Water = 1,
            // برق
            Electricity = 2,
            // گاز
            Gas = 3,
            // تلفن ثابت
            LandLine = 4,
            // تلفن همراه
            MobileLine = 5,
            // شهرداری
            Municipality = 6,
        };

        public enum SpecialServiceType
        {

            HamrahAvalMidTermInfo =124,
            HamrahAvalFinalTermInfo = 127,
            Financial = 200,
            JiringCharge = 214,
            JiringPurchace = 215,
            HamrahAvalFinalTermBill = 216,
            IrancellBill = 218,
            IrancellWimax = 219,
            HamrahAvalMidTermBill = 220,
            IrancellTopUp = 233,
            HamrahAvalTopUp = 234,
            Mobinnet = 237,
            Reversal = 400
            
        }
    }
}
