using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDataLib
{
    public partial class Data_information : Form
    {
        private string _table_name = string.Empty;
        private string _id = string.Empty;

        public Data_information(string table_name, string id="rn")
        {
            InitializeComponent();
            _table_name = table_name;
            _id = id;
        }

        private void data_information_Load(object sender, EventArgs e)
        {
            cbo_test_result.SelectedIndex = 0;
            dataGridView1.AllowUserToAddRows = false;   //去除最下面一行非数据
        }

        private void btn_Auto_Click(object sender, EventArgs e)
        {
            access_data_find();
        }

        private bool access_data_find()
        {
            try
            {
                string str_rn = txt_Serial.Text.Replace("\r", "").Replace("\n", "").Trim().ToUpper();
                string Start_Time = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dtp_Start.Value);
                string End_Time = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dtp_End.Value);

                if(_table_name == "")
                {
                    MessageBox.Show($"未指定数据表，无法查询");
                    return false;
                }

                //System_Cloud_DB.System_DB _db = new System_Cloud_DB.System_DB();
                // 本机数据库sqlite
                LocalMachineDB localMachineDB = new LocalMachineDB();
                DataTable dt = new DataTable();
                string str_sql = $"SELECT * FROM `{_table_name}` WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                if (str_rn.Length > 0)
                {
                    str_sql += $" and `{_id}`='{str_rn}'";
                }
                if(cbx_test_result.Checked && cbo_test_result.SelectedItem != null)
                {
                    str_sql += $" and `test_result`='{cbo_test_result.SelectedItem.ToString()}'";
                }
                str_sql += ";";
                string db_file = Environment.CurrentDirectory + @"\Local.db";
                string str_error_log = "";
                if (localMachineDB.SearchDataFromLocal(db_file, str_sql, ref dt, ref str_error_log) == false)
                {

                }
                this.bindingSource1.DataSource = dt;
                this.BindingNavigator1.BindingSource = bindingSource1;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                //this.dataGridView1.Columns["start_test_time"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                dataGridView1.AutoResizeRows();
                dataGridView1.AutoResizeColumns();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }

        private void btn_out_excel_Click(object sender, EventArgs e)
        {
            Out_Excel_1();
        }

        private bool Out_Excel_1()
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("当前页面没有数据可以导出！", "导出结果！", MessageBoxButtons.OK);
                    return false;
                }
                //实例化一个Excel.Application对象;
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //让后台执行设置为不可见，为True的话会看到打开一个Excel，然后往数据里面写；
                excel.Visible = false;
                //新增加一个工作薄，Workbook是直接保存，不会弹出保存对话框，加上application
                //会弹出保存对话框，值为false会报错；
                excel.Application.Workbooks.Add(true);
                for (int h = 2; h <= 80; h++)
                {
                    excel.Columns[h].NumberFormatLocal = "@";
                }
                //标题头获取；
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    excel.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                //datagridview的内容获取；
                for (int j = 1; j < dataGridView1.Rows.Count + 1; j++)
                {
                    for (int k = 1; k < dataGridView1.Columns.Count + 1; k++)
                    {
                        //if (Convert.ToString(dataGridView1[k - 1, j - 1].Value).Length == 0)
                        //{
                        //    dataGridView1[k - 1, j - 1].Value = "NULL";
                        //}
                        //if (this.dataGridView1[k - 1, j - 1].Value == "1310001437")
                        //{
                        //    MessageBox.Show("null");
                        //}
                        excel.Cells[j + 1, k] = dataGridView1[k - 1, j - 1].Value.ToString();// + "";
                        //if (Convert.ToString(excel.Cells[j + 1, k]) == "")
                        //{
                        //  excel.Cells[j + 1, k] = "N/A";
                        //}
                    }
                }
                excel.Cells.EntireColumn.AutoFit();
                excel.Visible = true;
                //excel.Quit();
                excel = null;
                //设置禁止弹出保存和覆盖的询问提示框-------
                //excel.DisplayAlerts = false;
                //excel.AlertBeforeOverwriting = false;
                //------------------------------------------ 
                //保存工作薄
                //excel.Save();
                //excel.Quit();
                //excel = null;
                return true;
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message);
                return false;
            }

        }
    }
}
