using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

//I have added some methods to structure SYSTEMTIME. After that the convertion between System.DateTime and SYSTEM became much easier. :)
    //Sample Code in C#:

    //Sample code for SetLocalTime and SYSTEMTIME structure
    //Author:    Yuan Xiaohui from www.farproc.com
    //Time :    August 30th, 2005



namespace PS.Kiosk.Messaging.Operations
{
    public class CsChangeDateTime
    {
        /// <summary>
        /// SYSTEMTIME structure with some useful methods
        /// </summary>
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            /// <summary>
            /// Convert form System.DateTime
            /// </summary>
            /// <param name="time"></param>
            public void FromDateTime(DateTime time)
            {
                wYear = (ushort)time.Year;
                wMonth = (ushort)time.Month;
                wDayOfWeek = (ushort)time.DayOfWeek;
                wDay = (ushort)time.Day;
                wHour = (ushort)time.Hour;
                wMinute = (ushort)time.Minute;
                wSecond = (ushort)time.Second;
                wMilliseconds = (ushort)time.Millisecond;
            }

            /// <summary>
            /// Convert to System.DateTime
            /// </summary>
            /// <returns></returns>
            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
            }
            /// <summary>
            /// STATIC: Convert to System.DateTime
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static DateTime ToDateTime(SYSTEMTIME time)
            {
                return time.ToDateTime();
            }
            

            //SetLocalTime C# Signature
            [DllImport("Kernel32.dll")]
            public static extern bool SetLocalTime( ref SYSTEMTIME Time );

            //Example
            public static void AddDays(int dayCount)
            {
                //Get current time and add 7 days to it
                DateTime t = new DateTime();

                t = DateTime.Now.AddDays(dayCount); ;

                //Convert to SYSTEMTIME
                SYSTEMTIME st = new SYSTEMTIME();
                st.FromDateTime(t);
                //Call Win32 API to set time
                SetLocalTime(ref st);
            }
        }
        
    }
}
