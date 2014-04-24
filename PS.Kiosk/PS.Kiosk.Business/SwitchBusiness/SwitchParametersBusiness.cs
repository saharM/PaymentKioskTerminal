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
        private static BaseParameters GetBaseParameters(ParametersFactory factory)
        {
            BaseParameters ParamObject = factory.GetParameters();
            
            ParamObject.TranTime = DateTime.Now.TimeOfDay;
            ParamObject.TranDate = DateTime.Now;

            ParametersDataAccess parametersDataAccess = new ParametersDataAccess();
            ParamObject.Stan = parametersDataAccess.NewStan;
            ParamObject.TranRefNumber = parametersDataAccess.NewTranRefNumber;
            ParamObject.BankAcceptorId = parametersDataAccess.BankAcceptorId;
            ParamObject.IP = parametersDataAccess.IP;
            ParamObject.Port = parametersDataAccess.Port;
            ParamObject.TerminalAcceptorId = parametersDataAccess.TerminalAcceptorId;
            ParamObject.TerminalAcceptorName = parametersDataAccess.TerminalAcceptorName;
            ParamObject.CardAcceptorId = parametersDataAccess.CardAcceptorId;
            ParamObject.CardAcceptorBinCode = parametersDataAccess.CardAcceptorBinCode;
            ParamObject.TimeOut = parametersDataAccess.TimeOut;
            return ParamObject;
        }

        public static SignOnParameters GetSignOnParameters()
        {
            GenericFactory<SignOnParameters> factory = new GenericFactory<SignOnParameters>();
            SignOnParameters signOnParameters = (SignOnParameters)GetBaseParameters(factory);

            signOnParameters.MsgType = Enums.MsgType.SignOn;
            signOnParameters.ProcessCode = Enums.ProcessCode.SignOn;
            signOnParameters.DeviceCodeP25 = ParametersDataAccess.GetInstance().DeviceCodeP25;
            signOnParameters.TerminalSerialNumberP53 = ParametersDataAccess.GetInstance().TerminalSerialNumberP53;
            signOnParameters.MACP64 = ParametersDataAccess.GetInstance().MACP64;
            return signOnParameters;
        }

        public static ReversalParameters GetReversalParameters()
        {
            GenericFactory<ReversalParameters> factory = new GenericFactory<ReversalParameters>();
            ReversalParameters reversalParameters = (ReversalParameters)GetBaseParameters(factory);
            return reversalParameters;
        }
    }
}
