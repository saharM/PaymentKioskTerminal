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
    /// Interaction logic for GetHamrahavalChargeType.xaml
    /// </summary>
    public partial class GetHamrahavalChargeType : UserControl
    {
        public GetHamrahavalChargeType()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void btn1000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState,Enums.ChargeType.HamrahAval, 10000);
        }

        private void btn10000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.HamrahAval, 100000);
        }

        private void btn2000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.HamrahAval, 20000);
        }

        private void btn20000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.HamrahAval, 200000);
        }

        private void btn5000_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.HamrahAval, 50000);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Return();
        }
    }
}
