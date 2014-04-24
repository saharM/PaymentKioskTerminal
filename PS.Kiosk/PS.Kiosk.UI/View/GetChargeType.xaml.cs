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
using System.Windows.Shapes;
using PS.Kiosk.Framework;

namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for GetChargeType.xaml
    /// </summary>
    public partial class GetChargeType : UserControl
    {
        public GetChargeType()
        {
            InitializeComponent();
        }

        private void btnIrancell_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowIrancellChargeType();
        }

        private void btnHamrahAval_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowHamrahAvalChargeType();
        }

        private void btnTalia_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowTaliaChargeType();
        }

        private void btnRightel_Click(object sender, RoutedEventArgs e)
        {
            StateManager.Instance.Current.ShowRightelChargeType();
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
