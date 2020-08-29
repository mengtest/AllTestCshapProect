namespace Z
{
	class Zattribute : Zchunk {
        public Zattribute () {
            ChunkType = "attribute";
        }
        public override string ToString () {
            string result = Tab;
            if (Modifier != "")
                result += Modifier + " ";
            if (IsStatic)
                result += "static ";
            result += $"{VarType} {Name} {{ get; set; }}\n";
            return result;
        }
    }
}