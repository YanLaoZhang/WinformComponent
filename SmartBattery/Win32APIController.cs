using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SmartBattery
{
    public class Win32APIController
    {
        // Win32 API声明
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
            string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("user32.dll")]
        static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(
            IntPtr hWnd,         // 目标窗口句柄
            StringBuilder lpClassName, // 接收类名的缓冲区
            int nMaxCount        // 缓冲区最大字符数
        );

        // 常量定义
        private const uint BM_CLICK = 0x00F5;

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        const uint GW_CHILD = 5;
        const uint GW_HWNDNEXT = 2;

        /// <summary>
        /// 查找指定进程的主窗口句柄
        /// </summary>
        public static IntPtr FindMainWindowByProcessName(string processName)
        {
            IntPtr result = IntPtr.Zero;
            uint targetPid = 0;

            // 先获取进程ID
            foreach (var process in Process.GetProcessesByName(processName))
            {
                targetPid = (uint)process.Id;
                break;
            }

            if (targetPid == 0)
                throw new ArgumentException("Process not found");

            // 枚举窗口
            EnumWindows(delegate (IntPtr hWnd, IntPtr param)
            {
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);

                if (pid == targetPid)
                {
                    result = hWnd;
                    return false; // 找到后停止枚举
                }
                return true;
            }, IntPtr.Zero);

            return result;
        }

        /// <summary>
        /// 查找并点击按钮
        /// </summary>
        public static void ClickButton(IntPtr mainWindow, string buttonText)
        {
            // 查找按钮控件
            IntPtr button1 = FindWindowEx(mainWindow, IntPtr.Zero, "Button", null);
            IntPtr button = FindWindowEx(mainWindow, IntPtr.Zero, "Button", buttonText);
            if (button == IntPtr.Zero)
                throw new Win32Exception("Button not found");

            // 发送点击消息
            if (SendMessage(button, BM_CLICK, IntPtr.Zero, IntPtr.Zero) == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void GetAllControl(IntPtr parentHwnd)
        {
            // 需先获取目标进程权限
            SetForegroundWindow(parentHwnd);  // 激活窗口
            //AttachThreadInput(currentThreadId, targetThreadId, true);

            // 示例：遍历所有同级控件
            IntPtr child = GetWindow(parentHwnd, GW_CHILD);
            while (child != IntPtr.Zero)
            {
                // 处理控件...
                child = GetWindow(child, GW_HWNDNEXT);
            }

            /*IntPtr child = IntPtr.Zero;
            while ((child = FindWindowEx(parentHwnd, child, "WindowsForms10.Window.8.app.0.141b42a_r7_ad1", null)) != IntPtr.Zero)
            {
                GetWindowRect(child, out RECT rect);
                Console.WriteLine($"句柄:0x{child:X8} 位置:[{rect.Left},{rect.Top}-{rect.Right},{rect.Bottom}]");
            }*/
        }

        public static string GetControlClassName(IntPtr hWnd)
        {
            StringBuilder className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);
            return className.ToString();
        }

        public static void GetAllButton(IntPtr parentHwnd)
        {
            // 示例：查找所有按钮
            List<IntPtr> buttons = new List<IntPtr>();
            EnumChildWindows(parentHwnd, (hWnd, param) => {
                if (GetControlClassName(hWnd) == "Button")
                {
                    buttons.Add(hWnd);
                }
                return true;
            }, IntPtr.Zero);
        }
    }
}
