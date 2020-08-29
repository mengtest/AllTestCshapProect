@echo on
@set Path=E:\Zhangxin\Project\Unity3D\project_qmonster2\Tools\Conf\tools\protogen
@md cs
%Path% -i:my.proto -o:cs\my.cs -d -ns:my
pause