using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.UI.Report
{
    /// <summary>
    /// Summary description for rptCash.
    /// </summary>
    public partial class rptRemain : DataDynamics.ActiveReports.ActiveReport3
    {
        public rptRemain(BalanceInquiryReplyParameteres Param )
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();


            txtDate.Text = PrintParameters.DateTime;

            txtCardNo.Text = Param.CardNumber.Substring(0, 6) + "******" + Param.CardNumber.Substring(Param.CardNumber.Length - 5);
            txtCashable.Text = Param.LedgerBalance.ToString();//CsParameters.GetMoney(CsParameters.Result); 
            txtAudit.Text = Param.Stan.ToString();
            txtTerminalNo.Text = PrintParameters.PosSN;
            txtRefNo.Text = Param.TranRefNumber.ToString();
            txtVahedSn.Text = PrintParameters.VahedTejariSN;
        }
        private void pageHeader_Format(object sender, EventArgs e)
        {

        }
       

    }
}
