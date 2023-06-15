using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA700Lib
{
    public class CVisaOpt
    {
        private MessageBasedSession mbSession = null;

        private ResourceManager mRes = null;

        public static string[] ResourceArray = null;

        public string[] FindResource(string strRes)
        {
            try
            {
                mRes = null;
                mRes = ResourceManager.GetLocalManager();
                if (mRes == null)
                {
                }

                ResourceArray = mRes.FindResources(strRes);
            }
            catch (ArgumentException)
            {
                ResourceArray = new string[1];
                ResourceArray[0] = "未能找到可用资源!";
            }

            return ResourceArray;
        }

        public void OpenResource(string strResourceName)
        {
            //IL_0023: Unknown result type (might be due to invalid IL or missing references)
            //IL_002d: Expected O, but got Unknown
            //IL_0042: Expected O, but got Unknown
            if (strResourceName != null)
            {
                try
                {
                    mRes = ResourceManager.GetLocalManager();
                    mbSession = (MessageBasedSession)mRes.Open(strResourceName);
                    ((Session)mbSession).Timeout = 15000;
                }
                catch (VisaException val)
                {
                    VisaException val2 = val;
                }
                catch (Exception)
                {
                }
            }
        }

        public void Write(string strCommand)
        {
            //IL_0022: Expected O, but got Unknown
            try
            {
                if (mbSession != null)
                {
                    mbSession.Write(strCommand);
                }
            }
            catch (VisaException val)
            {
                VisaException val2 = val;
            }
            catch (Exception ex)
            {
                throw new Exception("VisaCtrl-VisaOpen\n" + ex.Message);
            }
        }

        public string Read()
        {
            try
            {
                if (mbSession != null)
                {
                    return mbSession.ReadString();
                }
            }
            catch (VisaException)
            {
                return Convert.ToString(0);
            }

            return Convert.ToString(0);
        }

        public void SetOutTime(int time)
        {
            ((Session)mbSession).Timeout = time;
        }

        public void Release()
        {
            if (mbSession != null)
            {
                ((Session)mbSession).Dispose();
            }
        }
    }

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
