using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Text;


namespace PS.Kiosk.Framework.ExceptionManagement
{
    /// <summary>
    /// این کلاس وظیفه بازیابی خطاهای فارسی را از فایل دارد
    /// </summary>
	public class ErrorMessage: System.IDisposable
	{
        private static XDocument xmlErrorMessages;

        /// <summary>
        /// در این سازنده بررسی می شود که آیا فایل مربوط به خطاها وجود دارد یا خیر
        /// </summary>
        static ErrorMessage()
        {
            string xmlFile = PS.Kiosk.Framework.Properties.Resources.ErrorMessages;
            if (!string.IsNullOrEmpty(xmlFile))
                xmlErrorMessages = XDocument.Parse(xmlFile);
          
        }

        /// <summary>
        /// پیغام فارسی را مربوط به خطا را بازیابی می کند
        /// </summary>
        /// <param name="customError">متن انگلیسی خطا</param>
        /// <returns>متن فارسی خطا</returns>
		public static string GetErrorDescription( string customError )
		{
            if (customError != "" && xmlErrorMessages != null)
			{
                //اگر پیغام فارسی بود همان را برگرداند
				 const int MaxAnsiCode = 255;
                 if (customError.ToCharArray().Any(i => i > MaxAnsiCode))
                     return customError;

                var query = from m in xmlErrorMessages.Descendants("CustomErrorTable")
                            where m.Element("Name").Value == customError || m.Element("Code").Value == customError
                            select m.Element("Description").Value;

                string strMessage = query.SingleOrDefault();

                

                if (string.IsNullOrEmpty(strMessage))
                    strMessage = "خطا رخ داده است،دوباره امتحان کنيد.";

                
                return strMessage;
			}
			else
			{
                return "خطا رخ داده است،دوباره امتحان کنيد.";
			}
		}

        public static string GetErrorName(string ErrorCode)
        {
            if (ErrorCode != "" && xmlErrorMessages != null)
            {

                var query = from m in xmlErrorMessages.Descendants("CustomErrorTable")
                            where m.Element("Code").Value == ErrorCode
                            select m.Element("Name").Value;

                string strMessage = query.SingleOrDefault();

                
                if (string.IsNullOrEmpty(strMessage))
                    strMessage = "Unhandled Error";


                return strMessage;
            }
            else
            {
                return "Unhandled Error";
            }
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            xmlErrorMessages = null;
        }

        #endregion
    }

	

	

}