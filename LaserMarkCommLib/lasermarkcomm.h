#ifndef LASERMARKCOMM_H
#define LASERMARKCOMM_H

#include "lasermarkdeviceoptions.h"
#include <Windows.h>

#define EXPORT __declspec(dllexport)

/************************************************ 通讯控制 ***************************************************/

// 设置使用协议类型，必须在打开端口前设置协议类型
// 更改协议类型时，需要先关闭端口，然后设置新的协议类型，然后再重新打开端口
// 0：Ascii协议
// 1：Unicode协议
EXPORT void LM_SetProtocolType(int type);

// 设置串口端口参数
EXPORT void LM_SetPortOptions(const SerialPortOptions &options);

// 设置网口端口参数
EXPORT void LM_SetPortOptions(const SocketOptions &options);

// 打开端口
// 0：使用串口
// 1：使用网口
EXPORT bool LM_OpenPort(int type);

// 关闭端口
EXPORT void LM_ClosePort();

// 设置超时时间，单位毫秒
EXPORT void LM_SetTimeout(int msecs);



/************************************************ 激光系统控制 ***************************************************/

// 判断激光系统是否启动完成
EXPORT bool LM_IsMachineStartupFinished();

// 打开文档
EXPORT bool LM_LoadMarkFile(TCHAR *szFile);

// 更改标记内容
EXPORT bool LM_ChangeTextByName(TCHAR *szName, TCHAR *szTextValue);

// 查询打标状态
// -1：错误
// 0：未工作状态
// 1：正在打标中
// 2：正在红光预览
EXPORT int LM_GetMarkStatus();

// 开始和停止打标
EXPORT bool LM_StartMark();
EXPORT bool LM_StopMark();

// 开始打标并等待打标完成，waitMsecs为等待达标完成超时时间，建议在非UI线程调用此方法，以免造成UI卡顿
EXPORT bool LM_StartMarkAndWaitFinish(int waitMsecs);

// 获取当前打标完成个数，如果返回-1则表示错误
EXPORT long long LM_GetMarkCount();

// 开始和停止红光预览
EXPORT bool LM_StartInfraredPreview();
EXPORT bool LM_StopInfraredPreview();

// 获取板卡ID
EXPORT bool LM_BoardID(TCHAR *szId);

// 偏移旋转打标
EXPORT bool LM_OffsetDocMark(int size, OffsetData* pData);

// 设置第一个笔号的打标参数
//   参数speed  打标速度，单位mm/s
//   参数power  打标功率，单位%
//   参数frequency  打标频率，单位kHz
EXPORT bool LM_SetPenParams(int speed, int power, double frequency);

// 设置第一个笔号的脉冲宽度，单位us
EXPORT bool LM_SetPenPulseWidth(double usec);

// 设置IO输出
//   参数port  输出端口，1 - 15，代表板卡 OUT1 - OUT15
//   参数level  电平信号，0代表低电平，1代表高电平
//   参数duration  电平持续时间，单位ms
EXPORT bool LM_SetIOOut(int port, int level, int duration);

// 获取错误信息
EXPORT void LM_GetLastError(int *pError, TCHAR *szMsg, int nSize);

#endif // !LASERMARKCOMM_H