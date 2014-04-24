using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Globalization;


namespace PS.Kiosk.Messaging.Operations
{
    public class CsMerchant
    {
        #region Vars
        private Fanap.Messaging.Message _message = null;
        private object _merchID = null;
        public object _this = null;
        #endregion

        #region Construct
        public CsMerchant()
        {
        }
        #endregion


        #region properties
        public object MerchantID
        {
            set
            {
                _merchID = value;
            }
            get
            {
                return _merchID;
            }
        }
        public Fanap.Messaging.Message IsoMessage
        {
            set
            {
                _message = value;
            }
            get
            {
                return _message;
            }
        }



        #endregion
    }
}
