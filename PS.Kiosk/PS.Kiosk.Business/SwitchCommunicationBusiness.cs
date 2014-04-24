using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PS.Kiosk.Messaging.MsgCall;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data;

namespace PS.Kiosk.Business
{
    public class SwitchCommunicationBusiness
    {
        public static bool SignOnToSwitch(KioskModel thisKiosk)
        {            
            Transactions transations = new Transactions();
            SignOnReplyParameters signOnReplyParameters = transations.SignOn(SwitchParametersBusiness.GetSignOnParameters());
            (new TransactionsDataAccess()).InsertNewTransaction(signOnReplyParameters);
            //if (signOnReplyParameters.)
            //{
                
            //}
            return true;
        }
    }
}
