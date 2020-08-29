using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    public class Tree<T> where T : IComparable<T> {
        //继承的这个接口用于通用比较
        private T NodeData; //节点
        private Tree<T> LeftTree; //左孩子
        private Tree<T> RightTree; //右孩子
        public Tree (T nodeValue) {
            this.NodeData = nodeValue; //泛型数据
            this.LeftTree = null; //左孩子
            this.RightTree = null; //右孩子
        }

        public void Insert (T newItem) //树的插入操作实现二叉排序树
        {
            T currentNodeValue = this.NodeData;
            if (currentNodeValue.CompareTo(newItem) > 0) {
                if (this.LeftTree == null) {
                    this.LeftTree = new Tree<T>(newItem);

                } else {
                    this.LeftTree.Insert(newItem);
                }

            } else {
                if (this.RightTree == null) {
                    this.RightTree = new Tree<T>(newItem);
                } else {
                    this.RightTree.Insert(newItem);
                }
            }
        }

        //以下执行左中右的遍历方式

        public void WalkTree () //树的遍历
        {
            if (this.LeftTree != null) {
                this.LeftTree.WalkTree();
            }

            Console.WriteLine(this.NodeData.ToString());

            if (this.RightTree != null) {
                this.RightTree.WalkTree();
            }
        }
    }
}
