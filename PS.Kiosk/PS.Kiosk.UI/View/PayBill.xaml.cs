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
    /// Interaction logic for ShowPayBill.xaml
    /// </summary>
    public partial class PayBill : UserControl
    {
        /// <summary>
        /// اول شناسه قبض و سپس شناسه پرداخت باید وارد شود
        /// </summary>
        public PayBill()
        {
            InitializeComponent();
        }

        #region CustomMethod

        private void ClearControl()
        {
            txtBillID.Text = string.Empty;
            txtPayID.Text = string.Empty;
            lblPaymentAmount.Content = string.Empty;
            lblBillType.Content = string.Empty;
            lblError.Content = string.Empty;
        }

        private bool IsValidInputParameters()
        {
            string PayAmonut = Convert.ToString(lblPaymentAmount.Content).Replace(",", "");

            Int64 BillId; Int64 PayId;

            if (string.IsNullOrEmpty(txtBillID.Text) || string.IsNullOrEmpty(txtPayID.Text) || string.IsNullOrEmpty(Convert.ToString( lblPaymentAmount.Content)) ||
                Int64.TryParse(txtBillID.Text, out BillId) == false ||
                Int64.TryParse(txtPayID.Text, out PayId) == false)
                return false;
            else
                return true;
        }

        #endregion CustomMethod


        private void txtBillID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsVisible == false)
                return;

            lblBillType.Content = string.Empty;
            lblError.Content = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(txtBillID.Text))
                {

                    string strMsg;
                    lblBillType.Content = StateManager.Instance.Current.GetBillType(txtBillID.Text, out strMsg);
                    lblError.Content = strMsg;

                    if (!string.IsNullOrEmpty(Convert.ToString(lblError.Content)))
                    {
                        //btnOK.IsEnabled = false;
                        txtPayID.IsEnabled = false;
                    }
                    else
                        txtPayID.IsEnabled = true;

                }
                else
                    txtPayID.IsEnabled = false;
            }
            catch (Exception EX)
            {
                btnOK.IsEnabled = false;
                StateManager.Instance.Current.Error(EX.Message);
            }

        }

        private void txtPayID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsVisible == false)
                return;

            lblPaymentAmount.Content = string.Empty;
            lblError.Content = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(txtPayID.Text))
                {
                    lblError.Content = "شناسه پرداخت را وارد کنيد.";
                    //btnOK.IsEnabled = false;
                    return;
                }

                if (!string.IsNullOrEmpty(txtBillID.Text))
                {
                    string strMsg;
                    lblPaymentAmount.Content = StateManager.Instance.Current.GetBillPayment(txtBillID.Text, txtPayID.Text, out strMsg);

                    lblError.Content = strMsg;

                    if (Convert.ToInt32(lblPaymentAmount.Content) > 0 && string.IsNullOrEmpty(Convert.ToString(lblError.Content)))
                    {
                        lblPaymentAmount.Content = PS.Kiosk.Business.Utility.UtilMethods.GetMoneyFormat(Convert.ToString(lblPaymentAmount.Content));
                        //btnOK.IsEnabled = true;
                    }
                    //else
                    //    //btnOK.IsEnabled = false;


                }
                else
                {
                    lblError.Content = "شناسه قبض را وارد کنيد.";
                    //btnOK.IsEnabled = false;
                }
            }
            catch (Exception EX)
            {
                btnOK.IsEnabled = false;
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.Exit();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            try
            {
                ClearControl();
                if (this.IsVisible)
                    txtBillID.Focus();
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
                string PayAmonut = Convert.ToString(lblPaymentAmount.Content).Replace(",", "");
                Int64 Amount;
                if (Int64.TryParse(PayAmonut, out Amount) && Amount > 0)
                {
                    if (IsValidInputParameters())
                        StateManager.Instance.Current.ShowWaiting(States.PayBillState, txtBillID.Text, txtPayID.Text, Amount);
                    
                }
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void txtBillID_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBillID.IsFocused)
                {
                    BindingExpression bindingExpression = txtBillID.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = bindingExpression.ParentBinding.Path.Path;
                }
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }
       
        private void txtPayID_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPayID.IsFocused)
                {
                    BindingExpression bindingExpression = txtPayID.GetBindingExpression(TextBox.TextProperty);
                    StateManager.Instance.Current.BindedPropertyName = bindingExpression.ParentBinding.Path.Path;
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

       

        
    }
}
