
# StatusInfo

StatusInfo 是一个 Visual Studio 扩展，在状态栏显示当前 Visual Studio 进程和整台计算机的 CPU、内存使用情况。

## 支持版本

- Visual Studio 2019（16.x，32 位）
- Visual Studio 2022（17.x，64 位）
- Visual Studio 2026（18.x，64 位）
- .NET Framework 4.7.2 或更高版本

## 安装

关闭 Visual Studio，双击 `bin\\Release\\StatusInfo.vsix`，在 VSIX Installer 中选择要安装的 Visual Studio 实例。安装完成后重新启动 Visual Studio，指标会显示在窗口底部状态栏右侧。

## 配置

打开“工具 → 选项 → StatusBar Info → General”，可以修改刷新间隔、显示格式和固定宽度。

格式占位符：

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

## 构建

使用 VS2022 Developer PowerShell：

```powershell
$ErrorActionPreference = 'Stop'
msbuild StatusInfo.sln /restore /p:Configuration=Release
```

输出文件为 `bin\\Release\\StatusInfo.vsix`。

## 已知限制

状态栏容器是 Visual Studio 的内部 WPF 控件，扩展通过可视树中的 `StatusBarContainer` 定位它。Visual Studio 后续版本若调整内部控件结构，可能需要更新状态栏适配代码。
