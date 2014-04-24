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
            try
            {
                Transactions transations = new Transactions();
                SignOnReplyParameters signOnReplyParameters = transations.SignOn(SwitchParametersBusiness.GetSignOnParameters());
                return signOnReplyParameters.TranSuccess;
            }
            catch (Exception ex)
            {
                return true;
                throw ex;
            }

            //(new TransactionsDataAccess()).InsertNewTransaction(signOnReplyParameters);
            //if (signOnReplyParameters.)
            //{

            //}

        }

        public static bool Reversal(KioskModel thisKiosk)
        {
            Transactions transations = new Transactions();
            ReversalReplyParameters reversalReplyParameters = transations.Reversal(SwitchParametersBusiness.GetReversalParameters());

            //(new TransactionsDataAccess()).InsertNewTransaction(signOnReplyParameters);
            //if (reversalReplyParameters.)
            //{

            //}
            return true;
        }
    }
}
