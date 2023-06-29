using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDC01Action
{

    public class TestSpecification
    {
        public string _ip { get; set; } = "127.0.0.1";
        public int _port { get; set; } = 3306;

        public TestSpecification(string ip, int port) {
            _ip = ip;
            _port = port;
        }


        /// <summary>
        /// 读取规格参数
        /// </summary>
        /// <param name="specId"></param>
        /// <param name="testSpecMax"></param>
        /// <param name="testSpecMin"></param>
        /// <param name="str_log"></param>
        /// <returns></returns>
        public bool GetTestSpec(int specId, ref TestSpecMax testSpecMax, ref TestSpecMin testSpecMin, ref string str_log)
        {
            try
            {
                MySqlConnectionStringBuilder connectStr = new MySqlConnectionStringBuilder
                {
                    Server = _ip,
                    Port = (uint)_port,
                    Database = "xdc01_management",
                    UserID = "rckxdc01",
                    Password = "2022xdc01",
                    SslMode = MySqlSslMode.None,
                    ConnectionTimeout = 10,
                    //Pooling = true,
                    //MinimumPoolSize = 5,
                    //MaximumPoolSize = 200
                };
                using (var connection = new MySqlConnection(connectStr.ToString()))
                {
                    connection.Open();
                    MySqlCommand cmdMax = connection.CreateCommand();
                    cmdMax.CommandText = "SELECT * FROM `spec_max` WHERE id=@id;";
                    cmdMax.Parameters.AddWithValue("@id", specId);
                    using (var reader = cmdMax.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testSpecMax.vol_wifi = reader.GetFloat("voltage_wifi");
                            testSpecMax.vol_vcc_core = reader.GetFloat("voltage_vcc_core");
                            testSpecMax.vol_sensor1 = reader.GetFloat("voltage_sensor1");
                            testSpecMax.vol_sensor2 = reader.GetFloat("voltage_sensor2");
                            testSpecMax.vol_rtc = reader.GetFloat("voltage_rtc");
                            testSpecMax.vol_vcc = reader.GetFloat("voltage_vcc");
                            testSpecMax.vol_mcu = reader.GetFloat("voltage_mcu");
                            testSpecMax.vol_ddr = reader.GetFloat("voltage_ddr");
                            testSpecMax.work_current = reader.GetFloat("work_current");
                            testSpecMax.charge_current = reader.GetFloat("charge_current");
                            testSpecMax.standby_current = reader.GetFloat("standby_current");
                            testSpecMax.ping_rtt = reader.GetFloat("ping_rtt");
                            testSpecMax.light_val = reader.GetFloat("light");
                            testSpecMax.battery_voltage = reader.GetFloat("battery_voltage");
                            testSpecMax.cpu_temperature = reader.GetFloat("cpu_temperature");
                            testSpecMax.wifi_up_rate = reader.GetFloat("wifi_up_rate");
                            testSpecMax.wifi_up_loss = reader.GetFloat("wifi_up_loss");
                            testSpecMax.wifi_down_rate = reader.GetFloat("wifi_down_rate");
                            testSpecMax.wifi_down_loss = reader.GetFloat("wifi_down_loss");
                            testSpecMax.rf_frequency = reader.GetFloat("rf_tx_frenquency");
                            testSpecMax.rf_power = reader.GetFloat("rf_tx_power");
                        }
                    }
                    MySqlCommand cmdMin = connection.CreateCommand();
                    cmdMin.CommandText = "SELECT * FROM spec_min WHERE id=@id;";
                    cmdMin.Parameters.AddWithValue("@id", specId);
                    using (var reader = cmdMin.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testSpecMin.vol_wifi = reader.GetFloat("voltage_wifi");
                            testSpecMin.vol_vcc_core = reader.GetFloat("voltage_vcc_core");
                            testSpecMin.vol_sensor1 = reader.GetFloat("voltage_sensor1");
                            testSpecMin.vol_sensor2 = reader.GetFloat("voltage_sensor2");
                            testSpecMin.vol_rtc = reader.GetFloat("voltage_rtc");
                            testSpecMin.vol_vcc = reader.GetFloat("voltage_vcc");
                            testSpecMin.vol_mcu = reader.GetFloat("voltage_mcu");
                            testSpecMin.vol_ddr = reader.GetFloat("voltage_ddr");
                            testSpecMin.work_current = reader.GetFloat("work_current");
                            testSpecMin.charge_current = reader.GetFloat("charge_current");
                            testSpecMin.standby_current = reader.GetFloat("standby_current");
                            testSpecMin.ping_rtt = reader.GetFloat("ping_rtt");
                            testSpecMin.light_val = reader.GetFloat("light");
                            testSpecMin.battery_voltage = reader.GetFloat("battery_voltage");
                            testSpecMin.wifi_up_rate = reader.GetFloat("wifi_up_rate");
                            testSpecMin.wifi_up_loss = reader.GetFloat("wifi_up_loss");
                            testSpecMin.wifi_down_rate = reader.GetFloat("wifi_down_rate");
                            testSpecMin.wifi_down_loss = reader.GetFloat("wifi_down_loss");
                            testSpecMin.rf_frequency = reader.GetFloat("rf_tx_frenquency");
                            testSpecMin.rf_power = reader.GetFloat("rf_tx_power");
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                str_log = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 读取标准参数
        /// </summary>
        /// <param name="specId"></param>
        /// <param name="testStandard"></param>
        /// <param name="str_log"></param>
        /// <returns></returns>
        public bool GetTestStandard(int specId, ref TestStandard testStandard, ref string str_log)
        {
            try
            {
                MySqlConnectionStringBuilder connectStr = new MySqlConnectionStringBuilder
                {
                    Server = _ip,
                    Port = (uint)_port,
                    Database = "xdc01_management",
                    UserID = "rckxdc01",
                    Password = "2022xdc01",
                    SslMode = MySqlSslMode.None,
                    ConnectionTimeout = 10,
                    //Pooling = true,
                    //MinimumPoolSize = 5,
                    //MaximumPoolSize = 200
                };
                using (var connection = new MySqlConnection(connectStr.ToString()))
                {
                    connection.Open();
                    MySqlCommand cmdStandard = connection.CreateCommand();
                    cmdStandard.CommandText = "SELECT * FROM standard_version WHERE id=@id;";
                    cmdStandard.Parameters.AddWithValue("@id", specId);
                    using (var reader = cmdStandard.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            testStandard.firmware = reader.GetString("fw_version");
                            testStandard.hardware = reader.GetString("hw_version");
                            testStandard.mcu = reader.GetString("mcu_version");
                            testStandard.rn_length = reader.GetInt32("rn_length");
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                str_log = e.Message;
                return false;
            }
        }
    }

    public class TestSpecMin{
        public float vol_wifi;
        public float vol_vcc_core;
        public float vol_sensor1;
        public float vol_sensor2;
        public float vol_rtc;
        public float vol_vcc;
        public float vol_mcu;
        public float vol_ddr;
        public float work_current;
        public float charge_current;
        public float standby_current;
        public float ping_rtt;
        public float light_val;
        public float battery_voltage;
        public float cpu_temperature;
        public float wifi_up_rate;
        public float wifi_up_loss;
        public float wifi_down_rate;
        public float wifi_down_loss;
        public float rf_frequency;
        public float rf_power;
    }

    public class TestSpecMax
    {
        public float vol_wifi;
        public float vol_vcc_core;
        public float vol_sensor1;
        public float vol_sensor2;
        public float vol_rtc;
        public float vol_vcc;
        public float vol_mcu;
        public float vol_ddr;
        public float work_current;
        public float charge_current;
        public float standby_current;
        public float ping_rtt;
        public float light_val;
        public float battery_voltage;
        public float cpu_temperature;
        public float wifi_up_rate;
        public float wifi_up_loss;
        public float wifi_down_rate;
        public float wifi_down_loss;
        public float rf_frequency;
        public float rf_power;
    }

    public class TestStandard
    {
        public string firmware;
        public string hardware;
        public string mcu;
        public int rn_length;
    }

    public class TestParam
    {
        public string poweron_delay;
        public string db_ip;
        public string db_port;
        public string spec_id;
        public string standard_id;
        public string serial_port;
        public string stage_name;
        public string test_mode;
        public string cur_tagnumber;
        public string ping_ip;
        public string ping_count;
        public string led_interval;
        public string btn_timeout;
        public string pir_timeout;
        public string mic_record_duration;
        public string mic_wave_file;
        public string cur_wifi_ssid;
        public string cur_wifi_pwd;
        public string server_ip;
        public string duration;
        public string bandwidth;
        public string up_rate_corrected;
        public string down_rate_corrected;
        public string rf_tx_delay;
        public string rf_power_corrected;
        public string rf_rx_timeout;
        public string next_wifi_ssid;
        public string next_wifi_pwd;
        public string next_tagnumber;
        public string write_next_wifi;
        public string ng_continue;

        public string cloud_username;
        public string cloud_password;
        public string factoryId;
        public string colorId;
        public string productId;
        public string modeId;
        public string productType;

        public string local_server_name;
        public string rtos_serial_port;

        public string charge_relay_index;
        public string voltage_tdm_port;
        public string current_tdm_port;
        public string current_fluke_port;
        public string relay_port;
        public string trigger_relay_interval;
        public string serial_relay_index;

        public string printer_name;
        public string sn_count;
        public string mac_count;

        public string wav_1;
        public string wav_2;
        public string wav_3;
        public string wav_4;
        public string wav_5;
        public string wav_8;
        public string wav_9;
    }
}
