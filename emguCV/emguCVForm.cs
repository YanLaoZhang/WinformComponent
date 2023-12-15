using Emgu.CV.UI;
using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ZedGraph;
using Emgu.CV.Ocl;
using DirectShowLib;
using System.Drawing;

namespace emguCV
{
    public partial class EmguCVForm : Form
    {
        private VideoCapture capture;
        private int cameraIndex = 1; // 摄像头索引，默认为 0

        private int width;
        private int height;

        public EmguCVForm(int width, int height)
        {
            InitializeComponent();
            this.width = width;
            this.height = height;
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
                            //break;
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
                // 设置摄像头的编解码器为H.264
                capture.SetCaptureProperty(CapProp.FourCC, VideoWriter.Fourcc('H', '2', '6', '4'));

                // 获取摄像头实际的分辨率
                double frameWidth = capture.GetCaptureProperty(CapProp.FrameWidth);
                double frameHeight = capture.GetCaptureProperty(CapProp.FrameHeight);
                double FPS = capture.GetCaptureProperty(CapProp.Fps);
                double GFWM = capture.GetCaptureProperty(CapProp.GigaFrameWidthMax);
                double GFHM = capture.GetCaptureProperty(CapProp.GigaFrameHeighMax);
                double FourCC = capture.GetCaptureProperty(CapProp.FourCC);

                Console.WriteLine($"摄像头实际分辨率：{frameWidth} x {frameHeight},帧率：{FPS},FourCC:{FourCC}", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                int screenheight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                int screenwidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
                Console.WriteLine($"当前屏幕的工作区域分辨率为{screenwidth} * {screenheight}");

                if(width > 0 && height > 0)
                {
                    // 设置摄像头画面大小（宽度和高度）
                    capture.SetCaptureProperty(CapProp.FrameWidth, width); // 替换为希望的宽度
                    capture.SetCaptureProperty(CapProp.FrameHeight, height); // 替换为希望的高度
                    Console.WriteLine($"设置摄像头分辨率{width}*{height}");
                }
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
                Console.WriteLine($"摄像头连接异常，请检查！");
                Application.Idle -= ProcessFrame;
                capture?.Dispose();
            }

            GC.Collect();  //强制GC来恢复
        }

        public Bitmap SaveScreenshot(string filename)
        {
            try
            {
                // 获取摄像头画面帧
                Mat frame = capture.QueryFrame();
                // 捕获当前画面并保存为截图
                if (!frame.IsEmpty)
                {
                    Bitmap bitmap = frame.ToImage<Bgr, byte>().ToBitmap();
                    // 保存截图
                    bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Console.WriteLine("Screenshot saved to: " + filename);
                    return bitmap;
                }
                else
                {
                    Console.WriteLine("Screenshot is empty");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving screenshot: " + ex.Message);
                return null;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            capture?.Dispose();
        }
    }
}
