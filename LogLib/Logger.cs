﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogLib
{
    public class Logger
    {
        System.Windows.Forms.RichTextBox _richTextBox;
        string _LogPath;

        public static bool SaveRichTextBoxToFile(string log, string logPath)
        {
            try
            {
                Console.WriteLine($"上传内容到文件[{logPath}]");
                using (StreamWriter streamWriter = new StreamWriter(logPath))
                {
                    streamWriter.Write(log);
                }
                Console.WriteLine($"上传成功");
                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine($"保存RichTextBox的内容到文件发生异常：[{ee.Message}]");
                return false;
            }
        }

        public Logger(ref System.Windows.Forms.RichTextBox richTextBox, ref string LogPath) {
            _richTextBox = richTextBox;
            _LogPath = LogPath;
        }

        public bool ShowLog(string run_log)//, RichTextBox _richtextbox
        {
            try
            {
                string add_str = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + $": {run_log}\r\n";
                _richTextBox.BeginInvoke(new Action(() =>
                {
                    _richTextBox.Text += add_str;
                    _richTextBox.SelectionStart = _richTextBox.TextLength;
                    _richTextBox.ScrollToCaret();
                    //--------richtext特定字体颜色改变--第二种方法
                    if (_richTextBox.Text.Length > 0)
                    {
                        _richTextBox.Select(0, _richTextBox.TextLength);
                        _richTextBox.SelectionColor = Color.Black;
                        int _i = 0;
                        int _count = 0;
                        while (_richTextBox.Text.IndexOf("失败", _i) != -1)
                        {
                            Application.DoEvents();
                            _i = _richTextBox.Text.IndexOf("失败", _i) + 1;
                            _richTextBox.Select(_i - 1, 2);
                            _richTextBox.SelectionColor = Color.FromArgb(255, 0, 0);
                            _count++;
                        }
                        _i = 0;
                        _count = 0;
                        while (_richTextBox.Text.IndexOf("异常", _i) != -1)
                        {
                            Application.DoEvents();
                            _i = _richTextBox.Text.IndexOf("异常", _i) + 1;
                            _richTextBox.Select(_i - 1, 2);
                            _richTextBox.SelectionColor = Color.FromArgb(255, 0, 0);
                            _count++;
                        }
                        _i = 0;
                        _count = 0;
                        while (_richTextBox.Text.IndexOf("不通过", _i) != -1)
                        {
                            Application.DoEvents();
                            _i = _richTextBox.Text.IndexOf("不通过", _i) + 1;
                            _richTextBox.Select(_i - 1, 3);
                            _richTextBox.SelectionColor = Color.Red;
                            _count++;
                        }
                    }

                }));
                if (WriteLogToFile(add_str) == false)
                {

                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool WriteLogToFile(string str_www)
        {
            try
            {
                //str_www = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now) + "  " + str_www;
                //=============创建文件夹=========
                if (System.IO.Directory.Exists(_LogPath) == false)
                {
                    System.IO.Directory.CreateDirectory(_LogPath);
                }
                string data_filename = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string path_write = _LogPath + @"\" + data_filename + ".log";
                if (System.IO.File.Exists(path_write) == false)
                {
                    System.IO.FileStream filest = new System.IO.FileStream(path_write, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.Write("Log file" + "\r\n");
                    sw.WriteLine(str_www);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                else
                {
                    System.IO.FileStream filest = new System.IO.FileStream(path_write, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.WriteLine(str_www);//要写入的信息。 
                    sw.Close();
                    filest.Close();
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
                //MessageBox.Show(ee.Message);
            }
        }

        public bool WriteDataGridViewToFile(DataGridView dgv)
        {
            try
            {
                return WriteLogToFile($"Test Detail: \n{GetDataGridViewContent(dgv)}\n");
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public static string GetDataGridViewContent(DataGridView dgv)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                // Write the column headers
                for (int i = 0; i < dgv.Columns.Count; i++)
                {

                    sb.Append(dgv.Columns[i].HeaderText);
                    if (i < dgv.Columns.Count - 1)
                    {
                        sb.Append("\t"); // Tab delimited
                    }
                }
                sb.AppendLine();

                // Write the data rows
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow) // Skip the last new row
                    {
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            sb.Append(row.Cells[i].Value?.ToString());
                            if (i < dgv.Columns.Count - 1)
                            {
                                sb.Append("\t"); // Tab delimited
                            }
                        }
                        sb.AppendLine();
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:[{ex.Message}]");
                return string.Empty;
            }
        }
    }
}
