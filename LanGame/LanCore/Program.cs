using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanCore {
    public class UdpCore {
        private Thread receiveThread = null;
        private Socket server = null;
        private Socket client = null;
        public int port = 23333;
        public Action<string> dealmsg = null;
        public UdpCore () {
            client = new Socket(AddressFamily.InterNetwork , SocketType.Dgram , ProtocolType.Udp);
            server = new Socket(AddressFamily.InterNetwork , SocketType.Dgram , ProtocolType.Udp);
        }
        public void StartServer () {
            IPEndPoint point = new IPEndPoint(IPAddress.Any , 0);
            server.Bind(point);
            IPEndPoint p = (IPEndPoint)server.LocalEndPoint;
            port = p.Port;
            Console.WriteLine(p.Port);
            receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }
        public void StopServer () {
            receiveThread.Abort();
            receiveThread = null;
            server.Dispose();
            server.Close();
            server = null;
        }
        public void ReceiveMessage () {
            byte[] byt = new byte[1024];
            while (true) {
                EndPoint receivePoint = new IPEndPoint(IPAddress.Any , port);
                int length = server.ReceiveFrom(byt , ref receivePoint);
                string str = Encoding.UTF8.GetString(byt , 0 , length);
                string localIp = GetLocalIp();
                Console.WriteLine(str);
                Console.WriteLine("localIp == " + localIp);
                Console.WriteLine("receivePoint.ToString() == " + receivePoint.ToString());
                if (receivePoint.ToString() == localIp) {
                    Console.WriteLine(111);
                } else if (str == "RequestServer") {
                    SendMsg(localIp , receivePoint);
                } else if (dealmsg != null) {
                    dealmsg(str);
                }
            }
        }

        public void CheckServerRoom () {
            SendMsg("RequestServer" , new IPEndPoint(IPAddress.Broadcast , port));
        }

        public void SendMsg (string str , EndPoint ip = null) {
            if (ip == null)
                ip = new IPEndPoint(IPAddress.Broadcast , port);
            byte[] data = Encoding.UTF8.GetBytes(str);
            client.SetSocketOption(SocketOptionLevel.Socket , SocketOptionName.Broadcast , 1);
            client.SendTo(data , ip);
        }

        public string GetLocalIp () {
            string addressIP = "";
            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            for (int i = 0 ; i < ipEntry.AddressList.Length ; i++) {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                    addressIP = ipEntry.AddressList[i].ToString();
                }
            }
            return addressIP + ":" + port;
        }

        /// <summary>
        /// 获取第一个可用的端口号
        /// </summary>
        public int GetFirstAvailablePort () {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IList portUsed = PortIsUsed();
            Random random = new Random();
            while (true) {
                random = new Random(stopwatch.Elapsed.GetHashCode());
                int _prot = random.Next(5000 , 65535);
                if (!portUsed.Contains(_prot)) {
                    stopwatch.Stop();
                    return _prot;
                }
            }
        }

        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns></returns>
        public IList PortIsUsed () {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP)
                allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP)
                allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray)
                allPorts.Add(conn.LocalEndPoint.Port);
            return allPorts;
        }
        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns></returns>
        public IList UDPPortIsUsed () {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsUDP)
                allPorts.Add(ep.Port);
            return allPorts;
        }
    }
}
