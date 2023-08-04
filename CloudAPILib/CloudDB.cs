using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CloudAPILib
{
    public class System_Cloud
    {
        private string posturl_login = "http://120.78.224.26:8080/XDC01Management/token/login";

        private string posturl_compare_product_id = "http://120.78.224.26:8080/XDC01Management/token/create";

        private string posturl_creat_SN_UID = "http://120.78.224.26:8080/XDC01Management/api/product/createKey";

        private string posturl_check_SN_UID = "http://120.78.224.26:8080/XDC01Management/api/product/checkKey";

        private string posturl_S030_pcba = "http://120.78.224.26:8080/XDC01Management/product-test-report/pcba";

        private string posturl_S130_focus = "http://120.78.224.26:8080/XDC01Management/product-test-report/focusing";

        private string posturl_S140_product = "http://120.78.224.26:8080/XDC01Management/product-test-report/product";

        private string posturl_S150_SN_UID_MAC = "http://120.78.224.26:8080/XDC01Management/product-test-report/key";

        private string posturl_S160_Final = "http://120.78.224.26:8080/XDC01Management/product-test-report/final";

        private string posturl_S170_RF = "http://120.78.224.26:8080/XDC01Management/product-test-report/rfTest";

        private string posturl_find_product_key = "http://120.78.224.26:8080/XDC01Management/api/findProductKeyByMac";

        private string posturl_Unbind_MacAddress = "http://120.78.224.26:8080/XDC01Management/api/unbindMacAddress";

        private string str_header_key = "Authorization";

        private string str_path = Application.StartupPath + "\\Log_file\\cloud\\" + $"{DateTime.Now:yyyy.MM.dd}" + ".txt";

        public bool cloud_up_user_password_check(List<string> str_save_value, ref string str_error_log, ref string str_token)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("username", str_save_value[0]);
                dictionary.Add("password", str_save_value[1]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost_login(posturl_login, postdata, ref responseString))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "check user error_code:" + text2 + " Fail";
                    return false;
                }

                str_token = array[2].Replace("token:", "").Replace("data:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "check user error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_compare_product_id(List<string> str_save_value, ref string str_error_log, ref List<string> ret_product_id, string str_token)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("factoryId", str_save_value[0]);
                dictionary.Add("productId", str_save_value[1]);
                dictionary.Add("modelId", str_save_value[2]);
                dictionary.Add("colorId", str_save_value[3]);
                dictionary.Add("productType", str_save_value[4]);
                string postdata = JsonConvert.SerializeObject((object)dictionary);
                string responseString = null;
                if (!DealPost_creat_token(posturl_compare_product_id, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                if (!text.Contains("true"))
                {
                    str_error_log = "check user error_code:" + responseString + " Fail";
                    return false;
                }

                string text2 = array[1].Replace("error:", "");
                string item = array[2].Replace("data:", "").Replace("token:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                ret_product_id.Add(item);
                ret_product_id.Add(array[3].Replace("factoryId:", ""));
                ret_product_id.Add(array[4].Replace("productId:", ""));
                ret_product_id.Add(array[5].Replace("modelId:", ""));
                ret_product_id.Add(array[6].Replace("colorId:", ""));
                ret_product_id.Add(array[7].Replace("createTime:", ""));
                ret_product_id.Add(array[8].Replace("createTime:", ""));
                ret_product_id.Add(array[9].Replace("expiresTime:", ""));
                ret_product_id.Add(array[10].Replace("expiresTime:", ""));
                ret_product_id.Add(array[11].Replace("status:", ""));
                if (ret_product_id[9] != "true")
                {
                    str_error_log = "check user error_code:" + responseString + " Fail";
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "check user error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_Creat_SN_UID(List<string> str_save_value, ref string str_error_log, ref string str_ret_sn, ref string str_ret_macaddress, ref string str_uid, string str_token)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pcbaRn", str_save_value[0]);
                dictionary.Add("macAddress", str_save_value[1]);
                dictionary.Add("colorId", str_save_value[2]);
                dictionary.Add("factoryId", str_save_value[3]);
                dictionary.Add("productId", str_save_value[4]);
                dictionary.Add("modelId", str_save_value[5]);
                dictionary.Add("productType", str_save_value[6]);
                if (str_save_value[7] != "")
                {
                    dictionary.Add("customerId", str_save_value[6]);
                }
                string postdata = JsonConvert.SerializeObject((object)dictionary);
                string responseString = null;
                if (!DealPost_creat_sn_uid(posturl_creat_SN_UID, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "post SN UID error_code:" + text2 + " Fail";
                    return false;
                }

                str_ret_sn = array[2].Replace("data:", "").Replace("sn:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                str_ret_macaddress = array[3].Replace("macAddress:", "");
                str_uid = array[4].Replace("uid:", "");
                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "check user error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_Check_SN_UID(List<string> str_save_value, ref string str_error_log, ref string str_ret_sn, ref string str_ret_macaddress, ref string str_uid, string str_token)
        {
            string responseString = null;
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("sn", str_save_value[0]);
                dictionary.Add("macAddress", str_save_value[1]);
                dictionary.Add("uid", str_save_value[2]);
                dictionary.Add("pcbaRn", str_save_value[3]);
                string postdata = JsonConvert.SerializeObject((object)dictionary);
                if (!DealPost_creat_sn_uid(posturl_check_SN_UID, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "result:false_error_code:" + text2;
                    return false;
                }

                str_ret_sn = array[2].Replace("data:", "").Replace("sn:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                str_ret_macaddress = array[3].Replace("macAddress:", "");
                str_uid = array[4].Replace("uid:", "");
                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "check user error:[" + responseString + "]" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_Find_Product_Key(List<string> str_save_value, ref string str_error_log, string str_token, ref string str_result, ref string str_ret_pcbaRN, ref string str_ret_SN, ref string str_ret_mac_Address, ref string str_ret_UID)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("macAddress", str_save_value[0]);
                dictionary.Add("colorId", str_save_value[1]);
                dictionary.Add("factoryId", str_save_value[2]);
                dictionary.Add("productId", str_save_value[3]);
                dictionary.Add("modelId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_find_product_key, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                str_result = array[0].Replace("result:", "").ToLower();
                string text = array[1].Replace("error:", "");
                if (text.Trim().Length > 0)
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }

                str_ret_pcbaRN = array[2].Replace("data:", "").Replace("pcbaRn:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                str_ret_SN = array[3].Replace("data:", "").Replace("sn:", "").Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
                str_ret_mac_Address = array[4].Replace("macAddress:", "");
                str_ret_UID = array[5].Replace("uid:", "");
                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_Unbind_MacAddress(List<string> str_save_value, ref string str_error_log, string str_token)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("macAddress", str_save_value[0]);
                dictionary.Add("colorId", str_save_value[1]);
                dictionary.Add("factoryId", str_save_value[2]);
                dictionary.Add("productId", str_save_value[3]);
                dictionary.Add("modelId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_Unbind_MacAddress, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
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

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pcbaRn", str_save_value[0]);
                dictionary.Add("factoryId", str_save_value[1]);
                dictionary.Add("productId", str_save_value[2]);
                dictionary.Add("modelId", str_save_value[3]);
                dictionary.Add("colorId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                dictionary.Add("tagNumber", str_save_value[6]);
                dictionary.Add("voltageMcu", str_save_value[7]);
                dictionary.Add("voltageWifi", str_save_value[8]);
                dictionary.Add("voltageVccCore", str_save_value[9]);
                dictionary.Add("voltageDdr", str_save_value[10]);
                dictionary.Add("voltageRtc", str_save_value[11]);
                dictionary.Add("voltageSensor1", str_save_value[12]);
                dictionary.Add("voltageSensor2", str_save_value[13]);
                dictionary.Add("voltageBattery", str_save_value[14]);
                dictionary.Add("voltageDsp", str_save_value[15]);
                dictionary.Add("voltageDdr2", str_save_value[16]);
                dictionary.Add("voltageCore", str_save_value[17]);
                dictionary.Add("voltageImx225", str_save_value[18]);
                dictionary.Add("pcbaMic", str_save_value[19]);
                dictionary.Add("pcbaMicAmp", str_save_value[20]);
                dictionary.Add("ppcbaMicPower", str_save_value[21]);
                dictionary.Add("pcbaTemperatureReading", str_save_value[22]);
                dictionary.Add("pcbaLightReading", str_save_value[23]);
                dictionary.Add("pcbaVoltageReading", str_save_value[24]);
                dictionary.Add("pingStat", str_save_value[25]);
                dictionary.Add("micMaxAbs", str_save_value[26]);
                dictionary.Add("micDelta", str_save_value[27]);
                dictionary.Add("pcbaAudio", str_save_value[28]);
                dictionary.Add("vlcVideoCheck", str_save_value[29]);
                dictionary.Add("irCheck", str_save_value[30]);
                dictionary.Add("pcbaLed", str_save_value[31]);
                dictionary.Add("pcbaMotion", str_save_value[32]);
                dictionary.Add("pcbaButton", str_save_value[33]);
                dictionary.Add("pcbaCloudUp", str_save_value[34]);
                dictionary.Add("pcbaWorkingCurrent", str_save_value[35]);
                dictionary.Add("pcbaStandbyCurrent", str_save_value[36]);
                dictionary.Add("pcbaChargeCurrent", str_save_value[37]);
                dictionary.Add("pcbaOprNumber", str_save_value[38]);
                dictionary.Add("pcbaVersion", str_save_value[39]);
                dictionary.Add("result", str_save_value[40]);
                dictionary.Add("failMessage", str_save_value[41]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_S030_pcba, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_S130_focus(string fullPath, List<string> str_save_value, ref string str_error_log, string token_1)
        {
            try
            {
                string requestUri = posturl_S130_focus;
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(15.0);
                httpClient.MaxResponseContentBufferSize = 5242880L;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string arg = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
                multipartFormDataContent.Headers.Add("ContentType", $"multipart/form-data, boundary={arg}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_1);
                str_save_value[7] = Path.GetFileName(fullPath);
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

                multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(fullPath)), "file", fullPath);
                multipartFormDataContent.Add(new StringContent(str_save_value[0]), "pcbaRn");
                multipartFormDataContent.Add(new StringContent(str_save_value[1]), "factoryId");
                multipartFormDataContent.Add(new StringContent(str_save_value[2]), "productId");
                multipartFormDataContent.Add(new StringContent(str_save_value[3]), "modelId");
                multipartFormDataContent.Add(new StringContent(str_save_value[4]), "colorId");
                multipartFormDataContent.Add(new StringContent(str_save_value[5]), "productType");
                multipartFormDataContent.Add(new StringContent(str_save_value[6]), "tagNumber");
                multipartFormDataContent.Add(new StringContent(str_save_value[7]), "focusingPhoto");
                multipartFormDataContent.Add(new StringContent(str_save_value[8]), "focusingCenter");
                multipartFormDataContent.Add(new StringContent(str_save_value[9]), "focusingTopLeft");
                multipartFormDataContent.Add(new StringContent(str_save_value[10]), "focusingTopRight");
                multipartFormDataContent.Add(new StringContent(str_save_value[11]), "focusingBottomLeft");
                multipartFormDataContent.Add(new StringContent(str_save_value[12]), "focusingBottomRight");
                multipartFormDataContent.Add(new StringContent(str_save_value[13]), "focusingOprNumber");
                multipartFormDataContent.Add(new StringContent(str_save_value[14]), "focusingVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[15]), "focusingCloudUp");
                multipartFormDataContent.Add(new StringContent(str_save_value[16]), "imageDarkCornerResult");
                multipartFormDataContent.Add(new StringContent(str_save_value[17]), "result");
                multipartFormDataContent.Add(new StringContent(str_save_value[18]), "failMessage");
                HttpResponseMessage result = httpClient.PostAsync(requestUri, multipartFormDataContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    string result2 = result.Content.ReadAsStringAsync().Result;
                    string[] array = result2.Replace("{", "").Replace("}", "").Replace("\"", "")
                        .Split(',');
                    string text = array[0].Replace("result:", "").ToLower();
                    string text2 = array[1].Replace("error:", "");
                    if (text.Trim() != "true")
                    {
                        str_error_log = result2;
                        return false;
                    }

                    str_error_log = result2;
                    httpClient.Dispose();
                    multipartFormDataContent.Dispose();
                    return true;
                }

                httpClient.Dispose();
                multipartFormDataContent.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                txt_file_save(str_path, "system-error:" + ex.Message);
                str_error_log = ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        private bool txt_file_save(string str_file_path, string str_file_content)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(str_file_path);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (!File.Exists(str_file_path))
                {
                    FileStream fileStream = new FileStream(str_file_path, FileMode.Append, FileAccess.Write, FileShare.None);
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
                    streamWriter.WriteLine(str_file_content);
                    streamWriter.WriteLine("\r\n");
                    streamWriter.Close();
                    fileStream.Close();
                }
                else
                {
                    FileStream fileStream2 = new FileStream(str_file_path, FileMode.Append, FileAccess.Write, FileShare.None);
                    StreamWriter streamWriter2 = new StreamWriter(fileStream2);
                    streamWriter2.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
                    streamWriter2.WriteLine(str_file_content);
                    streamWriter2.WriteLine("\r\n");
                    streamWriter2.Close();
                    fileStream2.Close();
                }

                return true;
            }
            catch (Exception)
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

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pcbaRn", str_save_value[0]);
                dictionary.Add("factoryId", str_save_value[1]);
                dictionary.Add("productId", str_save_value[2]);
                dictionary.Add("modelId", str_save_value[3]);
                dictionary.Add("colorId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                dictionary.Add("tagNumber", str_save_value[6]);
                dictionary.Add("productTemperatureReading", str_save_value[7]);
                dictionary.Add("productLightReading", str_save_value[8]);
                dictionary.Add("productVoltageReading", str_save_value[9]);
                dictionary.Add("productAudio", str_save_value[10]);
                dictionary.Add("productLed", str_save_value[11]);
                dictionary.Add("productMicMaxAbs", str_save_value[12]);
                dictionary.Add("productMicDelta", str_save_value[13]);
                dictionary.Add("productMic", str_save_value[14]);
                dictionary.Add("productMicAmp", str_save_value[15]);
                dictionary.Add("productMicPower", str_save_value[16]);
                dictionary.Add("productVlcVideoCheck", str_save_value[17]);
                dictionary.Add("productIrCheck", str_save_value[18]);
                dictionary.Add("productButton", str_save_value[19]);
                dictionary.Add("productMotion", str_save_value[20]);
                dictionary.Add("upPacketLoss", str_save_value[21]);
                dictionary.Add("upRate", str_save_value[22]);
                dictionary.Add("downPacketLoss", str_save_value[23]);
                dictionary.Add("downRate", str_save_value[24]);
                dictionary.Add("rfResultmHz", str_save_value[25]);
                dictionary.Add("rfResultdb", str_save_value[26]);
                dictionary.Add("productCloudUp", str_save_value[27]);
                dictionary.Add("productMacAddress", str_save_value[28]);
                dictionary.Add("productOprNumber", str_save_value[29]);
                dictionary.Add("productVersion", str_save_value[30]);
                dictionary.Add("result", str_save_value[31]);
                dictionary.Add("failMessage", str_save_value[32]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_S140_product, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + text2 + " Fail";
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
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

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pcbaRn", str_save_value[0]);
                dictionary.Add("factoryId", str_save_value[1]);
                dictionary.Add("productId", str_save_value[2]);
                dictionary.Add("modelId", str_save_value[3]);
                dictionary.Add("colorId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                dictionary.Add("tagNumber", str_save_value[6]);
                dictionary.Add("sn", str_save_value[7]);
                dictionary.Add("uid", str_save_value[8]);
                dictionary.Add("mac", str_save_value[9]);
                dictionary.Add("printSn", str_save_value[10]);
                dictionary.Add("snCloudUp", str_save_value[11]);
                dictionary.Add("snOprNumber", str_save_value[12]);
                dictionary.Add("snVersion", str_save_value[13]);
                dictionary.Add("result", str_save_value[14]);
                dictionary.Add("failMessage", str_save_value[15]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_S150_SN_UID_MAC, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        public bool cloud_up_S160_final(string fullPath, List<string> str_save_value, ref string str_error_log, string token_1)
        {
            try
            {
                string requestUri = posturl_S160_Final;
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(15.0);
                httpClient.MaxResponseContentBufferSize = 5242880L;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string arg = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
                multipartFormDataContent.Headers.Add("ContentType", $"multipart/form-data, boundary={arg}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_1);
                str_save_value[22] = Path.GetFileName(fullPath);
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

                multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes(fullPath)), "file", fullPath);
                multipartFormDataContent.Add(new StringContent(str_save_value[0]), "pcbaRn");
                multipartFormDataContent.Add(new StringContent(str_save_value[1]), "factoryId");
                multipartFormDataContent.Add(new StringContent(str_save_value[2]), "productId");
                multipartFormDataContent.Add(new StringContent(str_save_value[3]), "modelId");
                multipartFormDataContent.Add(new StringContent(str_save_value[4]), "colorId");
                multipartFormDataContent.Add(new StringContent(str_save_value[5]), "productType");
                multipartFormDataContent.Add(new StringContent(str_save_value[6]), "tagNumber");
                multipartFormDataContent.Add(new StringContent(str_save_value[7]), "mcuVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[8]), "hardwareVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[9]), "fwVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[10]), "sensitivityTest");
                multipartFormDataContent.Add(new StringContent(str_save_value[11]), "checkSn");
                multipartFormDataContent.Add(new StringContent(str_save_value[12]), "checkMac");
                multipartFormDataContent.Add(new StringContent(str_save_value[13]), "checkUid");
                multipartFormDataContent.Add(new StringContent(str_save_value[14]), "finalBatteryLevel");
                multipartFormDataContent.Add(new StringContent(str_save_value[15]), "finalCloudUp");
                multipartFormDataContent.Add(new StringContent(str_save_value[16]), "finalReset");
                multipartFormDataContent.Add(new StringContent(str_save_value[17]), "checkFwVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[18]), "checkMcuVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[19]), "checkHwVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[20]), "finalOprNumber");
                multipartFormDataContent.Add(new StringContent(str_save_value[21]), "finalVersion");
                multipartFormDataContent.Add(new StringContent(str_save_value[22]), "finalPhoto");
                multipartFormDataContent.Add(new StringContent(str_save_value[23]), "result");
                multipartFormDataContent.Add(new StringContent(str_save_value[24]), "failMessage");
                HttpResponseMessage result = httpClient.PostAsync(requestUri, multipartFormDataContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    string result2 = result.Content.ReadAsStringAsync().Result;
                    string[] array = result2.Replace("{", "").Replace("}", "").Replace("\"", "")
                        .Split(',');
                    string text = array[0].Replace("result:", "").ToLower();
                    string text2 = array[1].Replace("error:", "");
                    if (text.Trim() != "true")
                    {
                        str_error_log = result2;
                        return false;
                    }

                    str_error_log = result2;
                    httpClient.Dispose();
                    multipartFormDataContent.Dispose();
                    return true;
                }

                httpClient.Dispose();
                multipartFormDataContent.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                str_error_log = ex.Message.Replace("\r\n", "");
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

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pcbaRn", str_save_value[0]);
                dictionary.Add("factoryId", str_save_value[1]);
                dictionary.Add("productId", str_save_value[2]);
                dictionary.Add("modelId", str_save_value[3]);
                dictionary.Add("colorId", str_save_value[4]);
                dictionary.Add("productType", str_save_value[5]);
                dictionary.Add("tagNumber", str_save_value[6]);
                dictionary.Add("fwVersion", str_save_value[7]);
                dictionary.Add("sensitivityTest", str_save_value[8]);
                dictionary.Add("rfCloudUp", str_save_value[9]);
                dictionary.Add("rfOprNumber", str_save_value[10]);
                dictionary.Add("rfVersion", str_save_value[11]);
                dictionary.Add("result", str_save_value[12]);
                dictionary.Add("failMessage", str_save_value[13]);
                string postdata = ParseToString(dictionary);
                string responseString = null;
                if (!DealPost(posturl_S170_RF, postdata, ref responseString, str_token))
                {
                    str_error_log = responseString;
                    return false;
                }

                string[] array = responseString.Replace("{", "").Replace("}", "").Replace("\"", "")
                    .Split(',');
                string text = array[0].Replace("result:", "").ToLower();
                string text2 = array[1].Replace("error:", "");
                if (text.Trim() != "true")
                {
                    str_error_log = "--Cloud up error_code:" + responseString;
                    return false;
                }

                str_error_log = responseString;
                return true;
            }
            catch (Exception ex)
            {
                str_error_log = "--Cloud up error:" + ex.Message.Replace("\r\n", "");
                return false;
            }
        }

        private bool DealPost_login(string posturl, string postdata, ref string responseString)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(posturl);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 5800;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                int num = bytes.Length;
                httpWebRequest.ContentLength = num;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, num);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string text = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = text.ToString();
                return true;
            }
            catch (Exception ex)
            {
                responseString = "DealPost--" + ex.Message + "Error";
                return false;
            }
        }

        private bool DealPost_creat_token(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(posturl);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 5800;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(str_header_key, str_header_value);
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                int num = bytes.Length;
                httpWebRequest.ContentLength = num;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, num);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string text = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = text.ToString();
                return true;
            }
            catch (Exception ex)
            {
                responseString = "DealPost--" + ex.Message + "Error";
                return false;
            }
        }

        private bool DealPost_creat_sn_uid(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(posturl);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 5800;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(str_header_key, str_header_value);
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                int num = bytes.Length;
                httpWebRequest.ContentLength = num;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, num);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string text = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = text.ToString();
                return true;
            }
            catch (Exception ex)
            {
                responseString = "DealPost--" + ex.Message + "Error";
                return false;
            }
        }

        private bool DealPost(string posturl, string postdata, ref string responseString, string str_header_value)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(posturl);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 5800;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Headers.Add(str_header_key, str_header_value);
                byte[] bytes = Encoding.UTF8.GetBytes(postdata);
                int num = bytes.Length;
                httpWebRequest.ContentLength = num;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, num);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string text = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                responseString = text.ToString();
                return true;
            }
            catch (Exception ex)
            {
                responseString = "DealPost--" + ex.Message + "Error";
                return false;
            }
        }

        private string ParseToString(IDictionary<string, string> parameters)
        {
            IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            StringBuilder stringBuilder = new StringBuilder("");
            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key;
                string value = enumerator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    stringBuilder.Append(key).Append("=").Append(value)
                        .Append("&");
                }
            }

            return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
        }
    }
}
