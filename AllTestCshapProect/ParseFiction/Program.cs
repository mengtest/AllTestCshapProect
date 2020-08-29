using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//解析html
//https://www.cnblogs.com/sjqq/p/7828521.html

namespace ParseFiction
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = G("https://www.qisuu.la/du/23/23361/");
            File.WriteAllText("1.txt", s);
        }

        public static string G(string _url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            //request.Headers["Accept"] = 

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stm = response.GetResponseStream();

            string strHTML = "";
            GZipStream gzip = new GZipStream(stm, CompressionMode.Decompress);//解压缩
            using (StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8")))//中文编码处理
            {
                strHTML = reader.ReadToEnd();
            }
            return strHTML;
        }
    }
}
