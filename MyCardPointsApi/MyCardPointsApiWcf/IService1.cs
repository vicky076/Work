using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MyCardPointsApiWcf
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
        // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。

        [OperationContract]
       List  <CheckMyCardForCardPoint> CheckMyCardForCardPoint(string MyCardNo, string GameServiceId, string GameServiceIp);

        // TODO: 在此新增您的服務作業
    }

    //使用下列範例中所示的資料合約，新增複合型別至服務作業。
    //[DataContract]
    //public class ReturnResult
    //{
    //    [DataMember]
    //    public string @Currency { get; set; }

    //    [DataMember]
    //    public decimal @CardPrice { get; set; }

    //    [DataMember]
    //    public int @ReturnMsgNo { get; set; }

    //    [DataMember]
    //    public string @ReturnMsg { get; set; }

    //    [DataMember]
    //    public string @ErrorCode { get; set; }

    //    [DataMember]
    //    public int @LogSn { get; set; }
    //}
}
