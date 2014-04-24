using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data;

namespace PS.Kiosk.Business
{
    public class SwitchParametersBusiness
    {
        public static SignOnParameters GetSignOnParameters()
        {
            SignOnParameters signOnParameters = new SignOnParameters();
            signOnParameters.MsgType = Enums.MsgType.SignOn;
            signOnParameters.ProcessCode = Enums.ProcessCode.SignOn;
            signOnParameters.TranTime = DateTime.Now.TimeOfDay;
            signOnParameters.TranDate = DateTime.Now;

            ParametersDataAccess parametersDataAccess = new ParametersDataAccess();
            signOnParameters.Stan = parametersDataAccess.NewStan;
            signOnParameters.TranRefNumber = parametersDataAccess.NewTranRefNumber;            
            signOnParameters.BankAcceptorId = parametersDataAccess.BankAcceptorId;
            signOnParameters.IP = parametersDataAccess.IP;
            signOnParameters.Port = parametersDataAccess.Port;
            signOnParameters.TerminalAcceptorId = parametersDataAccess.TerminalAcceptorId;
            signOnParameters.TerminalAcceptorName = parametersDataAccess.TerminalAcceptorName;
            signOnParameters.CardAcceptorId = parametersDataAccess.CardAcceptorId;
            signOnParameters.CardAcceptorBinCode = parametersDataAccess.CardAcceptorBinCode;
            signOnParameters.TimeOut = parametersDataAccess.TimeOut;

            signOnParameters.DeviceCodeP25 = parametersDataAccess.DeviceCodeP25;
            signOnParameters.TerminalSerialNumberP53 = parametersDataAccess.TerminalSerialNumberP53;
            signOnParameters.MACP64 = parametersDataAccess.MACP64;
            return signOnParameters;
        }
    }
}
