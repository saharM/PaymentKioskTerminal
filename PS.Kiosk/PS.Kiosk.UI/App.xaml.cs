using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using PS.Kiosk.UI.ViewModel;
using System.Threading;
using PS.Kiosk.Framework;
using PS.Kiosk.Common.Model;

namespace PS.Kiosk.UI
{
   

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        public static MainWindowViewModel viewModel;
        protected override void OnStartup(StartupEventArgs e)
        {
            SharedValue.AppMutex = new Mutex(false, "mutexName");

            try
            {
                if (!SharedValue.AppMutex.WaitOne(100, false))
                {
                    System.Windows.Forms.MessageBox.Show(".برنامه کیوسک هم اکنون باز است و يا در حال بارگذاری می باشد");
                    Application.Current.Shutdown();
                }
                else
                {
                    base.OnStartup(e);

                    MainWindow window = new MainWindow();
                    //PlayerWindow window = new PlayerWindow();
                    // Create the ViewModel to which 
                    // the main window binds.            
                    viewModel = new MainWindowViewModel();

                    // When the ViewModel asks to be closed, 
                    // close the window.
                    EventHandler handler = null;
                    handler = delegate
                    {
                        viewModel.RequestClose -= handler;
                        window.Close();
                    };
                    viewModel.RequestClose += handler;

                    // Allow all controls in the window to 
                    // bind to the ViewModel by setting the 
                    // DataContext, which propagates down 
                    // the element tree.
                    window.DataContext = viewModel;

                    window.Show();

                   
                }
            }
            catch (Exception EX)
            {
                KioskLogger.Instance.LogMessage(EX, "My Kiosk Unhandled Exception");
                SharedValue.AppMutex.ReleaseMutex();
                Application.Current.Shutdown();
            }
        }

    }
}
