using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WinFormeplayAd
{
    public partial class Form1 : Form
    {
        private bool TimerEnable = true;
        private bool ProcessStatus = true;
        string FileUpload = WinFormeplayAd.Properties.Settings.Default.FileUpload;

        public void WriteLog(string Desc)
        {
            string SavePath;
            string FileName;

            //資料夾 : /Log/
            SavePath = Application.StartupPath + "\\log\\";
            //檔案名稱 : 20070801Log.txt
            FileName = System.DateTime.Now.ToString("yyyyMMdd") + "Log.txt";
            try
            {
                //檢查:資料夾是否存在(若沒有則建立它)
                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);

                //檢查:檔案是否存在(若沒有則建立它)
                if (!File.Exists(SavePath + FileName))
                    File.AppendAllText(SavePath + FileName, string.Empty, Encoding.UTF8);

                File.AppendAllText(SavePath + FileName, Desc + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                rtbInfo.Text = rtbInfo.Text + ex.Message + Environment.NewLine;
            }
        }

        public Form1()
        {
            InitializeComponent();
            btnHandRun.Enabled = false;
            rtbInfo.Text = "程式將在10秒後自動執行！" + Environment.NewLine;
            WriteLog("＝＝＝ 時間： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，開始執行程式。 ＝＝＝");
            Timer1.Enabled = true;
        }

        //自動執行
        private void Button2_Click(object sender, EventArgs e)
        {
            switch (Timer1.Enabled)
            {
                case false:
                    rtbInfo.Text = rtbInfo.Text + "計時器啟動..." + Environment.NewLine;
                    Timer1.Enabled = true;
                    btnHandRun.Enabled = false;
                    TimerEnable = true;
                    break;
                case true:
                    rtbInfo.Text = rtbInfo.Text + "計時器終止..." + Environment.NewLine;
                    WriteLog("＝＝＝ 時間： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，中止程式執行。 ＝＝＝");
                    WriteLog("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝" + Environment.NewLine);
                    Timer1.Enabled = false;
                    btnHandRun.Enabled = true;
                    TimerEnable = false;
                    break;
            }
        }

        //手動執行
        private void btnHandRun_Click(object sender, EventArgs e)
        {
            rtbInfo.Text = rtbInfo.Text + "手動執行結果：" + Environment.NewLine;
            WriteLog("＝＝＝ 時間： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，(手動)開始執行程式。 ＝＝＝");

            if (ProcessStatus)
            {
                //查排程資料
                Service1.Service1Client wsService1 = new Service1.Service1Client();
                Service1.ReturnValue result = new Service1.ReturnValue();
                WriteLog("查排程資料");
                try
                {
                    result = wsService1.View_ADS_FilmInfo_UPLoadStatus();
                }
                catch (Exception ex)
                {
                    rtbInfo.Text = rtbInfo.Text + "取資料發生錯誤，" + ex.Message + Environment.NewLine;
                    WriteLog("取資料發生錯誤，" + ex.Message);
                    ProcessStatus = false;
                    Timer1.Enabled = true;
                }

                if (result.ReturnMsgNo == 1)
                {
                    int Sn;
                    bool vbsbool = true;
                    if (result.ReturnDataSet.Tables[0].Rows.Count > 0)
                    {
                        //distinct 影音檔名
                        DataView view = new DataView(result.ReturnDataSet.Tables[0]);
                        DataTable distinctValues = view.ToTable(true, "FilmRoute");
                        //產vbs
                        vbsbool = outScriptvbs(distinctValues);

                        if (vbsbool)
                        {
                            for (int i = 0; i < result.ReturnDataSet.Tables[0].Rows.Count - 1; i++)
                            {
                                //update 狀態
                                Service1.ReturnValue resultUpd = new Service1.ReturnValue();
                                try
                                {
                                    Sn = Convert.ToInt32(result.ReturnDataSet.Tables[0].Rows[i]["Sn"].ToString());
                                    resultUpd = wsService1.MyCard_ADS_FilmInfo_UPLoadStatus_UPDATE(Sn);
                                }
                                catch (Exception ex)
                                {
                                    rtbInfo.Text = rtbInfo.Text + "update發生錯誤：" + ex.ToString() + Environment.NewLine;
                                    WriteLog("update發生錯誤" + ex.ToString() + Environment.NewLine);
                                }
                            }
                            WriteLog("update狀態完成");
                        }
                        else
                        {
                            rtbInfo.Text = rtbInfo.Text + "產vbs失敗" + Environment.NewLine;
                            WriteLog("產vbs失敗" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        rtbInfo.Text = rtbInfo.Text + "無排程資料執行。" + Environment.NewLine;
                        WriteLog("無排程資料執行" + Environment.NewLine);
                    }
                }
                else
                {
                    rtbInfo.Text = rtbInfo.Text + "查詢排程發生錯誤:" + result.ReturnMsg + Environment.NewLine;
                    WriteLog("查詢排程發生錯誤：" + result.ReturnMsg);
                }
            }


            WriteLog("＝＝＝ 時間： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，程式執行完畢。 ＝＝＝");
            ProcessStatus = false;
            Timer1.Enabled = true;
            this.Dispose();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnHandRun_Click(sender, e);
        }

        //產vbs檔
        private bool outScriptvbs(DataTable ds)
        {
            WriteLog("開始產生vbs");

            string VBSFilename = "eplayAd.vbs";
            string VBSStr = "";
            VBSStr = "' VBS Script Generated by CuteFTP (TM) macro recorder." + Environment.NewLine;
            VBSStr = VBSStr + "' Create TEConnection object" + Environment.NewLine;
            VBSStr = VBSStr + "Set MySite = CreateObject(\"CuteFTPPro.TEConnection\")" + Environment.NewLine;
            VBSStr = VBSStr + "' Initialize remote server host name, protocol, port, etc." + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Host = \"203.66.134.179\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Protocol = \"SFTP\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Port = 22" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Retries = 30" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Delay = 30" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.MaxConnections = 4" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.TransferType = \"AUTO\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.DataChannel = \"DEFAULT\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.ClearCommandChannel = false" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.ClearDataConnection = false" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.AutoRename = \"OFF\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Login = \"softworld_it\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Password = \"softworldit123!\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.SocksInfo = \"\"" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.ProxyInfo = \"\"" + Environment.NewLine;
            VBSStr = VBSStr + "' Connect to remote server" + Environment.NewLine;
            VBSStr = VBSStr + "MySite.Connect" + Environment.NewLine;
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                VBSStr = VBSStr + "MySite.Upload " + FileUpload + ds.Rows[i]["FilmRoute"].ToString() + "," + "/softworld_it/" + ds.Rows[i]["FilmRoute"].ToString() + "" + Environment.NewLine;
            }
            VBSStr = VBSStr + "MySite.Disconnect";

            try
            {
                //檢查:資料夾是否存在(若沒有則建立它)
                if (!Directory.Exists(Application.StartupPath + "\\VBS\\"))
                    Directory.CreateDirectory(Application.StartupPath + "\\VBS\\");

                File.WriteAllText(Application.StartupPath + "\\VBS\\" + VBSFilename, VBSStr, System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                rtbInfo.Text = rtbInfo.Text + "產生vbs檔案時失敗，錯誤訊息：" + ex.Message + Environment.NewLine;
                WriteLog("產生vbs檔案時失敗:" + rtbInfo.Text);
                return false;
            }

            WriteLog("產生vbs檔案結束");
            return true;
        }
    }
}
