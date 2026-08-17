# StudioPulse Agent Guide

## 项目概述

StudioPulse 是一个 Visual Studio VSIX 扩展，在 Visual Studio 状态栏显示当前 VS 进程和整台计算机的 CPU、内存使用情况。

- C# 7.3
- .NET Framework 4.7.2
- WPF
- SDK 风格项目文件：`src/StudioPulse/StudioPulse.csproj`
- VSSDK 包：`Microsoft.VisualStudio.Shell.15.0` 和 `Microsoft.VSSDK.BuildTools`
- 主要命名空间：`StudioPulse`

## 构建

构建需要使用 Visual Studio 2022 提供的 MSBuild，并在 PowerShell 7 中执行：

```powershell
$ErrorActionPreference = 'Stop'
msbuild src\StudioPulse.sln /restore /p:Configuration=Release
```

发布脚本：

```powershell
$ErrorActionPreference = 'Stop'
& .\scripts\Publish.ps1
```

脚本会构建 Release，并将 VSIX 复制到 `publish\StudioPulse.vsix`。构建生成的清单位于 `obj` 或 `bin`，不要手动修改 `extension.vsixmanifest`。

## 目录

- `src/StudioPulse`：源代码、WPF 控件、VSIX 清单和 VSPackage 资源
- `scripts/Publish.ps1`：Release 构建和 VSIX 发布脚本
- `publish`：发布脚本生成的最终 VSIX 目录
- `src/StudioPulse/source.extension.vsixmanifest`：唯一需要维护的 VSIX 源清单
- `src/StudioPulse/VSPackage.resx`：Visual Studio 包注册使用的资源，ID 110 和 112 被 `InstalledProductRegistration` 引用

## 修改约束

- `source.extension.vsixmanifest` 的 `Identity Id` 必须是合法 GUID，并与 `StatusInfoIdentifiers.PackageId` 保持一致
- GitHub 地址放在 `MoreInfo`，不能放到 `Identity Id`
- `extension.vsixmanifest` 是构建生成文件，不要在源目录中保留副本
- `VSPackage.resx` 的 110、112 资源不能随意删除或改名
- SDK 风格项目自动包含源文件，新增文件通常不需要手动添加 `<Compile Include>`
- `Resources.resx` 和 `Resources.Designer.cs` 已删除，新增资源前先确认确实有运行时需求
- 修改 VSIX 版本时，确认清单版本、程序集版本和发布策略是否需要同步
