using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S7;

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

        public FrmMain()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            iLaserStatus = 0;
            tmInitLaser.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            iniFile = new IniFile(Application.StartupPath + @"\config.ini");

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
            if (!CommLib.LM_OpenPort(1)) 
            {
                iLaserStatus = 2;
                saveLog("设备连接失败！IP=" + sLaserIp);
            }
            else
            {
                saveLog("设备重连成功！");
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
            CommLib.LM_ClosePort();
            loadFile();
            // reConnectLaser();
            tmReconnect.Start();
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
            CommLib.LM_ClosePort();
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
    }
}
