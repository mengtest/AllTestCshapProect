//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: MsgData.proto
namespace MsgData
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GIPEndPoint")]
  public partial class GIPEndPoint : global::ProtoBuf.IExtensible
  {
    public GIPEndPoint() {}
    

    private int _Ip = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"Ip", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int Ip
    {
      get { return _Ip; }
      set { _Ip = value; }
    }

    private string _Port = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Port", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string Port
    {
      get { return _Port; }
      set { _Port = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GVector3")]
  public partial class GVector3 : global::ProtoBuf.IExtensible
  {
    public GVector3() {}
    

    private int _X = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"X", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int X
    {
      get { return _X; }
      set { _X = value; }
    }

    private int _Y = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Y", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int Y
    {
      get { return _Y; }
      set { _Y = value; }
    }

    private int _Z = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"Z", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int Z
    {
      get { return _Z; }
      set { _Z = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReceiveMessageData")]
  public partial class ReceiveMessageData : global::ProtoBuf.IExtensible
  {
    public ReceiveMessageData() {}
    

    private MsgData.GIPEndPoint _ReceivePoint = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"ReceivePoint", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public MsgData.GIPEndPoint ReceivePoint
    {
      get { return _ReceivePoint; }
      set { _ReceivePoint = value; }
    }

    private int _ReceiveLength = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"ReceiveLength", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ReceiveLength
    {
      get { return _ReceiveLength; }
      set { _ReceiveLength = value; }
    }
    private readonly global::System.Collections.Generic.List<int> _ReceiveBytes = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(3, Name=@"ReceiveBytes", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> ReceiveBytes
    {
      get { return _ReceiveBytes; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HeadMsg")]
  public partial class HeadMsg : global::ProtoBuf.IExtensible
  {
    public HeadMsg() {}
    

    private int _FlgHead = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"FlgHead", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int FlgHead
    {
      get { return _FlgHead; }
      set { _FlgHead = value; }
    }

    private int _Cmd = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Cmd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int Cmd
    {
      get { return _Cmd; }
      set { _Cmd = value; }
    }
    private readonly global::System.Collections.Generic.List<int> _SCmd = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(3, Name=@"SCmd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> SCmd
    {
      get { return _SCmd; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _MsgLen = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(4, Name=@"MsgLen", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> MsgLen
    {
      get { return _MsgLen; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _MsgOrder = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(5, Name=@"MsgOrder", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> MsgOrder
    {
      get { return _MsgOrder; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _MsgUid = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(6, Name=@"MsgUid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> MsgUid
    {
      get { return _MsgUid; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _MsgToken = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(7, Name=@"MsgToken", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> MsgToken
    {
      get { return _MsgToken; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _FlgEnd = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(8, Name=@"FlgEnd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> FlgEnd
    {
      get { return _FlgEnd; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CS_LoginUser")]
  public partial class CS_LoginUser : global::ProtoBuf.IExtensible
  {
    public CS_LoginUser() {}
    

    private uint _uid = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint uid
    {
      get { return _uid; }
      set { _uid = value; }
    }

    private string _userName = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"userName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string userName
    {
      get { return _userName; }
      set { _userName = value; }
    }

    private string _passWord = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"passWord", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string passWord
    {
      get { return _passWord; }
      set { _passWord = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SC_LoginUser")]
  public partial class SC_LoginUser : global::ProtoBuf.IExtensible
  {
    public SC_LoginUser() {}
    

    private int _Ret = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"Ret", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int Ret
    {
      get { return _Ret; }
      set { _Ret = value; }
    }

    private uint _Sex = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Sex", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint Sex
    {
      get { return _Sex; }
      set { _Sex = value; }
    }

    private string _Nick = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"Nick", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string Nick
    {
      get { return _Nick; }
      set { _Nick = value; }
    }

    private uint _Uid = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"Uid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint Uid
    {
      get { return _Uid; }
      set { _Uid = value; }
    }

    private string _Token = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"Token", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string Token
    {
      get { return _Token; }
      set { _Token = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MessageData")]
  public partial class MessageData : global::ProtoBuf.IExtensible
  {
    public MessageData() {}
    

    private MsgData.HeadMsg _Head = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"Head", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public MsgData.HeadMsg Head
    {
      get { return _Head; }
      set { _Head = value; }
    }

    private MsgData.CS_LoginUser _cs_LoginUser = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"cs_LoginUser", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public MsgData.CS_LoginUser cs_LoginUser
    {
      get { return _cs_LoginUser; }
      set { _cs_LoginUser = value; }
    }

    private MsgData.SC_LoginUser _sc_LoginUser = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"sc_LoginUser", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public MsgData.SC_LoginUser sc_LoginUser
    {
      get { return _sc_LoginUser; }
      set { _sc_LoginUser = value; }
    }

    private MsgData.MessageData.Corpus _corpus = MsgData.MessageData.Corpus.UNIVERSAL;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"corpus", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(MsgData.MessageData.Corpus.UNIVERSAL)]
    public MsgData.MessageData.Corpus corpus
    {
      get { return _corpus; }
      set { _corpus = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Corpus")]
    public enum Corpus
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"UNIVERSAL", Value=0)]
      UNIVERSAL = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WEB", Value=1)]
      WEB = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IMAGES", Value=2)]
      IMAGES = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOCAL", Value=3)]
      LOCAL = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NEWS", Value=4)]
      NEWS = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PRODUCTS", Value=5)]
      PRODUCTS = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"VIDEO", Value=6)]
      VIDEO = 6
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}