using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Data.DataAccessManagement;
using PS.Kiosk.Framework.ExceptionManagement;

namespace PS.Kiosk.Data.DataAccessObjects
{
    public class ShetabErrorDAL : BaseDAL<tblShetabError>
    {
        public string getShetabError(int ErrorCode)
        {
            IQueryable<tblShetabError> error = base.SelectAll().Where(i => i.C_ShetabErrorCode_ == ErrorCode);
            if (error.Count() > 0)
                return error.First().C_ShetabErrorDes_;
            else
                throw new CustomException(ErrorCode.ToString());
        }

        public List<int> getCapturingCardError()
        {
            List<int> list = new List<int>();
            IQueryable<tblShetabError> error = base.SelectAll().Where(i => i.C_ShetabErrorDes_.Contains("ضبط")).Distinct();

            if (error != null && error.Count() > 0)
                foreach (tblShetabError item in error)
                {
                    list.Add(item.C_ShetabErrorCode_);
                }

            return list;
            
        }
    }
}
