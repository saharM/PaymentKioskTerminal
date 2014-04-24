using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using System.Reflection;
using System.Xml;
using System.Data.EntityClient;


namespace PS.Kiosk.Data.DataAccessManagement
{
    public sealed class ConnectionManager
    {
        /// <summary>
        ///SingleTon Pattern پياده سازی
        /// </summary>
        private ConnectionManager() { }

        private static readonly ConnectionManager _ConnManagerInstance = new ConnectionManager();
        public static ConnectionManager ConnManagerInstance
        {
            get { return ConnectionManager._ConnManagerInstance; }
        } 


        private static  SqlConnection _ConnInstance ;
        public static SqlConnection ConnInstance 
        {
            get
            {
                if (_ConnInstance == null)
                {
                    _ConnInstance = new SqlConnection(ConnString);
                }

                return _ConnInstance;
            }
        }

        private static string _ConnString;
        public static string ConnString
        {
            get {
                ConnectionManager._ConnString = ConnStr;
                return ConnectionManager._ConnString; }
            set
            {
                if (value != ConnectionManager._ConnString)
                    ConnInstance.ConnectionString = value;
                ConnectionManager._ConnString = value;
            }
        }

        private static string _ConnStr;
        /// <summary>
        ///  رشته اتصالی به پایگاه داده سال مالی جاری
        /// </summary>
        public static string ConnStr
        {
            get
            {
                
                string pass = DBConnection.ConnectedPassword;

                EntityConnectionStringBuilder connStringBulider = new EntityConnectionStringBuilder();
                connStringBulider.Metadata = "res://*/DataModel.ParametersDataModel.csdl|res://*/DataModel.ParametersDataModel.ssdl|res://*/DataModel.ParametersDataModel.msl";
                connStringBulider.Provider = "System.Data.SqlClient";
                connStringBulider.ProviderConnectionString =
                    new System.Data.SqlClient.SqlConnectionStringBuilder()
                    {
                        InitialCatalog = DBConnection.DbName,
                        DataSource = DBConnection.ServerName,
                        UserID = DBConnection.DBUserName,
                        Password = DBConnection.DBPassword
                    }.ConnectionString;
                
                //_ConnStr = string.Format("server={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password='{3}'",
                //DBConnection.ServerName, DBConnection.DbName, GeneralHelper.ClearPersianText(DBConnection.DBUserName), pass);

                _ConnStr = connStringBulider.ConnectionString;
                return _ConnStr;
            }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Initializer InValid Parameter");
                    else
                        _ConnStr = value;

                }
                catch (Exception Ex)
                {
                    throw Ex;
                }


            }
        }

        #region Methods

       

        #endregion Methods




    }
}
