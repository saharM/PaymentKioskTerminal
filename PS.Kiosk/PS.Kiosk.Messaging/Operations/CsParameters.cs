using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Messaging.Operations
{
    public class CsParameters
    {

        #region variable

        private string _isoTrack = "";
        private string _Amount = "";
        private string _CVV2 = "    ";
        private string _BillType = "";
        private string _BillID = "";
        private string _PayID = "";
        private string _ResCode = "";
        private string _RefrenceNo = "";
        private string _Result = "";
        private string _Result44 = "";
        private string _MessageText = "";
        private string _PrintText = "";
        private string _Result62 = "";
        private byte[] _PINBlock = null;
        private byte[] _MacKey = null;
        private bool _isresponse = false;
        private int _AuditNumber = 0;
        private int _LastAuditNumber = 0;
        private string _DateTimeTrx = "";
        private int _trxType = 0;
        private int _messageType = 200;
        private string _PAN;
        private int _Language = 1; // 2 farsi 1 english
        private string _ProcessCode;
        private string _TerminalType = "14";
        private string _ExpireDate = "";
        private string _reverseReasonCode;
        private string _DestinationPAN;

        #endregion

        #region Construct
        public CsParameters()
        {
        }

        #endregion

        #region Properties

        public string DestinationPAN
        {
            set
            {
                _DestinationPAN = value;
            }
            get
            {
                return _DestinationPAN;
            }
        }
        public string ExpireDate
        {
            set
            {
                _ExpireDate = value;
            }
            get
            {
                return _ExpireDate;
            }
        }
        public string CVV2
        {
            set
            {
                _CVV2 = value;
            }
            get
            {
                return _CVV2;
            }
        }
        public string ProcessCode
        {
            set
            {
                _ProcessCode = value;
            }
            get
            {
                return _ProcessCode;
            }
        }
    
        public string BillType
        {
            set
            {
                _BillType = value;
            }
            get
            {
                return _BillType;
            }
        }
     
        public int Language
        {
            set
            {
                _Language = value;
            }
            get
            {
                return _Language;
            }
        }
        public string PAN
        {
            set
            {
                _PAN = value;
            }
            get
            {
                return _PAN;
            }
        }
        public int messageType
        {
            set
            {
                _messageType = value;
            }
            get
            {
                return _messageType;
            }
        }
        public int trxType
        {
            set
            {
                _trxType = value;
            }
            get
            {
                return _trxType;
            }
        }
        public string DateTimeTrx
        {
            set
            {
                _DateTimeTrx = value;
            }
            get
            {
                return _DateTimeTrx;
            }
        }

        public int LastAuditNumber
        {
            get
            {
                return _LastAuditNumber;
            }
            set
            {
                _LastAuditNumber = value;
            }
        }
        public int AuditNumber
        {
            get
            {
                return _AuditNumber;
            }
            set
            {
                _AuditNumber = value;
            }
        }
        public string isoTrack
        {
            get
            {
                return _isoTrack;
            }
            set
            {
                _isoTrack = value;
            }
        }
        public string Amount
        {
            set
            {
                _Amount = value;
            }
            get
            {
                return _Amount;
            }
        }
   
      
        public string TerminalType
        {
            set
            {
                _TerminalType = value;
            }
            get
            {
                return _TerminalType;
            }
        }
        public string BillID
        {
            set
            {
                _BillID = value;
            }
            get
            {
                return _BillID;
            }
        }
        public string PayID
        {
            set
            {
                _PayID = value;
            }
            get
            {
                return _PayID;
            }
        }
        public string ResCode
        {
            set
            {
                _ResCode = value;
            }
            get
            {
                return _ResCode;
            }
        }
        public string RefrenceNo
        {
            set
            {
                _RefrenceNo = value;
            }
            get
            {
                return _RefrenceNo;
            }
        }
        public string Result
        {
            set
            {
                _Result = value;
            }
            get
            {
                return _Result;
            }
        }
        public string Result62
        {
            set
            {
                _Result62 = value;
            }
            get
            {
                return _Result62;
            }
        }
        public string Result44
        {
            set
            {
                _Result44 = value;
            }
            get
            {
                return _Result44;
            }
        }
        public string MessageText
        {
            set
            {
                _MessageText = value;
            }
            get
            {
                return _MessageText;
            }
        }
        public string PrintText
        {
            set
            {
                _PrintText = value;
            }
            get
            {
                return _PrintText;
            }
        }
        public byte[] PINBlock
        {
            set
            {
                _PINBlock = value;
            }
            get
            {
                return _PINBlock;
            }
        }
        public byte[] MacKey
        {
            set
            {
                _MacKey = value;
            }
            get
            {
                return _MacKey;
            }
        }
        public bool isresponse
        {
            set
            {
                _isresponse = value;
            }
            get
            {
                return _isresponse;
            }
        }

        public string ReverseReasonCode
        {
            get
            {
                return _reverseReasonCode;
            }
            set
            {
                _reverseReasonCode = value;
            }
        }

        #endregion

        #region Methods



        #endregion

    }
}
