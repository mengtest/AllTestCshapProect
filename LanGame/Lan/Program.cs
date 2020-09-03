using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lan {
    class Program {
        static void Main(string[] args) {
            var s = new Base.MessageData();
            s.desc = "12313";
            Console.WriteLine(s.ToString());


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

            // 将消息序列化为二进制的方法
            // < param name="model">要序列化的对象< /param>
            public static byte[] Serialize<T> (T model) {
                try {
                    //涉及格式转换，需要用到流，将二进制序列化到流中
                    using (MemoryStream ms = new MemoryStream()) {
                        //使用ProtoBuf工具的序列化方法
                        ProtoBuf.Serializer.Serialize<T>(ms , model);
                        //定义二级制数组，保存序列化后的结果
                        byte[] result = new byte[ms.Length];
                        //将流的位置设为0，起始点
                        ms.Position = 0;
                        //将流中的内容读取到二进制数组中
                        ms.Read(result , 0 , result.Length);
                        return result;
                    }
                } catch (Exception ex) {
                    Console.WriteLine("序列化失败: " + ex.ToString());
                    return null;
                }
            }

            // 将收到的消息反序列化成对象
            // < returns>The serialize.< /returns>
            // < param name="msg">收到的消息.</param>
            public static T DeSerialize<T> (int len,byte[] msg) {
                try {
                    using (MemoryStream ms = new MemoryStream()) {
                        //将消息写入流中
                        ms.Write(msg , 0 , len);
                        //将流的位置归0
                        ms.Position = 0;
                        //Console.WriteLine(ms.ToArray().Length);
                        //使用工具反序列化对象
                        T result = ProtoBuf.Serializer.Deserialize<T>(ms);
                        return result;
                    }
                } catch (Exception ex) {
                    Console.WriteLine("反序列化失败: " + ex.ToString());
                    return default(T);
                }
            }

            private static void ReceiveMessage() {
                byte[] byt = new byte[1024];
                while (true) {
                    EndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
                    int length = Server.ReceiveFrom(byt, ref receivePoint);
                    var data = DeSerialize<Base.MessageData>(length , byt);
                    Console.WriteLine(LitJson.JsonMapper.ToJson(data));

                    byte[] byteArray = System.Text.Encoding.Default.GetBytes(data.data);

                    Console.WriteLine(BitConverter.ToString(byteArray));

                    var _data = DeSerialize<LoginUser.CS_LoginUser>(byteArray.Length , byteArray);
                    Console.WriteLine(LitJson.JsonMapper.ToJson(_data));


                    //string str = Encoding.UTF8.GetString(byt, 0, length);
                    //var data = new {
                    //    ip = receivePoint.ToString(),
                    //    str = str
                    //};
                    //Console.WriteLine("c-s:"+ LitJson.JsonMapper.ToJson(data));

                    //Console.WriteLine("Server ReceiveMessage ip to: == " + receivePoint.ToString());
                    //if (receivePoint.ToString() == LocalIp) {
                    //    Console.WriteLine(111);
                    //} else if (str == "RequestServer") {
                    //    SendMsg(LocalIp, receivePoint);
                    //} else if (Dealmsg != null) {
                    //    Dealmsg(str);
                    //}
                    //SendMsg("Server send to client ~", receivePoint);
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
