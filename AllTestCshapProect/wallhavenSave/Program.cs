using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Filters;

namespace wallhavenSave {
    class Program {
        static bool isOk = false;
        static void Main (string[] args) {
            //WebClient myWebClient = new WebClient();
            //myWebClient.DownloadFileAsync(new Uri("https://w.wallhaven.cc/full/39/wallhaven-395mgv.png"), "wallhaven-395mgv.png");
            //string ret = myWebClient.DownloadString("https://wallhaven.cc/search?q=%E6%98%8E%E6%97%A5%E6%96%B9%E8%88%9F&categories=111&purity=100&sorting=relevance&order=desc&page=6");

            SaveImgs(GetUrls());
            //GetUrls();
            //return;
            //SaveImgs(new List<string>()
            //{
            //    "https://w.wallhaven.cc/full/39/wallhaven-395mgv.png",
            //    "https://w.wallhaven.cc/full/96/wallhaven-96rwqx.jpg",
            //});

            while (!isOk) { }
            Console.WriteLine("download ok!");
        }

        static List<string> GetUrls () {
            List<string> ret = new List<string>();
            string url = "https://wallhaven.cc/search?";
            Dictionary<string , string> dic = new Dictionary<string , string>() {
                {"q", "明日方舟"},
                {"categories", "111"},
                {"purity", "100"},
                {"resolutions", "1280x800,1600x1000,1920x1200,2560x1600,3840x2400"},
                {"sorting", "relevance"},
                {"order", "desc" },
            };
            foreach (var key in dic.Keys) {
                url += $"{key}={WebUtility.UrlEncode(dic[key])}&";
            }

            WebClient myWebClient = new WebClient();
            string html = myWebClient.DownloadString(url);
            Lexer lexer = new Lexer(html);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(new HasAttributeFilter("class" , "lazyload"));

            for (int i = 0 ; i < htmlNodes.Count ; i++) {
                INode node = htmlNodes[i];
                ITag tag = (ITag)node;
                string src = tag.GetAttribute("data-src");
                ITag png = (ITag)(node.Parent.Children[3].Children[2]);
                string val = png.GetAttribute("class");
                val = val == "png" ? val : "jpg";

                src = src.Replace(".jpg" , $".{val}");
                src = src.Replace("https://th.wallhaven.cc/small/" , "https://w.wallhaven.cc/full/");
                string[] arr = src.Split('/');
                string str = $"wallhaven-{src.Substring(src.LastIndexOf('/') + 1)}";
                src = src.Replace(arr[arr.Length - 1] , str);
                Console.WriteLine(src);
                ret.Add(src);
            }
            return ret;
        }

        static List<WebClient> webList = new List<WebClient>();
        static void SaveImgs (List<string> _list) {
            string dir = "images/";
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            Console.WriteLine($"allUrl {_list.Count}");
            List<string> allList = new List<string>(_list);
            List<string> waitList = new List<string>();
            List<string> okList = new List<string>();
            while (allList.Count > 0) {
                if (waitList.Count < 5) {
                    string url = allList[0];
                    allList.RemoveAt(0);
                    string fileName = Path.GetFileName(url);
                    string newFileUrl = dir + fileName;
                    if (File.Exists(newFileUrl)) {
                        okList.Add(fileName);
                        Console.WriteLine($"over {fileName} {okList.Count}/{_list.Count}");
                    } else {
                        waitList.Add(fileName);
                        WebClient myWebClient = new WebClient();
                        webList.Add(myWebClient);
                        Console.WriteLine($"start {fileName} {allList.Count}/{_list.Count}");
                        myWebClient.DownloadFileAsync(new Uri(url) , newFileUrl , new object[] { fileName });
                        myWebClient.DownloadFileCompleted += (s , e) => {
                            object[] obj = e.UserState as object[];
                            string fName = obj[0] as string;
                            waitList.Remove(fName);
                            okList.Add(fName);
                            Console.WriteLine($"over {fName} {okList.Count}/{_list.Count}");
                            if (okList.Count == _list.Count) {
                                isOk = true;
                            }
                        };
                    }
                }
            }
        }
    }
}
