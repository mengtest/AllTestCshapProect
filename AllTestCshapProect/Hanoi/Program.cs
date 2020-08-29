using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Hanoi {
    class Program {
        static void Main (string[] args) {
            //{
            //    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //    stopwatch.Start(); //  开始监视代码运行时间
            //
            //    Console.WriteLine(start(new List<int>() { 1, 2, 3, 4 }));
            //
            //    stopwatch.Stop(); //  停止监视
            //    TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            //    double hours = timespan.TotalHours; // 总小时
            //    double minutes = timespan.TotalMinutes;  // 总分钟
            //    double seconds = timespan.TotalSeconds;  //  总秒数
            //    double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
            //
            //    Console.WriteLine(milliseconds);
            //}
            //
            //{
            //    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //    stopwatch.Start(); //  开始监视代码运行时间
            //    //int[] Nums = new int[] { 1, 2, 3,4,5,6,7,8};//这里可以放你的变量例如Nums = new int[] { a, b, c, d, e };赋值前确保变量有值。
            //    //Permutation(Nums, 0, Nums.Length);
            //    stopwatch.Stop(); //  停止监视
            //    TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            //    double hours = timespan.TotalHours; // 总小时
            //    double minutes = timespan.TotalMinutes;  // 总分钟
            //    double seconds = timespan.TotalSeconds;  //  总秒数
            //    double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
            //
            //    //Console.WriteLine(milliseconds);
            //}


            //MessageBox.Show("111");


            //Console.WriteLine(start(new List<int> (){ 1 , 2 , 3,4,5,6,7,8}));
            //Console.WriteLine(time);
            //Console.ReadKey();
        }

        public static string start (List<string> _arr) {
            List<string> cur = new List<string>();
            List<string> last = new List<string>(_arr);
            return start1(cur , last);
        }

        /// <summary>
        /// 全排序
        /// </summary>
        /// <param name="_cur">当前排列</param>
        /// <param name="_last">剩余数组</param>
        /// <returns></returns>
        public static string start1 (List<string> _cur , List<string> _last) {
            string ret = "";
            List<string> last = new List<string>(_last);
            foreach (string i in _last) {
                _cur.Add(i);
                last.Remove(i);
                ret += start1(_cur , last);
                last.Add(i);
                _cur.Remove(i);
            }
            if (_last.Count == 0) {
                _cur.ForEach((v) => { ret += v.ToString(); });
                ret += "\n";
            }
            return ret;
        }

        //
        //public static void Hanoi(int n, string x, string y, string z)
        //{
        //    //Hanoi(64, "x", "y", "z");
        //    if (n == 1)
        //    {
        //        Console.WriteLine(x + "--->" + z);
        //        time++;
        //    }
        //    else
        //    {
        //        Hanoi(n - 1, x, z, y);
        //        Hanoi(1, x, y, z);
        //        Hanoi(n - 1, y, x, z);
        //    }
        //}


        //public static int[] Nums;
        //static void Main(string[] args)
        //{
        //    Nums = new int[] { 1, 2, 3};//这里可以放你的变量例如Nums = new int[] { a, b, c, d, e };赋值前确保变量有值。
        //    Permutation(Nums, 0, Nums.Length);
        //    Console.ReadKey();
        //}

        /// <summary>
        /// 递归实现全排序并输出
        /// </summary>
        /// <param name="nums">待排序的字符数组</param>
        /// <param name="m">输出字符数组的起始位置</param>
        /// <param name="n">输出字符数组的长度</param>
        public static void Permutation (int[] nums , int m , int n) {
            int i, t;
            if (m < n - 1) {
                Permutation(nums , m + 1 , n);
                for (i = m + 1 ; i < n ; i++) {
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;
                    //
                    Permutation(nums , m + 1 , n);
                    //
                    t = nums[m];
                    nums[m] = nums[i];
                    nums[i] = t;
                }
            } else {
                for (int j = 0 ; j < nums.Length ; j++) {
                    Console.Write(nums[j]);
                }
                Console.WriteLine();
            }
        }
    }
}
