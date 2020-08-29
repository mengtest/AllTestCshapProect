using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTo2020 {
    class Program {
        static void Main (string[] args) {

        }

        public class ExecutionResult {
            public bool Status {get;set; }
            public string Message { get;set; }
            public string Anything { get;set; }
        }

        /// <summary>
        /// 压缩文件到zip
        /// </summary>
        /// <param name="filePath">来源文件路径</param>
        /// <param name="zipPath">新zip文件</param>
        /// <returns></returns>
        public ExecutionResult CompressFile (string filePath , string zipPath)//运行DOS命令
        {
            ExecutionResult exeRes = new ExecutionResult();
            exeRes.Status = true;
            try {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                string message = "";
                string command1 = "c:";
                string command2 = @"cd\";
                string command3 = @"cd C:\Progra~1\7-Zip";
                string command4 = "";


                command4 = "7Z a -tzip " + zipPath + "  " + filePath;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine(command1);
                process.StandardInput.WriteLine(command2);
                process.StandardInput.WriteLine(command3);
                process.StandardInput.WriteLine(command4);
                process.StandardInput.WriteLine("exit");
                message = process.StandardOutput.ReadToEnd(); //要等压缩完成后才可以来抓取这个压缩文件

                process.Close();
                if (!message.Contains("Everything is Ok")) {
                    exeRes.Status = false;
                    exeRes.Message = message;
                } else {
                    exeRes.Anything = zipPath;
                }
            } catch (Exception ex) {
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }

        /// <summary>
        /// 解压缩zip文件
        /// </summary>
        /// <param name="zipPath">zip文件</param>
        /// <param name="filePath">路径文件</param>
        /// <returns></returns>
        public ExecutionResult DeCompressFile (string zipPath , string filePath)//运行DOS命令
        {
            ExecutionResult exeRes = new ExecutionResult();
            exeRes.Status = true;
            try {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                string message = "";
                string command1 = "c:";
                string command2 = @"cd\";
                string command3 = @"cd C:\Progra~1\7-Zip";
                string command4 = "";


                command4 = "7Z x -tzip " + zipPath + " -o" + filePath + " -y";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine(command1);
                process.StandardInput.WriteLine(command2);
                process.StandardInput.WriteLine(command3);
                process.StandardInput.WriteLine(command4);
                process.StandardInput.WriteLine("exit");
                //process.WaitForExit();
                message = process.StandardOutput.ReadToEnd();//要等压缩完成后才可以来抓取这个压缩文件

                process.Close();
                if (!message.Contains("Everything is Ok")) {
                    exeRes.Status = false;
                    exeRes.Message = message;
                } else {
                    exeRes.Anything = filePath;
                }
            } catch (Exception ex) {
                exeRes.Message = ex.Message;
            }

            return exeRes;
        }
    }
}
