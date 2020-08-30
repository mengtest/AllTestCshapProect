using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanCore;

namespace AppClient {
    class AppClient {
        static void Main (string[] args) {
            UdpCore udp = new UdpCore();
            udp.Dealmsg = (str) => {
                Console.WriteLine("msg:" + str);
            };
            udp.StartServer();
            udp.CheckServerRoom();
            while (true) {
                string str = Console.ReadLine();
                udp.SendMsg(str);
            }
        }
    }
}