using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Backgrounds
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchKey = @"300 英雄";
            searchKey = WebUtility.UrlEncode(searchKey);
            string url = @"http://image.baidu.com/search/acjson?";
            Dictionary<string, string> keyMap = new Dictionary<string, string>();
            keyMap["tn"] = "resultjson_com";
            keyMap["ipn"] = "rj";
            keyMap["ct"] = "201326592";
            keyMap["is"] = "";
            keyMap["fp"] = "result";
            keyMap["queryWord"] = searchKey;
            keyMap["cl"] = "2";
            keyMap["lm"] = "-1";
            keyMap["ie"] = "utf-8";
            keyMap["oe"] = "utf-8";
            keyMap["adpicid"] = "";
            keyMap["st"] = "-1";
            keyMap["z"] = "";
            keyMap["ic"] = "0";
            keyMap["hd"] = "0";
            keyMap["latest"] = "0";
            keyMap["copyright"] = "0";
            keyMap["word"] = searchKey;
            keyMap["s"] = "";
            keyMap["se"] = "";
            keyMap["tab"] = "";
            keyMap["width"] = "1680";
            keyMap["height"] = "1050";
            keyMap["face"] = "0";
            keyMap["istype"] = "2";
            keyMap["qc"] = "";
            keyMap["nc"] = "";
            keyMap["fr"] = "";
            keyMap["expermode"] = "";
            keyMap["force"] = "";
            keyMap["pn"] = "30";
            keyMap["rn"] = "30";
            keyMap["gsm"] = "";

            foreach (var key in keyMap.Keys)
            {
                url += key + "=" + keyMap[key] + "&";
            }
            Console.WriteLine(url);

            //SetWall(@"D:/用户目录/下载/1a7013f94e5be3acd1d2d05e84cadd20.jpeg");
            //File.WriteAllText("1.json", GetHtml(url));
            //{
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            //myWebClient.DownloadFileAsync(new Uri("https://w.wallhaven.cc/full/39/wallhaven-395mgv.png"), "wallhaven-395mgv.png");
            string ret = myWebClient.DownloadString("https://wallhaven.cc/search?q=%E6%98%8E%E6%97%A5%E6%96%B9%E8%88%9F&categories=111&purity=100&sorting=relevance&order=desc&page=6");
            File.WriteAllText("1.txt", ret);
            return;
            myWebClient.DownloadFileCompleted += (s,e) => {
                //Console.WriteLine(e.UserState);
                //Console.WriteLine(e.Error.Message);
                //Console.WriteLine(e.Cancelled);
                Console.WriteLine("ok");
            };
            while (true)
            {

            }
        }

        public static string GetHtml(string _url, bool _ungzip = false)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            //request.Headers["Accept"] = 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stm = response.GetResponseStream();

            string strHTML = "";
            if (_ungzip)
            {
                stm = new GZipStream(stm, CompressionMode.Decompress);//解压缩
            }
            using (StreamReader reader = new StreamReader(stm, Encoding.GetEncoding("utf-8")))//中文编码处理
            {
                strHTML = reader.ReadToEnd();
            }
            return strHTML;
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo( int uAction, int uParam,  string lpvParam, int fuWinIni  );

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="url">壁纸物理路径</param>
        public static void SetWall(string url)
        {
            Console.WriteLine("emmmm");
            SystemParametersInfo(20, 0, url, 1);
        }
    }
}