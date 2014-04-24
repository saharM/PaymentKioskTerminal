using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
//using System.Linq.Dynamic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using PS.Kiosk.Framework;




namespace PS.Kiosk.Data.DataAccessManagement
{
    [System.ComponentModel.DataObject]
    public abstract class BaseDAL<TEntity> //, Common.DBSchema.Entities> 
       // where TObjectContext : Entities, new() 
        where TEntity : class , new()
    {

        public BaseDAL()
        {
            
                DBConnection.ServerName = PS.Kiosk.Data.Properties.Settings.Default.Server;
                DBConnection.DbName = PS.Kiosk.Data.Properties.Settings.Default.DB;
                DBConnection.DBUserName = PS.Kiosk.Data.Properties.Settings.Default.UserName;
                DBConnection.DBPassword = PS.Kiosk.Data.Properties.Settings.Default.Password;
            
        }

        #region Properties

        private string _ObjectContextKey;
        protected string ObjectContextKey
        {
            get
            {
                if (_ObjectContextKey == null)
                {
                    // Include the type name of the context.
                    // Results in "dataContext_MyNamespace.TableDataContext" for BaseDAL<MyTableObject, MyNamespace.TableDataContext>.SelectAll()
                    _ObjectContextKey = "objectContext_" + typeof(KioskDataEntities).FullName;
                }
                return _ObjectContextKey;
            }
        }

        public KioskDataEntities ObjectContext = new KioskDataEntities();

        //private  KioskDataEntities _ObjectContext;
        //public  KioskDataEntities ObjectContext
        //{
        //    get
        //    {
                
        //       if (_ObjectContext == null)
        //        {
        //        KioskDataEntities Dc = new KioskDataEntities();//(ConnectionManager.ConnStr);
        //            //Dc.Refresh(System.Data.Objects.RefreshMode.StoreWins, Dc.ObjectStateManager.GetObjectStateEntries(EntityState.Modified));
        //            _ObjectContext = Dc;//ObjectContextFactory<TObjectContext>.ObjectContext;

        //        }

               
        //        return _ObjectContext;


        //    }
        //    set
        //    {
        //        //if (value == null)
        //        //{
        //        //    ObjectContextFactory<TObjectContext>.DataContextDisposing();
        //        //}

        //        _ObjectContext = value;
        //    }
            
        //}

        //private readonly GenericDataSection config = ConfigurationManager.GetSection("GenericDataSection") as GenericDataSection;

        //protected TObjectContext CreateDataContext()
        //{
        //    return ObjectContextFactory<TObjectContext>.ObjectContext;
            
        //    //if (config != null)
        //    //{
        //    //    dataContext.Log = config.Logger;
        //    //}
           
        //}

        private TEntity _TEntityInstance;
        public TEntity TEntityInstance
        {
            get
            {

                //if (_TEntityInstance == null)
                    _TEntityInstance = new TEntity();


                return _TEntityInstance;
            }
            
        }

        private TEntity _TEntityObject;
        public TEntity TEntityObject
        {
            get
            {
                _TEntityObject = ObjectContext.CreateObject<TEntity>();
                return _TEntityObject;
            }
            //set { _TEntityTable = value; }
        }

        private List<TEntity> _TEntityList;
        public List<TEntity> TEntityList
        {
            get {
                
                return _TEntityList; }
            set { _TEntityList = value; }
        }

        public void SubmitChanges()
        {
            ObjectContext.SaveChanges();
        }

        public ObjectContextOptions LoadOptions
        {
            get
            {
                return ObjectContext.ContextOptions;
            }
            
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// در این مشخصه کلید اصلی جدول نگهداری می شود
        /// </summary>
        private MetaDataMember _metaPrimaryKey;
        private MetaDataMember MetaPrimaryKey
        {
            get
            {
                //if (_metaPrimaryKey == null)
                //{
                    
                //    Type entityType = typeof(TEntity);
                    
                //    MetaTable mapping = ObjectContext.Mapping.GetTable(entityType);

                //    int count = mapping.RowType.DataMembers.Count(d => d.IsPrimaryKey);
                //    if (count < 1) throw new Exception(String.Format("Table {0} does not contain a Primary Key field", mapping.TableName));
                //    if (count > 1) throw new Exception(String.Format("Table {0} contains a composite primary key field", mapping.TableName));

                //    _metaPrimaryKey = mapping.RowType.DataMembers.Single(d => d.IsPrimaryKey);
                //}

                
                return _metaPrimaryKey;
            }
        }

        /// <summary>
        /// در این مشخصه کلید اصلی کلاس نگهداری می شود
        /// </summary>
        private PropertyInfo _primaryKey;
        public PropertyInfo PrimaryKey
        {
            get
            {
                if (_primaryKey == null)
                {
                    _primaryKey = typeof(TEntity).GetProperty(PrimaryKeyName);
                }
                return _primaryKey;
            }
        }

        /// <summary>
        /// در این مشخصه کلید اصلی کلاس نگهداری می شود
        /// </summary>
        private string _primaryKeyName;
        public string PrimaryKeyName
        {
            get
            {
                if (string.IsNullOrEmpty(_primaryKeyName))
                {
                    _primaryKeyName = MetaPrimaryKey.Name;
                }

                return _primaryKeyName;
            }
        }

        
        protected object GetPrimaryKeyValue(TEntity entity)
        {
            return PrimaryKey.GetValue(entity, null);
        }

        protected TEntity GetEntity(TEntity entity)
        {
            return GetEntity(GetPrimaryKeyValue(entity));
        }

        private List<PropertyInfo> _databaseProperties;
        protected List<PropertyInfo> DatabaseProperties
        {
            get
            {
                if (_databaseProperties == null)
                {
                    //Type entityType = typeof (TEntity);
                    //MetaTable mapping = ObjectContext.Mapping.GetTable(typeof(TEntity));
                    //_databaseProperties = mapping.RowType.DataMembers
                    //    .Where(x => x.DbType != null)
                    //    .Select(x => entityType.GetProperty(x.Name))
                    //    .ToList();
                }
                return _databaseProperties;
            }
        }

        /// <summary>
        /// Update all properties of a linq object that are not associations with the values in another linq object.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        protected void UpdateOriginalFromChanged(ref TEntity destination, TEntity source)
        {
            foreach (PropertyInfo pi in DatabaseProperties)
            {
                pi.SetValue(destination, pi.GetValue(source, null), null);
            }
        }

        protected void DataContextDisposing()
        {
            ObjectContext = null;
           
        }

        protected void DataContextRejectChanges()
        {
            
            //foreach (object item in ObjectContext.GetChangeSet().Updates)
            //{
            //    if (item is TEntity)
            //        ObjectContext.Refresh(RefreshMode.OverwriteCurrentValues, item);
                
            //}
            /*
            foreach (object item in DataContext.GetChangeSet().Deletes)
            {
                if (item is TEntity)
                {
                    PropertyInfo[] properties = typeof(TEntity).GetProperties();
                    foreach (PropertyInfo prop in properties)
                    {
                        foreach (AssociationAttribute Attr in prop.GetCustomAttributes(typeof(AssociationAttribute), false))
                        {
                            //Attr.GetType().GetProperty(item.ToString()).SetValue(DataContext.GetTable<typeof(Attr)>(),null,null);
                            
                        }
                    }
                    //   DataContext.Refresh(RefreshMode.OverwriteCurrentValues, item);
                }

            }*/

            //var changeSet = DataContext.GetChangeSet();
            //var inserts = changeSet.Inserts.ToList();
            //var deleteAndUpdate = changeSet.Deletes.Concat(changeSet.Updates).ToList();

            //foreach (var inserted in inserts)
            //{
            //    var table = DataContext.GetTable(inserted.GetType());
            //    table.DeleteOnSubmit(inserted);
            //}

            //if (deleteAndUpdate.Count > 0)
            //{
            //    DataContext.Refresh(RefreshMode.OverwriteCurrentValues, deleteAndUpdate);
            //}

          
        }

        #endregion

        #region Generic CRUD methods
        //---------------------Selects----------------------------------
        
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected TEntity GetEntity(object id)
        {
            //MetaDataMember primaryKey = MetaPrimaryKey;

            //Type entityType = typeof(TEntity);
            //ParameterExpression param = Expression.Parameter(entityType, "e");
            //MemberExpression property = Expression.Property(param, primaryKey.Name);
            //ConstantExpression value = Expression.Constant(Convert.ChangeType(id, primaryKey.Type), primaryKey.Type);

            //Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property, value), param);

            
            
            //return ObjectContext.GetTable<TEntity>().SingleOrDefault(predicate);
            return TEntityInstance;
            
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected IQueryable<TEntity> SelectAll()
        {
            try
            {
                if (ObjectContext.Connection.State != ConnectionState.Open)
                    ObjectContext.Connection.Open();

                return ObjectContext.CreateObjectSet<TEntity>().AsQueryable<TEntity>();
            }
            catch (Exception EX)
            {

                throw EX;
            }
            finally { if (ObjectContext.Connection.State != ConnectionState.Closed)  ObjectContext.Connection.Close(); } //ObjectContext.Dispose(); }
        }
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected IQueryable<TEntity> SelectAll(string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
            {
                return SelectAll();
            }
            return SelectAll();//.OrderBy(sortExpression);
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected IQueryable<TEntity> SelectAll(string sortExpression, int maximumRows, int startRowIndex)
        {
            return SelectAll(sortExpression).Skip(startRowIndex).Take(maximumRows);
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected IQueryable<TEntity> SelectAll(int maximumRows, int startRowIndex)
        {
            return SelectAll().Skip(startRowIndex).Take(maximumRows);
        }

        public int Count()
        {
            return SelectAll().Count();
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        protected List<TEntity> SelectAllAsList()
        {
            return SelectAll().ToList();
        }

        
        //----------------------Insert------------------------------------
        /// <summary>
        /// موجودیت مورد نظر را ثبت می کند
        /// </summary>
        /// <param name="entity">موجودیت</param>
        /// <returns>کلید اصلی اطلاعات ثبت شده</returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Insert)]
        protected int Insert(TEntity entity)
        {
           return Insert(entity, true);
        }

        /// <summary>
        /// موجودیت مورد نظر را ثبت می کند
        /// </summary>
        /// <param name="entity">موجودیت</param>
        /// <param name="submitChanges">وارد پایگاه داده شود یا خیر</param>
        /// <returns>کلید اصلی اطلاعات ثبت شده</returns>
        protected int Insert(TEntity entity, bool submitChanges)
        {
             //ObjectContext.log = Console.Out;
            //ObjectContext.GetTable<TEntity>().InsertOnSubmit(entity);

            //KioskDataEntities.tblTransactions.AddObject(newTran);
            //KioskDataEntities.SaveChanges();

            ObjectContext.AddObject(entity.GetType().Name, entity);
            if (submitChanges)
            {
                ObjectContext.SaveChanges();
                ObjectContext.AcceptAllChanges();
                KioskLogger.Instance.LogMessage("Insert entity = " + entity.ToString());
            }

            //if (submitChanges)
            //{
            //    ObjectContext.SubmitChanges();
            //    ObjectContext.Refresh(RefreshMode.KeepChanges, entity);
            //}
            //Int32 PrimaryKey = Convert.ToInt32(GetPrimaryKeyValue(entity));

            
            return 0;
            
        }

        protected void Insert(List<TEntity> list)
        {
            Insert(list, true);
        }

        protected void Insert(List<TEntity> list, bool submitChanges)
        {
            
            //ObjectContext.GetTable<TEntity>().InsertAllOnSubmit(list);
            //if (submitChanges)
            //{
            //    ObjectContext.SubmitChanges();
            //    ObjectContext.Refresh(RefreshMode.KeepChanges, list);
            //}
        }
        //-----------------------Update-----------------------------------------
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Update)]
        public void Update()
        {
            
            Update(null, true);
           
        }

        protected void Update(TEntity entity)
        {
           
            Update(entity, true);
            
        }

        protected void Update(TEntity entity, bool submitChanges)
        {
           // ObjectContext.Log = Console.Out;
            if (entity != null)
            {
                TEntity original = GetEntity(entity);
                UpdateOriginalFromChanged(ref original, entity);
            }
            //if (submitChanges)
            //{
            //   ObjectContext.SubmitChanges();
            //   if (entity != null)
            //       ObjectContext.Refresh(RefreshMode.KeepChanges, entity);
            //}
        }

        protected void Update(List<TEntity> list)
        {
            Insert(list);
        }

        //----------------------Delete-------------------------------------------
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Delete)]
        protected void Delete(TEntity entity)
        {
            Delete(entity, true);
        }

        protected void Delete(TEntity entity, bool submitChanges)
        {
            ObjectContext.DeleteObject(entity);
            if (submitChanges)
                ObjectContext.SaveChanges();
            
        }

        protected void Delete(List<TEntity> list)
        {
            Delete(list, true);
        }

        protected void Delete(List<TEntity> list, bool submitChanges)
        {
            
            //ObjectContext.GetTable<TEntity>().DeleteAllOnSubmit<TEntity>(list);

            //if (submitChanges)
            //{
            //    ObjectContext.SubmitChanges();
            //    //DataContext.Refresh(RefreshMode.KeepChanges, list);
            //}
            
        }

        #endregion


        


    }
}