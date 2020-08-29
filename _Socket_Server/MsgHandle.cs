using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using _Socket_Core;
using Z_CShapCore;

namespace _Socket_Server {
    public class MsgHandle {
        private static MsgHandle _self = new MsgHandle();
        public static MsgHandle Self {
            get {
                return _self;
            }
        }
        private MsgHandle () { }
        public void SendMsg (BodyMsg body , List<Socket> list = null) {
            MessageData data = new MessageData();
            HeadMsg head = new HeadMsg();
            data.Head = head;
            data.Body = body;
            ByteBuffer _buff = new ByteBuffer();
            byte[] bytes = StaticTools.Serialize(data);
            _buff.WriteInt32(bytes.Length);
            _buff.WriteBytes(bytes);
            bytes = _buff.ToBytes();
            if (list == null) {
                list = GlobalData.GetAllPlayer();
            }
            foreach (var socket in list) {
                if (socket.Connected) {
                    socket.Send(bytes);
                }
            }
        }

        public void DealMsgSwitch (MessageData data) {
            switch (data.Head.Cmd) {
                case 1:
                    switch (data.Head.SCmd) {
                        case 1:
                            Deal_1_1(data);
                            break;
                        case 2:
                            Deal_1_2(data);
                            break;
                        case 10:
                            Deal_1_10(data);
                            break;
                        case 11:
                            Deal_1_11(data);
                            break;
                        case 12:
                            Deal_1_12(data);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnSocketConnect (Socket _socket , EndPoint _point) {
            Console.WriteLine($"{_point.ToString()} 连接了!");

            Player player = GlobalData.GetPlayer(_point);
            if (player == null) {
                GlobalData.AddPlayer(_point,_socket);

                //MessageData_1_2 b = new MessageData_1_2();
                //b.Describe = $"{_point.ToString()} 加入了 ！";
                //MsgHandle.Self.SendMsg(1 , 2 , b);
                //
                //for (int i = 0 ; i < 30 ; i++) {
                //    SC_UserInfo body = new SC_UserInfo();
                //    body.Info.Key = i + " : " + DateTime.Now.ToString();
                //    MsgHandle.Self.SendMsg(1 , 1 , body , new List<Socket>() { _socket });
                //}
            } else {
                
            }
        }

        public void Send_1_1 () {
            BodyMsg body = new BodyMsg();
            //SendMsg(1 , 1 , body);
        }
        
        public void Send_1_2 () {
            BodyMsg body = new BodyMsg();
            //SendMsg(1 , 2 , body);
        }

        public void Send_1_10 (string _str) {
            //MessageData_1_10 body = new MessageData_1_10();
            //body.TalkStr = _str;
            //SendMsg(1 , 10 , body);
        }

        public void Send_1_12 () {

        }
        public void Send_1_11 (Dictionary<int , List<GVector3>> _oper) {
            //MessageData_1_11 data = new MessageData_1_11();
            //data.Oper = _oper;
            //SendMsg(1 , 11 , data);
        }


        public void Deal_1_1 (MessageData data) {
            //CS_UserInfo messageData = data.Body as CS_UserInfo;
        }
        public void Deal_1_2 (MessageData data) {

        }
        public void Deal_1_10 (MessageData data) {
            //MessageData_1_10 messageData = data.Body as MessageData_1_10;
            //string str = messageData.TalkStr;
            //MessageData_1_10 body = new MessageData_1_10();
            //body.TalkStr = str;
            //SendMsg(1 , 10 , body);
        }
        public void Deal_1_11 (MessageData data) {
            //MessageData_1_11 messageData = data.Body as MessageData_1_11;
        }
        public void Deal_1_12 (MessageData data) {

        }
    }
}
