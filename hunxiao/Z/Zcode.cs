namespace Z
{
	class Zcode : Zchunk {
        public override string ToString () {
            string result = Tab;
            Zchunk var = GetVars();
            int idx = Tools.RandomNum(0 , 3);
            if (var.VarType == "bool") {
                if (idx == 0)
                    result += $"{var.Name} = {var.DefaultVal};";
                else if (idx == 1)
                    result += $"{var.Name} = {var.Name} || {var.Name};";
                else if (idx == 2)
                    result += $"{var.Name} = {var.Name} && {var.Name};";
            } else if (var.VarType == "int" || var.VarType == "long") {
                if (idx == 0)
                    result += $"{var.Name} = {var.DefaultVal};";
                else if (idx == 1)
                    result += $"{var.Name} += {var.Name};";
                else if (idx == 2)
                    result += $"{var.Name} -= {var.Name};";
            } else if (var.VarType == "string") {
                if (idx == 0)
                    result += $"{var.Name} = {var.DefaultVal};";
                else if (idx == 1)
                    result += $"{var.Name} += {var.Name};";
                else if (idx == 2)
                    result += $"{var.Name} = {var.DefaultVal};";
            }

            //TODO：类型是方法

            result += "\n";
            return result;
        }
    }
}