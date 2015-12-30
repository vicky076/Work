Imports System.Data.SqlClient
Imports System.IO

Public Class Form1
    Private TimerEnable As Boolean = True
    Private ProcessStatus As Boolean = True

    Dim wsGMGService As New GMGService.GMGService
    Dim Status As Integer = 0     '狀態
    Dim CreateUser As String '= "Hsu"
    Dim FileNM As String = "Facebook_DailySalesDetails_"
    Dim Foler As String = "GMG_-_Facebook_-_MyCard_-_"

    Public Sub WriteLog(ByVal Desc As String)
        Dim SavePath As String
        Dim FileName As String

        '資料夾 : /Log/
        SavePath = Application.StartupPath & "\log\"

        '檔案名稱 : 20070801Log.txt
        FileName = Date.Now.ToString("yyyyMMdd") & "Log.txt"

        Try
            '檢查:資料夾是否存在(若沒有則建立它)
            Dim folderExists As Boolean
            folderExists = My.Computer.FileSystem.DirectoryExists(SavePath)
            If folderExists = False Then
                My.Computer.FileSystem.CreateDirectory(SavePath)
            End If
            '檢查:檔案是否存在(若沒有則建立它)
            Dim fileExists As Boolean
            fileExists = My.Computer.FileSystem.FileExists(SavePath & FileName)
            If fileExists = False Then
                My.Computer.FileSystem.WriteAllText(SavePath & FileName, String.Empty, False)
            End If
            My.Computer.FileSystem.WriteAllText(SavePath & FileName, Desc & vbCrLf, True)
        Catch ex As Exception
            rtbInfo.Text = rtbInfo.Text & ex.Message & vbCrLf
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        btnHandRun.Enabled = False
        rtbInfo.Text = "程式將在10秒後自動執行！" & vbCrLf
        WriteLog("＝＝＝ 時間： " & Now.ToString("yyyy-MM-dd HH:mm:ss") & "，開始執行程式。 ＝＝＝")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        Button1_Click(sender, e)
    End Sub

    '手動執行
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnHandRun.Click
        rtbInfo.Text = rtbInfo.Text & "執行結果：" & vbCrLf

        If ProcessStatus Then
            '每天下午3點跑一次產vbs
            If (CInt(Now.ToString("HHmm") + 2) > CInt(My.Settings.RunScheduleTime)) And (CInt(Now.ToString("HHmm") - 2) < CInt(My.Settings.RunScheduleTime)) Then
                Runvbs()
            End If

            '檢查排程資料(View)
            Dim ds As New DataSet
            Try
                ds = wsGMGService.View_MyCardCPSave_ScheduleCheck(My.Settings.GameFacId)
            Catch ex As Exception
                rtbInfo.Text = rtbInfo.Text & "取資料發生錯誤，" & ex.Message & vbCrLf
                WriteLog("取資料發生錯誤，" & ex.Message)
                ProcessStatus = False
                Timer1.Enabled = True
            End Try

            Dim Sn As Integer = 0
            Dim GameFacId As String = ""
            Dim GameSerId As String = ""
            Dim SaveDate As String = ""
            If (Not IsNothing(ds.Tables())) Then
                If (ds.Tables(0).Rows.Count > 0) Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        SaveDate = Convert.ToDateTime(ds.Tables(0).Rows(i)("SaveDate")).ToString("yyyy/MM/dd")
                        Sn = ds.Tables(0).Rows(i)("Sn")
                        GameFacId = ds.Tables(0).Rows(i)("GameFacId")
                        GameSerId = ds.Tables(0).Rows(i)("GameSerId")
                        CreateUser = ds.Tables(0).Rows(i)("CreateUser")
                        '讀excel檔
                        WriteLog("ReadExcel|GameFacId=" & GameFacId & ",SaveDate=" & SaveDate + ",Sn=" & Sn.ToString())
                        ReadExcel(GameFacId, GameFacId, SaveDate, Sn)
                    Next
                Else
                    rtbInfo.Text = rtbInfo.Text & "查無排程資料執行。"
                    WriteLog("查無排程資料執行")
                End If
                ProcessStatus = False
                Timer1.Enabled = True
            Else
                rtbInfo.Text = rtbInfo.Text & "查無排程資料執行。"
                WriteLog("查無排程資料執行")
                ProcessStatus = False
                Timer1.Enabled = True
            End If
        Else
            WriteLog("＝＝＝ 時間： " & Now.ToString("yyyy-MM-dd HH:mm:ss") & "，程式執行完畢。 ＝＝＝")
            Me.Dispose()
            Me.Close()
        End If
    End Sub

    '自動啟動/停止
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Select Case TimerEnable
            Case False
                rtbInfo.Text = rtbInfo.Text & "計時器啟動..." & vbCrLf
                Timer1.Enabled = True
                btnHandRun.Enabled = False
                TimerEnable = True
            Case True
                rtbInfo.Text = rtbInfo.Text & "計時器終止..." & vbCrLf
                WriteLog("＝＝＝ 時間： " & Now.ToString("yyyy-MM-dd HH:mm:ss") & "，中止程式執行。 ＝＝＝")
                Timer1.Enabled = False
                btnHandRun.Enabled = True
                TimerEnable = False
        End Select
    End Sub

    Private Sub ReadExcel(ByVal GameFacId As String, ByVal GameSerId As String, ByVal SaveDate As DateTime, ByVal Sn As Integer)
        Dim FilePath, EName As String
        Dim dt As New DataTable
        Dim Yesterday As String = SaveDate.AddDays(-1).ToString("yyyyMMdd") 'Convert.ToDateTime(Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).ToString("yyyy/MM/dd"))
        Dim Year As String = ""
        Dim Month As String = ""
        Dim Day As String = ""


        'Year = Yesterday.Substring(0, 4)
        'If (SaveDate.Month.ToString().Length <> 2 Or Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).ToString().Length <> 2) Then
        '    If (SaveDate.Month.ToString().Length <> 2) Then
        '        Month = "0" & SaveDate.Month.ToString()
        '    Else
        '        Month = SaveDate.Month
        '    End If

        '    If (Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).Day.ToString().Length() <> 2) Then
        '        Day = "0" & Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).Day.ToString()
        '    Else
        '        Day = Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).Day.ToString()
        '    End If
        '    Yesterday = Year & Month & Day
        'End If

        'Yesterday = Yesterday.Replace("/", "")


        '跑八個國家的迴圈
        Dim country As String = My.Settings.Country
        If (country.Length > 0) Then

            '先清除TempTable
            Dim ReturnValue As New GMGService.ReturnValue
            Try
                ReturnValue = wsGMGService.MyCardCPSave_CPTempData_Insert(0, "", "", "", "", 0, DateTime.Now)

            Catch ex As Exception
                rtbInfo.Text = rtbInfo.Text & ex.ToString()
                WriteLog("MyCardCPSave_CPTempData_Insert|" + ex.ToString())
                Status = 99
            End Try

            If Status = 99 Then
                Exit Sub
            End If

            Dim Input As String = ""
            Dim Spilt() As String = Split(country, ",")
            For x = 0 To Spilt.Length - 1
                '開始讀取回的excel檔
                FilePath = Application.StartupPath & "\FTPGMG\" & Yesterday & "\" & Foler & Spilt(x).ToString() & "\" & FileNM & Yesterday & ".csv"
                WriteLog(FilePath)
                EName = Path.GetExtension(FilePath)
                If EName.ToLower = ".csv" Then
                    '先判斷檔案是否存在
                    If System.IO.File.Exists(FilePath) Then
                        Dim Strred As System.IO.StreamReader
                        '讀取csv檔寫進datatable
                        Try
                            Strred = New System.IO.StreamReader(FilePath, System.Text.Encoding.Default)

                            '建立一個dataset
                            Dim NewDSSet As New DataSet
                            NewDSSet.Tables.Add("dataSet")
                            NewDSSet.Tables(0).Columns.Add("CardAmount") '金額
                            NewDSSet.Tables(0).Columns.Add("TransDate") '交易日期
                            NewDSSet.Tables(0).Columns.Add("TransTime") '交易時間
                            NewDSSet.Tables(0).Columns.Add("ActivatorPartnerTransId") '廠商交易序號(智冠交易序號)


                            Dim StrData As String = ""
                            Dim cnt As Integer = 0
                            Do While Not Strred.EndOfStream
                                '先將txt檔資料存入dataset
                                StrData = Strred.ReadLine
                                StrData = StrData.Trim

                                If (cnt <> 0) Then
                                    If StrData <> "" Then
                                        Dim DataArray() As String = Split(StrData, ",")
                                        If (DataArray.Length = 27) Then
                                            Dim myrow As DataRow = NewDSSet.Tables(0).NewRow
                                            myrow.Item(0) = DataArray(19)
                                            myrow.Item(1) = DataArray(15)
                                            myrow.Item(2) = DataArray(16)
                                            myrow.Item(3) = DataArray(23)
                                            NewDSSet.Tables(0).Rows.Add(myrow)
                                        Else
                                            rtbInfo.Text = rtbInfo.Text & DataArray(12) & "資料格式不正確"
                                            WriteLog(DataArray(12) & "第" + cnt.ToString() & "行資料格式不正確")
                                            Exit Sub
                                        End If
                                    Else
                                        Continue For
                                    End If
                                End If
                                cnt = cnt + 1
                            Loop

                            Dim TransDT As DateTime = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
                            Dim CardAmount As String = ""
                            Dim txtVal As Integer = 0

                            For i As Integer = 0 To NewDSSet.Tables(0).Rows.Count - 1
                                TransDT = Convert.ToDateTime(Convert.ToDateTime(NewDSSet.Tables(0).Rows(i)("TransDate"))).ToString("yyyy/MM/dd") + " " + NewDSSet.Tables(0).Rows(i)("TransTime").ToString()
                                txtVal = NewDSSet.Tables(0).Rows(i)("CardAmount").ToString.Length
                                CardAmount = (Double.Parse(NewDSSet.Tables(0).Rows(i)("CardAmount").ToString()) / 100).ToString()  'NewDSSet.Tables(0).Rows(i)("CardAmount").ToString().Substring(0, (txtVal - 2))
                                Input = "1," & NewDSSet.Tables(0).Rows(i)("ActivatorPartnerTransId") & ",,,," & CardAmount & "," & TransDT.ToString()
                                ReturnValue = wsGMGService.MyCardCPSave_CPTempData_Insert(1, NewDSSet.Tables(0).Rows(i)("ActivatorPartnerTransId"), "", "", "", CardAmount, TransDT)

                                If (ReturnValue.ReturnMsgNo <> 1) Then
                                    rtbInfo.Text = rtbInfo.Text & ReturnValue.ReturnMsg
                                    Status = 99
                                    WriteLog("MyCardCPSave_CPTempData_Insert|Input|" + Input & "|ReturnMsgNo|" & ReturnValue.ReturnMsgNo & "|ReturnMsg|" & ReturnValue.ReturnMsg)
                                End If
                            Next

                            
                        Catch ex As Exception
                            rtbInfo.Text = rtbInfo.Text & ex.ToString()
                            WriteLog(ex.ToString())
                            Status = 99
                            Exit Sub
                        End Try

                    Else
                        rtbInfo.Text = rtbInfo.Text & "Excel檔案不存在。"
                        WriteLog(Spilt(x) & " 日期:" & Yesterday & " Excel檔案不存在。")
                        '如果檔案不存在 離開function
                        Exit Sub
                        'Continue For
                    End If
                End If
            Next

            ' 比對儲值差異
            Dim SaveStartDate As DateTime = Convert.ToDateTime(Convert.ToDateTime(CType(SaveDate, Date).AddDays(-1)).ToString("yyyy/MM/dd") & " 13:00:00")
            Dim SaveEndDate As DateTime = Convert.ToDateTime(Convert.ToDateTime(SaveDate.ToString("yyyy/MM/dd") & " 13:00:00"))
            Dim result As New GMGService.ReturnValue
            Input = GameFacId & "," & GameSerId & "," & SaveStartDate.ToString() & "," & SaveEndDate.ToString() & "," & CreateUser

            Try

                result = wsGMGService.MyCardCPSave_Difference_Proc(GameFacId, GameSerId, SaveStartDate, SaveEndDate, CreateUser)
                If result.ReturnMsgNo <> 1 Then
                    rtbInfo.Text = rtbInfo.Text & result.ReturnMsg
                    Status = 99
                End If
                WriteLog("MyCardCPSave_Difference_Proc|Input|" & Input & "|ReturnMsgNo|" & result.ReturnMsgNo & "|ReturnMsg|" & result.ReturnMsg)
            Catch ex As Exception
                rtbInfo.Text = rtbInfo.Text & ex.ToString()
                WriteLog("MyCardCPSave_Difference_Proc|Input|" & Input & "|" & ex.ToString())
                Status = 99
            End Try

            If Status <> 99 Then
                '修改排程狀態
                Dim updresult As New GMGService.ReturnValue
                If Status = 0 Then
                    Status = 5
                End If
                Input = Sn & "," & Status & "," & CreateUser
                Try

                    updresult = wsGMGService.MyCardCPSave_ScheduleCheck_UpdateStatus(Sn, Status, CreateUser)
                    If updresult.ReturnMsgNo <> 1 Then
                        rtbInfo.Text = rtbInfo.Text & updresult.ReturnMsg
                    End If
                    WriteLog("MyCardCPSave_ScheduleCheck_UpdateStatus|Input|" & Input & "|ReturnMsgNo|" & updresult.ReturnMsgNo & "|ReturnMsg|" & updresult.ReturnMsg)
                Catch ex As Exception
                    rtbInfo.Text = rtbInfo.Text & ex.ToString()
                    WriteLog("MyCardCPSave_ScheduleCheck_UpdateStatus|Input|" & Input & "|" & ex.ToString())
                End Try
            End If

            rtbInfo.Text = rtbInfo.Text & "執行完成。"
            WriteLog(Yesterday & " 執行完成。")
        Else
            rtbInfo.Text = rtbInfo.Text & "無資料。"
            WriteLog(Yesterday & " 無資料。")
        End If
    End Sub

    '跑vbs
    Private Sub Runvbs()
        Dim RunTime As String
        '如果config有設定日期，就產生設定日期的VBS檔
        If My.Settings.RunVBSDate <> "" Then
            Try
                RunTime = CDate(My.Settings.RunVBSDate).ToString("yyyyMMdd")
            Catch ex As Exception
                RunTime = Now.AddDays(-2).ToString("yyyyMMdd")
            End Try
        Else
            'config沒有設定日期，產生昨天日期的VBS檔
            RunTime = Now.AddDays(-1).ToString("yyyyMMdd")
        End If

        'Dim Yesterday As String = Convert.ToDateTime(Convert.ToDateTime(CType(DateTime.Now, Date).AddDays(-1)).ToString("yyyy/MM/dd"))
        'Dim Year As String = ""
        'Dim Month As String = ""
        'Dim Day As String = ""

        'Year = Yesterday.Substring(0, 4)
        'If (NowTime.Month.ToString().Length <> 2 Or Convert.ToDateTime(CType(NowTime, Date).AddDays(-1)).ToString().Length <> 2) Then
        '    If (NowTime.Month.ToString().Length <> 2) Then
        '        Month = "0" & NowTime.Month.ToString()
        '    Else
        '        Month = NowTime.Month
        '    End If

        '    If (Convert.ToDateTime(CType(NowTime, Date).AddDays(-1)).Day.ToString().Length() <> 2) Then
        '        Day = "0" & Convert.ToDateTime(CType(NowTime, Date).AddDays(-1)).Day.ToString()
        '    Else
        '        Day = Convert.ToDateTime(CType(NowTime, Date).AddDays(-1)).Day.ToString()
        '    End If
        '    Yesterday = Year & Month & Day
        'End If

        'Yesterday = Yesterday.Replace("/", "")
        WriteLog("產生VBS:" & RunTime)
        outScriptvbs(RunTime)
    End Sub


    '產vbs檔
    'Currency 國家
    'DT 日期 (yyyyMMdd)
    Private Sub outScriptvbs(ByRef Yesterday As String)
        Dim VBSFilename As String = "GMG.vbs"
        Dim VBSStr As String = ""
        VBSStr = "' VBS Script Generated by CuteFTP (TM) macro recorder." & vbCrLf
        VBSStr = VBSStr & "' Generated at:" & Now.ToString("yyyy/MM/dd HH:mm:ss") & vbCrLf
        VBSStr = VBSStr & "' Create TEConnection object" & vbCrLf
        VBSStr = VBSStr & "Set MySite = CreateObject(""CuteFTPPro.TEConnection"")" & vbCrLf
        VBSStr = VBSStr & "' Initialize remote server host name, protocol, port, etc." & vbCrLf
        VBSStr = VBSStr & "MySite.Host = ""ftp.gmgpulse.com""" & vbCrLf
        VBSStr = VBSStr & "MySite.Protocol = ""FTPS_AUTH_TLS""" & vbCrLf
        VBSStr = VBSStr & "MySite.Port = 21" & vbCrLf
        VBSStr = VBSStr & "MySite.Retries = 30" & vbCrLf
        VBSStr = VBSStr & "MySite.Delay = 30" & vbCrLf
        VBSStr = VBSStr & "MySite.MaxConnections = 4" & vbCrLf
        VBSStr = VBSStr & "MySite.TransferType = ""DEFAULT""" & vbCrLf
        VBSStr = VBSStr & "MySite.DataChannel = ""PASV""" & vbCrLf
        VBSStr = VBSStr & "MySite.ClearCommandChannel = false" & vbCrLf
        VBSStr = VBSStr & "MySite.ClearDataConnection = false" & vbCrLf
        VBSStr = VBSStr & "MySite.AutoRename = ""OFF""" & vbCrLf
        VBSStr = VBSStr & "' WARNING!!! SENSITIVE DATA: user name and password." & vbCrLf
        VBSStr = VBSStr & "MySite.Login = ""mycard-prod""" & vbCrLf
        VBSStr = VBSStr & "MySite.Password = ""MYcArdpROdUct!0n#""" & vbCrLf
        VBSStr = VBSStr & "MySite.SocksInfo = """"" & vbCrLf
        VBSStr = VBSStr & "MySite.ProxyInfo = """"" & vbCrLf
        VBSStr = VBSStr & "' CreateLocalFolder" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_HKG""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_IDN""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_MYS""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_PHL""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_SGP""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_THA""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_TWN""" & vbCrLf
        VBSStr = VBSStr & "MySite.CreateLocalFolder " & """D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_VNM""" & vbCrLf
        VBSStr = VBSStr & "' Connect to remote server" & vbCrLf
        VBSStr = VBSStr & "MySite.Connect" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_HKG/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_HKG""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_IDN/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_IDN""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_MYS/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_MYS""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_PHL/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_PHL""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_SGP/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_SGP""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_THA/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_THA""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_TWN/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_TWN""" & vbCrLf
        VBSStr = VBSStr & "MySite.Download " & """/GMG_-_Facebook_-_MyCard_-_VNM/Facebook_DailySalesDetails_" & Yesterday & ".csv"",""D:\FTPGMG\" & Yesterday & "\GMG_-_Facebook_-_MyCard_-_VNM""" & vbCrLf
        VBSStr = VBSStr & "MySite.Disconnect"
        Try
            File.WriteAllText(Application.StartupPath & "\VBS\" & VBSFilename, VBSStr, System.Text.Encoding.Default)
        Catch ex As Exception
            rtbInfo.Text = rtbInfo.Text & "產生vbs檔案時失敗，錯誤訊息：" & ex.Message & vbCrLf
            WriteLog(rtbInfo.Text)
            Exit Sub
        End Try
    End Sub
End Class
