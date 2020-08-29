using System;
using System.Reflection;
using HotUpdateCore;
using Z_CShapCore;

namespace HotUpdateApp {
    class MainApp {

        static void Main (string[] args) {
            Assembly ass = Assembly.Load("HotUpdateDll");
            Type t = ass.GetType("HotUpdateDll.App");
            IApp o = Activator.CreateInstance(t) as IApp;
            o.Start();
            Win32Api.ChangeTitleStr("6666");
            //System.Threading.Thread.Sleep(1000);
            //Console.WriteLine(Win32Api.GetForegroundWindow());
            //Console.WriteLine(HotKeys.GetCurrentWindowHandle());
            //HotKeys.Regist(HotKeys.GetCurrentWindowHandle() , HotkeyModifiers.Alt , Keys.E , () => {
            //    Console.WriteLine(1);
            //});
            Console.Read();
        }

        void test () {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            //Console.WriteLine(LitJson.JsonMapper.ToJson(new object()));

            //这里通过调用Assembly.Load方法反射加载MessageDisplay程序集会失败，因为本项目中没有引用该程序集
            //而且MessageDisplay程序集的dll文件也不在本项目生成的bin目录下，也不在GAC中。
            //所以这里会触发AssemblyResolve事件，调用处理函数CurrentDomain_AssemblyResolve来尝试执行自定义程序集加载逻辑
            //然后处理函数CurrentDomain_AssemblyResolve会为这里的Assembly.Load方法返回MessageDisplay.dll程序集
            var messageDisplayAssembly = Assembly.Load("LitJson");
            //使用反射动态调用MessageDisplayHelper类的构造函数
            var messageDisplayHelper = messageDisplayAssembly.CreateInstance("LitJson.JsonMapper");
            Console.WriteLine(messageDisplayHelper.ToString());

            //同样这里通过Type.GetType方法反射加载MessageDisplay程序集也会失败，会触发AssemblyResolve事件
            //调用处理函数CurrentDomain_AssemblyResolve来尝试执行自定义程序集加载逻辑
            //然后处理函数CurrentDomain_AssemblyResolve会为这里的Type.GetType方法返回所需要的程序集MessageDisplay.dll
            //和Assembly.Load方法不同，如果AssemblyResolve事件的处理函数CurrentDomain_AssemblyResolve为Type.GetType方法返回了null
            //Type.GetType方法并不会抛出异常，而是也返回一个null

            Type type = Type.GetType("LitJson.JsonMapper, LitJson");
            Console.WriteLine(type.ToString());
            Console.WriteLine();
            var method = type.GetMethod("ToJson" , new Type[] { typeof(string) });
            Console.WriteLine(method);
            Console.WriteLine(method.Invoke(null , new object[] { new object() }).ToString());

            //foreach (var item in type.GetMembers()) {
            //    Console.WriteLine(item);
            //}
            Console.Read();
        }

        private static Assembly CurrentDomain_AssemblyResolve (object sender , ResolveEventArgs args) {
            string resourceName = "UD.plugin." + new AssemblyName(args.Name).Name + ".dll";
            Console.WriteLine(resourceName);

            foreach (var item in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                Console.WriteLine(item);
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData , 0 , assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}