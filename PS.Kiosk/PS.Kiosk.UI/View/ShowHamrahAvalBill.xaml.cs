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

namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for ShowHamrahAvalBill.xaml
    /// </summary>
    public partial class ShowHamrahAvalBill : UserControl
    {

        public ShowHamrahAvalBill()
        {
            InitializeComponent();
        }

        private bool IsValidInputParameters()
        {

            if (string.IsNullOrEmpty(txtMobileNumber.Text) ||
                 PS.Kiosk.Business.Utility.UtilMethods.IsValidHamrahAvalNumber(txtMobileNumber.Text) == false)
            {
                lblError.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
            {
                lblError.Visibility = System.Windows.Visibility.Hidden;
                return true;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidInputParameters())
                    StateManager.Instance.Current.ShowWaiting(States.ShowHamrahAvalBillInfoState, txtMobileNumber.Text);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtMobileNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;

                //شماره تلفن همرا بیشتر از 11 رقم نباید باشد
                if (txtMobileNumber.Text.Length > 11)
                    StateManager.Instance.Current.Message = txtMobileNumber.Text.Substring(0, 11);

                
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
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
