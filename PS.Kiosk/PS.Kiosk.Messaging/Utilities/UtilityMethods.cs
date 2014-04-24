using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanap.Messaging;
using Fanap.Messaging.Iso8583;
using PS.Kiosk.Messaging.Operations;
using System.Runtime.InteropServices;

namespace PS.Kiosk.Messaging.Utilities
{
    public class UtilityMethods
    {
        #region Property

        private static byte[] _FieldSeprator; //={ 0x1c }; 
        public static byte[] FieldSeprator
        {
            get
            {
                if (UtilityMethods._FieldSeprator == null)
                    UtilityMethods._FieldSeprator = new byte[] { 0x1c };
                return UtilityMethods._FieldSeprator;
            }
           
        }

        private static string _FieldSepratorStr;
        public static string FieldSepratorStr
        {
            get
            {
                if (UtilityMethods._FieldSepratorStr == null)
                    UtilityMethods._FieldSepratorStr = System.Text.Encoding.ASCII.GetString(FieldSeprator);
                return UtilityMethods._FieldSepratorStr;
            }
           
        }
        
       
        private static CsUtil csUtil = new CsUtil();
        private const int HEADER_LEN = 4;

        

        public struct SYSTEMTIME
        {
            public short Year, Month, DayOfWeek, Day, Hour, Minute, Second, Millisecond;
            /// <summary>
            /// Convert form System.DateTime
            /// </summary>
            /// <param name="time">Creates System Time from this variable</param>
            public void FromDateTime(DateTime time)
            {
                Year = (short)time.Year;
                Month = (short)time.Month;
                DayOfWeek = (short)time.DayOfWeek;
                Day = (short)time.Day;
                Hour = (short)time.Hour;
                Minute = (short)time.Minute;
                Second = (short)time.Second;
                Millisecond = (short)time.Millisecond;
            }
        }

        #endregion Property

        #region DateTime

        public static DateTime GetDateTime(Message outMsg)
        {
           
            DateTime datetime = new DateTime();
            if(outMsg.Formatter is Fanap.Messaging.Iso8583.Iso8583Ascii1993MessageFormatter)
                datetime =  new DateTime(2000 + Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(0, 2)), Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(2, 2)), Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(4, 2)), Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(6, 2)), Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(8, 2)), Convert.ToInt32(outMsg.Fields[12].Value.ToString().Substring(10, 2)));
            if (outMsg.Formatter is Fanap.Messaging.Iso8583.Iso8583Ascii1987MessageFormatter)
                datetime = new DateTime(Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(0, 4)), Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(4, 2)), Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(6, 2)), Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(8, 2)), Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(10, 2)), Convert.ToInt32(outMsg.Fields[7].Value.ToString().Substring(12, 2)));

            return datetime;
        }

        /// <summary>
        /// Convert to DateTime
        /// </summary>
        /// <param name="YYYYMMDDHHMMSS">Input Format = YYYYMMDDHHMMSS</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string YYYYMMDDHHMMSS)
        {
            return new DateTime(Convert.ToInt32(YYYYMMDDHHMMSS.Substring(0, 4)), Convert.ToInt32(YYYYMMDDHHMMSS.Substring(4, 2)), Convert.ToInt32(YYYYMMDDHHMMSS.Substring(6, 2)), Convert.ToInt32(YYYYMMDDHHMMSS.Substring(8, 2)), Convert.ToInt32(YYYYMMDDHHMMSS.Substring(10, 2)), Convert.ToInt32(YYYYMMDDHHMMSS.Substring(12, 2)));
        }

        /// <summary>
        /// Convert To DateTime
        /// </summary>
        /// <param name="YYMMDDHHMMSS">Input Format = YYMMDDHHMMSS</param>
        /// <returns></returns>
        public static DateTime GetDateTime2(string YYMMDDHHMMSS)
        {
            return new DateTime(Convert.ToInt32(YYMMDDHHMMSS.Substring(0, 2)), Convert.ToInt32(YYMMDDHHMMSS.Substring(2, 2)), Convert.ToInt32(YYMMDDHHMMSS.Substring(4, 2)), Convert.ToInt32(YYMMDDHHMMSS.Substring(6, 2)), Convert.ToInt32(YYMMDDHHMMSS.Substring(8, 2)), Convert.ToInt32(YYMMDDHHMMSS.Substring(10, 2)));
        }

        public static string GetYYYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        public static string GetYYMMDDhhmmss()
        {
            return DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        public static string ChangeToShamsi(DateTime dateTime)
        {
            System.Globalization.PersianCalendar persian = new System.Globalization.PersianCalendar();
            return persian.GetYear(dateTime).ToString() + "/" + persian.GetMonth(dateTime).ToString()
                + "/" + persian.GetDayOfMonth(dateTime).ToString() + "     " + persian.GetHour(dateTime).ToString()
                + ":" + persian.GetMinute(dateTime).ToString() + ":" + persian.GetSecond(dateTime).ToString();
        }

        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "SetLocalTime")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool SetLocalTime([In] ref SYSTEMTIME lpLocalTime);

        /// <summary>
        /// تنظيم ساعت سيستم با زمان دريافتی
        /// </summary>
        /// <param name="systemTime">YYYYMMDDHHMMSS</param>
        public static bool  SetLclTime(string systemTime)
        {
            DateTime dt = GetDateTime(systemTime);
            SYSTEMTIME s = new SYSTEMTIME();
            s.FromDateTime(dt);
            return SetLocalTime(ref s);
 
        }

        #endregion DateTime

        public static byte[] StrtoByte(string str)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str); //new byte[str.Length * sizeof(char)];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string BytetoStr(byte[] bytes)
        {
            return  Encoding.ASCII.GetString(bytes);
            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }

        public static byte[] Int2Byte(int data)
        {
            string str = data.ToString().PadLeft(HEADER_LEN, '0');
            byte[] res = new byte[HEADER_LEN];
            for (int i = 0; i < HEADER_LEN; i++)
                res[i] = (byte)str[i];
            return res;
        }

        public static int Byte2Int(byte[] data, int len)
        {
            int res = 0;
            for (int i = 0; i < len; i++)
                res = res * 10 + data[i] - 0x30;
            return res;
        }

        public static int Byte2IntBCD(byte[] data, int len)
        {
            int res = 0;

            for (int i = 0; i < len; i++)
                res = res * 100 + Convert.ToInt16(data[i].ToString("X"));
            return res;
        }

        private string Hex2Str(string srIn)
        {
            return System.Text.Encoding.Unicode.GetString(csUtil.HexToBin(srIn));
        }

        public static string Ascii2Hex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static byte[] GetKeys(KeyType keyType, Iso8583Message MsgIn)
        {
            string POS;
            string serial;
            try
            {
                byte[] MACK, PINK;


                if (MsgIn.MessageTypeIdentifier == 1804)
                {
                    serial = MsgIn.Fields[48].Value.ToString().Split(';')[2];
                    csUtil.ExtractPinMacKeys(out MACK, out PINK, serial);
                }
                else
                {
                    if (MsgIn.Fields.Contains(41))
                        POS = MsgIn.Fields[41].Value.ToString();
                    else
                        POS = "";
                    return null;
                }

                if (keyType == KeyType.MACKey)
                    return MACK;
                else
                    return PINK;

            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public static string GetMoney(string str)
        {
            if (string.IsNullOrEmpty(str) || str == "0")
                return str;

            string res = "";
            // if(str.Length>3)
            for (int i = str.Length, j = 1; i > 0; i--, j++)
            {
                res = str[i - 1].ToString() + res;
                if ((j % 3) == 0 && i != 1)
                    res = "," + res;

            }
            return res;
        }

       

        

        

    }
}
