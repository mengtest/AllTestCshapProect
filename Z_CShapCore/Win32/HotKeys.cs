using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z_CShapCore {
    public class HotKeys {
        private static System.Collections.Hashtable processWnd = null;
        public static IntPtr GetCurrentWindowHandle () {
            IntPtr ptrWnd = IntPtr.Zero;
            int uiPid = System.Diagnostics.Process.GetCurrentProcess().Id;  // 当前进程 ID
            object objWnd = processWnd[uiPid];

            if (objWnd != null) {
                ptrWnd = (IntPtr)objWnd;
                if (ptrWnd != IntPtr.Zero && Win32Api.IsWindow(ptrWnd))  // 从缓存中获取句柄
                {
                    return ptrWnd;
                } else {
                    ptrWnd = IntPtr.Zero;
                }
            }

            bool bResult = Win32Api.EnumWindows(new Win32Api.WNDENUMPROC(EnumWindowsProc) , uiPid);
            // 枚举窗口返回 false 并且没有错误号时表明获取成功
            if (!bResult && System.Runtime.InteropServices.Marshal.GetLastWin32Error() == 0) {
                objWnd = processWnd[uiPid];
                if (objWnd != null) {
                    ptrWnd = (IntPtr)objWnd;
                }
            }
            return ptrWnd;
        }


        private static bool EnumWindowsProc (IntPtr hwnd , int lParam) {
            uint uiPid = 0;

            if (Win32Api.GetParent(hwnd) == IntPtr.Zero) {
                Win32Api.GetWindowThreadProcessId(hwnd , ref uiPid);
                if (uiPid == lParam)    // 找到进程对应的主窗口句柄
                {
                    processWnd[uiPid] = hwnd;   // 把句柄缓存起来
                    Win32Api.SetLastError(0);    // 设置无错误
                    return false;   // 返回 false 以终止枚举窗口
                }
            }
            return true;
        }


        ~HotKeys () {
            UnRegist(Win32Api.GetForegroundWindow());
        }
        static int keyid = 0;     //区分不同的快捷键
        static Dictionary<int, HotKeyCallBackHanlder> keymap = new Dictionary<int, HotKeyCallBackHanlder>();   //每一个key对于一个处理函数
        public delegate void HotKeyCallBackHanlder ();

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="hWnd">句柄，</param>
        /// <param name="modifiers"></param>
        /// <param name="vk"></param>
        /// <param name="callBack"></param>
        public static void Regist (IntPtr ptr , HotkeyModifiers modifiers , Keys vk , HotKeyCallBackHanlder callBack) {
            if (!Win32Api.RegisterHotKey(ptr , keyid , (int)modifiers , vk)) {
                throw new Exception("注册失败！");
            }
            keymap[keyid++] = callBack;
        }

        // 注销快捷键
        public static void UnRegist (IntPtr hWnd) {
            foreach (KeyValuePair<int , HotKeyCallBackHanlder> var in keymap) {
                Win32Api.UnregisterHotKey(hWnd , var.Key);
            }
        }

        // 快捷键消息处理
        public static void ProcessHotKey (Message m) {
            if (m.Msg == 0x312) {
                int id = m.WParam.ToInt32();
                HotKeyCallBackHanlder callback;
                if (keymap.TryGetValue(id , out callback))
                    callback();
            }
        }

        // 快捷键消息处理
        public static IntPtr ProcessHotKey (IntPtr handle , int message , IntPtr wParam , IntPtr lParam , ref bool handled) {
            if (message == 0x312) {
                int id = wParam.ToInt32();
                HotKeyCallBackHanlder callback;
                if (keymap.TryGetValue(id , out callback))
                    callback();
                return new IntPtr(1);
            }
            return IntPtr.Zero;
        }
    }
}
