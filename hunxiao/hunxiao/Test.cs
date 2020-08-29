using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class RegexTime
    {
        public static Stopwatch stopwatch = new Stopwatch();
        public static Random random;
        public static void InitStart()
        {
            stopwatch.Start();
        }
        public static void OverEnd()
        {
            stopwatch.Stop();
        }

        public static int GetMemoryRandomNum(int minNum, int maxNum)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            random = new Random(BitConverter.ToInt32(b, 0));
            return random.Next(minNum, maxNum);
        }
        public static int GetMemoryRandomNum(int maxNum)
        {
            return GetMemoryRandomNum(0, maxNum);
        }
        public static int GetTimeRandomNum(int minNum, int maxNum)
        {
            random = new Random(GetRandomTimeSeed);
            return random.Next(minNum, maxNum);
        }
        public static int GetTimeRandomNum(int maxNum)
        {
            return GetTimeRandomNum(0, maxNum);
        }

        public static int GetRandomTimeSeed
        {
            get {
                return stopwatch.Elapsed.GetHashCode();
            }
        }
        public static string GetBase64()
        {
            string base64 = EncodeBase64(Encoding.UTF8, GetRandomTimeSeed.ToString());
            base64 = base64.Replace("=", "");
            return base64;
        }
        public static string EncodeBase64(Encoding encode, string source)
        {
            string enString = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                enString = Convert.ToBase64String(bytes);
            }
            catch
            {
                enString = source;
            }
            return enString;
        }
        public static string RandomString(int length = 0, bool isUseNum = true, bool isUseLow = true, bool isUseUpp = true, bool isUseSpe = true, string custom = "")
        {
            if (length == 0)
            {
                length = GetTimeRandomNum(1, 30);
            }
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            random = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (isUseNum == true) { str += "0123456789"; }
            if (isUseLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (isUseUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (isUseSpe == true) { str += "!#$%&'()*+,-./:;<=>?@[]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(random.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }

    public class Node
    {
        public static List<string> Modifys = new List<string>() { "", "public"};
        public string Name { get; set; }
        public string Modify { get; set; }
        public Node nextNode { get; set; }
        public Node lastNode { get; set; }
        public List<Node> ChildNodes { get; set; }
        public void RandomModify() {

        }
        public void RandomName() {
            Name = RegexTime.GetBase64();
        }
        public virtual string Generate() {
            RegexClass node = lastNode as RegexClass;
            if (node.IsStatic)
            {
                return "static ";
            }
            return "";
        }
    }
    public class RegexNameSpace : Node
    {
        List<string> Names { get; set; }
    }
    public class RegexClass : Node
    {
        public bool IsStatic { get; set; }  //是否是静态
    }
    public class RegexVariable : Node
    {
        string VarValue { get; set; }
    }
    public class RegexIf : Node
    {

    }
    public class RegexFor : Node
    {
        int StartCount { get; set; }    //开始的值
        int EndCount { get; set; }      //结束的值
        string JudgeIf { get; set; }    //判断条件
    }
    public class RegexAttribute : Node
    {
        public override string Generate() {
            base.Generate();
            return "";
        }
    }
}