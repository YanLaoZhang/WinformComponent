using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;


namespace IT8500Controller
{
    public class IT8500Controller : IDisposable
    {
        private SerialPort _serialPort;

        public IT8500Controller(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000
            };
            _serialPort.Open();
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }

        private byte[] BuildCommand(byte address, byte command, byte[] data)
        {
            byte[] frame = new byte[26];
            frame[0] = 0xAA; // 同步头
            frame[1] = address; // 负载地址
            frame[2] = command; // 命令字
            Array.Copy(data, 0, frame, 3, Math.Min(data.Length, 22)); // 填充数据
            frame[25] = CalculateChecksum(frame); // 校验码
            return frame;
        }

        /// <summary>
        /// 计算校验码
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private byte CalculateChecksum(byte[] frame)
        {
            byte checksum = 0;
            for (int i = 0; i < 25; i++)
            {
                checksum += frame[i];
            }
            return checksum;
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="address"></param>
        /// <param name="command"></param>
        /// <param name="data"></param>
        public void SendCommand(byte address, byte command, byte[] data)
        {
            _serialPort.DiscardInBuffer(); // 清空接收缓冲区
            byte[] frame = BuildCommand(address, command, data);
            Console.WriteLine($"send: " + BitConverter.ToString(frame));
            _serialPort.Write(frame, 0, frame.Length);
        }

        /// <summary>
        /// 接收响应
        /// </summary>
        /// <returns></returns>
        public byte[] ReadResponse()
        {
            /*byte[] buffer = new byte[26];
            _serialPort.Read(buffer, 0, buffer.Length);
            Console.WriteLine($"recv: " + BitConverter.ToString(buffer));
            return buffer;*/

            /*// 改用循环读取的方式，确保缓冲区中的数据足够
            byte[] buffer = new byte[26];
            int bytesRead = 0;

            while (bytesRead < buffer.Length)
            {
                bytesRead += _serialPort.Read(buffer, bytesRead, buffer.Length - bytesRead);
            }

            Console.WriteLine($"recv: " + BitConverter.ToString(buffer));
            return buffer;*/

            // 
            List<byte> response = new List<byte>();
            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < _serialPort.ReadTimeout)
            {
                if (_serialPort.BytesToRead > 0)
                {
                    response.Add((byte)_serialPort.ReadByte());
                }

                // 假设设备响应以 0x0D (CR) 作为结束标志
                if (response.Count >= 26 && response.Last() == 0x0D)
                {
                    break;
                }
            }

            if (response.Count == 0)
                throw new TimeoutException("No data received within the timeout period.");

            Console.WriteLine($"recv: " + BitConverter.ToString(response.ToArray()));
            return response.ToArray();
        }

        /// <summary>
        /// 设置负载的控制模式 20H
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mode">操作模式（0为面板操作模式，1为远程操作模式）</param>
        public void SetControlMode(byte address, byte mode)
        {
            Console.WriteLine($"设置负载控制模式");
            byte[] data = new byte[22];
            data[0] = mode;
            SendCommand(address, 0x20, data);
        }

        /// <summary>
        /// 控制负载输入状态
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mode">负载输入状态（0 为输出OFF, 1 为输出ON）</param>
        public void SetLoadInputState(byte address, byte mode)
        {
            Console.WriteLine($"设置负载输入状态");
            byte[] data = new byte[22];
            data[0] = mode;
            SendCommand(address, 0x21, data);
        }

        /// <summary>
        /// 设置负载模式，设置模式：0-CC, 1-CV, 2-CW, 3-CR
        /// </summary>
        /// <param name="address"></param>
        /// <param name="mode"></param>
        public void SetLoadMode(byte address, byte mode)
        {
            Console.WriteLine($"设置负载模式");
            byte[] data = new byte[22];
            data[0] = mode; // 设置模式：0-CC, 1-CV, 2-CW, 3-CR
            SendCommand(address, 0x28, data);
        }

        /// <summary>
        /// 读取负载模式
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLoadMode(byte address)
        {
            Console.WriteLine($"读取负载模式");
            SendCommand(address, 0x29, new byte[22]);
            byte[] response = ReadResponse();
            return response[3];
        }

        /// <summary>
        /// 设置负载的定电流值 2A
        /// </summary>
        /// <param name="address"></param>
        /// <param name="current"></param>
        public void SetLoadConstantCurrentValue(byte address, double current)
        {
            Console.WriteLine($"设置负载的定电流值");
            byte[] data = new byte[22];
            int currentInDec = (int)(current * 10000); // 转换为0.1mA单位
            data[0] = (byte)(currentInDec & 0xFF);
            data[1] = (byte)((currentInDec >> 8) & 0xFF);
            data[2] = (byte)((currentInDec >> 16) & 0xFF);
            data[3] = (byte)((currentInDec >> 24) & 0xFF);
            SendCommand(address, 0x2A, data);
        }

        /// <summary>
        /// 读取负载的定电流值 2B
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double GetLoadConstantCurrentValue(byte address)
        {
            Console.WriteLine($"读取负载的定电流值");
            SendCommand(address, 0x2B, new byte[22]);
            byte[] response = ReadResponse();
            int currentDec = response[3] | (response[4] << 8) | (response[5] << 16) | (response[6] << 24);
            return currentDec / 10000.0; // 转换为A
        }

        /// <summary>
        /// 读取负载的定电压值 2C
        /// </summary>
        /// <param name="address"></param>
        /// <param name="current"></param>
        public void SetLoadConstantVoltageValue(byte address, double current)
        {
            Console.WriteLine($"设置负载的定电压值");
            byte[] data = new byte[22];
            int currentInDec = (int)(current * 1000); // 转换为1mV单位
            data[0] = (byte)(currentInDec & 0xFF);
            data[1] = (byte)((currentInDec >> 8) & 0xFF);
            data[2] = (byte)((currentInDec >> 16) & 0xFF);
            data[3] = (byte)((currentInDec >> 24) & 0xFF);
            SendCommand(address, 0x2C, data);
        }

        /// <summary>
        /// 读取负载的定电压值 2D
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double GetLoadConstantVoltageValue(byte address)
        {
            Console.WriteLine($"读取负载的定电压值");
            SendCommand(address, 0x2D, new byte[22]);
            byte[] response = ReadResponse();
            int currentDec = response[3] | (response[4] << 8) | (response[5] << 16) | (response[6] << 24);
            return currentDec / 1000.0; // 转换为V
        }

        /// <summary>
        /// 读取负载的输入电压 5F
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double ReadInputVoltage(byte address)
        {
            Console.WriteLine($"读取负载的输入电压");
            SendCommand(address, 0x5F, new byte[22]);
            byte[] response = ReadResponse();
            int voltageInDec = response[3] | (response[4] << 8) | (response[5] << 16) | (response[6] << 24);
            return voltageInDec / 1000.0; // 转换为 V
        }

        /// <summary>
        /// 读取负载的输入电流
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double ReadInputCurrent(byte address)
        {
            Console.WriteLine($"读取负载的输入电流");
            SendCommand(address, 0x5F, new byte[22]);
            byte[] response = ReadResponse();
            int currentInDec = response[7] | (response[8] << 8) | (response[9] << 16) | (response[10] << 24);
            return currentInDec / 10000.0; // 转换为 A
        }

        /// <summary>
        /// 读取负载的输入功率
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public double ReadInputPower(byte address)
        {
            Console.WriteLine($"读取负载的输入功率");
            SendCommand(address, 0x5F, new byte[22]);
            byte[] response = ReadResponse();
            int powerInDec = response[11] | (response[12] << 8) | (response[13] << 16) | (response[14] << 24);
            return powerInDec / 1000.0; // 转换为 w
        }

    }

}
