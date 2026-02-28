
namespace beipin
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnReconnect = new System.Windows.Forms.Button();
            this.tbxVouNo = new System.Windows.Forms.TextBox();
            this.tbxComId = new System.Windows.Forms.TextBox();
            this.btnStartMark = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbxLaserIp = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLaser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusMes = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tbxLog = new System.Windows.Forms.RichTextBox();
            this.tmInitLaser = new System.Windows.Forms.Timer(this.components);
            this.tbxStopMark = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btnFileList = new System.Windows.Forms.Button();
            this.tbxFileName = new System.Windows.Forms.ComboBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.tbxSignNamebak = new System.Windows.Forms.ComboBox();
            this.tbxSignTextNameBak = new System.Windows.Forms.ComboBox();
            this.tbxSignName = new System.Windows.Forms.TextBox();
            this.tbxSignTextName = new System.Windows.Forms.TextBox();
            this.tbxSignTextName2 = new System.Windows.Forms.TextBox();
            this.tmReconnect = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label1.Location = new System.Drawing.Point(41, 91);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(126, 25);
            label1.TabIndex = 3;
            label1.Text = "当前派工单号";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label2.Location = new System.Drawing.Point(79, 37);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(88, 25);
            label2.TabIndex = 5;
            label2.Text = "当前组织";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label4.Location = new System.Drawing.Point(38, 152);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(126, 25);
            label4.TabIndex = 14;
            label4.Text = "选择打标文件";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label5.Location = new System.Drawing.Point(76, 211);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(88, 25);
            label5.TabIndex = 16;
            label5.Text = "条码标签";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label6.Location = new System.Drawing.Point(76, 268);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(88, 25);
            label6.TabIndex = 24;
            label6.Text = "明码标签";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label7.Location = new System.Drawing.Point(308, 268);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(99, 25);
            label7.TabIndex = 29;
            label7.Text = "明码标签2";
            // 
            // btnReconnect
            // 
            this.btnReconnect.BackColor = System.Drawing.SystemColors.Info;
            this.btnReconnect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReconnect.Location = new System.Drawing.Point(303, 515);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(102, 42);
            this.btnReconnect.TabIndex = 0;
            this.btnReconnect.Text = "打标机重连";
            this.btnReconnect.UseVisualStyleBackColor = false;
            this.btnReconnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbxVouNo
            // 
            this.tbxVouNo.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxVouNo.Location = new System.Drawing.Point(170, 88);
            this.tbxVouNo.Name = "tbxVouNo";
            this.tbxVouNo.Size = new System.Drawing.Size(245, 33);
            this.tbxVouNo.TabIndex = 2;
            this.tbxVouNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxVouNo_KeyDown);
            // 
            // tbxComId
            // 
            this.tbxComId.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxComId.Location = new System.Drawing.Point(170, 34);
            this.tbxComId.Name = "tbxComId";
            this.tbxComId.ReadOnly = true;
            this.tbxComId.Size = new System.Drawing.Size(245, 33);
            this.tbxComId.TabIndex = 4;
            this.tbxComId.Text = "SH37";
            // 
            // btnStartMark
            // 
            this.btnStartMark.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStartMark.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStartMark.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartMark.ForeColor = System.Drawing.SystemColors.Info;
            this.btnStartMark.Location = new System.Drawing.Point(52, 331);
            this.btnStartMark.Name = "btnStartMark";
            this.btnStartMark.Size = new System.Drawing.Size(353, 65);
            this.btnStartMark.TabIndex = 6;
            this.btnStartMark.Text = "开始打标";
            this.btnStartMark.UseVisualStyleBackColor = false;
            this.btnStartMark.Click += new System.EventHandler(this.btnStartLaser_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(421, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "<<- 扫码识别";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tbxLaserIp,
            this.statusLaser,
            this.toolStripStatusLabel2,
            this.statusMes,
            this.statusTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 590);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1153, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(233, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "打标机连接状态：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbxLaserIp
            // 
            this.tbxLaserIp.Name = "tbxLaserIp";
            this.tbxLaserIp.Size = new System.Drawing.Size(19, 17);
            this.tbxLaserIp.Text = "ip";
            // 
            // statusLaser
            // 
            this.statusLaser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLaser.Name = "statusLaser";
            this.statusLaser.Size = new System.Drawing.Size(233, 17);
            this.statusLaser.Spring = true;
            this.statusLaser.Text = "连接中";
            this.statusLaser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(233, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "MES连接状态：";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusMes
            // 
            this.statusMes.Name = "statusMes";
            this.statusMes.Size = new System.Drawing.Size(233, 17);
            this.statusMes.Spring = true;
            this.statusMes.Text = "连接中";
            this.statusMes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusTime
            // 
            this.statusTime.Name = "statusTime";
            this.statusTime.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.statusTime.Size = new System.Drawing.Size(186, 17);
            this.statusTime.Text = "当前时间：2024-01-01 10:00:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tbxLog
            // 
            this.tbxLog.BackColor = System.Drawing.Color.Black;
            this.tbxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.tbxLog.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxLog.ForeColor = System.Drawing.SystemColors.Info;
            this.tbxLog.Location = new System.Drawing.Point(608, 0);
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ReadOnly = true;
            this.tbxLog.Size = new System.Drawing.Size(545, 590);
            this.tbxLog.TabIndex = 9;
            this.tbxLog.Text = "";
            this.tbxLog.WordWrap = false;
            // 
            // tmInitLaser
            // 
            this.tmInitLaser.Interval = 1000;
            this.tmInitLaser.Tick += new System.EventHandler(this.tmInitLaser_Tick);
            // 
            // tbxStopMark
            // 
            this.tbxStopMark.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxStopMark.ForeColor = System.Drawing.Color.Red;
            this.tbxStopMark.Location = new System.Drawing.Point(52, 419);
            this.tbxStopMark.Name = "tbxStopMark";
            this.tbxStopMark.Size = new System.Drawing.Size(353, 65);
            this.tbxStopMark.TabIndex = 17;
            this.tbxStopMark.Text = "停止打标";
            this.tbxStopMark.UseVisualStyleBackColor = true;
            this.tbxStopMark.Click += new System.EventHandler(this.button2_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.Info;
            this.button8.Location = new System.Drawing.Point(177, 516);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(119, 42);
            this.button8.TabIndex = 19;
            this.button8.Text = "停止红光预览";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.Info;
            this.button7.Location = new System.Drawing.Point(52, 516);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(119, 42);
            this.button7.TabIndex = 18;
            this.button7.Text = "开始红光预览";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnFileList
            // 
            this.btnFileList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFileList.ForeColor = System.Drawing.Color.Blue;
            this.btnFileList.Location = new System.Drawing.Point(415, 304);
            this.btnFileList.Name = "btnFileList";
            this.btnFileList.Size = new System.Drawing.Size(141, 49);
            this.btnFileList.TabIndex = 20;
            this.btnFileList.Text = "选择打标文件";
            this.btnFileList.UseVisualStyleBackColor = true;
            this.btnFileList.Visible = false;
            this.btnFileList.Click += new System.EventHandler(this.btnFileList_Click);
            // 
            // tbxFileName
            // 
            this.tbxFileName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbxFileName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxFileName.FormattingEnabled = true;
            this.tbxFileName.Location = new System.Drawing.Point(170, 152);
            this.tbxFileName.MaxDropDownItems = 20;
            this.tbxFileName.Name = "tbxFileName";
            this.tbxFileName.Size = new System.Drawing.Size(245, 28);
            this.tbxFileName.TabIndex = 21;
            this.tbxFileName.SelectedIndexChanged += new System.EventHandler(this.tbxFileName_SelectedIndexChanged);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadFile.ForeColor = System.Drawing.Color.Blue;
            this.btnLoadFile.Location = new System.Drawing.Point(425, 152);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(93, 28);
            this.btnLoadFile.TabIndex = 22;
            this.btnLoadFile.Text = "重新加载文件";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // tbxSignNamebak
            // 
            this.tbxSignNamebak.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbxSignNamebak.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSignNamebak.FormattingEnabled = true;
            this.tbxSignNamebak.Location = new System.Drawing.Point(412, 372);
            this.tbxSignNamebak.MaxDropDownItems = 20;
            this.tbxSignNamebak.Name = "tbxSignNamebak";
            this.tbxSignNamebak.Size = new System.Drawing.Size(144, 28);
            this.tbxSignNamebak.TabIndex = 23;
            this.tbxSignNamebak.Visible = false;
            // 
            // tbxSignTextNameBak
            // 
            this.tbxSignTextNameBak.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbxSignTextNameBak.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSignTextNameBak.FormattingEnabled = true;
            this.tbxSignTextNameBak.Location = new System.Drawing.Point(415, 419);
            this.tbxSignTextNameBak.MaxDropDownItems = 20;
            this.tbxSignTextNameBak.Name = "tbxSignTextNameBak";
            this.tbxSignTextNameBak.Size = new System.Drawing.Size(141, 28);
            this.tbxSignTextNameBak.TabIndex = 25;
            this.tbxSignTextNameBak.Visible = false;
            // 
            // tbxSignName
            // 
            this.tbxSignName.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSignName.Location = new System.Drawing.Point(170, 208);
            this.tbxSignName.Name = "tbxSignName";
            this.tbxSignName.Size = new System.Drawing.Size(245, 33);
            this.tbxSignName.TabIndex = 26;
            // 
            // tbxSignTextName
            // 
            this.tbxSignTextName.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSignTextName.Location = new System.Drawing.Point(170, 265);
            this.tbxSignTextName.Name = "tbxSignTextName";
            this.tbxSignTextName.Size = new System.Drawing.Size(116, 33);
            this.tbxSignTextName.TabIndex = 27;
            // 
            // tbxSignTextName2
            // 
            this.tbxSignTextName2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbxSignTextName2.Location = new System.Drawing.Point(413, 265);
            this.tbxSignTextName2.Name = "tbxSignTextName2";
            this.tbxSignTextName2.Size = new System.Drawing.Size(102, 33);
            this.tbxSignTextName2.TabIndex = 28;
            // 
            // tmReconnect
            // 
            this.tmReconnect.Interval = 1500;
            this.tmReconnect.Tick += new System.EventHandler(this.tmReconnect_Tick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(415, 515);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 43);
            this.button1.TabIndex = 30;
            this.button1.Text = "PLC 调试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1153, 612);
            this.Controls.Add(this.button1);
            this.Controls.Add(label7);
            this.Controls.Add(this.tbxSignTextName2);
            this.Controls.Add(this.tbxSignTextName);
            this.Controls.Add(this.tbxSignName);
            this.Controls.Add(this.tbxSignTextNameBak);
            this.Controls.Add(label6);
            this.Controls.Add(this.tbxSignNamebak);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.tbxFileName);
            this.Controls.Add(this.btnFileList);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.tbxStopMark);
            this.Controls.Add(label5);
            this.Controls.Add(label4);
            this.Controls.Add(this.tbxLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStartMark);
            this.Controls.Add(label2);
            this.Controls.Add(this.tbxComId);
            this.Controls.Add(label1);
            this.Controls.Add(this.tbxVouNo);
            this.Controls.Add(this.btnReconnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "贝频上位机系统v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.TextBox tbxVouNo;
        private System.Windows.Forms.TextBox tbxComId;
        private System.Windows.Forms.Button btnStartMark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusMes;
        private System.Windows.Forms.ToolStripStatusLabel statusTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel statusLaser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.RichTextBox tbxLog;
        private System.Windows.Forms.Timer tmInitLaser;
        private System.Windows.Forms.Button tbxStopMark;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ToolStripStatusLabel tbxLaserIp;
        private System.Windows.Forms.Button btnFileList;
        private System.Windows.Forms.ComboBox tbxFileName;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.ComboBox tbxSignNamebak;
        private System.Windows.Forms.ComboBox tbxSignTextNameBak;
        private System.Windows.Forms.TextBox tbxSignName;
        private System.Windows.Forms.TextBox tbxSignTextName;
        private System.Windows.Forms.TextBox tbxSignTextName2;
        private System.Windows.Forms.Timer tmReconnect;
        private System.Windows.Forms.Button button1;
    }
}

