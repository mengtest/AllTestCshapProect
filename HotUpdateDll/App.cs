using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotUpdateCore;

namespace HotUpdateDll {
    public class App : IApp {
        double ver = 1;
        public double Ver {
            get {
                return ver;
            }
            set {
                ver = value;
            }
        }
        
        public void CheckVer () {
            throw new NotImplementedException();
        }

        public void Start () {
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine(System.Environment.CurrentDirectory);
            var dir = System.Environment.CurrentDirectory;
            string version = System.Reflection.Assembly.LoadFile(dir+"/HotUpdateCore.dll").GetName().Version.ToString();
            //string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine(version);
            Console.WriteLine("Start2");
        }
    }
}
