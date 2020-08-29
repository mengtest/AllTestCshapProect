using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexTest
{
    class Program
    {
        static List<string> allDirs = new List<string> {
            @"\Assets\spine-csharp",
            @"\Assets\spine-unity",
            @"\Assets\Code",
            @"\Assets\Editor",
            @"\Assets\LuaFramework\Editor",
            @"\Assets\LuaFramework\Scripts",
            @"\Assets\LuaFramework\ToLua\BaseType",
            @"\Assets\LuaFramework\ToLua\Lua",
            @"\Assets\LuaFramework\ToLua\Misc",
            @"\Assets\LuaFramework\ToLua\Reflection",

            //@"\Assets\LuaFramework\ToLua\Core",
            //@"\Assets\LuaFramework\ToLua\Editor",

            //@"\Assets\LuaFramework\Scripts\Manager\GameManager.cs",
        };

        //白名单，不会处理的文件
        static List<string> whiteFilesUrl = new List<string>
        {
            @"\Assets\Code\WordFilter.cs",
            @"\Assets\LuaFramework\Scripts\Framework\Core\Message.cs",
        };
        
        static List<string> allFiles = new List<string>();

        static int allFilesCount = 0;
        static int curFilesCount = 0;

        static void GeAllFiles(string _url)
        {
            if (Directory.Exists(_url))
            {
                var files = Directory.GetFileSystemEntries(_url);
                foreach (var f in files) GeAllFiles(f);
            }
            else if(File.Exists(_url))
            {
                var name = Path.GetFileName(_url);
                var ext = Path.GetExtension(_url);
                if (whiteFilesUrl.Contains(name)) return;
                if (ext != ".cs") return;
                allFiles.Add(_url);
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("--代码混淆工具--");
            Console.WriteLine("注：请不要对已经混淆过的文件多次混淆！");
            RegexTools.stopwatch.Start(); //用代码运行时间生成不重复的key

            string curDirName = Directory.GetCurrentDirectory();
            if (!Directory.Exists(curDirName + @"\Assets"))
            {
                Console.WriteLine("请将工具放在项目中与Assets同级目录后重新运行，或手动输入项目路径！");
                Console.Write("请输入：");
                curDirName = Console.ReadLine();
                while (!Directory.Exists(curDirName + @"\Assets"))
                {
                    Console.WriteLine(curDirName + @"\Assets  不存在！");
                    Console.Write("请输入：");
                    curDirName = Console.ReadLine();
                }
            }
            
            string projectUrl = curDirName;

            string tempDirName = projectUrl + @"\TempRegexTest";      //备份目录
            int tempDirIdx = 1;
            while (Directory.Exists(tempDirName + tempDirIdx))
                tempDirIdx += 1;
            tempDirName = tempDirName + tempDirIdx;
            
            foreach (var dirUrl in allDirs)
                GeAllFiles(projectUrl + dirUrl);   //生成所有需要处理的文件路径

            allFilesCount = allFiles.Count;
            if (allFilesCount <= 0)
            {
                Console.WriteLine("没有需要处理的文件，请检查目录！");
                Console.WriteLine("请输入任意键退出");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"共{allFilesCount}个文件需要处理");
            System.Threading.Thread.Sleep(1000);

            //foreach (var fileUrl in allFiles)
            //{
            //    Console.WriteLine(fileUrl);
            //
            //    using (StreamReader read = File.OpenText(fileUrl))
            //    {
            //        string curLine = "";
            //
            //        List<string> strs = new List<string>();
            //        while ((curLine = read.ReadLine()) != null)
            //        {
            //            if (Regex.IsMatch(curLine, @"//")) curLine = Regex.Replace(curLine, @"//.*", "");
            //            if (curLine == "") continue;
            //            strs.Add(curLine);
            //            Console.WriteLine(curLine);
            //            if (Regex.IsMatch(curLine, @"\s*\bclass\b\s*") && !Regex.IsMatch(curLine, @"\(.*\)"))
            //            {
            //                break ;
            //            }
            //            else if (Regex.IsMatch(curLine, @"\s*\bnamespace\b\s*"))
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    //return;
            //}
            //return;
            
            foreach (var fileUrl in allFiles)
            {
                curFilesCount += 1;
                string result = DealFileContent(fileUrl);

                string newTempFileUrl = fileUrl.Replace(projectUrl, tempDirName);
                var newTempDir = Path.GetDirectoryName(newTempFileUrl);
                if (!Directory.Exists(newTempDir))
                    Directory.CreateDirectory(newTempDir);

                File.Copy(fileUrl, newTempFileUrl);     //备份原文件
                File.WriteAllText(fileUrl, result, Encoding.UTF8);  //结果写入

                Console.WriteLine($"{curFilesCount}/{allFilesCount} {fileUrl.Replace(projectUrl, "")}");
            }

            Console.WriteLine($"已完成！{curFilesCount}/{allFilesCount}");
            Console.WriteLine();
            Console.WriteLine("混淆完至少运行一次项目登录游戏确保混淆没问题，发现有个别错误地方需要手动核对修改！");
            Console.WriteLine($"提交项目查看混淆改动！");
            Console.WriteLine($"备份文件夹在  {tempDirName}");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("请输入任意键退出");
            Console.ReadKey();

            RegexTools.stopwatch.Stop();
        }
        
        static string DealFileContent(string fileUrl)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileUrl);
            fileName += $"_{curFilesCount}";
            CheckCode checkCode = new CheckCode();
            RegexTools.checkCode = checkCode;
            string result = $"#define {fileName}\n";
            using (StreamReader read = File.OpenText(fileUrl))
            {
                string curLine = "";
                while ((curLine = read.ReadLine()) != null)
                {
                    result += curLine + "\n";
                    
                    string tab = RegexTools.GetTab(curLine);    //计算制表符
                    curLine = curLine.Trim();
                    if (Regex.IsMatch(curLine, @"//")) curLine = Regex.Replace(curLine, @"//.*", "");   //去注释       字符中的注释符号可能会被误杀
                    if (curLine == "") continue;
                    if (Regex.IsMatch(curLine, @"^\s*using\s*;$")) continue;            // 跳过 开头的using块 

                    checkCode.checkInIf(curLine);              // if 
                    if (checkCode.lastIFCount > 0) continue;

                    checkCode.checkInInterface(curLine);   //Interface
                    checkCode.checkInStruct(curLine);     //Struct
                    checkCode.checkInClass(curLine);     //Class
                    checkCode.checkInFor(curLine);       //for

                    if (checkCode.isFor) continue;  //当前在for语句定义当中，而不是在循环当中

                    if (Regex.IsMatch(curLine, @"^(\s*).*;\s*$"))   //符合正常语句结束
                    {
                        if (Regex.IsMatch(curLine, @"^\s*return")) continue;     //return
                        if (Regex.IsMatch(curLine, @"^\s*break;")) continue;     //break
                        if (Regex.IsMatch(curLine, @"^\s*(private)?(public)?(protected)?\s*get\s*;\s*$")) continue;     //get
                        if (Regex.IsMatch(curLine, @"^\s*(private)?(public)?(protected)?\s*set\s*;\s*$")) continue;     //set
                        if (Regex.IsMatch(curLine, @"\s*delegate\s*")) continue;     //delegate

                        //Console.WriteLine(curLine);

                        if (checkCode.isStructIng) continue;             //Struct中声明的字段必须在构造方法结束后赋值，无法添加
                        else if (checkCode.isInterfaceIng) continue;    //Interface添加属性或方法后无法判断是否有实现，无法添加
                        else if (checkCode.isInEnumIng) continue;     //Enum中不添加任何东西
                        else if (checkCode.isInAbstractClass) continue;   //抽象类不添加
                        else if (checkCode.isInClassIng)        //在class定义当中
                        {
                            result += $"#if {fileName}\n";
                            result += tab;
                            RegexTools.InsterToClass(ref result);
                        }
                        else if (!checkCode.isInClass) continue; //不在class中，表示在语句外
                        else
                        {
                            result += $"#if {fileName}\n";
                            result += tab;
                            RegexTools.InsterToFun(ref result); //写入方法
                        }
                        result += $"#endif //{fileName}\n";
                    }
                    else
                    {
                        //Random random = new Random(RegexTools.stopwatch.Elapsed.GetHashCode());
                        //if (random.Next(0, 100) > 70)
                        //{
                        //    if (checkCode.isInClassIng)
                        //    {
                        //        result += $"#if {fileName}\n";
                        //        result += tab;              //计算制表符
                        //        RegexTools.InsterToClass(ref result);
                        //        result += $"#endif //{fileName}\n";
                        //    }
                        //}
                    }
                }
            }
            return result;
        }
    }
}