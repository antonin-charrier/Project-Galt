# This script builds CodeCakeBuilder with the help of nuget.exe (in Tools/, downloaded if missing)
# and MSBuild.
#
# You may move this bootstrap.ps1 to the solution directory or let it in CodeCakeBuilder folder:
# The $solutionDir and $builderDir are automatically adapted.
#
$solutionDir = $PSScriptRoot
$builderDir = Join-Path $solutionDir "src\CodeCakeBuilder"
if (!(Test-Path $builderDir -PathType Container)) {
    $builderDir = $PSScriptRoot
    $solutionDir = Join-Path $builderDir ".."
}

# Ensures that CodeCakeBuilder project exists.
$builderProj = Join-Path $builderDir "CodeCakeBuilder.xproj"
if (!(Test-Path $builderProj)) {
    Throw "Could not find CodeCakeBuilder.xproj"
}
# Ensures that packages.config file exists.
$builderPackageConfig = Join-Path $builderDir "project.json"
if (!(Test-Path $builderPackageConfig)) {
    Throw "Could not find project.json"
}

# Find MSBuild 4.0.
$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"
$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"
if (!(Test-Path $msbuildExe)) {
    Throw "Could not find msbuild.exe"
}
Invoke-Expression "dotnet restore"
&$msbuildExe $builderProj /p:Configuration=Release