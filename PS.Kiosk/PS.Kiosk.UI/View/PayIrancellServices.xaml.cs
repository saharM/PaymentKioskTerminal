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
    /// Interaction logic for PayIrancellServices.xaml
    /// </summary>
    public partial class PayIrancellServices : UserControl
    {
        public PayIrancellServices()
        {
            InitializeComponent();
        }

        #region CustomMethod

        private void ClearControl()
        {
            txtMobileNumber.Text = string.Empty;
            txtPayAmount.Text = string.Empty;
            lblError.Visibility = System.Windows.Visibility.Hidden;
        }

        private bool IsValidInputParameters()
        {
            string PayAmonut = Convert.ToString(txtPayAmount.Text).Replace(",", "");

            Int64 MobileNumber; Int64 PayId;

            if (string.IsNullOrEmpty(txtMobileNumber.Text) || string.IsNullOrEmpty(txtPayAmount.Text) ||
                Int64.TryParse(txtMobileNumber.Text, out MobileNumber) == false ||
                Int64.TryParse(txtPayAmount.Text, out PayId) == false)
            {
                lblError.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
            {
                lblError.Visibility = System.Windows.Visibility.Hidden;

                //برای اين سرويس ها بايد شماره موبايل چک شود
                if (StateManager.Instance.Current.PropertyDic["SpecialServiceType"].ToString() == Convert.ToString((int)Enums.SpecialServiceType.IrancellTopUp) ||
                    StateManager.Instance.Current.PropertyDic["SpecialServiceType"].ToString() == Convert.ToString((int)Enums.SpecialServiceType.IrancellBill))
                {
                    if (PS.Kiosk.Business.Utility.UtilMethods.IsValidMobileNumber(txtMobileNumber.Text) == false ||
                        PS.Kiosk.Business.Utility.UtilMethods.IsValidIrancellNumber(txtMobileNumber.Text) == false)
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
                else
                {
                    if (StateManager.Instance.Current.PropertyDic["SpecialServiceType"].ToString() == Convert.ToString((int)Enums.SpecialServiceType.IrancellWimax))
                    {
                        return PS.Kiosk.Business.Utility.UtilMethods.IsValidIrancellWimax(txtMobileNumber.Text);
                    }
                    else
                        return true;

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
                    BindingExpression BindingExpr = txtMobileNumber.GetBindingExpression(TextBox.TextProperty);
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

        private void txtMobileNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.IsVisible == false)
                    return;

                //شماره تلفن همرا بیشتر از 11 رقم نباید باشد
                if (txtMobileNumber.Text.Length > 11)
                    StateManager.Instance.Current.Message = txtMobileNumber.Text.Substring(0, 11);


                //btnOK.IsEnabled = IsValidInputParameters();

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
                {
                    ImageBrush Img = new ImageBrush();
                    Image image = new Image();
                    

                    string ServiceType = Convert.ToString(StateManager.Instance.Current.PropertyDic["SpecialServiceType"]);

                    if ((Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)) == Enums.SpecialServiceType.IrancellTopUp)
                        image.Source = new BitmapImage(new Uri(System.Windows.Forms.Application.StartupPath +"\\Resources\\Irancell - Sharje mostaghim.jpg"));
                    
                    if ((Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)) == Enums.SpecialServiceType.IrancellBill)
                        image.Source = new BitmapImage(new Uri(System.Windows.Forms.Application.StartupPath + "\\Resources\\Irancell - Pardakhte Ghabz.jpg"));
                       
                    if ((Enums.SpecialServiceType)(Convert.ToInt32(ServiceType)) == Enums.SpecialServiceType.IrancellWimax)
                        image.Source = new BitmapImage(new Uri(System.Windows.Forms.Application.StartupPath + "\\Resources\\Irancell - Vaimax.jpg"));

                    Img.ImageSource = image.Source;
                    CurrentGrid.Background = Img;

                    ClearControl();
                }
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
