using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Z_CShapCore {
    public static class Win32Api {
        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="id">热键唯一，序列号，不和其它冲突，自己定的，管理用的</param>
        /// <param name="modifiers">控制key，控制键，alt，ctrl。HotkeyModifiers枚举值 </param>
        /// <param name="vk">按键key</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey (IntPtr hWnd , int id , int modifiers , Keys vk);
        /// <summary>
        /// 卸载热键
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="id">热键唯一</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey (IntPtr hWnd , int id);
        /// <summary>
        /// 获取当前活动窗口句柄，在其它窗体热键运行时获取当前窗体句柄
        /// 获取当前窗口句柄，返回IntPtr格式，可以强转为int
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll" , CharSet = CharSet.Auto , ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow ();
        /// <summary>
        /// 找指定窗口句柄
        /// </summary>
        /// <param name="lpClassName">填null的也行</param>
        /// <param name="lpWindowName">进程名字，字符串</param>
        [DllImport("user32.dll")]
        public extern static IntPtr FindWindow (string lpClassName , string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage (IntPtr hwnd , uint wMsg , int wParam , int lParam);//发送消息
        /// <summary>
        /// 设置窗体显示方式
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow">WindowShowStatus枚举类型</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindow (IntPtr hwnd , int nCmdShow);
        /// <summary>
        /// 设置句柄窗口标题
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpString">标题字符串</param>
        /// <returns></returns>
        [DllImport("user32.dll" , EntryPoint = "SetWindowText" , CharSet = CharSet.Ansi)]
        public static extern int SetWindowText (int hwnd , string lpString);
        /// <summary>
        /// 获取线程ID
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <param name="ID">返回id </param>
        /// <returns></returns>
        [DllImport("User32.dll" , CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId (IntPtr hwnd , out int ID);
        /// <summary>
        /// 获取句柄窗体的标题
        /// </summary>
        /// <param name="hwnd">带文本的窗口或控制的句柄</param>
        /// <param name="lpString">指向接收文本的缓冲区的指针</param>
        /// <param name="cch">:指定要保存在缓冲区内的字符的最大个数</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText (int hwnd , StringBuilder lpString , int cch);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx (IntPtr hwndParent , IntPtr hwndChildAfter , string lpszClass , string lpszWindow);
        
        public delegate bool WNDENUMPROC (IntPtr hWnd , int lParam);
        /// <summary>
        /// 用来遍历所有窗口 
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnumWindows (WNDENUMPROC lpEnumFunc , int lParam);
        /// <summary>
        /// 获取窗口Text 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW (IntPtr hWnd , [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString , int nMaxCount);
        /// <summary>
        /// 获取窗口类名 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetClassNameW (IntPtr hWnd , [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString , int nMaxCount);

        [DllImport("user32.dll" , EntryPoint = "IsWindow")]
        public static extern bool IsWindow (IntPtr hWnd);

        [DllImport("user32.dll" , EntryPoint = "GetParent" , SetLastError = true)]
        public static extern IntPtr GetParent (IntPtr hWnd);

        [DllImport("user32.dll" , EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId (IntPtr hWnd , ref uint lpdwProcessId);

        [DllImport("kernel32.dll" , EntryPoint = "SetLastError")]
        public static extern void SetLastError (uint dwErrCode);


        /// <summary>
        /// 改变当前窗口标题
        /// </summary>
        /// <param name="_str"></param>
        public static void ChangeTitleStr (string _str) {
            // int lHwnd = FindWindow(null, "计算器");
            IntPtr lHwnd = GetForegroundWindow();
            SetWindowText((int)lHwnd , _str); //当前句柄改标题
        }

        /// <summary>
        /// 窗口最小化
        /// </summary>
        public static void ShowMinImized () {
            ShowWindow(GetForegroundWindow() , (int)WindowShowStatus.SW_SHOWMINIMIZED);
        }
        /// <summary>
        /// 窗口最大化
        /// </summary>
        public static void ShowMaxImized () {
            ShowWindow(GetForegroundWindow() , (int)WindowShowStatus.SW_SHOWMAXIMIZED);
        }
    }
}
