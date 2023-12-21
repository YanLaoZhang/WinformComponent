using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AForge
{
    public partial class AForgeForm : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public AForgeForm()
        {
            InitializeComponent();
        }

        private void AForgeForm_Load(object sender, EventArgs e)
        {
            try
            {
                string camera = "iCatch V37";
                // 获取所有摄像头
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                int index = -1;
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    if (videoDevices[i].Name.Contains(camera))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    MessageBox.Show($"未找到摄像头{camera}");
                    this.Close();
                }
                videoSource = new VideoCaptureDevice(videoDevices[index].MonikerString);

                // Get available video resolutions for the selected device
                var resolutions = videoSource.VideoCapabilities.Select(cap => cap.FrameSize);

                // Choose the highest resolution
                var highestResolution = resolutions.OrderByDescending(res => res.Width * res.Height).FirstOrDefault();

                if (highestResolution != null)
                {
                    // Set the video resolution
                    videoSource.VideoResolution = videoSource.VideoCapabilities
                        .FirstOrDefault(cap => cap.FrameSize == highestResolution);
                    Console.WriteLine($"{highestResolution}");
                    // Register event handler for new frames
                    videoSource.NewFrame += VideoSource_NewFrame;

                    int startNuma = Environment.TickCount;

                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 准备启动");
                    // Start capturing
                    videoSource.Start();
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 完成启动");

                    timer1.Interval = 1000;
                    timer1.Start();
                    panel1.Enabled = true;
                    CenterPanel();
                    progressBar1.Value = 0;
                }
                else
                {
                    MessageBox.Show("No supported resolutions found.");
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show($"开启摄像头，发生异常:{ee.Message}");
                this.Close();
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            timer1.Stop();
            progressBar1.Value = 100;
            panel1.Visible = false;
            var height = eventArgs.Frame.Height;
            var width = eventArgs.Frame.Width;
            //Console.WriteLine($"分辨率：w[{width}]-h[{height}]");
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 准备显示图片");
            pictureBox1.Image = (System.Drawing.Image)eventArgs.Frame.Clone();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 完成显示");

            GC.Collect();  //强制GC来恢复
        }

        public Bitmap CapturePNG(string Path_image_file)
        {
            try
            {
                if (pictureBox1.Image != null)
                {
                    Bitmap img = (Bitmap)pictureBox1.Image.Clone();
                    if (img != null)
                    {
                        img.Save(Path_image_file, System.Drawing.Imaging.ImageFormat.Png);
                        return img;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("摄像头未打开，无法截图");
                    return null;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show($"截图发生异常{ee.Message}");
                return null;
            }
        }


        private void AForgeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= VideoSource_NewFrame;
                Console.WriteLine($"已释放资源");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(18);
        }

        private void CenterPanel()
        {
            // 计算Panel应该居中的位置
            int x = (this.Width - panel1.Width) / 2;
            int y = (this.Height - panel1.Height) / 2;

            // 设置Panel的Location属性
            panel1.Location = new Point(x, y);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            // 在Form的Resize事件中重新计算并设置Panel的位置
            CenterPanel();
        }
    }
}
