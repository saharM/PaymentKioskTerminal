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
    /// Interaction logic for GetRightelChargeType.xaml
    /// </summary>
    public partial class GetRightelChargeType : UserControl
    {
        public GetRightelChargeType()
        {
            InitializeComponent();
        }

        private void btn20000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Rightel, 20000);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX);
            }
        }

        private void btn50000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Rightel, 50000);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX);
            }
        }

        private void btn100000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Rightel, 100000);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX);
            }
        }

        private void btn200000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Rightel, 200000);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX);
            }
        }

        private void btn500000_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowWaiting(States.GetChargeState, Enums.ChargeType.Rightel, 500000);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX);
            }
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
