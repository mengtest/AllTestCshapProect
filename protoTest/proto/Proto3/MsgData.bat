@echo on
@set Path=protoc
%Path% --csharp_out=. MsgData.proto
pause