using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace PS.Kiosk.Framework.ExceptionManagement
{
    public class CustomException : System.ApplicationException
    {
        private string _ActualMsg;
        public string ActualMsg
        {
            get { return _ActualMsg; }
            set { _ActualMsg = value; }
        }

        private string _LogMessage;
        public string LogMessage
        {
            get { return _LogMessage; }
            set { _LogMessage = value; }
        }

        private string m_message;
        public override string Message
        {
            get
            {
                return m_message;
            }
        }

        public CustomException(string CustomError)
        {
            this.ActualMsg = CustomError;
            this.LogMessage = CustomError;
            m_message = ErrorMessage.GetErrorDescription(CustomError);
            

        }

        public CustomException(string CustomError, string LogMsg)
        {
            this.ActualMsg = CustomError;
            this.LogMessage = LogMsg;
            m_message = ErrorMessage.GetErrorDescription(CustomError);


        }
        public CustomException(System.String message,
            System.Exception innerException) :
            base(message, innerException)
        {
        }
        protected CustomException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) :
            base(info, context)
        {
        }
        public CustomException()
            : base()
        {
        }
    }
}
