using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    /// <summary>
    /// اطلاعات مربوط به تراکنش اصلی از پيغام درخواست یعنی 200 گرفته می شود
    /// </summary>
    public class ReversalParameters : FinancialParameters
    {
        
        Enums.ProcessCode _PrimaryProcessCode;
        /// <summary>
        /// کد پردازش تراکنش اصلی
        /// </summary>
        public Enums.ProcessCode PrimaryProcessCode
        {
            get { return _PrimaryProcessCode; }
            set { _PrimaryProcessCode = value; }
        }

        Int64 _PrimaryAmount;
        /// <summary>
        /// مبلغ تراکنش اصلی
        /// </summary>
        public Int64 PrimaryAmount
        {
            get { return _PrimaryAmount; }
            set { _PrimaryAmount = value; }
        }

        Int64 _PrimaryTranRefNumber;
        /// <summary>
        /// شماره مرجع تراکنش اصلی
        /// </summary>
        public Int64 PrimaryTranRefNumber
        {
            get { return _PrimaryTranRefNumber; }
            set { _PrimaryTranRefNumber = value; }
        }

        int _PrimaryStan;
        /// <summary>
        /// شماره پيگيری تراکنش اصلی
        /// </summary>
        public int PrimaryStan
        {
            get { return _PrimaryStan; }
            set { _PrimaryStan = value; }
        }

        string _PrimaryDateTime;
        /// <summary>
        /// تاريخ تراکنش مورد اصلاحيه
        /// </summary>
        public string PrimaryDateTime
        {
            get { return _PrimaryDateTime; }
            set { _PrimaryDateTime = value; }
        }

        Int64 _PrimaryNewAmount;
        /// <summary>
        /// مبلغ جديد تراکنش اصلی
        /// </summary>
        public Int64 PrimaryNewAmount
        {
            get { return _PrimaryNewAmount; }
            set { _PrimaryNewAmount = value; }
        }

        static FinancialParameters _ReversalObject;
        /// <summary>
        /// در صورت بروز خطا این تراکنش را لازم است برگشت دهيم
        /// فقط برای حالتی که در همان لحظه خطا می خواهیم برگشت را بفرستیم
        /// </summary>
        public static FinancialParameters ReversalObject
        {
            get { return ReversalParameters._ReversalObject; }
            set { ReversalParameters._ReversalObject = value; }
        }

    }

    public class ReversalReplyParameters : FinancialReplyParameters
    {
        Enums.ProcessCode _PrimaryProcessCode;
        /// <summary>
        /// کد پردازش تراکنش اصلی
        /// </summary>
        public Enums.ProcessCode PrimaryProcessCode
        {
            get { return _PrimaryProcessCode; }
            set { _PrimaryProcessCode = value; }
        }

        Int64 _PrimaryAmount;
        /// <summary>
        /// مبلغ تراکنش اصلی
        /// </summary>
        public Int64 PrimaryAmount
        {
            get { return _PrimaryAmount; }
            set { _PrimaryAmount = value; }
        }

        Int64 _PrimaryTranRefNumber;
        /// <summary>
        /// شماره مرجع تراکنش اصلی
        /// </summary>
        public Int64 PrimaryTranRefNumber
        {
            get { return _PrimaryTranRefNumber; }
            set { _PrimaryTranRefNumber = value; }
        }

        string _PrimaryDateTime;
        /// <summary>
        /// تاريخ تراکنش مورد اصلاحيه
        /// </summary>
        public string PrimaryDateTime
        {
            get { return _PrimaryDateTime; }
            set { _PrimaryDateTime = value; }
        }

        BalanceInquiryReplyParameteres _BalanceInfo;
        /// <summary>
        /// اطلاعات موجودی با فرمت خاص در فيلد 54
        /// </summary>
        public BalanceInquiryReplyParameteres BalanceInfo
        {
            get { return _BalanceInfo; }
            set { _BalanceInfo = value; }
        }
    }
}
