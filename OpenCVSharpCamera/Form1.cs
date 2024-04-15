using System;
using System.IO;
using System.Windows.Forms;
using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCVSharpCamera
{
    public partial class Form1 : Form
    {

        int screenWidth;
        int screenHeight;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            // 创建名为"output"的子目录
            string outputDirectory = "screenshot";
            Directory.CreateDirectory(outputDirectory);
            // 查找摄像头
            string CameraName = "iCatch V37";
            int index = CameraIndex(CameraName);

            if (index == -1)
            {
                MessageBox.Show($"未找到指定摄像头{CameraName}");
                return;
            }
            using (VideoCapture capture = new VideoCapture(index))
            {
                // 检查摄像头是否成功打开
                if (!capture.IsOpened())
                {
                    Console.WriteLine("无法打开摄像头");
                    MessageBox.Show($"无法打开摄像头");
                    return;
                }

                capture.Set(3, 1280);
                capture.Set(4, 720);
                capture.Set(5, 30);

                // 创建一个窗口来显示摄像头画面
                Cv2.NamedWindow("Camera", WindowFlags.Normal);

                // 移动和调整窗口的位置和大小
                Cv2.MoveWindow("Camera", 0, 0); // 移动窗口到屏幕坐标(100, 100)
                Cv2.ResizeWindow("Camera", screenWidth, screenHeight); // 调整窗口大小为640x480像素

                while (true)
                {
                    // 读取摄像头画面
                    Mat frame = new Mat();
                    capture.Read(frame);

                    // 检查是否成功读取帧
                    if (frame.Empty())
                    {
                        Console.WriteLine("无法接收摄像头数据，程序退出");
                        MessageBox.Show($"无法接收摄像头数据，程序退出");
                        break;
                    }

                    // 在窗口中显示当前帧
                    Cv2.ImShow("Camera", frame);

                    // 检查键盘输入
                    int key = Cv2.WaitKey(1);
                    // 检查键盘输入，按下 'q' 键退出循环
                    if (key == 'q')
                        break;
                    
                    // 按下 's' 键保存当前帧
                    if (key == 's'){
                        // 保存图像到子目录中
                        string outputPath = Path.Combine(outputDirectory, $"screenshot_{DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss")}.png");
                        Cv2.ImWrite(outputPath, frame);
                        Console.WriteLine($"截图已保存为{outputPath}");
                        MessageBox.Show($"截图已保存为{outputPath}");
                    }

                }

            }
            Cv2.DestroyAllWindows();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取主显示器的分辨率
            Screen primaryScreen = Screen.PrimaryScreen;
            screenWidth = primaryScreen.Bounds.Width;
            screenHeight = primaryScreen.Bounds.Height;
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
                        if (device.Name == CameraName)
                        {
                            Console.WriteLine("已找到指定摄像头");
                            cameraIndex = index;
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

    }
}
