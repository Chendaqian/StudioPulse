# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

StatusInfo 是一个 Visual Studio 扩展（VSIX），在 VS 状态栏显示当前 VS 实例的 CPU 占用与内存占用，以及全机 CPU/内存信息。

- .NET Framework 4.5，C# 7.3，老式（非 SDK 风格）csproj
- 依赖 VSSDK 12（`VSSDK.Shell.12` / `VSSDK.GraphModel`），支持 VS 2013–2019（vsixmanifest 中 `[12.0,16.0)`）
- 命名空间统一为 `Lkytal.StatusInfo`，程序集已发布到 VS Marketplace（Publisher: Lkytal）

## 构建与调试

- 构建必须使用 VS2022 自带的 MSBuild（`Microsoft.VsSDK.targets` 依赖 `VSToolsPath`），建议在 Developer PowerShell 中执行：

```powershell
$ErrorActionPreference = 'Stop'
msbuild StatusInfo.sln /p:Configuration=Release
```

- 产物：`bin\Release\StatusInfo.vsix`（注意 .gitignore 忽略了 *.vsix）
- 调试：VS2022 打开 `StatusInfo.sln` 后按 F5，VSSDK 会自动启动 VS 实验实例（`/rootsuffix Exp`）
- 无测试项目、无 lint 配置；Release 配置启用了 CodeAnalysis

- **发版时三处版本号必须同步**：`StatusInfo.csproj` 的 `<AssemblyVersion>`、`Properties\AssemblyInfo.cs`、`source.extension.vsixmanifest` 的 `Identity Version`（三处当前并不一致，改哪边前先确认）

## 架构

### 启动链

`StatusInfoPackage`（VS 包入口，三种 UIContext 全部 AutoLoad）→ `Initialize()` 挂 DTE 事件 → `OnStartupComplete` 触发 `InitExt()`：

1. 创建 `System.Timers.Timer`（默认 1000ms）定时采样
2. 数据来源：
   - 本进程 CPU：`Ext.cs` 的 `ProcessExtension.GetCpuUsage()`（基于 `TotalProcessorTime` 增量自行计算，**不是** PerformanceCounter）
   - 本进程内存：`Process.WorkingSet64`
   - 全机 CPU / 可用内存：`PerformanceCounter`（`Processor\% Processor Time\_Total`、`Memory\Available Bytes`）
3. `UpdateInfoBar()` 通过 `Dispatcher.BeginInvoke` 切回 UI 线程，只写 `InfoControl` 的四个 set-only 属性：`CpuUsage` / `RamUsage` / `TotalCpuUsage` / `FreeRam`

### 状态栏注入（脆弱点）

`StatusBarInjector` 在 VS 主窗口可视树中按名字 **`"StatusBarContainer"`** 查找状态栏，取其父 `DockPanel`，把 `InfoControl` 以 `Dock.Right` 插入到 index 1。这依赖 VS 内部控件命名，VS 大版本升级后若失效优先怀疑这里。

### 格式化显示（核心机制）

`InfoControl` 按 `Format` 字符串中的占位符 token 动态生成 TextBlock 序列：

- 8 种指标（见构造函数中 `Formats` 数组：`CPU`、`TOTAL_CPU`、`RAM`、`FREE_RAM`、`TOTAL_USE_RAM` 及三个 `%` 变体），每种有 `<NAME>`（白色）和 `<#NAME>`（按负载 白→黄→红 渐变着色，`ColorExtension.FadeTo`）两种写法
- `Format` setter 用 `Ext.cs` 的 `StringExtension.IndexOfAny(string[], out string)` 切分字符串；同一 token 可出现多次，由 `TextBlockList`（`List<TextBlock>`）统一广播 `Text` / `Foreground`
- **新增指标**需三步：`Formats`/`FormatDescriptions` 数组加项（token 键由 `InitTextBlockLists` 自动生成）→ 在对应属性 setter 中更新 `textBlockLists["<NEW>"]` 和 `textBlockLists["<#NEW>"]` → 属性 setter 假设所有 token 键恒存在
- 默认格式：`"CPU: <#CPU>   RAM: <#RAM>"`

### 选项页

`OptionsPage`（工具 → 选项 → StatusBar Info → General）：`Format`、`Interval`(ms)、`UseFixedWidth`、`FixedWidth`。即时生效机制：属性 setter → `OptionUpdated(字符串名, 值)` → `GetService` 取回 `StatusInfoPackage.OptionUpdated()` 的 switch 分发。**新增选项需同时改两处**：`OptionsPage` 的属性 + `StatusInfoPackage.OptionUpdated` 的 switch 分支（按字符串名匹配，拼错只会静默走 default 分支打 Debug 日志）。

已知笔误（勿随手"修正"）：`OptionsPage` 的属性名是 `FixedWith`（少了 t），但 DisplayName 和传给 `OptionUpdated` 的键都是 `"FixedWidth"`；直接改属性名会破坏用户已保存的选项值。

### Ext.cs

跨命名空间的扩展方法集合，注意它把扩展方法放进了 BCL 命名空间（`System`、`System.Diagnostics`、`System.Windows.Media`），改签名影响面大；其中 `ProcessExtension` 用静态 Dictionary 按进程 ID 缓存 CPU 采样基线。

## 项目约束

- 老式 csproj：**新增 .cs 文件必须手动添加 `<Compile Include>`**（父级 AGENTS.md 已有此规则）
- `Guids.cs` 的 `guidStatusInfoPkgString` 与 vsixmanifest 的 `Identity Id` 是同一个 GUID（`72581eb6-...`），不能单独改其中一处
- `Resources.Designer.cs` 由 `Resources.resx` 自动生成，不要手改
- `VSPackage.resx` 中资源 ID 110/112 被 `InstalledProductRegistration` 特性引用，ID 400 是图标
