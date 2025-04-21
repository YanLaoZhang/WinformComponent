using HidLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TES136Debug
{
    public partial class Form1 : Form
    {
        public static string receive = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // 查找所有 HID 设备
            var devices = HidDevices.Enumerate().ToList();
            foreach (var device in devices)
            {
                Trace.WriteLine($"Device: {device.Description}");
                Trace.WriteLine($"Vendor ID: {device.Attributes.VendorId}");
                Trace.WriteLine($"Product ID: {device.Attributes.ProductId}");
                richTextBox1.Text += $"Device: {device.Description}\r\n";
                richTextBox1.Text += $"Vendor ID: {device.Attributes.VendorId}\r\n";
                richTextBox1.Text += $"Product ID: {device.Attributes.ProductId}\r\n";
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if(comboBoxHIDDevices.Text.Length < 0)
            {
                MessageBox.Show($"请先选择设备");
                comboBoxHIDDevices.Focus();
                return;
            }
            string vendorId = comboBoxHIDDevices.Text.Split('-')[0];
            string productId = comboBoxHIDDevices.Text.Split('-')[1];

            string str_error_log = "";
            HIDResult hIDResult = HidDevicesLib.GetHIDResult(vendorId, productId, ref str_error_log);
            if (hIDResult != null)
            {
                richTextBox1.Text += hIDResult.ToString() + "\r\n";
            }
            else
            {
                richTextBox1.Text += $"读取失败:{str_error_log}";
            }
        }


        static void ReadData(HidDevice device)
        {
            // 异步读取数据
            device.ReadReport(OnReport);
        }

        static void OnReport(HidReport report)
        {
            if (report != null && report.Data.Length > 0)
            {
                receive = BitConverter.ToString(report.Data);
                Trace.WriteLine("接收到的数据: " + BitConverter.ToString(report.Data));
                //MessageBox.Show("接收到的数据: " + BitConverter.ToString(report.Data));
            }
            else
            {
                Trace.WriteLine("读取失败或无数据返回。");
                //MessageBox.Show("读取失败或无数据返回");
            }
        }

        static void WriteData(HidDevice device)
        {
            // 根据协议发送指令（如：<0x00>, <0x01>, <0x62>）
            byte[] outputData = new byte[device.Capabilities.OutputReportByteLength];
            outputData[0] = 0x00; // Report ID，一般为 0
            outputData[1] = 0x01; // 指令
            outputData[2] = 0x62; // 指令

            bool success = device.WriteReport(new HidReport(outputData.Length, new HidDeviceData(outputData, HidDeviceData.ReadStatus.Success)));

            if (success)
            {
                Trace.WriteLine("指令发送成功！");
                //MessageBox.Show("指令发送成功！");
            }
            else
            {
                Trace.WriteLine("指令发送失败！");
                //MessageBox.Show("指令发送失败！");
            }
        }

        private void BtnSend1_Click(object sender, EventArgs e)
        {
            // 替换为实际的 Vendor ID 和 Product ID
            int targetVendorId = 0x1005; // 示例 Vendor ID
            int targetProductId = 0x0136; // 示例 Product ID

            // 查找目标 HID 设备
            var device = HidDevices.Enumerate(targetVendorId, targetProductId).FirstOrDefault();

            if (device != null)
            {
                Trace.WriteLine($"找到设备: {device.Description}");

                //richTextBox1.Text += $"找到设备: {device.Description}";
                device.OpenDevice();

                // 读取数据
                ReadData(device);

                // 写入数据
                WriteData(device);
                Thread.Sleep(1000);
                richTextBox1.Text += $"\r\nRECV: " + receive + "\r\n";

                device.CloseDevice();
            }
            else
            {
                Trace.WriteLine("设备未找到，请检查连接和ID配置。");

                richTextBox1.Text += $"设备未找到，请检查连接和ID配置";
            }
        }

        private void comboBoxHIDDevices_DropDown(object sender, EventArgs e)
        {
            List<HidDevice> hidDevices = HidDevicesLib.FindAllHIDDevices();
            comboBoxHIDDevices.Items.Clear();
            foreach (var device in hidDevices)
            {
                string str_devices = $"{device.Attributes.VendorId}-{device.Attributes.ProductId}-{device.Description}";
                comboBoxHIDDevices.Items.Add(str_devices);
            }
        }
    }
}
