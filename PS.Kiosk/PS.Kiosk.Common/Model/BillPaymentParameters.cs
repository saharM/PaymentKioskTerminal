using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class BillPaymentParameters : FinancialParameters
    {
        string _BillID;
        /// <summary>
        /// شناسه قبض
        /// </summary>
        public string BillID
        {
            get { return _BillID; }
            set { _BillID = value; }
        }

        string _PayID;
        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        public string PayID
        {
            get { return _PayID; }
            set { _PayID = value; }
        }


        string _ExpireDate;
        public string ExpireDate
        {
            get { return _ExpireDate; } 
            set{_ExpireDate = value;} 
        }

        private string _ServiceUseCode;
        public string ServiceUseCode
        {
            get
            {
                return _ServiceUseCode;
            }
            set
            {
                _ServiceUseCode = value;
            }
        }

        Enums.SpecialServiceType _ServiceType;
        public Enums.SpecialServiceType ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }
    }
}
