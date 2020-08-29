using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;
using Z_CShapCore;

namespace ddzServer
{
    // 1_10 公告
    // 2_10 登陆
    // 2_11 在线人数
    // 2_12 大厅聊天
    // 2_20 个人信息
    // 2_21 设置
    // 2_30 私聊
    // 3_10 房间列表
    // 3_12 房间信息
    // 3_20 创建房间
    // 3_21 加入房间
    // 3_22 离开房间
    // 3_30 房间聊天
    // 4_10 准备游戏
    public class TalkMsg {
        public GIPEndPoint send;
        public string str;
    }
    public class GlobalData {
        public static List<PokerData> pokerData = LitJson.JsonMapper.ToObject<List<PokerData>> (Tools.ReadFile ("poker.txt"));
        public static Dictionary<int, PokerData> pokerDataDic = null;
        public static Dictionary<int, PokerData> PokerDataDic {
            get {
                if (pokerDataDic == null) {
                    pokerDataDic = new Dictionary<int, PokerData> ();
                    for (int i = 0; i < pokerData.Count; i++) {
                        pokerDataDic.Add (i, pokerData[i]);
                    }
                }
                return pokerDataDic;
            }
        }
        public int WaitTime = 3;
        public static Dictionary<string, List<TalkMsg>> msgList = new Dictionary<string, List<TalkMsg>> (); //房间内消息队列
        public static Dictionary<string, Room> roomDic = new Dictionary<string, Room> ();
        public static Dictionary<GIPEndPoint, User> allUser = new Dictionary<GIPEndPoint, User> ();
        public static User GetUser (GIPEndPoint _ip) {
            try {
                return allUser[_ip];
            } catch (System.Exception ex) {
                throw new Exception ($"找不到玩家！ip:{_ip} {ex.Message}");
            }
        }
        public static List<Client> GetRoomPlayersClient (string _room) {
            if (!roomDic.ContainsKey (_room)) return null;
            List<Client> _clients = new List<Client> ();
            foreach (var _ip in roomDic[_room].players) {
                Client _client = Server.GetClient (_ip);
                if (_client.Connected) {
                    _clients.Add (_client);
                }
            }
            return _clients;
        }
        public static List<Client> GetUsersAllClient () {
            List<Client> _clients = new List<Client> ();
            foreach (var _ip in allUser.Keys) {
                Client _client = Server.GetClient (_ip);
                if (_client.Connected) {
                    _clients.Add (_client);
                }
            }
            return _clients;
        }
        public static List<Client> GetUsersAllClient (UserState _state) {
            List<Client> _clients = new List<Client> ();
            var _users = from b in allUser where b.Value.userState == _state select b;
            foreach (var item in _users) {
                Client _client = Server.GetClient (item.Key);
                if (_client.Connected) {
                    _clients.Add (_client);
                }
            }
            return _clients;
        }
    }
    public class Room {
        public Room (int _roomId) { roomId = _roomId; }
        public Room () { }
        public int roomId = 0;
        public RoomState roomState = RoomState.none;
        public DateTime createTime = DateTime.Now;
        public List<GIPEndPoint> waitPlayers = new List<GIPEndPoint> ();
        public List<GIPEndPoint> players = new List<GIPEndPoint> ();
        public int PlayersCount => players.Count;
        public void StartGame () {
            Thread _game = new Thread (GameRun);
            _game.IsBackground = true;
            _game.Start ();
            roomState = RoomState.gameing;
            curGameState = GameState.start;
        }
        public bool CheckGameStart () {
            if (players.Count != PlayersNum) return false;
            foreach (var _ip in players) {
                User _user = GlobalData.GetUser (_ip);
                if (_user.curScene != GameScene.game || _user.userState != UserState.ready) {
                    return false;
                }
            }
            return true;
        }
        public bool CheckNetWork () {
            foreach (var _ip in players) {
                Client _clint = Server.GetClient (_ip);
                if (!_clint.Connected) {
                    return false;
                }
            }
            return true;
        }
        List<int>[] handsPai = new List<int>[PlayersNum];
        List<int> dipais = new List<int> ();
        int[] playerSouce = new int[PlayersNum];
        int lastWinnerSite = -1;
        int curOperSite = -1;
        int lastOperSite = -1;
        List<int> lastOperPai = new List<int> ();
        GameState curGameState = GameState.none;
        GameState nextGameState = GameState.none;
        public const int DipaiNum = 3;
        public const int AallPaiNum = 54;
        public const int PlayersNum = 3;
        Random randomNumber = new Random ();
        public bool CheckOper (int[] _pokers, int _strVal, int _maxIdx, List<int> _haveVal, int length) {
            int count = 1;
            int lastVal = _strVal;
            foreach (var nextVal in _haveVal) {
                if (nextVal - lastVal == 1 && _pokers[nextVal] == _pokers[_maxIdx]) {
                    lastVal = nextVal;
                    count++;
                } else {
                    break;
                }
            }
            if (count == length) {
                return true;
            }
            return false;
        }
        public bool CheckPlayerMsg () {
            foreach (var _ip in players) {
                User user = GlobalData.GetUser (_ip);
                if (user.gameInfo.gameState == UserGameState.waitMsg) {
                    return false;
                }
            }
            return true;
        }
        List<GIPEndPoint> lastWait = null;
        public void SetPlayerWaitMsg (UserGameState _state, List<GIPEndPoint> _players = null) {
            if (_players == null) _players = players;
            // if (_state == UserGameState.waitMsg) lastWait = _players;
            foreach (var _ip in _players) {
                User user = GlobalData.GetUser (_ip);
                user.gameInfo.gameState = _state;
            }
        }
        public void SetPlayerWaitMsg (UserGameState _state, GIPEndPoint _players) {
            SetPlayerWaitMsg (_state, new List<GIPEndPoint> () { _players });
        }
        public void GameRun () {
            while (true) {
                if (!CheckNetWork ()) {
                    continue;
                }
                switch (curGameState) {
                    case GameState.start:
                        {
                            curOperSite = -1;
                            ByteBuffer buff = new ByteBuffer ();
                            MessageManage.Self.SendMsg (10, 0, buff);
                            curGameState = GameState.waitmsg;
                            nextGameState = GameState.init;
                            SetPlayerWaitMsg(UserGameState.waitMsg);
                        }
                        break;
                    case GameState.init:
                        {
                            curOperSite = -1;
                            curGameState = GameState.fapai;
                        }
                        break;
                    case GameState.fapai:
                        {
                            List<int> idxs = new List<int> ();
                            handsPai = new List<int>[PlayersNum] {
                                new List<int> (),
                                new List<int> (),
                                new List<int> ()
                            };
                            for (int i = 0; i < AallPaiNum; i++) {
                                idxs.Add (i);
                            }
                            int playerIdx = 0;
                            while (idxs.Count > DipaiNum) {
                                int pokerIndex = randomNumber.Next (0, idxs.Count);
                                handsPai[playerIdx].Add (idxs[pokerIndex]);
                                idxs.RemoveAt (pokerIndex);
                                playerIdx = (++playerIdx) % PlayersNum;
                            }
                            while (idxs.Count > 0) {
                                int length = idxs.Count - 1;
                                dipais.Add (length);
                                idxs.RemoveAt (length);
                            }
                            ByteBuffer buff = new ByteBuffer ();
                            buff.WriteSByte ((sbyte) handsPai.Length);
                            for (int i = 0; i < handsPai.Length; i++) {
                                buff.WriteSByte ((sbyte) handsPai[i].Count);
                                for (int j = 0; j < handsPai[i].Count; j++) {
                                    buff.WriteSByte ((sbyte) handsPai[i][j]);
                                }
                            }
                            buff.WriteSByte ((sbyte) dipais.Count);
                            MessageManage.Self.SendMsg (10, 1, buff);
                            curGameState = GameState.waitmsg;
                        }
                        break;
                    case GameState.jiaofen:
                        {
                            if (curOperSite == -1) {
                                if (lastWinnerSite != -1) //上一位赢家
                                {
                                    curOperSite = lastWinnerSite;
                                    lastWinnerSite = -1;
                                } else {
                                    curOperSite = randomNumber.Next (0, PlayersNum);
                                }
                                playerSouce = new int[PlayersNum];
                            } else if (curOperSite != -1) {
                                curOperSite = (++curOperSite) % PlayersNum; //下一位
                            }
                            ByteBuffer buff = new ByteBuffer ();
                            buff.WriteSByte ((sbyte) curOperSite);
                            MessageManage.Self.SendMsg (10, 2, buff);
                        }
                        curGameState = GameState.waitmsg;
                        break;
                    case GameState.fandipai:
                        {
                            ByteBuffer buff = new ByteBuffer ();
                            buff.WriteSByte ((sbyte) dipais.Count);
                            for (int i = 0; i < dipais.Count; i++) {
                                buff.WriteSByte ((sbyte) dipais[i]);
                            }
                            MessageManage.Self.SendMsg (10, 2, buff);
                        }
                        curGameState = GameState.waitmsg;
                        break;
                    case GameState.playerOper:
                        {
                            if (lastOperSite == -1) {
                                lastOperSite = curOperSite;
                            } else {

                            }
                            ByteBuffer buff = new ByteBuffer ();
                            buff.WriteSByte ((sbyte) lastOperSite);
                            MessageManage.Self.SendMsg (10, 2, buff);
                        }
                        curGameState = GameState.waitmsg;
                        break;
                    case GameState.waitmsg:
                        {
                            if (!CheckPlayerMsg ()) {
                                SetPlayerWaitMsg (UserGameState.none);
                                curGameState = nextGameState;
                                nextGameState = GameState.none;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        public PokerOper GetOperType (int[] realVal) {
            if (realVal.Length <= 0) {
                return PokerOper.none;
            }
            int allCount = 0;
            int strVal = -1;
            int maxIdx = -1;
            int[] pokers = new int[Enum.GetNames (typeof (PokerValue)).Length - 1];
            List<int> haveVal = new List<int> ();
            foreach (var item in realVal) {
                int val = (int) GlobalData.PokerDataDic[item].valueType;
                allCount++;
                if (strVal == -1) {
                    strVal = val;
                }
                if (pokers[val] == 0) {
                    haveVal.Add (val);
                }
                pokers[val]++;
                maxIdx = pokers[val] > maxIdx ? val : maxIdx;
            }
            haveVal.Remove (maxIdx);

            switch (allCount) {
                case 0:
                    return PokerOper.none;
                case 1:
                    return PokerOper.danzhang;
                case 2:
                    if (pokers[maxIdx] == allCount) {
                        return PokerOper.duizi;
                    }
                    return PokerOper.none;
                case 3:
                    if (pokers[maxIdx] == allCount) {
                        return PokerOper.sanbudai;
                    }
                    return PokerOper.none;
                case 4:
                    if (pokers[maxIdx] == allCount) {
                        return PokerOper.zhadan;
                    } else if (pokers[maxIdx] == 3) {
                        return PokerOper.sandaiyi;
                    } else if (pokers[maxIdx] == 2) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    }
                    return PokerOper.none;
                case 5:
                    if (pokers[maxIdx] == 3) {
                        if (haveVal[0] == 1) {
                            return PokerOper.sandai2danzhang;
                        } else if (haveVal[0] == 2) {
                            return PokerOper.sandai2dui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 6:
                    if (pokers[maxIdx] == 4) {
                        if (haveVal[0] == 1) {
                            return PokerOper.sidai2danzhang;
                        } else if (haveVal[0] == 2) {
                            return PokerOper.sidai1dui;
                        }
                    } else if (pokers[maxIdx] == 3) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.feijibudai;
                        }
                    } else if (pokers[maxIdx] == 2) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 7:
                    if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 8:
                    if (pokers[maxIdx] == 3) {
                        for (int i = strVal; i < pokers.Length - 1; i++) {
                            if (pokers[i] >= pokers[maxIdx] && pokers[i + 1] >= pokers[maxIdx]) {
                                return PokerOper.feijichibang;
                            }
                        }
                    } else if (pokers[maxIdx] == 2) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 9:
                    if (pokers[maxIdx] == 3) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 10:
                    if (pokers[maxIdx] == 2) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 11:
                    if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 12:
                    if (pokers[maxIdx] == 3) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 2) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount / pokers[maxIdx])) {
                            return PokerOper.liandui;
                        }
                    } else if (pokers[maxIdx] == 1) {
                        if (CheckOper (pokers, strVal, maxIdx, haveVal, allCount)) {
                            return PokerOper.shunzi;
                        }
                    }
                    return PokerOper.none;
                case 13:
                    return PokerOper.none;
                case 14:
                    return PokerOper.none;
                case 15:
                    return PokerOper.none;
                case 16:
                    return PokerOper.none;
                case 17:
                    return PokerOper.none;
                case 18:
                    return PokerOper.none;
                case 19:
                    return PokerOper.none;
                case 20:
                    return PokerOper.none;
                case 21:
                    return PokerOper.none;
                case 22:
                    return PokerOper.none;
                case 23:
                    return PokerOper.none;
                case 24:
                    return PokerOper.none;
                default:
                    break;
            }
            return PokerOper.none;
        }
    }
    public class Client {
        public Socket socket = null;
        public GIPEndPoint ip = null;
        public bool Connected {
            get {
                return socket == null ? false : socket.Connected;
            }
        }
    }
    public class User {
        public GameScene curScene = GameScene.none;
        public UserState userState = UserState.none;
        public UserInfo info = new UserInfo ();
        public UserGameInfo gameInfo = new UserGameInfo ();
    }
    public class UserInfo {
        public string name = "guset";
        public string password = "";
        public string headUrl = "";
        public int lv = 1;
        public int exc = 0;
        public int score = 0;
        public int coin = 10000;
    }
    public class UserGameInfo {
        public UserGameState gameState = UserGameState.none;
        public int site = 0;
        public int score = 0;
    }
    public class PlayerInfo {
        public int state = 0;
        public int[] dachuPai = null;
        public int[] lastPai = null;
        public int[] handspai = null;
        public int score = 0;
    }

    [ProtoContract]
    public class ReceiveMsgData {
        [ProtoMember (1)]
        public GIPEndPoint receivePoint;
        [ProtoMember (2)]
        public byte[] receiveBytes;
        [ProtoMember (3)]
        public int receiveLength;
    }

    [ProtoContract]
    [Serializable]
    public class GIPEndPoint {
        [ProtoMember (1)]
        public string ip = "127.0.0.1";
        [ProtoMember (2)]
        public int port = 1234;
        public GIPEndPoint () { }
        public static implicit operator IPEndPoint (GIPEndPoint payment) {
            return new IPEndPoint (IPAddress.Parse (payment.ip), payment.port);
        }
        public static implicit operator EndPoint (GIPEndPoint payment) {
            return new IPEndPoint (IPAddress.Parse (payment.ip), payment.port) as EndPoint;
        }
        public static implicit operator GIPEndPoint (IPEndPoint payment) {
            return new GIPEndPoint () {
                ip = payment.Address.ToString (),
                    port = payment.Port,
            };
        }
        public static implicit operator GIPEndPoint (EndPoint payment) {
            IPEndPoint point = (IPEndPoint) payment;
            return new GIPEndPoint () {
                ip = point.Address.ToString (),
                    port = point.Port,
            };
        }
        public override string ToString () {
            return string.Format ("{0}:{1}", ip, port);
        }
        public override bool Equals (object obj) {
            if (obj.GetType () == typeof (EndPoint) || obj.GetType () == typeof (IPEndPoint)) {
                IPEndPoint point = obj as IPEndPoint;
                return point.Address.ToString () == ip && port == point.Port;
            } else if (obj.GetType () == typeof (GIPEndPoint)) {
                GIPEndPoint point = obj as GIPEndPoint;
                return point.ip == ip && port == point.port;
            }
            return false;
        }
        public override int GetHashCode () {
            return ip.GetHashCode () + port.GetHashCode ();
        }
        public static bool operator == (GIPEndPoint left, GIPEndPoint right) {
            return left.ip == right.ip && left.port == right.port;
        }
        public static bool operator != (GIPEndPoint left, GIPEndPoint right) {
            return !(left == right);
        }
    }

    [ProtoContract]
    public class MessageData {
        [ProtoMember (1)]
        public HeadMsg head = new HeadMsg ();
        [ProtoMember (2)]
        public GIPEndPoint receivePoint = new GIPEndPoint ();
        [ProtoMember (3)]
        public byte[] data = null;
        public MessageData () {

        }
        public MessageData (byte[] _data) {
            data = _data;
        }
    }

    [ProtoContract]
    public class HeadMsg {
        [ProtoMember (1)]
        public int flgHead = 0;
        [ProtoMember (2)]
        public int cmd = 0;
        [ProtoMember (3)]
        public int scmd = 0;
        [ProtoMember (4)]
        public int msgLen = 0;
        [ProtoMember (5)]
        public int msgOrder = 0;
        [ProtoMember (6)]
        public int msgUid = 0;
        [ProtoMember (7)]
        public int msgToken = 0;
        [ProtoMember (8)]
        public int flgEnd = 0;
        public HeadMsg () { }
    }
    public class PokerData {
        public PokerValue valueType = PokerValue._3;
        public PokerType pokerType = PokerType.fangpian;
    }
    public enum PokerValue {
        none = -1,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        J,
        Q,
        K,
        A,
        _2,
        minJoker,
        maxJoker,
        laizi
    }
    public enum PokerType {
        none = -1,
        heitao,
        hongtao,
        meihua,
        fangpian,
        wang,
        laizi,
    }
    public enum PokerOper {
        none,
        danzhang,
        duizi,
        shunzi,
        sanbudai,
        zhadan,
        sandaiyi,
        sandai2danzhang,
        sandai2dui,
        sidai2danzhang,
        sidai1dui,
        feijibudai,
        feijichibang,
        liandui,
    }
    public enum GameState {
        none,
        start,
        fapai,
        fapaiOver,
        jiaofen,
        gameing,
        oversuanfen,
        over,
        waitmsg,
        fandipai,
        playerOper,
        init,
    }
    public enum RoomState {
        none,
        gameing,
    }
    public enum GameScene {
        none,
        game,
        room,
        hall,
        login,
    }
    public enum UserState {
        none,
        ready,
        gameing,
        viewer, //观战
    }
    public enum UserGameState {
        none,
        waitMsg
    }
}