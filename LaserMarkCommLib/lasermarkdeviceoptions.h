#ifndef LASERMARKDEVICEOPTIONS_H
#define LASERMARKDEVICEOPTIONS_H

#include <tchar.h>

/*************************************** 串口参数 ***************************************/

// 波特率
enum BaudRate {
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
enum DataBits {
	Data5 = 5,
	Data6 = 6,
	Data7 = 7,
	Data8 = 8
};

// 校验位
enum Parity {
	NoParity = 0,		//无校验
	OddParity = 1,		//奇校验
	EvenParity = 2,		//偶校验
	MarkParity = 3,		//固定校验
	SpaceParity = 4     //空检验
};

// 停止位
enum StopBits {
	OneStop = 0,			//1
	OneAndHalfStop = 1,		//1.5
	TwoStop = 2				//2
};

// 串口参数设置结构体
struct SerialPortOptions {
	wchar_t portName[32] = { 0x0 };      //串口名
	BaudRate baudRate = Baud115200;    //波特率
	DataBits dataBits = Data8;         //数据位
	Parity parity = NoParity;          //校验位
	StopBits stopBits = OneStop;       //停止位
};



/*************************************** 网口参数 ***************************************/

// 网口参数结构体
struct SocketOptions {
	unsigned long ipAddress = 0;		//ip地址，4字节32位，每个字节表示ip地址中的一段，例如0xC0A80164即表示地址192.168.1.100
	int portNumber = 2000;				//端口号
};


/*************************************** 错误信息 ***************************************/

enum ErrorType
{
	NoError = 0,				// 没有错误
	OpenFailedError,			// 打开端口失败错误，由于串口名错误或ip地址和端口号等参数设置不对时出现
	DidNotOpenError,            // 没有打开端口，在未成功打开端口的情况下调用控制函数的情况下出现
	TimeoutError,               // 超时错误，与激光机通讯时等待回应超时时出现
	InvalidReceivedError,       // 无效的回应数据，与激光机通讯时收到的数据无效，现场通讯质量不良时可能出现
	MarkRepeatedCodeError,      // 重复打标错误，表示打标发生重码
	InvalidCommandError,        // 无效指令错误，表示接收到无效指令
	InvalidParameter,			// 无效参数错误，调用时提供的参数错误
	UnknowError = 0xff			// 未知错误
};


/**************************************** 参数 ****************************************/

// 偏移和旋转参数
struct OffsetData {
	double horOffset = 0.0;  //水平偏移
	double verOffset = 0.0;  //竖直偏移
	double rotate = 0.0;  //旋转角度
};

#endif // LASERMARKDEVICEOPTIONS_H

