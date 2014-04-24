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
    /// Interaction logic for ShowHamrahAvalBillInfo.xaml
    /// </summary>
    public partial class ShowHamrahAvalBillInfo : UserControl
    {
        public ShowHamrahAvalBillInfo()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbtnFinalterm.IsChecked == true || rbtnMidterm.IsChecked == true)
                {
                    if (rbtnMidterm.IsChecked == true)
                    {
                        Int64 amount;
                        if (Int64.TryParse(lblMidTem.Content.ToString().Replace(",", ""), out amount) && amount > 0)
                        {
                            StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalMidTermBill);
                            StateManager.Instance.Current.ShowWaiting(States.PaySpeciallServicesState, null, amount);
                        }
                    }
                    else
                    {
                        Int64 amount;
                        if (Int64.TryParse(lblFinalTerm.Content.ToString().Replace(",", ""), out amount) && amount > 0)
                        {
                            StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalFinalTermBill);
                            StateManager.Instance.Current.ShowWaiting(States.PaySpeciallServicesState, null, amount);
                        }
                    }

                }
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.Return();
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.Exit();
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnMidtermSelect_Click(object sender, RoutedEventArgs e)
        {
            rbtnMidterm.IsChecked = true;
        }

        private void btnFinalTermSelect_Click(object sender, RoutedEventArgs e)
        {
            rbtnFinalterm.IsChecked = true;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
                StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalFinalTermInfo);
        }

       
    }
}
