using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Z_CShapCore {
    public class Tools {
        #region Unicode转字符串
        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        public static string Unicode2String (string source) {
            return new Regex(@"\\u([0-9A-F]{4})" , RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source , x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1") , 16)));
        }
        #endregion
        #region 保存文件
        public static void SaveFile (string path , string _txet) {
            FileStream fs = new FileStream(path , FileMode.Create);
            StreamWriter sw = new StreamWriter(fs , Encoding.UTF8);
            sw.Write(_txet);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static string ReadFile (string path) {
            string result = "";
            if (File.Exists(path)) {
                StreamReader sr = new StreamReader(path , Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null) {
                    result += line.ToString();
                }
            }
            return result;
        }
        #endregion
        #region 下载
        public static bool HttpDownload (string _url , string _saveFilePath) {
            try {
                HttpWebRequest request = WebRequest.Create(_url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr , 0 , (int)bArr.Length);
                FileStream fs = new FileStream(_saveFilePath , FileMode.Append , FileAccess.Write , FileShare.ReadWrite);
                while (size > 0) {
                    fs.Write(bArr , 0 , size);
                    size = responseStream.Read(bArr , 0 , (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
                Console.WriteLine("下载完毕 " + _saveFilePath);
                return true;
            } catch (Exception ex) {
                Console.WriteLine(_saveFilePath + "Error" + ex.Message);
                return false;
            }
        }
        public static void WebDownLoadFile (string addressUrl , string fileName = "") {
            if (fileName == "") {
                string[] split = addressUrl.Split('/');
                fileName = split[split.Length - 1];
            }
            bool isOk = false;

            //下载文件
            WebClient myWebClient = new System.Net.WebClient();
            myWebClient.DownloadProgressChanged += (o , e) => {
                if (!isOk) {
                    int allBytes = (int)e.TotalBytesToReceive;
                    int curBytes = (int)e.BytesReceived;
                    isOk = e.ProgressPercentage == 100;
                    Console.WriteLine(e.ProgressPercentage + "%" + " " + curBytes + "/" + allBytes);
                }
            };
            myWebClient.DownloadFileCompleted += (o , e) => {
                isOk = true;
                Process pro = new Process();
                pro.StartInfo.FileName = fileName;
                pro.Start();
                Console.WriteLine(fileName);
            };
            myWebClient.DownloadFileAsync(new Uri(addressUrl) , fileName);
            while (!isOk) { }
            Console.WriteLine("下载完成");
        }
        #endregion

        /// <summary>
        /// 返回文件MD5码
        /// </summary>
        /// <param name="_fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile (string _fileName) {
            try {
                if (File.Exists(_fileName)) {
                    FileStream file = new FileStream(_fileName , FileMode.Open);
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0 ; i < retVal.Length ; i++) {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
                return "";
            } catch (Exception ex) {
                Console.WriteLine("GetMD5HashFromFile() fail,error:" + ex.Message);
                return "";
            }
        }
    }
}