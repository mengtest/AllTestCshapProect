using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenCode {
    class Program {
        static void Main (string[] args) {
            int line = 0;
            Tools.stopwatch.Start();
            while (true) {
                switch (line) {
                    case 0;
                        break;
                    default:
                        break;
                }
                int idx = Tools.RandomNum(0 , 1);
                if (idx == 1) {
                    line++;
                }
            }
        }
    }
}
