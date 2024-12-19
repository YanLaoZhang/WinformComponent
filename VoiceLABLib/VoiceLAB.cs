using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceLABLib
{
    public class VoiceLABResult
    {
        public float Records;
        public float AvgValue;
        public float MaxValue;
        public float MinValue;

        public override string ToString()
        {
            return $"VoiceLAB Result: Records[{Records}]-AVG[{AvgValue}]-MAX[{MaxValue}]-MIN[{MinValue}]";
        }
    }

    public class VoiceLAB
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("user32.dll")]
        private static extern int MessageBoxTimeoutA(IntPtr hWnd, string msg, string caption, int type, int id, int time);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, StringBuilder lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam); 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public const uint BM_CLICK = 0x00F5; // 按钮点击的消息

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static bool IsOpenVoiceLAB()
        {
            IntPtr hWnd = FindWindow(null, "VoiceLAB V2.1");
            Trace.WriteLine("窗口句柄：" + hWnd.ToString("X"));
            if (hWnd == IntPtr.Zero)
            {
                Trace.WriteLine("数据采集软件未打开VoiceLAB V2.1");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 从VoiceLAB软件上采集分贝数据
        /// </summary>
        /// <param name="minSpec"></param>
        /// <param name="maxSpec"></param>
        /// <param name="maxValue"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static bool GetDataFromMeter(out VoiceLABResult voiceLABResult, int duration, ref string errorInfo)
        {
            IntPtr hWnd = FindWindow(null, "VoiceLAB V2.1");
            Trace.WriteLine("窗口句柄：" + hWnd.ToString("X"));
            if (hWnd == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未打开";
                voiceLABResult = null;
                return false;
            }
            IntPtr hUnknown = FindWindowEx(hWnd, IntPtr.Zero, null, "");
            Trace.WriteLine("一级子句柄：" + hUnknown.ToString("X"));
            IntPtr hMeasure = FindWindowEx(hUnknown, IntPtr.Zero, null, "Real Time Measure");
            Trace.WriteLine("测量页句柄：" + hMeasure.ToString("X"));
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                voiceLABResult = null;
                return false;
            }

            StringBuilder sBuilder = new StringBuilder(50);
            // 检查是否已开始
            IntPtr hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            //Trace.WriteLine("停止按钮句柄：" + hStopBtn.ToString("X"));
            if (hStopBtn == IntPtr.Zero)
            {
                Trace.WriteLine("数据采集软件未开启实时测试");
            }
            else
            {
                Trace.WriteLine("数据采集软件已开启实时测试，先关闭");
                Trace.WriteLine($"点击关闭按钮");
                SendMessage(hStopBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }

            // 点击清除按钮
            IntPtr hClearBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Clear   ");
            //Trace.WriteLine("清除按钮句柄：" + hClearBtn.ToString("X"));
            Trace.WriteLine($"点击清除按钮");
            SendMessage(hClearBtn, BM_CLICK, 50, sBuilder);
            Thread.Sleep(100);

            IntPtr hStartBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Start to Measure  ");
            //Trace.WriteLine("开始按钮句柄：" + hStartBtn.ToString("X"));
            if (hStartBtn == IntPtr.Zero)
            {
            }
            else
            {
                Trace.WriteLine($"点击开始按钮");
                SendMessage(hStartBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }
            hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                voiceLABResult = null;
                return false;
            }

            Thread.Sleep(duration * 1000);

            hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            //Trace.WriteLine("停止按钮句柄：" + hStopBtn.ToString("X"));
            if (hStopBtn != IntPtr.Zero)
            {
                Trace.WriteLine($"点击停止按钮");
                SendMessage(hStopBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }

            if (!GetVoiceLABResult(out voiceLABResult, ref errorInfo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 启动测量
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static bool StartDataFromMeter(ref string errorInfo)
        {
            IntPtr hWnd = FindWindow(null, "VoiceLAB V2.1");
            Trace.WriteLine("窗口句柄：" + hWnd.ToString("X"));
            if (hWnd == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未打开";
                return false;
            }
            IntPtr hUnknown = FindWindowEx(hWnd, IntPtr.Zero, null, "");
            Trace.WriteLine("一级子句柄：" + hUnknown.ToString("X"));
            IntPtr hMeasure = FindWindowEx(hUnknown, IntPtr.Zero, null, "Real Time Measure");
            Trace.WriteLine("测量页句柄：" + hMeasure.ToString("X"));
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                return false;
            }
            StringBuilder sBuilder = new StringBuilder(50);

            // 检查是否已开始
            IntPtr hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            //Trace.WriteLine("停止按钮句柄：" + hStopBtn.ToString("X"));
            if (hStopBtn == IntPtr.Zero)
            {
                Trace.WriteLine("数据采集软件未开启实时测试");
            }
            else
            {
                Trace.WriteLine("数据采集软件已开启实时测试，先关闭");
                Trace.WriteLine($"点击关闭按钮");
                SendMessage(hStopBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }

            // 点击清除按钮
            IntPtr hClearBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Clear   ");
            //Trace.WriteLine("清除按钮句柄：" + hClearBtn.ToString("X"));
            Trace.WriteLine($"点击清除按钮");
            SendMessage(hClearBtn, BM_CLICK, 50, sBuilder);

            IntPtr hStartBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Start to Measure  ");
            //Trace.WriteLine("开始按钮句柄：" + hStartBtn.ToString("X"));
            if (hStartBtn == IntPtr.Zero)
            {
            }
            else
            {
                Trace.WriteLine($"点击开始按钮");
                SendMessage(hStartBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }
            hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 停止测量并读取数据
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="duration"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static bool StopAndReadDataFromMeter(out VoiceLABResult voiceLABResult, ref string errorInfo)
        {
            IntPtr hWnd = FindWindow(null, "VoiceLAB V2.1");
            Trace.WriteLine("窗口句柄：" + hWnd.ToString("X"));
            if (hWnd == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未打开";
                voiceLABResult = null;
                return false;
            }
            IntPtr hUnknown = FindWindowEx(hWnd, IntPtr.Zero, null, "");
            Trace.WriteLine("一级子句柄：" + hUnknown.ToString("X"));
            IntPtr hMeasure = FindWindowEx(hUnknown, IntPtr.Zero, null, "Real Time Measure");
            Trace.WriteLine("测量页句柄：" + hMeasure.ToString("X"));
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                voiceLABResult = null;
                return false;
            }
            StringBuilder sBuilder = new StringBuilder(50);

            IntPtr hStartBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Start to Measure  ");
            //Trace.WriteLine("开始按钮句柄：" + hStartBtn.ToString("X"));
            
            IntPtr hStopBtn = FindWindowEx(hMeasure, IntPtr.Zero, null, "Stop to Measure  ");
            //Trace.WriteLine("停止按钮句柄：" + hStopBtn.ToString("X"));
            if (hStopBtn == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                voiceLABResult = null;
                return false;
            }
            else
            {
                Trace.WriteLine("数据采集软件已开启实时测试,停止");
                Trace.WriteLine($"点击停止按钮");
                SendMessage(hStopBtn, BM_CLICK, 50, sBuilder);
                Thread.Sleep(100);
            }
            if (!GetVoiceLABResult(out voiceLABResult, ref errorInfo))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 遍历窗口的所有子控件并打印其层次关系
        /// </summary>
        /// <param name="parentHandle">父窗口句柄</param>
        /// <param name="level">当前控件的层级</param>
        static void PrintWindowHierarchy(IntPtr parentHandle, int level)
        {
            EnumChildWindows(parentHandle, (hWnd, lParam) =>
            {
                // 1. 获取控件标题
                StringBuilder windowText = new StringBuilder(256);
                GetWindowText(hWnd, windowText, windowText.Capacity);

                // 2. 获取控件类名
                StringBuilder className = new StringBuilder(256);
                GetClassName(hWnd, className, className.Capacity);

                // 3. 打印控件的层次关系
                string indent = new string(' ', level * 2);
                Console.WriteLine($"{indent}句柄: 0x{hWnd.ToString("X")}, 类名: {className}, 标题: \"{windowText}\"");

                // 4. 递归枚举当前窗口的子控件
                PrintWindowHierarchy(hWnd, level + 1);
                return true; // 继续枚举
            }, IntPtr.Zero);
        }

        /// <summary>
        /// 读取界面的测量数据
        /// </summary>
        /// <param name="voiceLABResult"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static bool GetVoiceLABResult(out VoiceLABResult voiceLABResult, ref string errorInfo)
        {
            IntPtr hWnd = FindWindow(null, "VoiceLAB V2.1");
            Trace.WriteLine("窗口句柄：" + hWnd.ToString("X"));
            if (hWnd == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未打开";
                voiceLABResult = null;
                return false;
            }
            IntPtr hUnknown = FindWindowEx(hWnd, IntPtr.Zero, null, "");
            Trace.WriteLine("一级子句柄：" + hUnknown.ToString("X"));
            IntPtr hMeasure = FindWindowEx(hUnknown, IntPtr.Zero, null, "Real Time Measure");
            Trace.WriteLine("测量页句柄：" + hMeasure.ToString("X"));
            if (hMeasure == IntPtr.Zero)
            {
                errorInfo = "数据采集软件未开启实时测试";
                voiceLABResult = null;
                return false;
            }

            StringBuilder sBuilder = new StringBuilder(50);
            voiceLABResult = new VoiceLABResult();
            
            IntPtr hMinLabel = FindWindowEx(hMeasure, IntPtr.Zero, null, "MIN");
            //Trace.WriteLine("MIN句柄：" + hMinLabel.ToString("X"));
            IntPtr hMaxLabel = FindWindowEx(hMeasure, IntPtr.Zero, null, "MAX");
            //Trace.WriteLine("MAX句柄：" + hMaxLabel.ToString("X"));
            IntPtr hAvgLabel = FindWindowEx(hMeasure, IntPtr.Zero, null, "AVG");
            //Trace.WriteLine("AVG句柄：" + hAvgLabel.ToString("X"));
            IntPtr hRecordsLabel = FindWindowEx(hMeasure, IntPtr.Zero, null, "Records");
            //Trace.WriteLine("Records句柄：" + hRecordsLabel.ToString("X"));

            IntPtr hMaxValue = FindWindowEx(hMeasure, hMinLabel, null, "");
            //Trace.WriteLine("Max值句柄：" + hMaxValue.ToString("X"));
            sBuilder.Clear();
            SendMessage(hMaxValue, 0x000D, 50, sBuilder);
            Trace.WriteLine("Max值：" + sBuilder);
            voiceLABResult.MaxValue = float.TryParse(sBuilder.ToString(), out float tmp) ? tmp : 0;
            Trace.WriteLine($"响度的Max值为[{voiceLABResult.MaxValue}]");

            IntPtr hMinValue = FindWindowEx(hMeasure, hAvgLabel, null, "");
            //Trace.WriteLine("Min值句柄：" + hMinValue.ToString("X"));
            sBuilder.Clear();
            SendMessage(hMinValue, 0x000D, 50, sBuilder);
            Trace.WriteLine("Min值：" + sBuilder);
            voiceLABResult.MinValue = float.TryParse(sBuilder.ToString(), out float tmp1) ? tmp1 : 0;
            Trace.WriteLine($"响度的Min值为[{voiceLABResult.MinValue}]");

            IntPtr hAvgValue = FindWindowEx(hMeasure, IntPtr.Zero, null, "");
            //Trace.WriteLine("Avg值句柄：" + hAvgValue.ToString("X"));
            sBuilder.Clear();
            SendMessage(hAvgValue, 0x000D, 50, sBuilder);
            Trace.WriteLine("Avg值：" + sBuilder);
            voiceLABResult.AvgValue = float.TryParse(sBuilder.ToString(), out float tmp2) ? tmp2 : 0;
            Trace.WriteLine($"响度的Avg值为[{voiceLABResult.AvgValue}]");

            IntPtr hRecordsValue = FindWindowEx(hMeasure, hMaxLabel, null, "");
            //Trace.WriteLine("Records值句柄：" + hRecordsValue.ToString("X"));
            sBuilder.Clear();
            SendMessage(hRecordsValue, 0x000D, 50, sBuilder);
            Trace.WriteLine("Records值：" + sBuilder);
            voiceLABResult.Records = float.TryParse(sBuilder.ToString(), out float tmp3) ? tmp3 : 0;
            Trace.WriteLine($"响度的Records值为[{voiceLABResult.Records}]");
            return true;
        }
    }

}
