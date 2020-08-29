using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Z_CShapCore;

namespace ddzServer
{
    public class Server
    {
        //用户连接上Socket
        public static event Action<Socket, GIPEndPoint> OnClientConnent;
        public static event Action<List<GIPEndPoint>> OnClientQuit;
        public static event Action<ReceiveMsgData> OnReceiveMessage;
        public static List<Thread> threadList = new List<Thread>();
        //==============================================================================

        public static Dictionary<GIPEndPoint, Client> allClient = new Dictionary<GIPEndPoint, Client>();
        public static Client GetClient(GIPEndPoint _ip)
        {
            
            try
            {
                return allClient[_ip];
            }
            catch (System.Exception ex)
            {
                throw new Exception($"找不到客户端！ip:{_ip} {ex.Message}");
            }
        }
        
        public void OnSocketConnect(Socket socket, GIPEndPoint point)
        {
            Console.WriteLine($"{point.ToString()} connection!");

            if (!GlobalData.allUser.ContainsKey(point))
            {
                GlobalData.allUser.Add(point, new User()
                {
                    info = new UserInfo()
                    {
                        name = "66666666999999"
                    }
                });
            }

            if (!allClient.ContainsKey(point))
            {
                allClient.Add(point, new Client()
                {
                    socket = socket
                });
                ByteBuffer b = new ByteBuffer();
                b.WriteString($"{point.ToString()} 加入了 ！");
                MessageManage.Self.SendMsg(1, 1, b);
            }
            else
            {
                
            }
        }

        private static Server self;
        public static Server Self
        {
            get
            {
                if (self == null)
                {
                    self = new Server();
                }
                return self;
            }
        }
        private static int myProt = 8885;   //端口
        private static Socket serverSocket;
        public void Start()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("0.0.0.0"), myProt);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(point);  //绑定IP地址：端口
            serverSocket.Listen(10);                        //设定最多10个排队连接请求

            Console.WriteLine("start server {0} ok~!", serverSocket.LocalEndPoint.ToString());

            Thread myThread = new Thread(ListenSocketConnect);  //通过Clientsoket发送数据
            myThread.Start();
            threadList.Add(myThread);

            Thread beatHeart = new Thread(BeatHeart); // 心跳
            beatHeart.Start();
        }

        public void Stop()
        {
            serverSocket.Dispose();
            serverSocket.Close();
            foreach (var thread in threadList)
            {
                thread.Abort();
            }
            threadList.Clear();
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenSocketConnect()
        {
            while (true)
            {
                Socket socket = serverSocket.Accept();
                GIPEndPoint point = socket.RemoteEndPoint;

                if (OnClientConnent != null)
                {
                    OnSocketConnect(socket, point);
                    OnClientConnent(socket, point);
                }
                
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(socket);
                threadList.Add(receiveThread);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="clientSocket"></param>
        private void ReceiveMessage(object clientSocket)
        {
            Socket socket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    //通过clientSocket接收数据
                    int receiveLength = socket.Receive(result);

                    ReceiveMsgData data = new ReceiveMsgData();
                    data.receivePoint = socket.RemoteEndPoint;
                    data.receiveBytes = result;
                    data.receiveLength = receiveLength;

                    if (OnReceiveMessage != null)
                    {
                        OnReceiveMessage(data);
                    }

                    Console.WriteLine("{0} datalength = {1}", socket.RemoteEndPoint.ToString(), receiveLength);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="o"></param>
        private void BeatHeart(object o)
        {
            List<GIPEndPoint> waitRemoveList = new List<GIPEndPoint>();
            while (true)
            {
                Thread.Sleep(100);
                lock (allClient)
                {
                    lock (waitRemoveList)
                    {
                        waitRemoveList.Clear();
                        foreach (var item in allClient)
                        {
                            if (!item.Value.socket.Connected)
                            {
                                waitRemoveList.Add(item.Key);
                            }
                        }
                        if (waitRemoveList.Count > 0 && OnClientQuit != null)
                        {
                            OnClientQuit(waitRemoveList);
                        }
                    }
                }
            }
        }
    }
}