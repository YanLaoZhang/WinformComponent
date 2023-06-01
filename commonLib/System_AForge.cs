using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AForge.Video.DirectShow;
namespace System_AForge
{
    public class AForge_ver
    {
        public string ver()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public class AForge_CAM
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        List<string> list_device_name = new List<string>();
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
        bool bool_device = false;
        int index_of = 0;
        public bool AForge_Cam_detect(string str_device_name_0,ref string str_error,ref int int_stat) //"iCatch V37"
        {
            try
            {
                string str_device_name = "iCatch V37";
                list_device_name.Clear();
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //MessageBox.Show(videoDevices.ToString());
                if (videoDevices.Count == 0)
                {
                    return false;
                }
                foreach (FilterInfo device in videoDevices)
                {
                    Application.DoEvents();
                    list_device_name.Add(device.Name);
                    //MessageBox.Show(device.Name);
                }
                if (list_device_name.Count > 0)
                {
                    for (int i = 0; i < list_device_name.Count; i++)
                    {
                        Application.DoEvents();
                        if (list_device_name.Contains(str_device_name))
                        {
                            bool_device = true;
                            index_of = i;
                            int_stat = 0;
                            return true;
                        }
                    }
                }
                int_stat = 2;
                return true;
            }
            catch (Exception ee)
            {
                int_stat = 1;
                str_error = ee.Message.Replace("\r","").Replace("\n","");
                return false;
            }
        }


        public bool AForge_Cam_open(int pixel_sel,ref string str_error)
        {
            try
            {
                /*
                list_device_name.Clear();
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    return false;
                }
                foreach (FilterInfo device in videoDevices)
                {
                    list_device_name.Add(device.Name);
                }
                bool_device = false;
                int index_of = 0;
                for (int i = 0; i < list_device_name.Count; i++)
                {
                    Application.DoEvents();
                    if (list_device_name[i].Contains("iCatch V37"))
                    {
                        bool_device = true;
                        index_of = i;
                        break;
                    }
                }
                 */
                if (bool_device)
                {
                    videoSource = new VideoCaptureDevice(videoDevices[index_of].MonikerString);// ("UVC Camera");//
                    //设置下像素，这句话不写也可以正常运行：
                    videoSource.VideoResolution = videoSource.VideoCapabilities[pixel_sel];
                    //----------------------------------------------
                    

                    //----------------------------------------------
                    //在videoSourcePlayer1里显示
                    videoSourcePlayer1.VideoSource = videoSource;
                    videoSourcePlayer1.Start();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message;
                return false;
            }
        }


        public bool AForge_Cam_picture(ref System.Drawing.Bitmap bitmap)
        {
            try
            {
                bitmap = videoSourcePlayer1.GetCurrentVideoFrame();
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool AForge_Cam_close(ref string str_error)
        {
            try
            {
                //videoSourcePlayer1.Stop();
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                GC.Collect();
                return true;
            }
            catch (Exception ee)
            {
                str_error = "ee:" + ee.Message;
                return false;
            }
        }

        void Delay(int t)
        {
            int numa = Environment.TickCount;
            while (true)
            {
                Application.DoEvents();
                if (Environment.TickCount - numa > t)
                {
                    break;
                }
            }
        }

        public enum VideoProcAmpProperty
        {
            /// <summary>
            /// Brightness control.
            /// </summary>
            Brightness = 0,  //亮度

            /// <summary>
            /// Contrast control.
            /// </summary>
            Contrast,        //对比

            /// <summary>
            /// Hue control.
            /// </summary>
            Hue,             //色调

            /// <summary>
            /// Saturation control.
            /// </summary>
            Saturation,     //饱和

            /// <summary>
            /// Sharpness control.
            /// </summary>
            Sharpness,      //锐度

            /// <summary>
            /// Gamma control.
            /// </summary>
            Gamma,         //Gamma

            /// <summary>
            /// ColorEnable control.
            /// </summary>
            ColorEnable,   //色度

            /// <summary>
            /// WhiteBalance control.
            /// </summary>
            WhiteBalance,  //白平衡

            /// <summary>
            /// BacklightCompensation control.
            /// </summary>
            BacklightCompensation,  //背光补偿

            /// <summary>
            /// Gain control.
            /// </summary>
            Gain        //增益    
        }


        string[] str_array_VideoProcAmpProperty = new string[10] { "Brightness", "Contrast", "Hue", "Saturation", "Sharpness",
                                                                   "WhiteBalance", "BacklightCompensation", "Gamma", "ColorEnable",
                                                                   "Gain" };//10

        public bool Aforge_video_param_set_Brightness(byte byteVideoProcAmpFlags, int int_Brightness, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
                if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[0], byteVideoProcAmpFlags, int_Brightness, ref str_error_log) == false)
                {
                    return false;
                }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_Contrast(byte byteVideoProcAmpFlags, int int_Contrast, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
                if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_Contrast, ref str_error_log) == false)
                {
                    return false;
                }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_Hue(byte byteVideoProcAmpFlags, int int_Hue, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
               if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_Hue, ref str_error_log) == false)
               {
                   return false;
               }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_Saturation(byte byteVideoProcAmpFlags, int int_Saturation, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
               if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_Saturation, ref str_error_log) == false)
               {
                   return false;
               }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_Sharpness(byte byteVideoProcAmpFlags, int int_Sharpness, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
                if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_Sharpness, ref str_error_log) == false)
                {
                    return false;
                }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_WhiteBalance(byte byteVideoProcAmpFlags, int int_WhiteBalance, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
                if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_WhiteBalance, ref str_error_log) == false)
                {
                    return false;
                }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        public bool Aforge_video_param_set_BacklightCompensation(byte byteVideoProcAmpFlags, int int_BacklightCompensation, ref string str_error_log)
        {
            try
            { //0->None 1->Auto 2->Manual
                /*
                if (_Cam.AForge_Set_Video_Param(str_array_VideoProcAmpProperty[1], byteVideoProcAmpFlags, int_BacklightCompensation, ref str_error_log) == false)
                {
                    return false;
                }
                */
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }


        //public bool AForge_Set_Video_Param(string str_Auto_Param,
        //                                   byte int_Auto_Param,
        //                                   int int_Auto_Value,
        //                                   ref string str_error_log)
        //{
        //    try
        //    {

        //        switch (str_Auto_Param)
        //        {
        //            case "Brightness":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Brightness, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Brightness None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Brightness, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Brightness Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Brightness, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Brightness Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Contrast":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Contrast, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Contrast None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Contrast, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Contrast Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Contrast, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Contrast Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Hue":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Hue, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Hue None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Hue, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Hue Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Hue, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Hue Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Saturation":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Saturation, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Saturation None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Saturation, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Saturation Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Saturation, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Saturation Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Sharpness":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Sharpness, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Sharpness None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Sharpness, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Sharpness Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Sharpness, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Sharpness Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Gamma":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gamma, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gamma None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gamma, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gamma Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gamma, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gamma Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "ColorEnable":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.ColorEnable, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "ColorEnable None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.ColorEnable, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "ColorEnable Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.ColorEnable, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "ColorEnable Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "WhiteBalance":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.WhiteBalance, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "WhiteBalance None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.WhiteBalance, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "WhiteBalance Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.WhiteBalance, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "WhiteBalance Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "BacklightCompensation":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.BacklightCompensation, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "BacklightCompensation None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.BacklightCompensation, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "BacklightCompensation Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.BacklightCompensation, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "BacklightCompensation Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //            case "Gain":
        //                if (int_Auto_Param == 0)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gain, int_Auto_Value, VideoProcAmpFlags.None) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gain None";
        //                        return false;
        //                    }
        //                }
        //                else if (int_Auto_Param == 1)
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gain, int_Auto_Value, VideoProcAmpFlags.Auto) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gain Auto";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (videoSource.SetVideoProperty(VideoProcAmpProperty.Gain, int_Auto_Value, VideoProcAmpFlags.Manual) == false) //set VideoProcAmpProperty.Brightness,brightnessValue,VideoProcAmpFlags.Manual);
        //                    {
        //                        str_error_log = "Gain Manual";
        //                        return false;
        //                    }
        //                }
        //                break;
        //        }
        //        return true;
        //    }
        //    catch (Exception ee)
        //    {
        //        str_error_log = ee.Message;
        //        return false;
        //    }
        //}

        //public bool AForge_Get_Video_Param_Range(string str_Auto_Param,
        //                                   out int min_value,
        //                                   out int max_value,
        //                                   out int stepsize_value,
        //                                   out int default_value, ref string str_error_log)
        //{
        //    try
        //    {
        //        VideoProcAmpFlags _VideoProcAmpFlags = new VideoProcAmpFlags();
        //        min_value = -1;
        //        max_value = -1;
        //        stepsize_value = -1;
        //        default_value = -1;
        //        switch (str_Auto_Param)
        //        {
        //            case "Brightness":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Brightness, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Brightness Range";
        //                    return false;
        //                }
        //                break;
        //            case "Contrast":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Contrast, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Contrast Range";
        //                    return false;
        //                }
        //                break;
        //            case "Hue":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Hue, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Hue Range";
        //                    return false;
        //                }
        //                break;
        //            case "Saturation":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Saturation, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Saturation Range";
        //                    return false;
        //                }
        //                break;
        //            case "Sharpness":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Sharpness, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Sharpness Range";
        //                    return false;
        //                }
        //                break;
        //            case "Gamma":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Gamma, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Gamma Range";
        //                    return false;
        //                }
        //                break;
        //            case "ColorEnable":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.ColorEnable, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "ColorEnable Range";
        //                    return false;
        //                }
        //                break;
        //            case "WhiteBalance":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.WhiteBalance, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "WhiteBalance Range";
        //                    return false;
        //                }
        //                break;
        //            case "BacklightCompensation":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.BacklightCompensation, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "BacklightCompensation Range";
        //                    return false;
        //                }
        //                break;
        //            case "Gain":
        //                if (videoSource.GetVideoPropertyRange(VideoProcAmpProperty.Gain, out min_value, out max_value, out stepsize_value, out default_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Gain Range";
        //                    return false;
        //                }
        //                break;
        //        }

        //        return true;
        //    }
        //    catch (Exception ee)
        //    {
        //        min_value = -1;
        //        max_value = -1;
        //        stepsize_value = -1;
        //        default_value = -1;
        //        str_error_log = ee.Message;
        //        return false;
        //    }
        //}

        //public bool AForge_Get_Video_Param(string str_Auto_Param,
        //                                   out int set_value,
        //                                   ref string str_error_log)
        //{
        //    try
        //    {
        //        VideoProcAmpFlags _VideoProcAmpFlags = new VideoProcAmpFlags();
        //        set_value = -1;
        //        switch (str_Auto_Param)
        //        {
        //            case "Brightness":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Brightness, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Brightness";
        //                    return false;
        //                }
        //                break;
        //            case "Contrast":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Contrast, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Contrast";
        //                    return false;
        //                }
        //                break;
        //            case "Hue":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Hue, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Hue";
        //                    return false;
        //                }
        //                break;
        //            case "Saturation":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Saturation, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Saturation";
        //                    return false;
        //                }
        //                break;
        //            case "Sharpness":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Sharpness, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Sharpness";
        //                    return false;
        //                }
        //                break;
        //            case "Gamma":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Gamma, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Gamma";
        //                    return false;
        //                }
        //                break;
        //            case "ColorEnable":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.ColorEnable, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "ColorEnable";
        //                    return false;
        //                }
        //                break;
        //            case "WhiteBalance":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.WhiteBalance, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "WhiteBalance";
        //                    return false;
        //                }
        //                break;
        //            case "BacklightCompensation":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.BacklightCompensation, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "BacklightCompensation";
        //                    return false;
        //                }
        //                break;
        //            case "Gain":
        //                if (videoSource.GetVideoProperty(VideoProcAmpProperty.Gain, out set_value, out _VideoProcAmpFlags) == false)
        //                {
        //                    str_error_log = "Gain";
        //                    return false;
        //                }
        //                break;
        //        }

        //        return true;
        //    }
        //    catch (Exception ee)
        //    {
        //        set_value = -1;
        //        str_error_log = ee.Message;
        //        return false;
        //    }
        //}
    }
}
