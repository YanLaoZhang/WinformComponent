using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TES136Debug
{
    public class HIDResult
    {
        public float ev;
        public float tcp;
        public float uv;

        public override string ToString()
        {
            return $"TES136: EV-{ev};TCP-{tcp}; UV-{uv}";
        }
    }

    public class HidDevicesLib
    {
        public static List<HidDevice> FindAllHIDDevices()
        {
            // 查找所有 HID 设备
            return HidDevices.Enumerate().ToList();
        }

        // 将字符串转换为十六进制
        public static string StringToHex(string input)
        {
            // 将字符串转换为整数，再转换为十六进制字符串
            int number = int.Parse(input);
            return number.ToString("X"); // "X" 格式表示十六进制
        }

        // 将字节数组合并为一个长整型 (64 位)
        static long CombineBytesToLong(byte[] bytes)
        {
            if (bytes.Length > 8)
                throw new ArgumentException("字节数组长度不能超过 8 字节。");

            long result = 0;
            foreach (byte b in bytes)
            {
                result = (result << 8) | b; // 左移并拼接字节
            }
            return result;
        }

        public static HIDResult GetHIDResult(string vendor_id, string product_id, ref string str_error_log)
        {
            try
            {
                int vendorId = int.Parse(vendor_id); // 替换为你的设备 Vendor ID
                int productId = int.Parse(product_id); // 替换为你的设备 Product ID
                str_error_log += $"{vendorId}-{productId}";
                string error_log = "";

                // 查找设备
                var device = HidDevices.Enumerate(vendorId, productId).FirstOrDefault();

                if (device != null)
                {
                    Console.WriteLine("HID 设备已连接。");
                    device.OpenDevice();

                    if (!device.IsOpen)
                    {
                        str_error_log += "设备打开失败。";
                        return null;
                    }

                    short len = device.Capabilities.OutputReportByteLength;
                    byte[] outputData = new byte[len];
                    if (len < 3)
                    {
                        str_error_log += $"设备输出字节长度有限[{len}]";
                        return null;
                    }

                    outputData[0] = 0x00; // Report ID，一般为 0
                    outputData[1] = 0x01; // 指令
                    outputData[2] = 0x62; // 指令

                    // 创建一个任务来确保异步操作完成
                    var writeTaskCompletionSource = new TaskCompletionSource<bool>();

                    device.Write(outputData, success =>
                    {
                        if (success)
                        {
                            Console.WriteLine("指令发送成功。");
                            error_log = "指令发送成功";
                            writeTaskCompletionSource.SetResult(true);  // 标记写操作成功完成
                        }
                        else
                        {
                            Console.WriteLine("指令发送失败。");
                            error_log = "指令发送失败";
                            writeTaskCompletionSource.SetResult(false);  // 标记写操作失败
                        }
                    });

                    // 等待写操作完成
                    bool isSendSuccess = writeTaskCompletionSource.Task.Result;
                    if (!isSendSuccess)
                    {
                        str_error_log += error_log;
                        return null;
                    }

                    HIDResult hIDResult = new HIDResult();
                    var readTaskCompletionSource = new TaskCompletionSource<bool>();

                    device.ReadReport(report =>
                    {
                        if (report.Exists)
                        {
                            byte[] recv = report.Data;
                            Console.WriteLine("返回数据: " + BitConverter.ToString(recv));

                            error_log = $"返回数据: {BitConverter.ToString(recv)}";
                            // 解析第 14, 15, 16 位的数据 (1-based 索引)
                            byte[] evBytes = new byte[] { recv[13], recv[14], recv[15] }; // 0-based 索引
                            hIDResult.ev = CombineBytesToLong(evBytes) / 10000.0f;
                            // 解析第 41, 42 位的数据
                            byte[] tcpBytes = new byte[] { recv[40], recv[41] };
                            hIDResult.tcp = CombineBytesToLong(tcpBytes);
                            // 解析第 43, 44 位的数据
                            byte[] uvBytes = new byte[] { recv[42], recv[43] };
                            hIDResult.uv = CombineBytesToLong(uvBytes) / 10000.0f;

                            readTaskCompletionSource.SetResult(true);  // 标记读操作完成
                        }
                        else
                        {
                            error_log = "未收到返回数据";
                            readTaskCompletionSource.SetResult(false);  // 标记读操作失败
                        }
                    });

                    // 等待读操作完成
                    bool isRecvSuccess = readTaskCompletionSource.Task.Result;
                    if (!isRecvSuccess)
                    {
                        str_error_log += error_log;
                        return null;
                    }

                    device.CloseDevice();
                    return hIDResult;
                }
                else
                {
                    str_error_log += "未找到 HID 设备";
                    return null;
                }
            }
            catch (Exception ee)
            {
                str_error_log += ee.Message + "--" + ee.StackTrace;
                return null;
            }
        }


        public static HIDResult GetHIDResult1(string vendor_id, string product_id, ref string str_error_log)
        {
            try
            {

                int vendorId = int.Parse(vendor_id); // 替换为你的设备 Vendor ID
                int productId = int.Parse(product_id); // 替换为你的设备 Product ID
                str_error_log += $"{vendorId}-{productId}";
                string error_log = "";
                // 查找设备
                var device = HidDevices.Enumerate(vendorId, productId).FirstOrDefault();

                if (device != null)
                {
                    Console.WriteLine("HID 设备已连接。");
                    //richTextBox1.Text += "HID 设备已连接\r\n";
                    device.OpenDevice();

                    /*// 发送指令
                    byte[] command = new byte[65]; // HID 报告一般从索引 1 开始，索引 0 为 Report ID
                    command[1] = 0x01; // 替换为实际命令*/
                    // 根据协议发送指令（如：<0x00>, <0x01>, <0x62>）
                    short len = device.Capabilities.OutputReportByteLength;
                    byte[] outputData = new byte[device.Capabilities.OutputReportByteLength];
                    if (len < 3)
                    {
                        str_error_log += $"设备输出字节长度有限[{len}]";
                        return null;
                    }
                    outputData[0] = 0x00; // Report ID，一般为 0
                    outputData[1] = 0x01; // 指令
                    outputData[2] = 0x62; // 指令
                    bool isSend = false;
                    device.Write(outputData, success =>
                    {
                        if (success)
                        {
                            Console.WriteLine("指令发送成功。");
                            error_log = "指令发送成功";
                            isSend = true;
                        }
                        else
                        {
                            Console.WriteLine("指令发送失败。");
                            error_log = $"指令发送失败";
                        }
                    });
                    str_error_log += error_log;
                    if (!isSend) return null;

                    HIDResult hIDResult = new HIDResult();
                    // 读取返回数据
                    bool isRecv = false;
                    device.ReadReport(report =>
                    {
                        if (report.Exists)
                        {
                            isRecv = true;
                            byte[] recv = report.Data;
                            Console.WriteLine("返回数据: " + BitConverter.ToString(recv));

                            error_log = $"返回数据{BitConverter.ToString(recv)}";
                            // 解析第 14, 15, 16 位的数据 (1-based 索引)
                            byte[] evBytes = new byte[] { recv[13], recv[14], recv[15] }; // 0-based 索引
                                                                                          // 将 3 个字节组合为一个 64 位数值
                            hIDResult.ev = CombineBytesToLong(evBytes) / 10000.0f;
                            // 解析第 41, 42位的数据 (1-based 索引)
                            byte[] tcpBytes = new byte[] { recv[41], recv[42] }; // 0-based 索引
                                                                                 // 将 3 个字节组合为一个 64 位数值
                            hIDResult.tcp = CombineBytesToLong(tcpBytes);
                            // 解析第 43, 44位的数据 (1-based 索引)
                            byte[] uvBytes = new byte[] { recv[41], recv[42] }; // 0-based 索引
                                                                                // 将 3 个字节组合为一个 64 位数值
                            hIDResult.uv = CombineBytesToLong(uvBytes) / 10000.0f;
                            Console.WriteLine("清空旧数据: " + BitConverter.ToString(report.Data));
                            report = device.ReadReport();
                        }
                        else
                        {
                            Console.WriteLine("未收到返回数据。");
                            error_log = "未收到返回数据";
                        }
                    });
                    str_error_log += error_log;
                    if (!isRecv) return null;
                    device.CloseDevice();
                    str_error_log += error_log;
                    return hIDResult;
                }
                else
                {
                    Console.WriteLine("未找到 HID 设备。");
                    str_error_log += "未找到 HID 设备";
                    return null;
                }
            }
            catch (Exception ee)
            {
                str_error_log += ee.Message + "--"+ ee.StackTrace;
                return null;
            }

        }
    }
}
