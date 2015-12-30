using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace WinFormeplayAdWcf
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Service1.svc 或 Service1.svc.cs，然後開始偵錯。
    public class Service1 : IService1
    {
        public ReturnValue View_ADS_FilmInfo_UPLoadStatus()
        {
            ReturnValue result = new ReturnValue();
            Base obase = new Base();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = obase.sqlConnection(15);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select Sn,FilmName,UPLoadStatus,'HarryPotter.wmv' as FilmRoute  From MyCard_ADSell_DB.dbo.View_ADS_FilmInfo_UPLoadStatus Where 1=1 and UPLoadStatus = 0  Order by Sn";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    cmd.Connection.Open();
                    da.Fill(ds);
                    result.ReturnMsgNo = 1;
                    result.ReturnMsg = "成功";
                    result.ReturnDataSet = ds;
                }
                catch (Exception ex)
                {
                    result.ReturnMsgNo = -999;
                    result.ReturnMsg = "執行[MyCard_ADSell_DB].[dbo].[View_ADS_FilmInfo_UPLoadStatus]時發生錯誤，錯誤原因(Exception)：" + ex.Message;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
            return result;
        }

        public ReturnValue MyCard_ADS_FilmInfo_UPLoadStatus_UPDATE(int Sn)
        {
            ReturnValue result = new ReturnValue();
            Base obase = new Base();
            using (SqlCommand sqlComm = new SqlCommand())
            {
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.CommandText = @"[MyCard_ADSell_DB].[dbo].[MyCard_ADS_FilmInfo_UPLoadStatus_UPDATE]";
                sqlComm.Connection = obase.sqlConnection(15);
                sqlComm.Parameters.Add("@Sn", SqlDbType.Int).Value = Sn;
                SqlParameter ReturnMsgNo = sqlComm.Parameters.Add("@ReturnMsgNo", SqlDbType.Int);
                ReturnMsgNo.Direction = ParameterDirection.Output;
                SqlParameter ReturnMsg = sqlComm.Parameters.Add("@ReturnMsg", SqlDbType.NVarChar, 30);
                ReturnMsg.Direction = ParameterDirection.Output;
                DataSet ds = new DataSet();
                try
                {
                    sqlComm.Connection.Open();
                    sqlComm.ExecuteNonQuery();
                    result.ReturnMsgNo = int.Parse(ReturnMsgNo.Value.ToString());
                    result.ReturnMsg = ReturnMsg.Value.ToString();
                    result.ReturnDataSet = ds;
                }
                catch (Exception ex)
                {
                    throw new Exception("執行MyCard_ADS_FilmInfo_UPLoadStatus_UPDATE失敗|" + ex.ToString());
                }
                finally
                {
                    sqlComm.Connection.Close();
                }
            }
            return result;
        }
    }
}
