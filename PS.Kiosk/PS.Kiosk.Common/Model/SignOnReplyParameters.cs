using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class SignOnReplyParameters :BaseParameters // az BaseReply nabayad be ers beravad chon filed 48ash fargh mikonad
    {
        DateTime _DateTimeP7;
        /// <summary>
        /// تاریخ و زمان
        /// </summary>
        public DateTime DateTimeP7
        {
            get { return _DateTimeP7; }
            set { _DateTimeP7 = value; }
        }

        string _ReplyCodeP39;
        /// <summary>
        /// کد پاسخ به درخواست انجام تراکنش
        /// </summary>
        public string ReplyCodeP39
        {
            get { return _ReplyCodeP39; }
            set { _ReplyCodeP39 = value; }
        }

        string _MACP64;
        /// <summary>
        /// فیلد 64ام
        /// </summary>
        public string MACP64
        {
            get { return _MACP64; }
            set { _MACP64 = value; }
        }

        bool _IsAuthenticated;
        /// <summary>
        /// موفق بوده login 
        /// </summary>
        public bool IsAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        //پاسخ مربوط به فیلد 48
        //sigon on 93i ast va ba formate persian switch , namgozariha bar asase documente persianswitch ast
        static string _PosSN;
        /// <summary>
        /// کد پايانه(Terminal Number)
        /// </summary>
        public static string PosSN
        {
            get { return _PosSN; }
            set { _PosSN = value; }
        }

        static string _VahedTejariSN;
        /// <summary>
        /// کد پذيرنده
        /// </summary>
        public static string VahedTejariSN
        {
            get { return _VahedTejariSN; }
            set { _VahedTejariSN = value; }
        }

        static string _VahedeTejariDS;
        /// <summary>
        /// نام واحد تجاری
        /// </summary>
        public static string VahedeTejariDS
        {
            get { return _VahedeTejariDS; }
            set { _VahedeTejariDS = value; }
        }

        static string _SwitchSigonOnDateTime;
        /// <summary>
        /// تاريخ و زمان سوييچ
        /// YYYYMMDDHHMMSS
        /// </summary>
        public static string SwitchSigonOnDateTime
        {
            get { return _SwitchSigonOnDateTime; }
            set { _SwitchSigonOnDateTime = value; }
        }

        static string _VahedeTejariTel;
        /// <summary>
        /// تلفن واحد تجاری
        /// </summary>
        public static string VahedeTejariTel
        {
            get { return _VahedeTejariTel; }
            set { _VahedeTejariTel = value; }
        }

        static string _Tran_TelNo;
        /// <summary>
        /// شماره تماس انجام تراکنش
        /// </summary>
        public static string Tran_TelNo
        {
            get { return _Tran_TelNo; }
            set { _Tran_TelNo = value; }
        }

        static string _Tran_TelNo1;
        /// <summary>
        /// شماره تماس انجام تراکنش اوليه
        /// </summary>
        public static string Tran_TelNo1
        {
            get { return _Tran_TelNo1; }
            set { _Tran_TelNo1 = value; }
        }

        static string _TMS_TelNo;
        /// <summary>
        /// شماره تماس اتصال دریافت فایل
        /// </summary>
        public static string TMS_TelNo
        {
            get { return _TMS_TelNo; }
            set { _TMS_TelNo = value; }
        }


        static string _TMS_TelNo1;
        /// <summary>
        /// شماره تماس TMS(شماره اوليه)
        /// </summary>
        public static string TMS_TelNo1
        {
            get { return _TMS_TelNo1; }
            set { _TMS_TelNo1 = value; }
        }

        static string _Tran_IP;
        /// <summary>
        /// آدرس مربوط به سوييچ شرکت مبتنی بر اینترنت
        /// </summary>
        public static string Tran_IP
        {
            get { return _Tran_IP; }
            set { _Tran_IP = value; }
        }

        static string _TMS_IP;
        /// <summary>
        /// آدرس مرتبط با TMS
        /// </summary>
        public static string TMS_IP
        {
            get { return _TMS_IP; }
            set { _TMS_IP = value; }
        }


        static string _ApplicationVer;
        /// <summary>
        ///  نسخه ترمینال در مرکز که در صورت مغایرت لازم است بروزرسانی شود
        /// </summary>
        public static string ApplicationVer
        {
            get { return _ApplicationVer; }
            set { _ApplicationVer = value; }
        }

        static string _Account;
        /// <summary>
        /// حساب تصویه پذیرنده
        /// </summary>
        public static string Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        static string _AppID;
        /// <summary>
        /// کد بسته نرم افزاری برای بروزرسانی
        /// </summary>
        public static string AppID
        {
            get { return _AppID; }
            set { _AppID = value; }
        }

        static string _MACKey;
        /// <summary>
        /// کلید MAC
        /// </summary>
        public static string MACKey
        {
            get { return _MACKey; }
            set { _MACKey = value; }
        }

        static string _PinKey;
        /// <summary>
        /// کلید Pin
        /// </summary>
        public static string PinKey
        {
            get { return _PinKey; }
            set { _PinKey = value; }
        }

        static string _LOYALTYCARDTRACK2;
        /// <summary>
        /// کارت وفاداری
        /// </summary>
        public static string LOYALTYCARDTRACK2
        {
            get { return _LOYALTYCARDTRACK2; }
            set { _LOYALTYCARDTRACK2 = value; }
        }
    }
}
