using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;

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

        // 电压电流校准误差设置
        public double _diff_allow { get; set; }

        public SmartToolControl(double diff_allow=3)
        {
            _diff_allow = diff_allow;
            Trace.WriteLine($"DIFF is [{_diff_allow}]");
        }

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
            Trace.WriteLine($"ProcessName:[{processName}]");
            // 仅允许同时一个程序执行
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                try
                {
                    process.Kill();
                    Trace.WriteLine($"Successfully terminated.");
                    //break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Failed to Terminate.[{ex.Message}]");
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
                Trace.WriteLine($"DIFF is [{_diff_allow}]");
                string processName = Path.GetFileNameWithoutExtension(exePath);
                Trace.WriteLine($"ProcessName:[{processName}]");
                bool isRunning = false;
                // 仅允许同时一个程序执行
                foreach (Process process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        isRunning = true;
                        _process = process;
                        Trace.WriteLine($"Smart Battery Is Running.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Failed to Terminate.[{ex.Message}]");
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
                    bool started = _process.WaitForInputIdle(5000);
                    Trace.WriteLine($"Process already Startup");
                }
                for(int i=0; i<10; i++)
                {
                    try
                    {
                        _mainWindowHandle = _process.MainWindowHandle;
                        _mainWindow = AutomationElement.FromHandle(_mainWindowHandle);
                        Trace.WriteLine($"Handel OK.");
                        break;
                    }
                    catch (Exception ee)
                    {
                        Trace.WriteLine($"Handel Error. [{ee.Message}]");
                        Thread.Sleep(1000);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message;
                Trace.WriteLine($"Error Starting the Program: [{ex.Message}]");
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
                Trace.WriteLine($"Erro closing the program: [{ex.Message}]");
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
            Trace.WriteLine($"Click MenuItem--");
            bool isFound = false;
            AutomationElement menuItem = null;
            for (int i=0; i<4; i++)
            {
                menuItem = GetElementByName(_mainWindow, ControlType.MenuItem, menuItemName);
                /*
                AutomationElement menuItem = null;
                AutomationElementCollection allMenuItems = _mainWindow.FindAll(TreeScope.Descendants, 
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));
                foreach (AutomationElement item in allMenuItems)
                {
                    string EleName = item.Current.Name;
                    Trace.WriteLine($"MenuItem Name:{EleName}");
                    if(EleName.Contains(menuItemName))
                    {
                        menuItem = item;
                        break;
                    }
                }*/
                if (menuItem != null)
                {
                    isFound = true;
                    break;
                }
                else
                {
                    Trace.WriteLine($"[{i+1}/3]Retry find");
                    Thread.Sleep(500);
                    continue;
                }
            }

            if (isFound)
            {
                InvokeClickElement(menuItem);
                Trace.WriteLine($"Click MenuItem OK");
            }
            else
            {
                Trace.WriteLine($"Click MenuItem NG:[Element Null]");
            }

            return this;
        }

        /// <summary>
        /// 打开AFI操作
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public SmartToolControl SetAFIFile(string filePath, out bool result, out string str_error_log)
        {
            //Thread.Sleep(1000);
            // 点击菜单栏
            ClickMenuItem("File");
            //Thread.Sleep(100);

            ClickMenuItem("Open AFI File(*.afi)");
            //Thread.Sleep(100);

            // 查找文件选择框
            Trace.WriteLine($"-- To Find FileDialog.");
            AutomationElement fileDialog = null;
            bool isFindFileDialog = false;
            for(int i=0; i<4; i++)
            {
                fileDialog = _mainWindow.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Select AFI file: Channel 0"));
                if (fileDialog == null)
                {
                    Trace.WriteLine($"[{i + 1}/3]fileDialog is Null");
                    Thread.Sleep(500);
                    continue;
                }
                else
                {
                    Trace.WriteLine($"-- [{i + 1}/3]FileDialog already Open.");
                    isFindFileDialog = true;
                    break;
                }
            }
            if (!isFindFileDialog)
            {
                str_error_log = $"fileDialog is Null";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }

            // 查找文件名编辑框
            Trace.WriteLine($"-- To Find FileNameEdit.");
            AutomationElement fileNameEdit = GetElementByName(fileDialog, ControlType.ComboBox, "文件名(N):"); 
            if (fileNameEdit == null)
            {
                str_error_log = $"fileNameEdit no Found";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
            else
            {
                Trace.WriteLine($"-- FileNameEdit already Found.");
            }

            // 设置文件路径
            bool isInput = false;
            for(int i= 0; i < 4; i++)
            {
                try
                {
                    ValuePattern valuePattern = (ValuePattern)fileNameEdit.GetCurrentPattern(ValuePattern.Pattern);
                    valuePattern.SetValue(filePath);
                    Trace.WriteLine($"-- [{i + 1}/3]Input {filePath}");
                    isInput = true;
                    break;
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/3]Input FileName Error:[{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }

            if (!isInput)
            {
                str_error_log = $"InputFileName Error";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }

            //Thread.Sleep(100);

            // 查找并点击“打开”按钮
            AutomationElement openButton = GetElementByName(_mainWindow, ControlType.Button, "打开(O)");
            if (openButton == null)
            {
                str_error_log = $"OpenButton is Null";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
            else
            {
                Trace.WriteLine($"-- openButton already Found.");
            }
            /*AutomationElementCollection allButtonItems = fileDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));
            foreach (AutomationElement ele in allButtonItems)
            {
                Trace.WriteLine($"{ele.Current.Name}");
                if (ele.Current.Name.Contains("打开") || ele.Current.Name.Contains("Open"))
                {
                    openButton = ele;
                    break;
                }
            }
            if (openButton == null)
            {
                str_error_log = $"OpenButton is Null";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }*/

            InvokeClickElement(openButton);
            Thread.Sleep(6000);

            str_error_log = string.Empty;
            result = false;
            for(int i = 0; i < 30; i++)
            {
                GetDialogTip(out string content);
                if (content.Contains($"AFI Write Success"))
                {
                    str_error_log = $"AFI Write Success.";
                    Trace.WriteLine(str_error_log);
                    result = true;
                    break;
                }
                if (content.Contains($"AFI Write Fail"))
                {
                    str_error_log = $"AFI Write Fail.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    break;
                }
                Thread.Sleep(3000);
                continue;
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
            AutomationElement tipDialog = tipDialog = GetElementByName(_mainWindow, ControlType.Window, "");
            if (tipDialog == null)
            {
                Trace.WriteLine($"-- tipDialog No Found.");
            }
            else
            {
                var textElements = tipDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));
                content = string.Join(Environment.NewLine, textElements.Cast<AutomationElement>().Select(te => te.Current.Name));
                Trace.WriteLine($"content:[{content}]");
            }
            //AutomationElementCollection dialogs = _mainWindow.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            //foreach (AutomationElement item in dialogs)
            //{
            //    string EleName = item.Current.Name;
            //    Trace.WriteLine($"Window Name:[{EleName}]");
            //    if (EleName.Contains(""))
            //    {
            //        tipDialog = item;
            //        break;
            //    }
            //}

            AutomationElement button = button = GetElementByName(tipDialog, ControlType.Button, "确定");
            if (button == null)
            {
                Trace.WriteLine($"-- ButtonOK No Found.");
            }
            else
            {
                InvokeClickElement(button);
                Trace.WriteLine($"Click ButtonOK Success");
            }

            /*if (tipDialog != null)
            {
                var textElements = tipDialog.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text));

                content = string.Join(Environment.NewLine, textElements.Cast<AutomationElement>().Select(te => te.Current.Name));
                Trace.WriteLine($"content:[{content}]");
                var button = tipDialog.FindFirst(TreeScope.Descendants, new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                    new PropertyCondition(AutomationElement.NameProperty, "确定")
                    ));
                InvokeClickElement(button);
            }*/
            return this;
        }

        /// <summary>
        /// Board Offset Calibrate
        /// </summary>
        /// <returns></returns>
        public SmartToolControl OffsetCalibrate(out bool result, out string str_error_log)
        {
            if( _mainWindow == null)
            {
                str_error_log = $"mainWindow is Null";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                //Thread.Sleep(1000);
            }

            // Board Offset Calibrate的Calibrate按钮
            AutomationElement button_OffsetCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_OffsetCalibration");
            if (button_OffsetCalibration != null)
            {
                InvokeClickElement(button_OffsetCalibration);
                Thread.Sleep(2000);
            }

            str_error_log = string.Empty;
            result = false;
            for(int i= 0; i < 30; i++)
            {
                GetDialogTip(out string content);
                if (content.Contains($"Calibrate Success"))
                {
                    str_error_log = $"Board Offset Calibrate Success.";
                    Trace.WriteLine(str_error_log);
                    result = true;
                    break;
                }
                if (content.Contains($"Calibrate Fail"))
                {
                    str_error_log = $"Board Offset Calibrate Fail.";
                    Trace.WriteLine(str_error_log);
                    result = false;
                    break;
                }
                Thread.Sleep(2000);
                continue;
            }

            Thread.Sleep(500);
            return this;
        }

        /// <summary>
        /// Voltage Calibrate
        /// </summary>
        /// <param name="act_vol"></param>
        /// <returns></returns>
        public SmartToolControl VoltageCalibrate(string act_vol, out bool result, out string mes_vol, out string str_error_log)
        {
            mes_vol = string.Empty;
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                //Thread.Sleep(1000);
            }
            else
            {
                str_error_log = $"buttonMenu_Calibrate no Found.";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }

            // 勾选Voltage
            AutomationElement checkBox_Voltage = GetElementByAutomationID(_mainWindow, ControlType.CheckBox, "checkBox_Voltage");
            if (checkBox_Voltage != null)
            {
                CheckCheckBox(checkBox_Voltage);
                //Thread.Sleep(1000);
            }
            bool isCalibrate = false;
            for(int i = 0; i < 3; i++)
            {
                str_error_log = string.Empty;
                // Enter Actual Voltage
                AutomationElement textBox_VoltageAct = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_VoltageAct");
                if (textBox_VoltageAct != null)
                {
                    SetEditText(textBox_VoltageAct, act_vol);
                    //Thread.Sleep(1000);
                }

                // 最下方的Calibrate按钮
                AutomationElement button_DoCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_DoCalibration");
                if (button_DoCalibration != null)
                {
                    InvokeClickElement(button_DoCalibration);
                    Thread.Sleep(3000);
                }

                string content = string.Empty;
                for (int j = 0; j < 10; j++)
                {
                    GetDialogTip(out string content_cur);
                    if (content_cur.Contains($"Calibrate Success")|| content_cur.Contains($"Calibrate Fail"))
                    {
                        break;
                    }
                    Thread.Sleep(2000);
                    content = content_cur;
                }
                if (content.Contains($"Calibrate Success"))
                {
                    Trace.WriteLine($"Voltage Calibrate Success.");
                }
                if (content.Contains($"Calibrate Fail"))
                {
                    str_error_log = $"Voltage Calibrate Fail.";
                    Trace.WriteLine(str_error_log);
                    continue;
                }

                AutomationElement textBox_VoltageMes = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_VoltageMes");
                if (textBox_VoltageMes != null)
                {
                    string str_mes_vol = GetEditText(textBox_VoltageMes);
                    //Thread.Sleep(1000);
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
                        str_error_log = $"Voltage Calibrate Difference Fail.";
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
            }
            // 取消勾选
            if (checkBox_Voltage != null)
            {
                UncheckCheckBox(checkBox_Voltage);
                //Thread.Sleep(1000);
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

        /// <summary>
        /// Current Calibrate
        /// </summary>
        /// <param name="act_cur"></param>
        /// <returns></returns>
        public SmartToolControl CurrentCalibrate(string act_cur, out bool result, out string mes_cur, out string str_error_log)
        {
            mes_cur = string.Empty;
            // 最上方的Menu的Calibrate按钮
            AutomationElement buttonMenu_Calibrate = GetElementByAutomationID(_mainWindow, ControlType.Button, "buttonMenu_Calibrate");
            if (buttonMenu_Calibrate != null)
            {
                InvokeClickElement(buttonMenu_Calibrate);
                //Thread.Sleep(1000);
            }
            else
            {
                str_error_log = $"buttonMenu_Calibrate is NULL.";
                Trace.WriteLine(str_error_log);
                result = false;
                return this;
            }

            // 勾选Voltage
            AutomationElement checkBox_Current = GetElementByAutomationID(_mainWindow, ControlType.CheckBox, "checkBox_Current");
            if (checkBox_Current != null)
            {
                CheckCheckBox(checkBox_Current);
                //Thread.Sleep(1000);
            }
            bool isCalibrate = false;
            for(int i = 0; i < 3; i++)
            {
                str_error_log = string.Empty;
                // Enter Actual Current
                AutomationElement textBox_CurrentAct = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_CurrentAct");
                if (textBox_CurrentAct != null)
                {
                    SetEditText(textBox_CurrentAct, act_cur);
                    //Thread.Sleep(1000);
                }

                // 最下方的Calibrate按钮
                AutomationElement button_DoCalibration = GetElementByAutomationID(_mainWindow, ControlType.Button, "button_DoCalibration");
                if (button_DoCalibration != null)
                {
                    InvokeClickElement(button_DoCalibration);
                    Thread.Sleep(3000);
                }

                string content = string.Empty;
                for (int j = 0; j < 10; j++)
                {
                    GetDialogTip(out string content_cur);
                    if (content_cur.Contains($"Calibrate Success") || content_cur.Contains($"Calibrate Fail"))
                    {
                        break;
                    }
                    Thread.Sleep(2000);
                    content = content_cur;
                }
                if (content.Contains($"Calibrate Success"))
                {
                    Trace.WriteLine($"Current Calibrate Success.");
                }
                if (content.Contains($"Calibrate Fail"))
                {
                    str_error_log = $"Current Calibrate Fail.";
                    Trace.WriteLine(str_error_log);
                    continue;
                }

                AutomationElement textBox_CurrentMes = GetElementByAutomationID(_mainWindow, ControlType.Edit, "textBox_CurrentMes");
                if (textBox_CurrentMes != null)
                {
                    string str_mes_cur = GetEditText(textBox_CurrentMes);
                    //Thread.Sleep(1000);
                    try
                    {
                        mes_cur = double.Parse(str_mes_cur).ToString();
                    }
                    catch (Exception ee)
                    {
                        Trace.WriteLine($"GetEditText({str_mes_cur}) Error:['{ee.Message}'], set '0'.");
                        mes_cur = "0";
                    }
                    //Thread.Sleep(1000);
                    double diff_cur = Math.Abs(double.Parse(act_cur) - double.Parse(mes_cur));
                    Trace.WriteLine($"Actual Current and Measured Current Difference: [{diff_cur}]");
                    if (diff_cur > _diff_allow)
                    {
                        str_error_log = $"Current Calibrate Difference Fail.";
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
            }
            // 取消勾选
            if (checkBox_Current != null)
            {
                UncheckCheckBox(checkBox_Current);
                Thread.Sleep(1000);
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

        /// <summary>
        /// 选择CMD Panel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public SmartToolControl CMDPanelHandle(string item, out bool result, out string str_error_log)
        {
            AutomationElement comboBox_cmdPanel = GetElementByAutomationID(_mainWindow, ControlType.ComboBox, "comboBox_CMDInput");
            if (comboBox_cmdPanel != null)
            {
                SelectComboBox(comboBox_cmdPanel, item);
                Thread.Sleep(1000);
                str_error_log = string.Empty;
                result = true;
            }
            else
            {
                str_error_log = $"comboBox_cmdPanel is NULL.";
                Trace.WriteLine(str_error_log);
                result = false;
            }

            return this;
        }

        /// <summary>
        /// 依据ID查找控件
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static AutomationElement GetElementByAutomationID(AutomationElement parentElement, ControlType obj, string id)
        {
            Trace.WriteLine($"To Find Element By ID");
            AndCondition andCondition = new AndCondition
            (
                new PropertyCondition(AutomationElement.ControlTypeProperty, obj),
                new PropertyCondition(AutomationElement.AutomationIdProperty, id)
            );
            AutomationElement element = null;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    element = parentElement.FindFirst(TreeScope.Descendants, andCondition);
                    if (element != null)
                    {
                        Trace.WriteLine($"[{i + 1}/4] Type:[{obj.ProgrammaticName}], Name:[{element.Current.Name}], AutomationId:[{element.Current.AutomationId}]");
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] No Found Element By ID");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] Find Element By ID Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
            /*AutomationElement element = null;
            AutomationElementCollection allElements = parentElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, obj));
            foreach (AutomationElement item in allElements)
            {
                Trace.WriteLine($"Type:[{obj.ProgrammaticName}], Name:[{item.Current.Name}], AutomationId:[{item.Current.AutomationId}]");
                if(item.Current.AutomationId == id)
                {
                    element = item;
                    break;
                }
            }
            Trace.WriteLine($"Find Element OK By ID");*/
            return element;
        }

        /// <summary>
        /// 依据Name查找控件
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static AutomationElement GetElementByName(AutomationElement parentElement, ControlType obj, string name)
        {
            Trace.WriteLine($"To Find Element By Name");
            AndCondition andCondition = new AndCondition
            (
                new PropertyCondition(AutomationElement.ControlTypeProperty, obj),
                new PropertyCondition(AutomationElement.NameProperty, name)
            );
            AutomationElement element = null;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    element = parentElement.FindFirst(TreeScope.Descendants, andCondition);
                    if (element != null)
                    {
                        Trace.WriteLine($"[{i + 1}/4] Type:[{obj.ProgrammaticName}], Name:[{element.Current.Name}], AutomationId:[{element.Current.AutomationId}]");
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] No Found Element By Name");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] Find Element By Name Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }

            //AutomationElement element = null;
            //AutomationElementCollection allElements = parentElement.FindAll(TreeScope.Descendants,
            //    new PropertyCondition(AutomationElement.ControlTypeProperty, obj));

            //foreach (AutomationElement item in allElements)
            //{
            //    Trace.WriteLine($"Type:[{obj.ProgrammaticName}], Name:[{item.Current.Name}], AutomationId:[{item.Current.AutomationId}]");
            //    if (item.Current.Name == name)
            //    {
            //        element = item;
            //        break;
            //    }
            //}
            return element;
        }

        /// <summary>
        /// 点击操作
        /// </summary>
        /// <param name="menuItem"></param>
        static void InvokeClickElement(AutomationElement menuItem)
        {
            Trace.WriteLine($"To Click Element");
            for(int i = 0; i < 4; i++)
            {
                try
                {
                    if (menuItem != null && menuItem.TryGetCurrentPattern(InvokePattern.Pattern, out object pattern))
                    {
                        var invokePattern = (InvokePattern)pattern;
                        invokePattern.Invoke();
                        Trace.WriteLine($"[{i + 1}/4] Click Element OK");
                    }
                    break;
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] Click Element Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
        }

        /// <summary>
        /// 勾选CheckBox控件
        /// </summary>
        /// <param name="checkBox"></param>
        static void CheckCheckBox(AutomationElement checkBox)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (checkBox != null && checkBox.TryGetCurrentPattern(TogglePattern.Pattern, out object pattern))
                    {
                        TogglePattern togglePattern = (TogglePattern)pattern;
                        if(togglePattern.Current.ToggleState != ToggleState.On)
                        {
                            togglePattern.Toggle();
                            Trace.WriteLine($"[{i + 1}/4] CheckBox is checked.");
                        }
                        else
                        {
                            Trace.WriteLine($"[{i + 1}/4] CheckBox is already checked.");
                        }
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] TogglePattern not supported.");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] Check CheckBox Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
        }

        /// <summary>
        /// 取消勾选CheckBox控件
        /// </summary>
        /// <param name="checkBox"></param>
        static void UncheckCheckBox(AutomationElement checkBox)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (checkBox != null && checkBox.TryGetCurrentPattern(TogglePattern.Pattern, out object pattern))
                    {
                        TogglePattern togglePattern = (TogglePattern)pattern;
                        if (togglePattern.Current.ToggleState == ToggleState.On)
                        {
                            togglePattern.Toggle();
                            Trace.WriteLine($"[{i + 1}/4] CheckBox is unchecked.");
                        }
                        else
                        {
                            Trace.WriteLine($"[{i + 1}/4] CheckBox is already unchecked.");
                        }
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] TogglePattern not supported.");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] unCheck CheckBox Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
        }

        /// <summary>
        /// 输入文本到Edit控件
        /// </summary>
        /// <param name="editElement"></param>
        /// <param name="text"></param>
        static void SetEditText(AutomationElement editElement, string text)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (editElement != null && editElement.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern))
                    {
                        ValuePattern valuePattern = (ValuePattern)pattern;

                        if (!valuePattern.Current.IsReadOnly)
                        {
                            valuePattern.SetValue(text);
                            Trace.WriteLine($"[{i + 1}/4] Text input successful.");
                        }
                        else
                        {
                            Trace.WriteLine($"[{i + 1}/4] The Edit control is read-only.");
                        }
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] ValuePattern not supported.");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] SetEditText Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
        }

        /// <summary>
        /// 获取输入框Edit的内容
        /// </summary>
        /// <param name="editElement"></param>
        /// <returns></returns>
        static string GetEditText(AutomationElement editElement)
        {
            string result = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (editElement !=null && editElement.TryGetCurrentPattern(ValuePattern.Pattern, out object pattern))
                    {
                        ValuePattern valuePattern = (ValuePattern)pattern;
                        result = valuePattern.Current.Value;
                        break;
                    }
                    else
                    {
                        Trace.WriteLine($"[{i + 1}/4] ValuePattern not supported.");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] GetEditText Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
            return result;
        }

        /// <summary>
        /// 选择下拉框指定元素
        /// </summary>
        /// <param name="comboBoxElement"></param>
        /// <param name="item"></param>
        static void SelectComboBox(AutomationElement comboBoxElement, string item)
        {
            if (comboBoxElement == null)
            {
                Trace.WriteLine("No Found ComboBox");
                return;
            }

            // 获取ExpandCollapsePattern
            ExpandCollapsePattern expandCollapsePattern = null;
            bool isPattern = false;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    expandCollapsePattern = comboBoxElement.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;

                    if (expandCollapsePattern == null)
                    {
                        Trace.WriteLine($"[{i+1}/4]Cannot Get ExpandCollapsePattern");
                        Thread.Sleep(500);
                        continue;
                    }
                    isPattern = true;
                    break;
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] SelectComboBox Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
            if(!isPattern)
            {
                Trace.WriteLine("Cannot Get ExpandCollapsePattern");
                return;
            }

            // 展开ComboBox
            bool isExpand = false;
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    expandCollapsePattern.Expand();
                    isExpand = true;
                    break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"[{j + 1}/4]Expand ComboBox Error: " + ex.Message);
                    Thread.Sleep(500);
                    continue;
                }
            }
            if(!isExpand)
            {
                Trace.WriteLine("Cannot Get ExpandCollapsePattern");
                return;
            }

            //Thread.Sleep(1000);
            AutomationElement targetItem = GetElementByName(comboBoxElement, ControlType.ListItem, item);
            //AutomationElementCollection listItems = comboBoxElement.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            //foreach (AutomationElement element in listItems)
            //{
            //    Trace.WriteLine($"{element.Current.Name},{element.Current.ControlType.ProgrammaticName}");
            //    if (element.Current.Name == item)
            //    {
            //        targetItem = element;
            //        break;
            //    }
            //}

            if (targetItem == null)
            {
                Trace.WriteLine("No Found Select Item");
                return;
            }

            // 选择下拉列表项
            SelectionItemPattern selectionItemPattern = null;
            bool isSelectPattern = false;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    selectionItemPattern = targetItem.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

                    if (selectionItemPattern == null)
                    {
                        Trace.WriteLine($"[{i + 1}/4]Cannot Get SelectionItemPattern");
                        Thread.Sleep(500);
                        continue;
                    }
                    isSelectPattern = true;
                    break;
                }
                catch (Exception ee)
                {
                    Trace.WriteLine($"[{i + 1}/4] Get SelectionItemPattern Error: [{ee.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }
            if (!isSelectPattern)
            {
                Trace.WriteLine("Cannot Get SelectionItemPattern");
                return;
            }

            for (int j = 0; j < 4; j++)
            {
                try
                {
                    selectionItemPattern.Select();
                    Trace.WriteLine($"[{j + 1}/4]Select Item OK");
                    break;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"[{j + 1}/4]Select ComboBox Item Error: [{ex.Message}]");
                    Thread.Sleep(500);
                    continue;
                }
            }

        }
    }
}
