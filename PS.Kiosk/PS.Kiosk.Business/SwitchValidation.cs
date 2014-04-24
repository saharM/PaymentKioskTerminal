using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PS.Kiosk.Business.BusinessObjects;
using PS.Kiosk.Framework.ExceptionManagement;
using PS.Kiosk.Common.Model;
using PS.Kiosk.Data.DataAccessObjects;
using PS.Kiosk.Framework;

namespace PS.Kiosk.Business
{
    class SwitchValidation
    {
        private static List<int> _CardCapturingError;
        /// <summary>
        /// لیستی از کد خطاهایی که باید پس از آن کارت را ضبظ کرد
        /// </summary>
        public static List<int> CardCapturingError
        {
            get
            {
                if (SwitchValidation._CardCapturingError == null)
                    SwitchValidation._CardCapturingError = ShetabErrorBR.BRShetErrorInstance.getCapturingCardError();
                return SwitchValidation._CardCapturingError;
            }
           
        }

        public static void IsValid39(int Code , BaseReplyParameters tranReply = null)
        {
            
            if (Code != 0 )
            {
                if (tranReply == null || tranReply.MsgFormat == Enums.SwitchMsgFormat.Shetab87)
                {
                    string shetabError = ShetabErrorBR.BRShetErrorInstance.getShetabError(Code);
                    CheckForCardCapturing(Code);
                    throw new Exception(shetabError);
                }
                else
                {
                    //در پیغام ها به فرمت  93
                    //شرح خطا در فیلد48 است
                    if (tranReply.MsgFormat == Enums.SwitchMsgFormat.Shetab93)
                    {
                        if (tranReply.ExtraData != null && tranReply.ExtraData.Contains("ضبط"))
                        {
                            if (ParametersDataAccess.Instance.DetectForCardCapturing)
                            {
                                StateManager.Instance.Current.ShouldCaptureCard = true;
                            }
                            else
                                StateManager.Instance.Current.ShouldCaptureCard = false;
                        }
                        else
                            StateManager.Instance.Current.ShouldCaptureCard = false;

                        if (tranReply.ExtraData != null)
                            throw new Exception(tranReply.ExtraData);
                        else
                            throw new Exception("Internal Exception");
                    }
                }
            }
        }

        public static void IsValidTran(BaseParameters TranRequest , BaseReplyParameters TranReply) 
        {
            if (TranRequest.Stan != TranReply.Stan ||
                 Convert.ToInt32( TranRequest.TerminalAcceptorId) != Convert.ToInt32( TranReply.TerminalAcceptorId ) ||
                Convert.ToInt32(TranRequest.CardAcceptorId) != Convert.ToInt32(TranReply.CardAcceptorId))
                throw new CustomException("Invalid Tran", "Fields in Req and Reply is not Equal");

            
             
            IsValid39(Convert.ToInt32(TranReply.ReplyCodeP39) , TranReply);
        }

        /// <summary>
        /// در بعضی از موارد ادامه ارسال تراکنش پرداخت و برگشت باید متوقف شود
        /// </summary>
        /// <param name="TranReply"></param>
        /// <returns></returns>
        public static bool ContinueTran(BaseReplyParameters TranReply)
        {

            if (//عمليات تائيديه تراکنش قبلا با موفقطت انجام شده
                Convert.ToInt32(TranReply.ReplyCodeP39) == 2 ||
                //تراکنش اصلی يافت نشد
                Convert.ToInt32(TranReply.ReplyCodeP39) == 25 ||
                //کارت مظنون به تقلب است
                Convert.ToInt32(TranReply.ReplyCodeP39) == 34 ||
                //اتمام روز مالی
                Convert.ToInt32(TranReply.ReplyCodeP39) == 77 ||
                //بانک صادر کننده کارت معتبر نمی باشد
                Convert.ToInt32(TranReply.ReplyCodeP39) == 91)
                return false;
            else
                return true;
        }

        #region privateMethod

        private static void CheckForCardCapturing(int Code)
        {
            if (CardCapturingError.Contains(Code))
            {
                if (ParametersDataAccess.Instance.DetectForCardCapturing)
                {
                    StateManager.Instance.Current.ShouldCaptureCard = true;
                }
                else
                    StateManager.Instance.Current.ShouldCaptureCard = false;
            }
            else
                StateManager.Instance.Current.ShouldCaptureCard = false;
        }

        #endregion privateMethod
    }
}
