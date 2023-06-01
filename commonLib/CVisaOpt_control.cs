using System.Windows.Forms;
using System;

namespace RigolTest
{
    public class CVisaOpt_control
    {
        public string m_strResourceName = null;

        public bool bool_strdevice = false;

        public CVisaOpt m_VisaOpt = new CVisaOpt();

        private string str_read_frequency = ":CALCulate:MARKer1:X?";

        private string str_read_dbm = ":CALCulate:MARKer1:Y?";

        private string str_read_device_name = "*IDN?";

        public bool Connect_device(ref string str_ResourceName, ref string str_device_name)
        {
            try
            {
                string[] array = m_VisaOpt.FindResource("?*INSTR");
                if (!(array[0] == "未能找到可用资源!"))
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Application.DoEvents();
                        if (array[i].Contains("DSA8"))
                        {
                            m_strResourceName = array[i];
                        }
                    }
                }

                if (m_strResourceName == null)
                {
                    return false;
                }

                str_ResourceName = m_strResourceName;
                Delay_T(500);
                if (!Send_command(str_read_device_name, bool_ret_back: true, ref str_device_name))
                {
                    MessageBox.Show("read *IDN? fail...", "Send_command");
                    return false;
                }

                if (!str_device_name.ToUpper().Contains("DSA705"))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Delay_T(int t)
        {
            int tickCount = Environment.TickCount;
            do
            {
                bool flag = true;
                Application.DoEvents();
            }
            while (Environment.TickCount - tickCount <= t);
        }

        public bool Send_command(string str_send_command, bool bool_ret_back, ref string str_ret_value)
        {
            try
            {
                if (m_strResourceName == null)
                {
                    return false;
                }

                m_VisaOpt.OpenResource(m_strResourceName);
                m_VisaOpt.Write(str_send_command);
                if (!bool_ret_back)
                {
                    m_VisaOpt.Release();
                    return true;
                }

                str_ret_value = m_VisaOpt.Read();
                m_VisaOpt.Release();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Send_command");
                return false;
            }
        }

        public bool read_read_frequency(ref string str_ret_value)
        {
            try
            {
                if (!Send_command(str_read_frequency, bool_ret_back: true, ref str_ret_value))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool read_read_dbm(ref string str_ret_value)
        {
            try
            {
                if (!Send_command(str_read_dbm, bool_ret_back: true, ref str_ret_value))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool read_read_frequency(string str_n, ref string str_ret_value)
        {
            try
            {
                if (!Send_command(str_read_frequency.Replace("1", str_n), bool_ret_back: true, ref str_ret_value))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool read_read_dbm(string str_n, ref string str_ret_value)
        {
            try
            {
                if (!Send_command(str_read_dbm.Replace("1", str_n), bool_ret_back: true, ref str_ret_value))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TRACe_n_MODE(string Trace_mode, bool bool_return, ref string str_ret_value)
        {
            try
            {
                string str_send_command = "";
                switch (bool_return)
                {
                    case false:
                        str_send_command = $":TRACe1:MODE {Trace_mode}";
                        break;
                    case true:
                        str_send_command = ":TRACe1:MODE?";
                        break;
                }

                if (!Send_command(str_send_command, bool_return, ref str_ret_value))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}