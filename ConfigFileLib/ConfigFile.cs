using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConfigFileLib
{
    public class ConfigFile
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //写INI文件
        public static void IniWriteValue(string Section, string Key, string Value, string Path_ini)
        {
            WritePrivateProfileString(Section, Key, Value, Path_ini);
        }

        //读INI文件
        public static string IniReadValue(string Section, string Key, string Path_ini)
        {
            StringBuilder temp = new StringBuilder(256);
            GetPrivateProfileString(Section, Key, "", temp, 256, Path_ini);
            return temp.ToString();
        }
    }
}
