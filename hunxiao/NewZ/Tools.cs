using System;
using System.Diagnostics;
using System.Text;

namespace Z
{
	public class Tools {
        public static string[] varModifier = { "", "public" };
        public static string[] varType = { "int", "int", "string","long", "bool", "bool" };
        public static Stopwatch stopwatch = new Stopwatch();
        public static string GetBase64 () {
            TimeSpan timespan = stopwatch.Elapsed;
            string base64 = EncodeBase64(Encoding.UTF8 , timespan.GetHashCode().ToString());
            base64 = base64.Replace("=" , "");
            return base64;
        }

        public static string EncodeBase64 (Encoding encode , string source) {
            string enString = "";
            byte[] bytes = encode.GetBytes(source);
            try {
                enString = Convert.ToBase64String(bytes);
            } catch {
                enString = source;
            }
            return enString;
        }
        public static Random random = null;
        public static int RandomNum (int min , int max) {
            random = new Random(stopwatch.Elapsed.GetHashCode());
            return random.Next(min , max);
        }
    }
}