using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPInfoLib
{
    public class ERPInfo
    {
        private string _server_ip;
        private ushort _server_port;
        MySqlConnectionStringBuilder connectStr;

        public ERPInfo(string ip, ushort port) {
            _server_ip = ip;
            _server_port = port;
            connectStr = new MySqlConnectionStringBuilder
            {
                Server = _server_ip,
                Port = _server_port,
                Database = "erp_info",
                UserID = "rckerp",
                Password = "2023erp",
                SslMode = MySqlSslMode.None,
                ConnectionTimeout = 10
            };
        }

        /// <summary>
        /// 从erp_info.manufacturing_order_list中查询到相关工单号
        /// </summary>
        /// <param name="filterKey"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public List<string[]> GetWorkOrderListFromDB(string filterKey, ref string str_error_log)
        {
            // 存储工单号的集合
            List<string[]> workOrderNumbers = new List<string[]>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectStr.ToString()))
                {
                    string str_sql = $"SELECT order_number,material FROM erp_info.manufacturing_order_list WHERE material like '%{filterKey}%' ORDER BY create_time DESC;";
                    conn.Open();
                    using (var command = new MySqlCommand(str_sql, conn))
                    {
                        // 执行查询
                        using (var reader = command.ExecuteReader())
                        {
                            // 读取查询结果
                            while (reader.Read())
                            {
                                string[] strings = new string[2];
                                // 获取工单号并添加到集合中
                                strings[0] = reader.GetString("order_number");
                                strings[1] = reader.GetString("material");
                                workOrderNumbers.Add(strings);
                            }
                        }
                    }
                }

                return workOrderNumbers;
            }
            catch (Exception ee)
            {
                str_error_log = $"获取工单号列表发生异常：{ee.Message}";
                return null;
            }
        }

        /// <summary>
        /// 依据PO获取物料名称列表，[客户id,客户name,物料名称]
        /// </summary>
        /// <param name="po"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public List<string[]> GetMaterialListFromDB(string po, ref string str_error_log)
        {
            // 存储工单号的集合
            List<string[]> material_list = new List<string[]>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectStr.ToString()))
                {
                    string str_sql = $"SELECT customer_id, material_name FROM erp_info.system_order_list WHERE purchase_order = '{po}' ORDER BY create_time DESC;";
                    conn.Open();
                    var tempList = new List<string[]>();
                    using (var command = new MySqlCommand(str_sql, conn))
                    {
                        // 执行查询
                        using (var reader = command.ExecuteReader())
                        {
                            // 读取查询结果
                            while (reader.Read())
                            {
                                string[] strings = new string[3];
                                // 获取工单号并添加到集合中
                                strings[0] = reader.GetString("customer_id");
                                strings[2] = reader.GetString("material_name");
                                tempList.Add(strings);
                            }
                        }
                    }
                    foreach (var strings in tempList)
                    {
                        string get_customer_sql = $"SELECT customer_name FROM erp_info.customer_list WHERE customer_id='{strings[0]}';";
                        using (var command1 = new MySqlCommand(get_customer_sql, conn))
                        {
                            object name = command1.ExecuteScalar();
                            strings[1] = Convert.ToString(name);
                        }
                        material_list.Add(strings);
                    }
                }

                return material_list;
            }
            catch (Exception ee)
            {
                str_error_log = $"获取PO对应的物料名称列表发生异常：{ee.Message}";
                return null;
            }
        }

    }
}
