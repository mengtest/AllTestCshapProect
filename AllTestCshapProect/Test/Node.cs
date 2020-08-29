using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    public class Node {

        public int Data;

        public Node Left;
        public Node Right;
        public void DisplayNode () {
            Console.Write(Data + " ");
        }
    }
}
