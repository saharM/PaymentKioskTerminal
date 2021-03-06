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
    public partial class rptBalanceInquiry : GrapeCity.ActiveReports.SectionReport
    {

        public rptBalanceInquiry()
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

            BalanceInquiryReplyParameteres param = ( PrintParameters.PrintData as BalanceInquiryReplyParameteres);
            txtCardNumber.Text = param.CardNumber.Substring(0, 6) + "******" + param.CardNumber.Substring(param.CardNumber.Length - 5);
            txtBankName.Text = param.BankNam48;
            txtStan.Text = param.Stan.ToString();
            txtRefNumber.Text = param.TranRefNumber.ToString();
            txtLedger.Text = " " + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.LedgerBalance.ToString()) + " " + "ريال";
            txtAvailable.Text = " " + PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(param.AvailableBalance.ToString()) + " " + "ريال";

            if (!string.IsNullOrEmpty(param.Wage) && Convert.ToInt64(param.Wage) > 0)
            {
                lblWage.Visible = true;
                txtWage.Visible = true;
                txtWage.Text = PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(Convert.ToInt64(param.Wage).ToString()) + " " + "ريال";
            }

            if (!string.IsNullOrEmpty(param.SpecialMsg))
                txtTerminalMsg.Text = param.SpecialMsg;
            
            txtDate.Text = PrintParameters.PrintDate;
            
            
        }
    }
}
