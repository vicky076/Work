namespace WinFormeplayAd
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rtbInfo = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnHandRun = new System.Windows.Forms.Button();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // rtbInfo
            // 
            this.rtbInfo.Location = new System.Drawing.Point(12, 72);
            this.rtbInfo.Multiline = true;
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.Size = new System.Drawing.Size(386, 128);
            this.rtbInfo.TabIndex = 8;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(39, 12);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(102, 34);
            this.Button2.TabIndex = 7;
            this.Button2.Text = "程式啟動/停止";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(10, 57);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(65, 12);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "執行結果：";
            // 
            // btnHandRun
            // 
            this.btnHandRun.Location = new System.Drawing.Point(179, 14);
            this.btnHandRun.Name = "btnHandRun";
            this.btnHandRun.Size = new System.Drawing.Size(102, 30);
            this.btnHandRun.TabIndex = 5;
            this.btnHandRun.Text = "手動執行";
            this.btnHandRun.UseVisualStyleBackColor = true;
            this.btnHandRun.Click += new System.EventHandler(this.btnHandRun_Click);
            // 
            // Timer1
            // 
            this.Timer1.Interval = 10000;
            this.Timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 209);
            this.Controls.Add(this.rtbInfo);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnHandRun);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox rtbInfo;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnHandRun;
        private System.Windows.Forms.Timer Timer1;
    }
}

