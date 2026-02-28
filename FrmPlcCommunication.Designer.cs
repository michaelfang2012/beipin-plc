namespace beipin   // ★★★ 必须改成你自己的项目命名空间 ★★★
{
    partial class FrmPlcCommunication
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboCpuType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudSlot = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRack = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPlcIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPlcAddress = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboDataType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudReadInterval = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAutoRead = new System.Windows.Forms.CheckBox();
            this.btnReadOnce = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtPlcDataLog = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReadInterval)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboCpuType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudSlot);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudRack);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPlcIp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(570, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PLC通讯参数配置";
            // 
            // cboCpuType
            // 
            this.cboCpuType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCpuType.FormattingEnabled = true;
            this.cboCpuType.Items.AddRange(new object[] {
            "S7-1200",
            "S7-1500",
            "S7-300",
            "S7-400"});
            this.cboCpuType.Location = new System.Drawing.Point(450, 24);
            this.cboCpuType.Margin = new System.Windows.Forms.Padding(2);
            this.cboCpuType.Name = "cboCpuType";
            this.cboCpuType.Size = new System.Drawing.Size(91, 20);
            this.cboCpuType.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(405, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "PLC型号:";
            // 
            // nudSlot
            // 
            this.nudSlot.Location = new System.Drawing.Point(352, 24);
            this.nudSlot.Margin = new System.Windows.Forms.Padding(2);
            this.nudSlot.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSlot.Name = "nudSlot";
            this.nudSlot.Size = new System.Drawing.Size(38, 21);
            this.nudSlot.TabIndex = 5;
            this.nudSlot.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "槽号:";
            // 
            // nudRack
            // 
            this.nudRack.Location = new System.Drawing.Point(270, 24);
            this.nudRack.Margin = new System.Windows.Forms.Padding(2);
            this.nudRack.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRack.Name = "nudRack";
            this.nudRack.Size = new System.Drawing.Size(38, 21);
            this.nudRack.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "机架:";
            // 
            // txtPlcIp
            // 
            this.txtPlcIp.Location = new System.Drawing.Point(60, 24);
            this.txtPlcIp.Margin = new System.Windows.Forms.Padding(2);
            this.txtPlcIp.Name = "txtPlcIp";
            this.txtPlcIp.Size = new System.Drawing.Size(166, 21);
            this.txtPlcIp.TabIndex = 1;
            this.txtPlcIp.Text = "192.168.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP地址:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblConnStatus);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnDisConnect);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Location = new System.Drawing.Point(9, 78);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(570, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "通讯控制";
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.AutoSize = true;
            this.lblConnStatus.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblConnStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnStatus.Location = new System.Drawing.Point(488, 24);
            this.lblConnStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(55, 15);
            this.lblConnStatus.TabIndex = 3;
            this.lblConnStatus.Text = "未连接";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(425, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "通讯状态:";
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(135, 20);
            this.btnDisConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(75, 24);
            this.btnDisConnect.TabIndex = 1;
            this.btnDisConnect.Text = "断开PLC";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(45, 20);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 24);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "连接PLC";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPlcAddress);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cboDataType);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.nudReadInterval);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.chkAutoRead);
            this.groupBox3.Controls.Add(this.btnReadOnce);
            this.groupBox3.Location = new System.Drawing.Point(9, 139);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(570, 105);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据读取设置";
            // 
            // txtPlcAddress
            // 
            this.txtPlcAddress.Location = new System.Drawing.Point(265, 29);
            this.txtPlcAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtPlcAddress.Name = "txtPlcAddress";
            this.txtPlcAddress.Size = new System.Drawing.Size(91, 21);
            this.txtPlcAddress.TabIndex = 7;
            this.txtPlcAddress.Text = "M0.0";
            this.txtPlcAddress.TextChanged += new System.EventHandler(this.txtPlcAddress_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(184, 32);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "PLC读取地址:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // cboDataType
            // 
            this.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Items.AddRange(new object[] {
            "Bool",
            "Byte",
            "Int16",
            "Int32",
            "Float"});
            this.cboDataType.Location = new System.Drawing.Point(80, 29);
            this.cboDataType.Margin = new System.Windows.Forms.Padding(2);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(76, 20);
            this.cboDataType.TabIndex = 5;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 32);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "数据类型:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // nudReadInterval
            // 
            this.nudReadInterval.Location = new System.Drawing.Point(233, 68);
            this.nudReadInterval.Margin = new System.Windows.Forms.Padding(2);
            this.nudReadInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudReadInterval.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudReadInterval.Name = "nudReadInterval";
            this.nudReadInterval.Size = new System.Drawing.Size(75, 21);
            this.nudReadInterval.TabIndex = 3;
            this.nudReadInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(134, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "读取间隔(毫秒):";
            // 
            // chkAutoRead
            // 
            this.chkAutoRead.AutoSize = true;
            this.chkAutoRead.Location = new System.Drawing.Point(324, 69);
            this.chkAutoRead.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoRead.Name = "chkAutoRead";
            this.chkAutoRead.Size = new System.Drawing.Size(96, 16);
            this.chkAutoRead.TabIndex = 1;
            this.chkAutoRead.Text = "开启自动读取";
            this.chkAutoRead.UseVisualStyleBackColor = true;
            this.chkAutoRead.CheckedChanged += new System.EventHandler(this.chkAutoRead_CheckedChanged);
            // 
            // btnReadOnce
            // 
            this.btnReadOnce.Location = new System.Drawing.Point(28, 64);
            this.btnReadOnce.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadOnce.Name = "btnReadOnce";
            this.btnReadOnce.Size = new System.Drawing.Size(75, 24);
            this.btnReadOnce.TabIndex = 0;
            this.btnReadOnce.Text = "手动读取一次";
            this.btnReadOnce.UseVisualStyleBackColor = true;
            this.btnReadOnce.Click += new System.EventHandler(this.btnReadOnce_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtPlcDataLog);
            this.groupBox4.Location = new System.Drawing.Point(9, 248);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(570, 208);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "PLC信号读取日志(实时)";
            // 
            // txtPlcDataLog
            // 
            this.txtPlcDataLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlcDataLog.Location = new System.Drawing.Point(2, 16);
            this.txtPlcDataLog.Margin = new System.Windows.Forms.Padding(2);
            this.txtPlcDataLog.Multiline = true;
            this.txtPlcDataLog.Name = "txtPlcDataLog";
            this.txtPlcDataLog.ReadOnly = true;
            this.txtPlcDataLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPlcDataLog.Size = new System.Drawing.Size(566, 190);
            this.txtPlcDataLog.TabIndex = 0;
            // 
            // FrmPlcCommunication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 466);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "FrmPlcCommunication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "西门子PLC S7.NET通信监控（动态地址版）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPlcCommunication_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReadInterval)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboCpuType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudSlot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudRack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPlcIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPlcAddress;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboDataType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudReadInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAutoRead;
        private System.Windows.Forms.Button btnReadOnce;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtPlcDataLog;
    }
}