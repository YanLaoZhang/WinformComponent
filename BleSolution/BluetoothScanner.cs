using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;

namespace BleSolution
{
    public class BluetoothScanner
    {
        private BluetoothLEAdvertisementWatcher watcher;

        public async Task<bool> ScanForDeviceAsync(string targetDeviceName, TimeSpan scanDuration)
        {
            bool deviceFound = false;

            // 创建 BluetoothLEAdvertisementWatcher 实例
            watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active // 设置扫描模式为主动
            };

            // 订阅广告数据事件
            watcher.Received += (sender, args) =>
            {
                var advertisement = args.Advertisement;

                // 这里可以根据广告的数据来匹配特定的设备，比如名称或其它信息
                if (advertisement.LocalName.Equals(targetDeviceName, StringComparison.OrdinalIgnoreCase))
                {
                    deviceFound = true;
                    watcher.Stop(); // 找到设备后停止扫描
                }
            };

            // 订阅扫描结束事件
            watcher.Stopped += (sender, args) =>
            {
                if (!deviceFound)
                {
                    Console.WriteLine("扫描未找到目标设备.");
                }
            };

            // 启动扫描
            watcher.Start();

            // 等待指定的扫描时间
            await Task.Delay(scanDuration);

            // 停止扫描
            watcher.Stop();

            return deviceFound;
        }
    }

}
