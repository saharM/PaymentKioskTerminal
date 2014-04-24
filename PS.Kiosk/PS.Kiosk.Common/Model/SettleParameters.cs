using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class SettleParameters : BaseParameters
    {
        int _DeviceCodeP25;
        /// <summary>
        /// کد نوع دستگاه
        /// </summary>
        public int DeviceCodeP25
        {
            get { return _DeviceCodeP25; }
            set { _DeviceCodeP25 = value; }
        }

        string _TerminalSerialNumber53;
        /// <summary>
        /// شماره سریال ترمینال
        /// </summary>
        public string TerminalSerialNumber53
        {
            get { return _TerminalSerialNumber53; }
            set { _TerminalSerialNumber53 = value; }
        }

        
    }

    public class SettleReplyParameters : BaseReplyParameters
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
        /// Mac
        /// </summary>
        public string MACP64
        {
            get { return _MACP64; }
            set { _MACP64 = value; }
        }

        static FinancialReplyParameters _LastSettleObject;
        /// <summary>
        /// آخرین تراکنشی که بايد پرداخت شود
        /// </summary>
        public static FinancialReplyParameters LastSettleObject
        {
            get { return _LastSettleObject; }
            set { _LastSettleObject = value; }
        }
    }
}
