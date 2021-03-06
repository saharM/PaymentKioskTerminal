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
    public partial class rptChargePurchase : GrapeCity.ActiveReports.SectionReport
    {

        public rptChargePurchase()
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

            PurchaseChargeReplyParameters param = (PrintParameters.PrintData as PurchaseChargeReplyParameters);
            txtCardNumber.Text = param.CardNumber.Substring(0, 6) + "******" + param.CardNumber.Substring(param.CardNumber.Length - 5);
            txtBankName.Text = param.BankNam48;
            txtStan.Text = param.Stan.ToString();

            txtRefNumber.Text = param.TranRefNumber.ToString();
            txtAmount.Text = PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.ChargeAmount) + " " + "ريال";

            if (((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.Irancell)
                txtChargeType.Text = "ايرانسل:" + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.ChargeAmount) + " ريالی ";
            if (((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.HamrahAval)
                txtChargeType.Text = "همراه اول:" + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.ChargeAmount) + " ريالی ";
            if (((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.Talia)
                txtChargeType.Text = "تاليا:" + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.ChargeAmount) + " ريالی ";
            if (((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.Rightel)
                txtChargeType.Text = "رایتل:" + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.ChargeAmount) + " ريالی ";

            txtChargePass.Text = param.ChargePassword.ToString();
            txtChargePass2.Text = param.ChargePassword.ToString();
            txtSerial.Text = param.ChargeSerial;//.Replace("IR","");

            if (((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.Irancell ||
                ((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98))) == Enums.ChargeType.Rightel)
                txtUse.Text = "*141*" + param.ChargePassword + "#";

            if ((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98)) == Enums.ChargeType.HamrahAval)
                txtUse.Text = "*140*#" + param.ChargePassword + "#";

            if ((Enums.ChargeType)(Convert.ToInt32(param.ChargeTypeP98)) == Enums.ChargeType.Talia)
                txtUse.Text = "*140*" + param.ChargePassword + "#";

            if (!string.IsNullOrEmpty(param.SpecialMsg))
                txtTerminalMsg.Text = param.SpecialMsg;

            txtDate.Text = PrintParameters.PrintDate;
            
            
        }
    }
}
