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
    /// Interaction logic for ShowHamrahAvalServices.xaml
    /// </summary>
    public partial class ShowHamrahAvalServices : UserControl
    {
        public ShowHamrahAvalServices()
        {
            InitializeComponent();
        }

        private void btnJiring_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.JiringCharge);
                StateManager.Instance.Current.ShowBusy(States.ShowJiringState);
            }
            catch (Exception EX)
            {

                StateManager.Instance.Current.Error(EX.Message);
            }
        }

        private void btnTopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalTopUp);
                StateManager.Instance.Current.ShowBusy(States.ShowHamrahAvalTopUpState);
                
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
                StateManager.Instance.Current.AddToDictionary("SpecialServiceType", (int)Enums.SpecialServiceType.HamrahAvalMidTermInfo);
                StateManager.Instance.Current.ShowBusy(States.ShowHamrahAvalBillState);

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
