using System;
using System.Configuration;

namespace ConfigLib
{
    public class ConfigFile
    {
        public static void IniWriteValue(string Key, string Value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // 检查键是否存在，如果存在则更新值
            if (config.AppSettings.Settings[Key] != null)
            {
                config.AppSettings.Settings[Key].Value = Value;
            }
            else
            {
                // 如果键不存在，则添加新键
                config.AppSettings.Settings.Add(Key, Value);
            }

            // 保存更改
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        //读INI文件
        public static string IniReadValue(string Key)
        {
            return ConfigurationManager.AppSettings[Key] ?? string.Empty;
        }
    }
}
