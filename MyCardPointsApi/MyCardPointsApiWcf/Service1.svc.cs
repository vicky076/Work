using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;

namespace MyCardPointsApiWcf
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Service1.svc 或 Service1.svc.cs，然後開始偵錯。
    public class Service1 : IService1
    {
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        public List<CheckMyCardForCardPoint> CheckMyCardForCardPoint(string MyCardNo, string GameServiceId, string GameServiceIp)
        {
            List<CheckMyCardForCardPoint> result = new List<CheckMyCardForCardPoint>();
            sqlConnection(11);
            using (SqlCommand cmd = new SqlCommand("OGB_DB_CARD.dbo.CheckMyCardForCardPoint", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MyCardNo", SqlDbType.VarChar, 20).Value = MyCardNo;
                cmd.Parameters.Add("@GameServiceId", SqlDbType.VarChar, 30).Value = GameServiceId;
                cmd.Parameters.Add("@GameServiceIp", SqlDbType.VarChar, 50).Value = GameServiceIp;

                System.Data.SqlClient.SqlParameter Currency = new System.Data.SqlClient.SqlParameter();
                Currency = cmd.Parameters.Add("@Currency", SqlDbType.VarChar, 6);
                Currency.Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlParameter CardPrice = new System.Data.SqlClient.SqlParameter();
                CardPrice = cmd.Parameters.Add("@CardPrice", SqlDbType.Decimal);
                CardPrice.Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlParameter ReturnMsgNo = new System.Data.SqlClient.SqlParameter();
                ReturnMsgNo = cmd.Parameters.Add("@ReturnMsgNo", SqlDbType.Int);
                ReturnMsgNo.Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlParameter ReturnMsg = new System.Data.SqlClient.SqlParameter();
                ReturnMsg = cmd.Parameters.Add("@ReturnMsg", SqlDbType.NVarChar, 100);
                ReturnMsg.Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlParameter ErrorCode = new System.Data.SqlClient.SqlParameter();
                ErrorCode = cmd.Parameters.Add("@ErrorCode", SqlDbType.VarChar, 8);
                ErrorCode.Direction = ParameterDirection.Output;

                System.Data.SqlClient.SqlParameter LogSn = new System.Data.SqlClient.SqlParameter();
                LogSn = cmd.Parameters.Add("@LogSn", SqlDbType.Int);
                LogSn.Direction = ParameterDirection.Output;

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                CheckMyCardForCardPoint ds = new CheckMyCardForCardPoint();
                ds.Currency = string.IsNullOrEmpty(Convert.ToString(Currency.Value)).Equals(true) ? "" : Currency.Value.ToString();
                ds.CardPrice = Convert.ToString(string.IsNullOrEmpty(Convert.ToString(CardPrice.Value)).Equals(true) ? "0" : Convert.ToString(CardPrice.Value.ToString()));
                ds.ReturnMsgNo = Convert.ToString(string.IsNullOrEmpty(Convert.ToString(ReturnMsgNo.Value)).Equals(true) ? "-99" : Convert.ToString(ReturnMsgNo.Value.ToString()));
                ds.ReturnMsg = string.IsNullOrEmpty(Convert.ToString(ReturnMsg.Value)).Equals(true) ? "" : ReturnMsg.Value.ToString();
                ds.ErrorCode = string.IsNullOrEmpty(Convert.ToString(ErrorCode.Value)).Equals(true) ? "" : ErrorCode.Value.ToString();
                ds.LogSn = Convert.ToString(string.IsNullOrEmpty(Convert.ToString(LogSn.Value)).Equals(true) ? "0" : Convert.ToString((LogSn.Value.ToString())));

                result.Add(ds);
                return result;
            }
        }
        #region 工具
        /// <summary>
        /// 取得資料庫連線
        /// </summary>
        public void sqlConnection(int dbNum)
        {
            MyCardConn.Dbfuction myDbfnc = new MyCardConn.Dbfuction();
            int ConnectionMode = Properties.Settings.Default.ConnectionMode;
            switch (ConnectionMode)
            {
                case 1:
                    conn = myDbfnc.open100Db();
                    break;
                case 2:
                    conn = myDbfnc.openTestDb();
                    break;
                case 3:
                    switch (dbNum)
                    {
                        case 11:
                            conn = myDbfnc.open11Db();
                            break;
                        case 12:
                            conn = myDbfnc.open12Db();
                            break;
                        case 13:
                            conn = myDbfnc.open13Db();
                            break;
                        case 14:
                            conn = myDbfnc.open14Db();
                            break;
                        case 15:
                            conn = myDbfnc.open15Db();
                            break;
                        case 16:
                            conn = myDbfnc.open16Db();
                            break;
                        case 17:
                            conn = myDbfnc.open17Db();
                            break;
                        case 19:
                            conn = myDbfnc.open19Db();
                            break;
                        case 20:
                            conn = myDbfnc.open20Db();
                            break;
                        default:
                            conn = myDbfnc.open100Db();
                            break;
                    }
                    break;
                default:
                    conn = myDbfnc.open100Db();
                    break;
            }
        }
        #endregion
    }

    public class CheckMyCardForCardPoint
    {
        public string Currency { get; set; }
        public string CardPrice { get; set; }
        public string ReturnMsgNo { get; set; }
        public string ReturnMsg { get; set; }
        public string ErrorCode { get; set; }
        public string LogSn { get; set; }
    }
}
