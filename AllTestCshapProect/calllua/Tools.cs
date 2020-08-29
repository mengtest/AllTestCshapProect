using LuaInterface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Calllua {
    public class TBuffer {
        Stream orgStream;
        Stream desStream;
        public TBuffer (Stream _orgStream , Stream _desStrea) {
            orgStream = _orgStream;
            desStream = _desStrea;
        }

        byte[] buffer = new byte[1024 * 80];

        void OnRead (IAsyncResult asr) {
            try {
                if (asr.IsCompleted) {
                    if (orgStream.CanRead) {
                        int read = orgStream.EndRead(asr);
                        if (read > 0) {
                            if (desStream.CanWrite) {
                                desStream.Write(buffer , 0 , read);
                                Array.Clear(buffer , 0 , buffer.Length);      //清空数组
                                orgStream.BeginRead(buffer , 0 , buffer.Length , new AsyncCallback(OnRead) , null);
                            }
                        } else {

                        }
                    }
                }
            } catch (Exception ex) {
            }
        }
        HttpListenerContext ctx = null;
        public void Copy (HttpListenerContext _ctx = null) {
            ctx = _ctx;
            try {
                int offset = 0;
                if (ctx != null) {
                    ctx.Response.ContentLength64 = orgStream.Length;
                    NameValueCollection headers = ctx.Request.Headers;
                    if (headers != null) {
                        //foreach (string item in ctx.Request.Headers) Console.WriteLine("Request ==>" + item + ":" + ctx.Request.Headers[item]);
                        string range = headers.Get("Range");
                        if (range != null && range != "") {
                            range = range.Trim();
                            string[] nums = range.Split('=')[1].Split('-');
                            ctx.Response.StatusCode = (int)HttpStatusCode.PartialContent;
                            ctx.Response.Headers.Add("Content-Range" , nums[0] + "-" + nums[1] + "/" + orgStream.Length);
                            ctx.Response.Headers.Add("Accept-Ranges" , "bytes");

                            ctx.Response.ContentLength64 = int.Parse(nums[1]) - int.Parse(nums[0]);
                            offset = int.Parse(nums[0]);
                            orgStream.Seek(offset , SeekOrigin.Begin);
                            orgStream.Seek(orgStream.Length - int.Parse(nums[1]) , SeekOrigin.End);
                        }
                    }
                    //Console.WriteLine("offset ==" + offset);
                    //Console.WriteLine("ctx.Response.StatusCode ==" + ctx.Response.StatusCode);
                    //Console.WriteLine("ctx.Response.ContentLength64 ==" + ctx.Response.ContentLength64);
                    //foreach (string item in ctx.Response.Headers) Console.WriteLine("Response ==>" + item + ":" + ctx.Response.Headers[item]);
                    //Console.WriteLine();
                    //Console.WriteLine();
                }
                orgStream.BeginRead(buffer , 0 , buffer.Length , new AsyncCallback(OnRead) , null);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Tools {
        private Tools () { }
        private static Tools tools;
        public static Tools Self {
            get {
                if (tools == null) {
                    tools = new Tools();
                }
                return tools;
            }
        }

        public Lua lua = new Lua();
        public List<string> listenerUrls = new List<string>();
        public delegate void HttpListenerCallBack (HttpListenerContext ct);
        public event HttpListenerCallBack httpListenerCallBack;

        public void AddListener (HttpListenerCallBack _callback) {
            this.httpListenerCallBack += _callback;
        }
        public void RemoveListener (HttpListenerCallBack _callback) {
            this.httpListenerCallBack -= _callback;
        }

        public HttpListener listerner = new HttpListener();
        public void StartServer () {
            try {
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                foreach (var item in listenerUrls) { 
                    listerner.Prefixes.Add(item);
                }
                listerner.Start();
                listerner.BeginGetContext(new AsyncCallback(GetContextCallBack) , listerner);
            } catch (Exception ex) {
                Console.WriteLine("server start error..." + ex.Message);
                return;
            }
        }
        public void GetContextCallBack (IAsyncResult ar) {
            try {
                lock (listerner) {
                    listerner = ar.AsyncState as HttpListener;
                    HttpListenerContext context = listerner.EndGetContext(ar);
                    if (httpListenerCallBack != null) {
                        httpListenerCallBack(context);
                    }
                    listerner.BeginGetContext(new AsyncCallback(GetContextCallBack) , listerner);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetLocalIP () {
            try {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0 ; i < IpEntry.AddressList.Length ; i++) {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}