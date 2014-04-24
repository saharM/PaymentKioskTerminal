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
using System.Windows.Forms.Integration;
using System.Reflection;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.UI.View
{
    /// <summary>
    /// Interaction logic for ShowBusy.xaml
    /// </summary>
    public partial class ShowBusy : UserControl
    {
        public ShowBusy()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                WindowsFormsHost host = new WindowsFormsHost();
                host.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                host.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                Player player = new Player();
                if (this.IsVisible)
                {
                    //the Windows Forms Host hosts the Flash Player
                    host.Child = player;

                    //the WPF Grid hosts the Windows Forms Host
                    FlashPlayerGrid.Children.Add(host);
                    string path = System.Windows.Forms.Application.StartupPath + "\\Resources\\" + "loader.swf";
                    player.LoadMovie(path);

                    player.Play();
                }
                else
                    player.Stop();



              
            }
        
        }
    }
}
