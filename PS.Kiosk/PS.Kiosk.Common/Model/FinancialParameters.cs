using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public  class FinancialParameters : BaseParameters
    {
        string _CardNumberP2;
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string CardNumberP2
        {
            get { return _CardNumberP2; }
            set { _CardNumberP2 = value; }
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

        int _DeviceCodeP25;
        /// <summary>
        /// کد نوع دستگاه
        /// </summary>
        public int DeviceCodeP25
        {
            get { return _DeviceCodeP25; }
            set { _DeviceCodeP25 = value; }
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

        byte[] _PinBlockP52;
        /// <summary>
        /// pinBlock
        /// </summary>
        public byte[] PinBlockP52
        {
            get { return _PinBlockP52; }
            set { _PinBlockP52 = value; }
        }

        string _TerminalSerialNumberP53;
        /// <summary>
        /// شماره سریال ترمینال
        /// </summary>
        public string TerminalSerialNumberP53
        {
            get { return _TerminalSerialNumberP53; }
            set { _TerminalSerialNumberP53 = value; }
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

        //***

       

        string _DestinationPAN;

        public string DestinationPAN
        {
            get { return _DestinationPAN; }
            set { _DestinationPAN = value; }
        }

    }
}
