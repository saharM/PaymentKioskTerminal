using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using Fanap;
namespace PS.Kiosk.Messaging.Operations
{

    public class CsResponse
    {
        #region variables
        private string _strRes = "";
        private string _messText = "";
        private string _NameFamily = "";
        private bool _Result = false;
        private string _Audit = "";
        private string _datetime = "";
        #endregion

        #region constructors
        public CsResponse()
        {
        }

        #endregion

        #region properties
        public string DateTimeTrx
        {
            set
            {
                _datetime = value;
            }
            get
            {
                return _datetime;
            }
        }
        public bool Result
        {
            set
            {
                _Result = value;
            }
            get
            {
                return _Result; ;
            }
        }
        public string NameFamily
        {
            set
            {
                _NameFamily = value;
            }
            get
            {
                return _NameFamily;
            }
        }
        public string ResponseCode
        {
            set
            {
                _strRes = value;
            }
            get
            {
                return _strRes;
            }
        }
        public string MessageText
        {
            get
            {
                return _messText;
            }
            set
            {
                _messText = value;
            }
        }
        public string Audit
        {
            get
            {
                return _Audit;
            }
            set
            {
                _Audit = value;
            }
        }
        #endregion

        #region methods
       
        #endregion
    }
   
}
