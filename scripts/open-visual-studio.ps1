$ErrorActionPreference = 'Stop'
$daqRoot = Split-Path -Parent $PSScriptRoot
$daqVswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
if (-not (Test-Path -LiteralPath $daqVswhere)) { throw 'Visual Studio Installer was not found.' }
$daqIde = & $daqVswhere -latest -version '[17.11,17.12)' -products Microsoft.VisualStudio.Product.Community `
    -requires Microsoft.VisualStudio.Workload.ManagedDesktop -property productPath
if (-not $daqIde) { throw 'Install Visual Studio 2022 Community 17.11.x with .NET desktop development first.' }
$daqSolution = Join-Path $daqRoot 'Projects.sln'
Start-Process -FilePath $daqIde.Trim() -ArgumentList ('"' + $daqSolution + '"') -WorkingDirectory $daqRoot
