using System;
using LuaInterface;
using System.Net;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Calllua
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                string dir = System.Environment.CurrentDirectory;
                dir = dir.Replace("\\", "/");

                try {
                    Tools.Self.lua.DoString($"package.path = package.path .. \";{dir};{dir}/dll/?.dll;\"");   //添加lua查找路径
                } catch (Exception) {
                }

                Tools.Self.lua.DoFile("lua/main.lua");          //入口文件
                Tools.Self.lua.DoString("app_args = {}");   //启动参数
                for (int i = 0; i < args.Length; i++)
                {
                    string str = $"table.insert(app_args,[[{args[i]}]])";
                    Tools.Self.lua.DoString(str);
                }
                Tools.Self.lua.DoString("StartMain()");     //启动方法
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
            Console.ReadKey();
        }
    }
}