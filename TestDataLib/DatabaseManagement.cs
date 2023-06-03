using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;

namespace TestDataLib
{
    /// <summary>
    /// 本机数据库Sqlite
    /// </summary>
    public class LocalMachineDB
    {
        public bool SaveDataToLocal(string db_file, string str_sql, ref string str_error_log)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + db_file))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = str_sql;
                        int row = cmd.ExecuteNonQuery();
                        if (row > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message;
                return false;
            }
        }

        public bool SearchDataFromLocal(string db_file, string str_sql, ref System.Data.DataTable dt, ref string str_error_log)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + db_file))
                {
                    conn.Open();
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_sql, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message;
                return false;
            }
        }

    }

}
