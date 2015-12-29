Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Data
Imports System
Imports System.Web
Imports System.Xml

Public Class Form1
    Private TimerEnable As Boolean = True
    Private ProcessStatus As Boolean = True

    Dim wsShopMallService As New ShopMallService.ShopMallService
    Dim xmlcode As String = ""
    Dim xmlFile As String = Application.StartupPath & "\SitemapXML\"
    Dim copyFile As String = Application.StartupPath & "\copyXML\"

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
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        btnHandRun_Click(sender, e)
    End Sub

    '自動執行
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Select Case Timer1.Enabled
            Case False
                rtbInfo.Text = rtbInfo.Text & "計時器啟動..." & vbCrLf
                Timer1.Enabled = True
                btnHandRun.Enabled = False
                TimerEnable = True
            Case True
                rtbInfo.Text = rtbInfo.Text & "計時器終止..." & vbCrLf
                WriteLog("＝＝＝ 時間： " & Now.ToString("yyyy-MM-dd HH:mm:ss") & "，中止程式執行。 ＝＝＝")
                WriteLog("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝")
                Timer1.Enabled = False
                btnHandRun.Enabled = True
                TimerEnable = False
        End Select
    End Sub

    '手動
    Private Sub btnHandRun_Click(sender As System.Object, e As System.EventArgs) Handles btnHandRun.Click
        rtbInfo.Text = rtbInfo.Text & "執行結果：" & vbCrLf
        Dim ds1 As New DataSet
        Dim ds2 As New DataSet
        Dim ds3 As New DataSet
        Dim ds2sec As New DataSet
        Dim xloc, xchangefreq, xpriority As String

        Try
            ds1 = GetStoreType()
        Catch ex As Exception
            WriteLog("查詢失敗！ds1：" & ex.ToString())
            rtbInfo.Text = rtbInfo.Text & "ds1取資料發生錯誤，" & ex.Message & vbCrLf
            ProcessStatus = False
            Timer1.Enabled = True
            Exit Sub
        End Try

        WriteLog("開始查資料，組XML")
        If ds1.Tables(0).Rows.Count > 0 Then
            xmlcode = "<?xml version='1.0' encoding='UTF-8' ?>" & vbCrLf
            xmlcode &= "<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>" & vbCrLf
            xmlcode = CreateXML(My.Settings.webUrl, "always", "1")

            For i = 0 To ds1.Tables(0).Rows.Count - 1
                xloc = My.Settings.webUrl & "/ProductCategory/ProductCategory.aspx?TypeSn=" & ds1.Tables(0).Rows(i)("Sn").ToString().Trim()
                xchangefreq = "monthly"
                xpriority = "0.5"
                xmlcode = CreateXML(xloc, xchangefreq, xpriority)

                Try
                    ds2 = GetDataSn(ds1.Tables(0).Rows(i)("Sn"))
                Catch ex As Exception
                    WriteLog("ds2查詢失敗|SN=" & ds1.Tables(0).Rows(i)("Sn") & "|" & ex.ToString())
                    rtbInfo.Text = rtbInfo.Text & "ds2取資料發生錯誤，" & ex.Message & vbCrLf
                    ProcessStatus = False
                    Timer1.Enabled = True
                    Exit Sub
                End Try

                If ds2.Tables(0).Rows.Count > 0 Then
                    Dim DataView As DataView = ds2.Tables(0).DefaultView
                    DataView.RowFilter = "ParentSn=0  And StoreTypeSn = " & ds1.Tables(0).Rows(i)("Sn")

                    For Each drvCat As DataRowView In DataView
                        Try
                            ds3 = MSM_SP_GetProduct_FromProductTypeSn(drvCat("Sn"))
                        Catch ex As Exception
                            WriteLog("ds3查詢失敗|SN=" & drvCat("Sn") & "|" & ex.ToString())
                            rtbInfo.Text = rtbInfo.Text & "ds3取資料發生錯誤，" & ex.Message & vbCrLf
                            ProcessStatus = False
                            Timer1.Enabled = True
                            Exit Sub
                        End Try

                        If (ds3.Tables().Count > 0) Then
                            If ds3.Tables(0).Rows.Count > 0 Then
                                xloc = My.Settings.webUrl & "/ProductCategory/ProductCategory.aspx?prdCategory=" & drvCat("Sn").ToString().Trim() & "&amp;TypeSn=" & ds1.Tables(0).Rows(i)("Sn").ToString().Trim()
                                xchangefreq = "monthly"
                                xpriority = "0.5"
                                xmlcode = CreateXML(xloc, xchangefreq, xpriority)

                                For j = 0 To ds3.Tables(0).Rows.Count - 1
                                    xloc = My.Settings.webUrl & "/ProductCategory/product_sale.aspx?prdId=" & ds3.Tables(0).Rows(j)("ProductId").ToString().Trim()
                                    xchangefreq = "weekly"
                                    xpriority = "1"
                                    xmlcode = CreateXML(xloc, xchangefreq, xpriority)
                                Next
                            End If
                        End If
                    Next
                End If
            Next

            xmlcode &= "</urlset>"
        Else
            WriteLog("無資料產生xml")
            rtbInfo.Text = rtbInfo.Text & "無資料執行。" & vbCrLf
            ProcessStatus = False
            Timer1.Enabled = True
            rtbInfo.Text = rtbInfo.Text & "程式將在10秒後關閉..." & vbCrLf
            Me.Close()
        End If

        WriteLog("產完XML")

        '存檔
        If xmlcode <> "" Then
            Dim folderExists As Boolean
            Dim copyExists As Boolean
            '檔案名稱 : 20070801.xml
            Dim Name = Date.Now.ToString("yyyyMMdd") & ".xml"
            Try
                '檢查:資料夾是否存在(若沒有則建立它,反則刪除裡面檔案) 
                folderExists = My.Computer.FileSystem.DirectoryExists(xmlFile)
                If folderExists = False Then
                    My.Computer.FileSystem.CreateDirectory(xmlFile)
                End If

                '檢查:檔案是否存在(若沒有則建立它)
                Dim fileExists As Boolean
                fileExists = My.Computer.FileSystem.FileExists(xmlFile & "Sitemap.xml")
                If fileExists = False Then
                    My.Computer.FileSystem.WriteAllText(xmlFile & "Sitemap.xml", String.Empty, False)
                Else
                    Try
                        copyExists = My.Computer.FileSystem.DirectoryExists(copyFile)
                        If copyExists = False Then
                            My.Computer.FileSystem.CreateDirectory(copyFile)
                        End If

                        Dim copyfileExists = My.Computer.FileSystem.FileExists(copyFile & Name)
                        If copyfileExists = True Then
                            My.Computer.FileSystem.DeleteFile(copyFile & Name)
                        End If
                        My.Computer.FileSystem.MoveFile(xmlFile & "Sitemap.xml", copyFile & Name)

                    Catch ex As Exception
                        rtbInfo.Text = rtbInfo.Text & "建立copyXML發生錯誤:" & vbCrLf & ex.Message & vbCrLf
                        WriteLog("建立copyXML發生錯誤|" & ex.Message)
                        ProcessStatus = False
                        Timer1.Enabled = True
                    End Try
                End If
                My.Computer.FileSystem.WriteAllText(xmlFile & "Sitemap.xml", xmlcode & vbCrLf, True)
            Catch ex As Exception
                rtbInfo.Text = rtbInfo.Text & "建立SitemapXML發生錯誤:" & vbCrLf & ex.Message & vbCrLf
                WriteLog("建立SitemapXML發生錯誤|" & ex.Message)
                ProcessStatus = False
                Timer1.Enabled = True
            End Try

            Try
            Catch ex As Exception
                rtbInfo.Text = rtbInfo.Text & "建立copyXML發生錯誤:" & vbCrLf & ex.Message & vbCrLf
                WriteLog("建立copyXML發生錯誤|" & ex.Message)
                ProcessStatus = False
                Timer1.Enabled = True
            End Try
        End If

        rtbInfo.Text = "執行成功。"
        WriteLog("執行成功")
        ProcessStatus = False
        Me.Close()
    End Sub

    '產XML
    Public Function CreateXML(ByVal loc As String, ByVal changefreq As String, ByVal priority As String)
        xmlcode &= "<url>" & vbCrLf
        xmlcode &= "<loc>" & loc & "</loc>" & vbCrLf
        xmlcode &= "<changefreq>" & changefreq & "</changefreq>" & vbCrLf
        xmlcode &= "<priority>" & priority & "</priority>" & vbCrLf
        xmlcode &= "</url>" & vbCrLf
        Return xmlcode
    End Function

    '產品第一層
    Public Function GetStoreType()
        Dim ds As New DataSet
        Try
            ds = wsShopMallService.ViewGetStoreType()
        Catch ex As Exception
            WriteLog("查詢失敗！GetStoreType：" & ex.ToString())
            Return ""
            Exit Function
        End Try

        Return ds
    End Function

    '產品第二層
    Public Function GetDataSn(ByVal Sn As Integer)
        Dim ds As New DataSet
        Try
            ds = wsShopMallService.ViewGetProductType(Sn)
        Catch ex As Exception
            WriteLog("查詢失敗！GetDataSn：" & ex.ToString())
            Return ""
            Exit Function
        End Try

        Return ds
    End Function

    '產品第二層(子層商品)
    Public Function GetSecDataSn(ByVal Sn As Integer)
        Dim ds As New ShopMallService.ReturnValue
        Try
            ds = wsShopMallService.MSMSPGetProductFromStoreTypeSn(Sn, 2000, 1, "click")
        Catch ex As Exception
            WriteLog("查詢失敗！GetDataSn：" & ex.ToString())
            Return ""
            Exit Function
        End Try

        Return ds.ReturnDS
    End Function


    '產品第三層
    Public Function MSM_SP_GetProduct_FromProductTypeSn(ByVal ProductTypeSn As String)
        Dim ds As New ShopMallService.MSM_SP_GetProduct_FromProductTypeSnResult
        Try
            ds = wsShopMallService.MSM_SP_GetProduct_FromProductTypeSn(ProductTypeSn, 2000, 1)
        Catch ex As Exception
            WriteLog("查詢失敗！MSM_SP_GetProduct_FromProductTypeSn：" & ex.ToString())
            Return ""
            Exit Function
        End Try

        Return ds.ReturnDs
    End Function
End Class
