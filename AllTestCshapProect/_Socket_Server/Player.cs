
using System.Net.Sockets;
using System.Net;

namespace _Socket_Server {
    public class Player {
        public Socket PSocket;
        public EndPoint Ip;
        public PlayerInfo Info;
        public bool Connected {
            get {
                return PSocket == null ? false : PSocket.Connected;
            }
        }
    }

    public class PlayerInfo {
        public int UID { get; set; }
        public int Nick { get; set; }
        public int Sex { get; set; }
        public SceneState State { get; set; }
    }

    public enum SceneState {
        none,
        hall,
        room,
        game,
    }
}