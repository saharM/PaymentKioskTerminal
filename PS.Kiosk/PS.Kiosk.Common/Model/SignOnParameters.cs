using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Kiosk.Common.Model
{
    public class SignOnParameters : BaseParameters
    {
        int _DeviceCodeP25;
        /// <summary>
        /// کد نوع دستگاه
        /// </summary>
        public int DeviceCodeP25
        {
            get { return _DeviceCodeP25; }
            set { _DeviceCodeP25 = value; }
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

        string _MACP64;
        /// <summary>
        /// فیلد 64ام
        /// </summary>
        public string MACP64
        {
            get { return _MACP64; }
            set { _MACP64 = value; }
        }
    }
}
