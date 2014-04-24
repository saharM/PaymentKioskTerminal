using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class SettleReverse93Parameters : ReversalParameters
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
    }

    public class SettleReverse93ReplyParameters : ReversalReplyParameters
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
    }
}
