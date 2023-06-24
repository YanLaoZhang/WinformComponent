using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CameraRTOSLib
{
    public partial class ShowUSBCamera : Form
    {
        int index = 0;

        public ShowUSBCamera(int index)
        {
            InitializeComponent();
            this.index = index;
        }

        private void ShowUSBCamera_Load(object sender, EventArgs e)
        {
            //int height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            //int width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            //this.Height = height;
            //this.Width = height;
            //int startX = (width - height) / 2;
            //this.Location = new System.Drawing.Point(startX, 0);
            //if (height < 1536)
            //{
            //    videoSourcePlayer1.Height = height - 40;
            //    videoSourcePlayer1.Width = height - 40;
            //}

            //videoSourcePlayer1.Height = 1920;
            //videoSourcePlayer1.Width = 1080;

            GetCameraVideo();
        }

        private void ShowUSBCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShutCamera();
            //Form1.For_control_modify.showUSBCamera = null;
        }

        public bool CheckICatchV37(ref string str_error_log)
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
                    str_error_log = $"未找到摄像头{camera}";
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"开启摄像头，发生异常:{ee.Message}";
                return false;
            }
        }

        private bool GetCameraVideo()
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
                    return false;
                }
                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[index].MonikerString);
                videoSource.VideoResolution = videoSource.VideoCapabilities[this.index];
                videoSourcePlayer1.Height = videoSource.VideoResolution.FrameSize.Height;
                videoSourcePlayer1.Width = videoSource.VideoResolution.FrameSize.Width;
                videoSourcePlayer1.VideoSource = videoSource;
                videoSourcePlayer1.Start();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show($"开启摄像头，发生异常:{ee.Message}");
                return false;
            }
        }

        public Bitmap CapturePNG(string Path_image_file)
        {
            try
            {
                if (videoSourcePlayer1.VideoSource != null)
                {
                    Bitmap img = videoSourcePlayer1.GetCurrentVideoFrame();
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

        /// <summary>
        /// 关闭并释放摄像头
        /// </summary>
        public void ShutCamera()
        {
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                videoSourcePlayer1.VideoSource = null;
            }
        }

        private void ShowUSBCamera_Resize(object sender, EventArgs e)
        {
            //int min = Math.Min(panel1.Width, panel1.Height);
            //if (min < 1536)
            //{
            //    videoSourcePlayer1.Width = min;
            //    videoSourcePlayer1.Height = min;
            //}
            //else
            //{
            //    videoSourcePlayer1.Width = 1536;
            //    videoSourcePlayer1.Height = 1536;
            //}
            //int centerX = (panel1.Width - videoSourcePlayer1.Width) / 2;
            ////int centerY = (panel1.Height - videoSourcePlayer1.Height) / 2;

            //videoSourcePlayer1.Location = new System.Drawing.Point(centerX, 0);
        }
    }
}
