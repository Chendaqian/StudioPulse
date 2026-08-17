# StatusInfo

StatusInfo is a Visual Studio extension that displays CPU and memory usage for the current Visual Studio process and the whole computer in the status bar.

## Supported versions

- Visual Studio 2019 (16.x, 32-bit)
- Visual Studio 2022 (17.x, 64-bit)
- Visual Studio 2026 (18.x, 64-bit)
- .NET Framework 4.7.2 or later

## Installation

Close Visual Studio and double-click `bin\\Release\\StatusInfo.vsix`. Select the Visual Studio instances in VSIX Installer, then start Visual Studio again. The metrics appear on the right side of the bottom status bar.

## Configuration

Open `Tools -> Options -> StatusBar Info -> General` to change the refresh interval, display format, and fixed width.

Format tokens:

| Token | Meaning |
| --- | --- |
| `<CPU>` / `<#CPU>` | CPU usage of the Visual Studio process |
| `<TOTAL_CPU>` / `<#TOTAL_CPU>` | Total computer CPU usage |
| `<RAM>` / `<#RAM>` | Memory used by the Visual Studio process |
| `<RAM%>` / `<#RAM%>` | Visual Studio process memory percentage |
| `<FREE_RAM>` / `<#FREE_RAM>` | Available computer memory |
| `<FREE_RAM%>` / `<#FREE_RAM%>` | Available computer memory percentage |
| `<TOTAL_USE_RAM>` / `<#TOTAL_USE_RAM>` | Used computer memory |
| `<TOTAL_USE_RAM%>` / `<#TOTAL_USE_RAM%>` | Used computer memory percentage |

Tokens prefixed with `#` use a load-based color gradient.

## Build

Run the following command from a VS2022 Developer PowerShell:

```powershell
$ErrorActionPreference = 'Stop'
msbuild StatusInfo.sln /restore /p:Configuration=Release
```

The output is `bin\\Release\\StatusInfo.vsix`.

## Known limitation

The status bar container is an internal Visual Studio WPF control. The extension locates it through the visual tree using `StatusBarContainer`. Future Visual Studio versions may change this internal structure and require an adapter update.
