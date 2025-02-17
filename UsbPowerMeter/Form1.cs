using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsbPowerMeter
{
    public partial class Form1: Form
    {
        private static UsbPowerMeter _meter;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                BtnReadData.Enabled = false;
                string portName = comboBox1.Text;
                if(portName.Length == 0)
                {
                    MessageBox.Show("请选择串口");
                    return;
                }
                // 使用传统的using语句（兼容C# 7.3）
                using (_meter = new UsbPowerMeter(portName))
                {
                    try
                    {
                        _meter.Open();

                        // 订阅事件（传统事件注册方式）
                        _meter.SyncDataReceived += Meter_SyncDataReceived;
                        _meter.WaveformDataReceived += Meter_WaveformDataReceived;

                        // 设置单个参数
                        SetSingleParameterExample();

                        // 批量设置参数
                        SetAllParametersExample();

                        // 更改采样率
                        _meter.SetSamplingRate(SamplingSpeed.Medium);

                        // 请求同步数据
                        RequestSyncDataExample();

                        // 保持运行接收波形数据
                        //Console.WriteLine("Monitoring... Press any key to exit");
                        //Console.ReadKey();
                        Thread.Sleep(1000);

                        // 收集5秒数据
                        var stats = _meter.GetPowerStatistics(TimeSpan.FromSeconds(5));

                        Console.WriteLine($"统计结果（{stats.Samples}个样本）：");
                        Console.WriteLine($"最大值: {stats.Max:F4} W");
                        Console.WriteLine($"最小值: {stats.Min:F4} W");
                        Console.WriteLine($"平均值: {stats.Avg:F4} W");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                BtnReadData.Enabled = true;
            }
        }

        private static void Meter_SyncDataReceived(object sender, SyncDataEventArgs e)
        {
            // 传统字符串格式化（兼容C# 7.3）
            Console.WriteLine("Received sync data:");
            for (int i = 0; i < e.Parameters.Count; i++)
            {
                Console.WriteLine($"Page {(char)('A' + i)}: " +
                    $"{e.Parameters[i].Frequency}MHz, " +
                    $"{e.Parameters[i].Offset.ToString("+0.0;-0.0")}dB");
            }
        }

        private static void Meter_WaveformDataReceived(object sender, WaveformDataEventArgs e)
        {
            // 传统switch处理单位转换
            string unit;
            switch (e.Unit)
            {
                case PowerUnit.Microwatt:
                    unit = "μW";
                    break;
                case PowerUnit.Milliwatt:
                    unit = "mW";
                    break;
                case PowerUnit.Watt:
                    unit = "W";
                    break;
                default:
                    unit = "Unknown";
                    break;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] " +
                $"{e.DbmValue.ToString("+0.0;-0.0")}dBm, " +
                $"{e.PowerValue.ToString("0.00")}{unit}");
        }

        private static void SetSingleParameterExample()
        {
            // 设置A通道参数
            _meter.SetPageParameter('A', 5658, +10.0);
            Console.WriteLine("Set page A: 5658MHz +10.0dB");
        }

        private static void SetAllParametersExample()
        {
            // 传统集合初始化方式
            var parameters = new List<PageParameter>
        {
            new PageParameter(5658, +10.0),
            new PageParameter(5700, -5.5),
            new PageParameter(5750, +2.3),
            new PageParameter(5800, -1.8),
            new PageParameter(5850, +0.0),
            new PageParameter(5900, -3.2),
            new PageParameter(5950, +4.7),
            new PageParameter(6000, -6.1),
            new PageParameter(6050, +8.9)
        };

            _meter.SetAllPages(parameters);
            Console.WriteLine("All parameters set");
        }

        private static void RequestSyncDataExample()
        {
            _meter.RequestSyncData();
            Console.WriteLine("Sync data requested");
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var port in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(port);
            }
        }
    }
}
