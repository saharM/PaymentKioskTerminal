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


namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for GetBalanceInquiryAction.xaml
    /// </summary>
    public partial class GetBalanceInquiryAction : UserControl
    {
        

        public GetBalanceInquiryAction()
        {
            InitializeComponent();
            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

            StateManager.Instance.Current.Printing();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Return();
        }

        
       

        

        
    }
}
