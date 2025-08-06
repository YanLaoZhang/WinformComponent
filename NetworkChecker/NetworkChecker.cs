using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkChecker
{
    public class NetworkChecker
    {
        public enum ConnectionStatus
        {
            Success,  // 通讯正常
            PortClosed,  // 端口关闭
            HostUnreachable  // 主机不可达
        }

        public static async Task<ConnectionStatus> CheckIpAndPortAsync(string ipAddress, int port, int timeoutMS = 2000)
        {
            // 第一步：检查主机是否可达（ping）
            if(!await PingHostAsync(ipAddress, timeoutMS))
            {
                return ConnectionStatus.HostUnreachable;
            }

            // 第二步：检查端口是否开放（TCP连接）
            return await CheckPortAsync(ipAddress, port, timeoutMS) ? ConnectionStatus.Success: ConnectionStatus.PortClosed;

        }

        /// <summary>
        /// 使用Ping命令检测主机可达性
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="timeoutMS"></param>
        /// <returns></returns>
        public static async Task<bool> PingHostAsync(string ipAddress, int timeoutMS)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(ipAddress, timeoutMS);
                    return reply.Status == IPStatus.Success;
                }                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> CheckPortAsync(string ipAddress, int port, int timeoutMS)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(ipAddress, port);
                    var timeoutTask = Task.Delay(timeoutMS);

                    // 等待连接完成或超时
                    if(await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                    {
                        client.Dispose();
                        return false;
                    }
                    return client.Connected;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
