@echo on
@set Path=%~dp0\protogen\protogen.exe
%Path% -i:MsgData.proto -o:MsgData.cs -d
pause