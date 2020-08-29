using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    public class BinarySearchTree {
        public Node root;
        public BinarySearchTree () {
            root = null;
        }
        //插入元素
        public void Insert (int i) {
            Node newNode = new Node();
            newNode.Data = i;
            if (root == null) {
                root = newNode;
            } else {
                Node current = root;
                Node parent;
                while (true) {
                    parent = current;
                    if (i < current.Data) {
                        current = current.Left;
                        if (current == null) {
                            parent.Left = newNode;
                            break;
                        }
                    } else {
                        current = current.Right;
                        if (current == null) {
                            parent.Right = newNode;
                            break;
                        }
                    }
                }
            }
        }
        //中序遍历
        public void InOrder (Node theRoot) {
            if (!(theRoot == null)) {
                InOrder(theRoot.Left);
                theRoot.DisplayNode();
                InOrder(theRoot.Right);
            }
        }
        //先序遍历
        public void PreOrder (Node theRoot) {
            if (!(theRoot == null)) {
                theRoot.DisplayNode();
                PreOrder(theRoot.Left);
                PreOrder(theRoot.Right);
            }
        }
        //后序遍历
        public void PostOrder (Node theRoot) {
            if (!(theRoot == null)) {
                PostOrder(theRoot.Left);
                PostOrder(theRoot.Right);
                theRoot.DisplayNode();
            }
        }
        //查找最小值
        public int FindMin () {
            Node current = root;
            while (!(current.Left == null))
                current = current.Left;
            return current.Data;
        }
        public int FindMax ()
        //查找最大值

        {
            Node current = root;
            while (!(current.Right == null))
                current = current.Right;
            return current.Data;
        }
        //查找特定结点
        public Node Find (int key) {
            Node current = root;
            while (current.Data != key) {
                if (key < current.Data) {
                    current = current.Left;
                } else {
                    current = current.Right;
                }
                if (current == null) { return null; }
            }
            return current;
        }
        public bool Delete (int key) {
            Node current = root;
            Node parent = root;
            bool isLeftChild = true; //是否为左结点

            while (current.Data != key)
            //判断当前结点是否为左结点
            {
                parent = current;
                if (key < current.Data) {
                    isLeftChild = true;
                    current = current.Left;
                } else {
                    isLeftChild = false;
                    current = current.Right;
                }
                if ((current == null))
                    return false;
            }
            if ((current.Left == null) & (current.Right == null)) //当前结点的左右结点为空
            {
                if (current == root)
                    root = null;
                else if (isLeftChild)
                    parent.Left = null;
                else
                    parent.Right = null;
            } else if (current.Right == null)    //当前结点的右结点为空
            {
                if (current == root) {
                    root = current.Left;
                } else if (isLeftChild) {
                    parent.Left = current.Left;
                } else {
                    parent.Right = current.Left;
                }
            } else if (current.Left == null) //当前结点的左结点为空
            {
                if (current == root) {
                    root = current.Right;
                } else if (isLeftChild) {
                    parent.Left = current.Right;
                } else {
                    parent.Right = current.Right;
                }
            } else {
                Node successor = GetSuccessor(current);
                if (current == root) {
                    root = successor;
                } else if (isLeftChild) {
                    parent.Left = successor;
                } else {
                    parent.Right = successor;
                }
                successor.Left = current.Left;
            }
            return true;
        }
        public Node GetSuccessor (Node delNode) //查找要删除结点的后继结点
        {
            Node successorParent = delNode;
            Node successor = delNode;
            Node current = delNode.Right;
            while (!(current == null)) {
                successorParent = current;
                successor = current;
                current = current.Left;
            }
            if (!(successor == delNode.Right)) {
                successorParent.Left = successor.Right;
                successor.Right = delNode.Right;
            }
            return successor;
        }
    }
}
