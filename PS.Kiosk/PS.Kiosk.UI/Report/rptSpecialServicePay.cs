using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.UI.Report
{
    /// <summary>
    /// Summary description for rptBalanceInquiry.
    /// </summary>
    public partial class rptSpecialServicePay : GrapeCity.ActiveReports.SectionReport
    {

        public rptSpecialServicePay()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            if (SignOnReplyParameters.VahedeTejariDS != null)
                txtAcceptorName.Text = SignOnReplyParameters.VahedeTejariDS;
            if (SignOnReplyParameters.VahedeTejariTel != null)
                txtAcceptorTel.Text = SignOnReplyParameters.VahedeTejariTel;
            if (SignOnReplyParameters.PosSN.ToString() != null)
                txtPosSn.Text = SignOnReplyParameters.PosSN.ToString();
            txtAcceptorSn.Text = SignOnReplyParameters.VahedTejariSN;

            SpecialServiceReplyParameters param = (PrintParameters.PrintData as SpecialServiceReplyParameters);

           

            txtCardNumber.Text = param.CardNumber.Substring(0, 6) + "******" + param.CardNumber.Substring(param.CardNumber.Length - 5);
            txtBankName.Text = param.BankNam48;
            txtStan.Text = param.Stan.ToString();
            txtRefNumber.Text = param.TranRefNumber.ToString();

            string type = string.Empty;
            if (param.ServiceType == Enums.SpecialServiceType.JiringCharge)
            {
                lblTitle.Text = "شارژ جيرينگ";
                type = "جيرينگ";
                
            }
            if (param.ServiceType == Enums.SpecialServiceType.Mobinnet)
            {
                lblTitle.Text = "شارژ مبین نت";
                type = " مبین نت";

            }
            if (param.ServiceType == Enums.SpecialServiceType.IrancellTopUp)
            {
                lblTitle.Text = "شارژ ايرانسل";
                type = "ايرانسل";
            }
            if (param.ServiceType == Enums.SpecialServiceType.HamrahAvalTopUp)
            {
                lblTitle.Text = "شارژ همراه اول";
                type = "همراه اول";
            }
            if (param.ServiceType == Enums.SpecialServiceType.IrancellWimax)
            {
                lblTitle.Text = "شارژ وايمکس";
                type = "وايمکس";
                lblMobile.Text = "شماره وايمکس";
            }
            if (param.ServiceType == Enums.SpecialServiceType.IrancellBill)
            {
                lblTitle.Text = "پرداخت قبض ايرانسل";
            }

            txtJiringType.Text = type + " " + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.TranAmountP4.ToString()) + " " + "ريالی";

            if (param.ServiceType == Enums.SpecialServiceType.HamrahAvalFinalTermBill)
            {
                lblTitle.Text = "پرداخت قبض پایان دوره همراه اول";
                txtJiringType.Text = "مبلغ"+ " " + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.TranAmountP4.ToString()) + " " + "ريال";
            }

            if (param.ServiceType == Enums.SpecialServiceType.HamrahAvalMidTermBill)
            {
                lblTitle.Text = "پرداخت قبض میان دوره همراه اول";
                txtJiringType.Text = "مبلغ" + " " + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.TranAmountP4.ToString()) + " " + "ريال";
            }

            
            txtMobileNumber.Text = param.MobileNumber;

            if (!string.IsNullOrEmpty(param.SpecialMsg))
                txtTerminalMsg.Text = param.SpecialMsg;
            
            txtDate.Text = PrintParameters.PrintDate;
           
            
        }
    }
}
