using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Z_CShapCore;

namespace ddzServer
{
    public class MessageManage
    {
        private static MessageManage _self;
        public static MessageManage Self
        {
            get
            {
                if (_self == null)
                {
                   _self = new MessageManage();
                }
                return _self;
            }
        }

        private MessageManage() {

        }

        public void Init()
        {
            Server.OnReceiveMessage += DealMsg;
        }
        
        public void DealMsg(ReceiveMsgData _data)
        {
            if (_data == null) return;
            
            byte[] _dataBytes = _data.receiveBytes;
            IPEndPoint receivePoint = _data.receivePoint;
            ByteBuffer _buff = new ByteBuffer(_dataBytes);
            int length = _buff.ReadInt32();         //收到的消息结构实际长度

            if (length > 0)
            {
                if (length <= _data.receiveLength)
                {
                    byte[] _bytes = _buff.ReadBytes(length);
                    MessageData data = ProtoBufTools.DeSerialize<MessageData>(_bytes);
                    data.receivePoint = receivePoint;
                    DealMsgSwitch(data);
                }

                //除本次包内容外还有其它数据，有多余的字节，黏包，再处理一次
                int dataLength = _data.receiveLength - 4;
                if (dataLength > length)
                {
                    int leftLength = dataLength - length;
                    _data.receiveBytes = _buff.ReadBytes(leftLength);
                    _data.receiveLength = leftLength;
                    DealMsg(_data);
                }
            }
        }
        
        public void SendMsg(int cmd, int scmd, ByteBuffer buffer,List<Client> _users = null)
        {
            MessageData sendMsg = new MessageData();
            HeadMsg head = new HeadMsg();
            head.cmd = cmd;
            head.scmd = scmd;
            sendMsg.head = head;
            sendMsg.data = buffer.ToBytes();
            byte[] bytes = ProtoBufTools.Serialize(sendMsg);
            ByteBuffer sendBuffer = new ByteBuffer();
            sendBuffer.WriteInt32(bytes.Length);
            sendBuffer.WriteBytes(bytes);
            bytes = sendBuffer.ToBytes();
            if (_users == null)
            {
                _users = GlobalData.GetUsersAllClient();
            }
            foreach (var socket in _users)
            {
                if (socket.Connected)
                {
                    socket.socket.Send(bytes);
                }
            }
        }

        public void DealMsgSwitch(MessageData data)
        {
            switch (data.head.cmd)
            {
                case 1:
                    switch (data.head.scmd)
                    {
                        case 1: Deal_1_1(data); break;
                        case 2: Deal_1_2(data); break;
                        case 10: Deal_1_10(data); break;
                        case 11: Deal_1_11(data); break;
                        case 12: Deal_1_12(data); break;
                        default: break;
                    }
                    break;
                case 2:
                    switch (data.head.scmd)
                    {
                        case 1: Deal_2_1(data); break;
                        default: break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Send_1_1()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            SendMsg(1, 1, buff);
        }

        public void Deal_1_1(MessageData data)
        {

        }

        public void Send_2_1(GIPEndPoint _ip)
        {
            ByteBuffer buff = new ByteBuffer();
            User user = GlobalData.GetUser(_ip);
            buff.WriteString(user.ToJson());
            SendMsg(2, 1, buff);
        }
        public void Deal_2_1(MessageData data)
        {
            ByteBuffer buff = new ByteBuffer(data.data);
            Send_2_1(data.receivePoint);
        }

        public void Send_1_2()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            SendMsg(1, 2, buff);
        }

        public void Deal_1_2(MessageData data)
        {

        }

        public void Deal_1_10(MessageData data)
        {

        }


        public void Send_1_10(string _str)
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            SendMsg(1, 10, buff);
        }

        public void Deal_1_12(MessageData data)
        {

        }

        public void Send_1_12()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            SendMsg(1, 12, buff);
        }

        public void Deal_1_11(MessageData data)
        {
        }

        public void Send_1_11()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            SendMsg(1, 11, buff);
        }
    }
}
