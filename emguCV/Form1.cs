using Emgu.CV.UI;
using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ZedGraph;
using Emgu.CV.Ocl;
using DirectShowLib;

namespace emguCV
{
    public partial class Form1 : Form
    {
        private VideoCapture capture;
        private int cameraIndex = 1; // 摄像头索引，默认为 0
        public Form1()
        {
            InitializeComponent();
        }

        public int CameraIndex(string CameraName)
        {
            try
            {
                int cameraIndex = -1;
                // 获取所有视频输入设备
                var devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

                if (devices.Length > 0)
                {
                    // 显示摄像头信息
                    for (int index = 0; index < devices.Length; index++)
                    {
                        var device = devices[index];
                        Console.WriteLine($"摄像头索引：{index}");
                        Console.WriteLine($"摄像头名称: {device.Name}");
                        Console.WriteLine($"摄像头设备路径: {device.DevicePath}");
                        if(device.Name == CameraName)
                        {
                            Console.WriteLine("已找到指定摄像头");
                            cameraIndex =  index;
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("未找到摄像头设备");
                }
                return cameraIndex;
            }
            catch (Exception ee)
            {
                Console.WriteLine($"Exception:[{ee.Message}]");
                return -1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string CameraName = "iCatch V37";
                int index = CameraIndex(CameraName);

                if(index == -1)
                {
                    MessageBox.Show($"未找到指定摄像头{CameraName}");
                    return;
                }

                // 打开指定索引的摄像头
                capture = new VideoCapture(index);

                // 获取摄像头实际的分辨率
                double frameWidth = capture.GetCaptureProperty(CapProp.FrameWidth);
                double frameHeight = capture.GetCaptureProperty(CapProp.FrameHeight);
                double FPS = capture.GetCaptureProperty(CapProp.Fps);
                double GFWM = capture.GetCaptureProperty(CapProp.GigaFrameWidthMax);
                double GFHM = capture.GetCaptureProperty(CapProp.GigaFrameHeighMax);

                MessageBox.Show($"摄像头实际分辨率：{frameWidth} x {frameHeight},帧率：{FPS},{GFWM}-{GFHM}", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 设置摄像头画面大小（宽度和高度）
                capture.SetCaptureProperty(CapProp.FrameWidth, 1920); // 替换为希望的宽度
                capture.SetCaptureProperty(CapProp.FrameHeight, 1080); // 替换为希望的高度

                // 设置 ImageBox 控件为摄像头画面显示容器
                imageBox1.Image = capture.QueryFrame();
                Application.Idle += ProcessFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开摄像头：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            // 获取摄像头画面帧
            Mat frame = capture.QueryFrame();

            if (frame != null)
            {
                // 在 ImageBox 控件中显示摄像头画面
                imageBox1.Image = frame;
                Console.WriteLine("画面更新");
                //frame.Dispose();
            }
            else
            {
                Console.WriteLine("无画面");
                MessageBox.Show($"摄像头连接异常，请检查！");
                Application.Idle -= ProcessFrame;
                capture?.Dispose();
            }

            GC.Collect();  //强制GC来恢复
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            capture?.Dispose();
        }
    }
}
