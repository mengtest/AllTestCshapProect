namespace Z
{
	class Zfor : Zif {
        public Zfor () {
            ChunkType = "for";
        }
        public override string GetIfType () {
            Zchunk var = GetVars();
            return $" ( ; {var.Name} == {var.DefaultNoneVal} ; ) ";
        }
    }
}