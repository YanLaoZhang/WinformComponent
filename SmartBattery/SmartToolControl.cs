using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace SmartBattery
{
    public class SmartToolControl
    {
        // 程序进程
        private Process _process;

        // 程序主界面句柄
        private IntPtr _mainWindowHandle;

        // 程序主界面控件
        private AutomationElement _mainWindow;

        /* Calibrate界面控件ID
            Button Name:[Config],                           AutomationId:[button_ChannelConfig]
            Button Name:[Calibrate],                        AutomationId:[button_OffsetCalibration]
            Button Name:[下拉按钮],                         AutomationId:[DropDown]
            Button Name:[Calibrate],                        AutomationId:[button_DoCalibration]
            Button Name:[下拉按钮],                         AutomationId:[DropDown]
            Button Name:[Register],                         AutomationId:[buttonMenu_Registers]
            Button Name:[Memory],                           AutomationId:[buttonMenu_DataFlashMemory]
            Button Name:[Calibrate],                        AutomationId:[buttonMenu_Calibrate]
            Button Name:[Advaned Comm],                     AutomationId:[buttonMenu_Pro]
            Button Name:[Chip Update],                      AutomationId:[buttonMenu_FWUpdate]
            Button Name:[Tools],                            AutomationId:[buttonMenu_MTools]
            Button Name:[最小化],                           AutomationId:[Minimize]
            Button Name:[最大化],                           AutomationId:[Maximize]
            Button Name:[关闭],                             AutomationId:[Close]
              Edit Name:[Measured Voltage],                 AutomationId:[textBox_VoltageAct]
              Edit Name:[Measured Voltage],                 AutomationId:[textBox_VoltageMes]
              Edit Name:[Measured Temper],                  AutomationId:[textBox_Temper2Act]
              Edit Name:[Enter Actual Temper],              AutomationId:[textBox_Temper2Mes]
              Edit Name:[Measured Temper],                  AutomationId:[textBox_TemperAct]
              Edit Name:[Measured Temper],                  AutomationId:[textBox_TemperMes]
              Edit Name:[Measured Current],                 AutomationId:[textBox_CurrentAct]
              Edit Name:[Measured Current],                 AutomationId:[textBox_CurrentMes]
              Edit Name:[],                                 AutomationId:[1001]
              Edit Name:[15:57:04 2024/05/29],              AutomationId:[StatusBar.Pane0]
              Edit Name:[Flash R/W OK],                     AutomationId:[StatusBar.Pane1]
              Edit Name:[],                                 AutomationId:[StatusBar.Pane2]
              Edit Name:[Channel 1: BSTools/SubChan 1/TWI], AutomationId:[StatusBar.Pane3]
              Edit Name:[],                                 AutomationId:[StatusBar.Pane4]
              Edit Name:[USB Disconnected],                 AutomationId:[StatusBar.Pane5]
            CheckBox Name:[Voltage],                        AutomationId:[checkBox_Voltage]
            CheckBox Name:[Int Temper],                     AutomationId:[checkBox_IntTemper]
            CheckBox Name:[Ext Temper],                     AutomationId:[checkBox_ExtTemper]
            CheckBox Name:[Current],                        AutomationId:[checkBox_Current]
         */

        /// <summary>
        /// 清理所有已打开的Smart Battery
        /// </summary>
        /// <param name="exePath"></param>
        public void KillAllSmartTool(string exePath)
        {
            string processName = Path.GetFileNameWithoutExtension(exePath);
            Console.WriteLine($"ProcessName:[{processName}]");
            // 仅允许同时一个程序执行
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                try
                {
                    process.Kill();
                    Console.WriteLine($"Successfully terminated.");
                    //break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to Terminate.[{ex.Message}]");
                }
            }
        }

        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="str_error_log"></param>
        /// <param name="isOnlyOne"></param>
        /// <returns></returns>
        public SmartToolControl StartUp(string exePath, ref string str_error_log)
        {
            try
            {
                string processName = Path.GetFileNameWithoutExtension(exePath);
                Console.WriteLine($"ProcessName:[{processName}]");
                bool isRunning = false;
                // 仅允许同时一个程序执行
                foreach (Process process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        isRunning = true;
                        _process = process;
                        Console.WriteLine($"Smart Battery Is Running.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to Terminate.[{ex.Message}]");
                    }
                }
                if (!isRunning)
                {
                    _process = new Process 
                    { 
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = exePath,
                            UseShellExecute = true
                        }
                    };
                    _process.Start();
                    _process.WaitForInputIdle();
                    Thread.Sleep(1000);
                }
                _mainWindowHandle = _process.MainWindowHandle;
                _mainWindow = AutomationElement.FromHandle(_mainWindowHandle);
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Console.WriteLine($"Error Starting the Program: [{ex.Message}]");
                _process = null;
                _mainWindowHandle = IntPtr.Zero;
                _mainWindow = null;
            }
            return this;
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <returns></returns>
        public SmartToolControl Close()
        {
            try
            {
                _process?.CloseMainWindow();
                _process?.Close();
                _process = null;
                _mainWindowHandle = IntPtr.Zero;
                _mainWindow = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro closing the program: [{ex.Message}]");
            }
            return this;
        }

        /// <summary>
        /// 判断程序是否启动
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return _process != null && !_process.HasExited;
        }

        public SmartToolControl Automate(Action<AutomationElement> action)
        {
            if(_mainWindow != null)
            {
                action(_mainWindow);
            }
            return this;
        }

        public SmartToolControl ClickMenuItem(string menuItemName)
        {
            AutomationElement menuItem = null;
            AutomationElementCollection allMenuItems = _mainWindow.FindAll(TreeScope.Descendants, 
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));
            foreach (AutomationElement item in allMenuItems)
            {
                string EleName = item.Current.Name;
                Console.WriteLine($"MenuItem Name:{EleName}");
                if(EleName.Contains(menuItemName))
                {
                    menuItem = item;
                    break;
                }
            }

            InvokeClickElement(menuItem);

            return this;
        }

        /// <summary>
        /// 打开AFI操作
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public SmartToolControl SetAFIFile(string filePath)
        {
            // 查找文件选择框
            AutomationElement fileDialog = _mainWindow.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "Select AFI file: Channel 0")
                );
            if (fileDialog != null)
            {
                // 查找文件名编辑框
                AutomationElement fileNameEdit = null;
                AutomationElementCollection allComboBoxItems = fileDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ComboBox));
                foreach (AutomationElement ele in allComboBoxItems)
                {
                    Console.WriteLine($"{ele.Current.Name}");
                    if (ele.Current.Name.Contains("文件名"))
                    {
                        fileNameEdit = ele;
                        break;
                    }
                }
                if (fileNameEdit != null)
                {
                    // 设置文件路径
                    ValuePattern valuePattern = (ValuePattern)fileNameEdit.GetCurrentPattern(ValuePattern.Pattern);
                    Console.WriteLine($"{filePath}");
                    valuePattern.SetValue(filePath);

                    // 查找并点击“打开”按钮
                    AutomationElement openButton = null;
                    AutomationElementCollection allButtonItems = fileDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
                    foreach (AutomationElement ele in allButtonItems)
                    {
                        Console.WriteLine($"{ele.Current.Name}");
                        if (ele.Current.Name.Contains("打开") || ele.Current.Name.Contains("Open"))
                        {
                            openButton = ele;
                            break;
                        }
                    }
                    if (openButton != null)
                    {
                        InvokeClickElement(openButton);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 获取模态框提示信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public SmartToolControl GetDialogTip(out string content)
        {
            content = "";
            AutomationElement tipDialog = null;
            AutomationElementCollection dialogs = _mainWindow.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            foreach (AutomationElement item in dialogs)
            {
                string EleName = item.Current.Name;
                Console.WriteLine($"Window Name:[{EleName}]");
                if (EleName.Contains(""))
                {
                    tipDialog = item;
                    break;
                }
            }
            if (tipDialog != null)
            {
                var textElements = tipDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));

                content = string.Join(Environment.NewLine, textElements.Cast<AutomationElement>().Select(te => te.Current.Name));
                Console.WriteLine($"content:[{content}]");
                var button = tipDialog.FindFirst(TreeScope.Descendants, new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                    new PropertyCondition(AutomationElement.NameProperty, "确定")
                    ));
                InvokeClickElement(button);
            }
            return this;
        }

        /// <summary>
        /// Board Offset Calibrate
        /// </summary>
        /// <returns></returns>
        public SmartToolControl OffsetCalibrate()
        {
            if( _mainWindow == null)
            {
                Console.WriteLine($"mainWindow is Null");
                return this;
            }
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                Thread.Sleep(1000);
            }

            // Board Offset Calibrate的Calibrate按钮
            AutomationElement button_OffsetCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_OffsetCalibration");
            if (button_OffsetCalibration != null)
            {
                InvokeClickElement(button_OffsetCalibration);
                Thread.Sleep(1000);
            }

            GetDialogTip(out string content);
            if (content.Contains($"Calibrate Success"))
            {
                Console.WriteLine($"Board Offset Calibrate Success.");
            }
            if (content.Contains($"Calibrate Fail"))
            {
                Console.WriteLine($"Board Offset Calibrate Fail.");
            }
            Thread.Sleep(5000);
            return this;
        }

        /// <summary>
        /// Voltage Calibrate
        /// </summary>
        /// <param name="act_vol"></param>
        /// <returns></returns>
        public SmartToolControl VoltageCalibrate(string act_vol)
        {
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                Thread.Sleep(1000);
            }
            bool isCalibrate = false;
            do
            {
                // 勾选Voltage
                AutomationElement checkBox_Voltage = GetElementByAutomationID(_mainWindow, ControlType.CheckBox, "checkBox_Voltage");
                if (checkBox_Voltage != null)
                {
                    CheckCheckBox(checkBox_Voltage);
                    Thread.Sleep(1000);
                }

                // Enter Actual Voltage
                AutomationElement textBox_VoltageAct = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_VoltageAct");
                if (textBox_VoltageAct != null)
                {
                    SetEditText(textBox_VoltageAct, act_vol);
                    Thread.Sleep(1000);
                }

                // 最下方的Calibrate按钮
                AutomationElement button_DoCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_DoCalibration");
                if (button_DoCalibration != null)
                {
                    InvokeClickElement(button_DoCalibration);
                    Thread.Sleep(1000);
                }

                GetDialogTip(out string content);
                if (content.Contains($"Calibrate Success"))
                {
                    Console.WriteLine($"Voltage Calibrate Success.");
                }
                if (content.Contains($"Calibrate Fail"))
                {
                    Console.WriteLine($"Voltage Calibrate Fail.");
                    // 取消勾选
                    if (checkBox_Voltage != null)
                    {
                        UncheckCheckBox(checkBox_Voltage);
                        Thread.Sleep(1000);
                    }
                    break;
                }

                AutomationElement textBox_VoltageMes = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_VoltageMes");
                if (textBox_VoltageMes != null)
                {
                    string mes_vol = GetEditText(textBox_VoltageMes);
                    Thread.Sleep(1000);
                    double diff_vol = Math.Abs(double.Parse(act_vol) - double.Parse(mes_vol));
                    Console.WriteLine($"Actual Voltage and Measured Voltage Difference: [{diff_vol}]");
                    if (diff_vol > 10)
                    {
                        isCalibrate = true;
                        // 取消勾选
                        if (checkBox_Voltage != null)
                        {
                            UncheckCheckBox(checkBox_Voltage);
                            Thread.Sleep(1000);
                        }
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Voltage Calibrate OK");
                        // 取消勾选
                        if (checkBox_Voltage != null)
                        {
                            UncheckCheckBox(checkBox_Voltage);
                            Thread.Sleep(1000);
                        }
                        break;
                    }
                }
            } while (isCalibrate);

            return this;
        }

        /// <summary>
        /// Current Calibrate
        /// </summary>
        /// <param name="act_cur"></param>
        /// <returns></returns>
        public SmartToolControl CurrentCalibrate(string act_cur)
        {
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                Thread.Sleep(1000);
            }

            bool isCalibrate = false;
            do
            {
                // 勾选Voltage
                AutomationElement checkBox_Current = GetElementByAutomationID(_mainWindow, ControlType.CheckBox, "checkBox_Current");
                if (checkBox_Current != null)
                {
                    CheckCheckBox(checkBox_Current);
                    Thread.Sleep(1000);
                }

                // Enter Actual Current
                AutomationElement textBox_CurrentAct = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_CurrentAct");
                if (textBox_CurrentAct != null)
                {
                    SetEditText(textBox_CurrentAct, act_cur);
                    Thread.Sleep(1000);
                }

                // 最下方的Calibrate按钮
                AutomationElement button_DoCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_DoCalibration");
                if (button_DoCalibration != null)
                {
                    InvokeClickElement(button_DoCalibration);
                    Thread.Sleep(1000);
                }

                GetDialogTip(out string content);
                if (content.Contains($"Calibrate Success"))
                {
                    Console.WriteLine($"Current Calibrate Success.");
                }
                if (content.Contains($"Calibrate Fail"))
                {
                    Console.WriteLine($"Current Calibrate Fail.");
                    // 取消勾选
                    if (checkBox_Current != null)
                    {
                        UncheckCheckBox(checkBox_Current);
                        Thread.Sleep(1000);
                    }
                    break;
                }

                AutomationElement textBox_CurrentMes = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_CurrentMes");
                if (textBox_CurrentMes != null)
                {
                    string mes_cur = GetEditText(textBox_CurrentMes);
                    Thread.Sleep(1000);
                    double diff_cur = Math.Abs(double.Parse(act_cur) - double.Parse(mes_cur));
                    Console.WriteLine($"Actual Current and Measured Current Difference: [{diff_cur}]");
                    if (diff_cur > 10)
                    {
                        isCalibrate = true;
                        // 取消勾选
                        if (checkBox_Current != null)
                        {
                            UncheckCheckBox(checkBox_Current);
                            Thread.Sleep(1000);
                        }
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Current Calibrate OK");
                        // 取消勾选
                        if (checkBox_Current != null)
                        {
                            UncheckCheckBox(checkBox_Current);
                            Thread.Sleep(1000);
                        }
                        break;
                    }
                }
            } while (isCalibrate);

            return this;
        }

        static AutomationElement GetElementByAutomationID(AutomationElement parentElement, ControlType obj, string id)
        {
            AutomationElement element = null;
            AutomationElementCollection allElements = parentElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, obj));
            foreach (AutomationElement item in allElements)
            {
                Console.WriteLine($"Type:[{obj}], Name:[{item.Current.Name}], AutomationId:[{item.Current.AutomationId}]");
                if(item.Current.AutomationId == id)
                {
                    element = item;
                }
            }
            return element;
        }

        /// <summary>
        /// 点击操作
        /// </summary>
        /// <param name="menuItem"></param>
        static void InvokeClickElement(AutomationElement menuItem)
        {
            if (menuItem != null && menuItem.TryGetCurrentPattern(InvokePattern.Pattern, out object pattern))
            {
                var invokePattern = (InvokePattern)pattern;
                invokePattern.Invoke();
            }
        }

        /// <summary>
        /// 勾选CheckBox控件
        /// </summary>
        /// <param name="checkBox"></param>
        static void CheckCheckBox(AutomationElement checkBox)
        {
            if(checkBox != null && checkBox.TryGetCurrentPattern(TogglePattern.Pattern, out object pattern))
            {
                TogglePattern togglePattern = (TogglePattern)pattern;
                if(togglePattern.Current.ToggleState != ToggleState.On)
                {
                    togglePattern.Toggle();
                    Console.WriteLine($"CheckBox is checked.");
                }
                else
                {
                    Console.WriteLine($"CheckBox is already checked.");
                }
            }
            else
            {
                Console.WriteLine($"TogglePattern not supported.");
            }
        }

        /// <summary>
        /// 取消勾选CheckBox控件
        /// </summary>
        /// <param name="checkBox"></param>
        static void UncheckCheckBox(AutomationElement checkBox)
        {
            if (checkBox != null && checkBox.TryGetCurrentPattern(TogglePattern.Pattern, out object pattern))
            {
                TogglePattern togglePattern = (TogglePattern)pattern;
                if (togglePattern.Current.ToggleState == ToggleState.On)
                {
                    togglePattern.Toggle();
                    Console.WriteLine($"CheckBox is unchecked.");
                }
                else
                {
                    Console.WriteLine($"CheckBox is already unchecked.");
                }
            }
            else
            {
                Console.WriteLine($"TogglePattern not supported.");
            }
        }

        /// <summary>
        /// 输入文本到Edit控件
        /// </summary>
        /// <param name="editElement"></param>
        /// <param name="text"></param>
        static void SetEditText(AutomationElement editElement, string text)
        {
            if(editElement!=null && editElement.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern))
            {
                ValuePattern valuePattern = (ValuePattern)pattern;

                if (!valuePattern.Current.IsReadOnly)
                {
                    valuePattern.SetValue(text);
                    Console.WriteLine($"Text input successful.");
                }
                else
                {
                    Console.WriteLine($"The Edit control is read-only.");
                }
            }
            else
            {
                Console.WriteLine($"ValuePattern not supported.");
            }
        }

        /// <summary>
        /// 获取输入框Edit的内容
        /// </summary>
        /// <param name="editElement"></param>
        /// <returns></returns>
        static string GetEditText(AutomationElement editElement)
        {
            if (editElement !=null && editElement.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern))
            {
                ValuePattern valuePattern = (ValuePattern)pattern;
                return valuePattern.Current.Value;
            }
            else
            {
                Console.WriteLine("ValuePattern not supported.");
                return string.Empty;
            }
        }

    }
}
