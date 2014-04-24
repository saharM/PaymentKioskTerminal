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
    /// Interaction logic for GetPin.xaml
    /// </summary>
    public partial class GetPin : UserControl
    {
        public GetPin()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new SimpleDelegate(DoFocus));
        }
        private delegate void SimpleDelegate();
        
        void DoFocus()
        {
            bool a = txtPin.Focus();
        }

       
    }
}
