using System;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace DSA700Lib
{

    public class DSA700Controller : IDisposable
    {
        public static string DSA_SET_FREQ = ":SENSe:FREQuency:CENTer 433950000";  // 设置频谱仪中心频率
        public static string DSA_OPEN_SIG_CAPTURE = ":SENSe:SIGCapture:STATe ON";  // 打开频谱仪信号捕获
        public static string DSA_QUERY_SIG_CAPTURE = ":SENSe:SIGCapture:STATe?";  // 查询频谱仪信号捕获模式
        public static string DSA_OPEN_PEAK_SEARCH = ":CALCulate:MARKer1:CPEak:STATe ON";  // 打开频谱仪峰值搜索功能
        public static string DSA_OPEN_2FSK = ":SENSe:SIGCapture:2FSK:STATe ON";  // 打开频谱仪2FSK功能
        public static string DSA_SET_2FSK_MAX_HOLD = ":SENSe:SIGCapture:2FSK:MAXHold:STATe ON";  // 打开频谱仪2FSK最大保持功能
        public static string DSA_TRACe1_MODE_MAX_HOLD = ":TRACe1:MODE MAXHold";  // 设置迹线为最大保持
	    public static string DSA_TRACe1_MODE_WRITe = ":TRACe1:MODE WRITe";  // 设置指定迹线为清除写入
	    public static string DSA_OPEN_TRACKING = ":CALCulate:MARKer:TRACking:STATe ON";  // 打开信号追踪
	    public static string DSA_CLOSE_TRACKING = ":CALCulate:MARKer:TRACking:STATe OFF";  // 关闭信号追踪
	    public static string DSA_SET_MARKER1_PEAK_SERACH_MODE = ":CALCulate:MARKer1:PEAK:SEARch:MODE MAXimum";  // 设置峰值搜索模式为最大值，查找迹线上的最大值，并用光标标记。
        public static string DSA_SET_MARKER1_MAX = ":CALCulate:MARKer1:MAXimum:MAX";  // 根据:CALCulate:MARKer<n>:PEAK:SEARch:MODE 命令设置的搜索模式执行一次峰值搜索，并用指定光标1标记
        public static string DSA_QUERY_MARKE_FREQ = ":CALCulate:MARKer1:X?";  // 读取频率
	    public static string DSA_QUERY_MARKE_RANGE =":CALCulate:MARKer1:Y?";  // 读取功率
	    public static string DSA_SET_MARKE_RESET = ":SENSe:SIGCapture:2FSK:RESet";  // 2FSK复位

        private MessageBasedSession _visaSession;

        public DSA700Controller(string resourceName)
        {
            try
            {
                _visaSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(resourceName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open VISA session.", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">USB|TCPIP|GPIB</param>
        /// <returns></returns>
        public async static Task<string[]> FindAvailableDevicesAsync(string type = "USB")
        {
            return await Task.Run(() =>
            {
                try
                {
                    var resourceManager = ResourceManager.GetLocalManager();
                    return resourceManager.FindResources($"{type}?*INSTR");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error finding resources: {ex.Message}");
                    return Array.Empty<string>();
                }
            });
        }

        public static string[] GetAvailableDevices()
        {
            try
            {
                // 获取VISA资源管理器
                var resourceManager = ResourceManager.GetLocalManager();

                // 查找所有连接的设备
                string[] resources = resourceManager.FindResources("USB?*INSTR");

                return resources; // 返回所有符合条件的资源
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding resources: {ex.Message}");
                return Array.Empty<string>(); // 返回空数组表示没有找到设备
            }
        }

        public void SendCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            _visaSession.Write(command + "\n");
        }

        public string Query(string query)
        {
            SendCommand(query);
            return _visaSession.ReadString();
        }

        public void Dispose()
        {
            _visaSession?.Dispose();
        }
    }

    /*class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 获取所有可用设备
                var availableDevices = DSA700Controller.GetAvailableDevices();

                if (availableDevices.Length == 0)
                {
                    Console.WriteLine("No devices found.");
                    return;
                }

                // 打印所有可用设备
                Console.WriteLine("Available devices:");
                foreach (var device in availableDevices)
                {
                    Console.WriteLine(device);
                }

                // 选择第一个可用设备进行连接
                string resourceName = availableDevices[0];

                using (var dsa700 = new DSA700Controller(resourceName))
                {
                    // 设置中心频率为1 GHz
                    dsa700.SendCommand(":FREQ:CENT 1E9");
                    Console.WriteLine("Center Frequency set to 1 GHz.");

                    // 查询并显示当前中心频率
                    string frequency = dsa700.Query(":FREQ:CENT?");
                    Console.WriteLine($"Current Center Frequency: {frequency} Hz");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }*/

}
