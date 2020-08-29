using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Filters;
using System.Collections;
using Winista.Text.HtmlParser.Tags;
using System.Text.RegularExpressions;

//gzip 解压缩
//https://www.cnblogs.com/a849788087/p/6563724.html
//html 解析
//https://www.cnblogs.com/sjqq/p/7828521.html
//HtmlParser.Net
//https://www.cnblogs.com/huangcong/p/5142919.html
//https://www.iteye.com/blog/lqy1234567-1948836
//https://www.cnblogs.com/doll-net/archive/2007/06/29/800396.html

//https://www.qisuu.la/Writer/13.html
//http://zhannei.baidu.com/cse/site/?cc=qisuu.la&s=11735927224209550458&q=%E5%94%90%E5%AE%B6%E4%B8%89%E5%B0%91
namespace ParseHtml
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        static void GetBookUrl()
        {
            string url = "https://www.qisuu.la/Shtml24517.html";
            string html = GetHtml(url, true);
            Lexer lexer = new Lexer(html);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(new HasAttributeFilter("class", "showDown"));
            INode ul = htmlNodes[0].Children[1];
            string readUrl = ((ITag)htmlNodes[0].Children[1].Children[1].Children[1]).GetAttribute("href");
            Console.WriteLine(readUrl);

            string js = htmlNodes[0].Children[1].Children[5].Children[1].Children[0].GetText();
            string downloadUrl = Regex.Match(js, @"http.*\.txt").Value;
            Console.WriteLine(downloadUrl);
        }
        static void AuthorBooks()
        {
            string url = "https://www.qisuu.la/Writer/13.html";
            string html = GetHtml(url, true);
            Lexer lexer = new Lexer(html);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(new HasAttributeFilter("class", "listBox"));
            INode node = htmlNodes[0];
            string title = node.Children[1].Children[1].Children[0].GetText();
            Console.WriteLine(title);

            INode ul = node.Children[3];
            for (int i = 1; i < ul.Children.Count; i += 2)
            {
                INode li = ul.Children[i];
                ITag s = (ITag)li.Children[1];
                ITag a = (ITag)li.Children[3];
                ITag u = (ITag)li.Children[5];
                string bookTitle = a.Children[1].GetText();
                string bookImgSrc = ((ITag)a.Children[0]).GetAttribute("src");
                string bookHref = a.GetAttribute("href").Replace("/Shtml", "").Replace(".html", "");
                string bookDesc = u.Children[0].GetText();
                Console.WriteLine(bookTitle);
                Console.WriteLine(bookImgSrc);
                Console.WriteLine(bookHref);
                Console.WriteLine(bookDesc);

                string author = s.Children[0].GetText() + s.Children[1].Children[0].GetText();
                string status = s.Children[3].GetText();
                string size = s.Children[5].GetText();
                string words = s.Children[7].GetText();
                Console.WriteLine(author);
                Console.WriteLine(status);
                Console.WriteLine(size);
                Console.WriteLine(words);
                Console.WriteLine();
            }
        }
        static void SearchBook()
        {
            string query = WebUtility.HtmlEncode("唐家三少");
            string url = $"http://zhannei.baidu.com/cse/site/?cc=qisuu.la&s=11735927224209550458&q=" + query;
            string html = GetHtml(url);
            Lexer lexer = new Lexer(html);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(new HasAttributeFilter("class", "result f s0"));
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                {
                    INode node = htmlNodes[i];
                    INode nodeTitle = node.Children[1];
                    INode nodeATitle = nodeTitle.Children[1];
                    ITag aTag = nodeATitle as ITag;
                    object attribute = aTag.GetAttribute("href");
                    if (attribute != null)
                    {
                        string href = attribute.ToString().Trim();
                        href = Regex.Replace(href, "^http.*//", "");
                        {
                            int idx = href.IndexOf("/?");
                            if (idx != -1) href = href.Substring(0, idx);
                        }
                        {
                            int idx = href.IndexOf("/");
                            if (idx != -1) href = href.Substring(idx + 1);
                        }

                        href = href.Replace("Shtml", "");
                        href = href.Replace(".html", "");
                        if (href.Contains("du"))
                        {
                            href = href.Split('/')[2];
                        }
                        if (href.Contains("Writer"))
                        {
                            href = href.Split('/')[1];
                            Console.WriteLine("作者");
                        }
                        if (href != "")
                        {
                            Console.WriteLine($"href:{href}");
                        }
                    }
                    {
                        string title = "";
                        for (int j = 0; j < nodeATitle.Children.Count; j++)
                        {
                            INode tempNode = nodeATitle.Children[j];
                            if (tempNode is IText)
                            {
                                title += tempNode.GetText().Trim();
                            }
                        }
                        title = title.Trim();
                        if (title != "")
                        {
                            Console.WriteLine($"title:{title}");
                        }
                    }
                    {
                        INode nodeDiv = node.Children[3];
                        ForeachNode(nodeDiv, (_node) =>
                        {
                            ITag tag = _node as ITag;
                            object _class = tag.GetAttribute("class");
                            if (_class != null && _class.ToString() == "c-abstract")
                            {
                                string desc = "";
                                for (int j = 0; j < tag.Children.Count; j++)
                                {
                                    INode tempNode = tag.Children[j];
                                    if (tempNode is IText)
                                    {
                                        desc += tempNode.GetText().Trim();
                                    }
                                }
                                desc = desc.Trim();
                                if (desc != "")
                                {
                                    Console.WriteLine($"desc:{desc}");
                                }
                            }
                        });
                    }

                    Console.WriteLine();
                }

                //ForeachNode(htmlNodes[i], (node) =>
                //{
                //    ITag tag = node as ITag;
                //    object cpos = tag.GetAttribute("cpos");
                //    object href = tag.GetAttribute("href");
                //    object _class = tag.GetAttribute("class");
                //    if (_class != null && (string)_class == "c-abstract")
                //    {
                //        string content = "";
                //        for (int j = 0; j < node.Children.Count; j++)
                //        {
                //            if (node.Children[j] is IText)
                //            {
                //                string str = node.Children[j].GetText();
                //                str = str.Trim();
                //                if (str != "")
                //                {
                //                    content += str + " ";
                //                }
                //            }
                //        }
                //        content = content.Trim();
                //        if (content != "")
                //        {
                //            Console.WriteLine(content);
                //        }
                //    }
                //    if (href != null && (string)cpos == "title")
                //    {
                //        string line = "";
                //        for (int j = 0; j < node.Children.Count; j++)
                //        {
                //            if (node.Children[j] is IText)
                //            {
                //                string str = node.Children[j].GetText();
                //                str = str.Trim();
                //                if (str != "")
                //                {
                //                    line += str + " ";
                //                }
                //            }
                //        }
                //        line = line.Trim();
                //        if (line != "")
                //        {
                //            Console.WriteLine(line);
                //            Console.WriteLine(href);
                //        }
                //    }
                //});
            }
        }
        static void GetMenus()
        {
            string str = GetHtml("https://www.qisuu.la/du/23/23361/", true);
            Lexer lexer = new Lexer(str);
            Parser parser = new Parser(lexer);
            NodeList htmlNodes = parser.Parse(new HasAttributeFilter("class", "pc_list"));
            IList<int> start = new List<int>();

            List<INode> nodes = new List<INode>();
            for (int i = 1; i < htmlNodes.Count; i++)
            {
                ForeachNode(htmlNodes[i], (node) => {
                    ITag tag = node as ITag;
                    object cls = tag.GetAttribute("href");
                    if (cls != null)
                    {
                        nodes.Add(node);
                    }
                });
            }

            foreach (INode node in nodes)
            {
                ITag tag = node as ITag;
                string href = tag.GetAttribute("href");
                IText nextNode = node.Children[0] as IText;
                string title = nextNode.GetText();

                str = GetHtml("https://www.qisuu.la/du/23/23361/" + href, true);
                lexer = new Lexer(str);
                parser = new Parser(lexer);
                htmlNodes = parser.Parse(new HasAttributeFilter("id", "content1"));

                string result = "";
                for (int i = 0; i < htmlNodes[0].Children.Count; i++)
                {
                    INode txt = htmlNodes[0].Children[i] as INode;
                    if (txt is IText)
                    {
                        string line = txt.GetText();
                        line = line.Replace("&nbsp;", "").Trim();
                        if (line != "")
                        {
                            result += line + "\n";
                        }
                    }
                }
                Console.WriteLine(title);
                File.WriteAllText("books/" + title, result);
            }
        }
        public static string GetHtml(string _url,bool _ungzip = false)
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
        static void ForeachNode(INode node, Action<INode> fun)
        {
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    ForeachNode(node.Children[i], fun);
                }
            }

            ITag tag = node as ITag;
            if (tag != null)
            {
                if (fun != null)
                {
                    fun(node);
                }
            }
        }
    }
}
