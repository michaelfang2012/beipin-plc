using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S7;
using S7.Net;

namespace beipin
{
    public partial class FrmMain : Form
    {
        //三花服务
        private SanhuaApiService.SANHUAOpen sanhuaOpen = new SanhuaApiService.SANHUAOpen();

        private MyAccess m_Access = new MyAccess();

        private int iLaserStatus = 0;

        private string sLaserIp = "127.0.0.1";

        private int iLaserPort = 2000;

        private string sMesUrl = "";

        //打标超时等待时间
        private int timeout = 30000;

        private Keys hotkey;

        IniFile iniFile;

        private bool isPrint = false;

        // PLC核心通信对象
        private Plc _plc;

        // 存储Excel中1-5行的PLC状态（全局变量）
        private string _scanQrCode;          // 1.上传-扫描二维码（String[50]）
        private string _station2Status;      // 2.上传-工位2状态（Char）
        private string _station3Status;      // 3.上传-工位3状态（Char）
        private string _station4Status;      // 4.上传-工位4状态（Char）
        private ushort _cameraJudgeStatus;    // 5.上传-相机判断状态（Word）
        private string _qrCodeLevel;         // 9-二维码等级

        // 标记是否正在处理数据（避免重复触发）
        private bool _isProcessing = false;

        // 后台轮询定时器
        private System.Windows.Forms.Timer _pollTimer;

        // SQL Server 辅助类（新增）
        private SqlHelper _sqlHelper;

        //扫码枪图片路径
        private string qrImgPath;
        //相机图片路径
        private string cameraImgPath;
        //FTP路径
        private string ftpPath;


        public FrmMain()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            iLaserStatus = 0;
            tmInitLaser.Start();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            
            iniFile = new IniFile(Application.StartupPath + @"\config.ini");

            qrImgPath = iniFile.IniReadValue("参数设置", "扫码枪图片路径");
            cameraImgPath = iniFile.IniReadValue("参数设置", "相机图片路径");
            ftpPath = iniFile.IniReadValue("参数设置", "FTP路径");


            sLaserIp = iniFile.IniReadValue("参数设置", "激光设备IP");

            iLaserPort = int.Parse(iniFile.IniReadValue("参数设置", "激光设备端口"));

            timeout = int.Parse(iniFile.IniReadValue("参数设置", "打标时间"));

            tbxLaserIp.Text = sLaserIp;

//            loadFile();

            tbxSignName.Text = iniFile.IniReadValue("参数设置", "条码标签");
            
            if(tbxSignName.Text.Length==0)
            {
               // tbxSignName.Text = "条形码/二维码";
            }

            tbxSignTextName.Text = iniFile.IniReadValue("参数设置", "明码标签");

            if (tbxSignTextName.Text.Length == 0)
            {
              //  tbxSignTextName.Text = "SN";
            }

            tbxSignTextName2.Text = iniFile.IniReadValue("参数设置", "明码标签2");

            if (tbxSignTextName2.Text.Length == 0)
            {
              //  tbxSignTextName2.Text = "SN2";
            }

            string sKey = iniFile.IniReadValue("参数设置", "打标快捷键");
            if(sKey.Length==0)
            {
                sKey = "F2";
            }
            btnStartMark.Text = "开始打标-"+sKey;
            // 将字符串形式的快捷键转换为Keys枚举
            KeysConverter keysConverter = new KeysConverter();
            if (keysConverter.IsValid(sKey))
            {
                hotkey = (Keys)keysConverter.ConvertFromString(sKey);
            }
            else
            {
                // 默认快捷键设置为 F2
                hotkey = Keys.F2;
            }

            sMesUrl = iniFile.IniReadValue("参数设置", "MES系统API");

            sanhuaOpen.Url = sMesUrl;

//            if (!this.m_Access.ConnectToDatabase(Application.StartupPath + @"\db.mdb"))
//            {
//                MessageBox.Show("连接数据库失败，请确认数据库文件是否存在：" + Application.StartupPath + @"\db.mdb");
//                base.Close();
//            }

            timer1.Start();

            tmInitLaser.Start();

            // ====== 新增：读取SQL Server配置（从INI） ======
            string sqlServer = "localhost"; // SQL服务器地址
            string sqlDb = "beipin";      // 数据库名
            string sqlUser = "sa";            // 登录账号
            string sqlPwd = "1234";          // 登录密码
                                                                                    // 拼接SQL连接字符串（SQL Server 2014）
            string sqlConnStr = $"Data Source={sqlServer};Initial Catalog={sqlDb};User ID={sqlUser};Password={sqlPwd};Integrated Security=False;";
            // 初始化SQL辅助类
            _sqlHelper = new SqlHelper(sqlConnStr);


            try
            {
                // 读取PLC配置（对应config.ini的[PLCConfig]节点）
                string plcIp = iniFile.IniReadValue("PLCConfig", "IP");
                short rack = short.Parse(iniFile.IniReadValue("PLCConfig", "Rack"));
                short slot = short.Parse(iniFile.IniReadValue("PLCConfig", "Slot"));
                string cpuTypeStr = iniFile.IniReadValue("PLCConfig", "CpuType");
                // 步骤2：初始化PLC对象 + 异步连接（避免卡界面）
                CpuType cpuType = GetCpuType(cpuTypeStr);
                _plc = new Plc(cpuType, plcIp, rack, slot);

                await Task.Run(() => _plc.Open());
                if (_plc.IsConnected)
                {
                    saveLog("PLC后台连接成功:" + plcIp);
                    //MessageBox.Show("PLC后台连接成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // 启动后台轮询定时器
                    InitPollTimer();
                }
                else
                {
                    MessageBox.Show("PLC连接失败，请检查参数/网络！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"PLC初始化失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitPollTimer()
        {
            _pollTimer = new System.Windows.Forms.Timer();
            _pollTimer.Interval = 1000; // 轮询间隔1秒（可调整）
            _pollTimer.Tick += PollTimer_Tick;
            _pollTimer.Start();
        }

        private async void PollTimer_Tick(object sender, EventArgs e)
        {
            // 避免重复处理
            if (_isProcessing) return;

            // 检查PLC连接状态（断开则尝试重连）
            if (_plc == null || !_plc.IsConnected)
            {
                try
                {
                    await Task.Run(() => _plc.Open());
                    if (!_plc.IsConnected)
                    {
                        saveLog("PLC已断开，重连失败！");
                    }
                }
                catch (Exception ex)
                {
                    saveLog($"PLC重连异常：{ex.Message}");
                }
                return;
            }


            try
            {

                ushort startMarkFlag = ReadPlcWord("DB29.DBW68.0");
                if(startMarkFlag == 1)
                {
                    _isProcessing = true;

                    saveLog("接收到打标指令，流程开始...");
/*                    if (tbxVouNo.Text.Length == 0)
                    {
                        MessageBox.Show("工单信息为空，请输入或扫码！");
                        tbxVouNo.Focus();
                        return;
                    }*/
                    
                    isPrint = true;
                    try
                    {

                        if (tbxVouNo.Text.Length == 0)
                        {
                            MessageBox.Show("工单信息为空，请输入或扫码！");
                            tbxVouNo.Focus();
                            return;
                        }
                        if (tbxFileName.Text.Length == 0)
                        {
                            MessageBox.Show("请选择打标文件");
                            tbxFileName.Focus();
                            return;
                        }
                        if (tbxSignName.Text.Length == 0)
                        {
                            MessageBox.Show("请选择条码标签");
                            tbxSignName.Focus();
                            return;
                        }

                        //2、扫描工单申请条码
                        saveLog("开始向MES申请...");


                        string barListString = sanhuaOpen.WorkOrderApplyBarcode(tbxComId.Text, tbxVouNo.Text, 1);

                         if (String.IsNullOrEmpty(barListString))
                         {
                             saveLog("条码申请失败");
                             btnStartMark.Enabled = true;
                             return;
                         }
                         barListString = barListString.Replace("\\n", "");

                         saveLog("MES申请条码成功：" + barListString);

                         JArray jarr = JArray.Parse(barListString);
                         if (jarr.Count == 0)
                         {
                             saveLog("条码申请失败");
                             btnStartMark.Enabled = true;
                             return;
                         }
                         string barNo = jarr[0]["bar_no"].ToString();
                         saveLog("barNo:" + barNo);


                        // 获取 content 字段
                        JObject firstObject = (JObject)jarr[0];
                        JObject contentObject = (JObject)firstObject["content"];

                        // 获取 lot_no 字段的值
                        string lotNo = "";

                            lotNo = contentObject["lot_no"].ToString();
                            saveLog("lotNo:" + lotNo);
                       

                        // 获取 lot_no 字段的值
                        string ssNo = "";
                            ssNo = contentObject["ss_no"].ToString();
                            saveLog("ssNo:" + ssNo);
                     

                            //3、条码下发给设备时上传当前条码状态
                            sanhuaOpen.WorkOrderBarcodeWaitPrintFinsh(tbxComId.Text, barNo);

                        // string barNo = "1234567890";

                        //saveLog("下发指令至PLC...");

                        //WritePlcString("DB29.DBX256.0",barNo,barNo.Length);

                        //saveLog("下发二维码成功：" + barNo);
                        /* barNo = "12345678";
                         lotNo = "明码-123";
                         ssNo = "明码2-123";*/


                        saveLog("下发指令至打标机...");
                        if (startMark(barNo, lotNo, ssNo))
                        {
                            //4、设备条码打印完成信息回写
                            sanhuaOpen.WorkOrderBarcodePrintFinish(tbxComId.Text, barNo);
                            saveLog("条码打印成功" + barNo);
                            WritePlcWord("DB29.DBW68.0", 2);
                        }
                        else
                        {
                            saveLog("条码打印失败" + barNo);
                        }
                        btnStartMark.Enabled = true;
                        isPrint = false;

                        saveLog("打标结束!");

                    }
                    catch (Exception ex)
                    {
                        saveLog("PLC连接失败！" + ex.Message);
                        saveLog(ex.Message);
                    }
                    _isProcessing = false;
                    return;
                }


                // 读取第6行：上位机-数据传输开始（DB29.DBW60.0，Word类型）
                ushort transferStart = ReadPlcWord("DB29.DBW60.0");
                //short transferStart = (short)_plc.Read("DB29.DBW60.0");

                // 当值为1时，开始读取1-5行的状态
                if (transferStart == 1 )
                {
                    _plc.Write("DB29.DBW60.0", (short)0);

                    _isProcessing = true;
                    // 读取1-5行数据并存储到全局变量
                    await ReadPlcStatusData();
                    // 读取完成后：设置第7行（数据接收完成）为1，通知PLC
                    _plc.Write("DB29.DBW62.0", (short)1);

                    saveLog("PLC状态读取完成，已通知PLC！");

                    string processNo = (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond).ToString("D10");

                    try
                    {
                        // ====== 3. 先插入解析表 SHProcessPropertyParse ======
                        bool parseInsertSuccess = await Task.Run(() =>
                            _sqlHelper.InsertProcessParseTable(processNo)
                        );
                        if (!parseInsertSuccess)
                        {
                            saveLog("解析表插入失败，终止数据入库！");
                            _isProcessing = false;
                            return;
                        }

                        // ====== 4. 再插入存储表 SHProcessProperty（关联同一processNo）======
                        bool propertyInsertSuccess = await Task.Run(() =>
                            _sqlHelper.InsertProcessProperty(
                                processNo: processNo,        // 关联唯一processNo
                                barNo: _scanQrCode,          // 二维码→bar_no
                                station2Status: _station2Status, // 工位2→data001
                                station3Status: _station3Status, // 工位3→data002
                                station4Status: _station4Status, // 工位4→data003
                                cameraStatus: _cameraJudgeStatus, // 相机状态→data004
                                qrCodeLevel: _qrCodeLevel,
                                vouNo: tbxVouNo.Text
                            )
                        );
                        // ====== 5. 结果反馈 + 通知PLC ======
                        if (propertyInsertSuccess)
                        {
                            saveLog($"两张表插入成功！processNo：{processNo}，条码：{_scanQrCode}");
                            // 设置第7行（数据接收完成）为1，通知PLC
                           // _plc.Write("DB29.DBW62.0", (short)1);
                            // MessageBox.Show($"数据入库成功！processNo：{processNo}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            saveLog("存储表插入失败！");
                            // MessageBox.Show("数据入库失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        string lastQrFile = GetLatestCreatedImageFileName(qrImgPath);
                        if(null==lastQrFile)
                        {
                            saveLog("未找到二维码图片：" + qrImgPath);
                        }
                        else
                        {
                            saveLog("读取最新的二维码图片：" + lastQrFile + "，重命名为：" + processNo);
                            RenameFileKeepExtension(Path.Combine(qrImgPath) + "\\" + lastQrFile, processNo);
                            processQrImage(_scanQrCode, processNo, processNo);
                        }


                        string lastCameraFile = GetLatestCreatedImageFileName(cameraImgPath);
                        if (null == lastCameraFile)
                        {
                            saveLog("未找到相机图片：" + cameraImgPath);
                        }
                        else
                        {
                            saveLog("读取最新的相机图片：" + lastCameraFile);
                            processCameraImage(_scanQrCode, processNo, processNo);
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"解析表插入失败：{ex.Message}", "SQL错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    

                   
                    _isProcessing = false;

                }
            }
            catch (Exception ex)
            {
                _isProcessing = false;
                saveLog($"PLC轮询异常:" +ex.Message);
            }
        }

        private async Task ReadPlcStatusData()
        {
            await Task.Run(() =>
            {
                // 1. 上传-扫描二维码：String[50]（DB29.DBX0.0）→ 用ReadBytes读取50字节（String[50]占50字节）
                // 参数说明：DataType.DataBlock(数据块)、DB号(29)、起始字节地址(0)、读取长度(50)
                byte[] qrCodeBytes = _plc.ReadBytes(DataType.DataBlock, 29, 0, 50);
                // 将字节数组转字符串（去除末尾空字符）
                _scanQrCode = System.Text.Encoding.ASCII.GetString(qrCodeBytes).TrimEnd('\0');
                saveLog("PLC数据二维码："+_scanQrCode);

                // 2. 上传-工位2状态：Char（DB29.DBB52 + DB29.DBB53，读2字节）
                byte[] station2Bytes = _plc.ReadBytes(DataType.DataBlock, 29, 52, 2);
                _station2Status = Encoding.ASCII.GetString(station2Bytes).Trim();
                saveLog("PLC工位2状态：" + _station2Status);

                // 3. 上传-工位3状态：Char（DB29.DBB54 + DB29.DBB55，读2字节）
                byte[] station3Bytes = _plc.ReadBytes(DataType.DataBlock, 29, 54, 2);
                _station3Status = Encoding.ASCII.GetString(station3Bytes).Trim();
                saveLog("PLC工位3状态：" + _station3Status);

                // 4. 上传-工位4状态：Char（DB29.DBB56 + DB29.DBB57，读2字节）
                byte[] station4Bytes = _plc.ReadBytes(DataType.DataBlock, 29, 56, 2);
                _station4Status = Encoding.ASCII.GetString(station4Bytes).Trim();
                saveLog("PLC工位4状态：" + _station4Status);

                // 5. 上传-相机判断状态：Word（DB29.DBW58）
                _cameraJudgeStatus = ReadPlcWord("DB29.DBW58.0");
                saveLog("PLC相机判断状态：" + _cameraJudgeStatus);

                char level = ReadPlcChar("DB29.DBB66");
                _qrCodeLevel = level.ToString();
                saveLog("PLC二维码等级：" + _qrCodeLevel);
            });
        }

        private CpuType GetCpuType(string cpuTypeStr)
        {
            switch (cpuTypeStr.Trim())
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusTime.Text = "当前时间：" + DateTime.Now.ToString();
            if(iLaserStatus == 1)
            {
                statusLaser.Text = "正常";
                statusLaser.ForeColor = System.Drawing.Color.Green;
            }
            else if(iLaserStatus == 2)
            {
                statusLaser.Text = "异常";
                statusLaser.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                statusLaser.Text = "连接中";
                statusLaser.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            tbxVouNo.Focus();

        }

        private void reConnectLaser()
        {
            CommLib.LM_ClosePort();
            tmInitLaser.Stop();
            SocketOptions o;
            o.portNumber = iLaserPort;

            string[] ips = sLaserIp.Split('.');
            UInt32 ip = uint.Parse(ips[0]);
            ip = ip << 8;
            ip += uint.Parse(ips[1]);
            ip = ip << 8;
            ip += uint.Parse(ips[2]);
            ip = ip << 8;
            ip += uint.Parse(ips[3]);

            o.ipAddress = ip;

            CommLib.LM_SetProtocolType(1);
            CommLib.LM_SetTimeout(2000);
            CommLib.LM_SetPortOptions(ref o);

            if (CommLib.LM_OpenPort(1))
            {
                iLaserStatus = 1;
                saveLog("设备连接成功！");
                //btnLoadFile_Click(sender, e);
            }
            else
            {
                iLaserStatus = 2;
                saveLog("设备连接失败！IP=" + sLaserIp);
            }
        }

        private void tmInitLaser_Tick(object sender, EventArgs e)
        {
            CommLib.LM_ClosePort();
            tmInitLaser.Stop();
            SocketOptions o;
            o.portNumber = iLaserPort;

            string[] ips = sLaserIp.Split('.');
            UInt32 ip = uint.Parse(ips[0]);
            ip = ip << 8;
            ip += uint.Parse(ips[1]);
            ip = ip << 8;
            ip += uint.Parse(ips[2]);
            ip = ip << 8;
            ip += uint.Parse(ips[3]);

            o.ipAddress = ip;

            CommLib.LM_SetProtocolType(1);
            CommLib.LM_SetTimeout(2000);
            CommLib.LM_SetPortOptions(ref o);

            if (CommLib.LM_OpenPort(1))
            {
                iLaserStatus = 1;
                saveLog("设备连接成功！");
                //btnLoadFile_Click(sender, e);
            }
            else
            {
                iLaserStatus = 2;
                saveLog("设备连接失败！IP=" + sLaserIp);
            }
        }

        private void btnStartLaser_Click(object sender, EventArgs e)
        {

            saveLog("接收到打标指令，流程开始...");
            if (tbxVouNo.Text.Length == 0)
            {
                MessageBox.Show("工单信息为空，请输入或扫码！");
                tbxVouNo.Focus();
                return;
            }
            if(tbxFileName.Text.Length==0)
            {
                MessageBox.Show("请选择打标文件");
                tbxFileName.Focus();
                return;
            }
            if (tbxSignName.Text.Length == 0)
            {
                MessageBox.Show("请选择条码标签");
                tbxSignName.Focus();
                return;
            }
            if (tbxSignTextName.Text.Length == 0)
            {
                MessageBox.Show("请选择明码标签");
                tbxSignTextName.Focus();
                return;
            }
            isPrint = true;
            btnStartMark.Enabled = false;
            try
            {
                //1、扫描工单创建条码
                //  sanhuaOpen.WorkOrderCreateBarcode(tbxComId.Text, tbxVouNo.Text, "{}");

                //2、扫描工单申请条码
                saveLog("开始向MES申请...");
                   string barListString = sanhuaOpen.WorkOrderApplyBarcode(tbxComId.Text, tbxVouNo.Text, 1);
//                string barListString = "[{\"bar_no\":\"TB99651 A 250618110013\",\"vou_no\":\"20250618089867\",\"create_time\":\"2025-06-18T14:19:46.9\",\"print_time\":\"9999-12-31T23:59:59.997\",\"content\":{\"bar_no\":\"TB99651 A 250618110013\",\"\\nlot_no\":\"250618110013\"},\"state\":\"0\"}]";
                //string barListString = "[{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0010\",\"lot_no\":\"20200805004204\",\"create_time\":\"2021-04-06T16:25:32.823\",\"print_time\":\"9999-12-31T23:59:59.997\",\"content\":{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0010\"},\"state\":\"0\"},{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0020\",\"vou_no\":\"20200805004204\",\"create_time\":\"2021-04-06T16:25:32.823\",\"print_time\":\"9999-12-31T23:59:59.997\",\"content\":{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0020\"},\"state\":\"0\"},{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0030\",\"vou_no\":\"20200805004204\",\"create_time\":\"2021-04-06T16:25:32.823\",\"print_time\":\"9999-12-31T23:59:59.997\",\"content\":{\"bar_no\":\"P1523002-00-B:1TBNJ21DDDFNUMFNO0030\"},\"state\":\"0\"}]";
                //：[{"bar_no":"V500015060#M10035#240124000102","vou_no":"20240124031462","create_time":"2024-01-24T20:11:16.623","print_time":"9999-12-31T23:59:59.997","content":{"bar_no":"V500015060#M10035#240124000102","lot_no":"240124000102"},"state":"0"}]

                if (String.IsNullOrEmpty(barListString))
                {
                    saveLog("条码申请失败");
                    btnStartMark.Enabled = true;
                    return;
                }
                barListString = barListString.Replace("\\n", "");

                saveLog("MES申请条码成功：" + barListString);

                JArray jarr = JArray.Parse(barListString);
                if(jarr.Count==0)
                {
                    saveLog("条码申请失败");
                    btnStartMark.Enabled = true;
                    return;
                }
                string barNo = jarr[0]["bar_no"].ToString();
                saveLog("barNo:" + barNo);


                // 获取 content 字段
                JObject firstObject = (JObject)jarr[0];
                JObject contentObject = (JObject)firstObject["content"];

                // 获取 lot_no 字段的值
                string lotNo = "";

                try
                {
                   lotNo = contentObject["lot_no"].ToString();
                   saveLog("lotNo:" + lotNo);
                }
                catch (Exception ex) {

                }



                // 获取 lot_no 字段的值
                string ssNo = "";
                try
                {
                    ssNo = contentObject["ss_no"].ToString();
                    saveLog("ssNo:" + ssNo);
                }
                catch(Exception ex) {
                    
                }


                //3、条码下发给设备时上传当前条码状态
                sanhuaOpen.WorkOrderBarcodeWaitPrintFinsh(tbxComId.Text, barNo);

                saveLog("下发指令至打标机...");
                if (startMark(barNo, lotNo, ssNo))
                {
                    //4、设备条码打印完成信息回写
                    sanhuaOpen.WorkOrderBarcodePrintFinish(tbxComId.Text, barNo);
                    saveLog("条码打印成功" + barNo);
                }
                else
                {
                    saveLog("条码打印失败" + barNo);
                }
                btnStartMark.Enabled = true;
                isPrint = false;

                saveLog("打标结束!");
            }
            catch (Exception ex)
            {
                saveLog("MES连接失败！URL=" + sanhuaOpen.Url);
                saveLog(ex.Message);
            }

        }

        private bool loadMarkFile(string fileName)
        {
            int l = fileName.Length;
            char[] bFileName = new char[l + 1];
            fileName.ToCharArray().CopyTo(bFileName, 0);
            if (CommLib.LM_LoadMarkFile(bFileName))
            {
                saveLog("调取文档成功" + tbxFileName.Text);
                return true;
            }
            else
            {
                saveLog("调取文档失败" + tbxFileName.Text);
                return false;
            }
        }

        private bool startMark(string barNo, string lotNo,string ssNo)
        {
            string sBarNo = barNo.Replace(">", "\\>");
            string sPrintLotNo = lotNo.Replace(">", "\\>");
            string sPrintSSNo = ssNo.Replace(">", "\\>");

            try
            {
                // reConnectLaser();
                // loadMarkFile(tbxFileName.Text.Replace(".lmf3", ""));

                if (tbxSignName.Text.Length > 0)
                {

                    int nl = tbxSignName.Text.Length;
                    char[] nb = new char[nl + 1];
                    tbxSignName.Text.ToCharArray().CopyTo(nb, 0);

                    int tl = sBarNo.Length;
                    char[] tb = new char[tl + 1];
                    sBarNo.ToCharArray().CopyTo(tb, 0);
                    if (CommLib.LM_ChangeTextByName(nb, tb))
                    {
                        saveLog("修改条码标签内容成功。");
                    }
                    else
                    {
                        saveLog("修改条码标签内容失败。");
                    }
                }

                if (tbxSignTextName.Text.Length > 0)
                {
                    int nl1 = tbxSignTextName.Text.Length;
                    char[] nb1 = new char[nl1 + 1];
                    tbxSignTextName.Text.ToCharArray().CopyTo(nb1, 0);

                    int tl1 = sPrintLotNo.Length;
                    char[] tb1 = new char[tl1 + 1];
                    sPrintLotNo.ToCharArray().CopyTo(tb1, 0);
                    if (CommLib.LM_ChangeTextByName(nb1, tb1))
                    {
                        saveLog("修改明码标签内容成功。");
                    }
                    else
                    {
                        saveLog("修改明码标签内容失败。");
                    }
                }

                if(tbxSignTextName2.Text.Length>0 && ssNo.Length>0)
                {
                    int nl2 = tbxSignTextName2.Text.Length;
                    char[] nb2 = new char[nl2 + 1];
                    tbxSignTextName2.Text.ToCharArray().CopyTo(nb2, 0);

                    int tl2 = sPrintSSNo.Length;
                    char[] tb2 = new char[tl2 + 1];
                    sPrintSSNo.ToCharArray().CopyTo(tb2, 0);
                    if (CommLib.LM_ChangeTextByName(nb2, tb2))
                    {
                        saveLog("修改明码标签2内容成功。");
                    }
                    else
                    {
                        saveLog("修改明码标签2内容失败。");
                    }
                }

                //开始打标
                return CommLib.LM_StartMarkAndWaitFinish(timeout);
            }
            catch (Exception ex)
            {
                saveLog("打标异常："+ex.Message);
                return false;
            }
        }

        private void saveLog(string info)
        {
            tbxLog.AppendText(DateTime.Now.ToString() + " " + info + "\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
           if(CommLib.LM_StopMark())
            {
                saveLog("停止打标成功！");
            }
           else
            {
                saveLog("停止打标失败！");
            }
            btnStartMark.Enabled = true;
            isPrint = false;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CommLib.LM_StartInfraredPreview())
            {
                saveLog("开始红光预览成功。");
            }
            else
            {
                saveLog("开始红光预览失败。");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (CommLib.LM_StopInfraredPreview())
            {
                saveLog("停止红光预览成功。");
            }
            else
            {
                saveLog("停止红光预览失败。");
            }
        }

        private void btnFileList_Click(object sender, EventArgs e)
        {
            FrmFile frmFile = new FrmFile();
            frmFile.ShowDialog();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            //reConnectLaser();
           
            CommLib.LM_ClosePort();
            Thread.Sleep(500);
            loadFile();
            Thread.Sleep(500);
            reConnectLaser();
            //tmReconnect.Start();
        }

        private void loadFile()
        {
            string response = sendTcpMessage("<L>");
            saveLog(response);

            tbxFileName.Items.Clear();
            tbxSignNamebak.Items.Clear();

            // 移除字符串中的尖括号和逗号
            response = response.Replace("<L,", "").Replace(">", "");
            // 使用逗号分割字符串
            string[] parts = response.Split(',');
            if(parts.Length>0)
            {
                // 添加到 ComboBox 中
                foreach (string part in parts)
                {
                    // 移除可能的空格并添加到 ComboBox 的项中
                    tbxFileName.Items.Add(part.Trim());
                }
            }
        }

        //字符流协议
        private string sendTcpMessage(string message) {
            TcpClient tcpClient = new TcpClient();
            tcpClient.ReceiveTimeout = 3000;
            tcpClient.Connect(sLaserIp, iLaserPort);
            if (!tcpClient.Connected)
            {
                MessageBox.Show("tcp fail!");
            }

            // 获取网络流
            NetworkStream clientStream = tcpClient.GetStream();
            UnicodeEncoding bigEndianUnicode = new UnicodeEncoding(true, false);

            // 创建StreamReader用于读取客户端发来的数据
            StreamReader reader = new StreamReader(clientStream, bigEndianUnicode);

            // 创建StreamWriter用于向客户端发送数据
            StreamWriter writer = new StreamWriter(clientStream, bigEndianUnicode);

            // 向服务器发送数据
            saveLog("send:" + message);
            writer.Write(message);
            writer.Flush(); // 确保数据被写入网络流

            string response = ReadData(reader);

            tcpClient.Close();
            return response;
        }


        // 读取数据的方法
        private string ReadData(StreamReader reader)
        {
            StringBuilder sb = new StringBuilder();
            char[] buffer = new char[4096]; // 适当调整缓冲区大小

            int bytesRead;
            do
            {
                bytesRead = reader.Read(buffer, 0, buffer.Length);
                sb.Append(buffer, 0, bytesRead);

                // 检查数据末尾是否包含 '>'，如果是，则停止读取
            } while (bytesRead > 0 && sb.ToString().IndexOf('>') == -1);
            return sb.ToString();
        }

        private void tbxFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMarkFile(tbxFileName.Text.Replace(".lmf3", ""));

            // reConnectLaser();
            // loadMarkFile(tbxFileName.Text.Replace(".lmf3",""));
            /*            sendTcpMessage("<L" + tbxFileName.Text.Replace(".lmf3", "") + ">");
                        Thread.Sleep(100);
                        string response = sendTcpMessage("<D>");
                        saveLog("response:"+ response);

                        reConnectLaser();

                        tbxSignNamebak.Items.Clear();
                        tbxSignTextNameBak.Items.Clear();


                        // 移除字符串中的尖括号和逗号
                        response = response.Replace("<D,", "").Replace(">", "");
                        // 使用逗号分割字符串
                        string[] parts = response.Split(',');
                        if (parts.Length > 0)
                        {
                            // 添加到 ComboBox 中
                            foreach (string part in parts)
                            {
            *//*                    // 移除可能的空格并添加到 ComboBox 的项中
                                byte[] asciiBytes = Encoding.ASCII.GetBytes(part);

                                // 将ASCII码值转换为UTF-8编码的字节数组（调换高低位）
                                byte[] utf8Bytes = new byte[asciiBytes.Length];

                                for (int i = 0; i < asciiBytes.Length; i += 2)
                                {
                                    if(i+1>=asciiBytes.Length)
                                    {
                                        break;
                                    }
                                    utf8Bytes[i] = asciiBytes[i + 1]; // 低位
                                    utf8Bytes[i + 1] = asciiBytes[i]; // 高位
                                }

                                // 使用UTF-16编码解码字节数组为字符串
                                string resultString = Encoding.Unicode.GetString(utf8Bytes);*//*

                                tbxSignNamebak.Items.Add(part.Trim());
                                tbxSignTextNameBak.Items.Add(part.Trim());
                            }
                        }*/
        }

        private void tbxVouNo_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了回车键
            if (e.KeyCode == Keys.Enter)
            {
                // 将 TextBox 中的文本设为选中状态
                tbxVouNo.SelectAll();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 处理快捷键，例如 Ctrl + Alt + F
            if (keyData == hotkey)
            {
                if (!isPrint)
                {
                    btnStartLaser_Click(this, EventArgs.Empty);
                }
                return true;
            }

            // 调用基类的 ProcessCmdKey 方法继续处理其他键盘输入
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
         //   CommLib.LM_ClosePort();

            // 停止轮询定时器
            if (_pollTimer != null)
            {
                _pollTimer.Stop();
                _pollTimer.Dispose();
            }

            // 断开PLC连接
            if (_plc != null && _plc.IsConnected)
            {
                _plc.Close();
               // _plc.Dispose();
            }
        }

        private void tmReconnect_Tick(object sender, EventArgs e)
        {
            tmReconnect.Stop();
            reConnectLaser();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FrmPlcCommunication plcForm = new FrmPlcCommunication();
            plcForm.ShowDialog();//非模态打开，可同时操作其他页面
        }




        // 需先引用：using System; using System.Text; using S7.Net;

        // 1. 读取PLC Word类型（16位无符号）
        public ushort ReadPlcWord(string variableAddress)
        {
            object result = _plc.Read(variableAddress);
            if (result == null)
            {
                saveLog($"读取Word失败：地址[{variableAddress}]返回空");
            }
            return result is ushort ? (ushort)result : Convert.ToUInt16(result);
        }

        // 2. 读取PLC Int16类型（16位有符号）
        public short ReadPlcInt16(string variableAddress)
        {
            object result = _plc.Read(variableAddress);
            if (result == null)
            {
                saveLog($"读取Int16失败：地址[{variableAddress}]返回空");
            }
            return result is short ? (short)result : Convert.ToInt16(result);
        }

        // 3. 读取PLC Int32类型（32位有符号）
        public int ReadPlcInt32(string variableAddress)
        {
            object result = _plc.Read(variableAddress);
            if (result == null)
            {
                saveLog($"读取Int32失败：地址[{variableAddress}]返回空");
            }
            return result is int ? (int)result : Convert.ToInt32(result);
        }

        // 4. 读取PLC Char类型（8位ASCII字符）
        public char ReadPlcChar(string variableAddress)
        {
            object result = _plc.Read(variableAddress);
            if (result == null)
            {
                saveLog($"读取Char失败：地址[{variableAddress}]返回空");
            }
            byte byteValue = result is byte ? (byte)result : Convert.ToByte(result);
            return Encoding.ASCII.GetChars(new[] { byteValue })[0];
        }

        // 5. 读取PLC String类型（ASCII字符串）
        public string ReadPlcString(string startAddress, int strLength)
        {
            if (strLength <= 0)
                throw new ArgumentException("字符串长度必须大于0", nameof(strLength));

            // 解析地址（适配DB1.DBB100/MB10/IB10/QB10格式）
            DataType dataType = DataType.DataBlock;
            int dbNumber = 0;
            int startByteAdr = 0;

            if (startAddress.StartsWith("DB") && startAddress.Contains(".DBB"))
            {
                string[] parts = startAddress.Split('.');
                dbNumber = int.Parse(parts[0].Replace("DB", ""));
                startByteAdr = int.Parse(parts[1].Replace("DBB", ""));
                dataType = DataType.DataBlock;
            }
            else if (startAddress.StartsWith("MB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("MB", ""));
                dataType = DataType.Memory;
            }
            else if (startAddress.StartsWith("IB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("IB", ""));
                dataType = DataType.Input;
            }
            else if (startAddress.StartsWith("QB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("QB", ""));
                dataType = DataType.Output;
            }
            else
            {
                throw new ArgumentException($"地址格式错误：{startAddress}，请使用DB1.DBB100/MB10等格式", nameof(startAddress));
            }

            // 读取字节数组并转字符串
            byte[] bytes = _plc.ReadBytes(dataType, dbNumber, startByteAdr, strLength);
            if (bytes.Length != strLength)
                throw new Exception($"读取字符串字节数异常：预期{strLength}，实际{bytes.Length}，地址[{startAddress}]");

            return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }


        // 需先引用：using System; using System.Text; using S7.Net;

        // 1. 写入PLC Word类型（16位无符号）
        public void WritePlcWord(string variableAddress, ushort value)
        {
            _plc.Write(variableAddress, value);
        }

        // 2. 写入PLC Int16类型（16位有符号）
        public void WritePlcInt16(string variableAddress, short value)
        {
            _plc.Write(variableAddress, value);
        }


        private async void processQrImage(string barNo,string processNo,string fileName)
        {

            // 5. 写入SHProcessFile表（参数严格匹配表字段）
            bool writeSuccess = await Task.Run(() =>
                _sqlHelper.InsertProcessFileInfo(
                    bar_no: barNo,                // PLC扫描的主条码
                    process_no: processNo,             // 10位唯一工序号
                    file_type: "Img",                // 文件类型：Img（照片）
                    name: fileName,                  // 重命名后的文件名称
                    do_time: DateTime.Now,// 文件产生时间（文件创建时间）
                    ok_flag: "OK",                      // 文件判定结果（默认OK）
                    ng_msg: "",                         // 不良原因（无则空）
                    path: Path.Combine(ftpPath+"/qr/"+ fileName) ,                  // 三花内网FTP完整路径
                    flag: 0                            // 调度标记：0未调度
                   // sync_time: null,                    // 调度时间：未调度则为空
                   // sync_msg: ""                        // 调度异常信息：无则空
                )
            );
        }

        private async void processCameraImage(string barNo, string processNo, string fileName)
        {

            // 5. 写入SHProcessFile表（参数严格匹配表字段）
            bool writeSuccess = await Task.Run(() =>
                _sqlHelper.InsertProcessFileInfo(
                    bar_no: _scanQrCode,                // PLC扫描的主条码
                    process_no: processNo,             // 10位唯一工序号
                    file_type: "Img",                // 文件类型：Img（照片）
                    name: fileName,                  // 重命名后的文件名称
                    do_time: DateTime.Now,// 文件产生时间（文件创建时间）
                    ok_flag: "OK",                      // 文件判定结果（默认OK）
                    ng_msg: "",                         // 不良原因（无则空）
                    path: Path.Combine(ftpPath+ "/camera/"+fileName),                  // 三花内网FTP完整路径
                    flag: 0                            // 调度标记：0未调度
                   // sync_time: null,                    // 调度时间：未调度则为空
                   // sync_msg: ""                        // 调度异常信息：无则空
                )
            );
        }

        /// <summary>
        /// 读取指定目录中创建时间最新的图片文件名
        /// </summary>
        /// <param name="directoryPath">目录路径（绝对路径/相对路径均可）</param>
        /// <returns>创建时间最新的图片文件名（无图片时返回null）</returns>
        public static string GetLatestCreatedImageFileName(string directoryPath)
        {
            try
            {
                // 1. 验证目录是否存在
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"错误：目录 {directoryPath} 不存在！");
                    return null;
                }

                // 2. 定义需要筛选的图片扩展名（可根据需求扩展）
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

                // 3. 读取目录下所有文件，筛选出图片文件，并获取文件信息
                var imageFiles = Directory.GetFiles(directoryPath)
                    .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower())) // 过滤图片格式
                    .Select(file => new FileInfo(file)) // 转为FileInfo，方便获取创建时间
                    .ToList();

                // 4. 处理无图片文件的情况
                if (imageFiles.Count == 0)
                {
                    Console.WriteLine($"提示：目录 {directoryPath} 下未找到图片文件！");
                    return null;
                }

                // 5. 找到创建时间最新的图片文件
                var latestFile = imageFiles
                    .OrderByDescending(file => file.CreationTime) // 按创建时间降序排序
                    .First(); // 取第一个（最新的）

                // 6. 返回文件名（含扩展名，如 "test.png"；若需全路径则返回 latestFile.FullName）
                return latestFile.Name;
            }
            catch (Exception ex)
            {
                // 捕获权限不足、路径非法等异常
                Console.WriteLine($"读取图片文件失败：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 通用文件重命名函数（保留原文件扩展名）
        /// </summary>
        /// <param name="originalFilePath">原文件的完整路径（如：D:\Images\old_name.png）</param>
        /// <param name="newFileNameWithoutExt">新文件名（不含扩展名，如：new_name）</param>
        /// <returns>重命名后的文件完整路径（失败返回null）</returns>
        public static string RenameFileKeepExtension(string originalFilePath, string newFileNameWithoutExt)
        {
            try
            {
                // 1. 基础校验：参数不能为空
                if (string.IsNullOrWhiteSpace(originalFilePath))
                {
                    Console.WriteLine("错误：原文件路径不能为空！");
                    return null;
                }
                if (string.IsNullOrWhiteSpace(newFileNameWithoutExt))
                {
                    Console.WriteLine("错误：新文件名（不含扩展名）不能为空！");
                    return null;
                }

                // 2. 校验原文件是否存在
                if (!File.Exists(originalFilePath))
                {
                    Console.WriteLine($"错误：原文件不存在 → {originalFilePath}");
                    return null;
                }

                // 3. 提取原文件的核心信息：目录、扩展名
                string fileDirectory = Path.GetDirectoryName(originalFilePath); // 原文件所在目录
                string originalExt = Path.GetExtension(originalFilePath);       // 提取原扩展名（如 .png、.jpg）

                // 4. 拼接新文件的完整路径（目录 + 新名称 + 原扩展名）
                string newFilePath = Path.Combine(fileDirectory, $"{newFileNameWithoutExt}{originalExt}");

                // 5. 防覆盖处理：若新文件名已存在，自动加后缀（如 new_name_1.png）
                int suffix = 1;
                string tempNewFilePath = newFilePath;
                while (File.Exists(tempNewFilePath))
                {
                    tempNewFilePath = Path.Combine(fileDirectory, $"{newFileNameWithoutExt}_{suffix}{originalExt}");
                    suffix++;
                }
                newFilePath = tempNewFilePath;

                // 6. 执行重命名（核心操作）
                File.Move(originalFilePath, newFilePath);
                Console.WriteLine($"重命名成功：\n原文件：{Path.GetFileName(originalFilePath)}\n新文件：{Path.GetFileName(newFilePath)}");

                // 7. 返回重命名后的完整路径
                return newFilePath;
            }
            catch (IOException ex)
            {
                // 处理文件被占用、权限不足等IO异常
                Console.WriteLine($"重命名失败：文件被占用/权限不足 → {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // 捕获其他未知异常
                Console.WriteLine($"重命名失败：{ex.Message}");
                return null;
            }
        }


        // 3. 写入PLC String类型（ASCII字符串）
        public void WritePlcString(string startAddress, string value, int strLength)
        {
            if (strLength <= 0)
                throw new ArgumentException("字符串长度必须大于0", nameof(strLength));

            // 解析地址（适配DB1.DBB100/MB10/IB10/QB10格式）
            DataType dataType = DataType.DataBlock;
            int dbNumber = 0;
            int startByteAdr = 0;

            if (startAddress.StartsWith("DB") && startAddress.Contains(".DBB"))
            {
                string[] parts = startAddress.Split('.');
                dbNumber = int.Parse(parts[0].Replace("DB", ""));
                startByteAdr = int.Parse(parts[1].Replace("DBB", ""));
                dataType = DataType.DataBlock;
            }
            else if (startAddress.StartsWith("MB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("MB", ""));
                dataType = DataType.Memory; // M区对应DataType.Memory
            }
            else if (startAddress.StartsWith("IB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("IB", ""));
                dataType = DataType.Input;
            }
            else if (startAddress.StartsWith("QB"))
            {
                startByteAdr = int.Parse(startAddress.Replace("QB", ""));
                dataType = DataType.Output;
            }
            else
            {
                throw new ArgumentException($"地址格式错误：{startAddress}，请使用DB1.DBB100/MB10等格式", nameof(startAddress));
            }

            // 将字符串转为ASCII字节数组，长度不足补0，超长截断
            byte[] bytes = new byte[strLength];
            byte[] valueBytes = Encoding.ASCII.GetBytes(value);
            int copyLength = Math.Min(valueBytes.Length, strLength);
            Array.Copy(valueBytes, 0, bytes, 0, copyLength);

            // 写入字节数组
            _plc.WriteBytes(dataType, dbNumber, startByteAdr, bytes);
        }
    }
}
