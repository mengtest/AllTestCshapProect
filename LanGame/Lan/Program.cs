using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lan {
    class Program {
        static void Main(string[] args) {

            var h = new Phone();
            h.p

            var udpType = 0;
            var get = Console.ReadKey();
            Console.WriteLine();
            if (get.Key == ConsoleKey.Q) {
                UDPClient.Init();
                UDPClient.GetRoomList();
                udpType = 0;
            } else if (get.Key == ConsoleKey.W) {
                UDPServer.Init();
                udpType = 1;
            }
            while (true) {
                string str = Console.ReadLine();
                if (udpType == 0) {
                    UDPClient.SendMsg(str);
                } else {
                    UDPServer.SendMsg(str);
                }
            }
        }

        public class UDPClient {
            public static IPEndPoint ServerPoint { get; set; }
            public static Thread ReceiveThread { get; set; }
            public static Socket Client { get; set; }
            public static void Init() {
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
                Client.Bind(point);
                ReceiveThread = new Thread(ReceiveMessage);
                ReceiveThread.Start();
            }

            private static void ReceiveMessage() {
                byte[] byt = new byte[1024];
                while (true) {
                    EndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
                    int length = Client.ReceiveFrom(byt, ref receivePoint);
                    string str = Encoding.UTF8.GetString(byt, 0, length);
                    var data = new {
                        ip = receivePoint.ToString(),
                        str = str
                    };
                    Console.WriteLine("s-c:" + LitJson.JsonMapper.ToJson(data));
                    ServerPoint = (IPEndPoint)receivePoint;
                }
            }

            public static void UnInit() {
                ReceiveThread.Abort();
                ReceiveThread = null;
                Client.Dispose();
                Client.Close();
                Client = null;
            }
            public static void SendMsg(string str, EndPoint ip = null) {
                if (ip == null)
                    ip = ServerPoint;
                byte[] data = Encoding.UTF8.GetBytes(str);
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                Client.SendTo(data, ip);
                var _data = new {
                    ip = ip.ToString(),
                    str = str
                };
                Console.WriteLine("c-s:" + LitJson.JsonMapper.ToJson(_data));
            }
            public static void GetRoomList() {
                var a = new IPEndPoint(IPAddress.Broadcast, 2333);
                SendMsg("RequestServer", a);
            }
        }
        public class UDPServer {
            public static Thread ReceiveThread { get; set; }
            public static Socket Server { get; set; }
            public static void Init() {
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 2333);
                Server.Bind(point);
                ReceiveThread = new Thread(ReceiveMessage);
                ReceiveThread.Start();
                Console.WriteLine("start server to:" + GetLocalIp());
            }
            /// <summary>
            /// 获取本地ip
            /// </summary>
            /// <returns></returns>
            public static string GetLocalIp() {
                string addressIP = "";
                string hostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipEntry.AddressList.Length; i++) {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                        addressIP = ipEntry.AddressList[i].ToString();
                    }
                }
                return addressIP + ":" + 2333;
            }

            private static void ReceiveMessage() {
                byte[] byt = new byte[1024];
                while (true) {
                    EndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
                    int length = Server.ReceiveFrom(byt, ref receivePoint);
                    string str = Encoding.UTF8.GetString(byt, 0, length);
                    var data = new {
                        ip = receivePoint.ToString(),
                        str = str
                    };
                    Console.WriteLine("c-s:"+ LitJson.JsonMapper.ToJson(data));
                    
                    //Console.WriteLine("Server ReceiveMessage ip to: == " + receivePoint.ToString());
                    //if (receivePoint.ToString() == LocalIp) {
                    //    Console.WriteLine(111);
                    //} else if (str == "RequestServer") {
                    //    SendMsg(LocalIp, receivePoint);
                    //} else if (Dealmsg != null) {
                    //    Dealmsg(str);
                    //}
                    SendMsg("Server send to client ~", receivePoint);
                }
            }

            public static void UnInit() {
                ReceiveThread.Abort();
                ReceiveThread = null;
                Server.Dispose();
                Server.Close();
                Server = null;
            }
            public static void SendMsg(string str, EndPoint ip = null) {
                if (ip == null)
                    ip = new IPEndPoint(IPAddress.Broadcast, 0);
                byte[] data = Encoding.UTF8.GetBytes(str);
                Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                Server.SendTo(data, ip);
                var _data = new {
                    ip = ip.ToString(),
                    str = str
                };
                Console.WriteLine("s-c:" + LitJson.JsonMapper.ToJson(_data));
            }
            public static void GetRoomList() {
                var a = new IPEndPoint(IPAddress.Broadcast, 2333);
                SendMsg("RequestServer", a);
            }
        }
    }
}
