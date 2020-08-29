using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using Z_CShapCore;
using _Socket_Core;

namespace _Socket_Client {
    class AppMain {
        static void Main (string[] args) {
            NetClient.Start();
            while (true) {
                if (Console.KeyAvailable) {
                    ConsoleKeyInfo str = Console.ReadKey();
                    if (str.Key == ConsoleKey.W) {
                        Console.WriteLine("ok~!");
                    } else if (str.Key == ConsoleKey.Q) {
                        Console.WriteLine("quit~!");
                        NetClient.Close();
                        break;
                    } else if (str.Key == ConsoleKey.E) {
                        MessageData_1_10 body = new MessageData_1_10();
                        body.TalkStr = "12345";
                        NetClient.SendMsg(1 , 10 , body);
                    }
                }
            }

            return;

            //通过clientSocket接收数据
            //int receiveLength = clientSocket.Receive(result);
            //Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            Console.WriteLine("发送完毕，按回车键退出");
            Console.ReadLine();
        }
    }
}