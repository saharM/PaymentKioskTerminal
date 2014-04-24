using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public abstract class BaseParameters
    {
        string _AppVersion;
        /// <summary>
        /// ورژن برنامه
        /// </summary>
        public string AppVersion
        {
            get { return _AppVersion; }
            set { _AppVersion = value; }
        }

        Enums.SwitchMsgFormat _MsgFormat;
        /// <summary>
        /// ورژن پیغام که 87 است یا93 
        /// </summary>
        public Enums.SwitchMsgFormat MsgFormat
        {
            get { return _MsgFormat; }
            set { _MsgFormat = value; }
        }
        

        Enums.MsgType _MsgType;
        public Enums.MsgType MsgType
        {
            get { return _MsgType; }
            set { _MsgType = value; }
        }

        Enums.ProcessCode _ProcessCode;
        /// <summary>
        /// کد پردازش
        /// </summary>
        public Enums.ProcessCode ProcessCode
        {
            get { return _ProcessCode; }
            set { _ProcessCode = value; }
        }

        int _Stan;
        /// <summary>
        /// شماره پیگيری(Serial Tran Number)
        /// </summary>
        public int Stan
        {
            get { return _Stan; }
            set { _Stan = value; }
        }

        Int64 _LastSuccedStan;
        /// <summary>
        ///آخرین تراکنش دريافتی موفق Stan
        /// </summary>
        public Int64 LastSuccedStan
        {
            get { return _LastSuccedStan; }
            set { _LastSuccedStan = value; }
        }

        Int64 _TranRefNumber;
        /// <summary>
        /// شماره مرجع تراکنش
        /// </summary>
        public Int64 TranRefNumber
        {
            get { return _TranRefNumber; }
            set { _TranRefNumber = value; }
        }

        DateTime _DateTimeP7;
        /// <summary>
        /// تاریخ و زمان
        /// </summary>
        public DateTime DateTimeP7
        {
            get { return _DateTimeP7; }
            set { _DateTimeP7 = value; }
        }

        TimeSpan _TranTime;
        /// <summary>
        /// زمان تراکنش
        /// </summary>
        public TimeSpan TranTime
        {
            get { return _TranTime; }
            set { _TranTime = value; }
        }

        DateTime _TranDate;
        /// <summary>
        /// تاريخ تراکنش
        /// </summary>
        public DateTime TranDate
        {
            get { return _TranDate; }
            set { _TranDate = value; }
        }

        string _BankAcceptorId;
        /// <summary>
        /// کد شناسايي پذيرنده بانک
        /// </summary>
        public string BankAcceptorId
        {
            get { return _BankAcceptorId; }
            set { _BankAcceptorId = value; }
        }

        string _IsoTrack;
        /// <summary>
        /// شیار دوم نوار مغناطيسی
        /// </summary>
        public string IsoTrack
        {
            get { return _IsoTrack; }
            set { _IsoTrack = value; }
        }

        string _ExtraData;
        /// <summary>
        /// اطلاعات اضافی - رمز دریافتی
        /// </summary>
        public string ExtraData
        {
            get { return _ExtraData; }
            set { _ExtraData = value; }
        }

        string _IP;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        int _Port;
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        string _TerminalAcceptorlId;
        /// <summary>
        /// شماره شناسایی پایانه پذيرنده کارت
        /// </summary>
        public string TerminalAcceptorId
        {
            get { return _TerminalAcceptorlId; }
            set { _TerminalAcceptorlId = value; }
        }

        string _TerminalAcceptorName;
        /// <summary>
        ///  نام پایانه پذيرنده کارت
        /// </summary>
        public string TerminalAcceptorName
        {
            get { return _TerminalAcceptorName; }
            set { _TerminalAcceptorName = value; }
        }

        string _CardAcceptorId;
        /// <summary>
        /// شماره شناسایی پذيرنده کارت
        /// </summary>
        public string CardAcceptorId
        {
            get { return _CardAcceptorId; }
            set { _CardAcceptorId = value; }
        }

        string _CardAcceptorBinCode;

        public string CardAcceptorBinCode
        {
            get { return _CardAcceptorBinCode; }
            set { _CardAcceptorBinCode = value; }
        }

        int _TimeOut;
        public int TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        int _ErrorCode;
        /// <summary>
        /// کد خطای رخ داده
        /// </summary>
        public int ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        string _ErrorMsg;
        /// <summary>
        /// پیغام خطای رخ داده
        /// </summary>
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set { _ErrorMsg = value; }
        }

        bool _TranSuccess;
        /// <summary>
        /// نتیجه اجرای تراکنش موفقیت آمیز بود یا نه
        /// </summary>
        public bool TranSuccess
        {
            get { return _TranSuccess; }
            set { _TranSuccess = value; }
        }
        
    }

    public abstract class ParametersFactory
    {
        public abstract BaseParameters GetParameters();
    }
    public class GenericFactory<T> : ParametersFactory where T : BaseParameters,new()
    {
        public override BaseParameters GetParameters()
        {
            return new T();
        }
    }
}
