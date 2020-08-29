using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotUpdateCore {
    public interface IApp {
        double Ver { get; set; }
        void Start ();
        void CheckVer ();
    }
}
