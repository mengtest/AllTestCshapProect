using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexTest
{
    class RegexTools
    {
        public static CheckCode checkCode = null;
        public static Stopwatch stopwatch = new Stopwatch();
        public static Random random;
        public static int randomIdx = 0;
        
        /// <summary>
        /// 制表符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetTab(string str)
        {
            string result = "";
            foreach (char item in str)
                if (item == 32 || item == 9)
                    result += item;      //计算制表符
                else
                    break;
            return result;
        }

        /// <summary>
        /// 时间戳base64变量名
        /// </summary>
        /// <returns></returns>
        public static string GetBase64()
        {
            TimeSpan timespan = stopwatch.Elapsed;
            string base64 = EncodeBase64(Encoding.UTF8, timespan.GetHashCode().ToString());
            base64 = base64.Replace("=", "");
            return base64;
        }

        /// <summary>
        /// 插入到类中的定义
        /// </summary>
        /// <param name="result"></param>
        public static void InsterToClass(ref string result)
        {
            string base64 = GetBase64();
            Func<string>[] action = new Func<string>[] {
                RandomDefineAttribute,  //随机属性
                RandomDefineFun ,       //随机方法定义
                RandomDefineAttribute,  //随机属性
                RandomDefineFun ,       //随机方法定义
                RandomDefineVariable,
            }; //随机变量
            random = new Random(stopwatch.Elapsed.GetHashCode());
            randomIdx = random.Next(0, action.Length);
            result += action[randomIdx]() + "\n";
        }

        /// <summary>
        /// 插入到方法中的语句
        /// </summary>
        /// <param name="result"></param>
        public static void InsterToFun(ref string result)
        {
            string base64 = GetBase64();
            Func<string>[] action = new Func<string>[] {
                RandomDefineVariable,
                RandomFor,
                RandomIf
            };
            random = new Random(stopwatch.Elapsed.GetHashCode());
            randomIdx = random.Next(0, action.Length);
            result += action[randomIdx]() + "\n";
        }

        /// <summary>
        /// 随机定义方法
        /// </summary>
        /// <returns></returns>
        public static string RandomDefineFun()
        {
            string[] types = new string[] {
                "string",
                "int",
                "void"
            };
            string[] modifiers = new string[] {
                "public",
                "private",
                "",
                "public",
                "private",
                "",
            };

            random = new Random(stopwatch.Elapsed.GetHashCode());
            randomIdx = random.Next(0, types.Length - 1);
            string t = types[randomIdx];

            random = new Random(stopwatch.Elapsed.GetHashCode());
            randomIdx = random.Next(0, modifiers.Length - 1);
            string m = modifiers[randomIdx];

            string result = "";
            if (checkCode.isInClassIng && checkCode.isInStaticClass)
                result += "static ";
            if (m != "")
                result += m + " ";

            result += t + " ";
            result += GetBase64() + "() {";
            switch (t)
            {
                case "string":
                    result += $"return \"{RandomString()}\";";
                    break;
                case "int":
                    result += $"return {RandomInt()};";
                    break;
                default:
                    break;
            }
            result += "}\n";
            return result;
        }

        /// <summary>
        /// 随机变量
        /// </summary>
        /// <returns></returns>
        public static string RandomDefineVariable()
        {
            string[] types = new string[] {
                "string",
                "int"
            };

            random = new Random(stopwatch.Elapsed.GetHashCode());
            randomIdx = random.Next(0, types.Length - 1);
            string t = types[randomIdx];

            string result = "";
            if (checkCode.isInClassIng && checkCode.isInStaticClass)
                result += "static ";
            string varName = GetBase64();
            result += $"{t} {varName} = ";
            switch (t)
            {
                case "string":
                    result += "\"" + RandomString() + "\"";
                    break;
                case "int":
                    result += RandomInt();
                    break;
                default:
                    break;
            }
            result += "; ";
            if (!checkCode.isInClassIng)
                switch (t)
                {
                    case "string":
                        result += varName + " = \"\"";
                        break;
                    case "int":
                        result += varName + @" = 0";
                        break;
                    default:
                        break;
                }
                result += ";";
            return result;
        }

        /// <summary>
        /// 随机get;set;属性
        /// </summary>
        /// <returns></returns>
        public static string RandomDefineAttribute()
        {
            string[] types = new string[] {
                "string",
                "int",
                "string",
                "int",
            };
            string[] modifiers = new string[] {
                "public",
                "private",
                "",
                "public",
                "private",
                ""
            };
            string[] get_modifiers = new string[] {
                "null",
                ""
            };
            string varName = GetBase64();
            string result = "";

            string get = " get;";
            string set = " set;";

            string varType = "";
            {
                random = new Random(stopwatch.Elapsed.GetHashCode());
                randomIdx = random.Next(0, types.Length - 1);
                varType = types[randomIdx];
            }

            string varModifier = "";
            {
                random = new Random(stopwatch.Elapsed.GetHashCode());
                randomIdx = random.Next(0, modifiers.Length - 1);
                varModifier = modifiers[randomIdx];
            }

            if (checkCode.isInClassIng && checkCode.isInStaticClass)
                result += "static ";
            if (varModifier != "")
                result += varModifier + " ";
            result += varType + " " + varName;

            result += " {";
            if (get != "")
                result += get;
            if (set != "")
                result += set;
            result += " }";

            //Console.WriteLine("result ==" + result);
            return result;
        }

        /// <summary>
        /// 随机for
        /// </summary>
        /// <returns></returns>
        public static string RandomFor()
        {
            string result = "";
            string iVar = GetBase64();
            random = new Random(stopwatch.Elapsed.GetHashCode());
            result += $"for (int {iVar} = 0; {iVar} < 0; {iVar}++) {{ {RandomDefineVariable()} }}";
            return result;
        }

        /// <summary>
        /// 随机if
        /// </summary>
        /// <returns></returns>
        public static string RandomIf()
        {
            string result = "";
            string iVar = GetBase64();
            random = new Random(stopwatch.Elapsed.GetHashCode());

            result += $"bool {iVar} = false;if ({iVar}){{ {RandomDefineVariable()} ";

            int iidx = random.Next(1, 5);
            if (iidx > 1)
                for (int i = 1; i < iidx; i++)
                {
                    string v = GetBase64();
                    result += $"bool {v} = false;if ({v}){{ {RandomDefineVariable()} ";
                }
                for (int i = 1; i < iidx; i++)
                    result += $"}}";
            result += $"}}";

            //Console.WriteLine("RandomIf =="+result);
            return result;
        }

        /// <summary>
        /// 随机int
        /// </summary>
        /// <returns></returns>
        public static int RandomInt()
        {
            random = new Random(stopwatch.Elapsed.GetHashCode());
            return random.Next();
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="isUseNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="isUseLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="isUseUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="isUseSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string RandomString(int length = 0, bool isUseNum = true, bool isUseLow = true, bool isUseUpp = true, bool isUseSpe = true, string custom = "")
        {
            random = new Random(stopwatch.Elapsed.GetHashCode());
            if (length == 0) length = random.Next(1, 30);
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            random = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (isUseNum) 
                str += "0123456789"; 
            if (isUseLow) 
                str += "abcdefghijklmnopqrstuvwxyz"; 
            if (isUseUpp) 
                str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
            if (isUseSpe) 
                str += "!#$%&'()*+,-./:;<=>?@[]^_`{|}~"; 
            for (int i = 0; i < length; i++)
                s += str.Substring(random.Next(0, str.Length - 1), 1);
            return s;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encode">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
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
    }
}
