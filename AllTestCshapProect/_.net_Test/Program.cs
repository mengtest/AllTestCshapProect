using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _.net
{
    class Program
    {
        static void Main (string[] args) {
            int[] arr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var query = from n in arr
                        select new
                        {
                            ID = n,
                            Name = n.ToString()
                        };
      

        }

        #region dynamic 表达式
        ////public static dynamic content => new
        //{
        //    msgtype = "actionCard",
        //    actionCard = new
        //    {
        //        title = "",
        //        text = "",
        //        hideAvatar ="",
        //    },
        //};
        //Console.WriteLine(content.msgtype);

        //dynamic Dc = new {
        //    Name = "BBB" ,
        //    Age = 16 ,
        //    Class = new {
        //        Name = "BBB" ,
        //        Age = 16 ,
        //    }
        //};

        //public static dynamic OutT () {
        //    return new {
        //        Name = "31245" ,
        //        Age = 16 ,
        //        Class = new {
        //            Name = "BBB" ,
        //            Age = 16 ,
        //        } ,
        //    };
        //}
        #endregion
        #region 查询语句

        //int[] arr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //var query = from n in arr
        //            select new
        //            {
        //                ID = n,
        //                Name = n.ToString()
        //            };
        //Console.WriteLine(query);

        #endregion
        #region 方法定义
        //static string b (string a) => a;
        #endregion
        #region lambda表达式
        //public List<int> GetEvenNumber(List<int> list) {
        //    //参数Predicate 为委托，可匿名委托，可lambda表达式
        //    //表示定义一组条件并确定指定对象是否符合这些条件的方法。
        //    return list.FindAll(i => i % 2 == 0);
        //}
        #endregion
    }
}