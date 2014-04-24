using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class BillPaymentReplyParameters : FinancialReplyParameters
    {
        string _BillOrganization;
        /// <summary>
        /// سازمان قبض
        /// </summary>
        public string BillOrganization
        {
            get { return _BillOrganization; }
            set { _BillOrganization = value; }
        }

        string _BillId;
        /// <summary>
        /// شناسه قبض
        /// </summary>
        public string BillId
        {
            get { return _BillId; }
            set { _BillId = value; }
        }

        string _PayId;
        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        public string PayId
        {
            get { return _PayId; }
            set { _PayId = value; }
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
    }
}
