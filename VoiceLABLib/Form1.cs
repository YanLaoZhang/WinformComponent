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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace VoiceLABLib
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnRead.Enabled = false;
                richTextBox1.Clear();
                if (VoiceLAB.IsOpenVoiceLAB())
                {
                    Console.WriteLine("VoiceLAB V2.1程序已打开");
                }
                else
                {
                    MessageBox.Show("异常：VoiceLAB V2.1程序未打开");
                    return;
                }
                Console.WriteLine("请确保VoiceLAB V2.1程序的配置OK");
                // 读取分贝数据
                Console.WriteLine($"读取分贝仪记录的分贝值...");
                string str_error_log = "";
                int duration = (int)numericUpDownDuration.Value;
                if (VoiceLAB.GetDataFromMeter(out VoiceLABResult voiceLABResult, duration, ref str_error_log))
                {
                    richTextBox1.Text += voiceLABResult.ToString();
                }
                else
                {
                    Console.WriteLine($"读取蜂鸣器分贝值失败[{str_error_log}]");
                    richTextBox1.Text = $"读取蜂鸣器分贝值失败[{str_error_log}]";
                }
            }
            finally 
            {
                BtnRead.Enabled = true;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                BtnStart.Enabled = false;
                richTextBox1.Clear();
                if (VoiceLAB.IsOpenVoiceLAB())
                {
                    Console.WriteLine("VoiceLAB V2.1程序已打开");
                }
                else
                {
                    MessageBox.Show("异常：VoiceLAB V2.1程序未打开");
                    return;
                }
                Console.WriteLine("请确保VoiceLAB V2.1程序的配置OK");
                string str_error_log = "";
                if (VoiceLAB.StartDataFromMeter(ref str_error_log))
                {
                    richTextBox1.Text += $"开始测量完成";
                }
                else
                {
                    richTextBox1.Text = $"开始测量失败[{str_error_log}]";
                }
            }
            finally
            {
                BtnStart.Enabled = true;
            }
        }

        private void BtnStopAndRead_Click(object sender, EventArgs e)
        {
            try
            {
                BtnStopAndRead.Enabled = false;
                richTextBox1.Clear();
                if (VoiceLAB.IsOpenVoiceLAB())
                {
                    Console.WriteLine("VoiceLAB V2.1程序已打开");
                }
                else
                {
                    MessageBox.Show("异常：VoiceLAB V2.1程序未打开");
                    return;
                }
                Console.WriteLine("请确保VoiceLAB V2.1程序的配置OK");
                string str_error_log = "";
                if (VoiceLAB.StopAndReadDataFromMeter(out VoiceLABResult voiceLABResult, ref str_error_log))
                {
                    richTextBox1.Text += voiceLABResult.ToString();
                }
                else
                {
                    Console.WriteLine($"读取蜂鸣器分贝值失败[{str_error_log}]");
                    richTextBox1.Text = $"读取蜂鸣器分贝值失败[{str_error_log}]";
                }
            }
            finally
            {
                BtnStopAndRead.Enabled = true;
            }
        }
    }
}
