using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class FinancialReplyParameters : BaseReplyParameters
    {
        string _CardNumber;
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string CardNumber
        {
            get { return _CardNumber; }
            set { _CardNumber = value; }
        }

        Int64 _TranAmountP4;
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public Int64 TranAmountP4
        {
            get { return _TranAmountP4; }
            set { _TranAmountP4 = value; }
        }


        string _SpecialMsg;
        /// <summary>
        /// پيام های خاص
        /// </summary>
        public string SpecialMsg
        {
            get { return _SpecialMsg; }
            set { _SpecialMsg = value; }
        }

        string _Wage;
        /// <summary>
        /// کارمزد
        /// </summary>
        public string Wage
        {
            get { return _Wage; }
            set { _Wage = value; }
        }

        int _TranCurrencyCodeP49;
        /// <summary>
        /// کد ارز تراکنش
        /// </summary>
        public int TranCurrencyCodeP49
        {
            get { return _TranCurrencyCodeP49; }
            set { _TranCurrencyCodeP49 = value; }
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
        string _MACP128;
        /// <summary>
        /// Mac
        /// </summary>
        public string MACP128
        {
            get { return _MACP128; }
            set { _MACP128 = value; }
        }
    }
}
