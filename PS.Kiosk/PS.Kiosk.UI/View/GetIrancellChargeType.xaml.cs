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
    /// Interaction logic for GetIrancellChargeType.xaml
    /// </summary>
    public partial class GetIrancellChargeType : UserControl
    {
        public GetIrancellChargeType()
        {
            InitializeComponent();
        }

        private void btnIrancell1000_Click(object sender, RoutedEventArgs e)
        {
            
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Irancell, 10000);
        }

        private void btnIrancell2000_Click(object sender, RoutedEventArgs e)
        {

            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Irancell, 20000);
        }

        private void btnIrancell5000_Click(object sender, RoutedEventArgs e)
        {
            
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Irancell,50000);
        }

        private void btnIrancell10000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Irancell, 200000);
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
