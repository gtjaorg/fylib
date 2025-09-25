

// kernel32
using System;
using System.Runtime.InteropServices;
using System.Text;
using FyLib.API;

/// <summary>
/// 常用API合集
/// </summary>
public static class kernel32
{
    /// <summary>
    /// 打开进程
    /// </summary>
    /// <param name="dwDesiredAccess"></param>
    /// <param name="bInheritHandle"></param>
    /// <param name="dwProcessId"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
    /// <summary>
    /// 读取进程数据
    /// </summary>
    /// <param name="hProcess"></param>
    /// <param name="lpBaseAddress"></param>
    /// <param name="lpBuffer"></param>
    /// <param name="nSize"></param>
    /// <param name="lpNumberOfBytesRead"></param>
    /// <returns></returns>

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);
    /// <summary>
    /// 写进程数据
    /// </summary>
    /// <param name="hProcess"></param>
    /// <param name="lpBaseAddress"></param>
    /// <param name="lpBuffer"></param>
    /// <param name="nSize"></param>
    /// <param name="lpNumberOfBytesWritten"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);
    /// <summary>
    /// 申请内存
    /// </summary>
    /// <param name="hProcess"></param>
    /// <param name="lpAddress"></param>
    /// <param name="dwSize"></param>
    /// <param name="flAllocationType"></param>
    /// <param name="flProtect"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
    /// <summary>
    /// 释放内存
    /// </summary>
    /// <param name="hProcess"></param>
    /// <param name="lpAddress"></param>
    /// <param name="dwSize"></param>
    /// <param name="dwFreeType"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

    /// <summary>
    /// 表示将要分配的内存页已经分配了物理内存，可以被使用。这是常用的分配类型。
    /// </summary>
    public const uint MEM_COMMIT = 0x1000;
    /// <summary>
    /// 表示为指定的大小保留了一段地址空间，但尚未分配物理内存。这样做的目的是为了确保在稍后调用 VirtualAllocEx 或 MapViewOfFile 时有足够的地址空间可用。
    /// </summary>
    public const uint MEM_RESERVE = 0x2000;
    /// <summary>
    /// 表示重置一块已经提交的内存区域，使其回到初始状态。这样做会将内存区域的内容清空。
    /// </summary>
    public const uint MEM_RELEASE = 0x8000;
    /// <summary>
    /// 表示释放内存页所占用的物理内存，但保留虚拟地址空间。这样做会导致相应的内存页无法被访问。
    /// </summary>
    public const uint MEM_DECOMMIT = 0x4000;
    /// <summary>
    /// 等待线程
    /// </summary>
    /// <param name="hHandle"></param>
    /// <param name="dwMilliseconds"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
    /// <summary>
    /// 创建远程线程
    /// </summary>
    /// <param name="hProcess"></param>
    /// <param name="lpThreadAttributes"></param>
    /// <param name="dwStackSize"></param>
    /// <param name="lpStartAddress"></param>
    /// <param name="lpParameter"></param>
    /// <param name="dwCreationFlags"></param>
    /// <param name="lpThreadId"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    /// <summary>
    /// 获取当前线程ID
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern int GetCurrentThreadId();
    /// <summary>
    /// 获取用户名扩展
    /// </summary>
    /// <param name="NameFormat">要获取的格式</param>
    /// <param name="Name">缓冲区</param>
    /// <param name="Size">缓冲区长度</param>
    /// <returns></returns>
    [DllImport("secur32.dll", CharSet = CharSet.Auto)]
    public static extern int GetUserNameEx(EXTENDED_NAME_FORMAT NameFormat, StringBuilder Name, ref uint Size);

    /// <summary>
    /// 获取计算机名
    /// </summary>
    /// <param name="NameType"></param>
    /// <param name="lpBuffer"></param>
    /// <param name="lpnSize"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetComputerNameEx(COMPUTER_NAME_FORMAT NameType, StringBuilder lpBuffer, ref uint lpnSize);

    /// <summary>
    /// 内存清零
    /// </summary>
    /// <param name="address"></param>
    /// <param name="len"></param>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern void RtlZeroMemory(IntPtr address, int len);

    /// <summary>
    /// 填充内存
    /// </summary>
    /// <param name="address"></param>
    /// <param name="len"></param>
    /// <param name="data"></param>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern void RtlFillMemory(IntPtr address, int len, byte data);

    /// <summary>
    /// 指针长度
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int LocalSize(IntPtr address);

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SetHandleCount(byte[] value);

    /// <summary>
    /// 修改内存权限
    /// </summary>
    /// <param name="lpAddress"></param>
    /// <param name="dwSize"></param>
    /// <param name="flNewProtect"></param>
    /// <param name="lpflOldProtect"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, int flNewProtect, out int lpflOldProtect);
    /// <summary>
    /// 复制对象句柄
    /// </summary>
    /// <param name="hSourceProcessHandle">包含要复制句柄的进程句柄。句柄必须具有PROCESS_DUP_HANDLE访问权限。 有关详细信息，请参阅 进程安全性和访问权限。</param>
    /// <param name="hSourceHandle">要复制的句柄。 这是在源进程的上下文中有效的开放对象句柄。 有关可复制其句柄的对象列表，请参阅以下“备注”部分。</param>
    /// <param name="hTargetProcessHandle">要接收重复句柄的进程句柄。 句柄必须具有PROCESS_DUP_HANDLE访问权限。此参数是可选的，如果选项中设置了DUPLICATE_CLOSE_SOURCE标志，则可以指定为 NULL。</param>
    /// <param name="lpTargetHandle">指向接收重复句柄的变量的指针。 此句柄值在目标进程的上下文中有效。如果 hSourceHandle 是 GetCurrentProcess 或 GetCurrentThread 返回的伪句柄， 则 DuplicateHandle 将它分别转换为进程或线程。如果 lpTargetHandle 为 NULL，则该函数将复制句柄，但不向调用方返回重复句柄值。 此行为仅用于与以前版本的此函数的向后兼容性。 不应使用此功能，因为目标进程终止之前将丢失系统资源。如果 hTargetProcessHandle 为 NULL，则忽略此参数。</param>
    /// <param name="dwDesiredAccess">为新句柄请求的访问。 有关可为每个对象类型指定的标志，请参阅以下“备注”部分。如果 dwOptions 参数指定DUPLICATE_SAME_ACCESS标志，则忽略此参数。 否则，可以指定的标志取决于要复制其句柄的对象的类型。 如果 hTargetProcessHandle 为 NULL，则忽略此参数。</param>
    /// <param name="bInheritHandle">一个变量，指示句柄是否可继承。 如果 为 TRUE，则重复句柄可由目标进程创建的新进程继承。 如果 为 FALSE，则无法继承新句柄。如果 hTargetProcessHandle 为 NULL，则忽略此参数。</param>
    /// <param name="dwOptions">可选操作。 此参数可以是零，也可以是以下值的任意组合。DUPLICATE_CLOSE_SOURCE:0x00000001关闭源句柄。 无论返回的任何错误状态如何，都会发生这种情况。DUPLICATE_SAME_ACCESS:0x00000002忽略 dwDesiredAccess 参数。 忽略 dwDesiredAccess 参数。 重复句柄具有与源句柄相同的访问权限。</param>
    /// <returns>如果该函数成功，则返回值为非零值。如果函数失败，则返回值为零。 要获得更多的错误信息，请调用 GetLastError。</returns>
    /// <remarks>重复句柄引用与原始句柄相同的对象。 因此，对象的任何更改都通过这两个句柄反映。 例如，如果复制文件句柄，则两个句柄的当前文件位置始终相同。 若要使文件句柄具有不同的文件位置，请使用 CreateFile 函数创建共享对同一文件的访问权限的文件句柄。源进程使用 GetCurrentProcess 函数获取自身句柄。 此句柄是伪句柄，但 DuplicateHandle 将其转换为实际进程句柄。 若要获取目标进程句柄，可能需要使用某种形式的进程间通信 (，例如命名管道或共享内存) 将进程标识符传达给源进程。 源进程可以使用 OpenProcess 函数中的此标识符来获取目标进程的句柄。如果调用 DuplicateHandle 的进程不是目标进程，则源进程必须使用进程间通信将重复句柄的值传递给目标进程。DuplicateHandle 可用于在 32 位进程和 64 位进程之间复制句柄。 生成的句柄大小适当，用于在目标进程中工作。 有关详细信息，请参阅 进程互操作性。</remarks>
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
    /// <summary>
    /// 忽略 dwDesiredAccess 参数。 忽略 dwDesiredAccess 参数。 重复句柄具有与源句柄相同的访问权限。
    /// </summary>
    public const int DUPLICATE_SAME_ACCESS = 0x2;
    /// <summary>
    /// 关闭源句柄。 无论返回的任何错误状态如何，都会发生这种情况。
    /// </summary>
    public const int DUPLICATE_CLOSE_SOURCE = 0x1;

    /// <summary>
    /// 检索当前进程的伪句柄。
    /// </summary>
    /// <returns>返回值是当前进程的伪句柄。</returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetCurrentProcess();
    /// <summary>
    /// 进程访问权限
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        /// <summary>
        /// 所有权限
        /// </summary>
        All = 0x001F0FFF,
        /// <summary>
        /// 终止进程
        /// </summary>
        Terminate = 0x00000001,
        /// <summary>
        /// 创建线程
        /// </summary>
        CreateThread = 0x00000002,
        /// <summary>
        /// 虚拟内存操作
        /// </summary>
        VMOperation = 0x00000008,
        /// <summary>
        /// 虚拟内存读取
        /// </summary>
        VMRead = 0x00000010,
        /// <summary>
        /// 虚拟内存写入
        /// </summary>
        VMWrite = 0x00000020,
        /// <summary>
        /// 复制句柄
        /// </summary>
        DupHandle = 0x00000040,
        /// <summary>
        /// 设置信息
        /// </summary>
        SetInformation = 0x00000200,
        /// <summary>
        /// 查询信息
        /// </summary>
        QueryInformation = 0x00000400,
        /// <summary>
        /// 同步
        /// </summary>
        Synchronize = 0x00100000
    }
    /// <summary>
    /// 打开进程
    /// </summary>
    /// <param name="dwDesiredAccess">权限</param>
    /// <param name="bInheritHandle">是否继承</param>
    /// <param name="dwProcessId">PID</param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
    /// <summary>
    /// 关闭句柄
    /// </summary>
    /// <param name="hObject"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern int CloseHandle(IntPtr hObject);
    /// <summary>
    /// 获取进程句柄数量
    /// </summary>
    /// <param name="handle">进程句柄</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern int GetProcessHandleCount(IntPtr handle, ref int count);
    /// <summary>
    /// 检索有关指定的 MS-DOS 设备名称的信息。
    /// </summary>
    /// <param name="lpDeviceName"></param>
    /// <param name="lpTargetPath"></param>
    /// <param name="ucchMax"></param>
    /// <returns></returns>

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

}
