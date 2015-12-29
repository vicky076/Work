<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnHandRun = New System.Windows.Forms.Button()
        Me.rtbInfo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(26, 33)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(111, 51)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "程式啟動/停止"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnHandRun
        '
        Me.btnHandRun.Location = New System.Drawing.Point(168, 35)
        Me.btnHandRun.Name = "btnHandRun"
        Me.btnHandRun.Size = New System.Drawing.Size(111, 47)
        Me.btnHandRun.TabIndex = 10
        Me.btnHandRun.Text = "手動執行"
        Me.btnHandRun.UseVisualStyleBackColor = True
        '
        'rtbInfo
        '
        Me.rtbInfo.Location = New System.Drawing.Point(26, 133)
        Me.rtbInfo.Multiline = True
        Me.rtbInfo.Name = "rtbInfo"
        Me.rtbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.rtbInfo.Size = New System.Drawing.Size(313, 143)
        Me.rtbInfo.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 103)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "執行結果："
        '
        'Timer1
        '
        Me.Timer1.Interval = 10000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 300)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.rtbInfo)
        Me.Controls.Add(Me.btnHandRun)
        Me.Controls.Add(Me.Button2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnHandRun As System.Windows.Forms.Button
    Friend WithEvents rtbInfo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
