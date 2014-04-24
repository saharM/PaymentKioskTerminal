using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using PS.Kiosk.UI.DataAccess;
using PS.Kiosk.UI.Properties;
using System.Threading;

namespace PS.Kiosk.UI.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Properties
        private string _viewState;
        public string ViewState
        {
            get
            {
                return this._viewState;
            }
            set
            {
                if (value != "ChangeState")
                {
                    //Thread.Sleep(1000);
                    App.viewModel.ViewState = "ChangeState";
                }
                //Thread.Sleep(1000);
                this._viewState = value;
                base.OnPropertyChanged("ViewState");
            }
        }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get
            {
                return _ErrorMsg;
            }
            set
            {
                _ErrorMsg = value;
                base.OnPropertyChanged("ErrorMsg");
            }
        }

        private string _Message;
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                this._Message = value;
                base.OnPropertyChanged("Message");
            }
        }

        private string _Message2;

        public string Message2
        {
            get { return _Message2; }
            set
            {
                _Message2 = value;
                base.OnPropertyChanged("Message2");
            }
        }

        private string _Message3;

        public string Message3
        {
            get { return _Message3; }
            set
            {
                _Message3 = value;
                base.OnPropertyChanged("Message3");
            }
        }

        private bool _EnableControl;

        public bool EnableControl
        {
            get { return _EnableControl; }
            set { _EnableControl = value;
            base.OnPropertyChanged("EnableControl");
            }
        }
        #endregion

        #region Fields

        readonly KioskRepository _kioskRepository;
        ObservableCollection<WorkspaceViewModel> _workspaces;

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel()
        {
            base.DisplayName = Strings.MainWindowViewModel_DisplayName;

            _kioskRepository = new KioskRepository();
        }

        #endregion // Constructor

        #region Commands        

        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers

        private void ChangeState()
        {
            //this.ViewState = "WorkspaceState";
        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion // Private Helpers
    }
}
