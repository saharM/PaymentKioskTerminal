using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class PurchaseChargeReplyParameters : FinancialReplyParameters
    {
        string _ChargeCount;
        /// <summary>
        /// تعداد شارژ
        /// </summary>
        public string ChargeCount
        {
            get { return _ChargeCount; }
            set { _ChargeCount = value; }
        }

        string _ChargeSerial;
        /// <summary>
        /// شماره سریال شارژ
        /// </summary>
        public string ChargeSerial
        {
            get { return _ChargeSerial; }
            set { _ChargeSerial = value; }
        }

        string _ChargePassword;
        /// <summary>
        /// رمز شارژ
        /// </summary>
        public string ChargePassword
        {
            get { return _ChargePassword; }
            set { _ChargePassword = value; }
        }

        string _ChargeAmount;
        /// <summary>
        /// مبلغ شارژ
        /// </summary>
        public string ChargeAmount
        {
            get { return _ChargeAmount; }
            set { _ChargeAmount = value; }
        }

        string _ChargeTypeP98;
        /// <summary>
        /// نوع شارژ
        /// </summary>
        public string ChargeTypeP98
        {
            get { return _ChargeTypeP98; }
            set { _ChargeTypeP98 = value; }
        }


    }
}
