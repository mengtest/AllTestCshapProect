using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Socket_Core;
using ProtoBuf;

namespace _Socket_Server {
    class AppMain {
        public static void Main (string[] args) {
            NetServer server = new NetServer();
            server.OnClientConnent += MsgHandle.Self.OnSocketConnect;
            MessageManage.Self.DealMsgEvent += MsgHandle.Self.DealMsgSwitch;
            server.Start();
        }
    }
}