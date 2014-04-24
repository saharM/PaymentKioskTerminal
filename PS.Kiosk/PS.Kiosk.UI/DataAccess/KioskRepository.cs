using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using PS.Kiosk.Business;
using PS.Kiosk.Framework;

namespace PS.Kiosk.UI.DataAccess
{
    /// <summary>
    /// Represents a source of customers in the application.
    /// </summary>
    public class KioskRepository
    {

        #region Constructor

        /// <summary>
        /// Creates a new repository of customers.
        /// </summary>
        /// <param name="customerDataFile">The relative path to an XML resource file that contains customer data.</param>
        public KioskRepository()
        {
            if (StateManager.Start(new KioskBusiness()))
            {
                StateManager.Instance.Run();
            }            
            StateManager.Instance.Current.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(KioskRepository_KiokStateManagerPropertyChanged);
        }

        void KioskRepository_KiokStateManagerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentState":
                    App.viewModel.ViewState = StateManager.Instance.Current.CurrentState.ToString();
                    break;
                case "ErrorMsg":
                    App.viewModel.ErrorMsg = StateManager.Instance.Current.ErrorMsg;
                    break;
                case "Message":
                    App.viewModel.Message = StateManager.Instance.Current.Message.ToString();
                    break;
                case "Message2":
                    App.viewModel.Message2 = StateManager.Instance.Current.Message2.ToString();
                    break;
                case "Message3":
                    App.viewModel.Message3 = StateManager.Instance.Current.Message3.ToString();
                    break;
                case "EnableControl":
                    App.viewModel.EnableControl = StateManager.Instance.Current.EnableControl;
                    break;
                default:
                    break;
            }
            
        }

        #endregion // Constructor

        #region Private Helpers

        static Stream GetResourceStream(string resourceFile)
        {
            Uri uri = new Uri(resourceFile, UriKind.RelativeOrAbsolute);

            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info == null || info.Stream == null)
                throw new ApplicationException("Missing resource file: " + resourceFile);

            return info.Stream;
        }

        #endregion // Private Helpers
    }
}
