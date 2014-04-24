using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class BalanceInquiryReplyParameteres : FinancialReplyParameters
    {
        private Enums.AccountType _Accouttype;
        /// <summary>
        /// نوع حساب
        /// </summary>
        public Enums.AccountType Accouttype
        {
            get { return _Accouttype; }
            set { _Accouttype = value; }
        }

      

        private int _CurrencyType;
        /// <summary>
        /// نوع ارز
        /// </summary>
        public int CurrencyType
        {
            get { return _CurrencyType; }
            set { _CurrencyType = value; }
        }

        private Enums.AccountNature _Accountnature;
        /// <summary>
        /// ماهیت حساب
        /// </summary>
        public Enums.AccountNature Accountnature
        {
            get { return _Accountnature; }
            set { _Accountnature = value; }
        }

        Int64 _AvailableBalance;
        /// <summary>
        /// مانده قابل دسترس
        ///  </summary>
        public Int64 AvailableBalance
        {
            get { return _AvailableBalance; }
            set { _AvailableBalance = value; }
        }

        Int64 _LedgerBalance;
        /// <summary>
        /// مانده واقعی
        /// </summary>
        public Int64 LedgerBalance
        {
            get { return _LedgerBalance; }
            set { _LedgerBalance = value; }
        }

        
    }
}
