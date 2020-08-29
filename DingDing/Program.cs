using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DingDing
{
    public class Message
    {
        public string msgtype { get; set; }

        public text text { get; set; } = new text();

        public at at { get; set; } = new at();
    }
    public class at
    {
        public List<string> atMobiles { get; set; } = new List<string>();
        public bool isAtAll { get; set; }
    }
    public class text
    {
        public string content { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var list = args.ToList();
            //args = new string[]
            //{
            //    "1",".",
            //    "2",".",
            //    "3",".",
            //    "4",".",
            //    "5",".",
            //    "6",".",
            //    "7",".",
            //};
            //string content = "";
            //for (int i = 0; i < args.Length; i+=2)
            //{
            //    if (i + 1 < args.Length)
            //    {
            //        content += $"> {args[i]}：{args[i + 1]} \n";
            //    }
            //}
            if (list.Count > 0) {
                Console.WriteLine(1);
                string content = list[1] + "文本内容";
                string data = "{\"msgtype\": \"text\",\"text\": {\"content\": \" " + content + "\"}}";
                Send(list[0] , data);
            }
        }
        
        static void Send(string url,string data)
        {
            //发送请求
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            var byteData = Encoding.UTF8.GetBytes(data);
            var length = byteData.Length;
            request.ContentLength = length;
            var writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();

            request.GetResponse();
            //接收数据
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            
            Console.WriteLine(responseString);
            //Tools.Tools.SaveString("C:/5.txt", responseString);
        }
    }
}