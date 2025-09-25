using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FyLib.API
{
    /// <summary>
    /// ntdll.dll API 封装类
    /// 提供 Windows NT 层系统服务的接口，主要用于查询系统对象和句柄信息
    /// 注意：这些API是Windows内部使用的未公开API，使用时需要谨慎
    /// </summary>
    public static class Ntdll
    {
        /// <summary>
        /// 查询对象信息
        /// </summary>
        /// <param name="ObjectHandle">对象句柄</param>
        /// <param name="ObjectInformationClass">对象信息类别</param>
        /// <param name="ObjectInformation">用于接收信息的缓冲区指针</param>
        /// <param name="ObjectInformationLength">缓冲区大小</param>
        /// <param name="returnLength">返回实际需要的缓冲区大小</param>
        /// <returns>NT状态码</returns>
        [DllImport("ntdll.dll")]
        public static extern int NtQueryObject(IntPtr ObjectHandle, int ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength, ref int returnLength);

        /// <summary>
        /// 查询系统信息
        /// </summary>
        /// <param name="SystemInformationClass">系统信息类别</param>
        /// <param name="SystemInformation">用于接收信息的缓冲区指针</param>
        /// <param name="SystemInformationLength">缓冲区大小</param>
        /// <param name="returnLength">返回实际需要的缓冲区大小</param>
        /// <returns>NT状态码</returns>
        [DllImport("ntdll.dll")]
        public static extern uint NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, ref int returnLength);

        /// <summary>
        /// 最大路径长度常量
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// 状态码：信息长度不匹配
        /// </summary>
        public const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;


        /// <summary>
        /// 对象信息类别枚举
        /// 用于指定要查询的对象信息类型
        /// </summary>
        public enum ObjectInformationClass : int
        {
            /// <summary>
            /// 基本对象信息
            /// </summary>
            ObjectBasicInformation = 0,
            /// <summary>
            /// 对象名称信息
            /// </summary>
            ObjectNameInformation = 1,
            /// <summary>
            /// 对象类型信息
            /// </summary>
            ObjectTypeInformation = 2,
            /// <summary>
            /// 所有对象类型信息
            /// </summary>
            ObjectAllTypesInformation = 3,
            /// <summary>
            /// 对象句柄信息
            /// </summary>
            ObjectHandleInformation = 4
        }
        /// <summary>
        /// 系统句柄信息结构体
        /// 用于存储系统中句柄的相关信息（信息类 16）
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SYSTEM_HANDLE_INFORMATION
        {
            /// <summary>
            /// 进程ID
            /// </summary>
            public int ProcessID;
            /// <summary>
            /// 对象类型编号
            /// </summary>
            public byte ObjectTypeNumber;
            /// <summary>
            /// 句柄标志：0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
            /// </summary>
            public byte Flags;
            /// <summary>
            /// 句柄值
            /// </summary>
            public ushort Handle;
            /// <summary>
            /// 对象指针
            /// </summary>
            public int Object_Pointer;
            /// <summary>
            /// 授予的访问权限
            /// </summary>
            public UInt32 GrantedAccess;
        }
        /// <summary>
        /// 对象基本信息结构体
        /// 用于存储对象的基本属性和统计信息（信息类 0）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_BASIC_INFORMATION
        {
            /// <summary>
            /// 对象属性
            /// </summary>
            public int Attributes;
            /// <summary>
            /// 授予的访问权限
            /// </summary>
            public int GrantedAccess;
            /// <summary>
            /// 句柄计数
            /// </summary>
            public int HandleCount;
            /// <summary>
            /// 指针计数
            /// </summary>
            public int PointerCount;
            /// <summary>
            /// 分页池使用量
            /// </summary>
            public int PagedPoolUsage;
            /// <summary>
            /// 非分页池使用量
            /// </summary>
            public int NonPagedPoolUsage;
            /// <summary>
            /// 保留字段1
            /// </summary>
            public int Reserved1;
            /// <summary>
            /// 保留字段2
            /// </summary>
            public int Reserved2;
            /// <summary>
            /// 保留字段3
            /// </summary>
            public int Reserved3;
            /// <summary>
            /// 名称信息长度
            /// </summary>
            public int NameInformationLength;
            /// <summary>
            /// 类型信息长度
            /// </summary>
            public int TypeInformationLength;
            /// <summary>
            /// 安全描述符长度
            /// </summary>
            public int SecurityDescriptorLength;
            /// <summary>
            /// 创建时间
            /// </summary>
            public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
        }
        /// <summary>
        /// 对象类型信息结构体
        /// 用于存储对象类型的详细信息（信息类 2）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_TYPE_INFORMATION
        {
            /// <summary>
            /// 对象类型名称
            /// </summary>
            public UNICODE_STRING Name;
            /// <summary>
            /// 该类型对象的总数
            /// </summary>
            public int ObjectCount;
            /// <summary>
            /// 该类型对象的句柄总数
            /// </summary>
            public int HandleCount;
            /// <summary>
            /// 保留字段1
            /// </summary>
            public int Reserved1;
            /// <summary>
            /// 保留字段2
            /// </summary>
            public int Reserved2;
            /// <summary>
            /// 保留字段3
            /// </summary>
            public int Reserved3;
            /// <summary>
            /// 保留字段4
            /// </summary>
            public int Reserved4;
            /// <summary>
            /// 该类型对象的最大数量
            /// </summary>
            public int PeakObjectCount;
            /// <summary>
            /// 该类型对象句柄的最大数量
            /// </summary>
            public int PeakHandleCount;
            /// <summary>
            /// 保留字段5
            /// </summary>
            public int Reserved5;
            /// <summary>
            /// 保留字段6
            /// </summary>
            public int Reserved6;
            /// <summary>
            /// 保留字段7
            /// </summary>
            public int Reserved7;
            /// <summary>
            /// 保留字段8
            /// </summary>
            public int Reserved8;
            /// <summary>
            /// 无效属性
            /// </summary>
            public int InvalidAttributes;
            /// <summary>
            /// 通用映射
            /// </summary>
            public GENERIC_MAPPING GenericMapping;
            /// <summary>
            /// 有效的访问权限
            /// </summary>
            public int ValidAccess;
            /// <summary>
            /// 未知字段
            /// </summary>
            public byte Unknown;
            /// <summary>
            /// 是否维护句柄数据库
            /// </summary>
            public byte MaintainHandleDatabase;
            /// <summary>
            /// 池类型
            /// </summary>
            public int PoolType;
            /// <summary>
            /// 分页池使用量
            /// </summary>
            public int PagedPoolUsage;
            /// <summary>
            /// 非分页池使用量
            /// </summary>
            public int NonPagedPoolUsage;
        }
        /// <summary>
        /// 通用映射结构体
        /// 定义对象类型的标准访问权限映射
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct GENERIC_MAPPING
        {
            /// <summary>
            /// 通用读取权限
            /// </summary>
            public int GenericRead;
            /// <summary>
            /// 通用写入权限
            /// </summary>
            public int GenericWrite;
            /// <summary>
            /// 通用执行权限
            /// </summary>
            public int GenericExecute;
            /// <summary>
            /// 通用所有权限
            /// </summary>
            public int GenericAll;
        }
        /// <summary>
        /// 对象名称信息结构体
        /// 用于存储对象的名称信息（信息类 1）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_NAME_INFORMATION
        {
            /// <summary>
            /// 对象名称
            /// </summary>
            public UNICODE_STRING Name;
        }
        /// <summary>
        /// Unicode字符串结构体
        /// 用于表示Windows内核中的Unicode字符串
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UNICODE_STRING
        {
            /// <summary>
            /// 字符串长度（以字节为单位）
            /// </summary>
            public ushort Length;
            /// <summary>
            /// 最大长度（以字节为单位）
            /// </summary>
            public ushort MaximumLength;
            /// <summary>
            /// 指向字符串缓冲区的指针
            /// </summary>
            public IntPtr Buffer;
        }

    }
}
