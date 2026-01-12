using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace beipin   // ★★★ 这里改成你自己项目的命名空间 ★★★
{
    public partial class FrmPlcCommunication : Form
    {
        #region 全局变量 - PLC核心对象
        /// <summary>
        /// S7.Net PLC核心通信对象
        /// </summary>
        private Plc _plc = null;

        /// <summary>
        /// 自动读取PLC数据的定时器(后台执行，不卡界面)
        /// </summary>
        private System.Windows.Forms.Timer _plcAutoReadTimer = new System.Windows.Forms.Timer();
        #endregion

        public FrmPlcCommunication()
        {
            InitializeComponent();
            // 初始化页面参数
            InitPageParam();
            // 初始化定时器事件
            InitAutoReadTimer();
            // 默认选中S7-1200
            cboCpuType.SelectedIndex = 0;
        }

        #region 初始化方法
        /// <summary>
        /// 初始化页面控件默认值
        /// </summary>
        private void InitPageParam()
        {
            // 未连接时，禁用读取相关控件
            btnReadOnce.Enabled = false;
            chkAutoRead.Enabled = false;

            // 日志框初始化
            txtPlcDataLog.AppendText($"【{DateTime.Now:yyyy-MM-dd HH:mm:ss}】PLC通信页面已加载，等待配置连接参数\r\n");
            txtPlcDataLog.AppendText("--------------------------------------------------------\r\n");
        }

        /// <summary>
        /// 初始化自动读取定时器
        /// </summary>
        private void InitAutoReadTimer()
        {
            // 绑定定时器触发事件
            _plcAutoReadTimer.Tick += PlcAutoReadTimer_Tick;
            // 设置默认间隔(毫秒)
            _plcAutoReadTimer.Interval = Convert.ToInt32(nudReadInterval.Value);
            // 默认关闭定时器
            _plcAutoReadTimer.Enabled = false;
        }
        #endregion

        #region PLC核心通信方法 - 连接/断开
        /// <summary>
        /// 连接PLC按钮点击事件
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 先判断是否已连接
                if (_plc != null && _plc.IsConnected)
                {
                    MessageBox.Show("PLC已处于连接状态，无需重复连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取页面选择的PLC型号
                CpuType cpuType = GetSelectCpuType();
                string plcIp = txtPlcIp.Text.Trim();
                int rack = Convert.ToInt32(nudRack.Value);
                int slot = Convert.ToInt32(nudSlot.Value);

                // 实例化PLC对象
                _plc = new Plc(cpuType, plcIp, (short)rack, (short)slot);
                // 打开PLC连接
                _plc.Open();

                // 判断连接状态
                if (_plc.IsConnected)
                {
                    lblConnStatus.Text = "已连接";
                    lblConnStatus.ForeColor = System.Drawing.Color.Green;
                    btnReadOnce.Enabled = true;
                    chkAutoRead.Enabled = true;
                    AddLog($"PLC连接成功 → IP:{plcIp} 机架:{rack} 槽号:{slot} 型号:{cpuType.ToString()}");
                    MessageBox.Show("PLC通信连接成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblConnStatus.Text = "连接失败";
                    lblConnStatus.ForeColor = System.Drawing.Color.Red;
                    AddLog("PLC连接失败，检查IP/机架号/槽号是否正确，或PLC是否在线！");
                    MessageBox.Show("PLC连接失败，请检查参数和PLC状态！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblConnStatus.Text = "连接异常";
                lblConnStatus.ForeColor = System.Drawing.Color.Red;
                AddLog($"PLC连接异常：{ex.Message}");
                MessageBox.Show($"PLC连接出错：{ex.Message}", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 断开PLC按钮点击事件
        /// </summary>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 停止自动读取定时器
                _plcAutoReadTimer.Enabled = false;
                chkAutoRead.Checked = false;

                // 断开PLC连接
                if (_plc != null && _plc.IsConnected)
                {
                    _plc.Close();
                   // _plc.Dispose();
                    _plc = null;
                }

                lblConnStatus.Text = "未连接";
                lblConnStatus.ForeColor = System.Drawing.Color.Red;
                btnReadOnce.Enabled = false;
                chkAutoRead.Enabled = false;
                AddLog("PLC已手动断开连接");
                MessageBox.Show("PLC已断开连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AddLog($"PLC断开异常：{ex.Message}");
                MessageBox.Show($"断开PLC出错：{ex.Message}", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取下拉框选择的PLC型号
        /// </summary>
        private CpuType GetSelectCpuType()
        {
            switch (cboCpuType.Text.Trim())
            {
                case "S7-300":
                    return CpuType.S7300;
                case "S7-400":
                    return CpuType.S7400;
                case "S7-1500":
                    return CpuType.S71500;
                default://默认S7-1200
                    return CpuType.S71200;
            }
        }
        #endregion

        #region PLC数据读取 - 手动读取 + 后台自动读取(核心)
        /// <summary>
        /// 手动读取一次PLC数据
        /// </summary>
        private async void btnReadOnce_Click(object sender, EventArgs e)
        {
            await ReadPlcDataAsync();
        }

        /// <summary>
        /// 定时器Tick事件 - 后台自动读取PLC数据
        /// </summary>
        private async void PlcAutoReadTimer_Tick(object sender, EventArgs e)
        {
            // 防止读取耗时超过间隔，导致任务堆积
            _plcAutoReadTimer.Enabled = false;
            await ReadPlcDataAsync();
            _plcAutoReadTimer.Enabled = true;
        }

        /// <summary>
        /// 自动读取开关勾选事件
        /// </summary>
        private void chkAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            if (!(_plc != null && _plc.IsConnected))
            {
                chkAutoRead.Checked = false;
                MessageBox.Show("请先连接PLC再开启自动读取！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 更新定时器间隔
            _plcAutoReadTimer.Interval = Convert.ToInt32(nudReadInterval.Value);
            // 开启/关闭定时器
            _plcAutoReadTimer.Enabled = chkAutoRead.Checked;

            if (chkAutoRead.Checked)
            {
                AddLog($"已开启PLC自动读取 → 读取间隔：{nudReadInterval.Value} 毫秒");
            }
            else
            {
                AddLog($"已关闭PLC自动读取");
            }
        }

        /// <summary>
        /// 【核心方法】异步读取PLC数据 - 防界面卡死，工业级必用
        /// </summary>
        private async Task ReadPlcDataAsync()
        {
            // 校验PLC连接状态
            if (_plc == null || !_plc.IsConnected)
            {
                AddLog("PLC未连接，读取数据失败");
                return;
            }

            try
            {
                // 异步执行读取操作，不阻塞UI线程，界面丝滑不卡
                await Task.Run(() =>
                {
                    #region ★★★ 【修改这里】替换成你自己的PLC信号地址，按需增删即可 ★★★
                    // 读取PLC信号 - 所有西门子PLC通用格式，S7.Net完美支持
                    bool m0_0 = (bool)_plc.Read("M0.0");    // 位信号：存储器区M0.0
                    bool i0_1 = (bool)_plc.Read("I0.1");    // 位信号：输入区I0.1
                    bool q0_2 = (bool)_plc.Read("Q0.2");    // 位信号：输出区Q0.2
                    byte db1_dbb0 = (byte)_plc.Read("DB1.DBB0");  // 字节：数据块1 字节0
                    short db1_dbw2 = (short)_plc.Read("DB1.DBW2");// 整数(16位)：数据块1 字2
                    int db1_dbd4 = (int)_plc.Read("DB1.DBD4");    // 双整数(32位)：数据块1 双字4
                    float db1_dbd8 = (float)_plc.Read("DB1.DBD8");// 浮点数(32位)：数据块1 双字8
                    bool db1_dbx10_0 = (bool)_plc.Read("DB1.DBX10.0");//数据块1 字节10 位0
                    #endregion

                    // 跨线程更新UI - WinForm必须用Invoke，否则报错
                    this.Invoke(new Action(() =>
                    {
                        // 清空旧日志（可选：如果想保留历史日志，注释掉这行即可）
                        txtPlcDataLog.Clear();
                        // 添加最新读取的数据
                        txtPlcDataLog.AppendText($"【{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}】PLC数据读取成功\r\n");
                        txtPlcDataLog.AppendText($"▷ 输入区：I0.1 = {i0_1}\r\n");
                        txtPlcDataLog.AppendText($"▷ 输出区：Q0.2 = {q0_2}\r\n");
                        txtPlcDataLog.AppendText($"▷ 存储器：M0.0 = {m0_0}\r\n");
                        txtPlcDataLog.AppendText($"▷ DB1.DBB0(字节) = {db1_dbb0}\r\n");
                        txtPlcDataLog.AppendText($"▷ DB1.DBW2(16位整数) = {db1_dbw2}\r\n");
                        txtPlcDataLog.AppendText($"▷ DB1.DBD4(32位整数) = {db1_dbd4}\r\n");
                        txtPlcDataLog.AppendText($"▷ DB1.DBD8(浮点数) = {db1_dbd8:F2}\r\n");
                        txtPlcDataLog.AppendText($"▷ DB1.DBX10.0(数据块位) = {db1_dbx10_0}\r\n");
                        txtPlcDataLog.AppendText("--------------------------------------------------------\r\n");

                        // 日志框自动滚动到最新内容
                        txtPlcDataLog.SelectionStart = txtPlcDataLog.Text.Length;
                        txtPlcDataLog.ScrollToCaret();
                    }));
                });
            }
            catch (Exception ex)
            {
                // 读取异常，添加日志并提示
                AddLog($"PLC数据读取失败：{ex.Message}");
            }
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 添加日志到文本框
        /// </summary>
        private void AddLog(string logMsg)
        {
            txtPlcDataLog.AppendText($"【{DateTime.Now:yyyy-MM-dd HH:mm:ss}】{logMsg}\r\n");
            txtPlcDataLog.SelectionStart = txtPlcDataLog.Text.Length;
            txtPlcDataLog.ScrollToCaret();
        }
        #endregion

        #region 窗体关闭 - 释放资源，防止内存泄漏+PLC连接残留
        private void FrmPlcCommunication_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止定时器
            if (_plcAutoReadTimer != null)
            {
                _plcAutoReadTimer.Enabled = false;
                _plcAutoReadTimer.Dispose();
            }

            // 断开PLC连接并释放对象
            if (_plc != null)
            {
                if (_plc.IsConnected)
                {
                    _plc.Close();
                }
                //_plc.Dispose();
                _plc = null;
            }

            AddLog("PLC通信页面关闭，所有资源已释放");
        }
        #endregion
    }
}