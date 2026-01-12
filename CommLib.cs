using System;
using System.Runtime.InteropServices;


namespace beipin
{
    class CommLib
    {
        /************************************************ 通讯控制 ***************************************************/

        // 设置使用协议类型，必须在打开端口前设置协议类型
        // 更改协议类型时，需要先关闭端口，然后设置新的协议类型，然后再重新打开端口
        // 0：Ascii协议
        // 1：Unicode协议
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetProtocolType@@YAXH@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_SetProtocolType(int type);


        // 设置串口端口参数
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetPortOptions@@YAXABUSerialPortOptions@@@Z", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_SetPortOptions(ref SerialPortOptions options);


        // 设置网口端口参数
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetPortOptions@@YAXABUSocketOptions@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_SetPortOptions(ref SocketOptions options);


        // 打开端口
        // 0：使用串口
        // 1：使用网口
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_OpenPort@@YA_NH@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_OpenPort(int type);


        // 关闭端口
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_ClosePort@@YAXXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_ClosePort();


        // 设置超时时间，单位毫秒
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetTimeout@@YAXH@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_SetTimeout(int msecs);




        /************************************************ 激光系统控制 ***************************************************/

        // 判断激光系统是否启动完成
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_IsMachineStartupFinished@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_IsMachineStartupFinished();


        // 打开文档
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_LoadMarkFile@@YA_NPA_W@Z", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_LoadMarkFile(char[] szFile);


        // 更改标记内容
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_ChangeTextByName@@YA_NPA_W0@Z", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_ChangeTextByName(char[] szName, char[] szTextValue);


        // 查询打标状态
        // -1：错误
        // 0：未工作状态
        // 1：正在打标中
        // 2：正在红光预览
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_GetMarkStatus@@YAHXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LM_GetMarkStatus();


        // 开始打标
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_StartMark@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_StartMark();


        // 停止打标
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_StopMark@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_StopMark();


        // 开始打标并等待打标完成，waitMsecs为等待达标完成超时时间，建议在非UI线程调用此方法，以免造成UI卡顿
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_StartMarkAndWaitFinish@@YA_NH@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_StartMarkAndWaitFinish(int waitMsecs);


        // 获取当前打标完成个数，如果返回-1则表示错误
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_GetMarkCount@@YA_JXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern long LM_GetMarkCount();


        // 开始红光预览
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_StartInfraredPreview@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_StartInfraredPreview();


        // 停止红光预览
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_StopInfraredPreview@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_StopInfraredPreview();

        
        // 查询板卡ID，板卡ID赋值到参数szId数组，参数szId数组大小必须大于16
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_BoardID@@YA_NPA_W@Z", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_BoardID(char[] szId);


        // 偏移旋转打标
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_OffsetDocMark@@YA_NHPAUOffsetData@@@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_OffsetDocMark(int size, OffsetData[] options);


        // 设置第一个笔号的打标参数
        //   参数speed  打标速度，单位mm/s
        //   参数power  打标功率，单位%
        //   参数frequency  打标频率，单位kHz
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetPenParams@@YA_NHHN@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_SetPenParams(int speed, int power, double frequency);


        // 设置第一个笔号的脉冲宽度，单位us
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetPenPulseWidth@@YA_NN@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_SetPenPulseWidth(double waitMsecs);


        // 设置IO输出
        //   参数port  输出端口，1 - 15，代表板卡 OUT1 - OUT15
        //   参数level  电平信号，0代表低电平，1代表高电平
        //   参数duration  电平持续时间，单位ms
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_SetIOOut@@YA_NHHH@Z", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LM_SetIOOut(int port, int level, int duration);


        // 获取错误信息
        [DllImport("LaserMarkCommLib.dll", EntryPoint = "?LM_GetLastError@@YAXPAHPA_WH@Z", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void LM_GetLastError(ref int error, char[] szMsg, int nSize);


    }


    // 波特率
    enum BaudRate
    {
        Baud1200 = 1200,
        Baud2400 = 2400,
        Baud4800 = 4800,
        Baud9600 = 9600,
        Baud19200 = 19200,
        Baud38400 = 38400,
        Baud57600 = 57600,
        Baud115200 = 115200
    };

    // 数据位
    enum DataBits
    {
        Data5 = 5,
        Data6 = 6,
        Data7 = 7,
        Data8 = 8
    };

    // 校验位
    enum Parity
    {
        NoParity = 0,       //无校验
        OddParity = 1,      //奇校验
        EvenParity = 2,     //偶校验
        MarkParity = 3,     //固定校验
        SpaceParity = 4     //空检验
    };

    // 停止位
    enum StopBits
    {
        OneStop = 0,            //1
        OneAndHalfStop = 1,     //1.5
        TwoStop = 2             //2
    };

    // 串口参数设置结构体
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SerialPortOptions
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U2)]
        public char[] portName;      //串口名
        public BaudRate baudRate;    //波特率
        public DataBits dataBits;         //数据位
        public Parity parity;          //校验位
        public StopBits stopBits;       //停止位
    };



    /*************************************** 网口参数 ***************************************/

    // 网口参数结构体
    struct SocketOptions
    {
        public UInt32 ipAddress;        //ip地址，4字节32位，每个字节表示ip地址中的一段，例如0xC0A80164即表示地址192.168.1.100
        public int portNumber;              //端口号
    };


    /*************************************** 错误信息 ***************************************/

    enum ErrorType
    {
        NoError = 0,                // 没有错误
        OpenFailedError,            // 打开端口失败错误，由于串口名错误或ip地址和端口号等参数设置不对时出现
        DidNotOpenError,            // 没有打开端口，在未成功打开端口的情况下调用控制函数的情况下出现
        TimeoutError,               // 超时错误，与激光机通讯时等待回应超时时出现
        InvalidReceivedError,       // 无效的回应数据，与激光机通讯时收到的数据无效，现场通讯质量不良时可能出现
        MarkRepeatedCodeError,      // 重复打标错误，表示打标发生重码
        InvalidCommandError,        // 无效指令错误，表示接收到无效指令
        InvalidParameter,			// 无效参数错误，调用时提供的参数错误
        UnknowError = 0xff          // 未知错误
    };


    /**************************************** 参数 ****************************************/

    // 偏移和旋转参数
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct OffsetData
    {
        public double horOffset;  //水平偏移
        public double verOffset;  //竖直偏移
        public double rotate;  //旋转角度
    };

}
