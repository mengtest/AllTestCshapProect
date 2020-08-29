using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _Socket_Core {
    [ProtoContract]
    public class GVector3 {
        [ProtoMember(1)]
        public double X { get; set; }
        [ProtoMember(2)]
        public double Y { get; set; }
        [ProtoMember(3)]
        public double Z { get; set; }
        public GVector3 () {

        }
        public GVector3 (double _x , double _y , double _z) {
            X = _x;
            Y = _y;
            Z = _z;
        }
        public override string ToString () {
            return string.Format("{0},{1},{2}" , X , Y , Z);
        }
    }

    [ProtoContract]
    public class GIPEndPoint {
        [ProtoMember(1)]
        public string Ip { get; set; }
        [ProtoMember(2)]
        public int Port { get; set; }
        public GIPEndPoint () {
        }
    }

    [ProtoContract]
    public class ReceiveMessageData {
        [ProtoMember(1)]
        public GIPEndPoint ReceivePoint { get; set; }
        [ProtoMember(2)]
        public byte[] ReceiveBytes { get; set; }
        [ProtoMember(3)]
        public int ReceiveLength { get; set; }
    }

    [ProtoContract]
    public class MessageData{
        [ProtoMember(1)]
        public HeadMsg Head { get; set; }
        [ProtoMember(2)]
        public BodyMsg Body { get; set; }
        public MessageData () {
        }
    }

    [ProtoContract]
    public class HeadMsg {
        [ProtoMember(1)]
        public GIPEndPoint ReceivePoint { get; set; }
        [ProtoMember(2)]
        public int DealFlg { get; set; }
        [ProtoMember(3)]
        public string HandleName { get; set; }
        public HeadMsg () { }
    }

    [ProtoContract]
    public class BodyMsg {
        [ProtoMember(1)]
        public SC_UserInfo sc_UserInfo { get; set; }
        [ProtoMember(2)]
        public CS_UserInfo cs_UserInfo { get; set; }

        public BodyMsg () {
        }
    }

    [ProtoContract]
    public class SC_UserInfo {
        [ProtoMember(1)]
        public int Uid { get; set; }
        [ProtoMember(2)]
        public int Nick { get; set; }
        [ProtoMember(3)]
        public int Sex { get; set; }
        [ProtoMember(4)]
        public int Level { get; set; }
        [ProtoMember(5)]
        public int Exp { get; set; }
        [ProtoMember(6)]
        public int Score { get; set; }
        [ProtoMember(7)]
        public int VipLevel { get; set; }
        public SC_UserInfo () {
        }
    }

    [ProtoContract]
    public class CS_UserInfo {

    }
}