using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Z_CShapCore;

namespace _Socket_Core {
    public class MessageManage {

        private static MessageManage _self = new MessageManage();

        public static MessageManage Self {
            get {
                return _self;
            }
        }

        public event Action<MessageData> DealMsgEvent;

        public void DealMsg (ReceiveMessageData _data) {
            if (_data == null)
                return;
            byte[] _dataBytes = _data.ReceiveBytes;
            ByteBuffer _buff = new ByteBuffer(_dataBytes);
            int length = _buff.ReadInt32();         //收到的消息结构长度
            byte[] _bytes = _buff.ReadBytes(length);

            int dataLength = _data.ReceiveLength - 4;   //去掉包长度的字节，剩下的为内容长度
            if (dataLength > length) {  //有多余的字节，黏包
                int leftLength = dataLength - length;
                _data.ReceiveBytes = _buff.ReadBytes(leftLength);
                _data.ReceiveLength = leftLength;
                DealMsg(_data);
            } else if (dataLength == length) {
                MessageData data = ProtoBufTools.DeSerialize<MessageData>(_bytes);
                data.Head.ReceivePoint = _data.ReceivePoint;
                if (DealMsgEvent != null) {
                    DealMsgEvent(data);
                }
            }
        }
    }
}
