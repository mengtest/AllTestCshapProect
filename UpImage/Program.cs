using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Reflection;
using LitJson;

namespace UpImage {
    class Program {
        static Program () {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        static List<bool> wait = new List<bool>();
        static string[] fileType = new string[] { "jpg", "png" };
        static List<string> files = new List<string>();
        static int waitCount = 0;

        private static Assembly CurrentDomain_AssemblyResolve (object sender , ResolveEventArgs args) {
            //获取加载失败的程序集的全名
            var assName = new AssemblyName(args.Name).FullName;
            if (args.Name.Contains("LitJson")) {
                //读取资源
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UpImage.LitJson.dll")) {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes , 0 , (int)stream.Length);
                    return Assembly.Load(bytes);//加载资源文件中的dll,代替加载失败的程序集
                }
            }
            throw new DllNotFoundException(assName);
        }
        static void Main (string[] args) {
            if (args.Length > 0) {
                foreach (string item in args) {
                    if (Directory.Exists(item)) {
                        foreach (string fileName in Directory.GetFiles(item)) {
                            string[] re = fileName.Split('.');
                            if (re.Length > 0) {
                                string fType = re[re.Length - 1];
                                foreach (var type in fileType) {
                                    if (fType == type) {
                                        files.Add(fileName);
                                    }
                                }
                            }
                        }
                    } else if (File.Exists(item)) {
                        string[] re = item.Split('.');
                        if (re.Length > 0) {
                            string fType = re[re.Length - 1];
                            foreach (var type in fileType) {
                                if (fType == type) {
                                    files.Add(item);
                                }
                            }
                        }
                    }
                }
                if (files.Count == 0) {
                    Console.WriteLine("所选文件中没有图片，或不支持格式");
                    Console.ReadKey(true);
                    return;
                }
                waitCount = files.Count;
                foreach (object item in files) {
                    wait.Add(true);
                    Thread receiveThread = new Thread(UpFile);
                    receiveThread.Start(item);
                }
                while (wait.Count > 0) { }
                Console.WriteLine("\n\n已完成！");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine("请拖拽文件或文件夹打开");
            Console.ReadKey(true);
            return;
        }

        private static void UpFile (object url) {
            string[] file = ((string)url).Split('\\');
            string name = file[file.Length - 1];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://tinypng.com/web/shrink");
            request.Method = @"POST";
            request.Host = @"tinypng.com";
            request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:61.0) Gecko/20100101 Firefox/61.0";
            request.Accept = @"*/*";
            request.Headers["Accept-Language"] = @"zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2";
            byte[] btBodys = System.IO.File.ReadAllBytes(url.ToString());
            request.ContentType = "image/png";
            request.ContentLength = btBodys.Length;

            Stream newStream = request.GetRequestStream();
            newStream.Write(btBodys , 0 , btBodys.Length);
            newStream.Close();

            string responseContent = "";
            try {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse()) {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) {
                        responseContent = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                    httpWebResponse.Close();
                }
                request.Abort();
            } catch (Exception) {
                Console.WriteLine(name + " 上传失败");
                return;
            }

            JsonData data = JsonMapper.ToObject(responseContent);
            int oldSize = int.Parse(data["input"]["size"].ToJson());
            int newSize = int.Parse(data["output"]["size"].ToJson());
            float ratio = float.Parse(data["output"]["ratio"].ToJson());
            string _url = data["output"]["url"].ToJson();
            _url = _url.Replace("\"" , "");

            Console.WriteLine(string.Format("{0} 上传完毕，原大小{1}，现大小{2}，-{3}% " , name , oldSize , newSize , 100 - 100 * ratio));

            string saveUrl = ((string)url).Replace(name , "");
            if (!Directory.Exists(saveUrl + "new")) {
                Directory.CreateDirectory(saveUrl + "new");
            }
            using (WebClient client = new WebClient()) {
                client.DownloadFileAsync(new Uri(_url) , saveUrl + "new/" + name);
                client.DownloadFileCompleted += (s , es) => {
                    Console.WriteLine(string.Format("{0} 下载完毕！    {1}/{2}" , name , waitCount - wait.Count + 1 , waitCount));
                    wait.RemoveAt(0);
                };
            }
        }
    }
}