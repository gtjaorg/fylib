# FyLib

FyLib 是一个包含各种常用功能的 C# 工具库，旨在提供开发者在日常开发中经常使用的实用功能。

## 功能特性

- **哈希计算** - 提供各种哈希算法的便捷方法
- **HTTP 请求** - 简化 HTTP 客户端操作，支持连接池和 Gzip 压缩
- **时间处理** - 时间格式化和计算的实用工具
- **文件操作** - 文件系统相关的扩展方法
- **字符串扩展** - 字符串处理的扩展方法
- **字节数组操作** - 字节数组处理和转换
- **日志记录** - 简单的日志记录功能
- **网络工具** - IP 地址和网络相关的工具
- **INI 文件处理** - INI 配置文件的读写操作

## 安装

通过 NuGet 安装：

```
Install-Package FyLib
```

或者使用 .NET CLI：

```
dotnet add package FyLib
```

## 使用示例

### 哈希计算
```csharp
using FyLib;

string hash = HashHelper.MD5("hello world");
```

### HTTP 请求
```csharp
using FyLib.Http;

var response = await QuickHttp.GetAsync("https://api.example.com/data");
```

### 时间处理
```csharp
using FyLib;

string formattedTime = TimeHelper.FormatTime(DateTime.Now);
```

## 系统要求

- .NET 10.0 或更高版本

## 许可证

Copyright © 枫影傲然 2024

## 贡献

欢迎提交 Issue 和 Pull Request 来改进这个库。
