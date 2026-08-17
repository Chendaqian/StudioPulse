$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $repoRoot 'src\StudioPulse.sln'
$projectDirectory = Join-Path $repoRoot 'src\StudioPulse'
$publishDirectory = Join-Path $repoRoot 'publish'

$msbuildCandidates = @(
    'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
    'C:\Program Files\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe'
)

$msbuildPath = $msbuildCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $msbuildPath) {
    throw '未找到 VS2022 MSBuild，请安装 Visual Studio 的 MSBuild 或修改脚本中的路径'
}

if (-not (Test-Path -LiteralPath $solutionPath)) {
    throw "未找到解决方案文件: $solutionPath"
}

Write-Host '开始构建 StudioPulse Release...' -ForegroundColor Cyan
& $msbuildPath $solutionPath /p:Configuration=Release /m
if ($LASTEXITCODE -ne 0) {
    throw "构建失败，MSBuild 退出码: $LASTEXITCODE"
}

$vsixCandidates = @(
    (Join-Path $projectDirectory 'bin\Release\net472\StudioPulse.vsix'),
    (Join-Path $projectDirectory 'bin\Release\StudioPulse.vsix')
)
$vsixPath = $vsixCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $vsixPath) {
    throw "构建成功但未找到 VSIX 产物，已检查: $($vsixCandidates -join ', ')"
}

New-Item -ItemType Directory -Path $publishDirectory -Force | Out-Null
$publishedVsixPath = Join-Path $publishDirectory 'StudioPulse.vsix'
Copy-Item -LiteralPath $vsixPath -Destination $publishedVsixPath -Force

Write-Host ''
Write-Host '发布完成:' -ForegroundColor Green
Write-Host $publishedVsixPath -ForegroundColor Green
Write-Host ''
Read-Host '按 Enter 键退出'
