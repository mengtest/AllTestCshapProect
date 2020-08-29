using System.Collections.Generic;

namespace Z
{
	class Zfun : Zchunk {
        public Zfun () {
            ChunkType = "function";
            int idx = Tools.RandomNum(0 , 6);
            for (int i = 0 ; i < idx ; i++) {
                Zchunk var = New<ZfunArgs>();
                Args.Add(var);
            }
        }
        private List<Zchunk> args = new List<Zchunk>();

        public List<Zchunk> Args {
            get {
                return args;
            }

            set {
                args = value;
            }
        }

        public override string ToString () {
            string result = Tab;
            if (Modifier != "")
                result += Modifier + " ";
            if (IsStatic)
                result += "static ";

            result += $"{VarType} {Name} (";
            for (int i = 0 ; i < Args.Count ; i++) {
                result += Args[i];
                if (i != Args.Count - 1)
                    result += ",";
            }

            result += "){\n";
            result += base.ToString();
            result += $"{Tab}\treturn {DefaultVal};\n";
            result += Tab + "}\n";
            return result;
        }
    }
}