using System;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using _Socket_Core;

namespace _Socket_Server {
    public class NetServer {
        private int ServerProt { get; set; }
        public event Action<Socket, EndPoint> OnClientConnent;
        private static Queue<ReceiveMessageData> receiveDataQueue = new Queue<ReceiveMessageData>();
        private static Socket serverSocket;
        public void Start (int port = 8885) {
            ServerProt = port;
            try {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse("0.0.0.0") , ServerProt);
                serverSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
                serverSocket.Bind(point);
                serverSocket.Listen(10);                        //设定最多10个排队连接请求
                Console.WriteLine("start server {0} ok~!" , serverSocket.LocalEndPoint.ToString());
                GlobalData.AddThread(ListenSocketConnect);
                GlobalData.AddThread(UpdateBeatHeart);
                GlobalData.AddThread(DealMessage);
            } catch (Exception ex ) {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop () {
            serverSocket.Dispose();
            serverSocket.Close();
        }

        private void ListenSocketConnect () {
            while (true) {
                Socket socket = serverSocket.Accept();
                EndPoint point = socket.RemoteEndPoint; //来源
                if (OnClientConnent != null) {
                    OnClientConnent(socket , point);
                }
                GlobalData.AddThread(ReceiveMessage, socket);
            }
        }

        private void ReceiveMessage (object clientSocket) {
            Socket socket = (Socket)clientSocket;
            while (true) {
                try {
                    byte[] result = new byte[1024];
                    int receiveNumber = socket.Receive(result);
                    ReceiveMessageData data = new ReceiveMessageData();
                    data.ReceivePoint = socket.RemoteEndPoint.AsGIPEndPoint();
                    data.ReceiveBytes = result;
                    receiveDataQueue.Enqueue(data);
                    //Console.WriteLine("{0}:{1}" , socket.RemoteEndPoint.ToString() , result.Length);
                } catch (Exception ex) {
                    Console.WriteLine(" Error msg==>NetServer.ReceiveMessage :" + ex.Message);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    break;
                }
            }
        }

        private static void DealMessage (object o) {
            while (true) {
                lock (receiveDataQueue) {
                    if (receiveDataQueue.Count > 0) {
                        ReceiveMessageData data = receiveDataQueue.Dequeue();
                        MessageManage.Self.DealMsg(data);
                    }
                }
            }
        }
        private void UpdateBeatHeart (object o) {
            List<EndPoint> list = new List<EndPoint>();
            while (true) {
                Thread.Sleep(1000);
                list.Clear();
                lock (GlobalData.AllPlayer) {
                    foreach (var item in GlobalData.AllPlayer) {
                        if (!item.Value.PSocket.Connected) {
                            list.Add(item.Key);
                        }
                    }
                    foreach (var item in list) {
                        GlobalData.AllPlayer.Remove(item);
                        Console.WriteLine($"{item.ToString()} disconnect!");
                    }
                    foreach (var item2 in list) {
                        //MessageData_1_2 body = new MessageData_1_2();
                        //body.Describe = $"{item2.ToString()} 离开了 ！";
                        //MsgHandle.Self.SendMsg(1 , 2 , body);
                    }
                }
            }
        }
    }
}