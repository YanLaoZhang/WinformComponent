using CloudAPILib;
using FLUKE8808ALib;
using LogLib;
using MyCustomDialog;
using PCCommandLib;
using RelaySerialLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDMSerialLib;
using VLCForm;
using XDC01SerialLib;

namespace XDC01Action
{
    public class TestAction
    {
        System.Windows.Forms.DataGridView _dataGridView;

        /// <summary>
        /// 非阻塞式等待
        /// </summary>
        /// <param name="t"></param>
        public void Delay(int t)
        {
            int numa = Environment.TickCount;
            while (true)
            {
                Application.DoEvents();
                if (Environment.TickCount - numa > t)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 等待开机
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="logger"></param>
        /// <param name="testParam"></param>
        /// <returns></returns>
        public bool WaitPoweronByifconfig(XDC01Serial xDC01Serial, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-等待设备开机...");
                int numa = Environment.TickCount;
                string str_error_log = "";
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > (int)decimal.Parse(testParam.poweron_delay) * 1000)
                    {
                        logger.ShowLog($"等待开机超时");
                        return false;
                    }
                    if (xDC01Serial.CheckIfconfig(ref str_error_log))
                    {
                        break;
                    }
                    else
                    {
                        Delay(2000);
                    }
                }
                logger.ShowLog("设备已开机");
                return true;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"等待开机发生异常：[{ee.Message}]");
                return false;
            }
        }

        public bool WaitPoweronRTOS(XDC01Serial xDC01Serial, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-等待设备开机...");
                string str_error_log = "";
                int numa = Environment.TickCount;
                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    if (Environment.TickCount - numa > (int)decimal.Parse(testParam.poweron_delay) * 1000)
                    {
                        logger.ShowLog($"等待开机超时");
                        return false;
                    }
                    if (xDC01Serial.CheckPowerStatusRTOS(ref str_error_log))
                    {
                        break;
                    }
                    else
                    {
                        Delay(2000);
                    }
                }
                logger.ShowLog("设备已开机");
                return true;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"等待开机发生异常：[{ee.Message}]");
                return false;
            }
        }

        /// <summary>
        /// PCBA电压测试
        /// </summary>
        /// <returns></returns>
        public List<TestItem> PCBAVoltageTest(RelaySerial relaySerial, TDMSerial tDMVolSerial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行电压测试...");
                int start_time = Environment.TickCount;
                _dataGridView = dataGridView;
                if (!relaySerial.CheckStatus())
                {
                    logger.ShowLog($"继电器串口未连接");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    _dataGridView.Invoke((MethodInvoker)delegate
                    {
                        // 设置数据源
                        _dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器串口未连接", "FAIL", Duration.ToString("F2"));
                    });
                    return null;
                }
                if(!tDMVolSerial.CheckStatus())
                {
                    logger.ShowLog($"电压表串口未连接");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    _dataGridView.Invoke((MethodInvoker)delegate
                    {
                        // 设置数据源
                        _dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "电压表串口未连接", "FAIL", Duration.ToString("F2"));
                    });
                    return null;
                }

                #region 环境准备
                string str_error_log = "";
                //--------停止连续获取数据
                if (tDMVolSerial._vol_stop_continuous() == false)
                {
                    logger.ShowLog($"初始化电压表失败");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    //dataGridView.Invoke((Action)(() =>
                    //{
                    //    dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "电压表控制异常", "FAIL", Duration.ToString("F2"));
                    //}));
                    _dataGridView.Invoke((MethodInvoker)delegate
                    {
                        // 设置数据源
                        _dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "电压表控制异常", "FAIL", Duration.ToString("F2"));
                    });
                    return null;
                }
                else
                {
                    ////-------关闭所有继电器
                    //if (relaySerial.CloseAllRelay() == false)
                    //{
                    //    logger.ShowLog($"初始化继电器失败");
                    //    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    //    dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                    //    return null;
                    //}
                    //else
                    //{
                    //    Delay(200);
                    //    str_error_log = "";
                    //    int[] int_relay_status = new int[16];
                    //    if (relaySerial.GetRelayStatus(ref str_error_log, ref int_relay_status) == false)
                    //    {
                    //        logger.ShowLog($"获取继电器状态发生异常[{str_error_log}]");
                    //        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    //        dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                    //        return null;
                    //    }
                    //    for (int i = 0; i < 8; i++)
                    //    {
                    //        if (int_relay_status[i] != 0)
                    //        {
                    //            logger.ShowLog($"继电器状态错误：[全部断开后，通道{i}仍是打开状态]");
                    //            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    //            dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                    //            return null;
                    //        }
                    //    }
                    //}
                }
                #endregion

                List<TestItem> volTestItems = new List<TestItem>
                {
                    new TestItem() { Name = "voltage_mcu", NgItem = "voltage_mcu", MinValue = testSpecMin.vol_mcu, MaxValue = testSpecMax.vol_mcu },
                    new TestItem() { Name = "voltage_vcc_core", NgItem = "voltage_vcc_core", MinValue = testSpecMin.vol_vcc_core, MaxValue = testSpecMax.vol_vcc_core },
                    new TestItem() { Name = "voltage_sensor1", NgItem = "voltage_sensor1", MinValue = testSpecMin.vol_sensor1, MaxValue = testSpecMax.vol_sensor1 },
                    new TestItem() { Name = "voltage_sensor2", NgItem = "voltage_sensor2", MinValue = testSpecMin.vol_sensor2, MaxValue = testSpecMax.vol_sensor2 },
                    new TestItem() { Name = "voltage_rtc", NgItem = "voltage_rtc", MinValue = testSpecMin.vol_rtc, MaxValue = testSpecMax.vol_rtc },
                    new TestItem() { Name = "voltage_vcc", NgItem = "voltage_vcc", MinValue = testSpecMin.vol_vcc, MaxValue = testSpecMax.vol_vcc },
                    new TestItem() { Name = "voltage_wifi", NgItem = "voltage_wifi", MinValue = testSpecMin.vol_wifi, MaxValue = testSpecMax.vol_wifi },
                    new TestItem() { Name = "voltage_ddr", NgItem = "voltage_ddr", MinValue = testSpecMin.vol_ddr, MaxValue = testSpecMax.vol_ddr }
                };

                int itemNum = volTestItems.Count;

                int trigger_interval = 500;
                try
                {
                    trigger_interval = (int)decimal.Parse(testParam.trigger_relay_interval) * 1000;
                }
                catch(Exception ee)
                {
                    logger.ShowLog($"继电器切换间隔参数异常：[{ee.Message}]");
                }
                
                StringBuilder ng_items = new StringBuilder();
                for (int i = 0; i < itemNum; i++)
                {
                    TestItem testItem = volTestItems[i];
                    start_time = Environment.TickCount;
                    // 继电器开关序号
                    ushort relayNum = (ushort)(i+1);

                    if (relaySerial.TriggerRelaySingle(relayNum, true) == false)
                    {
                    }
                    Delay(300);
                    str_error_log = "";
                    int[] relay_status = new int[16];
                    if (relaySerial.GetRelayStatus(ref str_error_log, ref relay_status) == false)
                    {
                        logger.ShowLog($"获取继电器状态发生异常[{str_error_log}]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                        Delay(300);
                        continue;
                    }
                    if (relay_status[relayNum - 1] == 0)
                    {
                        logger.ShowLog($"继电器状态错误：[通道{relayNum}未打开]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                        Delay(300);
                        continue;
                    }
                    Delay(300);
                    float float_vol1 = 0;
                    // 读取电压表数据
                    if (tDMVolSerial._voltage_value(ref float_vol1) == false)
                    {
                        Delay(300);
                        if (tDMVolSerial._voltage_value(ref float_vol1) == false)
                        {
                            logger.ShowLog($"读取电压表数据失败");
                            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "电压表控制异常", "FAIL", Duration.ToString("F2"));
                        }
                    }
                    else
                    {
                        testItem.Value = float_vol1;
                        // 判断电压数据情况
                        if (testItem.Value> testItem.MaxValue || testItem.Value < testItem.MinValue)
                        {
                            logger.ShowLog($"---电压{testItem.Name}[{float_vol1:F3}V],测试失败");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"---电压{testItem.Name}[{float_vol1:F3}V],测试通过");
                            testItem.Result = "PASS";
                        }
                        // 关闭继电器
                        if (relaySerial.TriggerRelaySingle(relayNum, false) == false)
                        {
                        }

                        Delay(trigger_interval);  // 间隔0.5秒
                        testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, testItem.Standard, testItem.MinValue, testItem.MaxValue, testItem.Value, testItem.StrVal,testItem.Result, testItem.Duration.ToString("F2"));
                    }
                }

                return volTestItems;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PCBA电压测试发生异常：[{ee.Message}]");
                return null;
            }
            finally
            {
                //-------关闭所有继电器
                //Delay(200);
                //if (relaySerial.CloseAllRelay() == false)
                //{
                //    logger.ShowLog($"继电器恢复失败");
                //}
            }
        }

        /// <summary>
        /// 工作电流读取
        /// </summary>
        /// <returns></returns>
        public TestItem PCBAWorkCurrentTest(TDMSerial tDMCurSerial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin)
        {
            try
            {
                logger.ShowLog("-进行工作电流测试...");
                int start_time = Environment.TickCount;
                if (!tDMCurSerial.CheckStatus())
                {
                    logger.ShowLog($"电流表串口未连接");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取工作电流", "-", "-", "-", "-", "电流表串口未连接", "FAIL", Duration.ToString("F2"));
                    return null;
                }

                TestItem testItem = new TestItem() { 
                    Name = "工作电流", 
                    NgItem = "work_current",
                    Standard = "-",
                    MinValue = testSpecMin.work_current, 
                    MaxValue = testSpecMax.work_current,
                    StrVal = "-"
                };
                //--------停止连续获取数据
                if (tDMCurSerial._vol_stop_continuous() == false)
                {
                    logger.ShowLog($"初始化电流表失败");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取工作电流", "-", "-", "-", "-", "电流表控制异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    Delay(300);
                    float float_current = 0;
                    // 读取电流表数据
                    if (tDMCurSerial._voltage_value(ref float_current) == false)
                    {
                        logger.ShowLog($"读取电流表数据失败");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("读取工作电流", "-", "-", "-", "-", "电流表控制异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        float_current *= 1000;
                        testItem.Value = float_current;
                        // 判断电流数据情况
                        if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                        {
                            logger.ShowLog($"---工作电流[{float_current.ToString("F3")}mA],测试失败");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"---工作电流[{float_current.ToString("F3")}mA],测试通过");
                            testItem.Result = "PASS";
                        }
                        testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, testItem.Standard, testItem.MinValue, testItem.MaxValue, testItem.Value, testItem.StrVal, testItem.Result, testItem.Duration.ToString("F2"));
                        return testItem;
                    }
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PCBA工作电流测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 漏电流读取
        /// </summary>
        /// <returns></returns>
        public TestItem PCBAStandbyCurrentTest(FlukeSerial flukeSerial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin)
        {
            try
            {
                logger.ShowLog("-进行漏电流测试...");
                int start_time = Environment.TickCount;
                if (!flukeSerial.CheckStatus())
                {
                    logger.ShowLog($"FLUKE电流表串口未连接");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取漏电流", "-", "-", "-", "-", "FLUKE电流表串口未连接", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                string str_error_log = "";
                TestItem testItem = new TestItem()
                {
                    Name = "漏电流",
                    NgItem = "standby_current",
                    Standard = "-",
                    MinValue = testSpecMin.standby_current,
                    MaxValue = testSpecMax.standby_current,
                    StrVal = "-"
                };
                //--------切换量程为200uA
                if (flukeSerial.SetRange("1", ref str_error_log) == false)
                {
                    logger.ShowLog($"FLUKE电流表切换量程为200uA失败");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取漏电流", "-", "-", "-", "-", "FLUKE电流表控制异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    Delay(2000);
                    float max_current = 0.00f;
                    //int count = int.Parse(_param_run.str_current_count);
                    for (int i = 0; i < 10; i++)
                    {
                        string current_value = "";
                        string unit = "";
                        string error_log_current = "";
                        if (flukeSerial.GetCurrent(ref current_value, ref unit, ref error_log_current))
                        {
                            Console.WriteLine(current_value);
                            float f_current_value = float.Parse(current_value);
                            if (max_current <= f_current_value)
                            {
                                max_current = f_current_value;
                            }
                        }
                    }
                    testItem.Value = max_current;
                    // 判断电流数据情况
                    if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                    {
                        logger.ShowLog($"---漏电流[{max_current:F3}uA],测试失败");
                        testItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"---漏电流[{max_current:F3}uA],测试通过");
                        testItem.Result = "PASS";
                    }
                    testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(testItem.Name, testItem.Standard, testItem.MinValue, testItem.MaxValue, testItem.Value, testItem.StrVal, testItem.Result, testItem.Duration.ToString("F2"));
                    return testItem;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PCBA漏电流测试发生异常：[{ee.Message}]");
                return null;
            }
            finally
            {
                if (flukeSerial.CheckStatus())
                {
                    string str_error_log = "";
                    // 切换量程为20mA
                    logger.ShowLog("万用表切换量程为200mA");
                    if (flukeSerial.SetRange("4", ref str_error_log) == false)
                    {
                        logger.ShowLog($"FLUKE电流表切换量程为200mA失败");
                    }
                    else
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 充电电流读取
        /// </summary>
        /// <returns></returns>
        public TestItem PCBAChargeCurrentTest(FlukeSerial flukeSerial, RelaySerial relaySerial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行充电电流测试...");
                int start_time = Environment.TickCount;
                if (!flukeSerial.CheckStatus())
                {
                    logger.ShowLog($"FLUKE电流表串口未连接");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取漏电流", "-", "-", "-", "-", "FLUKE电流表串口未连接", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                string str_error_log = "";
                TestItem testItem = new TestItem()
                {
                    Name = "充电电流",
                    NgItem = "charge_current",
                    Standard = "-",
                    MinValue = testSpecMin.charge_current,
                    MaxValue = testSpecMax.charge_current,
                    StrVal = "-"
                };

                float max_current = 0.00f;
                //int count = int.Parse(_param_run.str_current_count);
                for (int i = 0; i < 10; i++)
                {
                    string current_value = "";
                    string unit = "";
                    string error_log_current = "";
                    if (flukeSerial.GetCurrent(ref current_value, ref unit, ref error_log_current))
                    {
                        Console.WriteLine(current_value);
                        float f_current_value = Math.Abs(float.Parse(current_value));
                        if (max_current <= f_current_value)
                        {
                            max_current = f_current_value;
                        }
                    }
                }
                testItem.Value = max_current;
                // 判断电流数据情况
                if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                {
                    logger.ShowLog($"---充电电流[{max_current:F3}mA],测试失败");
                    testItem.Result = "FAIL";
                }
                else
                {
                    logger.ShowLog($"---充电电流[{max_current:F3}mA],测试通过");
                    testItem.Result = "PASS";
                }

                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, testItem.Standard, testItem.MinValue, testItem.MaxValue, testItem.Value, testItem.StrVal, testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;

                /*// 切换量程为20mA
                logger.ShowLog("万用表切换量程为200mA");
                if (flukeSerial.SetRange("4", ref str_error_log) == false)
                {
                    logger.ShowLog($"FLUKE电流表切换量程为200mA失败");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取充电电流", "-", "-", "-", "-", "FLUKE电流表控制异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    

                    *//*// 继电器开关序号
                    ushort relayNum = ushort.Parse(testParam.charge_relay_index);

                    if (relaySerial.TriggerRelaySingle(relayNum, true) == false)
                    {
                    }
                    str_error_log = "";
                    int[] relay_status = new int[16];
                    if (relaySerial.GetRelayStatus(ref str_error_log, ref relay_status) == false)
                    {
                        logger.ShowLog($"获取继电器状态发生异常[{str_error_log}]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                    }
                    if (relay_status[relayNum - 1] == 0)
                    {
                        logger.ShowLog($"继电器状态错误：[通道{relayNum}未打开]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                    }

                    // 提示打开开关
                    CustomDialog customDialog = new CustomDialog("充电电流测试", "请打开治具上的开关，给设备上电");
                    DialogResult result = customDialog.ShowDialog();

                    if (result == DialogResult.Yes)
                    {
                        logger.ShowLog($"--- 已打开上电开关");

                        Delay(3000);

                        float max_current = 0.00f;
                        //int count = int.Parse(_param_run.str_current_count);
                        for (int i = 0; i < 10; i++)
                        {
                            string current_value = "";
                            string unit = "";
                            string error_log_current = "";
                            if (flukeSerial.GetCurrent(ref current_value, ref unit, ref error_log_current))
                            {
                                Console.WriteLine(current_value);
                                float f_current_value = float.Parse(current_value);
                                if (max_current <= f_current_value)
                                {
                                    max_current = f_current_value;
                                }
                            }
                        }
                        testItem.Value = max_current;
                        // 判断电流数据情况
                        if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                        {
                            logger.ShowLog($"---充电电流[{max_current:F3}mA],测试失败");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"---充电电流[{max_current:F3}mA],测试通过");
                            testItem.Result = "PASS";
                        }
                    }
                    else
                    {
                        logger.ShowLog($"--- 未打开上电开关");
                        testItem.Result = "FAIL";
                        testItem.StrVal = "未打开上电开关";
                    }
                    *//*
                    
                }*/
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PCBA充电电流测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        public List<TestItem> CheckRNandTagname(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam, TestStandard testStandard)
        {
            try
            {
                logger.ShowLog("-读取RN和工序号...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                // 1、读取RN
                string str_rn = "";
                if (xDC01Serial.GetRN(ref str_rn, ref str_error_log) == false)
                {
                    logger.ShowLog($"--- 读RN失败：{str_error_log}");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取RN号", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    TestItem RNtestItem = new TestItem
                    {
                        Name = "RN号",
                        NgItem = "rn",
                        Standard = $"{testStandard.rn_length}位",
                        StrVal = str_rn
                    };
                    // RN号格式检查
                    if (str_rn.Length != testStandard.rn_length)
                    {
                        logger.ShowLog("--- Rn长度错误...");
                        RNtestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"读取到RN号为{str_rn}");
                        RNtestItem.Result = "PASS";
                    }
                    RNtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(RNtestItem.Name, RNtestItem.Standard, "-", "-", "-", RNtestItem.StrVal, RNtestItem.Result, RNtestItem.Duration.ToString("F2"));
                    testItems.Add(RNtestItem);
                }

                // 2、读取测试工序号tagNumber
                start_time = Environment.TickCount;
                if (testParam.test_mode == "operator")
                {
                    string str_tagNumber = "";
                    if (xDC01Serial.GetTagNumber(ref str_tagNumber, ref str_error_log) == false)
                    {
                        logger.ShowLog($"--- 读工序号失败：{str_error_log}");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("读取工序号", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        TestItem tagNumbertestItem = new TestItem()
                        {
                            Name = "工序号",
                            NgItem = "tagnumber",
                            Standard = $"{testParam.cur_tagnumber}"
                            //Value = float.Parse(cameraInfo.Battery)
                        };
                        tagNumbertestItem.StrVal = str_tagNumber;
                        if (str_tagNumber != testParam.cur_tagnumber)
                        {
                            logger.ShowLog($"--- 工序号[{str_tagNumber}]与预期[{testParam.cur_tagnumber}]不匹配");
                            tagNumbertestItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"--- 读取到工序号[{str_tagNumber}]符合工序控制");
                            tagNumbertestItem.Result = "PASS";
                        }
                        tagNumbertestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(tagNumbertestItem.Name, tagNumbertestItem.Standard, "-", "-", "-", tagNumbertestItem.StrVal, tagNumbertestItem.Result, tagNumbertestItem.Duration.ToString("F2"));
                        testItems.Add(tagNumbertestItem);
                    }
                }
                else
                {
                    logger.ShowLog("--- 工程师测试模式，不读工序号");
                }
                return testItems;
            }
            catch (Exception e)
            {

                logger.ShowLog($"RN和工序号检查发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// Wifi联网测试-ping
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="str_ip"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        /// <returns></returns>
        public TestItem CheckWiFi(XDC01Serial xDC01Serial, System.Windows.Forms.DataGridView dataGridView, Logger logger, 
            TestSpecMax testSpecMax, TestSpecMin testSpecMin, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行Ping网测试...");
                int start_time = Environment.TickCount;
                if (testParam.ping_ip == "")
                {
                    logger.ShowLog($"必须指定要ping的IP");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("Ping网测试", "-", "-", "-", "-", "未指定IP", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    string ip = "";
                    string str_error_log = "";
                    if (xDC01Serial.GetSystemIP(ref ip, ref str_error_log) == false)
                    {
                        logger.ShowLog($"读取IP失败：{str_error_log}");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("Ping网测试", "-", "-", "-", "-", "设备WiFi连接异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }

                    TestItem testItem = new TestItem() { 
                        Name = "Ping网测试", 
                        NgItem = "ping_rtt", 
                        MinValue = testSpecMin.ping_rtt, 
                        MaxValue = testSpecMax.ping_rtt 
                    };
                    
                    string str_rtt = "";
                    if (xDC01Serial.TestPingDelay(ref str_rtt, testParam.ping_ip, ref str_error_log, int.Parse(testParam.ping_count)) == false)
                    {
                        logger.ShowLog($"ping网测试异常：[{str_error_log}]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("Ping网测试", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        try
                        {
                            testItem.Value = float.Parse(str_rtt);
                        }
                        catch (Exception ee)
                        {
                            logger.ShowLog($"数值转换异常：[{ee.Message}]");
                            testItem.Value = -1;
                        }
                        if (testItem.Value > testItem.MaxValue)
                        {
                            logger.ShowLog($"--- ping测试失败 [{testItem.Value}ms]");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"--- ping测试通过 [{testItem.Value}ms]");
                            testItem.Result = "PASS";
                        }
                        testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", testItem.MinValue, testItem.MaxValue, testItem.Value, "-", testItem.Result, testItem.Duration.ToString("F2"));
                        return testItem;
                    }
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"WiFi联网测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 麦克风测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="pcCommand"></param>
        /// <param name="axWindowsMediaPlayer1"></param>
        /// <param name="duration"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TestItem CheckMicrophone(XDC01Serial xDC01Serial, System.Windows.Forms.DataGridView dataGridView, Logger logger,
            PCCommand pcCommand, AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行麦克风测试...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";

                // 打开麦克风
                xDC01Serial.OpenMic(ref str_error_log);
                // 启动录音
                xDC01Serial.RecordMic(testParam.mic_record_duration, ref str_error_log);
                // PC播放音频文件
                logger.ShowLog("PC开始播放音频");
                pcCommand.PlayWavFile(axWindowsMediaPlayer1, "", true);
                // 播放
                Delay(int.Parse(testParam.mic_record_duration)*1000);
                // PC停止播放音频文件
                logger.ShowLog("PC停止播放音频");
                pcCommand.PlayWavFile(axWindowsMediaPlayer1, "", false);
                // 关闭麦克风
                xDC01Serial.CloseMic(ref str_error_log);

                TestItem MicTestItem = new TestItem()
                {
                    Name = "麦克风测试",
                    NgItem = "mic_test",
                    StrVal = ""
                };

                bool AutoPass = false;    // 用于控制自动加人工时，自动通过，不再进行人工测试
                string auto_data = "";
                if(testParam.mic_test_mode.Contains("Auto"))   // 自动判断
                {
                    logger.ShowLog("-- 当前为自动判断方式...");
                    string str_max_abs = "";
                    string str_delta = "";
                    string str_result = "";
                    xDC01Serial.TestRecordWav(ref str_max_abs, ref str_delta, ref str_result, ref str_error_log);

                    auto_data = MicTestItem.StrVal = $"max_abs:{str_max_abs},delta:{str_delta},result:{str_result}";

                    if (str_result == "True")
                    {
                        logger.ShowLog($"--- 麦克风测试(自动)通过");
                        AutoPass = true;
                        // 自动判断通过，麦克风测试就通过
                        MicTestItem.Result = "PASS";
                        MicTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(MicTestItem.Name, "-", "-", "-", "-", MicTestItem.StrVal, MicTestItem.Result, MicTestItem.Duration.ToString("F2"));
                        return MicTestItem;
                    }
                    else
                    {
                        logger.ShowLog($"--- 麦克风测试(自动)失败");
                    }
                }
                if(!AutoPass && testParam.mic_test_mode.Contains("Manual"))    // 人工判断
                {
                    logger.ShowLog("-- 当前为人工判断方式...");
                    logger.ShowLog("--- 请检查播放的录音声音，音质正常OK请按[PASS]，音质有问题NG请按[FAIL]");
                    // 播放录音文件
                    xDC01Serial.PlayRecordWav(ref str_error_log);
                    Delay(int.Parse(testParam.mic_record_duration));
                    CustomDialog customDialog = new CustomDialog("麦克风测试（人工）", "麦克风录音是否正常？");
                    DialogResult result = customDialog.ShowDialog();

                    if (result == DialogResult.Yes)
                    {
                        logger.ShowLog($"--- 麦克风测试(人工)通过");
                        MicTestItem.StrVal = "Manual_Pass";
                        MicTestItem.Result = "PASS";
                    }
                    else
                    {
                        logger.ShowLog($"--- 麦克风测试(人工)失败");
                        MicTestItem.StrVal = "Manual_Fail";
                        MicTestItem.Result = "FAIL";
                    }

                    if(auto_data.Length > 0)
                    {
                        MicTestItem.StrVal += $"|{auto_data}";
                    }

                    MicTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(MicTestItem.Name, "-", "-", "-", "-", MicTestItem.StrVal, MicTestItem.Result, MicTestItem.Duration.ToString("F2"));
                    return MicTestItem;
                }
                else
                {
                    logger.ShowLog("当前未设置人工判断，自动判断不通过，麦克风测试不通过");
                    MicTestItem.Result = "FAIL";
                    MicTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(MicTestItem.Name, "-", "-", "-", "-", MicTestItem.StrVal, MicTestItem.Result, MicTestItem.Duration.ToString("F2"));
                    return MicTestItem;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"麦克风测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 整机 喇叭播放音频测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testParam"></param>
        /// <returns></returns>
        public TestItem CheckAudioManual(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行喇叭功能测试(人工)...");
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "喇叭功能测试(人工)",
                    NgItem = "audio"
                };
                string str_error_log = "";
                logger.ShowLog("--- 请检查播放的音频声音，音质正常OK请按[PASS]，音质有问题NG请按[FAIL]");
                // 播放音频文件
                if (testParam.wav_1 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.1");
                    if (xDC01Serial.PlayWav("1", ref str_error_log) == false)
                    {
                        str_error_log = "audio1";
                    }
                    Delay(500);
                }
                if (testParam.wav_2 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.2");
                    if (xDC01Serial.PlayWav("2", ref str_error_log) == false)
                    {
                        str_error_log = "audio2";
                    }
                    Delay(500);
                }

                if (testParam.wav_3 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.3");
                    if (xDC01Serial.PlayWav("3", ref str_error_log) == false)
                    {
                        str_error_log = "audio3";
                    }
                    Delay(500);
                }
                if (testParam.wav_4 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.4");
                    if (xDC01Serial.PlayWav("4", ref str_error_log) == false)
                    {
                        str_error_log = "audio4";
                    }
                    Delay(500);
                }
                if (testParam.wav_5 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.5");
                    if (xDC01Serial.PlayWav("5", ref str_error_log) == false)
                    {
                        str_error_log = "audio5";
                    }
                    Delay(500);
                }
                if (testParam.wav_8 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.8");
                    if (xDC01Serial.PlayWav("8", ref str_error_log) == false)
                    {
                        str_error_log = "audio8";
                    }
                    Delay(500);
                }
                if (testParam.wav_9 == "True")
                {
                    logger.ShowLog("--- 设备正在播放音频 wav.9");
                    if (xDC01Serial.PlayWav("9", ref str_error_log) == false)
                    {
                        str_error_log = "audio9";
                    }
                    Delay(500);
                }

                // 人工判断
                CustomDialog customDialog = new CustomDialog("喇叭功能测试（人工）", "喇叭播放音频是否正常？");
                DialogResult result = customDialog.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 喇叭功能测试(人工)通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 喇叭功能测试(人工)失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"喇叭功能测试(人工)发生异常：[{ee.Message}]");
                return null;
            }
        }

        public TestItem CheckButton(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行按键功能测试...");
                int start_time = Environment.TickCount;
                string str_error_log = "";

                int interval = 5000;
                try
                {
                    decimal temp = decimal.Parse(testParam.btn_timeout) * 1000;
                    interval = (int)temp;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"按键测试超时参数异常：[{ee.Message}]");
                }

                TestItem btnTestItem = new TestItem()
                {
                    Name = "按键测试",
                    NgItem = "button"
                };
                xDC01Serial.SetPIR("off", ref str_error_log);
                CustomDialog btnDialog = new CustomDialog("按键测试", "请按键！", false, false);
                //DialogResult result = btnDialog.ShowDialog();
                btnDialog.Show();

                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > interval)
                    {
                        logger.ShowLog($"--- 按键功能测试失败(超时)");
                        btnTestItem.Result = "FAIL";
                        break;
                    }
                    if (xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonStatus = 1") && xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonRelease Trigger = 1"))
                    {
                        logger.ShowLog($"--- 按键功能测试通过");
                        btnTestItem.Result = "PASS";
                        break;
                    }
                }
                btnDialog.Close();

                btnTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(btnTestItem.Name, "-", "-", "-", "-", "-", btnTestItem.Result, btnTestItem.Duration.ToString("F2"));
                return btnTestItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"按键测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// PCBA 按键功能测试and喇叭测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public List<TestItem> CheckButtonAndSpeaker(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行按键功能测试...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";

                int interval = 5000;
                try
                {
                    decimal temp = decimal.Parse(testParam.btn_timeout) * 1000;
                    interval = (int)temp;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"按键测试超时参数异常：[{ee.Message}]");
                }

                TestItem btnTestItem = new TestItem() { 
                    Name = "按键测试", 
                    NgItem = "button" 
                };
                xDC01Serial.SetPIR("off", ref str_error_log);
                CustomDialog btnDialog = new CustomDialog("按键测试", "请按键！", false, false);
                //DialogResult result = btnDialog.ShowDialog();
                btnDialog.Show();

                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > interval)
                    {
                        logger.ShowLog($"--- 按键功能测试失败(超时)");
                        btnTestItem.Result = "FAIL";
                        break;
                    }
                    if (xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonStatus = 1") && xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonRelease Trigger = 1"))
                    {
                        logger.ShowLog($"--- 按键功能测试通过");
                        btnTestItem.Result = "PASS";
                        break;
                    }
                }
                btnDialog.Close();

                /*if (result == DialogResult.Yes)
                {
                    Delay(1000);
                    if (xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonStatus = 1") && xDC01Serial.str_Receive_skybell.Contains("SystemParam.ButtonRelease Trigger = 1"))
                    {
                        logger.ShowLog($"--- 按键功能测试通过");
                        btnTestItem.Result = "PASS";
                    }
                    else
                    {
                        logger.ShowLog($"--- 按键功能测试失败");
                        btnTestItem.Result = "FAIL";
                    }
                }
                else
                {
                    logger.ShowLog($"--- 按键功能测试失败(人工选择NO)");
                    btnTestItem.Result = "FAIL";
                }*/
                btnTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(btnTestItem.Name, "-", "-", "-", "-", "-", btnTestItem.Result, btnTestItem.Duration.ToString("F2"));
                testItems.Add(btnTestItem);

                start_time = Environment.TickCount;
                // 按键铃声作为喇叭测试依据，人工判断
                logger.ShowLog("-进行喇叭测试...");
                TestItem speakerTestItem = new TestItem() { 
                    Name = "喇叭测试", 
                    NgItem = "audio" 
                };
                CustomDialog speakerDialog = new CustomDialog("喇叭测试", "按键是否响铃？");
                DialogResult SpeakerResult = speakerDialog.ShowDialog();

                if (SpeakerResult == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 喇叭测试(人工)通过");
                    speakerTestItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 喇叭测试(人工)失败");
                    speakerTestItem.Result = "FAIL";
                }
                speakerTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(speakerTestItem.Name, "-", "-", "-", "-", "-", speakerTestItem.Result, speakerTestItem.Duration.ToString("F2"));
                testItems.Add(speakerTestItem);

                return testItems;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"按键测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 移动感应测试
        /// </summary>
        /// <returns></returns>
        public TestItem CheckPIRMotion(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                int start_time = Environment.TickCount;
                string str_error_log = "";
                logger.ShowLog("-进行移动感应PIR测试...");

                int interval = 5000;
                try
                {
                    decimal temp = decimal.Parse(testParam.pir_timeout) * 1000;
                    interval = (int)temp;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"移动感应测试超时参数异常：[{ee.Message}]");
                }

                TestItem testItem = new TestItem(){ 
                    Name = "移动感应", 
                    NgItem = "motion" 
                };
                xDC01Serial.SetPIR("on", ref str_error_log);
                CustomDialog pirbtnDialog = new CustomDialog("移动感应PIR测试", "请在PIR上方挥手!", false, false);
                pirbtnDialog.Show();

                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > interval)
                    {
                        logger.ShowLog($"--- 移动感应功能测试失败(超时)");
                        testItem.Result = "FAIL";
                        break;
                    }
                    if (xDC01Serial.str_Receive_skybell.Contains("SystemParam.MotionTriggerEvent = 1 service_up 0"))
                    {
                        logger.ShowLog($"--- 移动感应功能测试通过");
                        testItem.Result = "PASS";
                        break;
                    }
                }
                pirbtnDialog.Close();

                /*DialogResult result = btnDialog.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    if (xDC01Serial.str_Receive_skybell.Contains("SystemParam.MotionTriggerEvent = 1 service_up 0"))
                    {
                        logger.ShowLog($"--- 移动感应功能测试通过");
                        testItem.Result = "PASS";
                    }
                    else
                    {
                        logger.ShowLog($"--- 移动感应功能测试失败");
                        testItem.Result = "FAIL";
                    }
                }
                else
                {
                    logger.ShowLog($"--- 移动感应功能测试失败(人工选择NO)");
                    testItem.Result = "FAIL";
                }*/
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PIR测试发生异常：[{ee.Message}]");
                return null;
            }
            finally
            {
                string str_error_log = "";
                xDC01Serial.SetPIR("off", ref str_error_log);
            }
        }

        /// <summary>
        /// LED灯颜色测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="interval"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TestItem CheckLEDColor(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行LED灯颜色测试...");
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "LED颜色测试", 
                    NgItem = "led_color" 
                };
                string str_error_log = "";
                logger.ShowLog("--- 请检查LED灯颜色");
                int interval = 1000;
                try
                {
                    decimal temp = decimal.Parse(testParam.led_interval) * 1000;
                    interval = (int)temp;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"LED切换间隔参数异常：[{ee.Message}]");
                }

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                Task ledTask = Task.Run(() =>
                {
                    // 循环控制LED灯颜色，监测人工是否完成判定
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Application.DoEvents();

                        // 红色
                        if (xDC01Serial.SetBtnLEDColor("red", ref str_error_log) == false)
                        {

                        }
                        // 检查取消标记
                        if (cancellationToken.IsCancellationRequested)
                        {
                            // 在需要停止任务的地方进行清理或处理
                            Console.WriteLine("任务被取消");
                            break;
                        }
                        Delay(interval);
                        // 绿色
                        if (xDC01Serial.SetBtnLEDColor("green", ref str_error_log) == false)
                        {

                        }
                        // 检查取消标记
                        if (cancellationToken.IsCancellationRequested)
                        {
                            // 在需要停止任务的地方进行清理或处理
                            Console.WriteLine("任务被取消");
                            break;
                        }
                        Delay(interval);
                        // 蓝色
                        if (xDC01Serial.SetBtnLEDColor("blue", ref str_error_log) == false)
                        {

                        }
                        // 检查取消标记
                        if (cancellationToken.IsCancellationRequested)
                        {
                            // 在需要停止任务的地方进行清理或处理
                            Console.WriteLine("任务被取消");
                            break;
                        }
                        Delay(interval);
                        // 白色
                        if (xDC01Serial.SetBtnLEDColor("white", ref str_error_log) == false)
                        {

                        }
                        // 检查取消标记
                        if (cancellationToken.IsCancellationRequested)
                        {
                            // 在需要停止任务的地方进行清理或处理
                            Console.WriteLine("任务被取消");
                            break;
                        }
                        Delay(interval);
                    }
                }).ContinueWith(task => {
                    xDC01Serial.SetBtnLEDColor("white", ref str_error_log);
                });

                CustomDialog customDialog = new CustomDialog("LED灯检查测试（人工）", "是否依次红、绿、蓝、白四种颜色？");
                DialogResult result = customDialog.ShowDialog();

                // 停止切换灯光
                cancellationTokenSource.Cancel();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- LED灯检查测试(人工)通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- LED灯检查测试(人工)失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"LED灯颜色测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// VLC视频检查\IR_LED\IR_CUT
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public List<TestItem> CheckRTSP(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                logger.ShowLog("-进行VLC RTSP Video测试...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                string ip = "";
                TestItem vlcTestItem = new TestItem() { 
                    Name = "RTSP视频检查", 
                    NgItem = "vlc_rtsp" 
                };
                if (xDC01Serial.GetSystemIP(ref ip, ref str_error_log) == false)
                {
                    logger.ShowLog($"读取IP失败：{str_error_log}");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("RTSP视频检查", "-", "-", "-", "-", "设备WiFi连接异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    logger.ShowLog($"Camera的IP为[{ip}]");
                    // 打开实时影像
                    if (xDC01Serial.OpenRTSP(ref str_error_log) == false)
                    {
                        logger.ShowLog($"打开实时影像失败:{str_error_log}");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("RTSP视频检查", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        FormVLC vLC = new FormVLC($"rtsp://{ip}/stream0");
                        vLC.Show();

                        Delay(3000);
                        CustomDialog customDialog = new CustomDialog("VLC视频检查测试（人工）", "是否正常出现摄像头画面？", true);
                        DialogResult result = customDialog.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            logger.ShowLog($"--- VLC视频检查测试(人工)通过");
                            vlcTestItem.Result = "PASS";
                        }
                        else
                        {
                            logger.ShowLog($"--- VLC视频检查测试(人工)失败");
                            vlcTestItem.Result = "FAIL";
                        }
                        vlcTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(vlcTestItem.Name, "-", "-", "-", "-", "-", vlcTestItem.Result, vlcTestItem.Duration.ToString("F2"));
                        testItems.Add(vlcTestItem);

                        start_time = Environment.TickCount;
                        //------12 通过vlc video确认IR Cut IR LED
                        Delay(500);
                        TestItem IRLedTestItem = new TestItem() { 
                            Name = "IR_LED", 
                            NgItem = "ir_led" 
                        };
                        str_error_log = "";
                        if (xDC01Serial.SwitchIR_CUT("on", ref str_error_log) == false)
                        {
                            logger.ShowLog($"切换夜视模式异常：[{str_error_log}]");
                        }
                        logger.ShowLog("--- 请检查IR_LED灯功能");
                        CustomDialog IRLedDialog = new CustomDialog("IR_LED检查测试（人工）", "请检查六颗红外灯是否全亮？", true);
                        DialogResult IRLedresult = IRLedDialog.ShowDialog();

                        if (IRLedresult == DialogResult.Yes)
                        {
                            logger.ShowLog($"--- IR_LED检查测试(人工)通过");
                            IRLedTestItem.Result = "PASS";
                        }
                        else
                        {
                            logger.ShowLog($"--- IR_LED检查测试(人工)失败");
                            IRLedTestItem.Result = "FAIL";
                        }
                        IRLedTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(IRLedTestItem.Name, "-", "-", "-", "-", "-", IRLedTestItem.Result, IRLedTestItem.Duration.ToString("F2"));
                        testItems.Add(IRLedTestItem);

                        start_time = Environment.TickCount;
                        TestItem IRCutTestItem = new TestItem() { 
                            Name = "IR_CUT", 
                            NgItem = "ir_cut" 
                        };
                        logger.ShowLog("--- 请检查IR_CUT夜视功能");
                        CustomDialog IRCutDialog = new CustomDialog("IR_CUT检查测试（人工）", "是否已切换到夜视模式？", true);
                        DialogResult IRCutresult = IRCutDialog.ShowDialog();

                        if (IRCutresult == DialogResult.Yes)
                        {
                            logger.ShowLog($"--- IR_CUT检查测试(人工)通过");
                            IRCutTestItem.Result = "PASS";
                        }
                        else
                        {
                            logger.ShowLog($"--- IR_CUT检查测试(人工)失败");
                            IRCutTestItem.Result = "FAIL";
                        }
                        IRCutTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(IRCutTestItem.Name, "-", "-", "-", "-", "-", IRCutTestItem.Result, IRCutTestItem.Duration.ToString("F2"));
                        testItems.Add(IRCutTestItem);
                        vLC.Close();

                        return testItems;
                    }
                }
                
            }
            catch (Exception ee)
            {
                logger.ShowLog($"检查实时影像，发生异常：{ee.Message}");
                return null;
            }
            finally
            {
                string str_error_log = "";
                if (xDC01Serial.SwitchIR_CUT("off", ref str_error_log) == false)
                {
                    logger.ShowLog($"关闭夜视发生异常[{str_error_log}]");
                }
                else
                {
                    logger.ShowLog($"已关闭夜视");
                }
            }
        }

        /// <summary>
        /// Light sensor亮度值测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        /// <returns></returns>
        public TestItem CheckLightSensor(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin)
        {
            try
            {
                logger.ShowLog("-进行Light Sensor亮度值检查...");
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem() { Name = "亮度值", NgItem = "light", MinValue = testSpecMin.light_val, MaxValue = testSpecMax.light_val };
                string str_error_log = "";
                string str_lightsensor = "";
                if (xDC01Serial.GetLightSensor(ref str_lightsensor, ref str_error_log) == false)
                {
                    logger.ShowLog("--- 读取亮度值失败：" + str_error_log);
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取亮度值", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    testItem.Value = int.Parse(str_lightsensor);
                    if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                    {
                        logger.ShowLog($"--- 亮度值[{testItem.Value}]测试失败");
                        testItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- 亮度值[{testItem.Value}]测试通过");
                        testItem.Result = "PASS";
                    }
                    testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(testItem.Name, "-", testItem.MinValue, testItem.MaxValue, testItem.Value, "-", testItem.Result, testItem.Duration.ToString("F2"));
                    return testItem;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"DUT Light sensor测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 读取设备信息
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        /// <param name="testStandard"></param>
        /// <returns></returns>
        public List<TestItem> CheckDUTInfo(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin, TestStandard testStandard)
        {
            try
            {
                logger.ShowLog("-进行设备信息检查...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                CameraInfo cameraInfo = new CameraInfo();
                if (xDC01Serial.GetSystemParam(ref cameraInfo, ref str_error_log) == false)
                {
                    logger.ShowLog("--- 读取设备信息失败：" + str_error_log);
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取设备信息", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    // 固件firmware版本
                    TestItem FWtestItem = new TestItem
                    {
                        Name = "固件版本",
                        NgItem = "fw_version",
                        Standard = testStandard.firmware,
                        StrVal = cameraInfo.FwVersion
                    };
                    if (FWtestItem.Standard != FWtestItem.StrVal)
                    {
                        logger.ShowLog($"--- 固件版本测试失败");
                        FWtestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- 固件版本测试通过");
                        FWtestItem.Result = "PASS";
                    }
                    FWtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(FWtestItem.Name, FWtestItem.Standard, "-", "-", "-", FWtestItem.StrVal, FWtestItem.Result, FWtestItem.Duration.ToString("F2"));
                    testItems.Add(FWtestItem);

                    // 硬件版本
                    start_time = Environment.TickCount;
                    TestItem HWtestItem = new TestItem() { 
                        Name = "硬件版本", 
                        NgItem = "hw_version", 
                        Standard = testStandard.hardware, 
                        StrVal = cameraInfo.HwVersion };
                    if (HWtestItem.Standard != HWtestItem.StrVal)
                    {
                        logger.ShowLog($"--- 硬件版本测试失败");
                        HWtestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- 硬件版本测试通过");
                        HWtestItem.Result = "PASS";
                    }
                    HWtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(HWtestItem.Name, HWtestItem.Standard, "-", "-", "-", HWtestItem.StrVal, HWtestItem.Result, HWtestItem.Duration.ToString("F2"));
                    testItems.Add(HWtestItem);

                    // MCU版本
                    start_time = Environment.TickCount;
                    TestItem MCUtestItem = new TestItem() { 
                        Name = "MCU版本", 
                        NgItem = "mcu_version", 
                        Standard = testStandard.mcu, 
                        StrVal = cameraInfo.McuAppV };
                    if (MCUtestItem.Standard != MCUtestItem.StrVal)
                    {
                        logger.ShowLog($"--- MCU版本测试失败");
                        MCUtestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- MCU版本测试通过");
                        MCUtestItem.Result = "PASS";
                    }
                    MCUtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(MCUtestItem.Name, MCUtestItem.Standard, "-", "-", "-", MCUtestItem.StrVal, MCUtestItem.Result, MCUtestItem.Duration.ToString("F2"));
                    testItems.Add(MCUtestItem);

                    // 电池电压
                    start_time = Environment.TickCount;
                    TestItem BatteryVoltestItem = new TestItem
                    {
                        Name = "电池电压",
                        NgItem = "battery_voltage",
                        MinValue = testSpecMin.battery_voltage,
                        MaxValue = testSpecMax.battery_voltage,
                        Value = float.Parse(cameraInfo.Battery) / 100.0f
                    };
                    // 判断电压数据情况
                    if (BatteryVoltestItem.Value > BatteryVoltestItem.MaxValue || BatteryVoltestItem.Value < BatteryVoltestItem.MinValue)
                    {
                        logger.ShowLog($"--- 电池电压{BatteryVoltestItem.Name}[{BatteryVoltestItem.Value:F3}V],测试失败");
                        BatteryVoltestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- 电池电压{BatteryVoltestItem.Name}[{BatteryVoltestItem.Value:F3}V],测试通过");
                        BatteryVoltestItem.Result = "PASS";
                    }
                    BatteryVoltestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(BatteryVoltestItem.Name, "-", BatteryVoltestItem.MinValue, BatteryVoltestItem.MaxValue, BatteryVoltestItem.Value, "-", BatteryVoltestItem.Result, BatteryVoltestItem.Duration.ToString("F2"));
                    testItems.Add(BatteryVoltestItem);

                    // CPU温度
                    start_time = Environment.TickCount;
                    TestItem CPUTempVoltestItem = new TestItem()
                    {
                        Name = "CPU温度",
                        NgItem = "cpu_temperature",
                        MinValue = testSpecMin.cpu_temperature,
                        MaxValue = testSpecMax.cpu_temperature,
                        //Value = float.Parse(cameraInfo.Battery)
                    };
                    CPUTempVoltestItem.Value = float.Parse(cameraInfo.Temp) / 10.0f;
                    // 判断CPU温度情况
                    if (CPUTempVoltestItem.Value > CPUTempVoltestItem.MaxValue || CPUTempVoltestItem.Value < CPUTempVoltestItem.MinValue)
                    {
                        logger.ShowLog($"--- CPU温度{CPUTempVoltestItem.Name}[{CPUTempVoltestItem.Value:F3}],测试失败");
                        CPUTempVoltestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"--- CPU温度{CPUTempVoltestItem.Name}[{CPUTempVoltestItem.Value:F3}],测试通过");
                        CPUTempVoltestItem.Result = "PASS";
                    }
                    CPUTempVoltestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(CPUTempVoltestItem.Name, "-", CPUTempVoltestItem.MinValue, CPUTempVoltestItem.MaxValue, CPUTempVoltestItem.Value, "-", CPUTempVoltestItem.Result, CPUTempVoltestItem.Duration.ToString("F2"));
                    testItems.Add(CPUTempVoltestItem);

                    return testItems;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"设备信息检查发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// WIFI吞吐量测试
        /// </summary>
        /// <returns></returns>
        public List<TestItem> CheckWiFiThroughput(XDC01Serial xDC01Serial, PCCommand pcCommand,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin,
            TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行WiFi吞吐量测试...");
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                /*// 0、读取WiFi信息
                string str_wifi_ssid = "";
                string str_wifi_password = "";
                if(xDC01Serial.GetWiFiSSID(ref str_wifi_ssid, ref str_error_log))
                {
                }
                if (xDC01Serial.GetWiFiPassword(ref str_wifi_password, ref str_error_log))
                {

                }
                logger.ShowLog($"获取到WIFI信息为[{str_wifi_ssid}] - [{str_wifi_password}]");
                if(str_wifi_ssid != testParam.cur_wifi_ssid || str_wifi_password!= testParam.cur_wifi_pwd)
                {
                    logger.ShowLog("WiFi信息和设置的不一致");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("WiFI吞吐量测试", "-", "-", "-", "-", "WiFi信息和设置的不一致", "FAIL", Duration.ToString("F2"));
                    return null;
                }*/
                string ip = "";
                if (xDC01Serial.GetSystemIP(ref ip, ref str_error_log) == false)
                {
                    logger.ShowLog($"读取IP失败：{str_error_log}");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("WiFI吞吐量测试", "-", "-", "-", "-", "设备WiFi连接异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    str_error_log = "";
                    pcCommand.CheckIperf3(true);
                    if (pcCommand.OpenIperf3(ref str_error_log) == false)
                    {
                        logger.ShowLog($"打开iPerf3失败：[{str_error_log}]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("WiFI吞吐量测试", "-", "-", "-", "-", "打开iPerf3失败", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        logger.ShowLog($"打开iPerf3成功");

                        string str_ip = testParam.server_ip;
                        string str_duration = testParam.duration;
                        string str_bandwidth = testParam.bandwidth;

                        // 上行速率
                        start_time = Environment.TickCount;
                        string str_up_rate = "";
                        string str_up_loss = "";
                        if (xDC01Serial.TestWifiUpThroughput(ref str_up_rate, ref str_up_loss, ref str_error_log, str_ip, str_duration, str_bandwidth) == false)
                        {
                            logger.ShowLog("--- 上行测试失败：" + str_error_log);
                            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add("WiFI吞吐量测试", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                            return null;
                        }
                        else
                        {
                            TestItem WiFiUpRatetestItem = new TestItem()
                            {
                                Name = "WIFI上行速率",
                                NgItem = "wifi_up_rate",
                                MinValue = testSpecMin.wifi_up_rate,
                                MaxValue = testSpecMax.wifi_up_rate,
                            };
                            // 添加修正
                            logger.ShowLog($"iperf3实际值: [{str_up_rate}]");
                            float current_up_rate = float.Parse(str_up_rate) + float.Parse(testParam.up_rate_corrected);
                            WiFiUpRatetestItem.Value = current_up_rate;
                            logger.ShowLog($"修正后: [{WiFiUpRatetestItem.Value}]");
                            if (current_up_rate < testSpecMin.wifi_up_rate)
                            {
                                logger.ShowLog($"---上行速率测试失败");
                                WiFiUpRatetestItem.Result = "FAIL";
                            }
                            else
                            {
                                logger.ShowLog($"---上行速率测试通过");
                                WiFiUpRatetestItem.Result = "PASS";
                            }
                            WiFiUpRatetestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(WiFiUpRatetestItem.Name, "-", WiFiUpRatetestItem.MinValue, WiFiUpRatetestItem.MaxValue, WiFiUpRatetestItem.Value, "-", WiFiUpRatetestItem.Result, WiFiUpRatetestItem.Duration.ToString("F2"));
                            testItems.Add(WiFiUpRatetestItem);
                            // 上行丢包率
                            start_time = Environment.TickCount;
                            TestItem WiFiUplosstestItem = new TestItem
                            {
                                Name = "WIFI上行丢包率",
                                NgItem = "wifi_up_loss",
                                MinValue = testSpecMin.wifi_up_loss,
                                MaxValue = testSpecMax.wifi_up_loss,
                                Value = float.Parse(str_up_loss)/100.00f
                            };
                            logger.ShowLog($"上行丢包率: [{WiFiUplosstestItem.Value}]");
                            if (WiFiUplosstestItem.Value > testSpecMax.wifi_up_loss)
                            {
                                logger.ShowLog($"---上行丢包率测试失败");
                                WiFiUplosstestItem.Result = "FAIL";
                            }
                            else
                            {
                                logger.ShowLog($"---上行丢包率测试通过");
                                WiFiUplosstestItem.Result = "PASS";
                            }
                            WiFiUplosstestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(WiFiUplosstestItem.Name, "-", WiFiUplosstestItem.MinValue, WiFiUplosstestItem.MaxValue, WiFiUplosstestItem.Value, "-", WiFiUplosstestItem.Result, WiFiUplosstestItem.Duration.ToString("F2"));
                            testItems.Add(WiFiUplosstestItem);
                        }

                        // 下行速率
                        start_time = Environment.TickCount;
                        string str_down_rate = "";
                        string str_down_loss = "";
                        if (xDC01Serial.TestWifiDownThroughput(ref str_down_rate, ref str_down_loss, ref str_error_log, str_ip, str_duration, str_bandwidth) == false)
                        {
                            logger.ShowLog("--- 下行测试失败：" + str_error_log);
                            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add("WiFI吞吐量测试", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                            return null;
                        }
                        else
                        {
                            TestItem WiFiDownRatetestItem = new TestItem()
                            {
                                Name = "WIFI下行速率",
                                NgItem = "wifi_down_rate",
                                MinValue = testSpecMin.wifi_down_rate,
                                MaxValue = testSpecMax.wifi_down_rate,
                            };
                            // 添加修正
                            logger.ShowLog($"iperf3实际值: [{str_down_rate}]");
                            float current_down_rate = float.Parse(str_down_rate) + float.Parse(testParam.down_rate_corrected);
                            WiFiDownRatetestItem.Value = current_down_rate;
                            logger.ShowLog($"修正后: [{WiFiDownRatetestItem.Value}]");
                            if (current_down_rate < testSpecMin.wifi_down_rate)
                            {
                                logger.ShowLog($"---下行速率测试失败");
                                WiFiDownRatetestItem.Result = "FAIL";
                            }
                            else
                            {
                                logger.ShowLog($"---下行速率测试通过");
                                WiFiDownRatetestItem.Result = "PASS";
                            }
                            WiFiDownRatetestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(WiFiDownRatetestItem.Name, "-", WiFiDownRatetestItem.MinValue, WiFiDownRatetestItem.MaxValue, WiFiDownRatetestItem.Value, "-", WiFiDownRatetestItem.Result, WiFiDownRatetestItem.Duration.ToString("F2"));
                            testItems.Add(WiFiDownRatetestItem);

                            // 下行丢包率
                            start_time = Environment.TickCount;
                            TestItem WiFiDownlosstestItem = new TestItem
                            {
                                Name = "WIFI下行丢包率",
                                NgItem = "wifi_down_loss",
                                MinValue = testSpecMin.wifi_down_loss,
                                MaxValue = testSpecMax.wifi_down_loss,
                                Value = float.Parse(str_down_loss)/100.00f
                            };
                            logger.ShowLog($"下行丢包率: [{WiFiDownlosstestItem.Value}]");
                            if (WiFiDownlosstestItem.Value > testSpecMax.wifi_down_loss)
                            {
                                logger.ShowLog($"---下行丢包率测试失败");
                                WiFiDownlosstestItem.Result = "FAIL";
                            }
                            else
                            {
                                logger.ShowLog($"---下行丢包率测试通过");
                                WiFiDownlosstestItem.Result = "PASS";
                            }
                            WiFiDownlosstestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(WiFiDownlosstestItem.Name, "-", WiFiDownlosstestItem.MinValue, WiFiDownlosstestItem.MaxValue, WiFiDownlosstestItem.Value, "-", WiFiDownlosstestItem.Result, WiFiDownlosstestItem.Duration.ToString("F2"));
                            testItems.Add(WiFiDownlosstestItem);
                        }

                        return testItems;
                    }
                }
            }
            catch (Exception e)
            {
                logger.ShowLog($"DUT WIFI吞吐量测试发生异常：[{e.Message}]");
                return null;
            }
            finally
            {
                pcCommand.CheckIperf3(true);
            }
        }

        /// <summary>
        /// RF433 TX 性能测试，频率和功率
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        /// <param name="testStandard"></param>
        /// <param name="_DSA700"></param>
        /// <param name="testParam"></param>
        /// <returns></returns>
        public List<TestItem> CheckRFTxTest(XDC01Serial xDC01Serial, 
            System.Windows.Forms.DataGridView dataGridView, Logger logger, 
            TestSpecMax testSpecMax, TestSpecMin testSpecMin, TestStandard testStandard,
            DSA700Lib.CVisaOpt_control _DSA700, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-进行RF433性能测试...");
                List<TestItem> testItems = new List<TestItem>();
                
                int start_time = Environment.TickCount;
                string str_error_log = "";

                string str_ResourceName = "";
                string str_dsa700_device = "";
                if (_DSA700.Connect_device(ref str_ResourceName, ref str_dsa700_device) == false)
                {
                    logger.ShowLog("--- 连接频谱仪失败...");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("RF发送测试", "-", "-", "-", "-", "连接频谱仪失败", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    logger.ShowLog("--- 连接频谱仪成功...");
                    str_error_log = "";
                    //板子的rf会持续发送20次，每次间隔0.5s  10s
                    if (xDC01Serial.SetRFMode("send", ref str_error_log) == false)
                    {
                        logger.ShowLog("--- RF测试:指令异常");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("RF发送测试", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        string str_dsa700_ret_value = "";
                        //-----设定最大保持------MAXHold：最大保持。---------- ----------
                        if (_DSA700.TRACe_n_MODE("MAXHold", false, ref str_dsa700_ret_value) == false)
                        {

                        }
                        Delay(int.Parse(testParam.rf_tx_delay) * 1000);
                        //-----WRITe：清除写入。  ----------
                        /*
                        if (_DSA700.TRACe_n_MODE("WRITe", false, ref str_dsa700_ret_value) == false)
                        {

                        }*/
                        TestItem rfFrequencytestItem = new TestItem()
                        {
                            Name = "RF发送频率",
                            NgItem = "rf_tx_frenquency",
                            MinValue = testSpecMin.rf_frequency,
                            MaxValue = testSpecMax.rf_frequency,
                            StrVal = "-"
                        };
                        string str_rf_frequency = "";
                        if (_DSA700.read_read_frequency(ref str_rf_frequency))
                        {
                            str_rf_frequency = str_rf_frequency.Replace("\r\n", "");
                            if (str_rf_frequency != "0")
                            {
                                rfFrequencytestItem.Value = (float)Math.Round(double.Parse(str_rf_frequency) / 1000000, 2);
                        
                                if (rfFrequencytestItem.Value > rfFrequencytestItem.MaxValue ||
                                rfFrequencytestItem.Value < rfFrequencytestItem.MinValue)
                                {
                                    logger.ShowLog($"--- RF频率测试失败：{rfFrequencytestItem.Value}Mhz");
                                    rfFrequencytestItem.Result = "FAIL";
                                }
                                else
                                {
                                    logger.ShowLog($"--- RF频率测试成功: {rfFrequencytestItem.Value}Mhz]");
                                    rfFrequencytestItem.Result = "PASS";
                                }
                            }
                            else
                            {
                                logger.ShowLog("--- RF频率测试异常");
                                rfFrequencytestItem.StrVal = "RF频率测试异常";
                                rfFrequencytestItem.Result = "FAIL";
                            }
                        }
                        else
                        {
                            logger.ShowLog("--- 读取频谱仪频率失败");
                            rfFrequencytestItem.StrVal = "读取频谱仪频率失败";
                            rfFrequencytestItem.Result = "FAIL";
                        }
                        rfFrequencytestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(rfFrequencytestItem.Name, "-", rfFrequencytestItem.MinValue, rfFrequencytestItem.MaxValue, rfFrequencytestItem.Value, rfFrequencytestItem.StrVal, rfFrequencytestItem.Result, rfFrequencytestItem.Duration.ToString("F2"));
                        testItems.Add(rfFrequencytestItem);

                        start_time = Environment.TickCount;
                        // RF功率
                        TestItem rfPowertestItem = new TestItem()
                        {
                            Name = "RF发送功率",
                            NgItem = "rf_tx_power",
                            MinValue = testSpecMin.rf_power,
                            MaxValue = testSpecMax.rf_power,
                            StrVal = "-"
                            //Value = float.Parse(cameraInfo.Battery)
                        };
                        string str_rf_power = "";
                        if (_DSA700.read_read_dbm(ref str_rf_power))
                        {
                            str_rf_power = str_rf_power.Replace("\r\n", "");
                            if (str_rf_power != "0" && str_rf_power.ToUpper().Contains("E"))
                            {
                                Decimal dData = 0.0M;
                                if (str_rf_power.Contains("E"))
                                {
                                    dData = Decimal.Parse(str_rf_power, System.Globalization.NumberStyles.Float);
                                }
                                logger.ShowLog($"频谱仪读数: [{dData}]");
                                // 添加修正值
                                float current_rf_db = (float)dData + float.Parse(testParam.rf_power_corrected);
                                rfPowertestItem.Value = current_rf_db;
                                logger.ShowLog($"修正后: [{rfPowertestItem.Value}]");
                                if (rfPowertestItem.Value > rfPowertestItem.MaxValue ||
                                rfPowertestItem.Value < rfPowertestItem.MinValue)
                                {
                                    logger.ShowLog($"--- RF功率测试失败：{rfPowertestItem.Value}dBm");
                                    rfPowertestItem.Result = "FAIL";
                                }
                                else
                                {
                                    logger.ShowLog($"--- RF功率测试成功: {rfPowertestItem.Value}dBm]");
                                    rfPowertestItem.Result = "PASS";
                                }
                            }
                            else
                            {
                                logger.ShowLog("--- RF功率测试异常");
                                rfPowertestItem.StrVal = "RF功率测试异常";
                                rfPowertestItem.Result = "FAIL";
                            }
                        }
                        else
                        {
                            logger.ShowLog("--- 读取频谱仪功率失败");
                            rfPowertestItem.StrVal = $"读取频谱仪功率失败";
                            rfPowertestItem.Result = "FAIL";
                        }
                        rfPowertestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(rfPowertestItem.Name, "-", rfPowertestItem.MinValue, rfPowertestItem.MaxValue, rfPowertestItem.Value, rfPowertestItem.StrVal, rfPowertestItem.Result, rfPowertestItem.Duration.ToString("F2"));
                        testItems.Add(rfPowertestItem);

                        return testItems;
                    }
                }
            }
            catch (Exception e)
            {
                logger.ShowLog($"DUT RF433性能测试发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// RF433灵敏度测试
        /// </summary>
        /// <returns></returns>
        public TestItem CheckRFRxTest(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "RF接收灵敏度", 
                    NgItem = "rf_rx" 
                };
                string str_error_log = "";
                logger.ShowLog("-进行灵敏度测试...");

                int interval = 5000;
                try
                {
                    decimal temp = decimal.Parse(testParam.rf_rx_timeout) * 1000;
                    interval = (int)temp;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"RF RX测试超时参数异常：[{ee.Message}]");
                }

                xDC01Serial.SetRFMode("rcv", ref str_error_log);

                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > interval)
                    {
                        logger.ShowLog($"--- RF RX灵敏度测试失败");
                        testItem.Result = "FAIL";
                        break;
                    }
                    if (xDC01Serial.str_Receive_skybell.Contains("[RF-Send-test:rcv]") &&
                       xDC01Serial.str_Receive_skybell.Contains("[RF Receive test successful = 3]"))
                    {
                        logger.ShowLog($"--- RF RX灵敏度测试通过");
                        testItem.Result = "PASS";
                        break;
                    }
                }

                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"DUT RF433灵敏度测试发生异常：[{ee.Message}]");
                return null;
            }
            finally
            {
                string str_error_log = "";
                // 关闭rf433接收状态
                if (xDC01Serial.SetRFMode("send", ref str_error_log) == false)
                {
                    logger.ShowLog(str_error_log);
                }
            }
        }

        /// <summary>
        /// 复位键功能测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TestItem CheckResetButton(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "复位键测试(人工)", 
                    NgItem = "reset_button" 
                };
                logger.ShowLog("-进行复位键功能测试...");
                CustomDialog btnDialog = new CustomDialog("复位键测试(人工)", "请按REST键，RESET键是否正常");
                DialogResult result = btnDialog.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 复位键功能测试通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 复位键功能测试失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"DUT 复位键测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 将DUT恢复出厂设置
        /// </summary>
        /// <param name="test_result"></param>
        /// <returns></returns>
        public TestItem SetFactoryReset(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                { Name = "恢复出厂设置", NgItem = "factory_reset" };
                string str_error_log = "";
                logger.ShowLog("-进行恢复出厂设置...");
                if (xDC01Serial.ResetFactory(ref str_error_log))
                {
                    logger.ShowLog($"--- 恢复出厂设置通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 恢复出厂设置失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception e)
            {
                logger.ShowLog($"DUT恢复出厂设置，发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 测试通过时调用，写入下一站的wifi信息
        /// </summary>
        /// <returns></returns>
        public TestItem SetNextStationWiFi(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-写入WiFi设置...");
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "写下一站WIFI", 
                    NgItem = "write_next_wifi" 
                };
                string str_error_log = "";
                if (xDC01Serial.SetWIFIConfig(testParam.next_wifi_ssid, testParam.next_wifi_pwd, ref str_error_log))
                {
                    logger.ShowLog("--- 写WiFi信息成功");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog("--- 写WiFi信息异常：" + str_error_log);
                    testItem.Result = "FAIL";
                }
                float Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", $"{testParam.next_wifi_ssid}-{testParam.next_wifi_pwd}", testItem.Result, Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception e)
            {
                logger.ShowLog($"写入下一站WiFi信息,发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 测试通过时调用，写入测试工序号
        /// </summary>
        /// <returns></returns>
        public TestItem WriteTagNumber(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                logger.ShowLog("-写当前站工序...");
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "写当前站工序", 
                    NgItem = "write_tagnumber" 
                };
                string str_error_log = "";
                if (xDC01Serial.SetTagNumber(testParam.next_tagnumber, ref str_error_log))
                {
                    logger.ShowLog("--- 当前站的工序号设置成功");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog("--- 写工序号操作异常：" + str_error_log);
                    testItem.Result = "FAIL";
                }
                float Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", testParam.next_tagnumber, testItem.Result, Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception e)
            {
                logger.ShowLog($"写入当前站工序号，发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 申请SN/UID/MAC
        /// </summary>
        /// <param name="cloudLoginForm"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="cloudModel"></param>
        /// <returns></returns>
        public TestItem ApplySNandUIDFromCloud(CloudLoginForm cloudLoginForm, System.Windows.Forms.DataGridView dataGridView, Logger logger, CloudModel cloudModel)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "申请SN/UID/MAC", 
                    NgItem = "apply_sn_uid_mac" 
                };
                string str_error_log = "";
                logger.ShowLog("-从云端申请SN\\UID\\MAC...");
                // Setup4. 从云端申请SN\UID\MAC
                string str_respone = "";
                if (cloudLoginForm.cloud_token(cloudModel, ref str_respone))
                {
                    logger.ShowLog("--- 登录成功");
                    str_respone = "";
                    if (cloudLoginForm.cloud_create_token(cloudModel, ref str_respone))
                    {
                        logger.ShowLog("--- token创建成功");
                        Delay(500);
                        str_error_log = "";
                        if (cloudLoginForm.GetSNandUIDFromCloud(cloudModel, ref str_error_log) == false)
                        {
                            logger.ShowLog("--- 从云端获取MAC、SN和UID失败：" + str_error_log);
                            if (str_error_log.Contains("2017"))
                            {
                                logger.ShowLog("解绑mac");
                                cloudLoginForm.UnbindMAC(cloudModel, ref str_error_log);
                                logger.ShowLog(str_error_log);
                            }
                            logger.ShowLog($"RN:{cloudModel.str_rn}需要云端解绑\r\nSN:{cloudModel.str_sn}");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"--- 云端分配SN:{cloudModel.str_sn}");
                            logger.ShowLog($"--- 云端分配UID:{cloudModel.str_uid}");
                            logger.ShowLog($"--- 云端分配MAC:{cloudModel.str_mac_cloud}");
                            testItem.StrVal = $"SN:{cloudModel.str_sn};UID:{cloudModel.str_uid};MAC:{cloudModel.str_mac_cloud}";
                            testItem.Result = "PASS";
                        }
                    }
                    else
                    {
                        logger.ShowLog("--- token创建失败：" + str_respone);
                        testItem.Result = "FAIL";
                    }
                }
                else
                {
                    logger.ShowLog($"--- 登录失败：{str_respone}");
                    testItem.Result = "FAIL";
                }

                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"从云端申请SN/UID/MAC发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// SN/UID写入DUT
        /// </summary>
        /// <returns></returns>
        public List<TestItem> WriteSnUidToDUT(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, CloudModel cloudModel)
        {
            try
            {
                List<TestItem> testItems = new List<TestItem>();

                int start_time = Environment.TickCount;
                string str_error_log = "";
                logger.ShowLog("-写入SN/UID");
                TestItem writeSN = new TestItem()
                {
                    Name = "写入SN",
                    NgItem = "write_sn",
                    Standard = cloudModel.str_sn,
                };
                TestItem writeUID = new TestItem()
                {
                    Name = "写入UID",
                    NgItem = "write_uid",
                    Standard = cloudModel.str_uid,
                };
                //-----写入SN/UID 
                if (xDC01Serial.SetUIDandSN(cloudModel.str_uid, cloudModel.str_sn, ref str_error_log) == false)
                {
                    logger.ShowLog("-- 写SN或UID失败：" + str_error_log);
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("写入SN和UID", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    logger.ShowLog("-- 写SN和UID完成");
                    // 确认写入是否成功
                    string str_read_sn = "";
                    //string str_read_bell_mac = "";
                    str_error_log = "";
                    if (xDC01Serial.GetSN(ref str_read_sn, ref str_error_log) == false)
                    {
                        logger.ShowLog("--- 读SN失败");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("读SN", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        writeSN.StrVal = str_read_sn;
                        if (string.Equals(str_read_sn.ToUpper(), cloudModel.str_sn.ToUpper()) == false)
                        {
                            logger.ShowLog("--- 读到的SN：" + str_read_sn.ToUpper() + "和写入的SN：" + cloudModel.str_sn.ToUpper() + "不一致");
                            writeSN.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog("--- 读到的SN：" + str_read_sn.ToUpper() + "和写入的SN：" + cloudModel.str_sn.ToUpper() + "一致");
                            writeSN.Result = "PASS";
                        }
                        writeSN.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(writeSN.Name, writeSN.Standard, "-", "-", "-", writeSN.StrVal, writeSN.Result, writeSN.Duration.ToString("F2"));
                        testItems.Add(writeSN);
                    }
                    start_time = Environment.TickCount;
                    string str_read_uid = "";
                    str_error_log = "";
                    if (xDC01Serial.GetUID(ref str_read_uid, ref str_error_log) == false)
                    {
                        logger.ShowLog("--- 读UID失败");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("读UID", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        writeUID.StrVal = str_read_uid;
                        if (string.Equals(str_read_uid.ToUpper(), cloudModel.str_uid.ToUpper()) == false)
                        {
                            logger.ShowLog("--- 读到的UID：" + str_read_uid.ToUpper() + "和写入的UID：" + cloudModel.str_uid.ToUpper() + "不一致");
                            writeUID.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog("--- 读到的UID：" + str_read_uid.ToUpper() + "和写入的UID：" + cloudModel.str_uid.ToUpper() + "一致");
                            writeUID.Result = "PASS";
                        }
                        writeUID.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(writeUID.Name, writeUID.Standard, "-", "-", "-", writeUID.StrVal, writeUID.Result, writeUID.Duration.ToString("F2"));
                        testItems.Add(writeUID);
                    }
                }

                //-----写入Mac address
                logger.ShowLog("-写入MAC");
                start_time = Environment.TickCount;
                TestItem writeMAC = new TestItem()
                {
                    Name = "写入MAC",
                    NgItem = "write_mac",
                    Standard = cloudModel.str_mac,
                };
                str_error_log = "";
                if (string.Equals(cloudModel.str_mac.ToUpper(), cloudModel.str_mac_cloud.ToUpper()))
                {
                    logger.ShowLog("--- 设备原MAC地址和云端相同，不再重新写入");
                    writeMAC.Result = "PASS";
                    writeMAC.StrVal = cloudModel.str_mac_cloud;
                    writeMAC.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(writeMAC.Name, writeMAC.Standard, "-", "-", "-", writeMAC.StrVal, writeMAC.Result, writeMAC.Duration.ToString("F2"));
                    testItems.Add(writeMAC);
                }
                else
                {
                    if (xDC01Serial.SetSystemMac(cloudModel.str_mac_cloud, ref str_error_log) == false)
                    {
                        logger.ShowLog("--- 写MAC地址失败：" + str_error_log);
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("写入MAC", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        logger.ShowLog("--- 写MAC地址完成");
                        str_error_log = "";
                        string str_read_mac = "";
                        if (xDC01Serial.GetSystemMac(ref str_read_mac, ref str_error_log) == false)
                        {
                            logger.ShowLog("--- 读MAC地址失败"); 
                            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add("读MAC", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                            return null;
                        }
                        else
                        {
                            writeMAC.StrVal = str_read_mac;
                            if (string.Equals(str_read_mac.ToUpper(), cloudModel.str_mac_cloud.ToUpper()) == false)
                            {
                                logger.ShowLog("--- 读到的MAC：" + str_read_mac.ToUpper() + "和写入的MAC：" + cloudModel.str_mac_cloud.ToUpper() + "不一致");
                                writeMAC.Result = "FAIL";
                            }
                            else
                            {
                                logger.ShowLog("--- 读到的MAC：" + str_read_mac.ToUpper() + "和写入的MAC：" + cloudModel.str_mac_cloud.ToUpper() + "一致");
                                writeMAC.Result = "PASS";
                            }
                            writeMAC.Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add(writeMAC.Name, writeMAC.Standard, "-", "-", "-", writeMAC.StrVal, writeMAC.Result, writeMAC.Duration.ToString("F2"));
                            testItems.Add(writeMAC);
                        }
                    }
                }
                return testItems;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"SN/UID/MAC写入DUT,发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 检查SN/UID/MAC
        /// </summary>
        /// <param name="cloudLoginForm"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="cloudModel"></param>
        /// <returns></returns>
        public TestItem CheckSNandUIDFromCloud(CloudLoginForm cloudLoginForm, System.Windows.Forms.DataGridView dataGridView, Logger logger, CloudModel cloudModel)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "检查SN/UID/MAC", 
                    NgItem = "check_sn_uid_mac" 
                };
                string str_error_log = "";
                logger.ShowLog("-云端检查SN|UID|MAC...");
                string str_respone = "";
                if (cloudLoginForm.cloud_token(cloudModel, ref str_respone))
                {
                    logger.ShowLog("--- 登录成功");
                    str_respone = "";
                    if (cloudLoginForm.cloud_create_token(cloudModel, ref str_respone))
                    {
                        logger.ShowLog("--- token创建成功");
                        Delay(500);
                        str_error_log = "";
                        if (cloudLoginForm.CheckSNandUIDFromCloud(cloudModel, ref str_error_log))
                        {
                            logger.ShowLog("--- 云端检查SN,UID,MAC成功");
                            testItem.Result = "PASS";
                        }
                        else
                        {
                            logger.ShowLog("--- 云端检查SN,UID,MAC失败：" + str_error_log);
                            testItem.Result = "FAIL";
                        }
                    }
                    else
                    {
                        logger.ShowLog("--- token创建失败：" + str_respone);
                        testItem.Result = "FAIL";
                    }
                }
                else
                {
                    logger.ShowLog($"--- 登录失败：{str_respone}");
                    testItem.Result = "FAIL";
                }

                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"云端检查SN/UID/MAC发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 打印SN/MAC
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <param name="testParam"></param>
        /// <param name="cloudModel"></param>
        /// <returns></returns>
        public List<TestItem> PrintSnMAC(System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam, CloudModel cloudModel)
        {
            try
            {
                List<TestItem> testItems = new List<TestItem>();
                int start_time = Environment.TickCount;
                TestItem SNtestItem = new TestItem()
                {
                    Name = "打印SN",
                    NgItem = "print_sn"
                };
                string str_error_log = "";
                logger.ShowLog("-打印SN和MAC");

                if (Printer.Print_SN(logger, testParam.printer_name, cloudModel.str_sn, testParam.sn_count, ref str_error_log) == false)
                {
                    logger.ShowLog("--- 打印SN失败：" + str_error_log);
                    SNtestItem.Result = "FAIL";
                }
                else
                {
                    logger.ShowLog("--- 打印SN成功");
                    SNtestItem.Result = "PASS";
                }
                SNtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(SNtestItem.Name, "-", "-", "-", "-", "-", SNtestItem.Result, SNtestItem.Duration.ToString("F2"));
                testItems.Add(SNtestItem);

                TestItem MACtestItem = new TestItem()
                {
                    Name = "打印MAC",
                    NgItem = "print_mac"
                };
                str_error_log = "";
                if (Printer.Print_MAC(logger, testParam.printer_name, cloudModel.str_mac_cloud, testParam.mac_count, ref str_error_log) == false)
                {
                    logger.ShowLog("--- 打印MAC地址失败：" + str_error_log);
                    MACtestItem.Result = "FAIL";
                }
                else
                {
                    logger.ShowLog("--- 打印MAC地址成功");
                    MACtestItem.Result = "PASS";
                }

                MACtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(MACtestItem.Name, "-", "-", "-", "-", "-", MACtestItem.Result, MACtestItem.Duration.ToString("F2"));
                testItems.Add(MACtestItem);

                return testItems;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"打印SN/MAC发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// RTOS测试通过时调用，写入测试工序号
        /// </summary>
        /// <returns></returns>
        public TestItem WriteTagNumberRTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "写当前站工序",
                    NgItem = "write_tagnumber"
                };
                string str_error_log = "";
                logger.ShowLog("-写当前站工序...");
                if (xDC01Serial.SetTagNumberRTOS(testParam.next_tagnumber, ref str_error_log))
                {
                    logger.ShowLog("--- 当前站的工序号设置成功");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog("--- 写工序号操作异常：" + str_error_log);
                    testItem.Result = "FAIL";
                }
                float Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", testParam.next_tagnumber, testItem.Result, Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception e)
            {
                logger.ShowLog($"写入当前站工序号，发生异常：[{e.Message}]");
                return null;
            }
        }

        public List<TestItem> CheckRNandTagnameRTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam, TestStandard testStandard)
        {
            try
            {
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                logger.ShowLog("-读取RN和工序号");
                // 1、读取RN
                string str_rn = "";
                if (xDC01Serial.GetRnRTOS(ref str_rn, ref str_error_log) == false)
                {
                    logger.ShowLog($"--- 读RN失败：{str_error_log}");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("读取RN号", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    TestItem RNtestItem = new TestItem
                    {
                        Name = "RN号",
                        NgItem = "rn",
                        Standard = $"{testStandard.rn_length}位",
                        StrVal = str_rn
                    };
                    // RN号格式检查
                    if (str_rn.Length != testStandard.rn_length)
                    {
                        logger.ShowLog("--- Rn长度错误...");
                        RNtestItem.Result = "FAIL";
                    }
                    else
                    {
                        logger.ShowLog($"读取到RN号为{str_rn}");
                        RNtestItem.Result = "PASS";
                    }
                    RNtestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(RNtestItem.Name, RNtestItem.Standard, "-", "-", "-", RNtestItem.StrVal, RNtestItem.Result, RNtestItem.Duration.ToString("F2"));
                    testItems.Add(RNtestItem);
                }

                // 2、读取测试工序号tagNumber
                start_time = Environment.TickCount;
                if (testParam.test_mode == "operator")
                {
                    string str_tagNumber = "";
                    if (xDC01Serial.GetTagNumber(ref str_tagNumber, ref str_error_log) == false)
                    {
                        logger.ShowLog($"--- 读工序号失败：{str_error_log}");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("读取工序号", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        TestItem tagNumbertestItem = new TestItem()
                        {
                            Name = "工序号",
                            NgItem = "tagnumber",
                            Standard = $"{testParam.cur_tagnumber}"
                            //Value = float.Parse(cameraInfo.Battery)
                        };
                        tagNumbertestItem.StrVal = str_tagNumber;
                        if (str_tagNumber != testParam.cur_tagnumber)
                        {
                            logger.ShowLog($"--- 工序号[{str_tagNumber}]与预期[{testParam.cur_tagnumber}]不匹配");
                            tagNumbertestItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"--- 读取到工序号[{str_tagNumber}]符合工序控制");
                            tagNumbertestItem.Result = "PASS";
                        }
                        tagNumbertestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(tagNumbertestItem.Name, tagNumbertestItem.Standard, "-", "-", "-", tagNumbertestItem.StrVal, tagNumbertestItem.Result, tagNumbertestItem.Duration.ToString("F2"));
                        testItems.Add(tagNumbertestItem);
                    }
                }
                else
                {
                    logger.ShowLog("--- 工程师测试模式，不读工序号");
                }
                return testItems;
            }
            catch (Exception e)
            {

                logger.ShowLog($"RTOS RN和工序号检查发生异常：[{e.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 镜头清晰度检查
        /// </summary>
        /// <returns></returns>
        public TestItem CheckDayResolutionRTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "镜头清晰度检查",
                    NgItem = "day_video_check"
                };
                logger.ShowLog("-镜头清晰度检查...");
                CustomDialog btnDialog = new CustomDialog("镜头清晰度检查", "请检查镜头画面清晰度", true);
                DialogResult result = btnDialog.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 镜头清晰度检查通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 镜头清晰度检查失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"镜头清晰度检查，发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 镜头暗角检查
        /// </summary>
        /// <returns></returns>
        public TestItem CheckDayDarkCornerRTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "镜头暗角检查",
                    NgItem = "dark_corner"
                };
                logger.ShowLog("-镜头暗角检查...");
                CustomDialog btnDialog = new CustomDialog("镜头暗角检查", "请确认画面有无暗角", true);
                DialogResult result = btnDialog.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 镜头暗角检查通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 镜头暗角检查失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"镜头暗角检查，发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 发送夜视切换指令，听声音并查看LED灯
        /// </summary>
        /// <returns></returns>
        public List<TestItem> CheckIR_CUT_LED_RTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                logger.ShowLog("-进行夜视切换检查...");
                //------12 通过vlc video确认IR Cut IR LED
                TestItem IRLedTestItem = new TestItem()
                {
                    Name = "IR_LED",
                    NgItem = "ir_led"
                };
                string str_error_log = "";
                if (xDC01Serial.SwitchIR_CUT_RTOS("on", ref str_error_log) == false)
                {
                    logger.ShowLog($"切换夜视模式异常：[{str_error_log}]");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("切换夜视检查", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    logger.ShowLog("--- 请检查IR_LED灯功能");
                    CustomDialog IRLedDialog = new CustomDialog("IR_LED检查测试（人工）", "请确认\r\n1、是否听到切换的动作音？\r\n2、六颗红外LED灯是否全亮？", true);
                    DialogResult IRLedresult = IRLedDialog.ShowDialog();

                    if (IRLedresult == DialogResult.Yes)
                    {
                        logger.ShowLog($"--- IR_LED检查测试(人工)通过");
                        IRLedTestItem.Result = "PASS";
                    }
                    else
                    {
                        logger.ShowLog($"--- IR_LED检查测试(人工)失败");
                        IRLedTestItem.Result = "FAIL";
                    }
                    IRLedTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(IRLedTestItem.Name, "-", "-", "-", "-", "-", IRLedTestItem.Result, IRLedTestItem.Duration.ToString("F2"));
                    testItems.Add(IRLedTestItem);

                    start_time = Environment.TickCount;
                    TestItem IRCutTestItem = new TestItem()
                    {
                        Name = "IR_CUT",
                        NgItem = "ir_cut"
                    };
                    logger.ShowLog("--- 请检查IR_CUT夜视功能");
                    CustomDialog IRCutDialog = new CustomDialog("IR_CUT检查测试（人工）", "是否已切换到夜视模式？", true);
                    DialogResult IRCutresult = IRCutDialog.ShowDialog();

                    if (IRCutresult == DialogResult.Yes)
                    {
                        logger.ShowLog($"--- IR_CUT检查测试(人工)通过");
                        IRCutTestItem.Result = "PASS";
                    }
                    else
                    {
                        logger.ShowLog($"--- IR_CUT检查测试(人工)失败");
                        IRCutTestItem.Result = "FAIL";
                    }
                    IRCutTestItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add(IRCutTestItem.Name, "-", "-", "-", "-", "-", IRCutTestItem.Result, IRCutTestItem.Duration.ToString("F2"));
                    testItems.Add(IRCutTestItem);

                    return testItems;
                }
            }
            catch (Exception ee)
            {
                logger.ShowLog($"IR-CUT|IR_LED功能检查，发生异常：[{ee.Message}]");
                return null;
            }
        }

        /// <summary>
        /// 将机器放入测试暗箱预留卡位，查看黑夜模式下的图像清晰度
        /// </summary>
        /// <returns></returns>
        public TestItem CheckNightResolutionRTOS(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem()
                {
                    Name = "夜视清晰度检查",
                    NgItem = "night_video_check"
                };
                logger.ShowLog("-夜视清晰度检查...");
                CustomDialog btnDialog = new CustomDialog("夜视清晰度检查", "请检查夜视画面清晰度", true);
                DialogResult result = btnDialog.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    logger.ShowLog($"--- 夜视清晰度检查通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 夜视清晰度检查失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"镜头夜视清晰度检查，发生异常：[{ee.Message}]");
                return null;
            }
        }

    }
}
