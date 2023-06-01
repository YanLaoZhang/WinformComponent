using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;

namespace System_Cloud_DB
{
    /// <summary>
    /// Cloud API
    /// </summary>
    public class System_Cloud
    {
        //---用户名验证...
        private string posturl_login = "http://120.78.224.26:8080/XDC01Management/token/login";
        //---创建id
        private string posturl_compare_product_id = "http://120.78.224.26:8080/XDC01Management/token/create";
        //---post SN UID
        private string posturl_creat_SN_UID = "http://120.78.224.26:8080/XDC01Management/api/product/createKey";

        private string posturl_check_SN_UID = "http://120.78.224.26:8080/XDC01Management/api/product/checkKey";
        //S030 PCBA Function Test Report
        private string posturl_S030_pcba = "http://120.78.224.26:8080/XDC01Management/product-test-report/pcba";

        //S130 Focusing Test Report
        private string posturl_S130_focus = "http://120.78.224.26:8080/XDC01Management/product-test-report/focusing";

        //S140 Product Function Test Report
        private string posturl_S140_product = "http://120.78.224.26:8080/XDC01Management/product-test-report/product";
        //S150 SN/UID/MAC Report
        private string posturl_S150_SN_UID_MAC = "http://120.78.224.26:8080/XDC01Management/product-test-report/key";
        //S160 Final Test Report
        private string posturl_S160_Final = "http://120.78.224.26:8080/XDC01Management/product-test-report/final";

        private string posturl_S170_RF = "http://120.78.224.26:8080/XDC01Management/product-test-report/rfTest";

        private string posturl_find_product_key = "http://120.78.224.26:8080/XDC01Management/api/findProductKeyByMac";

        private string posturl_Unbind_MacAddress = "http://120.78.224.26:8080/XDC01Management/api/unbindMacAddress";

        private string str_header_key = "Authorization";
        //private string str_header_value = "eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDY5ODc0Mjl9.ITXoMsLk9w.CNL4I3NIIr7SIUVAf6eetnRWvtg821TI";//"eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDEwNDY0OTV9.yoyntiAWucTlg2MA_n6KCVeRrcMyivXpRCBQM97lTOk";

        private string str_path = System.Windows.Forms.Application.StartupPath + @"\Log_file\cloud\" + string.Format("{0:yyyy.MM.dd}", System.DateTime.Now) + ".txt";
        
        public bool cloud_up_user_password_check(List<string> str_save_value, ref string str_error_log, ref string str_token)
        {
            try
            {

                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("username", str_save_value[0]);//save_value.str_Rn_1);
                p_dic_data.Add("password", str_save_value[1]);//""
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost_login(posturl_login, postdata, ref responseString) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "check user error_code:" + str_error_code + " Fail";
                    return false;
                }
                str_token = responsestring_post[2].Replace("token:", "").Replace("data:", "").Replace("\r", "").Replace("\n", "").Trim();
                /*
                if (str_data != str_header_value)
                {
                    str_error_log = "check user error_code:" + str_data + " Fail";
                    return false;
                }
                 */
                // str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "check user error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_compare_product_id(List<string> str_save_value, ref string str_error_log, ref List<string> ret_product_id, string str_token)
        {
            try
            {
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("factoryId", str_save_value[0]);
                p_dic_data.Add("productId", str_save_value[1]);
                p_dic_data.Add("modelId", str_save_value[2]);
                p_dic_data.Add("colorId", str_save_value[3]);
                p_dic_data.Add("productType", str_save_value[4]);
                //string postdata = ParseToString(p_dic_data);

                string Contentjson = JsonConvert.SerializeObject(p_dic_data);
                //MessageBox.Show("Contentjson:" + Contentjson,"dll",MessageBoxButtons.OK,MessageBoxIcon.Information);
                string responseString = null;
                if (DealPost_creat_token(posturl_compare_product_id, Contentjson, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();  //result true

                if (!str_result.Contains("true"))
                {
                    str_error_log = "check user error_code:" + responseString + " Fail";
                    return false;
                }
                string str_error_code = responsestring_post[1].Replace("error:", "");
                string str_data = responsestring_post[2].Replace("data:", "").Replace("token:", "").Replace("\r", "").Replace("\n", "").Trim();

                ret_product_id.Add(str_data);              //token:eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NTAxMDE4MjV9.xRiOKAALo309-tiGtWPg6gHpsR4U0sMqPA-muAP7Mbk
                ret_product_id.Add(responsestring_post[3].Replace("factoryId:", ""));//factoryId: 1
                ret_product_id.Add(responsestring_post[4].Replace("productId:", ""));//productId: 200
                ret_product_id.Add(responsestring_post[5].Replace("modelId:", ""));  //modelId: 2
                ret_product_id.Add(responsestring_post[6].Replace("colorId:", ""));  //colorId: 1
                ret_product_id.Add(responsestring_post[7].Replace("createTime:", ""));//createTime: Apr 16,
                ret_product_id.Add(responsestring_post[8].Replace("createTime:", ""));// 2021 5:37:05 PM
                ret_product_id.Add(responsestring_post[9].Replace("expiresTime:", ""));//expiresTime: Apr 16, 
                ret_product_id.Add(responsestring_post[10].Replace("expiresTime:", ""));//2022 5:37:05 PM
                ret_product_id.Add(responsestring_post[11].Replace("status:", ""));     //status: true
                if (ret_product_id[9] != "true")
                {
                    str_error_log = "check user error_code:" + responseString + " Fail";
                    return false;
                }
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "check user error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_Creat_SN_UID(List<string> str_save_value, ref string str_error_log, ref string str_ret_sn, ref string str_ret_macaddress, ref string str_uid, string str_token)
        {
            try
            {
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("pcbaRn", str_save_value[0]);
                p_dic_data.Add("macAddress", str_save_value[1]);
                p_dic_data.Add("colorId", str_save_value[2]);
                p_dic_data.Add("factoryId", str_save_value[3]);
                p_dic_data.Add("productId", str_save_value[4]);
                p_dic_data.Add("modelId", str_save_value[5]);
                p_dic_data.Add("productType", str_save_value[6]);
                //string postdata = ParseToString(p_dic_data);
                string Contentjson = JsonConvert.SerializeObject(p_dic_data);
                string responseString = null;
                if (DealPost_creat_sn_uid(posturl_creat_SN_UID, Contentjson, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "post SN UID error_code:" + str_error_code + " Fail";
                    return false;
                }
                str_ret_sn = responsestring_post[2].Replace("data:", "").Replace("sn:", "").Replace("\r", "").Replace("\n", "").Trim();
                str_ret_macaddress = responsestring_post[3].Replace("macAddress:", "");
                str_uid = responsestring_post[4].Replace("uid:", "");
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "check user error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_Check_SN_UID(List<string> str_save_value, ref string str_error_log, ref string str_ret_sn, ref string str_ret_macaddress, ref string str_uid, string str_token)
        {
            string responseString = null;
            try
            {
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("sn", str_save_value[0]);
                p_dic_data.Add("macAddress", str_save_value[1]);
                p_dic_data.Add("uid", str_save_value[2]);
                p_dic_data.Add("pcbaRn", str_save_value[3]);
                //string postdata = ParseToString(p_dic_data);
                string Contentjson = JsonConvert.SerializeObject(p_dic_data);

                if (DealPost_creat_sn_uid(posturl_check_SN_UID, Contentjson, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "result:false_" + "error_code:" + str_error_code;
                    return false;
                }

                str_ret_sn = responsestring_post[2].Replace("data:", "").Replace("sn:", "").Replace("\r", "").Replace("\n", "").Trim();
                str_ret_macaddress = responsestring_post[3].Replace("macAddress:", "");
                str_uid = responsestring_post[4].Replace("uid:", "");

                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "check user error:[" + responseString + "]" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_Find_Product_Key(List<string> str_save_value, ref string str_error_log, string str_token, ref string str_result,
                                           ref string str_ret_pcbaRN, ref string str_ret_SN, ref string str_ret_mac_Address, ref string str_ret_UID)
        {
            try
            {
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("macAddress", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("colorId", str_save_value[1]);//""
                p_dic_data.Add("factoryId", str_save_value[2]);//""
                p_dic_data.Add("productId", str_save_value[3]);//""
                p_dic_data.Add("modelId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_find_product_key, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_error_code.Trim().Length > 0)
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }


                str_ret_pcbaRN = responsestring_post[2].Replace("data:", "").Replace("pcbaRn:", "").Replace("\r", "").Replace("\n", "").Trim();
                str_ret_SN = responsestring_post[3].Replace("data:", "").Replace("sn:", "").Replace("\r", "").Replace("\n", "").Trim();
                str_ret_mac_Address = responsestring_post[4].Replace("macAddress:", "");
                str_ret_UID = responsestring_post[5].Replace("uid:", "");
                str_error_log = responseString;
                /*
{
"result": true,
"error": "",
"data": {
"pcbaRn":"9c54da0f4203",
"sn":"200000000111111",
"macAddress":"9C:54:DA:0F:42:03",
"uid":"A0AA80000497111A"
}
}
                 */
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }
        public bool cloud_Unbind_MacAddress(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("macAddress", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("colorId", str_save_value[1]);//""
                p_dic_data.Add("factoryId", str_save_value[2]);//""
                p_dic_data.Add("productId", str_save_value[3]);//""
                p_dic_data.Add("modelId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_Unbind_MacAddress, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }
                str_error_log = responseString;
                /*
                 {
"result": true,
"error": "",
"data": {}
}
                 */
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_S030_pcba(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                for (int i = 0; i < str_save_value.Count; i++)
                {
                    Application.DoEvents();
                    if (str_save_value[i].ToUpper() == "OK")
                    {
                        str_save_value[i] = "true";
                    }
                    if (str_save_value[i].ToUpper() == "NG")
                    {
                        str_save_value[i] = "false";
                    }
                }
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("pcbaRn", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("factoryId", str_save_value[1]);//""
                p_dic_data.Add("productId", str_save_value[2]);//""
                p_dic_data.Add("modelId", str_save_value[3]);//""
                p_dic_data.Add("colorId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""

                p_dic_data.Add("tagNumber", str_save_value[6]);

                p_dic_data.Add("voltageMcu", str_save_value[7]);
                p_dic_data.Add("voltageWifi", str_save_value[8]);
                p_dic_data.Add("voltageVccCore", str_save_value[9]);
                p_dic_data.Add("voltageDdr", str_save_value[10]);
                p_dic_data.Add("voltageRtc", str_save_value[11]);

                p_dic_data.Add("voltageSensor1", str_save_value[12]);
                p_dic_data.Add("voltageSensor2", str_save_value[13]);
                p_dic_data.Add("voltageBattery", str_save_value[14]);//""

                p_dic_data.Add("voltageDsp", str_save_value[15]);
                p_dic_data.Add("voltageDdr2", str_save_value[16]);//""
                p_dic_data.Add("voltageCore", str_save_value[17]);//""
                p_dic_data.Add("voltageImx225", str_save_value[18]);//""

                p_dic_data.Add("pcbaMic", str_save_value[19]);//true,//true=OK,false=NG
                p_dic_data.Add("pcbaMicAmp", str_save_value[20]);
                p_dic_data.Add("ppcbaMicPower", str_save_value[21]);

                p_dic_data.Add("pcbaTemperatureReading", str_save_value[22]);
                p_dic_data.Add("pcbaLightReading", str_save_value[23]);
                p_dic_data.Add("pcbaVoltageReading", str_save_value[24]);
                p_dic_data.Add("pingStat", str_save_value[25]);
                p_dic_data.Add("micMaxAbs", str_save_value[26]);//""
                p_dic_data.Add("micDelta", str_save_value[27]);//""
                p_dic_data.Add("pcbaAudio", str_save_value[28]);//true,//声音是否正常 true=OK,false=NG
                p_dic_data.Add("vlcVideoCheck", str_save_value[29]);//true,//视频检查 true=OK,false=NG
                p_dic_data.Add("irCheck", str_save_value[30]);//true,//IR-CUT检查 true=OK,false=NG
                p_dic_data.Add("pcbaLed", str_save_value[31]);//true,//Led检查 true=OK,false=NG
                p_dic_data.Add("pcbaMotion", str_save_value[32]);//true,//Motion检查 true=OK,false=NG
                p_dic_data.Add("pcbaButton", str_save_value[33]);//true,//Button检查 true=OK,false=NG
                p_dic_data.Add("pcbaCloudUp", str_save_value[34]);//true,//Cloudup true=OK,false=NG
                p_dic_data.Add("pcbaWorkingCurrent", str_save_value[35]);//""
                p_dic_data.Add("pcbaStandbyCurrent", str_save_value[36]);//""
                p_dic_data.Add("pcbaChargeCurrent", str_save_value[37]);
                p_dic_data.Add("pcbaOprNumber", str_save_value[38]);
                p_dic_data.Add("pcbaVersion", str_save_value[39]);
                p_dic_data.Add("result", str_save_value[40]);
                p_dic_data.Add("failMessage", str_save_value[41]);
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_S030_pcba, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        private bool txt_file_save(string str_file_path, string str_file_content)
        {
            try
            {
                //string str_path_write = System.Windows.Forms.Application.StartupPath + @"\Log_file" + @"\power_on_error_" + string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now) + ".txt";
                string str_path_folder = System.IO.Path.GetDirectoryName(str_file_path);
                if (!System.IO.Directory.Exists(str_path_folder))
                {
                    System.IO.Directory.CreateDirectory(str_path_folder);
                }
                if (System.IO.File.Exists(str_file_path) == false)
                {
                    //System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.WriteLine(string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now));
                    sw.WriteLine(str_file_content);//要写入的信息。 
                    sw.WriteLine("\r\n");
                    //sw.Write(str_file_content);
                    sw.Close();
                    filest.Close();
                }
                else
                {
                    //System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.FileStream filest = new System.IO.FileStream(str_file_path, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(filest);
                    sw.WriteLine(string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now));
                    sw.WriteLine(str_file_content);//要写入的信息。 
                    sw.WriteLine("\r\n");
                    sw.Close();
                    filest.Close();
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool cloud_up_S140_product(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                for (int i = 0; i < str_save_value.Count; i++)
                {
                    Application.DoEvents();
                    if (str_save_value[i].ToUpper() == "OK")
                    {
                        str_save_value[i] = "true";
                    }
                }


                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("pcbaRn", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("factoryId", str_save_value[1]);//""
                p_dic_data.Add("productId", str_save_value[2]);//""
                p_dic_data.Add("modelId", str_save_value[3]);//""
                p_dic_data.Add("colorId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""

                p_dic_data.Add("tagNumber", str_save_value[6]);

                p_dic_data.Add("productTemperatureReading", str_save_value[7]);
                p_dic_data.Add("productLightReading", str_save_value[8]);
                p_dic_data.Add("productVoltageReading", str_save_value[9]);
                p_dic_data.Add("productAudio", str_save_value[10]);
                p_dic_data.Add("productLed", str_save_value[11]);
                p_dic_data.Add("productMicMaxAbs", str_save_value[12]);
                p_dic_data.Add("productMicDelta", str_save_value[13]);
                p_dic_data.Add("productMic", str_save_value[14]);
                p_dic_data.Add("productMicAmp", str_save_value[15]);
                p_dic_data.Add("productMicPower", str_save_value[16]);
                p_dic_data.Add("productVlcVideoCheck", str_save_value[17]);
                p_dic_data.Add("productIrCheck", str_save_value[18]);
                p_dic_data.Add("productButton", str_save_value[19]);
                p_dic_data.Add("productMotion", str_save_value[20]);
                p_dic_data.Add("upPacketLoss", str_save_value[21]);
                p_dic_data.Add("upRate", str_save_value[22]);
                p_dic_data.Add("downPacketLoss", str_save_value[23]);
                p_dic_data.Add("downRate", str_save_value[24]);
                //"rfResult":"1", //RF433/RF345功率测试
                p_dic_data.Add("rfResultmHz", str_save_value[25]);
                p_dic_data.Add("rfResultdb", str_save_value[26]);

                p_dic_data.Add("productCloudUp", str_save_value[27]);
                p_dic_data.Add("productMacAddress", str_save_value[28]);
                p_dic_data.Add("productOprNumber", str_save_value[29]);
                p_dic_data.Add("productVersion", str_save_value[30]);
                p_dic_data.Add("result", str_save_value[31]);
                p_dic_data.Add("failMessage", str_save_value[32]);
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_S140_product, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + str_error_code + " Fail";
                    return false;
                }
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_S150_SN_UID_MAC(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                for (int i = 0; i < str_save_value.Count; i++)
                {
                    Application.DoEvents();
                    if (str_save_value[i].ToUpper() == "OK")
                    {
                        str_save_value[i] = "true";
                    }
                }
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("pcbaRn", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("factoryId", str_save_value[1]);//""
                p_dic_data.Add("productId", str_save_value[2]);//""
                p_dic_data.Add("modelId", str_save_value[3]);//""
                p_dic_data.Add("colorId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""

                p_dic_data.Add("tagNumber", str_save_value[6]);
                p_dic_data.Add("sn", str_save_value[7]);
                p_dic_data.Add("uid", str_save_value[8]);
                p_dic_data.Add("mac", str_save_value[9]);
                p_dic_data.Add("printSn", str_save_value[10]);
                p_dic_data.Add("snCloudUp", str_save_value[11]);
                p_dic_data.Add("snOprNumber", str_save_value[12]);
                p_dic_data.Add("snVersion", str_save_value[13]);
                p_dic_data.Add("result", str_save_value[14]);
                p_dic_data.Add("failMessage", str_save_value[15]);
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_S150_SN_UID_MAC, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_S170_RF_test(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                for (int i = 0; i < str_save_value.Count; i++)
                {
                    Application.DoEvents();
                    if (str_save_value[i].ToUpper() == "OK")
                    {
                        str_save_value[i] = "true";
                    }
                }
                Dictionary<string, string> p_dic_data = new Dictionary<string, string>();
                p_dic_data.Add("pcbaRn", str_save_value[0]);//save_value.str_Rn_1);

                p_dic_data.Add("factoryId", str_save_value[1]);//""
                p_dic_data.Add("productId", str_save_value[2]);//""
                p_dic_data.Add("modelId", str_save_value[3]);//""
                p_dic_data.Add("colorId", str_save_value[4]);//""
                p_dic_data.Add("productType", str_save_value[5]);//""

                p_dic_data.Add("tagNumber", str_save_value[6]);
                p_dic_data.Add("fwVersion", str_save_value[7]);       //FW版本

                p_dic_data.Add("sensitivityTest", str_save_value[8]); //灵敏度_测试
                p_dic_data.Add("rfCloudUp", str_save_value[9]);       //RF测试上传云 true=OK,false=NG
                p_dic_data.Add("rfOprNumber", str_save_value[10]);    //RF测试检查工序号
                p_dic_data.Add("rfVersion", str_save_value[11]);      //RF测试检查版本号    

                p_dic_data.Add("result", str_save_value[12]);//测试结果 true=OK,false=NG
                p_dic_data.Add("failMessage", str_save_value[13]);//不良项目，错误信息
                string postdata = ParseToString(p_dic_data);
                string responseString = null;
                if (DealPost(posturl_S170_RF, postdata, ref responseString, str_token) == false)
                {
                    str_error_log = responseString;
                    return false;
                }
                string[] responsestring_post = responseString.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');// count:13  or   14
                string str_result = responsestring_post[0].Replace("result:", "").ToLower();
                string str_error_code = responsestring_post[1].Replace("error:", "");
                if (str_result.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }
                str_error_log = responseString;
                return true;
            }
            catch (Exception ee)
            {
                str_error_log = "--Cloud up error:" + ee.Message.Replace("\r\n", "");
                return false;
            }
        }
        
        private bool DealPost_login(string posturl, string postdata, ref string responseString)//, string str_header_value)
        {
            try
            {
                //string str_header_key = "Authorization";
                //string str_header_value = "eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDEwNDY0OTV9.yoyntiAWucTlg2MA_n6KCVeRrcMyivXpRCBQM97lTOk";
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(posturl);
                req.Method = "POST";
                req.Timeout = 5800;//2020/12/12 -- 增加请求时间设定
                req.ContentType = "application/x-www-form-urlencoded";
                //req.Headers.Add(str_header_key, str_header_value);
                byte[] bytedata = Encoding.UTF8.GetBytes(postdata);
                int length = bytedata.Length;
                req.ContentLength = length;
                System.IO.Stream writer = req.GetRequestStream();
                writer.Write(bytedata, 0, length);
                writer.Close();
                var response = (System.Net.HttpWebResponse)req.GetResponse();
                var responseString_0 = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = responseString_0.ToString();
                return true;
            }
            catch (Exception ee)
            {
                responseString = "DealPost--" + ee.Message + "Error";
                return false;
            }
        }

        private bool DealPost_creat_token(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                //string str_header_key = "Authorization";
                //string str_header_value = "eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDEwNDY0OTV9.yoyntiAWucTlg2MA_n6KCVeRrcMyivXpRCBQM97lTOk";
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(posturl);
                req.Method = "POST";
                req.Timeout = 5800;//2020/12/12 -- 增加请求时间设定
                req.ContentType = "application/json"; //charset=utf-8";
                req.Headers.Add(str_header_key, str_header_value);

                byte[] bytedata = Encoding.UTF8.GetBytes(postdata);
                int length = bytedata.Length;
                req.ContentLength = length;
                System.IO.Stream writer = req.GetRequestStream();
                writer.Write(bytedata, 0, length);
                writer.Close();
                var response = (System.Net.HttpWebResponse)req.GetResponse();
                var responseString_0 = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = responseString_0.ToString();
                return true;
            }
            catch (Exception ee)
            {
                responseString = "DealPost--" + ee.Message + "Error";
                return false;
            }
        }

        private bool DealPost_creat_sn_uid(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                //string str_header_key = "Authorization";
                //string str_header_value = "eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDEwNDY0OTV9.yoyntiAWucTlg2MA_n6KCVeRrcMyivXpRCBQM97lTOk";
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(posturl);
                req.Method = "POST";
                req.Timeout = 5800;//2020/12/12 -- 增加请求时间设定
                req.ContentType = "application/json";//"application/x-www-form-urlencoded";
                req.Headers.Add(str_header_key, str_header_value);
                byte[] bytedata = Encoding.UTF8.GetBytes(postdata);
                int length = bytedata.Length;
                req.ContentLength = length;
                System.IO.Stream writer = req.GetRequestStream();
                writer.Write(bytedata, 0, length);
                writer.Close();
                var response = (System.Net.HttpWebResponse)req.GetResponse();
                var responseString_0 = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = responseString_0.ToString();
                return true;
            }
            catch (Exception ee)
            {
                responseString = "DealPost--" + ee.Message + "Error";
                return false;
            }
        }

        private bool DealPost(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                //string str_header_key = "Authorization";
                //string str_header_value = "eyJhbGciOiJIUzI1NiIsIlR5cGUiOiJKd3QiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJyZWRiZWUiLCJleHAiOjE2NDEwNDY0OTV9.yoyntiAWucTlg2MA_n6KCVeRrcMyivXpRCBQM97lTOk";
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(posturl);
                req.Method = "POST";
                req.Timeout = 5800;//2020/12/12 -- 增加请求时间设定
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add(str_header_key, str_header_value);
                byte[] bytedata = Encoding.UTF8.GetBytes(postdata);
                int length = bytedata.Length;
                req.ContentLength = length;
                System.IO.Stream writer = req.GetRequestStream();
                writer.Write(bytedata, 0, length);
                writer.Close();
                var response = (System.Net.HttpWebResponse)req.GetResponse();
                var responseString_0 = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = responseString_0.ToString();
                return true;
            }
            catch (Exception ee)
            {
                responseString = "DealPost--" + ee.Message + "Error";
                return false;
            }
        }

        private string ParseToString(IDictionary<string, string> parameters)
        {
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);
            return content;
        }
    }


    public class Cloud_DB_ver
    {
        public string ver()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    /// <summary>
    /// PCBA电压电流测试
    /// </summary>
    public class T010_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_voltage_sensor1;
        public string str_voltage_sensor2;
        public string str_voltage_sensor3;
        public string str_voltage_wifi;
        public string str_voltage_mcu;
        public string str_voltage_rf;
        public string str_voltage_vcc_io_3v0;
        public string str_voltage_vcc_io_1v8;
        public string str_voltage_vcc_core;
        public string str_voltage_ddr3;
        public string str_voltage_vddpwrc;
        public string str_voltage_led2_vcc;
        public string str_mic_manual;
        public string str_mic_result;
        public string str_Mic_amp_max_abs;
        public string str_Mic_power_delta;
        public string str_ping_result;
        public string str_ping_delay;
        public string str_work_current;
        public string str_rf_mhz;
        public string str_rf_db;
        public string str_temperature_reading;
    }

    /// <summary>
    /// PCBA 功能测试
    /// </summary>
    public class T020_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_charge_current;
        public string str_button;
        public string str_led;
        public string str_vlc_video;
        public string str_ir_cut;
        public string str_ir_led;
        public string str_audio;
        public string str_motion;
        public string str_Light_read;
        public string str_battery_level;
    }

    /// <summary>
    /// WM_Buttom 挂墙板pcba测试
    /// </summary>
    public class T030_save_value
    {
        public string str_Sn;
        public string str_sn_type;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_work_current;
        public string str_rf_tx_mHz;
        public string str_rf_tx_db;
        public string str_button_led;
        public string str_standby_current;
    }

    /// <summary>
    /// 半成品功耗测试
    /// </summary>
    public class T040_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_work_current;
        public string str_reset_button;
        public string str_standby_current;
    }

    /// <summary>
    /// 镜头测试
    /// </summary>
    public class T050_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_top_left;
        public string str_top_right;
        public string str_center;
        public string str_bottom_left;
        public string str_bottom_right;
        public string str_video_check;
        public string str_video_photo;
        public string str_dark_corner;
        public string str_IR_CUT;
        public string str_IR_LED;
        public string str_IR_video_check;
        public string str_IR_video_photo;
    }

    /// <summary>
    /// 整机功能测试1
    /// </summary>
    public class T060_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_Sensitivity_result;
        public string str_Motion;
        public string str_Button;
        public string str_Led;
        public string str_Audio_consistency;
        public string str_Voltage_read;
        public string str_Temperature_read;
        public string str_Audio;
        public string str_mic_manual;
        public string str_mic_result;
        public string str_Mic_amp_max_abs;
        public string str_Mic_power_delta;
    }

    /// <summary>
    /// 整机功能测试2
    /// </summary>
    public class T070_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_Light_read;
        public string str_vlc_rtsp;
        public string str_up_Packet_Loss;
        public string str_up_rate;
        public string str_down_Packet_Loss;
        public string str_down_rate;
        public string str_rf_mhz;
        public string str_rf_db;
    }

    /// <summary>
    /// SN、UID申请与写入
    /// </summary>
    public class T080_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_sn;
        public string str_mac;
        public string str_uid;
        public string print_sn;
    }

    /// <summary>
    /// 电量检查与充电指示
    /// </summary>
    public class T090_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_HW_Ver;
        public string str_battery_level;
        public string str_led_indicator;
        public string str_charging_mode;
    }

    /// <summary>
    /// 设备信息检查
    /// </summary>
    public class T100_save_value
    {
        public string str_Rn;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_HW_Ver;
        public string str_mcu_version;
        public string str_sn;
        public string str_uid;
        public string str_mac;
        public string str_final_photo;
        public string str_final_battery_voltage;
        public string str_final_reset;
    }

    /// <summary>
    /// WM_Buttom 挂墙板整机测试
    /// </summary>
    public class T110_save_value
    {
        public string str_Sn;
        public string str_sn_type;
        public string str_Start_Date;
        public string str_End_Date;
        public string str_Pc_name;
        public string str_pc_tool_ver;
        public string str_factoryId_31;
        public string str_productId32;
        public string str_modeId33;
        public string str_colorId34;
        public string str_productType35;
        public string str_ng_item;
        public string str_test_result;
        public string str_test_mode;
        public string str_read_tagnumber;
        public string str_write_tagnumber;
        public string str_FW_Ver;
        public string str_button_led;
        public string str_sn_compare;
        public string str_read_sn;
    }

    public struct _line_production_data
    {
        public string str_banbie;
        public int int_tourushu; //投入数
        public int int_canchushu; //产出数---等于最后一次OK数
        public int int_ngshu; //中间变量，投入数-str_ngshu=一次OK数
        public int int_zongtourushu;// '总投入数
        public int int_zongokshu;// '总ok数
        public int int_zongngshu;//'总NG数
        public int int_yiciokshu;// '一次OK数

        public string str_A;// '分类A
        public string str_B;//'分类B
    }
    public class System_DB
    {
        #region T010 PCBA电压测试站
        public bool T010_data_save(object obj, ref string str_error)
        {
            try
            {
                T010_save_value save_value = (T010_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t010_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `voltage_sensor1`, `voltage_sensor2`, `voltage_sensor3`, `voltage_wifi`, `voltage_mcu`, `voltage_rf`" +
                            $", `voltage_vcc_io_3v0`, `voltage_vcc_io_1v8`, `voltage_vcc_core`, `voltage_ddr3`, `voltage_vddpwrc`, `voltage_led2_vcc`" +
                            $", `pcba_mic_manual`, `pcba_mic`, `pcba_mic_amp`, `pcba_mic_power`, `ping_result`, `ping_delay`" +
                            $", `work_current`, `rf_tx_mHz`, `rf_tx_db`, `temperature_reading`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_voltage_sensor1}', '{save_value.str_voltage_sensor2}', '{save_value.str_voltage_sensor3}', '{save_value.str_voltage_wifi}'" +
                            $", '{save_value.str_voltage_mcu}', '{save_value.str_voltage_rf}', '{save_value.str_voltage_vcc_io_3v0}', '{save_value.str_voltage_vcc_io_1v8}'" +
                            $", '{save_value.str_voltage_vcc_core}', '{save_value.str_voltage_ddr3}', '{save_value.str_voltage_vddpwrc}', '{save_value.str_voltage_led2_vcc}'" +
                            $", '{save_value.str_mic_manual}', '{save_value.str_mic_result}', '{save_value.str_Mic_amp_max_abs}', '{save_value.str_Mic_power_delta}', '{save_value.str_ping_result}'" +
                            $", '{save_value.str_ping_delay}', '{save_value.str_work_current}', '{save_value.str_rf_mhz}', '{save_value.str_rf_db}'" +
                            $", '{save_value.str_temperature_reading}');";
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
                str_error = ee.Message;
                return false;
            }
        }

        public bool T010_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t010_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T020 PCBA功能测试1站
        public bool T020_data_save(object obj, ref string str_error)
        {
            try
            {
                T020_save_value save_value = (T020_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t020_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `pcaba_charge_current`, `pcba_button`, `pcba_led`, `pcba_vlc_video`, `pcba_ir_cut`, `pcba_ir_led`" +
                            $", `pcba_audio`, `pcba_motion`, `pcba_Light_read`, `pcba_battery_level`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_charge_current}', '{save_value.str_button}', '{save_value.str_led}', '{save_value.str_vlc_video}'" +
                            $", '{save_value.str_ir_cut}', '{save_value.str_ir_led}', '{save_value.str_audio}', '{save_value.str_motion}'" +
                            $", '{save_value.str_Light_read}', '{save_value.str_battery_level}')";
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool T020_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t020_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T030 WM-Button 挂墙板PCBA测试站
        public bool T030_data_save(object obj, ref string str_error)
        {
            try
            {
                T030_save_value save_value = (T030_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t030_test_report`(`sn`, `sn_type`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `pcba_work_current`, `pcba_rf_tx_mHz`, `pcba_rf_tx_db`, `pcba_button_led`, `pcba_standby_current`)" +
                            $" VALUES('{save_value.str_Sn}', '{save_value.str_sn_type}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_work_current}', '{save_value.str_rf_tx_mHz}', '{save_value.str_rf_tx_db}', '{save_value.str_button_led}'" +
                            $", '{save_value.str_standby_current}')";
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
                str_error = ee.Message;
                return false;
            }
        }

        public bool T030_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = "";
                    str_find_mdb = $"SELECT * FROM `t030_test_report`" +
                                   $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `sn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }

            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T040 半成品功耗测试站
        public bool T040_data_save(object obj, ref string str_error)
        {
            try
            {
                T040_save_value save_value = (T040_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t040_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `work_current`, `reset_button`, `standby_current`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_work_current}', '{save_value.str_reset_button}', '{save_value.str_standby_current}')";
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
                str_error = ee.Message;
                return false;
            }
        }

        public bool T040_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = "";
                    str_find_mdb = $"SELECT * FROM `t040_test_report`" +
                                   $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }

            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T050 镜头测试站
        public bool T050_data_save(object obj, ref string str_error)
        {
            try
            {
                T050_save_value save_value = (T050_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t050_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `top_left`, `top_right`, `center`, `bottom_left`, `bottom_right`, `video_check`" +
                            $", `video_photo`, `dark_corner`, `IR_CUT`, `IR_LED`, `IR_video_check`, `IR_video_photo`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_top_left}', '{save_value.str_top_right}', '{save_value.str_center}', '{save_value.str_bottom_left}'" +
                            $", '{save_value.str_bottom_right}', '{save_value.str_video_check}', '{save_value.str_video_photo}', '{save_value.str_dark_corner}'" +
                            $", '{save_value.str_IR_CUT}', '{save_value.str_IR_LED}', '{save_value.str_IR_video_check}', '{save_value.str_IR_video_photo}')";
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
                str_error = ee.Message;
                return false;
            }
        }

        public bool T050_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = "";
                    str_find_mdb = $"SELECT * FROM `t050_test_report`" +
                                   $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
                
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T060 整机功能测试1站
        public bool T060_data_save(object obj, ref string str_error_log)
        {
            try
            {
                T060_save_value save_value = (T060_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t060_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `sensitivity_test`, `product_motion`, `product_button`, `product_led`, `product_audio_consistency`, `product_voltage_reading`" +
                            $", `product_temperature_reading`, `product_audio`, `product_mic_manual`, `product_mic`, `product_mic_amp`, `product_mic_power`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_Sensitivity_result}', '{save_value.str_Motion}', '{save_value.str_Button}', '{save_value.str_Led}'" +
                            $", '{save_value.str_Audio_consistency}', '{save_value.str_Voltage_read}', '{save_value.str_Temperature_read}', '{save_value.str_Audio}'" +
                            $", '{save_value.str_mic_manual}', '{save_value.str_mic_result}', '{save_value.str_Mic_amp_max_abs}', '{save_value.str_Mic_power_delta}');";
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
            catch (Exception e)
            {
                str_error_log = e.Message;
                return false;
            }
        }

        public bool T060_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t060_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}';";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T070 整机功能测试2站
        public bool T070_data_save(object obj, ref string str_error)
        {
            try
            {
                T070_save_value save_value = (T070_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t070_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `Light_read`, `vlc_rtsp`, `up_Packet_Loss`, `up_rate`, `down_Packet_Loss`, `down_rate`, `rf_mhz`, `rf_db`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_Light_read}', '{save_value.str_vlc_rtsp}', '{save_value.str_up_Packet_Loss}', '{save_value.str_up_rate}', '{save_value.str_down_Packet_Loss}'" +
                            $", '{save_value.str_down_rate}', '{save_value.str_rf_mhz}', '{save_value.str_rf_db}')";
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool T070_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t070_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T080 SN、UID申请与写入 测站
        public bool T080_data_save(object obj, ref string str_error)
        {
            try
            {
                T080_save_value save_value = (T080_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t080_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `sn`, `mac`, `uid`, `print_sn`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_sn}', '{save_value.str_mac}', '{save_value.str_uid}', '{save_value.print_sn}')";
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool T080_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t080_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T090 电量检查与充电指示 测站
        public bool T090_data_save(object obj, ref string str_error)
        {
            try
            {
                T090_save_value save_value = (T090_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t090_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `hw_version`, `battery_level`, `led_indicator`, `charging_mode`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_HW_Ver}', '{save_value.str_battery_level}', '{save_value.str_led_indicator}', '{save_value.str_charging_mode}')";
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool T090_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t090_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T100 设备信息检查 测站
        public bool T100_data_save(object obj, ref string str_error)
        {
            try
            {
                T100_save_value save_value = (T100_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t100_test_report`(`rn`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `hw_version`, `mcu_version`, `mac`, `sn`, `uid`, `final_photo`" +
                            $", `final_battery_level`, `final_reset`)" +
                            $" VALUES('{save_value.str_Rn}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_HW_Ver}', '{save_value.str_mcu_version}', '{save_value.str_mac}', '{save_value.str_sn}'" +
                            $", '{save_value.str_uid}', '{save_value.str_final_photo}', '{save_value.str_final_battery_voltage}', '{save_value.str_final_reset}')";
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool T100_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = $"SELECT * FROM `t100_test_report` " +
                                          $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `rn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        #region T110 WM-Button 挂墙板整机测试站
        public bool T110_data_save(object obj, ref string str_error)
        {
            try
            {
                T110_save_value save_value = (T110_save_value)obj;
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO `t110_test_report`(`sn`, `sn_type`, `start_test_time`, `end_test_time`, `PC_name`, `pc_tool_version`" +
                            $", `factoryId`, `productId`, `modeId`, `colorId`, `productType`" +
                            $", `ng_items`, `test_result`, `test_mode`, `read_tagnumber`, `write_tagnumber`, `fw_version`" +
                            $", `button_led`, `sn_compare`, `read_sn`)" +
                            $" VALUES('{save_value.str_Sn}', '{save_value.str_sn_type}', '{save_value.str_Start_Date}', '{save_value.str_End_Date}' ,'{save_value.str_Pc_name}'" +
                            $", '{save_value.str_pc_tool_ver}', '{save_value.str_factoryId_31}', '{save_value.str_productId32}', '{save_value.str_modeId33}'" +
                            $", '{save_value.str_colorId34}', '{save_value.str_productType35}', '{save_value.str_ng_item}', '{save_value.str_test_result}'" +
                            $", '{save_value.str_test_mode}', '{save_value.str_read_tagnumber}', '{save_value.str_write_tagnumber}', '{save_value.str_FW_Ver}'" +
                            $", '{save_value.str_button_led}', '{save_value.str_sn_compare}', '{save_value.str_read_sn}')";
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
                str_error = ee.Message;
                return false;
            }
        }

        public bool T110_data_find(ref System.Data.DataTable dt, string Start_Time, string End_Time, bool bool_serial, string str_serial, bool bool_test_result, string str_result, bool bool_zuihou)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\LocalMachine.db"))
                {
                    conn.Open();
                    string str_find_mdb = "";
                    str_find_mdb = $"SELECT * FROM `t110_test_report`" +
                                   $"WHERE `start_test_time` between '{Start_Time}' and '{End_Time}'";
                    if (bool_serial)
                    {
                        str_find_mdb += "and `rn`='" + str_serial + "'";
                    }
                    if (bool_test_result)
                    {
                        str_find_mdb += "and `test_result`='" + str_result + "'";
                    }
                    if (bool_zuihou)
                    {
                        string str_t = str_find_mdb;
                        str_t = str_find_mdb.Replace("*", "max(`id`)") + "group by `sn`";
                        str_find_mdb += "and `id` in(" + str_t + ")";
                    }
                    using (SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(str_find_mdb, conn))
                    {
                        mAdapter.Fill(dt);
                        return true;
                    }
                }

            }
            catch (Exception ee)
            {
                return false;
            }
        }
        #endregion

        private System.Data.OleDb.OleDbConnection S030_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_pcba.db;Persist Security Info=False");
        private System.Data.OleDb.OleDbConnection S130_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_focus.db;Persist Security Info=False");
        private System.Data.OleDb.OleDbConnection S140_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_product.db;Persist Security Info=False");
        private System.Data.OleDb.OleDbConnection S150_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_sn_uid_print.db;Persist Security Info=False");
        private System.Data.OleDb.OleDbConnection S160_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_rf_check_sn_uid.db;Persist Security Info=False");
        private System.Data.OleDb.OleDbConnection S170_data = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=database.2;Data Source=customer_sensitivity.db;Persist Security Info=False");

        public bool S130_production_show(ref int int_F060_OK, ref int int_F060_NG)
        {
            try
            {
                //1.获取当前时间
                DateTime temp_datetime_now = DateTime.Now;
                //2.获取日期和时间;
                string str_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                string str_time = string.Format("{0:HH:mm:ss}", DateTime.Now);//_HH-mm-ss

                //--投入数-----------------------------------
                string str_tourushu = "";     //投入数 -- 投入台数,不计算重复数
                string str_canchushu = "";    //产出数---也就是最后一次为OK的计数

                str_tourushu = "select DISTINCT pcbaRn from Focus_S130 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                str_canchushu = "select * from Focus_S130 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                //-----以下求产出数(最后一次OK数)-----------------------------------
                string str_temp = str_canchushu; //记录产出数临时值
                str_temp = str_temp.Replace("*", "max(Date)") + " group by pcbaRn";
                str_canchushu += " and Date in (" + str_temp + ")";
                str_canchushu += " and result='OK'";
                //============================================================================
                int dt_num = 0;//-----投入的数量
                _line_production_data line_production_data = new _line_production_data();
                if (S130_data_Datatable_num(str_tourushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_tourushu = dt_num;

                dt_num = 0;  //--------投入数量 ----产出数量(投入数量OK数)
                if (S130_data_Datatable_num(str_canchushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_canchushu = dt_num;
                int_F060_OK = line_production_data.int_canchushu;
                int_F060_NG = line_production_data.int_tourushu - line_production_data.int_canchushu;
                //==========================================================================
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool S130_data_Datatable_num(string str_sql, ref int dt_num)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str_sql, S130_data);
                da.Fill(dt);
                dt_num = dt.Rows.Count;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        //--------S140---------------------------
        public bool S140_production_show(ref int int_F070_OK, ref int int_F070_NG)
        {
            try
            {
                //1.获取当前时间
                DateTime temp_datetime_now = DateTime.Now;
                //2.获取日期和时间;
                string str_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                string str_time = string.Format("{0:HH:mm:ss}", DateTime.Now);//_HH-mm-ss

                //--投入数-----------------------------------
                string str_tourushu = "";     //投入数 -- 投入台数,不计算重复数
                string str_canchushu = "";    //产出数---也就是最后一次为OK的计数

                str_tourushu = "select DISTINCT pcbaRn from PRODUCT_Function_Test where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                str_canchushu = "select * from PRODUCT_Function_Test where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                //-----以下求产出数(最后一次OK数)-----------------------------------
                string str_temp = str_canchushu; //记录产出数临时值
                str_temp = str_temp.Replace("*", "max(Date)") + " group by pcbaRn";
                str_canchushu += " and Date in (" + str_temp + ")";
                str_canchushu += " and result='OK'";
                //============================================================================
                int dt_num = 0;//-----投入的数量
                _line_production_data line_production_data = new _line_production_data();
                if (S140_datatable_num(str_tourushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_tourushu = dt_num;

                dt_num = 0;  //--------投入数量 ----产出数量(投入数量OK数)
                if (S140_datatable_num(str_canchushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_canchushu = dt_num;
                int_F070_OK = line_production_data.int_canchushu;
                int_F070_NG = line_production_data.int_tourushu - line_production_data.int_canchushu;
                //==========================================================================
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool S140_datatable_num(string str_sql, ref int dt_num)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str_sql, S140_data);
                da.Fill(dt);
                dt_num = dt.Rows.Count;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        //--------S150---------------------------
        public bool S150_production_show(ref int int_F070_OK, ref int int_F070_NG)
        {
            try
            {
                //1.获取当前时间
                DateTime temp_datetime_now = DateTime.Now;
                //2.获取日期和时间;
                string str_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                string str_time = string.Format("{0:HH:mm:ss}", DateTime.Now);//_HH-mm-ss

                //--投入数-----------------------------------
                string str_tourushu = "";     //投入数 -- 投入台数,不计算重复数
                string str_canchushu = "";    //产出数---也就是最后一次为OK的计数

                str_tourushu = "select DISTINCT pcbaRn from SN_S150 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                str_canchushu = "select * from SN_S150 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                //-----以下求产出数(最后一次OK数)-----------------------------------
                string str_temp = str_canchushu; //记录产出数临时值
                str_temp = str_temp.Replace("*", "max(Date)") + " group by pcbaRn";
                str_canchushu += " and Date in (" + str_temp + ")";
                str_canchushu += " and result='OK'";
                //============================================================================
                int dt_num = 0;//-----投入的数量
                _line_production_data line_production_data = new _line_production_data();
                if (S150_datatable_num(str_tourushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_tourushu = dt_num;

                dt_num = 0;  //--------投入数量 ----产出数量(投入数量OK数)
                if (S150_datatable_num(str_canchushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_canchushu = dt_num;
                int_F070_OK = line_production_data.int_canchushu;
                int_F070_NG = line_production_data.int_tourushu - line_production_data.int_canchushu;
                //==========================================================================
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool S150_datatable_num(string str_sql, ref int dt_num)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str_sql, S150_data);
                da.Fill(dt);
                dt_num = dt.Rows.Count;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        //----------------S160--------------
        public bool S160_production_show(ref int int_F070_OK, ref int int_F070_NG)
        {
            try
            {
                //1.获取当前时间
                DateTime temp_datetime_now = DateTime.Now;
                //2.获取日期和时间;
                string str_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                string str_time = string.Format("{0:HH:mm:ss}", DateTime.Now);//_HH-mm-ss

                //--投入数-----------------------------------
                string str_tourushu = "";     //投入数 -- 投入台数,不计算重复数
                string str_canchushu = "";    //产出数---也就是最后一次为OK的计数

                str_tourushu = "select DISTINCT pcbaRn from RF_S160 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                str_canchushu = "select * from RF_S160 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                //-----以下求产出数(最后一次OK数)-----------------------------------
                string str_temp = str_canchushu; //记录产出数临时值
                str_temp = str_temp.Replace("*", "max(Date)") + " group by pcbaRn";
                str_canchushu += " and Date in (" + str_temp + ")";
                str_canchushu += " and result='OK'";
                //============================================================================
                int dt_num = 0;//-----投入的数量
                _line_production_data line_production_data = new _line_production_data();
                if (S160_datatable_num(str_tourushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_tourushu = dt_num;

                dt_num = 0;  //--------投入数量 ----产出数量(投入数量OK数)
                if (S160_datatable_num(str_canchushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_canchushu = dt_num;
                int_F070_OK = line_production_data.int_canchushu;
                int_F070_NG = line_production_data.int_tourushu - line_production_data.int_canchushu;
                //==========================================================================
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool S160_datatable_num(string str_sql, ref int dt_num)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str_sql, S160_data);
                da.Fill(dt);
                dt_num = dt.Rows.Count;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        //----------------S170--------------
        public bool S170_production_show(ref int int_S170_OK, ref int int_S170_NG)
        {
            try
            {
                //1.获取当前时间
                DateTime temp_datetime_now = DateTime.Now;
                //2.获取日期和时间;
                string str_date = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                string str_time = string.Format("{0:HH:mm:ss}", DateTime.Now);//_HH-mm-ss

                //--投入数-----------------------------------
                string str_tourushu = "";     //投入数 -- 投入台数,不计算重复数
                string str_canchushu = "";    //产出数---也就是最后一次为OK的计数

                str_tourushu = "select DISTINCT pcbaRn from SN_S170 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                str_canchushu = "select * from SN_S170 where Date>= CDate('" + str_date + " 00:00:00') And Date <= CDate('" + str_date + " 23:59:59')";
                //-----以下求产出数(最后一次OK数)-----------------------------------
                string str_temp = str_canchushu; //记录产出数临时值
                str_temp = str_temp.Replace("*", "max(Date)") + " group by pcbaRn";
                str_canchushu += " and Date in (" + str_temp + ")";
                str_canchushu += " and result='OK'";
                //============================================================================
                int dt_num = 0;//-----投入的数量
                _line_production_data line_production_data = new _line_production_data();
                if (S170_datatable_num(str_tourushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_tourushu = dt_num;

                dt_num = 0;  //--------投入数量 ----产出数量(投入数量OK数)
                if (S170_datatable_num(str_canchushu, ref dt_num) == false)
                {
                    return false;
                }
                line_production_data.int_canchushu = dt_num;
                int_S170_OK = line_production_data.int_canchushu;
                int_S170_NG = line_production_data.int_tourushu - line_production_data.int_canchushu;
                //==========================================================================
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        private bool S170_datatable_num(string str_sql, ref int dt_num)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str_sql, S170_data);
                da.Fill(dt);
                dt_num = dt.Rows.Count;
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

    }
}
