using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.DeviceController.Printers;

namespace PS.Kiosk.DeviceController.Services
{
    public class PrinterService
    {
        #region PublicMethods

       
       

        public static bool IsAvailable()
        {
            return new Print().ChekStatus();
        }

        #endregion PublicMethods
    }
}
