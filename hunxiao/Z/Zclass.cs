namespace Z
{
	class Zclass : Zchunk {
        public Zclass () {
            ChunkType = "class";
        }
        public override string ToString () {
            string result = Tab;
            if (Modifier != "")
                result += Modifier + " ";
            if (IsStatic)
                result += "static ";
            result += $"{ChunkType} {Name}";
            result += "{\n";
            result += base.ToString();
            result += Tab + "}\n";
            return result;
        }
    }
}