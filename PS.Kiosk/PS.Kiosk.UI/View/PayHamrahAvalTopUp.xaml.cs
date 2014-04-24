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
    /// Interaction logic for Jiring.xaml
    /// </summary>
    public partial class PayHamrahAvalTopUp : UserControl
    {
        public PayHamrahAvalTopUp()
        {
            InitializeComponent();
        }

        #region CustomMethod

        private void ClearControl()
        {
            txtPayAmount.Text = string.Empty;
            txtMobileNumber.Text = string.Empty;
            lblError.Visibility = System.Windows.Visibility.Hidden;
        }

        private bool IsValidInputParameters()
        {
            Int64 telNumber; Int64 PayAmount;

            if (string.IsNullOrEmpty(txtMobileNumber.Text) || string.IsNullOrEmpty(txtPayAmount.Text) ||
                PS.Kiosk.Business.Utility.UtilMethods.IsValidHamrahAvalNumber(txtMobileNumber.Text) == false ||
                Int64.TryParse(txtMobileNumber.Text, out telNumber) == false ||
                Int64.TryParse(txtPayAmount.Text, out PayAmount) == false)
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

        #endregion CustomMethod

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible)
                {
                    StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalTopUp);
                    ClearControl();
                }
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidInputParameters())
                {
                   
                    StateManager.Instance.Current.ShowWaiting(States.PaySpeciallServicesState, txtMobileNumber.Text, txtPayAmount.Text);
                }
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtMobileNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;

                if (txtMobileNumber.IsFocused)
                {
                    BindingExpression bindingExp = txtMobileNumber.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = bindingExp.ParentBinding.Path.Path;
                }
            }
            catch (Exception EX)
            {
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtPayAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;

                if (txtPayAmount.IsFocused)
                {
                    BindingExpression bindingExpr = txtPayAmount.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = bindingExpr.ParentBinding.Path.Path;
                }
            }
            catch (Exception EX)
            { 
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtPayAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;
                //مبالغ بیشتر از 10 رقم نباشد
                if (txtPayAmount.Text.Length > 10)
                    StateManager.Instance.Current.Message2 = txtPayAmount.Text.Substring(0, 10);
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
                    StateManager.Instance.Current.Message = txtMobileNumber.Text.Substring(0,11);

                
                //btnOK.IsEnabled = IsValidInputParameters();
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Return();
        }

      
    }
}
