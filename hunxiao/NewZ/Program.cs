using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z;

namespace NewZ {
    class Program {
        //base  行
        //声明:class,interface,属性,变量,function,(public,static,virtual,overriding)
        //语法块:using,lock,namespace,if,while,for,function,switch
        //语句:计算,call function,赋值,return,
        public abstract class Base {
            public int CurLine { get; set; }
            public Base Parent { get; set; }
            public string Tab {
                get {
                    string tab = "";
                    for (int i = 0 ; i < CurLine ; i++)
                        tab += "\t";
                    return tab;
                }
            }
            public override string ToString () {
                return $"{GetHead()}{GetStart()}{GetCode()}{GetEnd()}";
            }
            public virtual string GetHead () => "";
            public virtual string GetStart () => "";
            public virtual string GetCode () => "";
            public virtual string GetEnd () => "";
        }

        public abstract class Chunk : Base {
            public List<Base> Allchunk { get; set; } = new List<Base>();
            public string ToStringAllChunk () {
                string result = "";
                for (int i = 0 ; i < Allchunk.Count ; i++)
                    result += Allchunk[i].ToString();
                return result;
            }

            public Base AddChunk (Base _z) {
                Allchunk.Add(_z);
                _z.CurLine = this.CurLine + 1;
                _z.Parent = this;
                return _z;
            }
            public override string GetCode () {
                return ToStringAllChunk();
            }
        }

        public class Define : Chunk {
            public virtual string Name { get; set; } = "测试";
            public ModifierEnum Modifier { get; set; } = ModifierEnum.Public;
            public bool IsStatic { get; set; } = true;
            public bool IsAbstract { get; set; } = false;
            public string GetIsStatic {
                get { return IsStatic ? "static " : ""; }
            }
            public string GetIsAbstract {
                get { return IsAbstract ? "abstract " : ""; }
            }
            public string GetModifier {
                get {
                    switch (Modifier) {
                        case ModifierEnum.None:
                            return "";
                        case ModifierEnum.Private:
                            return "private ";
                        case ModifierEnum.Public:
                            return "public ";
                        default:
                            return "";
                    }
                }
            }
        }

        public class Using : Define {
            public override string GetCode () {
                return $"using {Name};\n";
            }
        }

        public class Else : Chunk {
            public override string GetHead () {
                return $"{Tab}else";
            }
        }

        public enum ModifierEnum {
            None,
            Private,
            Public,
        }

        public class Class : Define {
            public override string GetHead () {
                return $"{Tab}{GetModifier}{GetIsAbstract}{GetIsStatic}class {Name}";
            }
        }

        public class Namespace : Chunk {
            public override string GetStart () {
                return $"{Tab}namespace {Name}{base.GetStart()}";
            }
        }

        public class Condition : Chunk {
            public virtual string TypeName { get; set; }
            public override string GetHead () {
                return $"{Tab}{TypeName} ({GetCondition()})";
            }
            public string GetCondition () {
                return "true";
            }
        }

        public class If : Condition {
            public override string TypeName { get; set; } = "if";
        }

        public class Code : List<Base> {
            public override string ToString () {
                string ret = "";
                foreach (var item in this) {
                    ret += item;
                }
                return ret;
            }
        }

        static void Main (string[] args) {
            var a = new Code();
            a.Add(new Using());
            a.Add(new Using());
            var n = new Namespace() { Name = "测试" };
            n.AddChunk(new Class() { Name = "测试" });

            n.AddChunk(new If());

            a.Add(n);
            File.WriteAllText("1.txt" , a.ToString());
            Console.WriteLine(a);
        }
    }
}