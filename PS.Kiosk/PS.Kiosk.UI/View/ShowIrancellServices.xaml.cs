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

namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for ShowIrancellServices.xaml
    /// </summary>
    public partial class ShowIrancellServices : UserControl
    {
        public ShowIrancellServices()
        {
            InitializeComponent();
        }

        private void BtnPayIrancellBill_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.IrancellBill);
            StateManager.Instance.Current.ShowBusy(States.ShowIrancellServicePaymentState);
        }

        private void BtnWimaxCharge_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.IrancellWimax);
            StateManager.Instance.Current.ShowBusy(States.ShowIrancellServicePaymentState);
            //StateManager.Instance.Current.ShowIrancellServicePayment();
        }

        private void BtnReCharge_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.IrancellTopUp);
            StateManager.Instance.Current.ShowBusy(States.ShowIrancellServicePaymentState);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Return();
        }
    }
}
