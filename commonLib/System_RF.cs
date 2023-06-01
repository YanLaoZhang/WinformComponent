using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.VisaNS;
using RigolTest;
namespace System_RF
{
    public class DS705
    {
        CVisaOpt m_VisaOpt = new CVisaOpt();
        string m_strResourceName = null; //仪器资源名
        public bool m_DS705(string str_device_name,ref List<string> list_str_m_device_name)
        {
            try
            {
                string[] InstrResourceArray = m_VisaOpt.FindResource("?*INSTR"); //查找资源
                if (InstrResourceArray[0] == "未能找到可用资源!")
                {
                    return false;
                }
                else
                {
                    //示例，选取DSG800系列仪器作为选中仪器
                    for (int i = 0; i < InstrResourceArray.Length; i++)
                    {
                        list_str_m_device_name.Add(InstrResourceArray[i]);
                        if (InstrResourceArray[i].Contains(str_device_name)) //(InstrResourceArray[i].Contains("DSG"))
                        {
                            m_strResourceName = InstrResourceArray[i];
                            
                        }
                    }
                }
                //如果没有找到指定仪器直接退出
                if (m_strResourceName == null)
                {
                    return false;
                }
                //str_m_device_name = m_strResourceName;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool m_DS705_set(string str_ds705_ghz, string str_ds705_dbm,ref string str_ret_value)
        {
            try
            {
                //打开指定资源
                if(m_strResourceName == null)
                {
                    return false;
                }
                m_VisaOpt.OpenResource(m_strResourceName);
                //发送命令
                m_VisaOpt.Write("*IDN?");
                //读取命令
                string strback = m_VisaOpt.Read();
                //设置操作命令 1GHz频率 -10dBm幅度 打开RF输出开关

                m_VisaOpt.Write(string.Format(":SOURce:FREQuency {0}", str_ds705_ghz));
                m_VisaOpt.Write(string.Format(":SOURce:LEVel {0}dBm", str_ds705_dbm));

                /*
                m_VisaOpt.Write(":SOURce:FREQuency 1GHz");
                m_VisaOpt.Write(":SOURce:LEVel -10dBm");
                 */
                m_VisaOpt.Write(":OUTPut:STATe ON");
                //显示读取内容
                //Console.Write(strback);
                str_ret_value = strback;
                //是否设备资源
                m_VisaOpt.Release();
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
    }

}
