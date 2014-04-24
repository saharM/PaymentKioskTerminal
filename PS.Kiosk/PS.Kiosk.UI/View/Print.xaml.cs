using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PS.Kiosk.Framework;
using PS.Kiosk.Common.Model;
using PS.Kiosk.UI.Report;
using GrapeCity.ActiveReports.Document.Section;
using PS.Kiosk.Framework.ExceptionManagement;
using System.Threading;
using System.Windows.Forms.Integration;

namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : UserControl
    {
        
        // اين دو متغيير در اينجا تعريف شده اند که در زمان بارگزاری برنامه ايجاد شوند که در زمان پرينت در زمان صرفه جویی شود
        GrapeCity.ActiveReports.Viewer.Win.Viewer ReportViewer = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
        GrapeCity.ActiveReports.SectionReport rpt = new GrapeCity.ActiveReports.SectionReport();

        public Print()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(Print_VisibleChanged);
        }

        private void Print_VisibleChanged(object sender,DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                ShowWaiting();
                PrintData();
            }
        }

        private void PrintData()
        {
            States StateForPrint = StateManager.Instance.Current.PreviousState;
            bool IsFinancialTran = false;
            try
            {
                
                
                if (StateForPrint == States.EmptyState)
                    return;

                if (PrintParameters.PrintData != null)
                {


                    //چاپ موجودی
                    if (StateForPrint == States.GetBalanceInquiryActionState && PrintParameters.PrintData is BalanceInquiryReplyParameteres)
                        rpt = new rptBalanceInquiry();

                    //چاپ شارژ
                    if ((StateForPrint == States.GetChargeState )
                        && PrintParameters.PrintData is PurchaseChargeReplyParameters)
                    {
                        IsFinancialTran = true;
                        rpt = new rptChargePurchase();
                    }

                    //چاپ پرداخت قبض
                    if (StateForPrint == States.PayBillState && PrintParameters.PrintData is BillPaymentReplyParameters)
                    {
                        IsFinancialTran = true;
                        rpt = new rptPayBill();
                    }

                    //چاپ سرویس ويژه
                    if (StateForPrint == States.PaySpeciallServicesState && PrintParameters.PrintData is SpecialServiceReplyParameters)
                        rpt = new rptSpecialServicePay();

                    if (!StateManager.Instance.Current.CheckPrinterStatus())
                    {
                        throw new Exception("Printer is UnAvailable");
                    }

                    //throw new Exception("Printer is UnAvailable");

                    rpt.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                    rpt.PageSettings.PaperSource = System.Drawing.Printing.PaperSourceKind.Custom;
                    rpt.PageSettings.PaperWidth = 8f;
                    if (StateForPrint == States.PayBillState)
                        rpt.PageSettings.PaperHeight = 5f;
                    else
                        rpt.PageSettings.PaperHeight = 16f;
                    rpt.PageSettings.Margins = new Margins(0, 0, 0, 0);
                    ReportViewer.Document = rpt.Document;
                    rpt.Run();
                    ReportViewer.Print(false, false, false);

                    //throw new Exception("Test");

                    //برای تراکنش های مالی بعد از پرینت باید درخواست پرداخت انجام شود
                    //تراکنش های 93 ای ندارند
                    if (IsFinancialTran)
                        StateManager.Instance.Current.SettleReversal93((int)Enums.SpecialServiceType.Financial);
                        //StateManager.Instance.Current.Settle();

                    PrintParameters.PrintData = null;
                    PrintParameters.PrintDate = string.Empty;
                    StateManager.Instance.Current.Return();


                }
                else
                    throw new CustomException("No Data For Print");
            }
            catch (CustomException EX)
            {
                //در بسیاری از تراکنشهای مالی درصورتی که نتوانست چاپ کند باید برگشت انجام دهد
                //تراکنش های 93 ای ندارند
                if (IsFinancialTran)
                    StateManager.Instance.Current.SettleReversal93((int)Enums.SpecialServiceType.Reversal);
                    //StateManager.Instance.Current.Reversal();

                StateManager.Instance.Current.Error(EX);
            }
            catch (Exception EX)
            {
                

                //در بسیاری از تراکنشهای مالی درصورتی که نتوانست چاپ کند باید برگشت انجام دهد
                if (IsFinancialTran)
                    StateManager.Instance.Current.SettleReversal93((int)Enums.SpecialServiceType.Reversal);
                    //StateManager.Instance.Current.Reversal();

                StateManager.Instance.Current.Error(EX.Message);
                
            }
        }

        private void ShowWaiting()
        {
            WindowsFormsHost host = new WindowsFormsHost();
            host.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            host.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            Player player = new Player();
            if (this.IsVisible)
            {
                //the Windows Forms Host hosts the Flash Player
                host.Child = player;

                //the WPF Grid hosts the Windows Forms Host
                FlashPlayerGrid.Children.Add(host);
                string path = System.Windows.Forms.Application.StartupPath + "\\Resources\\" + "printing.swf";

                player.LoadMovie(path);

                player.Play();
            }
            else
                player.Stop();
        }

    }
}
