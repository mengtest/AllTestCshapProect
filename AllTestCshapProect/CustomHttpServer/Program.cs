using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace CustomHttpServer
{
    class Program
    {
        static Object o = new object();
        static void Main(string[] args)
        {
            HttpListener listerner = new HttpListener();
            try
            {
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                listerner.Prefixes.Add("http://127.0.0.1:1500/get/");
                listerner.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("服务启动失败..." + ex.Message);
                return;
            }
            Console.WriteLine("服务器启动成功.......");

            Console.WriteLine("\n\n等待客户连接中。。。。");
            while (true)
            {
                //等待请求连接
                //没有请求则GetContext处于阻塞状态
                HttpListenerContext ctx = listerner.GetContext();
                Thread beatHeart = new Thread(TaskProc);
                beatHeart.Start(ctx);
            }
        }

        static void TaskProc(object o)
        {
            HttpListenerContext ctx = (HttpListenerContext)o;

            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码

            //接收Get参数
            string type = ctx.Request.QueryString["type"];
            string userId = ctx.Request.QueryString["userId"];
            string password = ctx.Request.QueryString["password"];
            string filename = Path.GetFileName(ctx.Request.RawUrl);

            //接收POST参数
            Stream stream = ctx.Request.InputStream;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            String body = reader.ReadToEnd();

            ctx.Response.ContentType = "text/html;charset=utf-8";
            using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
            {
                writer.Write("<http><body>Hello world<br/></body></http>");
                writer.Close();
                ctx.Response.Close();
            }
        }
    }
}