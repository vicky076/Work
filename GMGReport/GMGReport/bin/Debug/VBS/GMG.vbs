' VBS Script Generated by CuteFTP (TM) macro recorder.
' Generated at:2014/12/16 12:22:01
' Create TEConnection object
Set MySite = CreateObject("CuteFTPPro.TEConnection")
' Initialize remote server host name, protocol, port, etc.
MySite.Host = "ftp.gmgpulse.com"
MySite.Protocol = "FTPS_AUTH_TLS"
MySite.Port = 21
MySite.Retries = 30
MySite.Delay = 30
MySite.MaxConnections = 4
MySite.TransferType = "DEFAULT"
MySite.DataChannel = "PASV"
MySite.ClearCommandChannel = false
MySite.ClearDataConnection = false
MySite.AutoRename = "OFF"
' WARNING!!! SENSITIVE DATA: user name and password.
MySite.Login = "mycard-prod"
MySite.Password = "MYcArdpROdUct!0n#"
MySite.SocksInfo = ""
MySite.ProxyInfo = ""
' CreateLocalFolder
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_HKG"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_IDN"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_MYS"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_PHL"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_SGP"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_THA"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_TWN"
MySite.CreateLocalFolder "D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_VNM"
' Connect to remote server
MySite.Connect
MySite.Download "/GMG_-_Facebook_-_MyCard_-_HKG/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_HKG"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_IDN/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_IDN"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_MYS/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_MYS"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_PHL/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_PHL"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_SGP/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_SGP"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_THA/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_THA"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_TWN/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_TWN"
MySite.Download "/GMG_-_Facebook_-_MyCard_-_VWN/Facebook_DailySalesDetails_20141214.csv","D:\FTPGMG\20141214\GMG_-_Facebook_-_MyCard_-_VNM"
MySite.disConnect