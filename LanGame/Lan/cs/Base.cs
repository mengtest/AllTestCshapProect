//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Base.proto
namespace Base
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
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MessageData")]
  public partial class MessageData : global::ProtoBuf.IExtensible
  {
    public MessageData() {}
    
    private int _ret = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"ret", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ret
    {
      get { return _ret; }
      set { _ret = value; }
    }
    private string _desc = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"desc", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string desc
    {
      get { return _desc; }
      set { _desc = value; }
    }
    private int _com = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"com", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int com
    {
      get { return _com; }
      set { _com = value; }
    }
    private int _tsak = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"tsak", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int tsak
    {
      get { return _tsak; }
      set { _tsak = value; }
    }
    private string _data = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PhoneNumber")]
  public partial class PhoneNumber : global::ProtoBuf.IExtensible
  {
    public PhoneNumber() {}
    
    private string _number;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"number", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string number
    {
      get { return _number; }
      set { _number = value; }
    }
    private Base.PhoneType _type = Base.PhoneType.HOME;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Base.PhoneType.HOME)]
    public Base.PhoneType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Person")]
  public partial class Person : global::ProtoBuf.IExtensible
  {
    public Person() {}
    
    private string _name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private int _id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _email = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"email", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string email
    {
      get { return _email; }
      set { _email = value; }
    }
    private readonly global::System.Collections.Generic.List<Base.PhoneNumber> _phone = new global::System.Collections.Generic.List<Base.PhoneNumber>();
    [global::ProtoBuf.ProtoMember(4, Name=@"phone", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Base.PhoneNumber> phone
    {
      get { return _phone; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AddressBook")]
  public partial class AddressBook : global::ProtoBuf.IExtensible
  {
    public AddressBook() {}
    
    private readonly global::System.Collections.Generic.List<Base.Person> _person = new global::System.Collections.Generic.List<Base.Person>();
    [global::ProtoBuf.ProtoMember(1, Name=@"person", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Base.Person> person
    {
      get { return _person; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
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
  
    [global::ProtoBuf.ProtoContract(Name=@"PhoneType")]
    public enum PhoneType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"MOBILE", Value=0)]
      MOBILE = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"HOME", Value=1)]
      HOME = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WORK", Value=2)]
      WORK = 2
    }
  
}