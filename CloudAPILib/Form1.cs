using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudAPILib
{
    public partial class CloudLoginForm : Form
    {
        public CloudLoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void CloudLoginForm_Load(object sender, EventArgs e)
        {
            button1.Focus();
        }

        public bool cloud_token(CloudModel cloudModel, ref string str_respone)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>
                {
                    cloudModel.str_username,
                    cloudModel.str_password
                };
                if (_cloud_up.cloud_up_user_password_check(str_save_value, ref str_respone, ref cloudModel.str_token_login) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_respone = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }

        }

        public bool cloud_create_token(CloudModel cloudModel, ref string str_error_log)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>
                {
                    cloudModel.str_factoryId,
                    cloudModel.str_productId,
                    cloudModel.str_modeId,
                    cloudModel.str_colorId,
                    cloudModel.str_productType
                };
                cloudModel.list_product_info.Clear();
                if (_cloud_up.cloud_up_compare_product_id(str_save_value, ref str_error_log, ref cloudModel.list_product_info, cloudModel.str_token_login) == false)
                {
                    return false;
                }
                cloudModel.str_token_creat = cloudModel.list_product_info[0];
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }
        }

        /// <summary>
        /// 上传DUT的Rn码\MAC地址到云端申请SN/UID/新MAC
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool GetSNandUIDFromCloud(CloudModel cloudModel, ref string str_error_log)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>
                {
                    cloudModel.str_rn,
                    cloudModel.str_mac,
                    cloudModel.str_colorId,
                    cloudModel.str_factoryId,
                    cloudModel.str_productId,
                    cloudModel.str_modeId,
                    cloudModel.str_productType,
                    cloudModel.str_customerId
                };
                if (_cloud_up.cloud_up_Creat_SN_UID(str_save_value,
                                                    ref str_error_log,
                                                    ref cloudModel.str_sn,
                                                    ref cloudModel.str_mac_cloud,
                                                    ref cloudModel.str_uid,
                                                    cloudModel.str_token_creat) == false)
                {
                    return false;
                }
                cloudModel.str_mac_cloud = cloudModel.str_mac_cloud.Replace("macAddress:", "").Replace("sn:", "").Trim();
                cloudModel.str_sn = cloudModel.str_sn.Replace("sn:", "").Replace("macAddress:", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }
        }

        /// <summary>
        /// 解绑mac
        /// </summary>
        /// <param name="cloudModel"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool UnbindMAC(CloudModel cloudModel, ref string str_error_log)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>();
                str_save_value.Add(cloudModel.str_mac);
                str_save_value.Add(cloudModel.str_colorId);
                str_save_value.Add(cloudModel.str_factoryId);
                str_save_value.Add(cloudModel.str_productId);
                str_save_value.Add(cloudModel.str_modeId);
                str_save_value.Add(cloudModel.str_productType);
                if (_cloud_up.cloud_Unbind_MacAddress(str_save_value,
                                                    ref str_error_log,
                                                    cloudModel.str_token_creat) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cloudModel"></param>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool CheckSNandUIDFromCloud(CloudModel cloudModel, ref string str_error_log)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>
                {
                    cloudModel.str_sn,
                    cloudModel.str_mac_cloud,
                    cloudModel.str_uid,
                    cloudModel.str_rn
                };

                if (_cloud_up.cloud_up_Check_SN_UID(str_save_value, ref str_error_log, ref cloudModel.str_sn, ref cloudModel.str_mac_cloud, ref cloudModel.str_uid, cloudModel.str_token_creat) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }
        }

        /// <summary>
        /// 替换新SN
        /// </summary>
        /// <param name="str_error_log"></param>
        /// <returns></returns>
        public bool ReplacementSNFromCloud(CloudModel cloudModel, ref string str_error_log)
        {
            try
            {
                System_Cloud _cloud_up = new System_Cloud();
                List<string> str_save_value = new List<string>
                {
                    cloudModel.str_rn,
                    cloudModel.str_sn,
                    cloudModel.str_mac,
                    cloudModel.str_uid,
                    cloudModel.str_customerId
                };
                if (_cloud_up.cloud_up_Replace_SN(str_save_value,
                                                    ref str_error_log,
                                                    ref cloudModel.str_replace_sn,
                                                    ref cloudModel.str_replace_mac,
                                                    ref cloudModel.str_replace_uid,
                                                    cloudModel.str_token_creat) == false)
                {
                    return false;
                }
                cloudModel.str_mac_cloud = cloudModel.str_mac_cloud.Replace("macAddress:", "").Replace("sn:", "").Trim();
                cloudModel.str_sn = cloudModel.str_sn.Replace("sn:", "").Replace("macAddress:", "").Trim();
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = ee.Message.Replace("\r", "").Replace("\n", "");
                return false;
            }
        }
    }

    public class CloudModel
    {
        public string str_username = string.Empty;
        public string str_password = string.Empty;
        public string str_factoryId = string.Empty;
        public string str_productId = string.Empty;
        public string str_modeId = string.Empty;
        public string str_colorId = string.Empty;
        public string str_productType = string.Empty;
        public string str_customerId = string.Empty;
        public string str_token_login = string.Empty;
        public string str_token_creat = string.Empty;
        public List<string> list_product_info = new List<string>();
        public string str_rn=string.Empty;
        public string str_mac=string.Empty;
        public string str_sn=string.Empty;
        public string str_uid=string.Empty;
        public string str_mac_cloud = string.Empty;
        public string str_replace_mac = string.Empty;
        public string str_replace_sn = string.Empty;
        public string str_replace_uid = string.Empty;
    }
}
