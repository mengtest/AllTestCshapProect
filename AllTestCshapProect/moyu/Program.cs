using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace moyu
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ch = Regex.Split(File.ReadAllText(@"D:\8823.txt"), "------------");
            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < ch.Length; i++)
            {
                List<string> ret = new List<string>();
                foreach (var item in ch[i].Split('\n'))
                {
                    if (item.Trim() != "")
                    {
                        ret.Add(item);
                    }
                }
                if (ret.Count > 0)
                {
                    list.Add(ret);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i][0]);
            }
        }

        public static string Read(string path)
        {
            string result = "";
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            //Console.WriteLine(line);
                            result += line.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
