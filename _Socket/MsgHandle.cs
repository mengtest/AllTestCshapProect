using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using _Socket_Core;
using Z_CShapCore;

namespace _Socket_Client {
    public class MsgHandle {

        private static MsgHandle _self = new MsgHandle();

        public static MsgHandle Self {
            get {
                return _self;
            }
        }

        private MsgHandle () { }

        public void DealMsgSwitch (MessageData data) {
            Console.WriteLine($"dealMsg {data.Head.Cmd}_{data.Head.SCmd}");
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
                            Console.WriteLine("not del {data.head.cmd}_{data.head.scmd}");
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Send_1_1 () {
            CS_UserInfo body = new CS_UserInfo();
            //NetClient.SendMsg(1 , 1 , body);
        }

        public void Deal_1_1 (MessageData data) {
            Console.WriteLine("个人信息");
            //SC_UserInfo messageData = data.Body as SC_UserInfo;
            //Console.WriteLine(StaticTools.ToJson(messageData));
        }

        public void Send_1_2 () {
            BodyMsg body = new BodyMsg();
            NetClient.SendMsg(1 , 2 , body);
        }

        public void Deal_1_2 (MessageData data) {
            //MessageData_1_2 messageData = data.Body as MessageData_1_2;
            //Console.WriteLine(messageData.Describe);
        }

        public void Deal_1_10 (MessageData data) {
            //MessageData_1_10 messageData = data.Body as MessageData_1_10;
            //string str = messageData.TalkStr;

            //Console.WriteLine(StaticTools.ToJson(messageData));

            //MessageData_1_10 body = new MessageData_1_10();
            //body.talkStr = str;
            //SendMsg(1, 10, body);
        }

        public void Send_1_10 (string _str) {
            //MessageData_1_10 body = new MessageData_1_10();
            //body.TalkStr = _str;
            //NetClient.SendMsg(1 , 10 , body);
        }

        public void Deal_1_12 (MessageData data) {

        }

        public void Send_1_12 () {
            //MessageData_1_11 messageData = new MessageData_1_11();
            //BodyMsg body = new BodyMsg();
            //NetClient.SendMsg(1 , 12 , body);
        }

        public void Deal_1_11 (MessageData data) {
            //MessageData_1_11 messageData = data.Body as MessageData_1_11;
        }

        public void Send_1_11 (Dictionary<int , List<GVector3>> _oper) {
            //MessageData_1_11 data = new MessageData_1_11();
            //data.Oper = _oper;
            //NetClient.SendMsg(1 , 11 , data);
        }
    }
}