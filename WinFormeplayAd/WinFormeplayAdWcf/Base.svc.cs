using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WinFormeplayAdWcf
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Base"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Base.svc 或 Base.svc.cs，然後開始偵錯。
    public class Base : IBase
    {
        public System.Data.SqlClient.SqlConnection sqlConnection(int dbNum)
        {
            MyCardConn.Dbfuction myDbfnc = new MyCardConn.Dbfuction();
            int ConnectionMode = Properties.Settings.Default.ConnectionMode;
            switch (ConnectionMode)
            {
                case 1:
                    return myDbfnc.open100Db();
                case 2:
                    return myDbfnc.openTestDb();
                case 3:
                    switch (dbNum)
                    {
                        case 11:
                            return myDbfnc.open11Db();
                        case 12:
                            return myDbfnc.open12Db();
                        case 13:
                            return myDbfnc.open13Db();
                        case 14:
                            return myDbfnc.open14Db();
                        case 15:
                            return myDbfnc.open15Db();
                        case 16:
                            return myDbfnc.open16Db();
                        case 17:
                            return myDbfnc.open17Db();
                        case 19:
                            return myDbfnc.open19Db();
                        case 20:
                            return myDbfnc.open20Db();
                        default:
                            return myDbfnc.open100Db();
                    }
                default:
                    return myDbfnc.open100Db();
            }
        }
    }
}
