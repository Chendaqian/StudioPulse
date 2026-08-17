[![.NET Framework 4.7.2](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)](https://dotnet.microsoft.com/download/dotnet-framework)
[![WPF](https://img.shields.io/badge/UI-WPF-purple)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
[![Windows](https://img.shields.io/badge/Platform-Windows-0078d4)](https://www.microsoft.com/windows)
[![GitHub Release](https://img.shields.io/github/v/release/Chendaqian/StudioPulse?label=Release)](https://github.com/Chendaqian/StudioPulse/releases/latest)
[![Build Status](https://img.shields.io/github/actions/workflow/status/Chendaqian/StudioPulse/publish.yml?label=Build)](https://github.com/Chendaqian/StudioPulse/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/Chendaqian/StudioPulse?style=flat)](https://github.com/Chendaqian/StudioPulse/stargazers)
[![GitHub Downloads](https://img.shields.io/github/downloads/Chendaqian/StudioPulse/total?style=flat)](https://github.com/Chendaqian/StudioPulse/releases/latest)
[![GitHub Last Commit](https://img.shields.io/github/last-commit/Chendaqian/StudioPulse?style=flat)](https://github.com/Chendaqian/StudioPulse/commits/master)


# StudioPulse

<div align="center">
  ![icon](https://raw.githubusercontent.com/Chendaqian/StudioPulse/refs/heads/master/src/StudioPulse/Resources/icon.png)
  ![info](https://raw.githubusercontent.com/Chendaqian/StudioPulse/refs/heads/master/src/StudioPulse/Resources/info.png)
</div>

[English document](https://github.com/Chendaqian/StudioPulse/blob/master/README.en.md)

StudioPulse 是一个 Visual Studio 扩展，在状态栏显示当前 Visual Studio 进程和整台计算机的 CPU、内存使用情况。

## 功能

- 显示当前 Visual Studio 进程的 CPU 使用率
- 显示当前 Visual Studio 进程的内存使用量和百分比
- 显示整台计算机的 CPU 使用率
- 显示可用内存、已用内存和对应百分比
- 支持自定义显示格式、刷新间隔和固定宽度
- 支持根据负载显示渐变颜色

## 支持环境

- Visual Studio 2019（16.x，32 位）
- Visual Studio 2022（17.x，64 位）
- Visual Studio 2026（18.x，64 位）
- .NET Framework 4.7.2 或更高版本
- Windows

## 安装

关闭 Visual Studio，双击发布目录中的 `StudioPulse.vsix`，在 VSIX Installer 中选择要安装的 Visual Studio 实例。安装完成后重新启动 Visual Studio，指标会显示在窗口底部状态栏右侧。

## 配置

打开“工具 → 选项 → StatusBar Info → General”，可以修改刷新间隔、显示格式和固定宽度。

支持的格式占位符：

| 占位符 | 含义 |
| --- | --- |
| `<CPU>` / `<#CPU>` | Visual Studio 进程 CPU 使用率 |
| `<TOTAL_CPU>` / `<#TOTAL_CPU>` | 全机 CPU 使用率 |
| `<RAM>` / `<#RAM>` | Visual Studio 进程内存 |
| `<RAM%>` / `<#RAM%>` | Visual Studio 进程内存百分比 |
| `<FREE_RAM>` / `<#FREE_RAM>` | 全机可用内存 |
| `<FREE_RAM%>` / `<#FREE_RAM%>` | 全机可用内存百分比 |
| `<TOTAL_USE_RAM>` / `<#TOTAL_USE_RAM>` | 全机已用内存 |
| `<TOTAL_USE_RAM%>` / `<#TOTAL_USE_RAM%>` | 全机已用内存百分比 |

带 `#` 的占位符会根据负载显示渐变颜色。

## 构建与发布

使用 VS2022 Developer PowerShell：

```powershell
$ErrorActionPreference = 'Stop'
msbuild src\StudioPulse.sln /restore /p:Configuration=Release
```

也可以运行发布脚本：

```powershell
$ErrorActionPreference = 'Stop'
& .\scripts\Publish.ps1
```

发布产物为：

```text
publish\StudioPulse.vsix
```

## 已知限制

状态栏容器是 Visual Studio 的内部 WPF 控件，扩展通过可视树中的 `StatusBarContainer` 定位它。Visual Studio 后续版本若调整内部控件结构，可能需要更新状态栏适配代码。

## Contributors

<a href="https://github.com/Chendaqian/StudioPulse/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Chendaqian/StudioPulse" />
</a>

## Star History

<a href="https://www.star-history.com/?repos=Chendaqian%2FStudioPulse&type=date&legend=top-left">
 <picture>
   <source media="(prefers-color-scheme: dark)" srcset="https://api.star-history.com/chart?repos=Chendaqian/StudioPulse&type=date&theme=dark&legend=top-left" />
   <source media="(prefers-color-scheme: light)" srcset="https://api.star-history.com/chart?repos=Chendaqian/StudioPulse&type=date&legend=top-left" />
   <img alt="Star History Chart" src="https://api.star-history.com/chart?repos=Chendaqian/StudioPulse&type=date&legend=top-left" />
 </picture>
</a>
