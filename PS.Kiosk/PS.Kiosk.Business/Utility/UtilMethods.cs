using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using billLibrary.BillException;

namespace PS.Kiosk.Business.Utility
{
    public  class UtilMethods
    {
        public static string GetMoneyFormat(string str)
        {
            return PS.Kiosk.Messaging.Utilities.UtilityMethods.GetMoney(str);
        }

        public static DateTime GetDateFormat(string YYYYMMDDHHMMSS)
        {
            return PS.Kiosk.Messaging.Utilities.UtilityMethods.GetDateTime(YYYYMMDDHHMMSS);
        }

        public static string GetBillType(string BillID, out string ErrorMessage)
        {
            ErrorMessage = string.Empty;

            try
            {
                billLibrary.bill BillObj = billLibrary.bill.parseBillIdentifiers(BillID);
                billLibrary.bill.ServiceType srvType = BillObj.SType;

                if (srvType == billLibrary.bill.ServiceType.Electricity)
                    return "برق";
                if (srvType == billLibrary.bill.ServiceType.Gas)
                    return "گاز";
                if (srvType == billLibrary.bill.ServiceType.LandLine)
                    return "تلفن ثابت";
                if (srvType == billLibrary.bill.ServiceType.MobileLine)
                    return "تلفن همراه";
                if (srvType == billLibrary.bill.ServiceType.Water)
                    return "آب";
                if (srvType == billLibrary.bill.ServiceType.Municipality)
                    return "شهرداری";
                
            }
            catch (PaymentIdentifierException payExp)
            {
                ErrorMessage = payExp.Message;
            }
            catch (PaymentIdentifierLengthException payExp)
            {
                ErrorMessage = payExp.Message;
            }
            catch (BillIdentifierException billExp)
            {
                ErrorMessage = billExp.Message;
            }
            catch (BillIdentifierLengthException billExp)
            {
                ErrorMessage = billExp.Message;
            }

            return string.Empty;
        }

        public static Int64 GetBillPayment(string BillID, string PayID,out string ErrorMessage)
        {
            ErrorMessage = string.Empty;
            try
            {
                billLibrary.bill BillObj = billLibrary.bill.parseIdentifiers(BillID, PayID);
                return BillObj.IntAmount;
            }
            catch (PaymentIdentifierException payExp)
            {
                ErrorMessage = payExp.Message;
            }
            catch (PaymentIdentifierLengthException payExp)
            {
                ErrorMessage = payExp.Message;
            }
            catch (BillIdentifierException billExp)
            {
                ErrorMessage = billExp.Message;
            }
            catch (BillIdentifierLengthException billExp)
            {
                ErrorMessage = billExp.Message;
            }

            return 0;
        }

        public static bool IsValidMobileNumber(string MobileNumber)
        {
            Int64 Number;
            if (MobileNumber.StartsWith("09") == false ||  !Int64.TryParse(MobileNumber, out Number) ||
                MobileNumber.Length < 11)
                return false;
            else
                return true;
        }

        public static bool IsValidIrancellNumber(string MobileNumber)
        {
            Int64 Number;
            if (MobileNumber.StartsWith("093") && Int64.TryParse(MobileNumber, out Number) &&
                MobileNumber.Length == 11)
                return true;
            else
                return false;
        }

        public static bool IsValidHamrahAvalNumber(string MobileNumber)
        {
            Int64 Number;

            if (MobileNumber.StartsWith("091") && Int64.TryParse(MobileNumber, out Number) &&
                MobileNumber.Length == 11)
                return true;
            else
                return false;
        }

        public static bool IsValidIrancellWimax(string MobileNumber)
        {
            Int64 Number;
            if (MobileNumber.StartsWith("094") && Int64.TryParse(MobileNumber, out Number))
                return true;
            else
                return false;
        }

        public static bool IsValidMobinNetId(string MoninetId)
        {

            MoninetId = MoninetId.Trim().Replace("+", "").Replace("-", "");

            int tempout;

            if (string.IsNullOrEmpty(MoninetId) || !int.TryParse(MoninetId, out tempout))

                return false;

            if (MoninetId.Length > 15 || MoninetId.Length < 3)

                return false;

            string basePart = MoninetId.Substring(0, MoninetId.Length - 2);

            string checkDigit = MoninetId.Substring(MoninetId.Length - 2, 2);



            if (GetFullMobinNetShenaseh(basePart) == MoninetId)

                return true;

            return false;

        }

        private static string GetFullMobinNetShenaseh(string shenaseh)
        {

            string originalShenaseh = shenaseh;

            int sumOfFactors = 0;

            int[] constFactors = { 15, 14, 13, 12, 11, 10, 9, 1, 2, 3, 4, 5, 6, 7, 8 };

            shenaseh = shenaseh.PadLeft(15, '0');

            char[] digitArray = shenaseh.ToArray<char>();

            int[] primaryDigits = new int[shenaseh.Length];

            for (int i = 0; i < 15; i++)

                sumOfFactors += Convert.ToInt32(digitArray[i].ToString()) * constFactors[i];



            string cDigits = (sumOfFactors % 99).ToString().PadLeft(2, '0');

            return originalShenaseh + cDigits;

        }
    }
}
