param([switch]$Rebuild)

$ErrorActionPreference = 'Stop'
$daqRoot = Split-Path -Parent $PSScriptRoot
$daqPython = Join-Path $daqRoot '.venv\Scripts\python.exe'
$daqAppDirectory = Join-Path $daqRoot 'src\IndustrialDAQ.UI\bin\Debug\net8.0-windows'
$daqAppExe = Join-Path $daqAppDirectory 'IndustrialDAQ.UI.exe'
$daqLogs = Join-Path $daqRoot 'artifacts\local-run'

if (-not (Test-Path -LiteralPath $daqPython)) {
    throw 'Missing .venv. Create it with Python, then install config\requirements-simulator.txt.'
}

$daqRunningApp = Get-Process -Name 'IndustrialDAQ.UI' -ErrorAction SilentlyContinue |
    Where-Object { $_.Path -eq $daqAppExe } | Select-Object -First 1
if ($Rebuild -and $daqRunningApp) {
    throw 'Close the IndustrialDAQ window before rebuilding.'
}

if ($Rebuild -or -not (Test-Path -LiteralPath $daqAppExe)) {
    & dotnet build (Join-Path $daqRoot 'src\IndustrialDAQ.UI\IndustrialDAQ.UI.csproj') --nologo --verbosity minimal
    if ($LASTEXITCODE -ne 0) { throw 'Building IndustrialDAQ failed.' }
}

# Use a generated configuration beside the executable. Keep the source
# production configuration intact, and derive device/tag definitions from it.
$daqDevices = Get-Content -Raw -Encoding UTF8 (Join-Path $daqRoot 'config\production-line.json') | ConvertFrom-Json
$daqLocalDevices = @($daqDevices | Where-Object {
    $_.driverType -eq 'Modbus' -and
    $_.ipAddress -in @('127.0.0.1', 'localhost', '::1') -and $_.port -eq 502
})
if ($daqLocalDevices.Count -eq 0) { throw 'No loopback Modbus device on port 502 exists in the source configuration.' }
New-Item -ItemType Directory -Path $daqLogs -Force | Out-Null

$daqTcp = New-Object System.Net.Sockets.TcpClient
try {
    $daqConnect = $daqTcp.ConnectAsync('127.0.0.1', 502)
    $daqPortOpen = $daqConnect.Wait(1000) -and $daqTcp.Connected
}
catch { $daqPortOpen = $false }
finally { $daqTcp.Dispose() }

if (-not $daqPortOpen) {
    $env:PYTHONUTF8 = '1'
    $daqSimulatorScript = Join-Path $daqRoot 'config\python_modbus_slave.py'
    $daqSimulator = Start-Process -FilePath $daqPython -ArgumentList '-u', ('"' + $daqSimulatorScript + '"') `
        -WorkingDirectory $daqRoot -WindowStyle Hidden `
        -RedirectStandardOutput (Join-Path $daqLogs 'simulator.stdout.log') `
        -RedirectStandardError (Join-Path $daqLogs 'simulator.stderr.log') -PassThru
    $daqSimulator.Id | Set-Content (Join-Path $daqLogs 'simulator.pid')
    $daqReady = $false
    for ($daqAttempt = 0; $daqAttempt -lt 20; $daqAttempt++) {
        $daqSimulator.Refresh()
        if ($daqSimulator.HasExited) { throw 'Simulator exited. Check artifacts\local-run\simulator.stderr.log.' }
        $daqProbe = New-Object System.Net.Sockets.TcpClient
        try { $daqProbe.Connect('127.0.0.1', 502); $daqReady = $true }
        catch { Start-Sleep -Milliseconds 250 }
        finally { $daqProbe.Dispose() }
        if ($daqReady) { break }
    }
    if (-not $daqReady) { throw 'Simulator did not start listening on 127.0.0.1:502.' }
}

if ($daqRunningApp) {
    Write-Host "IndustrialDAQ is already running (PID $($daqRunningApp.Id))."
    return
}

$daqConfigDirectory = Join-Path $daqAppDirectory 'config'
New-Item -ItemType Directory -Path $daqConfigDirectory -Force | Out-Null
ConvertTo-Json -InputObject $daqLocalDevices -Depth 30 |
    Set-Content -Encoding UTF8 (Join-Path $daqConfigDirectory 'production-line.json')
$daqApp = Start-Process -FilePath $daqAppExe -WorkingDirectory $daqAppDirectory `
    -RedirectStandardOutput (Join-Path $daqLogs 'app.stdout.log') `
    -RedirectStandardError (Join-Path $daqLogs 'app.stderr.log') -PassThru
$daqApp.Id | Set-Content (Join-Path $daqLogs 'app.pid')
Write-Host "IndustrialDAQ started (PID $($daqApp.Id)); local Modbus endpoint: 127.0.0.1:502."
Write-Host 'Logs: artifacts\local-run. Close the app before running with -Rebuild.'
