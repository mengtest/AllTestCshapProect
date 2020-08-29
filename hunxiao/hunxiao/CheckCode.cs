using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexTest
{
    class CheckCode
    {
        public bool isInterface = false;
        public bool isInterfaceIng = false;
        public int isInterfaceNum = 0;
        public void checkInInterface(string str)
        {
            //跳过接口部分
            if (Regex.IsMatch(str, @"\s*interface\s*"))
                isInterface = true;
            if (isInterface)
            {
                if (Regex.IsMatch(str, @"(^\s*{\s*|\s*{\s*$)"))//以结尾的{开始 或者以 开头的{开始
                    isInterfaceNum++;
                if (Regex.IsMatch(str, @"^\s*\};?$"))//以 } 或者 }; 结尾
                {
                    isInterfaceNum--;
                    if (isInterfaceNum == 0)
                        isInterface = false;
                }
                //Console.WriteLine("isInterfaceNum ==" + isInterfaceNum + str);
                isInterfaceIng = isInterfaceNum == 1;
            }
        }


        public bool isInEnum = false;
        public bool isInEnumIng = false;
        public int isEnumNum = 0;
        public void checkInEnum(string str)
        {
            //跳过接口部分
            if (Regex.IsMatch(str, @"\s*enum\s*"))
                isInEnum = true;
            if (isInEnum)
            {
                if (Regex.IsMatch(str, @"(^\s*{\s*|\s*{\s*$)"))
                    isEnumNum++;
                if (Regex.IsMatch(str, @"^\s*\};?$"))
                {
                    isEnumNum--;
                    if (isEnumNum == 0)
                        isInEnum = false;
                }
                //Console.WriteLine("isEnumNum ==" + isEnumNum + str);
                isInEnumIng = isEnumNum == 1;
            }
        }

        public class ClassStruct
        {
            public int num = 0;
            public bool isStaitc = false;
            public bool isAbstract = false;
        }

        public bool isInClass = false;
        public bool isInStaticClass = false;
        public bool isInClassIng = false;
        public bool isInAbstractClass = false;
        public int classNum = 0;
        public Dictionary<int, ClassStruct> nums = new Dictionary<int, ClassStruct>();
        public void checkInClass(string str)
        {
            if (Regex.IsMatch(str, @"\s*\bclass\b\s*") && !Regex.IsMatch(str, @"\(.*\)"))
            {
                //Console.WriteLine("okkkkkkkkkkkkkkkkkkkkkkkk " + str);
                isInClass = true;
                isInClassIng = false;
                isInStaticClass = false;
                isInAbstractClass = false;
                classNum++;
            }
            if (!nums.ContainsKey(classNum))
                nums.Add(classNum, new ClassStruct());

            if (Regex.IsMatch(str, @"\s+static\s*class\s*"))
                nums[classNum].isStaitc = true;

            if (Regex.IsMatch(str, @"\s+abstract\s*class\s*"))
                nums[classNum].isAbstract = true;

            if (classNum > 0)
            {
                if (Regex.IsMatch(str, @"^\s*{\s*") || Regex.IsMatch(str, @"\s*{\s*$"))
                    nums[classNum].num++;
                if (Regex.IsMatch(str, @"\{ .*\}$")) { }
                else if (Regex.IsMatch(str, @"^\s*\}[;|,]?$")
                    || Regex.IsMatch(str, @"^\s*\}\s*else.*") 
                    || Regex.IsMatch(str, @"^\s*\}\);.*$") 
                    || Regex.IsMatch(str, @"^\s*\}\s*,") 
                    || Regex.IsMatch(str, @"^\s*\}\s*catch.*"))
                {
                    nums[classNum].num--;
                    if (nums[classNum].num == 0)
                        classNum--;
                    if (classNum <= 0)
                    {
                        //Console.WriteLine("kkkkkkkkkkkkkkkkkk");
                        nums = new Dictionary<int, ClassStruct>();
                        isInClass = false;
                    }
                }

                //System.Threading.Thread.Sleep(10);

                if (classNum > 0)
                {
                    isInClassIng = nums[classNum].num == 1;
                    isInStaticClass = classNum > 0 && nums[classNum].isStaitc;
                    isInAbstractClass = classNum > 0 && nums[classNum].isAbstract;
                //    Console.WriteLine(classNum + "  isInClassNum ==" + nums[classNum].num + str);
                //   System.Threading.Thread.Sleep(100);
                }
                else
                {
                    //Console.WriteLine(classNum + "  isInClassNum ==" + str);
                    isInClassIng = false;
                    isInStaticClass = false;
                    isInAbstractClass = false;
                }
            }
        }
        
        public bool isStruct = false;       //是否在结构体当中
        public bool isStructIng = false;    //是否在定义块当中
        public int isStructNum = 0;         //括号的数量
        public void checkInStruct(string str)
        {
            if (Regex.IsMatch(str, @"struct\s*"))
                isStruct = true;
            if (isStruct)
            {
                if (Regex.IsMatch(str, @"(^\s*{\s*|\s*{\s*$)"))
                    isStructNum++;
                if (Regex.IsMatch(str, @"^\s*\};?$"))
                {
                    isStructNum--;
                    if (isStructNum == 0)
                        isStruct = false;
                }
                //Console.WriteLine("isStructNum ==" + isStructNum + str);
                isStructIng = isStructNum == 1;
            }
        }

        public bool isFor = false;
        public void checkInFor(string str)
        {
            if (Regex.IsMatch(str, @"^\s*for\s?\("))
                isFor = true;
            if (isFor)
                if (Regex.IsMatch(str, @"\){1}"))   // 匹配for中的语句是不会换行的，换行属异常
                    isFor = false;
        }

        //if 和 else 开始的地方 过后两行才能插入代码
        public int lastIFCount = 0;
        public void checkInIf(string str)
        {
            //Console.WriteLine($"checkInIf  {lastIFCount}  {str}");
            //System.Threading.Thread.Sleep(100);
            if (lastIFCount > 0)
            {
                if (Regex.IsMatch(str, @"^.*{\s*") || Regex.IsMatch(str, @"\s*{\s*$")) //遇到有括号，是完整的if语句
                {
                    lastIFCount = 0;
                    return;
                }
                else if (Regex.IsMatch(str, @"^\s*\};?$")) //遇到有括号，是完整的if语句
                {
                    lastIFCount = 0;
                    return;
                }
                else
                    lastIFCount -= 1;
            }
            if (Regex.IsMatch(str, @"^\s*if.*{$"))  //if以括号结尾，之后直接插入语句
            {
                lastIFCount = 0;
                return;
            }
            else if (Regex.IsMatch(str, @"^\s*if.*"))   //if语句开始
            {
                lastIFCount = 2;
                if (Regex.IsMatch(str, @"^\s*{\s*") || Regex.IsMatch(str, @"\s*{\s*$"))
                {
                    lastIFCount = 0;
                    return;
                }
            }
            if (Regex.IsMatch(str, @"^\s*else.*"))   //else语句开始
            {
                lastIFCount = 2;

                if (Regex.IsMatch(str, @"^\s*{\s*") || Regex.IsMatch(str, @"\s*{\s*$"))
                {
                    lastIFCount = 0;
                    return;
                }
                return;
            }
        }
    }
}