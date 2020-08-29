using System;
using System.Collections.Generic;

namespace Z {
    class Zchunk {
        public Zchunk () {  Init(); }
        public void Init () {
            Name = Tools.GetBase64();
            Modifier = Tools.varModifier[Tools.RandomNum(0 , Tools.varModifier.Length)];
            VarType = Tools.varType[Tools.RandomNum(0 , Tools.varType.Length)];
            IsStatic = Tools.RandomNum(0 , 2) == 1;
        }
        public static T New<T> () {  return Activator.CreateInstance<T>(); }
        public List<Zchunk> Allchunk { get; set; } = new List<Zchunk>();
        public int CurLine { get; set; }
        public string Modifier { get; set; }
        public bool IsStatic { get; set; }
        public string VarType { get; set; }
        public Zchunk Parent { get; set; }
        public string Name { get; set; }
        public string ChunkType { get; set; }
        public string Tab {
            get {
                string tab = "";
                for (int i = 0 ; i < CurLine ; i++)
                    tab += "\t";
                return tab;
            }
        }
        public string DefaultVal {
            get {
                if (VarType == "int" || VarType == "long")
                    return "0";
                else if (VarType == "string")
                    return "\"0\"";
                else if (VarType == "bool")
                    return "false";
                return "";
            }
        }
        public string DefaultNoneVal {
            get {
                if (VarType == "int" || VarType == "long")
                    return "-1";
                else if (VarType == "string")
                    return "\"-1\"";
                else if (VarType == "bool")
                    return "false";
                return "";
            }
        }
        public Zchunk GetVars () {
            List<Zchunk> list = GetVars(this);
            List<Zchunk> funs = GetVars(this , new List<Type>() {
                typeof(Zfun),
            });
            if (list.Count > 0) {
                if (funs.Count > 0 && funs[0].IsStatic) {
                    for (int i = list.Count - 1 ; i >= 0 ; i--) {
                        Zchunk chunk = list[i];
                        if (chunk.GetType().Name != typeof(ZfunArgs).Name)
                            if (chunk.IsStatic == false)
                                list.RemoveAt(i);
                    }
                }
                if (list.Count > 0)
                    return list[Tools.RandomNum(0 , list.Count)];
            }
            return null;
        }
        public List<Zchunk> GetVars (Zchunk curChunk , List<Type> types = null) {
            List<Zchunk> list = new List<Zchunk>();
            if (curChunk == null)
                return list;
            if (types == null)
                types = new List<Type>() {
                    typeof(Zvar),
                    typeof(Zattribute),
                    typeof(ZfunArgs)
                };
            if (types.Contains(typeof(ZfunArgs))) {
                Zfun fun = curChunk as Zfun;
                if (fun != null && fun.Args != null) {
                    foreach (var arg in fun.Args)
                        list.Add(arg);
                }
                foreach (var chunk in curChunk.Allchunk) {
                    if (types.Contains(chunk.GetType()))
                        list.Add(chunk);
                }
            } else {
                if (types.Contains(curChunk.GetType()))
                    list.Add(curChunk);
            }
            if (curChunk.Parent != null)
                list.AddRange(GetVars(curChunk.Parent , types));
            return list;
        }

        public override string ToString () {
            string result = "";
            for (int i = 0 ; i < Allchunk.Count ; i++)
                result += Allchunk[i].ToString();
            return result;
        }

        public virtual Zchunk AddChunk (Zchunk _z) {
            Allchunk.Add(_z);
            _z.CurLine = this.CurLine + 1;

            if (GetType() == typeof(Zclass))
                if (IsStatic)
                    _z.IsStatic = IsStatic;

            _z.Parent = this;
            return _z;
        }
    }
}