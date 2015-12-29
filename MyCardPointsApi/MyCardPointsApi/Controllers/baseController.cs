using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MyCardPointsApi.Controllers
{
    public class baseController : Controller
    {
        protected string userIp;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (Request.ServerVariables["HTTP_CLIENTIP"] == null)
            {
                userIp = Request.ServerVariables["REMOTE_ADDR"] == null ? "" : Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                userIp = Request.ServerVariables["HTTP_CLIENTIP"].ToString();
            }

            base.OnActionExecuting(filterContext);
        }

        #region Log紀錄
        protected void InsertErrorLog(string ErrorCode, ErrorLog_WS.ErrorType ErrorType, ErrorLog_WS.FaultType FaultType, string ObjName, string KeyWord, string ErrorMsg, string TradeRecordValues)
        {
            ErrorLog_WS.SystemErrorLogServiceClient wsErrorLog = new ErrorLog_WS.SystemErrorLogServiceClient();
            ErrorLog_WS.MyCardSystemErrorLogResult result = new ErrorLog_WS.MyCardSystemErrorLogResult();
            try
            {
                result = wsErrorLog.MyCardSystemErrorLog("MyCardPointsApi", ErrorCode, ErrorType, FaultType, ObjName, KeyWord, ErrorMsg, TradeRecordValues, userIp);
            }
            catch (Exception ex)
            {
                InsertTxtLog("記LOG例外錯誤|" + ex.ToString());
            }
        }
        protected void InsertTxtLog(string InputValue)
        {
            if (Properties.Settings.Default.TxtLogMode.ToLower() == "true")
            {
                string path = HostingEnvironment.MapPath("~/log/" + DateTime.Now.ToString("yyyyMMdd") + "MyCardPointsApi_Log.txt");
                System.IO.File.AppendAllText(path, DateTime.Now.ToString() + "\t" + InputValue + Environment.NewLine);
            }
        }
        public string ErrorMsg(string ErrorCode, string Currency, string returnMsg)
        {
            string Msg = returnMsg;
            ErrorLog_WS.SystemErrorLogServiceClient Errorlog = new ErrorLog_WS.SystemErrorLogServiceClient();
            ErrorLog_WS.MyCardSystemErrorLogQueryResult ErrorLogQueryResult = new ErrorLog_WS.MyCardSystemErrorLogQueryResult();
            try
            {
                ErrorLogQueryResult = Errorlog.MyCardSystemErrorLogQuery(ErrorCode, Currency);
            }
            catch (Exception)
            {
                return Msg;
            }
            finally
            {
                Errorlog.Close();
            }
            if (string.IsNullOrEmpty(ErrorLogQueryResult.ReturnErrorMsg) == false)
            {
                Msg = ErrorLogQueryResult.ReturnErrorMsg;
            }
            Msg = returnMsg;
            return Msg;
        }
        #endregion

        #region 檢查字串格式長度
        protected bool CheckFormat(string InputValue, string strRegex, int oLength, ref string Msg)
        {
            if (string.IsNullOrEmpty(InputValue))
                return true;
            else
            {
                if (System.Text.Encoding.Default.GetBytes(InputValue).Length > oLength)
                {
                    Msg = "長度有誤";
                    return false;
                }
                string VE1 = strRegex;
                Regex Aregx = new Regex(VE1);
                Match M = Aregx.Match(InputValue);
                if (M.Success & M.Length == InputValue.Length)
                    return true;
                else
                {
                    Msg = "格式有誤";
                    return false;
                }
            }
        }
        #endregion

        protected string sha256(string input)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();

            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = sha.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
