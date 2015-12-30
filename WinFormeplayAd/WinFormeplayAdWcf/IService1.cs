using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace WinFormeplayAdWcf
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        ReturnValue View_ADS_FilmInfo_UPLoadStatus();

        [OperationContract]
        ReturnValue MyCard_ADS_FilmInfo_UPLoadStatus_UPDATE(int Sn);

        // TODO: 在此新增您的服務作業
    }


    //使用下列範例中所示的資料合約，新增複合型別至服務作業。
    [DataContract]
    public class ReturnValue
    {
        [DataMember]
        public int ReturnMsgNo { get; set; }
        [DataMember]
        public string ReturnMsg { get; set; }
        [DataMember]
        public DataSet ReturnDataSet { get; set; }
    }
}
