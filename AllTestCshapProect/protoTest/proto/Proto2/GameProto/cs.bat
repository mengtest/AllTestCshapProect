@echo on
@set Path=E:\Zhangxin\Project\Unity3D\project_qmonster2\Tools\Conf\tools\protogen
@md cs
%Path% -i:GameProtocol.Account.proto -o:cs\GameProtocol.Account.cs -d -ns:GameProto
%Path% -i:GameProtocol.Center.proto -o:cs\GameProtocol.Center.cs -d -ns:GameProto
%Path% -i:GameProtocol.Common.proto -o:cs\GameProtocol.Common.cs -d -ns:GameProto
%Path% -i:GameProtocol.LOG.proto -o:cs\GameProtocol.LOG.cs -d -ns:GameProto
%Path% -i:GameProtocol.Mail.proto -o:cs\GameProtocol.Mail.cs -d -ns:GameProto
%Path% -i:GameProtocol.MsgID.proto -o:cs\GameProtocol.MsgID.cs -d -ns:GameProto
%Path% -i:GameProtocol.Rank.proto -o:cs\GameProtocol.Rank.cs -d -ns:GameProto
%Path% -i:GameProtocol.RegAuth.proto -o:cs\GameProtocol.RegAuth.cs -d -ns:GameProto
%Path% -i:GameProtocol.USERDB.proto -o:cs\GameProtocol.USERDB.cs -d -ns:GameProto
%Path% -i:GameProtocol.World.proto -o:cs\GameProtocol.World.cs -d -ns:GameProto
%Path% -i:GameProtocol.Zone.proto -o:cs\GameProtocol.Zone.cs -d -ns:GameProto
%Path% -i:GameProtocol.CS.proto -o:cs\GameProtocol.CS.cs -d -ns:GameProto

pause