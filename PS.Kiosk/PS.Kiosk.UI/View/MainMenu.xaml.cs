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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnGetBalanceInquiry_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan t1 = DateTime.Now.TimeOfDay;
            StateManager.Instance.Current.ShowWaiting(States.GetBalanceInquiryActionState,null);
            TimeSpan t2 = DateTime.Now.TimeOfDay;

            Console.WriteLine( "\n"+"TotalUI = " +  (t2 - t1).ToString() +"\n");
        }

        private void btnGetChargeType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowChargeType();
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
                StateManager.Instance.Current.FinishSession();
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnPayBill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowBusy(States.ShowPayBillState);
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnPayJiring_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowHamrahAvalServices();
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnIrancellServices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowIrancellServices();

                //SharedValue.AppMutex.ReleaseMutex();
                //Application.Current.Shutdown();
                //System.Windows.Forms.Application.Restart();
            }
            catch (Exception EX)
            {
                
                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnMobinetWimax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.ShowBusy(States.ShowMobinetPaymentState);
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
                    if (!StateManager.Instance.Current.CheckPrinterStatus())
                        throw new Exception("Printer is UnAvailable2");

                }
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void ChangeEnability(bool MakeEnable)
        {
            btnIrancellServices.IsEnabled = MakeEnable;
            btnIrancellServices.Background = new SolidColorBrush(Colors.Transparent);
            btnMobinetWimax.IsEnabled = MakeEnable;
            btnPayJiring.IsEnabled = MakeEnable;
            btnPayBill.IsEnabled = MakeEnable;
            btnGetChargeType.IsEnabled = MakeEnable;
           
        }
    }
}
