using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Data.DataAccessObjects;

namespace PS.Kiosk.Business.BusinessObjects
{
    class ShetabErrorBR : BaseBR<ShetabErrorDAL>
    {
        private ShetabErrorBR()
        {

        }

        static ShetabErrorBR _BRShetErrorInstance;
        internal static ShetabErrorBR BRShetErrorInstance
        {
            get
            {
                if (_BRShetErrorInstance == null)
                    _BRShetErrorInstance = new ShetabErrorBR();
                return _BRShetErrorInstance;
            }
            
        }

        public string getShetabError(int ErrorCode)
        {
            return DalInstance.getShetabError(ErrorCode);
        }

        public List<int> getCapturingCardError()
        {
            return DalInstance.getCapturingCardError();
        }
    }
}
