using LabelManager2;
using LogLib;
using System;
using System.IO;

namespace XDC01Action
{
    public class Printer
    {
        /// <summary>
        /// 打印mac标签
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="printerName"></param>
        /// <param name="str_print_mac"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public static bool Print_MAC(string printerName, string str_print_mac, string mac_count, ref string str_error_log)
        {
            try
            {
                // 打印第一张
                string labelFilePath = System.Windows.Forms.Application.StartupPath + @"\LabelFile\mac.lab";

                if (!File.Exists(labelFilePath))
                {
                    str_error_log = $"找不到打印模板文件\n请确认文件[{labelFilePath}]是否存在";
                    return false;
                }

                ApplicationClass labelManager = new ApplicationClass();
                Document labelDocument = labelManager.Documents.Open(labelFilePath);

                // 设置变量值
                labelDocument.Variables.FormVariables.Item("mac").Value = str_print_mac;

                // 设置打印机名称
                labelDocument.Printer.SwitchTo(printerName);

                // 打印标签
                int count = 1;
                try
                {
                    count = int.Parse(mac_count);
                }
                catch (Exception)
                {
                    str_error_log = $"参数mac_count[{mac_count}]异常";
                }
                labelDocument.PrintDocument(count); // 参数为打印份数

                // 关闭标签文档
                labelDocument.Close();

                // 释放资源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelDocument);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelManager);

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"打印MAC标签出现异常:{ee.Message}";
                return false;
            }
        }

        public static bool Print_SN(string printerName, string str_print_sn, string sn_count, ref string str_error_log)
        {
            try
            {
                // 打印第一张
                string labelFilePath = System.Windows.Forms.Application.StartupPath + @"\LabelFile\sn.lab";

                if (!File.Exists(labelFilePath))
                {
                    str_error_log = $"找不到打印模板文件\n请确认文件[{labelFilePath}]是否存在";
                    return false;
                }

                ApplicationClass labelManager = new ApplicationClass();
                Document labelDocument = labelManager.Documents.Open(labelFilePath);

                // 设置变量值
                labelDocument.Variables.FormVariables.Item("sn").Value = str_print_sn;

                // 设置打印机名称
                labelDocument.Printer.SwitchTo(printerName);

                // 打印标签
                int count = 1;
                try
                {
                    count = int.Parse(sn_count);
                }
                catch (Exception)
                {
                    str_error_log = $"参数mac_count[{sn_count}]异常";
                }
                labelDocument.PrintDocument(count); // 参数为打印份数

                // 关闭标签文档
                labelDocument.Close();

                // 释放资源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelDocument);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelManager);

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"打印SN标签出现异常:{ee.Message}";
                return false;
            }
        }

        public static bool Print_SN_with_Code(string printerName, string customer_code, string version_code, string str_print_sn, 
            string sn_count, ref string str_error_log)
        {
            try
            {
                // 打印第一张
                string labelFilePath = System.Windows.Forms.Application.StartupPath + @"\LabelFile\sn_code.lab";

                if (!File.Exists(labelFilePath))
                {
                    str_error_log = $"找不到打印模板文件\n请确认文件[{labelFilePath}]是否存在";
                    return false;
                }

                ApplicationClass labelManager = new ApplicationClass();
                Document labelDocument = labelManager.Documents.Open(labelFilePath);

                // 设置变量值
                labelDocument.Variables.FormVariables.Item("sn_num").Value = str_print_sn;
                labelDocument.Variables.FormVariables.Item("customer").Value = customer_code;
                labelDocument.Variables.FormVariables.Item("version").Value = version_code;
                // 设置打印机名称
                labelDocument.Printer.SwitchTo(printerName);

                // 打印标签
                int count = 1;
                try
                {
                    count = int.Parse(sn_count);
                }
                catch (Exception)
                {
                    str_error_log = $"参数mac_count[{sn_count}]异常";
                }
                labelDocument.PrintDocument(count); // 参数为打印份数

                // 关闭标签文档
                labelDocument.Close();

                // 释放资源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelDocument);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(labelManager);

                return true;
            }
            catch (Exception ee)
            {
                str_error_log = $"打印SN标签出现异常:{ee.Message}";
                return false;
            }
        }
    }
}
