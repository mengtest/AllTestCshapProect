using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    class Program {
        public class User : IEnumerable<User> {
            public IEnumerator<User> GetEnumerator () {
                return new Usertor();
            }

            IEnumerator IEnumerable.GetEnumerator () {
                throw new NotImplementedException();
            }
        }

        public class Usertor : IEnumerator<User> {
            public User Current {
                get {
                    throw new NotImplementedException();
                }
            }

            object IEnumerator.Current {
                get {
                    throw new NotImplementedException();
                }
            }

            public void Dispose () {
                throw new NotImplementedException();
            }

            public bool MoveNext () {
                throw new NotImplementedException();
            }

            public void Reset () {
                throw new NotImplementedException();
            }
        }


        public static void Main () {

            Console.WriteLine("请输入一个数字");
            int num = Convert.ToInt32(Console.ReadLine());
            int sum = num * 2 - 1;
            for (int i = 1 ; i < num + 1 ; i++) {
                string x = new string(' ' , sum - i);
                string s = new string('*' , i * 2 - 1);
                Console.WriteLine(x + s);
            }
            for (int i = num + 1 ; i > 0 ; i--) {
                string x = new string(' ' , sum - i <= 0 ? sum : sum - i);
                string s = new string('*' , i * 2 - 1);
                Console.WriteLine(x + s);
            }
            Console.ReadKey();





            //Tree<int> t = new Tree<int> (1); //二叉树初始节点
            //t.Insert (2);
            //t.Insert (-1);
            //t.Insert (3);
            //t.Insert (-3);
            //t.Insert (-2);
            //Tree<int> t = new Tree<int> (100); //二叉树初始节点
            //t.Insert(25);
            //t.Insert(125);
            //t.Insert(99);
            //t.Insert(50);
            //t.Insert(70);
            //t.Insert(160);
            //t.Insert(115);
            //t.WalkTree (); //二叉树的遍历
            // Console.WriteLine ("elll");

            //BinarySearchTree nums = new BinarySearchTree ();
            //nums.Insert (100);
            //nums.Insert (25);
            //nums.Insert (125);
            //nums.Insert (99);
            //nums.Insert (50);
            //nums.Insert (70);
            //nums.Insert (160);
            //nums.Insert (115);
            //Console.WriteLine ("--------中序遍历-----------");
            //nums.InOrder (nums.root);
            //Console.WriteLine ();
            //Console.WriteLine ("--------查找最小值---------");
            //Console.WriteLine (nums.FindMin ());
            //Console.WriteLine ("--------查找最大值---------");
            //Console.WriteLine (nums.FindMax ());
            //nums.Delete (99);
            //Console.WriteLine ("--------删除结点后-------");
            //nums.InOrder (nums.root);
            //Console.ReadLine ();


        }
    }
}