using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PS.Kiosk.Business.BusinessObjects
{
    /// <summary>
    /// BR کلاس پايه در لايه
    /// </summary>
    /// <typeparam name="TBrClass">کلاس  مورد نظر در لايه اشتراک برای نگهداری خروجی</typeparam>
    /// <typeparam name="TDalClass">کلاس  مورد نظر در لايه داده  </typeparam>
    internal abstract class BaseBR<TDalClass> 
        where TDalClass : class, new()
        
    {
        

        private TDalClass _DalInstance;
        /// <summary>
        /// نمونه ای از کلاس منتسب شده به این کلاس از لایه داده
        /// </summary>
        public  TDalClass DalInstance
        {
            get
            {
                if (_DalInstance == null)
                    _DalInstance = new TDalClass();
                return _DalInstance;
            }
          
        }

      
        
        
    }
}
