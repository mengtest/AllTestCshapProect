using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _Socket_Core;
using Z_CShapCore;

namespace _Socket_Client {
    public class NetClient {
        public static int ServerPort = 8885;
        public static string ServerIP = "127.0.0.1";

        public static Thread receiveDataThread;
        public static Thread dealDataThread;
        public static Socket socket;
        public static Queue<ReceiveMessageData> receiveDataQueue = new Queue<ReceiveMessageData>();

        public static void Start () {
            socket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
            try {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ServerIP) , ServerPort)); //配置服务器IP与端口
                Console.WriteLine("连接中...");
                receiveDataThread = new Thread(ReceiveMessage);
                receiveDataThread.Start(socket);

                dealDataThread = new Thread(DealMessage);
                dealDataThread.Start();
            } catch {
                throw new Exception();
            }
        }

        private static void DealMessage (object o) {
            while (true) {
                lock (receiveDataQueue) {
                    if (receiveDataQueue.Count > 0) {
                        ReceiveMessageData data = receiveDataQueue.Dequeue();
                        //MsgHandle.Self.DealMsg(data);
                    }
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private static void ReceiveMessage (object clientSocket) {
            try {
                Socket myClientSocket = (Socket)clientSocket;
                while (true) {
                    byte[] result = new byte[1024];
                    int receiveLength = myClientSocket.Receive(result);
                    if (receiveLength > 0) {
                        ReceiveMessageData data = new ReceiveMessageData();
                        data.ReceivePoint = myClientSocket.RemoteEndPoint.AsGIPEndPoint();
                        data.ReceiveBytes = result;
                        data.ReceiveLength = receiveLength;
                        receiveDataQueue.Enqueue(data);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SendMsg (int cmd , int scmd , BodyMsg body) {
            MessageData data = new MessageData();
            HeadMsg head = new HeadMsg();
            data.Head = head;
            data.Body = body;
            ByteBuffer _buff = new ByteBuffer();
            byte[] bytes = StaticTools.Serialize(data);
            _buff.WriteInt32(bytes.Length);
            _buff.WriteBytes(bytes);
            bytes = _buff.ToBytes();
            socket.Send(bytes);
        }

        public static void Close () {
            if (receiveDataThread != null) {
                receiveDataThread.Interrupt();
                receiveDataThread.Abort();
            }
            if (socket != null) {
                socket.Close();
            }
        }
    }
}
