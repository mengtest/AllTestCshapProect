namespace Z
{
	class ZfunArgs : Zvar {
        public ZfunArgs () {
            ChunkType = "funArgs";
        }
        public override string ToString () {
            return $"{VarType} {Name} = {DefaultVal}";
        }
    }
}