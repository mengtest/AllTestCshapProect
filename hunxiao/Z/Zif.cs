using System.Collections.Generic;

namespace Z
{
	class Zif : Zchunk {
        public List<Zchunk> elseIf = new List<Zchunk>();
        public Zif () {
            ChunkType = "if";
        }
        public virtual string GetIfType () {
            Zchunk var = GetVars();
            string result = "";
            int idx = Tools.RandomNum(0 , 3);
            if (idx == 0)
                result += $" ({var.Name} == {var.DefaultNoneVal}) ";
            else if (idx == 1)
                result += $" ({var.Name} == {var.DefaultNoneVal} || {var.Name} == {var.DefaultNoneVal}) ";
            else if (idx == 2)
                result += $" ({var.Name} == {var.DefaultNoneVal} && {var.Name} == {var.DefaultNoneVal}) ";
            return result;
        }
        public override string ToString () {
            string result = Tab;
            result += $"{ChunkType}";
            result += GetIfType() + "{\n";
            result += base.ToString();
            result += Tab + "}\n";
            for (int i = 0 ; i < elseIf.Count ; i++)
                result += elseIf[i].ToString();
            return result;
        }
        public ZelseIf AddElseIf (ZelseIf _if) {
            _if.Parent = this.Parent;
            _if.CurLine = this.CurLine;
            elseIf.Add(_if);
            return _if;
        }
        public Zelse AddElseIf (Zelse _if) {
            _if.Parent = this.Parent;
            _if.CurLine = this.CurLine;
            elseIf.Add(_if);
            return _if;
        }
    }
}