using System.Collections.Generic;
using System.Net;

namespace _Socket_Server
{
	public class Room {
        public int RoomId { get; set; }
        public List<EndPoint> playerList = new List<EndPoint>();
        public int PlayerCount {
            get {
                return playerList.Count;
            }
        }
    }
}