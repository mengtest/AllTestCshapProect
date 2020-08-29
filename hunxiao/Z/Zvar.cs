namespace Z
{
	class Zvar : Zchunk {
        public Zvar () {
            ChunkType = "var";
        }
        public override string ToString () {
            string result = Tab;
            if (Parent != null && Parent.GetType() == typeof(Zclass)) {
                if (Modifier != "")
                    result += Modifier + " ";
                if (IsStatic)
                    result += "static ";
            }
            result += $"{VarType} {Name}";
            if (Parent == null || Parent.GetType() == typeof(Zclass))
                result += $" = {DefaultVal};\n";

            return result;
        }
    }
}