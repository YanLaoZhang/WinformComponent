using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace KP184Lib
{
    /*
     通讯功能详解：
    1. 串口参数设定
    使用串口对负载进行操作前， 应先确保波特率、校验位、数据位、停止位设置正确。
    负载通讯端口的波特率可选， 详情参考菜单设定详解， 
    校验位、数据位和停止位固定为N81(无校验位、8位数据位、1位停止位)。
    2. 通讯地址设定
    上位机发送的指令中， 包含有一字节的地址码， 用以区分挂接在通讯总线上的多台设备， 
    只有负载设置的地址与指令中地址码相同的负载会响应指令， 如何设置参考菜单设定详解， 同一总线上的多台负载地址不可重复， 设置范围为1-99。
    3.端口连接
    KP184负载有232 和485 两个通讯接口， 端口定义见机身标识。
    4.通讯协议
    KP184负载采用 MODBUS-RTU 协议。一条指令包括:
    设备地址 功能码 数据 校验码
    设备地址为指令要控制的负载上设置的通讯地址。当指令中地址码为 0时， 表示此条指令为广播指令， 即所有在总线上的负载都会响应。
    功能码表示此条指令要执行的操作的类型， KP184负载目前只开放以下三种功能码：
    0x03: 读取寄存器。
    0x06： 写入单个寄存器。
    0x10： 写入多个连续寄存器。
    校验码是根据指令码计算得出的， 附加在指令后发送， 用于校验数据传输是否准确。KP184负载采用循环冗余校验方式(CRC)，RTU标准， 其生成方式如下：
    a. 设置一个16位的CRC结果寄存器, 并赋初值0xFFFF。
    b. 将指令串的第一个字节， 即地址码， 与CRC寄存器的低8位按位异或， 结果保存在CRC寄存器中。
    c. 将CRC寄存器右移一位， 并检测移出的位， 如果为1， 则将CRC寄存器与固定值0xA001异或。
    d. 重复c步骤8次。
    e. 对指令串的所有字节， 重复b、c、d步骤。
    f. 最后CRC寄存器中的值， 即为最终计算结果， 发送指令时，将其高位在前， 低位在后， 附在指令之后一起发送。
    生成CRC 校验码的C语言函数：
     unsigned short Get _CRC16RTU(volatile unsigned char * ptr, unsigned char len)
    {unsigned char i;
     unsigned short crc = 0xFFFF;
     if(len==0) len = 1;
     while(len--)通讯协议 KUNKUN
    {crc ^= * ptr;
     for(i=0; i<8; i++)
    {if(crc&1)
    {crc >>= 1;
     crc ^= 0xA001;}
     else crc >>= 1;}
     ptr++;
    }
     return(crc);
    }
    常用寄存器列表：
    寄存器     地址   字节数 读写属性 取值范围    说明
    LOAD ONOFF 0x010E 4      R/W      0,1         加载开关 1为加载ON, 0为加载 OFF
    LOAD MODE  0x0110 4      R/W      0-3         加载模式 0-CV,1-CC,2-CR,3-CW,加载为ON时不能切换加载模式
    CV SETTING 0x0112 4      R/W      0-150000    电压加载值， 单位mV
    CC SETTING 0x0116 4      R/W      0-30000     加载加载值, 单位mA
    CR SETTING 0x011A 4      R/W      0-80000     电阻加载值, 单位欧姆
    CW SETTING 0x011E 4      R/W      0-2500      功率加载值, 单位0.1W
    U MEASURE  0x0122 4      R        0-150000    电压测量值, 单位mV
    I MEASURE  0x0126 4      R        0-30000     电流测量值, 单位mA
    指令举例：
    以下举例均假定负载的通讯地址为01， 数据均为16进制格式。
    1. 设置拉载电流：
    发送指令: 01 06 01 16 00 01 04 00 00 07 D0 0C 9D其中各字节代表的意义为：
    01 ：设备地址。
    06 ：写单个寄存器命令号。
    01 16 ： 写入目标寄存器(拉载电流值寄存器)的地址。
    00 01 ： 写入目标寄存器的个数。通讯协议 KUNKIN
    04 ： 要写入目标寄存器的数据的字节数， 目标寄存器为 4 字节寄存器， 所以为4。
    00 00 07 D0 : 数据, 这里举例为07D0, 即将电流设为2000mA。
    OC 9D :校验码, 高位在前, 低位在后。
    负载返回数据: 01 06 01 16 00 01 04 00 00 07 D0 0C 9D对负载进行写单个寄存器的操作时， 负载返回数据为将指令原样返回。
    2. 设置拉载电压：、
    发送指令: 01 06 01 12 00 01 04 00 00 4E 20 2B AB
    各字节代表的意义与设置电流的指令相同， 数据字节 0x00004E20 表示将拉载电压设置为20V。负载返回数据为将指令原样返回。
    3. 设置拉载模式：
    发送指令: 01 06 01 10 00 01 04 00 00 00 01 4A DF
    各字节代表的意义与设置电流的指令相同， 数据字节 0x00000001表示将拉载模式设置为CC模式。写入O为CV 模式， 1为CC 模式， 2 为CR模式， 3为CW模式。负载返回数据为将指令原样返回。
    4. 设置拉载开关：
    发送指令: 01 06 01 0E 00 01 04 00 00 00 01 CA 5F
    各字节代表的意义与设置电流的指令相同， 数据字节 0x00000001表示将负载设为拉载ON。写入O为拉载OFF， 写入1 为拉载 ON。负载返回数据为将指令原样返回。
    5. 读取实际电压电流：
    发送指令: 01 03 03 00 00 00 8E 45
    这是一条特殊指令， 方便一次读取常用数据寄存器组：
    01 ：设备地址。
    03 ：读寄存器命令号。
    03 00 ：读常用寄存器组特殊定义地址。
    00 00 ：在本条特殊指令中无意义， 可为任意值。
    8E 45 :校验码, 高位在前, 低位在后。
    负载返回数据:01 03 30 D1 D2 D3 D4 D5 D6 D7 D8 D9 D10 D11 D12D13 D14 D15 D16 D17 D18 CRCH CRCL
    其中D1-D18为有效数据。
    D1.0为ON/OFF位, D1.1-D1.2为模式位。
    D3-D5实际电压值(单位mV)，三字节24位数据， 高位在前， 低位在后。
    D6-D8实际电流值(单位mA)，三字节24位数据， 高位在前， 低位在后。
    如需更多有关通讯协议的详细信息， 请联系本公司技术支持部门索取。技术参数 KUNKIN
    KP182系列负载各项技术参数如下表：
    指 标 型 号 KP182 KP184
    供电输入 电压 AC 110V/220V ±10%, 50±2Hz
    功耗 < 20 W
    负载输入 负载电压 DC 1 - 150 V
    负载电流 0-20A 0-40A
    负载功率 0-200W 0-400W
    测量精度 电流 ±0.1%+5mA ±0.05%+5mA
    电压 ±0.1%+5mV ±0.05%+5mV
    控制精度 电流 ±0.1%+5mA ±0.05%+5mA
    电压 ±0.1%+5mV ±0.05%+5mV
    通讯方式 不支持通讯 RS232/RS485
    保护 过压(OV) 大于152 V 关闭带载
    过温(OT) 85℃
    过功率(OP) 单通道210W 单通道410W
    供电输入保险丝0. 5A
    使用环境温度范围 0~50℃
    使用环境湿度范围 10~90%RH
    DC 输入端对机箱耐压 ±500VDC
    DC 输入端对机箱绝缘电阻 >20MΩ,500VDC时
    AC 输入端对机箱绝缘电阻 >20MΩ,500VDC时
    外形尺寸(mm) L×B×H 裸机约300×90×190,包装后约390×160×270
    重量(约) 3.5 Kg 4.5 Kg
    补充特性
    建议校准频率： 1次/年
    操作环境温度: 0 - 40 °C
    储存环境温度: - 20 to 70 °C
    使用环境： 室内使用设计， 最大湿度 95%
     */
    public class KP184: IDisposable
    {
        private SerialPort _serialPort;
        private readonly byte _deviceAddress;

        public KP184(string portName, int baudRate, byte deviceAddress)
        {
            if (deviceAddress < 1 || deviceAddress > 99)
                throw new ArgumentOutOfRangeException(nameof(deviceAddress), "Device address must be between 1 and 99.");

            _deviceAddress = deviceAddress;
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.ReadTimeout = 100;
            _serialPort.Open();
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

        /// <summary>
        /// LOAD ONOFF 设备开关，1为开ON,0为关OFF
        /// </summary>
        /// <param name="on"></param>
        public void SetLoadSwitch(bool on)
        {
            WriteSingleRegister(0x010E, on ? 1 : 0);
        }

        /// <summary>
        /// LOAD MODE 设置设备模式 0-CV，1-CC, 2-CR, 3-CW,设备ON时不能切换加载模式
        /// </summary>
        /// <param name="mode"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetLoadMode(int mode)
        {
            if (mode < 0 || mode > 3)
                throw new ArgumentOutOfRangeException(nameof(mode), "Mode must be between 0 and 3.");

            WriteSingleRegister(0x0110, mode);
        }

        /// <summary>
        /// CC SETTING 设置设备加载值，单位mA
        /// </summary>
        /// <param name="currentInMilliAmps"></param>
        public void SetCurrentLoad(int currentInMilliAmps)
        {
            WriteSingleRegister(0x0116, currentInMilliAmps);
        }

        /// <summary>
        /// CV SETTING 设置设备电压值，单位mV
        /// </summary>
        /// <param name="voltageInMilliVolts"></param>
        public void SetVoltageLoad(int voltageInMilliVolts)
        {
            WriteSingleRegister(0x0112, voltageInMilliVolts);
        }

        /// <summary>
        /// CR SETTING 设置设备电阻值，单位mV
        /// </summary>
        /// <param name="resistanceInOhms"></param>
        public void SetResistanceLoad(int resistanceInOhms)
        {
            WriteSingleRegister(0x011A, resistanceInOhms);
        }

        /// <summary>
        /// CW SETTING 设置设备功率值，单位0.1W
        /// </summary>
        /// <param name="powerInTenthWatts"></param>
        public void SetPowerLoad(int powerInTenthWatts)
        {
            WriteSingleRegister(0x011E, powerInTenthWatts);
        }

        /// <summary>
        /// 读取实际电压电流值
        /// </summary>
        /// <param name="voltage"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool ReadVoltageAndCurrent(out int voltage, out int current)
        {
            /*
             » 01 03 03 00 00 00 45 8E
             « 01 03 1E 01 00 00 00 00 00 00 00 00 14 B4 0F A0 0F A0 00 00 00 D4 09
             */
            /*var response = SendCommand(new byte[] { _deviceAddress, 0x03, 0x03, 0x00, 0x00, 0x00 }, 23);
            Console.WriteLine(response.Length);
            if (response.Length < 23)
            {
                Console.WriteLine("Invalid response length.");
                voltage = -1;
                current = -1;
                return false;
            }*/
            try
            {
                var response = ReadSingleRegister(0x0300);

                voltage = (response[5] << 16) | (response[6] << 8) | response[7];
                current = (response[8] << 16) | (response[9] << 8) | response[10];
                return true;

            }
            catch (Exception ee)
            {
                voltage = 0; current = 0;   
                return false;
            }
        }

        private void WriteSingleRegister(ushort registerAddress, int value)
        {
            byte[] data = new byte[]
            {
                _deviceAddress,                     // 设备地址
                0x06,                               // 功能码：0x03-读取寄存器；0x06-写入单个寄存器；0x10-写入多个连续寄存器
                (byte)(registerAddress >> 8),       // 写入目标寄存器的地址
                (byte)(registerAddress & 0xFF),     // 写入目标寄存器的地址
                0x00, 0x01,                         // 写入目标寄存器的个数
                0x04,                               // 要写入目标寄存器的数据的字节数，目标寄存器为4字节寄存器，所以为4
                (byte)(value >> 24),                // 数据
                (byte)((value >> 16) & 0xFF),       // 数据
                (byte)((value >> 8) & 0xFF),        // 数据
                (byte)(value & 0xFF)                // 数据
            };
            SendCommand(data);
        }

        public void ReadVoltageMeasure()
        {
            ReadSingleRegister(0x0122);
        }

        private byte[] ReadSingleRegister(ushort registerAddress)
        {
            byte[] data = new byte[]
            {
                _deviceAddress,                     // 设备地址
                0x03,                               // 功能码：0x03-读取寄存器；0x06-写入单个寄存器；0x10-写入多个连续寄存器
                (byte)(registerAddress >> 8),       // 读取目标寄存器的地址
                (byte)(registerAddress & 0xFF),     // 读取目标寄存器的地址
                0x00, 0x00,                         // 无意义，任意值
            };
            return SendCommand(data);
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private byte[] SendCommand(byte[] command, int expectedLength = 8)
        {
            try
            {
                // 计算CRC并拼接指令
                byte[] crc = CalculateCRC(command);
                byte[] fullCommand = command.Concat(crc).ToArray();
                Console.WriteLine("Send: " + BitConverter.ToString(fullCommand));

                // 清空接收缓冲区，防止读取到之前的数据
                _serialPort.DiscardInBuffer();

                // 发送指令
                _serialPort.Write(fullCommand, 0, fullCommand.Length);

                // 接收返回数据
                List<byte> receivedData = new List<byte>();
                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalMilliseconds < _serialPort.ReadTimeout)
                {
                    if (_serialPort.BytesToRead > 0)
                    {
                        receivedData.Add((byte)_serialPort.ReadByte());
                    }

                    // 假设设备响应以 0x0D (CR) 作为结束标志
                    if (receivedData.Count >= expectedLength && receivedData.Last() == 0x0D)
                    {
                        break;
                    }
                }

                // 返回完整的接收数据
                byte[] response = receivedData.ToArray();
                Console.WriteLine("Recv: " + BitConverter.ToString(response));
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Array.Empty<byte>();
            }
        }

        private bool IsCompleteResponse(int expectedLength, List<byte> receivedData)
        {
            //int expectedLength = 16; // 根据协议定义的固定长度
            return receivedData.Count >= expectedLength;
        }


        /// <summary>
        /// 生成校验码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] CalculateCRC(byte[] data)
        {
            ushort crc = 0xFFFF;
            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 1) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return new byte[]
            {
                (byte)(crc & 0xFF),
                (byte)(crc >> 8)
            };
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }
}
