using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;



namespace PS.Kiosk.Data.DataAccessManagement
{
    /// <summary>
    /// کلاس مربوط به اتصال به پایگاه داده
    /// </summary>
    public class DBConnection
    {
        #region Properties
        
        private static SqlConnection _ObjConnection;
        /// <summary>
        /// اتصال به پایگاه داده
        /// </summary>
        public static SqlConnection ObjConnection
        {
            get
            {
                
                _ObjConnection = ConnectionManager.ConnInstance;
                return DBConnection._ObjConnection;
            }
            set
            {
                _ObjConnection = value;
            }
        }

       private static string _ConnectedUsername;
        /// <summary>
        /// کاربر جاری در رشته اتصال به بانک
        /// </summary>
        public static string ConnectedUsername
        {
            get { return DBConnection._ConnectedUsername; }
            set { DBConnection._ConnectedUsername = value; }
        }

        private static string _ConnectedPassword;
        /// <summary>
        /// رمز عبور در رشته اتصال به بانک
        /// </summary>
        public static string ConnectedPassword
        {
            get { return DBConnection._ConnectedPassword; }
            set { DBConnection._ConnectedPassword = value; }
        }

        //*******************************

        private static string _dbName;
        /// <summary>
        /// نام بانک اطلاعاتی مربوط به سال مالی جاری
        /// </summary>
        public static string DbName
        {
            get { return DBConnection._dbName; }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Initializer InValid Parameter");
                    else
                    {
                        //if (value != DBConnection.DbName)
                        //{
                        //    DBConnection.ObjConnection = null;
                        //    DBConnection.ObjAddminConnection = null;
                        //}
                            
                        _dbName = value;
                    }

                }
                catch (Exception Ex)
                {
                    //ErrorHandler.Show(Ex);
                    throw Ex;
                }


            }
        }

        private static string _serverName;
        /// <summary>
        /// نام سرویس دهنده اطلاعاتی
        /// </summary>
        public static string ServerName
        {
            get { return DBConnection._serverName; }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Initializer InValid Parameter");
                    else
                    {
                        if (value != DBConnection.ServerName)
                            DBConnection.ObjConnection = null;
                           
                        _serverName = value;
                    }

                }
                catch (Exception Ex)
                {
                    //ErrorHandler.Show(Ex);
                    throw Ex;
                }


            }
        }

        private static string _dBUserName;
        /// <summary>
        /// نام کاربری کاربر جاری بانک اطلاعاتی
        /// </summary>
        public static string DBUserName
        {
            get { return DBConnection._dBUserName; }
            set
            { DBConnection._dBUserName = value;
            }
        }

        private static string _dBPassword;
        /// <summary>
        /// کلمه عبور کاربر جاری بانک اطلاعاتی
        /// </summary>
        public static string DBPassword
        {
            get { return DBConnection._dBPassword; }
            set
            {     
                DBConnection._dBPassword = value;
            }
        }

        private static string _serverIP;
        /// <summary>
        /// آدرس سرویس دهنده
        /// </summary>
        public static string ServerIP
        {
            get { return DBConnection._serverIP; }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Initializer InValid Parameter");
                    else
                        _serverIP = value;

                }
                catch (Exception Ex)
                {
                    //ErrorHandler.Show(Ex);
                    throw Ex;
                }


            }
        }

       

        #endregion Properties

        #region Methods


        /// <summary>
        /// اتصال به پایگاه داده در صورت بسته بودن را باز می کند
        /// </summary>
        public static void OpenConnection()
        {
            
            try
            {
                if (ObjConnection.State != ConnectionState.Open)
                    ObjConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("UnableToConnectDB");
            }
        }

        /// <summary>
        /// اتصال به پایگاه داده در صورت باز بودن را می بندد
        /// </summary>
        public static void CloseConnection()
        {
            try
            {
                if (ObjConnection.State != ConnectionState.Closed)
                    ObjConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("UnableToConnectDB");
            }
           
        }

        #endregion Methods
    }
}
