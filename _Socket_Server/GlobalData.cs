using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _Socket_Server {
    public class GlobalData {
        private static List<Thread> ThreadList { get; set; } = new List<Thread>();
        public static Dictionary<EndPoint, Player> AllPlayer = new Dictionary<EndPoint, Player>();
        public static Dictionary<string, Room> RoomList = new Dictionary<string, Room>();

        public static void AddThread (ThreadStart _start) {
            Thread thread = new Thread(_start);
            ThreadList.Add(thread);
            thread.Start();
        }

        public static void AddThread (ParameterizedThreadStart _start,object _socket = null) {
            Thread thread = new Thread(_start);
            ThreadList.Add(thread);
            thread.Start(_socket);
        }
        public static Player GetPlayer (EndPoint _point) {
            return AllPlayer.ContainsKey(_point) ? AllPlayer[_point] : null;
        }

        public static void AddPlayer (EndPoint _point, Socket _socket) {
            if (!AllPlayer.ContainsKey(_point)) {
                AllPlayer[_point] = new Player() {
                    PSocket = _socket
                };
            }
        }

        public static List<Player> GetRoomPlayer (string _room) {
            if (!RoomList.ContainsKey(_room))
                return null;
            List<Player> list = new List<Player>();
            foreach (var key in RoomList[_room].playerList) {
                Player player = GetPlayer(key);
                if (player != null && player.Connected) {
                    list.Add(player);
                }
            }
            return list;
        }

        public static List<Socket> GetAllPlayer () {
            List<Socket> list = new List<Socket>();
            foreach (var item in AllPlayer.Values) {
                list.Add(item.PSocket);
            }
            return list;
        }
    }
}