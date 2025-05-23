﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace SmartBattery
{
    public class SmartToolControlFlaUI
    {
        public FlaUI.Core.Application app;
        public System.IntPtr _mainForm;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ActivateWindow(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
        }

        public SmartToolControlFlaUI()
        {
        }

        public void ShutDown(ref string str_error_log)
        {
            try
            {
                if (app != null && !app.HasExited)
                {
                    using (var automation = new UIA3Automation())
                    {
                        var mainWindow = app.GetMainWindow(automation);

                        if (mainWindow != null)
                        {
                            Console.WriteLine("尝试关闭窗口...");
                            mainWindow.Close();

                            // 等待窗口关闭
                            Thread.Sleep(2000);

                            // 再次检查是否关闭
                            if (!app.HasExited)
                            {
                                Console.WriteLine("窗口仍然打开，尝试点击关闭按钮...");
                                var closeButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("关闭")));
                                closeButton?.AsButton()?.Invoke();

                                Thread.Sleep(2000);
                            }
                        }
                    }

                    // 如果窗口仍未关闭，则强制终止进程
                    if (!app.HasExited)
                    {
                        Console.WriteLine("窗口未正常关闭，强制结束进程...");
                        app.Close();  // 关闭进程
                    }

                    Console.WriteLine("应用已成功关闭");
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine($"Error Closing the Program: [{ex.Message}]");
            }
        }

        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="str_error_log"></param>
        /// <param name="isOnlyOne"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI StartUp(string exePath, ref string str_error_log)
        {
            try
            {
                string processName = Path.GetFileNameWithoutExtension(exePath);
                Trace.WriteLine($"ProcessName:[{processName}]");
                using (var automation = new UIA3Automation())
                {
                    // 检查是否已经运行
                    var existingProcess = Process.GetProcessesByName(processName).FirstOrDefault();

                    if (existingProcess != null)
                    {
                        Console.WriteLine("应用已在运行，附加到现有进程...");
                        app = FlaUI.Core.Application.Attach(existingProcess);
                    }
                    else
                    {
                        Console.WriteLine("应用未运行，启动新进程...");
                        app = FlaUI.Core.Application.Launch(exePath);
                    }

                    // 等待应用就绪
                    app.WaitWhileBusy();

                    _mainForm = app.MainWindowHandle;

                    var mainWindow = app.GetMainWindow(automation);
                    Console.WriteLine("主窗口标题: " + mainWindow.Title);
                    // 激活窗口
                    ActivateWindow(_mainForm);

                    // 关闭应用（如果需要）
                    // app.Close();
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine($"Error Starting the Program: [{ex.Message}]");
            }
            return this;
        }

        /// <summary>
        /// 点击ScanAll
        /// </summary>
        /// <param name="result"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI ScanAll(out bool result, out string str_error_log)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find buttonMenu_Registers.");
                    var buttonMenu_Registers = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("buttonMenu_Registers"))?.AsButton(), TimeSpan.FromSeconds(5)).Result;
                    if (buttonMenu_Registers == null) 
                    {
                        str_error_log = $"[{DateTime.Now}] buttonMenu_Registers not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] buttonMenu_Registers found successfully.");
                    Retry.WhileFalse(() => buttonMenu_Registers.IsEnabled, TimeSpan.FromSeconds(5));
                    buttonMenu_Registers?.Click();
                    // 查找按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find button_SetScan.");
                    var button_SetScan = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("button_SetScan"))?.AsButton(), TimeSpan.FromSeconds(5)).Result;
                    if (button_SetScan == null)
                    {
                        str_error_log = $"[{DateTime.Now}] button_SetScan not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] button_SetScan found successfully.");
                    Retry.WhileFalse(() => button_SetScan.IsEnabled, TimeSpan.FromSeconds(5));
                    button_SetScan?.Click();

                    str_error_log = $"[{DateTime.Now}] Click ScanAll successfully.";
                    Trace.WriteLine(str_error_log);
                    result = true;
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
        }

        /// <summary>
        /// 烧录AFI文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="result"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI SetAFIFile(string filePath, out bool result, out string str_error_log)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找 MenuBar
                    Trace.WriteLine($"[{DateTime.Now}] To Find MenuFile.");
                    var menuBar = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByName("File"))?.AsMenuItem(), TimeSpan.FromSeconds(5)).Result;
                    if (menuBar == null)
                    {
                        str_error_log = $"[{DateTime.Now}]MenuFile not Found";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}]MenuFile found successfully.");
                    Retry.WhileFalse(() => menuBar.IsEnabled, TimeSpan.FromSeconds(5));
                    menuBar.Click();

                    // 查找菜单项
                    Trace.WriteLine($"[{DateTime.Now}] To Find MenuOpenFile.");
                    var menuItem = Retry.WhileNull(() => menuBar.FindFirstDescendant(cf => cf.ByName("Open AFI File(*.afi)"))?.AsMenuItem(), TimeSpan.FromSeconds(5)).Result;
                    if (menuItem == null)
                    {
                        str_error_log = $"[{DateTime.Now}]MenuOpenFile not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}]MenuOpenFile found successfully.");
                    Retry.WhileFalse(() => menuItem.IsEnabled, TimeSpan.FromSeconds(5));
                    menuItem.Click();

                    // 等待并定位文件对话框
                    Trace.WriteLine($"[{DateTime.Now}] To Find FileDialog.");
                    var fileDialog = Retry.WhileNull(() => mainWindow.FindFirstDescendant(c => c.ByName("Select AFI file: Channel 0"))?.AsWindow(), TimeSpan.FromSeconds(5)).Result;
                    if (fileDialog == null)
                    {
                        str_error_log = $"[{DateTime.Now}] fileDialog not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] fileDialog found successfully.");

                    // 查找文件名编辑框
                    Trace.WriteLine($"[{DateTime.Now}] To Find FileNameEdit.");
                    var fileNameEdit = Retry.WhileNull(() => fileDialog.FindFirstDescendant(c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Edit).And(c.ByName("文件名(N):")))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                    if (fileNameEdit == null)
                    {
                        str_error_log = $"[{DateTime.Now}] fileNameEdit not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] FileNameEdit found successfully.");
                    // 清空输入框并输入指定的文件名称
                    Trace.WriteLine($"Enter filePath: {filePath}");
                    Retry.WhileFalse(() => fileNameEdit.IsEnabled, TimeSpan.FromSeconds(5));
                    fileNameEdit?.Patterns.Value.Pattern.SetValue(filePath);
                    //fileNameEdit?.Enter("");  // 先清空
                    //fileNameEdit?.Enter(filePath);

                    // 点击打开按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find OpenButton.");
                    var openButton = Retry.WhileNull(() => fileDialog.FindFirstDescendant(c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(c.ByName("打开(O)"))).AsButton(), TimeSpan.FromSeconds(5)).Result;
                    if (openButton == null)
                    {
                        str_error_log = $"[{DateTime.Now}] OpenButton not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] OpenButton found successfully.");
                    Retry.WhileFalse(() => openButton.IsEnabled, TimeSpan.FromSeconds(5));
                    openButton?.Click();

                    Trace.WriteLine($"[{DateTime.Now}] wait flashing...");
                    str_error_log = string.Empty;
                    result = false;
                    for (int i = 0; i < 45; i++)
                    {
                        GetDialogTip(mainWindow, out string content);
                        // Eone
                        if (content.Contains($"AFI Write Success"))
                        {
                            str_error_log = $"AFI Write Success.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            return this;
                        }
                        if (content.Contains($"AFI Write Fail"))
                        {
                            str_error_log = $"AFI Write Fail.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            return this;
                        }
                        // VD12D
                        if (content.Contains($"Write AFI Success"))
                        {
                            str_error_log = $"AFI Write Success.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            return this;
                        }
                        if (content.Contains($"Write AFI Fail"))
                        {
                            str_error_log = $"AFI Write Fail.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            return this;
                        }
                        Thread.Sleep(1000);
                        continue;
                    }

                    str_error_log = $"[{DateTime.Now}] Flash Result Timeout(45s).";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
        }

        /// <summary>
        /// 主板校准
        /// </summary>
        /// <param name="result"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI OffsetCalibrate(out bool result, out string str_error_log)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找buttonMenu_Calibrate按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find buttonMenu_Calibrate.");
                    var buttonMenu_Calibrate = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("buttonMenu_Calibrate"))?.AsButton();
                    if (buttonMenu_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] buttonMenu_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] buttonMenu_Calibrate found successfully.");
                    Retry.WhileFalse(() => buttonMenu_Calibrate.IsEnabled, TimeSpan.FromSeconds(5));
                    buttonMenu_Calibrate?.Click();
                    // 查找Form_Calibrate
                    Trace.WriteLine($"[{DateTime.Now}] To Find Form_Calibrate.");
                    var Form_Calibrate = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Form_Calibrate")), TimeSpan.FromSeconds(5)).Result;
                    if (Form_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] Form_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] Form_Calibrate found successfully.");
                    // 查找groupBox_BoardOffsetCalibration
                    Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_BoardOffsetCalibration.");
                    var groupBox_BoardOffsetCalibration = Retry.WhileNull(() => Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_BoardOffsetCalibration")), TimeSpan.FromSeconds(5)).Result;
                    if (groupBox_BoardOffsetCalibration == null)
                    {
                        str_error_log = $"[{DateTime.Now}] groupBox_BoardOffsetCalibration not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] groupBox_BoardOffsetCalibration found successfully.");
                    // 查找button_OffsetCalibration按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find button_OffsetCalibration.");
                    var button_OffsetCalibration = groupBox_BoardOffsetCalibration.FindFirstDescendant(cf => cf.ByAutomationId("button_OffsetCalibration"))?.AsButton();
                    if (button_OffsetCalibration == null)
                    {
                        str_error_log = $"[{DateTime.Now}] button_OffsetCalibration not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] button_OffsetCalibration found successfully.");
                    Retry.WhileFalse(() => button_OffsetCalibration.IsEnabled, TimeSpan.FromSeconds(5));
                    button_OffsetCalibration?.Click();
                    Thread.Sleep(300);

                    str_error_log = string.Empty;
                    result = false;
                    for (int i = 0; i < 30; i++)
                    {
                        GetDialogTip(mainWindow, out string content);
                        if (content.Contains($"Calibrate Success"))
                        {
                            str_error_log = $"Board Offset Calibrate Success.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            return this;
                        }
                        if (content.Contains($"Calibrate Fail"))
                        {
                            str_error_log = $"Board Offset Calibrate Fail.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            return this;
                        }
                        Thread.Sleep(1000);
                        continue;
                    }

                    str_error_log = $"[{DateTime.Now}] Board Offset Calibrate Result Timeout(30s).";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
        }

        /// <summary>
        /// Eone电压校准
        /// </summary>
        /// <param name="result"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI VoltageCalibrateEone(string act_vol, out bool result, out string mes_vol, out string str_error_log, double _diff_allow=3)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false; 
                    mes_vol = string.Empty;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找buttonMenu_Calibrate按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find buttonMenu_Calibrate.");
                    var buttonMenu_Calibrate = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("buttonMenu_Calibrate"))?.AsButton();
                    if (buttonMenu_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] buttonMenu_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] buttonMenu_Calibrate found successfully.");
                    Retry.WhileFalse(() => buttonMenu_Calibrate.IsEnabled, TimeSpan.FromSeconds(5));
                    buttonMenu_Calibrate?.Click();
                    // 查找Form_Calibrate
                    Trace.WriteLine($"[{DateTime.Now}] To Find Form_Calibrate.");
                    var Form_Calibrate = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Form_Calibrate")), TimeSpan.FromSeconds(5)).Result;
                    if (Form_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] Form_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] Form_Calibrate found successfully.");
                    // 查找groupBox_VoltageCalibration
                    Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_VoltageCalibration.");
                    var groupBox_VoltageCalibration = Retry.WhileNull(() => Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_VoltageCalibration")), TimeSpan.FromSeconds(5)).Result;
                    if (groupBox_VoltageCalibration == null)
                    {
                        str_error_log = $"[{DateTime.Now}] groupBox_VoltageCalibration not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] groupBox_VoltageCalibration found successfully.");

                    // 查找checkBox_Voltage
                    Trace.WriteLine($"[{DateTime.Now}] To Find checkBox_Voltage.");
                    var checkBox_Voltage = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(cf => cf.ByAutomationId("checkBox_Voltage"))?.AsCheckBox(), TimeSpan.FromSeconds(5)).Result;
                    if (checkBox_Voltage == null)
                    {
                        str_error_log = $"[{DateTime.Now}] checkBox_Voltage not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol = string.Empty;
                        return this;
                    }
                    // 勾选复选框checkBox_Voltage
                    Retry.WhileFalse(() => checkBox_Voltage.IsEnabled, TimeSpan.FromSeconds(5));
                    checkBox_Voltage.IsChecked = true;

                    bool isCalibrate = false;
                    mes_vol = string.Empty;
                    for (int i = 0; i < 3; i++)
                    {
                        // 查找textBox_VoltageAct
                        Trace.WriteLine($"[{DateTime.Now}] To Find textBox_VoltageAct.");
                        var textBox_VoltageAct = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_VoltageAct"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_VoltageAct == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_VoltageAct not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageAct found successfully.");
                        // 清空输入框并输入指定值
                        Trace.WriteLine($"Enter content: {act_vol}");
                        Retry.WhileFalse(() => textBox_VoltageAct.IsEnabled, TimeSpan.FromSeconds(5));
                        textBox_VoltageAct?.Patterns.Value.Pattern.SetValue(act_vol);

                        // 查找button_DoCalibration按钮
                        Trace.WriteLine($"[{DateTime.Now}] To Find button_DoCalibration.");
                        var button_DoCalibration = Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("button_DoCalibration"))?.AsButton();
                        if (button_DoCalibration == null)
                        {
                            str_error_log = $"[{DateTime.Now}] button_DoCalibration not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] button_DoCalibration found successfully.");
                        Retry.WhileFalse(() => button_DoCalibration.IsEnabled, TimeSpan.FromSeconds(5));
                        button_DoCalibration?.Click();
                        Thread.Sleep(300);

                        str_error_log = string.Empty;
                        result = false;
                        for (int j = 0; j < 30; j++)
                        {
                            GetDialogTip(mainWindow, out string content);
                            if (content.Contains($"Calibrate Success"))
                            {
                                str_error_log = $"Voltage  Calibrate Success.";
                                Trace.WriteLine(str_error_log);
                                result = true;
                                break;
                            }
                            if (content.Contains($"Calibrate Fail"))
                            {
                                str_error_log = $"Voltage Calibrate Fail.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            if (content.Contains($"Err Input"))
                            {
                                str_error_log = $"Voltage Calibration Err Input.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (!result) 
                        {
                            str_error_log = $"[{i+1}/3]Voltage Calibrate Fail. Try Again.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            mes_vol = string.Empty;
                            continue;
                        }

                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] Voltage Calibrate Success. Check Difference");

                        // 查找textBox_VoltageMes
                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] To Find textBox_VoltageMes.");
                        var textBox_VoltageMes = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_VoltageMes"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_VoltageMes == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_VoltageMes not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageMes found successfully.");
                        // 获取内容
                        string str_mes_vol = textBox_VoltageMes.Text;
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageMes text: {str_mes_vol}.");
                        try
                        {
                            mes_vol = double.Parse(str_mes_vol).ToString();
                        }
                        catch (Exception ee)
                        {
                            Trace.WriteLine($"GetEditText({str_mes_vol}) Error:['{ee.Message}'], set '0'.");
                            mes_vol = "0";
                        }
                        double diff_vol = Math.Abs(double.Parse(act_vol) - double.Parse(mes_vol));
                        Trace.WriteLine($"Actual Voltage and Measured Voltage Difference: [{diff_vol}]");
                        if (diff_vol > _diff_allow)
                        {
                            str_error_log = $"[{i+1}/3]Voltage Calibrate Difference Fail.";
                            Trace.WriteLine(str_error_log);
                            continue;
                        }
                        else
                        {
                            isCalibrate = true;
                            Trace.WriteLine($"Voltage Calibrate Difference Success.");
                            break;
                        }
                    }

                    // 取消勾选
                    if (checkBox_Voltage != null)
                    {
                        checkBox_Voltage.IsChecked = false;
                    }
                    if (isCalibrate)
                    {
                        str_error_log = $"Voltage Calibrate Final Success.";
                        Trace.WriteLine(str_error_log);
                        result = true;
                    }
                    else
                    {
                        str_error_log = $"Voltage Calibrate Final Fail.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                    }
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                mes_vol = string.Empty;
                return this;
            }
        }

        /// <summary>
        /// VD12D电压校准
        /// </summary>
        /// <param name="act_vol_cell"></param>
        /// <param name="act_vol_bat"></param>
        /// <param name="act_vol_pack"></param>
        /// <param name="result"></param>
        /// <param name="mes_vol_cell"></param>
        /// <param name="mes_vol_bat"></param>
        /// <param name="mes_vol_pack"></param>
        /// <param name="str_error_log"></param>
        /// <param name="_diff_allow"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI VoltageCalibrateVD12D(string act_vol_cell, string act_vol_bat, string act_vol_pack, out bool result, out string mes_vol_cell, out string mes_vol_bat, out string mes_vol_pack, out string str_error_log, double _diff_allow = 3)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    mes_vol_bat = string.Empty;
                    mes_vol_cell = string.Empty;
                    mes_vol_pack = string.Empty;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找buttonMenu_Calibrate按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find buttonMenu_Calibrate.");
                    var buttonMenu_Calibrate = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("buttonMenu_Calibrate"))?.AsButton();
                    if (buttonMenu_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] buttonMenu_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol_bat = string.Empty;
                        mes_vol_cell = string.Empty;
                        mes_vol_pack = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] buttonMenu_Calibrate found successfully.");
                    Retry.WhileFalse(() => buttonMenu_Calibrate.IsEnabled, TimeSpan.FromSeconds(5));
                    buttonMenu_Calibrate?.Click();
                    // 查找Form_Calibrate
                    Trace.WriteLine($"[{DateTime.Now}] To Find Form_Calibrate.");
                    var Form_Calibrate = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Form_Calibrate")), TimeSpan.FromSeconds(5)).Result;
                    if (Form_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] Form_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol_bat = string.Empty;
                        mes_vol_cell = string.Empty;
                        mes_vol_pack = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] Form_Calibrate found successfully.");
                    // 查找groupBox_VoltageCalibration
                    Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_VoltageCalibration.");
                    var groupBox_VoltageCalibration = Retry.WhileNull(() => Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_VoltageCalibration")), TimeSpan.FromSeconds(5)).Result;
                    if (groupBox_VoltageCalibration == null)
                    {
                        str_error_log = $"[{DateTime.Now}] groupBox_VoltageCalibration not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol_bat = string.Empty;
                        mes_vol_cell = string.Empty;
                        mes_vol_pack = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] groupBox_VoltageCalibration found successfully.");

                    // 查找checkBox_Voltage
                    Trace.WriteLine($"[{DateTime.Now}] To Find checkBox_Voltage.");
                    var checkBox_Voltage = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(cf => cf.ByAutomationId("checkBox_Voltage"))?.AsCheckBox(), TimeSpan.FromSeconds(5)).Result;
                    if (checkBox_Voltage == null)
                    {
                        str_error_log = $"[{DateTime.Now}] checkBox_Voltage not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_vol_bat = string.Empty;
                        mes_vol_cell = string.Empty;
                        mes_vol_pack = string.Empty;
                        return this;
                    }
                    // 勾选复选框checkBox_Voltage
                    Retry.WhileFalse(() => checkBox_Voltage.IsEnabled, TimeSpan.FromSeconds(5));
                    checkBox_Voltage.IsChecked = true;

                    bool isCalibrate = false; 
                    mes_vol_bat = string.Empty;
                    mes_vol_cell = string.Empty;
                    mes_vol_pack = string.Empty;
                    for (int i = 0; i < 3; i++)
                    {
                        // 查找textBox_VoltageAct
                        Trace.WriteLine($"[{DateTime.Now}] To Find textBox_VoltageAct.");
                        var textBox_VoltageAct = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_VoltageAct"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_VoltageAct == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_VoltageAct not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageAct found successfully.");
                        // 清空输入框并输入指定值
                        Trace.WriteLine($"Enter content: {act_vol_cell}");
                        Retry.WhileFalse(() => textBox_VoltageAct.IsEnabled, TimeSpan.FromSeconds(5));
                        textBox_VoltageAct?.Patterns.Value.Pattern.SetValue(act_vol_cell);

                        // 查找textBox_BatteryVoltageAct
                        Trace.WriteLine($"[{DateTime.Now}] To Find textBox_BatteryVoltageAct.");
                        var textBox_BatteryVoltageAct = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_BatteryVoltageAct"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_BatteryVoltageAct == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_BatteryVoltageAct not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_BatteryVoltageAct found successfully.");
                        // 清空输入框并输入指定值
                        Trace.WriteLine($"Enter content: {act_vol_bat}");
                        Retry.WhileFalse(() => textBox_BatteryVoltageAct.IsEnabled, TimeSpan.FromSeconds(5));
                        textBox_BatteryVoltageAct?.Patterns.Value.Pattern.SetValue(act_vol_bat);

                        // 查找textBox_PackVoltageAct
                        Trace.WriteLine($"[{DateTime.Now}] To Find textBox_PackVoltageAct.");
                        var textBox_PackVoltageAct = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_PackVoltageAct"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_PackVoltageAct == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_PackVoltageAct not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_PackVoltageAct found successfully.");
                        // 清空输入框并输入指定值
                        Trace.WriteLine($"Enter content: {act_vol_pack}");
                        Retry.WhileFalse(() => textBox_PackVoltageAct.IsEnabled, TimeSpan.FromSeconds(5));
                        textBox_PackVoltageAct?.Patterns.Value.Pattern.SetValue(act_vol_pack);

                        // 查找button_DoCalibration按钮
                        Trace.WriteLine($"[{DateTime.Now}] To Find button_DoCalibration.");
                        var button_DoCalibration = Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("button_DoCalibration"))?.AsButton();
                        if (button_DoCalibration == null)
                        {
                            str_error_log = $"[{DateTime.Now}] button_DoCalibration not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] button_DoCalibration found successfully.");
                        Retry.WhileFalse(() => button_DoCalibration.IsEnabled, TimeSpan.FromSeconds(5));
                        button_DoCalibration?.Click();
                        Thread.Sleep(300);

                        str_error_log = string.Empty;
                        result = false;
                        for (int j = 0; j < 30; j++)
                        {
                            GetDialogTip(mainWindow, out string content);
                            if (content.Contains($"Calibrate Success"))
                            {
                                str_error_log = $"Voltage  Calibrate Success.";
                                Trace.WriteLine(str_error_log);
                                result = true;
                                break;
                            }
                            if (content.Contains($"Calibrate Fail"))
                            {
                                str_error_log = $"Voltage Calibrate Fail.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            if (content.Contains($"Err Input"))
                            {
                                str_error_log = $"Voltage Calibration Err Input.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (!result)
                        {
                            str_error_log = $"[{i + 1}/3]Voltage Calibrate Fail. Try Again.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            continue;
                        }

                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] Voltage Calibrate Success. Check Difference");

                        // 查找textBox_VoltageMes
                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] To Find textBox_VoltageMes.");
                        var textBox_VoltageMes = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_VoltageMes"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_VoltageMes == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_VoltageMes not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageMes found successfully.");

                        // 查找textBox_BatteryVoltageMes
                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] To Find textBox_BatteryVoltageMes.");
                        var textBox_BatteryVoltageMes = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_BatteryVoltageMes"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_BatteryVoltageMes == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_BatteryVoltageMes not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_BatteryVoltageMes found successfully.");

                        // 查找textBox_PackVoltageMes
                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] To Find textBox_PackVoltageMes.");
                        var textBox_PackVoltageMes = Retry.WhileNull(() => groupBox_VoltageCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_PackVoltageMes"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_PackVoltageMes == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_PackVoltageMes not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_PackVoltageMes found successfully.");

                        // 获取textBox_VoltageMes内容
                        string str_mes_vol_cell = textBox_VoltageMes.Text;
                        Trace.WriteLine($"[{DateTime.Now}] textBox_VoltageMes text: {str_mes_vol_cell}.");
                        try
                        {
                            mes_vol_cell = double.Parse(str_mes_vol_cell).ToString();
                        }
                        catch (Exception ee)
                        {
                            Trace.WriteLine($"GetEditText({str_mes_vol_cell}) Error:['{ee.Message}'], set '0'.");
                            mes_vol_cell = "0";
                        }
                        double diff_vol_cell = Math.Abs(double.Parse(act_vol_cell) - double.Parse(mes_vol_cell));
                        Trace.WriteLine($"CELL Actual Voltage and Measured Voltage Difference: [{diff_vol_cell}]");
                        // 获取textBox_BatteryVoltageMes内容
                        string str_mes_vol_bat = textBox_BatteryVoltageMes.Text;
                        Trace.WriteLine($"[{DateTime.Now}] textBox_BatteryVoltageMes text: {str_mes_vol_bat}.");
                        try
                        {
                            mes_vol_bat = double.Parse(str_mes_vol_bat).ToString();
                        }
                        catch (Exception ee)
                        {
                            Trace.WriteLine($"GetEditText({str_mes_vol_bat}) Error:['{ee.Message}'], set '0'.");
                            mes_vol_bat = "0";
                        }
                        double diff_vol_bat = Math.Abs(double.Parse(act_vol_bat) - double.Parse(mes_vol_bat));
                        Trace.WriteLine($"BAT Actual Voltage and Measured Voltage Difference: [{diff_vol_bat}]");
                        // 获取textBox_PackVoltageMes内容
                        string str_mes_vol_pack = textBox_PackVoltageMes.Text;
                        Trace.WriteLine($"[{DateTime.Now}] textBox_PackVoltageMes text: {str_mes_vol_pack}.");
                        try
                        {
                            mes_vol_pack = double.Parse(str_mes_vol_pack).ToString();
                        }
                        catch (Exception ee)
                        {
                            Trace.WriteLine($"GetEditText({str_mes_vol_pack}) Error:['{ee.Message}'], set '0'.");
                            mes_vol_pack = "0";
                        }
                        double diff_vol_pack = Math.Abs(double.Parse(act_vol_pack) - double.Parse(mes_vol_pack));
                        Trace.WriteLine($"PACK Actual Voltage and Measured Voltage Difference: [{diff_vol_pack}]");

                        if (diff_vol_cell > _diff_allow || diff_vol_bat > _diff_allow || diff_vol_pack > _diff_allow)
                        {
                            str_error_log = $"[{i + 1}/3]Voltage Calibrate Difference Fail.";
                            Trace.WriteLine(str_error_log); 
                            mes_vol_bat = string.Empty;
                            mes_vol_cell = string.Empty;
                            mes_vol_pack = string.Empty;
                            continue;
                        }
                        else
                        {
                            isCalibrate = true;
                            Trace.WriteLine($"[{i + 1}/3]Voltage Calibrate Difference Success.");
                            break;
                        }
                    }

                    // 取消勾选
                    if (checkBox_Voltage != null)
                    {
                        checkBox_Voltage.IsChecked = false;
                    }
                    if (isCalibrate)
                    {
                        str_error_log = $"Voltage Calibrate Final Success.";
                        Trace.WriteLine(str_error_log);
                        result = true;
                    }
                    else
                    {
                        str_error_log = $"Voltage Calibrate Final Fail.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                    }
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                mes_vol_bat = string.Empty;
                mes_vol_cell = string.Empty;
                mes_vol_pack = string.Empty;
                return this;
            }
        }

        /// <summary>
        /// 电流校准
        /// </summary>
        /// <param name="act_cur"></param>
        /// <param name="result"></param>
        /// <param name="mes_cur"></param>
        /// <param name="str_error_log"></param>
        /// <param name="_diff_allow"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI CurrentCalibrate(string act_cur, out bool result, out string mes_cur, out string str_error_log, double _diff_allow = 3)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    mes_cur = string.Empty;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);
                    // 查找buttonMenu_Calibrate按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find buttonMenu_Calibrate.");
                    var buttonMenu_Calibrate = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("buttonMenu_Calibrate"))?.AsButton();
                    if (buttonMenu_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] buttonMenu_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_cur = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] buttonMenu_Calibrate found successfully.");
                    Retry.WhileFalse(() => buttonMenu_Calibrate.IsEnabled, TimeSpan.FromSeconds(5));
                    buttonMenu_Calibrate?.Click();
                    // 查找Form_Calibrate
                    Trace.WriteLine($"[{DateTime.Now}] To Find Form_Calibrate.");
                    var Form_Calibrate = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Form_Calibrate")), TimeSpan.FromSeconds(5)).Result;
                    if (Form_Calibrate == null)
                    {
                        str_error_log = $"[{DateTime.Now}] Form_Calibrate not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_cur = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] Form_Calibrate found successfully.");
                    // groupBox_CurrentCalibration
                    Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_CurrentCalibration.");
                    var groupBox_CurrentCalibration = Retry.WhileNull(() => Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_CurrentCalibration")), TimeSpan.FromSeconds(5)).Result;
                    if (groupBox_CurrentCalibration == null)
                    {
                        str_error_log = $"[{DateTime.Now}] groupBox_CurrentCalibration not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_cur = string.Empty;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] groupBox_CurrentCalibration found successfully.");

                    // checkBox_Current
                    Trace.WriteLine($"[{DateTime.Now}] To Find checkBox_Current.");
                    var checkBox_Current = Retry.WhileNull(() => groupBox_CurrentCalibration.FindFirstDescendant(cf => cf.ByAutomationId("checkBox_Current"))?.AsCheckBox(), TimeSpan.FromSeconds(5)).Result;
                    if (checkBox_Current == null)
                    {
                        str_error_log = $"[{DateTime.Now}] checkBox_Current not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        mes_cur = string.Empty;
                        return this;
                    }
                    // 勾选复选框checkBox_Current
                    Retry.WhileFalse(() => checkBox_Current.IsEnabled, TimeSpan.FromSeconds(5));
                    checkBox_Current.IsChecked = true;

                    bool isCalibrate = false;
                    mes_cur = string.Empty;
                    for (int i = 0; i < 3; i++)
                    {
                        // 查找textBox_CurrentAct
                        Trace.WriteLine($"[{DateTime.Now}] To Find textBox_CurrentAct.");
                        var textBox_CurrentAct = Retry.WhileNull(() => groupBox_CurrentCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_CurrentAct"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_CurrentAct == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_CurrentAct not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_cur = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_CurrentAct found successfully.");
                        // 清空输入框并输入指定值
                        Trace.WriteLine($"Enter content: {act_cur}");
                        Retry.WhileFalse(() => textBox_CurrentAct.IsEnabled, TimeSpan.FromSeconds(5));
                        textBox_CurrentAct?.Patterns.Value.Pattern.SetValue(act_cur);

                        // 查找button_DoCalibration按钮
                        Trace.WriteLine($"[{DateTime.Now}] To Find button_DoCalibration.");
                        var button_DoCalibration = Form_Calibrate.FindFirstDescendant(cf => cf.ByAutomationId("button_DoCalibration"))?.AsButton();
                        if (button_DoCalibration == null)
                        {
                            str_error_log = $"[{DateTime.Now}] button_DoCalibration not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_cur = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] button_DoCalibration found successfully.");
                        Retry.WhileFalse(() => button_DoCalibration.IsEnabled, TimeSpan.FromSeconds(5));
                        button_DoCalibration?.Click();
                        Thread.Sleep(300);

                        str_error_log = string.Empty;
                        result = false;
                        for (int j = 0; j < 30; j++)
                        {
                            GetDialogTip(mainWindow, out string content);
                            if (content.Contains($"Calibrate Success"))
                            {
                                str_error_log = $"Current Calibrate Success.";
                                Trace.WriteLine(str_error_log);
                                result = true;
                                break;
                            }
                            if (content.Contains($"Calibrate Fail"))
                            {
                                str_error_log = $"Current Calibrate Fail.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            if (content.Contains($"Err Input"))
                            {
                                str_error_log = $"Current Calibration Err Input.";
                                Trace.WriteLine(str_error_log);
                                result = false;
                                break;
                            }
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (!result)
                        {
                            str_error_log = $"[{i + 1}/3]Current Calibrate Fail. Try Again.";
                            Trace.WriteLine(str_error_log);
                            result = true;
                            mes_cur = string.Empty;
                            continue;
                        }

                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] Current Calibrate Success. Check Difference");

                        // 查找textBox_CurrentMes
                        Trace.WriteLine($"[{DateTime.Now}] [{i + 1}/3] To Find textBox_CurrentMes.");
                        var textBox_CurrentMes = Retry.WhileNull(() => groupBox_CurrentCalibration.FindFirstDescendant(c => c.ByAutomationId("textBox_CurrentMes"))?.AsTextBox(), TimeSpan.FromSeconds(5)).Result;
                        if (textBox_CurrentMes == null)
                        {
                            str_error_log = $"[{DateTime.Now}] textBox_CurrentMes not found.";
                            Trace.WriteLine(str_error_log);
                            result = false;
                            mes_cur = string.Empty;
                            return this;
                        }
                        Trace.WriteLine($"[{DateTime.Now}] textBox_CurrentMes found successfully.");
                        // 获取内容
                        string str_mes_cur = textBox_CurrentMes.Text;
                        Trace.WriteLine($"[{DateTime.Now}] textBox_CurrentMes text: {str_mes_cur}.");
                        try
                        {
                            mes_cur = double.Parse(str_mes_cur).ToString();
                        }
                        catch (Exception ee)
                        {
                            Trace.WriteLine($"GetEditText({str_mes_cur}) Error:['{ee.Message}'], set '0'.");
                            mes_cur = "0";
                        }
                        double diff_vol = Math.Abs(double.Parse(act_cur) - double.Parse(mes_cur));
                        Trace.WriteLine($"Actual Current and Measured Current Difference: [{diff_vol}]");
                        if (diff_vol > _diff_allow)
                        {
                            str_error_log = $"[{i + 1}/3]Current Calibrate Difference Fail.";
                            Trace.WriteLine(str_error_log);
                            continue;
                        }
                        else
                        {
                            isCalibrate = true;
                            Trace.WriteLine($"Current Calibrate Difference Success.");
                            break;
                        }
                    }

                    // 取消勾选
                    if (checkBox_Current != null)
                    {
                        checkBox_Current.IsChecked = false;
                    }
                    if (isCalibrate)
                    {
                        str_error_log = $"Current Calibrate Final Success.";
                        Trace.WriteLine(str_error_log);
                        result = true;
                    }
                    else
                    {
                        str_error_log = $"Current Calibrate Final Fail.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                    }
                    return this;
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                mes_cur = string.Empty;
                return this;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI CMDPanelHandle(string item, out bool result, out string str_error_log)
        {
            try
            {
                if (app == null)
                {
                    str_error_log = $"app is not running.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    return this;
                }
                ActivateWindow(_mainForm);
                using (var automation = new UIA3Automation())
                {
                    var mainWindow = app.GetMainWindow(automation);

                    //// groupBox_ClearLog
                    //Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_ClearLog.");
                    //var groupBox_ClearLog = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_ClearLog")), TimeSpan.FromSeconds(5)).Result;
                    //if (groupBox_ClearLog == null)
                    //{
                    //    str_error_log = $"[{DateTime.Now}] groupBox_ClearLog not found.";
                    //    Trace.WriteLine(str_error_log);
                    //    result = false;
                    //    return this;
                    //}
                    //Trace.WriteLine($"[{DateTime.Now}] groupBox_ClearLog found successfully.");
                    //// 查找btn_ClearCMDLog
                    //Trace.WriteLine($"[{DateTime.Now}] To Find btn_ClearCMDLog.");
                    //var btn_ClearCMDLog = Retry.WhileNull(() => groupBox_ClearLog.FindFirstDescendant(cf => cf.ByAutomationId("btn_ClearCMDLog"))?.AsButton(), TimeSpan.FromSeconds(5)).Result;
                    //if (btn_ClearCMDLog == null)
                    //{
                    //    str_error_log = $"[{DateTime.Now}] btn_ClearCMDLog not found.";
                    //    Trace.WriteLine(str_error_log);
                    //    result = false;
                    //    return this;
                    //}
                    //Trace.WriteLine($"[{DateTime.Now}] btn_ClearCMDLog found successfully.");
                    //Retry.WhileFalse(() => btn_ClearCMDLog.IsEnabled, TimeSpan.FromSeconds(5));
                    //btn_ClearCMDLog?.Click();
                    //Trace.WriteLine($"[{DateTime.Now}] Click Clear Log successfully.");

                    // groupBox_CMDPanel
                    Trace.WriteLine($"[{DateTime.Now}] To Find groupBox_CMDPanel.");
                    var groupBox_CMDPanel = Retry.WhileNull(() => mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("groupBox_CMDPanel")), TimeSpan.FromSeconds(5)).Result;
                    if (groupBox_CMDPanel == null)
                    {
                        str_error_log = $"[{DateTime.Now}] groupBox_CMDPanel not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] groupBox_CMDPanel found successfully.");

                    // 查找按钮
                    Trace.WriteLine($"[{DateTime.Now}] To Find comboBox_CMDInput.");
                    var comboBox_CMDInput = Retry.WhileNull(() => groupBox_CMDPanel.FindFirstDescendant(cf => cf.ByAutomationId("comboBox_CMDInput"))?.AsComboBox(), TimeSpan.FromSeconds(5)).Result;
                    if (comboBox_CMDInput == null)
                    {
                        str_error_log = $"[{DateTime.Now}] comboBox_CMDInput not found.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                    Trace.WriteLine($"[{DateTime.Now}] comboBox_CMDInput found successfully.");
                    Retry.WhileFalse(() => comboBox_CMDInput.IsEnabled, TimeSpan.FromSeconds(5));
                    // 选择指定内容
                    comboBox_CMDInput.Select(item); // 或使用 comboBox.SelectedItem = item;

                    // 验证选择的项
                    if (comboBox_CMDInput.SelectedItem.Text == item)
                    {
                        str_error_log = $"[{DateTime.Now}] comboBox_CMDInput select success.";
                        Trace.WriteLine(str_error_log);
                        result = true;
                        return this;
                    }
                    else
                    {
                        str_error_log = $"[{DateTime.Now}] comboBox_CMDInput select fail.";
                        Trace.WriteLine(str_error_log);
                        result = false;
                        return this;
                    }
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
        }

        /// <summary>
        /// 获取模态框提示内容
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public SmartToolControlFlaUI GetDialogTip(Window mainWindow, out string content)
        {
            try
            {
                //var modalWindows = mainWindow.ModalWindows;
                //foreach (var window in modalWindows)
                //{
                //    Console.WriteLine("模态窗口：" + window.Title);
                //}

                content = "";
                Trace.WriteLine($"[{DateTime.Now}] To Find tipDialog.");
                var tipDialog = mainWindow.FindFirstDescendant(c => c.ByClassName("#32770"));
                if (tipDialog == null)
                {
                    Trace.WriteLine($"[{DateTime.Now}] tipDialog No Found.");
                    return this;
                }
                else
                {
                    var textElements = tipDialog.FindFirstDescendant(c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
                    if(textElements == null)
                    {
                        Trace.WriteLine($"[{DateTime.Now}] textElements No Found.");
                        content = string.Empty;
                        return this;
                    }
                    content = textElements.Name;
                    Trace.WriteLine($"[{DateTime.Now}] content:[{content}]"); 
                    var button = tipDialog.FindFirstChild(c => c.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(c.ByName("确定")))?.AsButton();
                    if (button == null)
                    {
                        Trace.WriteLine($"[{DateTime.Now}] ButtonOK No Found.");
                    }
                    else
                    {
                        button?.Click();
                        Trace.WriteLine($"[{DateTime.Now}] Click ButtonOK Success");
                    }
                }
                
                return this;
            }
            catch (Exception ee)
            {
                Trace.WriteLine(ee.StackTrace);
                content = string.Empty;
                return this;
            }

        }

    }
}
