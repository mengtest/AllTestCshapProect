namespace Z
{
	class Znamespace : Zchunk {
        public Znamespace () {
            ChunkType = "namespace";
        }
        public override string ToString () {
            string result = "";
            result += $"{ChunkType} {Name}";
            result += Tab + "{\n";
            result += base.ToString();
            result += Tab + "}\n";
            return result;
        }
    }
}