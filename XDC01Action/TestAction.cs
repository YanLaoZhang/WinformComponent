using CloudAPILib;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using TDMSerialLib;
using VLCForm;
using XDC01SerialLib;

namespace XDC01Action
{
    public class TestAction
    {

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
        /// PCBA电压测试
        /// </summary>
        /// <returns></returns>
        public List<TestItem> PCBAVoltageTest(RelaySerial relaySerial, TDMSerial tDMVolSerial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestSpecMax testSpecMax, TestSpecMin testSpecMin)
        {
            try
            {
                #region 环境准备
                int start_time = Environment.TickCount;
                string str_error_log = "";
                //--------停止连续获取数据
                if (tDMVolSerial._vol_stop_continuous() == false)
                {
                    logger.ShowLog($"初始化电压表失败");
                    float Duration = (Environment.TickCount - start_time) / 1000.00f;
                    dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "电压表控制异常", "FAIL", Duration.ToString("F2"));
                    return null;
                }
                else
                {
                    //-------关闭所有继电器
                    if (relaySerial.CloseAllRelay() == false)
                    {
                        logger.ShowLog($"初始化继电器失败");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                        return null;
                    }
                    else
                    {
                        Delay(200);
                        str_error_log = "";
                        int[] int_relay_status = new int[16];
                        if (relaySerial.GetRelayStatus(ref str_error_log, ref int_relay_status) == false)
                        {
                            logger.ShowLog($"获取继电器状态发生异常[{str_error_log}]");
                            float Duration = (Environment.TickCount - start_time) / 1000.00f;
                            dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                            return null;
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            if (int_relay_status[i] != 0)
                            {
                                logger.ShowLog($"继电器状态错误：[全部断开后，通道{i}仍是打开状态]");
                                float Duration = (Environment.TickCount - start_time) / 1000.00f;
                                dataGridView.Rows.Add("电压测试", "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
                                return null;
                            }
                        }
                    }
                }
                #endregion

                List<TestItem> volTestItems = new List<TestItem>
                {
                    new TestItem() { Name = "voltage_wifi", NgItem = "voltage_wifi", MinValue = testSpecMin.vol_wifi, MaxValue = testSpecMax.vol_wifi },
                    new TestItem() { Name = "voltage_vcc_core", NgItem = "voltage_vcc_core", MinValue = testSpecMin.vol_vcc_core, MaxValue = testSpecMax.vol_vcc_core },
                    new TestItem() { Name = "voltage_sensor1", NgItem = "voltage_sensor1", MinValue = testSpecMin.vol_sensor1, MaxValue = testSpecMax.vol_sensor1 },
                    new TestItem() { Name = "voltage_sensor2", NgItem = "voltage_sensor2", MinValue = testSpecMin.vol_sensor2, MaxValue = testSpecMax.vol_sensor2 },
                    new TestItem() { Name = "voltage_rtc", NgItem = "voltage_rtc", MinValue = testSpecMin.vol_rtc, MaxValue = testSpecMax.vol_rtc },
                    new TestItem() { Name = "voltage_vcc", NgItem = "voltage_vcc", MinValue = testSpecMin.vol_vcc, MaxValue = testSpecMax.vol_vcc },
                    new TestItem() { Name = "voltage_mcu", NgItem = "voltage_mcu", MinValue = testSpecMin.vol_mcu, MaxValue = testSpecMax.vol_mcu },
                    new TestItem() { Name = "voltage_ddr", NgItem = "voltage_ddr", MinValue = testSpecMin.vol_ddr, MaxValue = testSpecMax.vol_ddr }
                };

                int itemNum = volTestItems.Count;
                
                StringBuilder ng_items = new StringBuilder();
                for (int i = 0; i < itemNum; i++)
                {
                    TestItem testItem = volTestItems[i];
                    start_time = Environment.TickCount;
                    // 继电器开关序号
                    ushort relayNum = (ushort)i;

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
                        continue;
                    }
                    if (relay_status[relayNum - 1] == 0)
                    {
                        logger.ShowLog($"继电器状态错误：[通道{relayNum}未打开]");
                        float Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "继电器控制异常", "FAIL", Duration.ToString("F2"));
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
                        testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                        dataGridView.Rows.Add(testItem.Name, testItem.Standard, testItem.MinValue, testItem.MaxValue, testItem.Value, testItem.StrVal,testItem.Result, testItem.Duration.ToString("F2"));
                    }

                    Delay(3000);  // 间隔3秒
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
                Delay(200);
                if (relaySerial.CloseAllRelay() == false)
                {
                    logger.ShowLog($"继电器恢复失败");
                }
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
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem() { 
                    Name = "工作电流", 
                    NgItem = "work_current", 
                    MinValue = testSpecMin.vol_wifi, 
                    MaxValue = testSpecMax.vol_wifi 
                };
                logger.ShowLog("-进行工作电流测试...");
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
                        testItem.Value = float_current;
                        // 判断电流数据情况
                        if (testItem.Value > testItem.MaxValue || testItem.Value < testItem.MinValue)
                        {
                            logger.ShowLog($"---电流{testItem.Name}[{float_current.ToString("F3")}V],测试失败");
                            testItem.Result = "FAIL";
                        }
                        else
                        {
                            logger.ShowLog($"---电流{testItem.Name}[{float_current.ToString("F3")}V],测试通过");
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
                logger.ShowLog($"PCBA电流测试发生异常：[{ee.Message}]");
                return null;
            }
        }

        public List<TestItem> CheckRNandTagname(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam, TestStandard testStandard)
        {
            try
            {
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                logger.ShowLog("-读取RN");
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
                    TestItem testItem = new TestItem() { 
                        Name = "Ping网测试", 
                        NgItem = "ping_rtt", 
                        MinValue = testSpecMin.ping_rtt, 
                        MaxValue = testSpecMax.ping_rtt 
                    };
                    string str_error_log = "";
                    logger.ShowLog("-进行WiFi连网测试...");
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
        /// 麦克风测试(自动)
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="pcCommand"></param>
        /// <param name="axWindowsMediaPlayer1"></param>
        /// <param name="duration"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TestItem CheckMicrophoneAuto(XDC01Serial xDC01Serial, System.Windows.Forms.DataGridView dataGridView, Logger logger, 
            PCCommand pcCommand, AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1, TestParam testParam)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "麦克风测试(自动)", 
                    NgItem = "mic_auto" 
                };
                string str_error_log = "";
                logger.ShowLog("--- 进行麦克风测试...");

                // 打开麦克风
                xDC01Serial.OpenMic(ref str_error_log);
                // 启动录音
                xDC01Serial.RecordMic(testParam.mic_record_duration, ref str_error_log);
                // PC播放音频文件
                pcCommand.PlayWavFile(axWindowsMediaPlayer1, "", true);
                // 播放
                Delay(int.Parse(testParam.mic_record_duration));
                // PC停止播放音频文件
                pcCommand.PlayWavFile(axWindowsMediaPlayer1, "", false);
                // 关闭麦克风
                xDC01Serial.CloseMic(ref str_error_log);
                string str_max_abs = "";
                string str_delta = "";
                string str_result = "";
                xDC01Serial.TestRecordWav(ref str_max_abs, ref str_delta, ref str_result, ref str_error_log);

                testItem.StrVal = $"max_abs:{str_max_abs},delta:{str_delta},result:{str_result}";

                if(str_result == "True")
                {
                    logger.ShowLog($"--- 麦克风测试(自动)通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 麦克风测试(自动)失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", testItem.StrVal, testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"麦克风测试(自动)发生异常：【{ee.Message}】");
                return null;
            }
        }

        /// <summary>
        /// 麦克风测试（人工）
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="duration"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TestItem CheckMicrophoneManual(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger, TestParam testParam)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "麦克风测试(人工)", 
                    NgItem = "mic_manual" 
                };
                string str_error_log = "";
                logger.ShowLog("--- 进行麦克风测试(人工)...");
                logger.ShowLog("--- 请检查播放的录音声音，音质正常OK请按[F2]，音质有问题NG请按[F12]");
                // 播放录音文件
                xDC01Serial.PlayRecordWav(ref str_error_log);
                Delay(int.Parse(testParam.mic_record_duration));
                CustomDialog customDialog = new CustomDialog("麦克风测试（人工）", "麦克风录音是否正常？");
                DialogResult result = customDialog.ShowDialog();

                if (result == DialogResult.Yes) {
                    logger.ShowLog($"--- 麦克风测试(人工)通过");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog($"--- 麦克风测试(人工)失败");
                    testItem.Result = "FAIL";
                }
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"麦克风测试(人工)发生异常：【{ee.Message}】");
                return null;
            }
        }

        /// <summary>
        /// 按键功能测试and喇叭测试
        /// </summary>
        /// <param name="xDC01Serial"></param>
        /// <param name="dataGridView"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public List<TestItem> CheckButtonAndSpeaker(XDC01Serial xDC01Serial,
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                logger.ShowLog("-进行按键功能测试...");
                TestItem btnTestItem = new TestItem() { 
                    Name = "按键测试", 
                    NgItem = "button" 
                };
                xDC01Serial.SetPIR("off", ref str_error_log);
                CustomDialog btnDialog = new CustomDialog("按键测试（人工）", "请按键！");
                DialogResult result = btnDialog.ShowDialog();

                if (result == DialogResult.Yes)
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
            System.Windows.Forms.DataGridView dataGridView, Logger logger)
        {
            try
            {
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "移动感应", 
                    NgItem = "motion" 
                };
                string str_error_log = "";
                logger.ShowLog("-进行移动感应PIR测试...");
                xDC01Serial.SetPIR("on", ref str_error_log);
                CustomDialog btnDialog = new CustomDialog("移动感应PIR测试（人工）", "请在PIR上方挥手!");
                DialogResult result = btnDialog.ShowDialog();

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
                testItem.Duration = (Environment.TickCount - start_time) / 1000.00f;
                dataGridView.Rows.Add(testItem.Name, "-", "-", "-", "-", "-", testItem.Result, testItem.Duration.ToString("F2"));
                return testItem;
            }
            catch (Exception ee)
            {
                logger.ShowLog($"PIR测试发生异常：[{ee.Message}]");
                return null;
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
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "LED颜色测试", 
                    NgItem = "led_color" 
                };
                string str_error_log = "";
                logger.ShowLog("-进行LED灯颜色测试...");
                logger.ShowLog("--- 请检查LED灯颜色，OK请按[F2]，NG请按[F12]");
                int interval = 1000;
                try
                {
                    interval = int.Parse(testParam.led_interval) * 1000;
                }
                catch (Exception ee)
                {
                    logger.ShowLog($"LED切换间隔参数异常：[{ee.Message}]");
                }

                bool shouldStop = false;
                Task.Run(() =>
                {
                    // 循环控制LED灯颜色，监测人工是否完成判定
                    while (!shouldStop)
                    {
                        Application.DoEvents();
                        // 红色
                        if (xDC01Serial.SetBtnLEDColor("red", ref str_error_log) == false)
                        {

                        }
                        Delay(interval);
                        // 绿色
                        if (xDC01Serial.SetBtnLEDColor("green", ref str_error_log) == false)
                        {

                        }
                        Delay(interval);
                        // 蓝色
                        if (xDC01Serial.SetBtnLEDColor("blue", ref str_error_log) == false)
                        {

                        }
                        Delay(interval);
                        // 白色
                        if (xDC01Serial.SetBtnLEDColor("white", ref str_error_log) == false)
                        {

                        }
                        Delay(interval);
                    }
                });

                CustomDialog customDialog = new CustomDialog("LED灯检查测试（人工）", "是否依次红、绿、蓝、白四种颜色？");
                DialogResult result = customDialog.ShowDialog();

                // 停止切换灯光
                shouldStop = true;

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
                    dataGridView.Rows.Add("RTSP视频检查", "-", "-", "-", "-", "串口指令发送异常", "FAIL", Duration.ToString("F2"));
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
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem() { Name = "亮度值", NgItem = "light", MinValue = testSpecMin.light_val, MaxValue = testSpecMax.light_val };
                string str_error_log = "";
                logger.ShowLog("-进行Light Sensor亮度值检查...");
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
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                logger.ShowLog("-进行设备信息检查...");
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
                logger.ShowLog($"设备信息检查发生异常：【{ee.Message}】");
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
                int start_time = Environment.TickCount;
                List<TestItem> testItems = new List<TestItem>();
                string str_error_log = "";
                logger.ShowLog("-进行WiFi吞吐量测试...");
                // 0、读取WiFi信息
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
                                Value = float.Parse(str_up_loss)
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
                                Value = float.Parse(str_down_loss)
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
                List<TestItem> testItems = new List<TestItem>();
                
                int start_time = Environment.TickCount;
                string str_error_log = "";
                logger.ShowLog("-进行RF433性能测试...");

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
                        Delay(int.Parse(testParam.rf_tx_delay));
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
                                float current_rf_db = rfPowertestItem.Value + float.Parse(testParam.rf_power_corrected);
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
                logger.ShowLog($"DUT RF433性能测试发生异常：【{e.Message}】");
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
                { Name = "RF接收灵敏度", NgItem = "rf_rx" };
                string str_error_log = "";
                logger.ShowLog("-进行灵敏度测试...");
                xDC01Serial.SetRFMode("rcv", ref str_error_log);

                int numa = Environment.TickCount;
                while (true)
                {
                    Application.DoEvents();
                    if (Environment.TickCount - numa > int.Parse(testParam.rf_rx_timeout) * 1000)
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
                logger.ShowLog($"DUT RF433灵敏度测试发生异常：【{ee.Message}】");
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
                TestItem testItem = new TestItem()
                { Name = "复位键测试(人工)", NgItem = "reset_button" };
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
                if (xDC01Serial.ResetFactory(ref str_error_log) == false)
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
                int start_time = Environment.TickCount;
                TestItem testItem = new TestItem(){ 
                    Name = "写下一站工序", 
                    NgItem = "write_tagnumber" 
                };
                string str_error_log = "";
                logger.ShowLog("-写下一站工序...");
                if (xDC01Serial.SetTagNumber(testParam.next_tagnumber, ref str_error_log))
                {
                    logger.ShowLog("--- 下一工站的工序号设置成功");
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
                logger.ShowLog($"写入下一站工序号，发生异常：[{e.Message}]");
                return null;
            }
        }


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
                if(cloudLoginForm.CheckSNandUIDFromCloud(cloudModel, ref str_error_log))
                {
                    logger.ShowLog("--- 云端检查SN,UID,MAC成功");
                    testItem.Result = "PASS";
                }
                else
                {
                    logger.ShowLog("--- 云端检查SN,UID,MAC失败：" + str_error_log);
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

    }
}
