using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class SpecialServiceParameters : FinancialParameters
    {
        Enums.SpecialServiceType _ServiceType;
        /// <summary>
        /// نوع سرویس ويژه 
        /// </summary>
        public Enums.SpecialServiceType ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }

        string _TerminalSerialNumberP48;
        /// <summary>
        /// شماره سریال ترمینال
        /// </summary>
        public string TerminalSerialNumberP48
        {
            get { return _TerminalSerialNumberP48; }
            set { _TerminalSerialNumberP48 = value; }
        }

        string _ExpireDate;
        /// <summary>
        /// زمان انقضای کارت
        /// </summary>
        public string ExpireDate
        {
            get { return _ExpireDate; }
            set { _ExpireDate = value; }
        }

        string _ServiceUseCode;
        /// <summary>
        /// کد نوع استفاده از خدمات
        /// </summary>
        public string ServiceUseCode
        {
            get { return _ServiceUseCode; }
            set { _ServiceUseCode = value; }
        }

        string _MobileNumber;
        public string MobileNumber
        {
            get { return _MobileNumber; }
            set { _MobileNumber = value; }
        }

        string _ServiceId;
        /// <summary>
        /// شناسه سرویس - مثلا شناسه مبین نت
        /// </summary>
        public string ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }

        PurchaseChargeParameters _ChargeParam;
        /// <summary>
        /// پارامترهای خرید شارژ
        /// </summary>
        public PurchaseChargeParameters ChargeParam
        {
            get { return _ChargeParam; }
            set { _ChargeParam = value; }
        }

    }


    public class SpecialServiceReplyParameters : FinancialReplyParameters
    {
        Enums.SpecialServiceType _ServiceType;
        /// <summary>
        /// نوع سرویس ويژه 
        /// </summary>
        public Enums.SpecialServiceType ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }

        string _ServiceId;
        /// <summary>
        /// شناسه سرویس - مثلا شناسه مبین نت
        /// </summary>
        public string ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
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

        string _MobileNumber;
        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        public string MobileNumber
        {
            get { return _MobileNumber; }
            set { _MobileNumber = value; }
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

        PurchaseChargeReplyParameters _ChargeParam;
        /// <summary>
        /// پارامترهای شارژ
        /// </summary>
        public PurchaseChargeReplyParameters ChargeParam
        {
            get { return _ChargeParam; }
            set { _ChargeParam = value; }
        }
    }
}
