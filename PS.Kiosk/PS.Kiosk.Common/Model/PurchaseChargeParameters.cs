using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class PurchaseChargeParameters : FinancialParameters
    {
        private Int64 _ChargeAmount;
        /// <summary>
        /// مبلغ کل شارژ
        /// </summary>
        public Int64 ChargeAmount
        {
            get { return _ChargeAmount; }
            set { _ChargeAmount = value; }
        }

        private Enums.ChargeType _chargetype98;
        /// <summary>
        /// نوع شارژ
        /// </summary>
        public Enums.ChargeType Chargetype98
        {
            get { return _chargetype98; }
            set { _chargetype98 = value; }
        }

        int _ChargeCountP48;
        /// <summary>
        /// تعداد شارژ خواسته شده
        /// </summary>
        public int ChargeCountP48
        {
            get { return _ChargeCountP48; }
            set { _ChargeCountP48 = value; }
        }
    }
}
