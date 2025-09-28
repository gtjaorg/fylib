# 🚀 FyLib - 现代化 C# 工具库

[![NuGet Version](https://img.shields.io/nuget/v/FyLib?style=flat-square)](https://www.nuget.org/packages/FyLib)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue?style=flat-square)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)
[![AOT Compatible](https://img.shields.io/badge/AOT-Compatible-brightgreen?style=flat-square)]()

**FyLib** 是一个功能丰富、性能卓越的现代化 C# 工具库，专为 .NET 10.0 设计，充分利用了最新的 C# 语言特性，为开发者提供高效、易用的常用功能集合。

## ✨ 核心优势

### 🔥 **采用最新技术栈**
- **.NET 10.0** 专属设计，充分利用最新性能优化
- **扩展类型（Extension Types）** 语法，提供更自然的 API 体验
- **AOT 兼容**，支持原生编译，启动更快、内存占用更少
- **可空引用类型**，提供更安全的类型系统

### ⚡ **极致性能优化**
- 使用 `AsSpan()` 和现代字符串操作，减少内存分配
- HTTP 客户端连接池，提升网络请求性能
- 字节池管理，优化内存使用
- 零分配的哈希计算和字符串处理

### 🛠️ **全面功能覆盖**
- **10+ 核心模块**，涵盖开发常见需求
- **60+ 实用方法**，开箱即用
- **完善的类型扩展**，让代码更简洁优雅
- **企业级功能**，满足复杂业务场景

## 🎯 功能特性

### 🔐 **安全加密模块**
```csharp
// 支持多种哈希算法：MD5、SHA1、SHA256、SHA512、CRC32
string hash = "hello world".MD5;           // 直接属性访问
string sha256 = HashHelper.sha256("data"); // 工业级安全
```

### 🌐 **现代 HTTP 客户端**
```csharp
// 链式调用，优雅简洁
var result = await "https://api.example.com"
    .ToQuickHttp()
    .SetTimeout(5000)
    .SetProxy("127.0.0.1", 8080)
    .AddHeader("Authorization", "Bearer token")
    .PostJsonAsync(data);
```

### 🚀 **强大字符串扩展**
```csharp
// .NET 10.0 扩展类型语法，如同原生方法
string text = "  Hello World  ";
bool isNum = text.IsNumeric;        // 数值验证
bool isIp = "192.168.1.1".IsIp;     // IP格式验证
string clean = text.Filter;         // 过滤空白字符
byte[] bytes = "48656C6C6F".ToBytes(); // 十六进制转换
```

### ⏰ **智能时间处理**
```csharp
// 丰富的时间戳处理功能
var now = TimeHelper.TimeStamp();                    // UTC时间戳
var millis = TimeHelper.TimeStampX();               // 毫秒时间戳
var tomorrow7AM = TimeHelper.GetTimestampAtSpecificTime(1, "07:00");
bool inRange = TimeHelper.IsTimeInRange("09:00-18:00");
```

### 📦 **高效数据打包**
```csharp
// 类型安全的数据序列化
using var pack = new Pack();
pack.push(123);           // int
pack.push("hello");       // string  
pack.push<byte>(255);     // 泛型支持
var data = pack.Get();    // 获取字节数组
```

### 🔧 **实用工具集合**
```csharp
// 网络工具
bool online = Other.Ping("google.com", 3000);
string mac = Other.GetRandMac();

// 文件操作
var fileInfo = PathHelper.GetFileInfo("/path/to/file");
var content = FileExtensions.ReadAllText("config.txt");

// 系统信息
var process = RemoteProcess.GetProcessById(1234);
var ipInfo = IPHelper.GetLocalIP();
```

## 📋 完整功能列表

| 模块 | 功能描述 | 亮点特性 |
|-----|---------|----------|
| 🔐 **HashHelper** | MD5/SHA/CRC32 等哈希计算 | 支持字符串和字节数组，性能优化 |
| 🌐 **QuickHttp** | HTTP 客户端封装 | 连接池、Gzip、代理、链式调用 |
| ⏰ **TimeHelper** | 时间戳和日期处理 | UTC/本地时间转换、范围判断 |
| 📝 **StringExtension** | 字符串扩展方法 | .NET 10 语法、20+ 验证和转换方法 |
| 📦 **Pack/UnPack** | 数据序列化 | 类型安全、高性能二进制打包 |
| 🔧 **Other** | 系统和网络工具 | Ping、MAC地址、IMEI生成 |
| 📁 **FileExtensions** | 文件操作扩展 | 路径处理、文件读写 |
| 📋 **IniHelper** | INI 配置文件 | 配置文件读写和管理 |
| 📊 **IPHelper** | 网络IP工具 | IP地址获取和验证 |
| 🖥️ **API** | Windows API 封装 | Kernel32、User32、ntdll 等 |

## 🚀 快速开始

### 安装
```bash
# NuGet 包管理器
Install-Package FyLib

# .NET CLI
dotnet add package FyLib

# PackageReference
<PackageReference Include="FyLib" Version="2.2.0" />
```

### 基础使用
```csharp
using FyLib;
using FyLib.Http;

// 字符串处理
string data = "Hello123";
if (data.IsNumeric) 
{
    Console.WriteLine($"MD5: {data.MD5}");
}

// HTTP 请求
var response = await "https://httpbin.org/get"
    .ToQuickHttp()
    .SetTimeout(5000)
    .GetAsync();

// 时间处理
var timestamp = TimeHelper.TimeStamp();
var dateStr = Other.TimeStampToString(timestamp);

// 哈希计算
string hash = HashHelper.sha256("important data");
```

## 🎯 适用场景

- ✅ **Web API 开发** - HTTP 客户端和数据处理
- ✅ **数据处理** - 字符串解析和格式验证  
- ✅ **系统工具** - 文件操作和系统信息获取
- ✅ **网络编程** - TCP/UDP 数据包处理
- ✅ **安全相关** - 数据加密和哈希验证
- ✅ **配置管理** - INI 文件和参数处理
- ✅ **性能敏感应用** - AOT 编译优化支持

## ⚙️ 系统要求

- **.NET 10.0** 或更高版本
- **C# 13.0** 语言特性支持
- 支持 **Windows**、**Linux**、**macOS**
- **AOT** 原生编译兼容

## 🤝 贡献指南

我们欢迎社区贡献！请查看 [贡献指南](CONTRIBUTING.md) 了解如何参与项目开发。

## 📄 许可证

Copyright © 枫影傲然 2024 - [MIT License](LICENSE)

## 🔗 相关链接

- 📦 [NuGet 包](https://www.nuget.org/packages/FyLib)
- 🐛 [问题反馈](https://github.com/gtjaorg/fylib/issues)  
- 💡 [功能建议](https://github.com/gtjaorg/fylib/discussions)
- 📖 [完整文档](https://github.com/gtjaorg/fylib/wiki)

---

⭐ 如果这个项目对你有帮助，请给个 Star 支持一下！
