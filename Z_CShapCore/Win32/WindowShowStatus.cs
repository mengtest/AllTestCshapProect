using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_CShapCore {
    public enum WindowShowStatus {
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        SW_HIDE = 0,
        /// <summary>
        /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态，与SW_RESTORE 相同
        /// </summary>
        SW_SHOWNORMAL = 1,
        /// <summary>
        /// 最小化窗口，并将其激活
        /// </summary>
        SW_SHOWMINIMIZED = 2,
        /// <summary>
        /// 最大化窗口，并将其激活
        /// </summary>
        SW_SHOWMAXIMIZED = 3,
        /// <summary>
        /// 用最近的大小和位置显示一个窗口，同时不改变活动窗口
        /// </summary>
        SW_SHOWNOACTIVATE = 4,
        /// <summary>
        /// 用当前的大小和位置显示一个窗口，同时令其进入活动状态
        /// </summary>
        SW_SHOW = 5,
        /// <summary>
        /// 最小化窗口
        /// </summary>
        SW_MINIMIZE = 6,
        /// <summary>
        /// 最小化一个窗口，同时不改变活动窗口
        /// </summary>
        SW_SHOWMINNOACTIVE = 7,
        /// <summary>
        /// 用当前的大小和位置显示一个窗口，不改变活动窗口
        /// </summary>
        SW_SHOWNA = 8,
        /// <summary>
        /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
        /// </summary>
        SW_RESTORE = 9,
        /// <summary>
        /// 关闭窗体
        /// </summary>
        WM_CLOSE = 0x10
    }
}
