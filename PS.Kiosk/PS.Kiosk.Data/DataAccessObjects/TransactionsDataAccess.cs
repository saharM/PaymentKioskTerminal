using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data.DataAccessManagement;
using PS.Kiosk.Framework;




namespace PS.Kiosk.Data.DataAccessObjects
{
    public class TransactionsDataAccess : BaseDAL<tblTransactions>
    {
        private TransactionsDataAccess()
        {

        }

        private static TransactionsDataAccess _Instance;
        public static TransactionsDataAccess Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TransactionsDataAccess();
                return TransactionsDataAccess._Instance;
            }

        }

        public Int64 InsertNewTransaction(BaseParameters tranParams)
        {
            try
            {
                if (tranParams == null)
                    return -1;

                tblTransactions newTran = base.TEntityObject;
                newTran.MessageType = tranParams.MsgType.ToString();
                newTran.RefrenceNumber = tranParams.TranRefNumber.ToString();
                newTran.Stan = tranParams.Stan.ToString();

                newTran.SendDateTime = tranParams.TranDate;

                newTran.ID = ParametersDataAccess.Instance.NewReqID;
                base.Insert(newTran);

                PS.Kiosk.Framework.KioskLogger.Instance.LogMessage("Should Insert TranId =" + newTran.ID.ToString() + "Successfully");

                return newTran.ID;
            }
            catch (Exception EX)
            {

                PS.Kiosk.Framework.KioskLogger.Instance.LogMessage(EX, "");
            }

            return -1;

        }

    }

    public class TransactionsReplyDataAccess : BaseDAL<tblTransactionsReply>
    {
        private TransactionsReplyDataAccess()
        {

        }

        private static TransactionsReplyDataAccess _Instance;
        public static TransactionsReplyDataAccess Instance
        {
            get
            {
                if (TransactionsReplyDataAccess._Instance == null)
                    TransactionsReplyDataAccess._Instance = new TransactionsReplyDataAccess();
                return TransactionsReplyDataAccess._Instance;
            }
           
        }

        public void InsertNewReplyTransaction(BaseReplyParameters tranParams , Int64 TranReqId)
        {

            try
            {
                if (TranReqId <= 0 || tranParams == null)
                    return;

                tblTransactionsReply newTran = base.TEntityObject;
                newTran.MessageType = tranParams.MsgType.ToString();
                newTran.RefrenceNumber = tranParams.TranRefNumber.ToString();
                newTran.Stan = tranParams.Stan.ToString();

                if (tranParams.TranDate == null || tranParams.TranDate.ToString() == "0001/01/01 12:00:00 ق.ظ") // اگر لاگین درست انجام نشود
                    newTran.SendDateTime = DateTime.Now;
                //else
                //    if (tranParams.DateTimeP7 != null && tranParams.DateTimeP7.ToString() != "0001/01/01 12:00:00 ق.ظ")
                //        newTran.SendDateTime = tranParams.DateTimeP7;
                else
                    newTran.SendDateTime = tranParams.TranDate;

                newTran.ReplyCode = Convert.ToInt32(tranParams.ReplyCodeP39);
                newTran.ID = ParametersDataAccess.Instance.NewReplyID;
                newTran.TranReqID = TranReqId;
                base.Insert(newTran);

                PS.Kiosk.Framework.KioskLogger.Instance.LogMessage("Should Insert TranReplyId =" + newTran.ID.ToString() + "Successfully");
            }
            catch (Exception EX)
            {
                
                KioskLogger.Instance.LogMessage(EX, "");
            }

           
        }

        /// <summary>
        ///SignOn ثبت پاسخ 
        /// </summary>
        /// <param name="tranParams"></param>
        /// <param name="TranReqId"></param>
        public void InsertNewReplyTransaction(BaseParameters tranParams, Int64 TranReqId)
        {

            try
            {
                if (TranReqId <= 0)
                    return;

                tblTransactionsReply newTran = base.TEntityObject;
                newTran.MessageType = tranParams.MsgType.ToString();
                newTran.RefrenceNumber = tranParams.TranRefNumber.ToString();
                newTran.Stan = tranParams.Stan.ToString();

                //if (tranParams.TranDate == null || tranParams.TranDate.ToString() == "0001/01/01 12:00:00 ق.ظ") // اگر لاگین درست انجام نشود
                //    newTran.SendDateTime = DateTime.Now;
                //else
                //    if (tranParams.DateTimeP7 != null && tranParams.DateTimeP7.ToString() != "0001/01/01 12:00:00 ق.ظ")
                //        newTran.SendDateTime = tranParams.DateTimeP7;
                //else
                //if (tranParams.TranDate == null || tranParams.TranDate.ToString() != "0001/01/01 12:00:00 ق.ظ")
                //    newTran.SendDateTime = tranParams.TranDate;
                //else
                    newTran.SendDateTime = DateTime.Now;

                if (tranParams is SignOnReplyParameters)
                    newTran.ReplyCode = Convert.ToInt32((tranParams as SignOnReplyParameters).ReplyCodeP39);
                
                newTran.ID = ParametersDataAccess.Instance.NewReplyID;
                newTran.TranReqID = TranReqId;
                base.Insert(newTran);

                PS.Kiosk.Framework.KioskLogger.Instance.LogMessage("Should Insert TranReplyId =" + newTran.ID.ToString() + "Successfully");
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }


        }



    }

    public class ReversalDataAccess : BaseDAL<tblReversalTrans>
    {
        private ReversalDataAccess()
        {

        }

        private static ReversalDataAccess _Instance;
        public static ReversalDataAccess Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ReversalDataAccess();
                return ReversalDataAccess._Instance;
            }

        }

        public Int64 InsertNewTransaction(ReversalParameters ReversalParams)
        {

            tblReversalTrans newReversalTran = base.TEntityObject;
            newReversalTran.IsInTry = true;
            newReversalTran.CardNumber = ReversalParams.CardNumberP2;
            //if (ReversalParams.DateTimeP7 != null)
            //    newReversalTran.SendDateTime = ReversalParams.DateTimeP7;
            //else
            newReversalTran.SendDateTime = ReversalParams.TranDate;
            newReversalTran.PrimaryProcessCode = ((int)ReversalParams.PrimaryProcessCode).ToString();
            newReversalTran.TranRefNumber = ReversalParams.TranRefNumber.ToString();
            newReversalTran.Stan = ReversalParams.Stan.ToString();
            newReversalTran.PrimaryStan = Convert.ToString( ReversalParams.PrimaryStan);
            newReversalTran.PrimaryRefNumber = Convert.ToString( ReversalParams.PrimaryTranRefNumber);
            newReversalTran.PrimaryDateTime = ReversalParams.PrimaryDateTime;
            newReversalTran.PrimaryAmount = Convert.ToString( ReversalParams.PrimaryAmount);
            newReversalTran.PrimaryNewAmount = Convert.ToString(ReversalParams.PrimaryNewAmount);
            newReversalTran.IsoTrack = ReversalParams.IsoTrack;
            newReversalTran.PinBlock = ReversalParams.PinBlockP52;
            newReversalTran.ID = ParametersDataAccess.Instance.NewReversalID;
            base.Insert(newReversalTran);

            return newReversalTran.ID;
        }

        public bool DeleteTransaction(tblReversalTrans entity)
        {
            
            try
            {
                if (entity == null)
                    throw new Exception("Invaliad entity For Delete in tblReversalTrans");
                base.Delete(entity);
                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX,"");
            }
            return false;
        }

        public bool DeleteTransaction(Int64 ID)
        {
           
            try
            {
                if (ID == 0)
                    throw new Exception("Invaliad ID For Delete in tblReversalTrans");

                tblReversalTrans entity = base.SelectAll().Where(i => i.ID == ID).First();
                base.Delete(entity);
                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
            return false;
        }

        public void UpdateIsIntry(Int64 ID , bool isInTry)
        {
            try
            {
                tblReversalTrans tbltran = ObjectContext.tblReversalTrans.Where(i => i.ID == ID).First();
                if (tbltran != null)
                {
                    tbltran.IsInTry = isInTry;
                    ObjectContext.SaveChanges();
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX,"");
            }
        }

        #region 93

        public Int64 InsertNewTransaction93(SettleReverse93Parameters SettleReverseParams)
        {

            tblSettleReversTrans newReversalTran = new tblSettleReversTrans();
            newReversalTran.IsInTry = true;
            newReversalTran.CardNumber = SettleReverseParams.CardNumberP2;
            //if (ReversalParams.DateTimeP7 != null)
            //    newReversalTran.SendDateTime = ReversalParams.DateTimeP7;
            //else
            newReversalTran.SendDateTime = SettleReverseParams.TranDate;
            newReversalTran.PrimaryProcessCode = ((int)SettleReverseParams.PrimaryProcessCode).ToString();
            newReversalTran.TranRefNumber = SettleReverseParams.TranRefNumber.ToString();
            newReversalTran.Stan = SettleReverseParams.Stan.ToString();
            newReversalTran.PrimaryStan = Convert.ToString(SettleReverseParams.PrimaryStan);
            newReversalTran.PrimaryRefNumber = Convert.ToString(SettleReverseParams.PrimaryTranRefNumber);
            newReversalTran.PrimaryDateTime = SettleReverseParams.PrimaryDateTime;
            newReversalTran.PrimaryAmount = Convert.ToString(SettleReverseParams.PrimaryAmount);
            newReversalTran.PrimaryNewAmount = Convert.ToString(SettleReverseParams.PrimaryNewAmount);
            newReversalTran.IsoTrack = SettleReverseParams.IsoTrack;
            newReversalTran.PinBlock = SettleReverseParams.PinBlockP52;
            newReversalTran.ID = ParametersDataAccess.Instance.NewReversalID;
            newReversalTran.ServiceType = (int)SettleReverseParams.ServiceType;
            //base.Insert(newReversalTran);

            ObjectContext.AddObject(newReversalTran.GetType().Name, newReversalTran);
            ObjectContext.SaveChanges();
            ObjectContext.AcceptAllChanges();
            KioskLogger.Instance.LogMessage("Insert entity = " + newReversalTran.ToString());
            return newReversalTran.ID;
        }

        public bool DeleteTransaction93(tblSettleReversTrans entity)
        {

            try
            {
                if (entity == null)
                    throw new Exception("Invaliad entity For Delete in tblReversalTrans");

                ObjectContext.DeleteObject(entity);
                ObjectContext.SaveChanges();

                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
            return false;
        }

        public bool DeleteTransaction93(Int64 ID)
        {

            try
            {
                if (ID == 0)
                    throw new Exception("Invaliad ID For Delete in tblReversalTrans");

                tblSettleReversTrans entity = ObjectContext.tblSettleReversTrans.Where(i => i.ID == ID).First();
                ObjectContext.DeleteObject(entity);
                ObjectContext.SaveChanges();

                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
            return false;
        }

        public void UpdateIsIntry93(Int64 ID, bool isInTry)
        {
            try
            {
                tblSettleReversTrans tbltran = ObjectContext.tblSettleReversTrans.Where(i => i.ID == ID).First();
                if (tbltran != null)
                {
                    tbltran.IsInTry = isInTry;
                    ObjectContext.SaveChanges();
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
        }

        #endregion 93

    }

    public class SettleDataAccess : BaseDAL<tblSettleTrans>
    {
        private SettleDataAccess()
        {

        }

        private static SettleDataAccess _Instance;
        public static SettleDataAccess Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new SettleDataAccess();
                return _Instance;
            }
           
        }

        public Int64 InsertNewSettleTran(SettleParameters newSettleParam)
        {
            try
            {
                tblSettleTrans newSettleTran = base.TEntityObject;
                newSettleTran.IsInTry = true;
                newSettleTran.ID = ParametersDataAccess.Instance.NewSettleID;
                newSettleTran.Stan = Convert.ToString(newSettleParam.Stan);
                newSettleTran.TranRefNumber = Convert.ToString(newSettleParam.TranRefNumber);
                //if (newSettleParam.DateTimeP7 != null)
                //    newSettleTran.SendDateTime = newSettleParam.DateTimeP7;
                //else
                    newSettleTran.SendDateTime = newSettleParam.TranDate;
                base.Insert(newSettleTran);

                return newSettleTran.ID;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }

            return -1;
        }

        public bool DeleteTransaction(tblSettleTrans entity)
        {
            try
            {
                if (entity == null)
                    throw new Exception("Invaliad ID For Delete in tblSettleTrans");

                base.Delete(entity);
                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
            return false;
        }

        public bool DeleteTransaction(Int64 ID)
        {

            try
            {
                if (ID == 0)
                    throw new Exception("Invaliad ID For Delete in tblSettleTrans");

                tblSettleTrans entity = base.SelectAll().Where(i => i.ID == ID).First();
                base.Delete(entity);
                return true;
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX, "");
            }
            return false;
        }

        public void UpdateIsIntry(Int64 ID, bool isInTry)
        {
            try
            {
                tblSettleTrans tbltran = ObjectContext.tblSettleTrans.Where(i => i.ID == ID).First();

                if (tbltran != null)
                {
                    tbltran.IsInTry = isInTry;
                   int UpdateCount = ObjectContext.SaveChanges();
                   KioskLogger.Instance.LogMessage("Update IsIntry Succeessfulyy , UpdateCount" + UpdateCount.ToString());
                }
            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX,"");
            }
        }
    }

    public class Parameters : BaseDAL<tblParameters>
    {
        static Parameters _Instance;

        public static Parameters Instance
        {
            get
            {
                _Instance = new Parameters();
                return _Instance;
            }
           
        }

        public void Update(string TerminalAcceptorId, string CardAcceptorId)
        {
            try
            {
                KioskDataEntities ObjectContext = new KioskDataEntities();

                tblParameters cardAcceptorId = ObjectContext.tblParameters.Where(i => i.ParamKey == "CardAcceptorId").First();
                cardAcceptorId.ParamValue = CardAcceptorId;

                tblParameters terminalAcceptorId = ObjectContext.tblParameters.Where(i => i.ParamKey == "TerminalAcceptorId").First();
                terminalAcceptorId.ParamValue = TerminalAcceptorId;

                ObjectContext.SaveChanges();

            }
            catch (Exception EX)
            {

                KioskLogger.Instance.LogMessage(EX.Message);
            }

 
        }

        public void Update(string[] ParametersKey, string[] ParametersValue)
        {
            try
            {
                KioskDataEntities ObjectContext = new KioskDataEntities();
                int j = 0;
                foreach (string paramKey in ParametersKey)
                {
                    if (j <= ParametersValue.Count() - 1)
                    {
                        tblParameters tblParam = ObjectContext.tblParameters.Where(i => i.ParamKey == paramKey).First();
                        if (paramKey == "ApplicationRestartHour")
                            tblParam.ParamValue = string.Concat(ParametersValue[j], tblParam.ParamValue.Substring(tblParam.ParamValue.Length -6 , 6));
                        else
                            tblParam.ParamValue = ParametersValue[j];
                    }

                    j++;
                }

                ObjectContext.SaveChanges();
            }
            catch (Exception EX)
            {
                
                 KioskLogger.Instance.LogMessage(EX.Message);
            }

        }

    }
}
