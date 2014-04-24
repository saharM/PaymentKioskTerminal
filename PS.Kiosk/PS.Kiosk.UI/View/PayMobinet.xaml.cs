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
    //برقص من باتو بیدارم،نترس تنهات نمیزارم،بمون ،از عشق و با من بخون

    /// <summary>
    /// Interaction logic for PayIrancellServices.xaml
    /// </summary>
    public partial class PayMobinet : UserControl
    {
        public PayMobinet()
        {
            InitializeComponent();
        }

        #region CustomMethod

        private void ClearControl()
        {
            txtMobinetNumber.Text = string.Empty;
            txtPayAmount.Text = string.Empty;
            lblError.Visibility = System.Windows.Visibility.Hidden;
        }

        private bool IsValidInputParameters()
        {
            string PayAmonut = Convert.ToString(txtPayAmount.Text).Replace(",", "");

            Int64 MobileNumber; Int64 PayId; Int64 MobinetId;

            if (string.IsNullOrEmpty(txtMobileNumber.Text) || string.IsNullOrEmpty(txtPayAmount.Text) || string.IsNullOrEmpty(txtMobinetNumber.Text) ||
                Int64.TryParse(txtMobileNumber.Text, out MobileNumber) == false || Int64.TryParse(txtMobinetNumber.Text, out MobinetId) == false ||
                Int64.TryParse(txtPayAmount.Text, out PayId) == false)
            {
                lblError.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
            {
                lblError.Visibility = System.Windows.Visibility.Hidden;
                if (PS.Kiosk.Business.Utility.UtilMethods.IsValidMobinNetId(txtMobinetNumber.Text) == false)
                {
                    lblError.Visibility = System.Windows.Visibility.Visible;
                    return false;
                }
                else
                {
                    lblError.Visibility = System.Windows.Visibility.Hidden;
                    if (PS.Kiosk.Business.Utility.UtilMethods.IsValidMobileNumber(txtMobileNumber.Text))
                    {
                        lblError2.Visibility = System.Windows.Visibility.Hidden;
                        return true;
                    }
                    else
                    {
                        lblError2.Visibility = System.Windows.Visibility.Visible;
                        return false;
                    }
                }

            }
        }

        #endregion CustomMethod

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidInputParameters())
                {
                    StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.Mobinnet);
                    StateManager.Instance.Current.ShowWaiting(States.PaySpeciallServicesState, txtMobileNumber.Text, txtPayAmount.Text , txtMobinetNumber.Text);
                }
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void txtMobinetNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;

                if (txtMobinetNumber.IsFocused)
                {
                    BindingExpression BindingExpr = txtMobinetNumber.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = BindingExpr.ParentBinding.Path.Path;
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
                    BindingExpression BindingExpr = txtPayAmount.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = BindingExpr.ParentBinding.Path.Path;
                }
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtMobinetNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;
                if (txtMobinetNumber.Text.Length > 15)
                    StateManager.Instance.Current.Message = txtMobinetNumber.Text.Substring(0, 15);

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

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible)
                    ClearControl();
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
                    StateManager.Instance.Current.Message3 = txtMobileNumber.Text.Substring(0, 11);

                //btnOK.IsEnabled = IsValidInputParameters();

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
                BindingExpression BindingExpr = txtMobileNumber.GetBindingExpression(TextBox.TextProperty);
                StateManager.Instance.Current.BindedPropertyName = BindingExpr.ParentBinding.Path.Path;
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        
    }
}
