using System;
using System.Management;
using LabelManager2;

namespace System_Print
{
    public class Print
    {
        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="str_path">Lab模版路径</param>
        /// <param name="str_printer_name">打印机名称</param>
        /// <param name="str_name_1">Lab变量：front</param>
        /// <param name="str_value_1">变量内容</param>
        /// <param name="bool_print_1">是否打印</param>
        /// <param name="str_name_2">Lab变量：middle</param>
        /// <param name="str_value_2">变量内容</param>
        /// <param name="bool_print_2">是否打印</param>
        /// <param name="str_name_3">Lab变量：back</param>
        /// <param name="str_value_3">变量内容</param>
        /// <param name="bool_print_3">是否打印</param>
        /// <param name="print_num">打印数量</param>
        /// <param name="str_error_log">错误信息</param>
        /// <returns></returns>
        public bool print_label(string str_path, string str_printer_name,
                             string str_name_1, string str_value_1, bool bool_print_1,
                             string str_name_2, string str_value_2, bool bool_print_2,
                             string str_name_3, string str_value_3, bool bool_print_3,
                             //string str_name_4, string str_value_4, bool bool_print_4,
                             //string str_name_5, string str_value_5, bool bool_print_5,
                             int print_num, ref string str_error_log)
        {
            ApplicationClass lbl = new ApplicationClass();
            
            try
            {
                lbl.Documents.Open(str_path, false);// 调用设计好的label文件
                Document doc = lbl.ActiveDocument;
                // Strings printers = lbl.PrinterSystem().Printers(enumKindOfPrinters.lppxAllPrinters);
                // lbl.Dialogs.Item(enumDialogType.lppxPrinterSelectDialog).Show();

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer ");
                bool is_offline = true;
                foreach (ManagementObject printer in searcher.Get())
                {
                    if (printer.Properties["Name"].Value.ToString().Equals(str_printer_name))
                    {
                        if (printer["WorkOffline"].ToString().Equals("False"))
                        {
                            is_offline = false;
                        }
                        break;
                    }
                }
                if (is_offline)
                {
                    str_error_log += "没有连接到打印机：" + str_printer_name;
                    return false;
                }
                //doc.Printer.SwitchTo(str_printer_name, "USB001");
                doc.Printer.SwitchTo(str_printer_name);
                if (bool_print_1)
                {
                    doc.Variables.FormVariables.Item(str_name_1).Length = str_value_1.Length;
                    doc.Variables.FormVariables.Item(str_name_1).Value = str_value_1;

                }
                if (bool_print_2)
                {
                    doc.Variables.FormVariables.Item(str_name_2).Length = str_value_2.Length;
                    doc.Variables.FormVariables.Item(str_name_2).Value = str_value_2;

                }
                if (bool_print_3)
                {
                    doc.Variables.FormVariables.Item(str_name_3).Length = str_value_3.Length;
                    doc.Variables.FormVariables.Item(str_name_3).Value = str_value_3;
                }
                //if (bool_print_4)
                //{
                //    doc.Variables.FormVariables.Item(str_name_4).Length = str_value_4.Length;
                //    doc.Variables.FormVariables.Item(str_name_4).Value = str_value_4;
                //}
                //if (bool_print_5)
                //{
                //    doc.Variables.FormVariables.Item(str_name_5).Length = str_value_5.Length;
                //    doc.Variables.FormVariables.Item(str_name_5).Value = str_value_5;
                //}
                doc.PrintDocument(print_num);    //打印
                doc.Save();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.ToString();
                return false;
            }
            finally
            {
                lbl.Quit();
            }
        }
    }
}
