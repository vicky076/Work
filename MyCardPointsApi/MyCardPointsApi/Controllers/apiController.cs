using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyCardPointsApi.Controllers
{
    public class apiController : baseController
    {
        //
        // GET: /api/
        /// <summary>
        /// <param name="FacTradeSeq"></param>
        /// <param name="MyCardId"></param>
        /// </summary>
        /// <returns></returns>
        public JsonResult MyCardQuery(string MyCardNo, string GameServiceId)
        {
            string InputValue = "";
            CPAuthResult CPAuthResult = new CPAuthResult();
            CPAuthResult.CardPrice = Convert.ToString(0);

            //Request.Url.AbsoluteUri.ToString()
            InputValue = "[MyCardNo:" + (string.IsNullOrEmpty(MyCardNo) ? "" : MyCardNo) +
                                     "][GameServiceId:" + (string.IsNullOrEmpty(GameServiceId) ? "" : GameServiceId) + "]";

            InsertTxtLog("B2B-MyCardQuery起始:" + Request.Url.AbsoluteUri.ToString() + "|參數InputValue|" + InputValue + "|userIp:" + userIp);
            InsertErrorLog("MCP", ErrorLog_WS.ErrorType.WS, ErrorLog_WS.FaultType.WS一般錯誤, "MyCardPointsApi|api|MyCardQuery", MyCardNo, "10", "IN|" + InputValue);
            //檢查字串
            //必填參數需有值
            if (string.IsNullOrEmpty(MyCardNo) || string.IsNullOrEmpty(GameServiceId))
            {
                CPAuthResult.ReturnMsgNo = "MCP001";
                CPAuthResult.ReturnMsg = "(MCP001)  Error";
                InsertErrorLog("MCP001", ErrorLog_WS.ErrorType.WS, ErrorLog_WS.FaultType.WS一般錯誤, "MyCardQuery", "MyCardPointsApi", "必填字串有空值", InputValue);
                return Json(CPAuthResult, JsonRequestBehavior.AllowGet);
            }
            //檢查字串格式和長度
            string retMsg = "";
            if (!CheckFormat(MyCardNo, "^[0-9a-zA-Z_-]*", 20, ref retMsg))
            {
                CPAuthResult.ReturnMsgNo = "MCP002";
                CPAuthResult.ReturnMsg = "(MCP002) MyCardNo Error";
            }

            if (!CheckFormat(GameServiceId, "^[0-9a-zA-Z_-]*", 30, ref retMsg))
            {
                CPAuthResult.ReturnMsgNo = "MCP003";
                CPAuthResult.ReturnMsg = "(MCP003) GameServiceId Error";
            }

            //DB
            InputValue = "[MyCardNo:" + MyCardNo + "][GameServiceId:" + GameServiceId + "][userIp:" + userIp + "]";
            ServiceReference1.Service1Client wsMyCardPointsApiWcf = new ServiceReference1.Service1Client();
            List<ServiceReference1.CheckMyCardForCardPoint> result = new List<ServiceReference1.CheckMyCardForCardPoint>();
            try
            {
                result = wsMyCardPointsApiWcf.CheckMyCardForCardPoint(MyCardNo, GameServiceId, userIp).ToList();
            }
            catch (Exception ex)
            {
                CPAuthResult.ReturnMsgNo = "MCP099";
                CPAuthResult.ReturnMsg = "(MCP099) Error";
                InsertTxtLog("CheckMyCardForCardPoint查詢失敗|參數|" + InputValue + "|EX|" + ex.ToString());
                InsertErrorLog("MCP099", ErrorLog_WS.ErrorType.WS, ErrorLog_WS.FaultType.WS例外錯誤, "CheckMyCardForCardPoint", MyCardNo, "[ReturnCode:" + CPAuthResult.ReturnMsgNo + "][ReturnMsg:" + CPAuthResult.ReturnMsg + "][EX:" + ex.ToString() + "]", InputValue);
                return Json(CPAuthResult, JsonRequestBehavior.AllowGet);
            }

            CPAuthResult.Currency = result[0].Currency.ToString();
            CPAuthResult.CardPrice = Convert.ToString(Convert.ToDecimal(string.IsNullOrEmpty(Convert.ToString(result[0].CardPrice)).Equals(true) ? "0" : Convert.ToString(result[0].CardPrice)));
            CPAuthResult.ReturnMsgNo = result[0].ReturnMsgNo.ToString() == "1" ? "1" : "MCP004";

            if (result[0].ReturnMsgNo.ToString() != "1")
                CPAuthResult.ReturnMsg = "(" + result[0].ReturnMsgNo.ToString() + ")" + result[0].ReturnMsg.ToString();
            else
                CPAuthResult.ReturnMsg = result[0].ReturnMsg.ToString();

            return Json(CPAuthResult, JsonRequestBehavior.AllowGet);
        }

        public class CPAuthResult
        {
            public string Currency;
            public string CardPrice;
            public string ReturnMsgNo;
            public string ReturnMsg;
        }
    }
}
