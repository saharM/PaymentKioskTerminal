using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public abstract class BaseReplyParameters : BaseParameters
    {
       

        string _ReplyCodeP39;
        /// <summary>
        /// کد پاسخ به درخواست انجام تراکنش
        /// </summary>
        public string ReplyCodeP39
        {
            get { return _ReplyCodeP39; }
            set { _ReplyCodeP39 = value; }
        }

        string _DayMessage48;
        /// <summary>
        /// پيام روز
        /// </summary>
        public string DayMessage48
        {
            get { return _DayMessage48; }
            set { _DayMessage48 = value; }
        }

        string _BuyerMsg48;
        /// <summary>
        /// اطلاعات نمایشی خاص سوييچ در رسید خريدار
        /// </summary>
        public string BuyerMsg48
        {
            get { return _BuyerMsg48; }
            set { _BuyerMsg48 = value; }
        }

        string _AcceptorMsg48;
        /// <summary>
        /// اطلاعات نمایشی خاص سوييچ در رسید پذيرنده
        /// </summary>
        public string AcceptorMsg48
        {
            get { return _AcceptorMsg48; }
            set { _AcceptorMsg48 = value; }
        }

        string _BankNam48;
        /// <summary>
        /// نام بانک صادر کننده
        /// </summary>
        public string BankNam48
        {
            get { return _BankNam48; }
            set { _BankNam48 = value; }
        }

        string _CardType48;
        /// <summary>
        /// نوع کارت
        /// </summary>
        public string CardType48
        {
            get { return _CardType48; }
            set { _CardType48 = value; }
        }

        string _AcceptorName48;
        /// <summary>
        /// نام پذيرنده
        /// </summary>
        public string AcceptorName48
        {
            get { return _AcceptorName48; }
            set { _AcceptorName48 = value; }
        }

        string _AcceptorAddress48;
        /// <summary>
        /// آدرس پذيرنده
        /// </summary>
        public string AcceptorAddress48
        {
            get { return _AcceptorAddress48; }
            set { _AcceptorAddress48 = value; }
        }

        string _AcceptorTelNumber48;
        /// <summary>
        /// تلفن پذيرنده
        /// </summary>
        public string AcceptorTelNumber48
        {
            get { return _AcceptorTelNumber48; }
            set { _AcceptorTelNumber48 = value; }
        }

        string _AcceptorWebsite48;
        /// <summary>
        /// وب سایت پذيرنده
        /// </summary>
        public string AcceptorWebsite48
        {
            get { return _AcceptorWebsite48; }
            set { _AcceptorWebsite48 = value; }
        }

        string _BankWebSite48;
        /// <summary>
        /// وب سایت بانک
        /// </summary>
        public string BankWebSite48
        {
            get { return _BankWebSite48; }
            set { _BankWebSite48 = value; }
        }

        string _TerminalMsg48;
        /// <summary>
        /// پیام ترمینال
        /// </summary>
        public string TerminalMsg48
        {
            get { return _TerminalMsg48; }
            set { _TerminalMsg48 = value; }
        }
    }
}
