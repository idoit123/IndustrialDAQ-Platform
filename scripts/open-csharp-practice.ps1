$ErrorActionPreference = 'Stop'
$practiceRoot = Split-Path -Parent $PSScriptRoot
$practiceVswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
if (-not (Test-Path -LiteralPath $practiceVswhere)) { throw 'Visual Studio Installer was not found.' }
$practiceIde = & $practiceVswhere -latest -version '[17.11,17.12)' -products Microsoft.VisualStudio.Product.Community `
    -requires Microsoft.VisualStudio.Workload.ManagedDesktop -property productPath
if (-not $practiceIde) { throw 'Visual Studio 2022 Community 17.11.x with .NET desktop development is required.' }
$practiceSolution = Join-Path $practiceRoot 'sandbox\CSharpPractice\CSharpPractice.sln'
if (-not (Test-Path -LiteralPath $practiceSolution)) { throw 'The CSharpPractice solution was not found.' }
Start-Process -FilePath $practiceIde.Trim() -ArgumentList ('"' + $practiceSolution + '"') -WorkingDirectory $practiceRoot
