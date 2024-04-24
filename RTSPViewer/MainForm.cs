using System;
using System.Windows.Forms;
using FFmpeg.AutoGen;

namespace RTSPViewer
{
    public partial class MainForm : Form
    {
        private FFmpeg.MediaDemuxer demuxer;
        private FFmpeg.VideoStream videoStream;
        private FFmpeg.ImageConverter converter;
        private Timer timer;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 初始化FFmpeg
            FFmpegHelper.Initialize();

            // 创建媒体解封装器
            demuxer = new FFmpeg.MediaDemuxer("rtsp://your_rtsp_stream_url");

            // 获取视频流
            videoStream = demuxer.GetVideoStream();

            // 创建图像转换器
            converter = new FFmpeg.ImageConverter(FFmpeg.AVPixelFormat.AV_PIX_FMT_BGR24, videoStream.Width, videoStream.Height);

            // 创建定时器用于定时刷新图像
            timer = new Timer();
            timer.Interval = 33; // 设置刷新频率为30帧/秒
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 释放资源
            timer.Dispose();
            converter.Dispose();
            videoStream.Dispose();
            demuxer.Dispose();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 读取视频帧
            FFmpeg.Frame frame = videoStream.ReadFrame();

            // 将帧转换为图像
            FFmpeg.Image image = converter.Convert(frame);

            // 显示图像
            pictureBox1.Image = image.ToBitmap();
        }
    }
}
