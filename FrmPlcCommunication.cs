using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace beipin   // ★★★ 必须改成你自己的项目命名空间 ★★★
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
            // 初始化页面参数（替代Load事件，避免报错）
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

            // 数据类型下拉框默认选中Bool
            cboDataType.SelectedIndex = 0;
            // 读取地址默认值
            txtPlcAddress.Text = "M0.0";

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
                _plc = new Plc(cpuType, plcIp,(short) rack,(short) slot);
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
                    //_plc.Dispose();
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

        #region PLC数据读取 - 手动读取 + 后台自动读取(核心，动态地址版)
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
        /// 【核心方法】异步读取PLC数据 - 动态地址+动态数据类型（防界面卡死）
        /// </summary>
        private async Task ReadPlcDataAsync()
        {
            // 校验PLC连接状态
            if (_plc == null || !_plc.IsConnected)
            {
                AddLog("PLC未连接，读取数据失败");
                return;
            }

            // 1. 输入验证：地址和数据类型不能为空
            string plcAddress = txtPlcAddress.Text.Trim();
            if (string.IsNullOrEmpty(plcAddress))
            {
                AddLog("读取失败：PLC读取地址不能为空！");
                return;
            }
            if (cboDataType.SelectedIndex == -1)
            {
                AddLog("读取失败：请选择数据类型！");
                return;
            }

            try
            {
                // 异步执行读取操作，不阻塞UI线程
                await Task.Run(() =>
                {
                    object readValue = null; // 存储读取的原始值
                    string dataType = cboDataType.Text.Trim();

                    // 2. 根据选择的类型读取对应地址的数据
                    switch (dataType)
                    {
                        case "Bool":
                            readValue = (bool)_plc.Read(plcAddress);
                            break;
                        case "Byte":
                            readValue = (byte)_plc.Read(plcAddress);
                            break;
                        case "Int16":
                            readValue = (short)_plc.Read(plcAddress);
                            break;
                        case "Int32":
                            readValue = (int)_plc.Read(plcAddress);
                            break;
                        case "Float":
                            readValue = (float)_plc.Read(plcAddress);
                            break;
                        default:
                            readValue = "不支持的数据类型";
                            break;
                    }

                    // 3. 跨线程更新UI显示结果
                    this.Invoke(new Action(() =>
                    {
                        txtPlcDataLog.Clear(); // 清空旧日志
                        // 显示读取结果（带时间戳+地址+类型+值）
                        txtPlcDataLog.AppendText($"【{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}】PLC数据读取成功\r\n");
                        txtPlcDataLog.AppendText($"▷ 读取地址：{plcAddress}\r\n");
                        txtPlcDataLog.AppendText($"▷ 数据类型：{dataType}\r\n");
                        txtPlcDataLog.AppendText($"▷ 读取值：{readValue}\r\n");
                        txtPlcDataLog.AppendText("--------------------------------------------------------\r\n");

                        // 自动滚动到最新内容
                        txtPlcDataLog.SelectionStart = txtPlcDataLog.Text.Length;
                        txtPlcDataLog.ScrollToCaret();
                    }));
                });
            }
            catch (Exception ex)
            {
                // 捕获所有异常（地址错误/类型不匹配/PLC离线等）
                AddLog($"PLC数据读取失败：{ex.Message}");
                // 常见异常提示（帮助排查问题）
                if (ex.Message.Contains("does not exist"))
                {
                    AddLog("❌ 可能原因：PLC地址不存在或格式错误！");
                }
                else if (ex.Message.Contains("cast"))
                {
                    AddLog("❌ 可能原因：数据类型选择错误（如地址是Bool却选了Byte）！");
                }
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

        private void txtPlcAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cboDataType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}