# Unity Project Cache Cleaner
# This script will delete Unity's Library folder to force a clean recompilation

param(
    [Parameter(Mandatory = $false)]
    [string]$ProjectPath = "D:\PRU213\FPT-Student-Simulator"
)

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Unity Cache Cleaner" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

$libraryPath = Join-Path $ProjectPath "Library"
$tempPath = Join-Path $ProjectPath "Temp"

# Check if Unity is running
$unityProcesses = Get-Process -Name "Unity" -ErrorAction SilentlyContinue

if ($unityProcesses) {
    Write-Host "[!] WARNING: Unity is currently running!" -ForegroundColor Yellow
    Write-Host "Please close Unity before running this script." -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Project Path: $ProjectPath" -ForegroundColor Green
Write-Host ""

# Delete Library folder
if (Test-Path $libraryPath) {
    Write-Host "[*] Deleting Library folder..." -ForegroundColor Yellow
    Remove-Item -Path $libraryPath -Recurse -Force
    Write-Host "[OK] Library folder deleted" -ForegroundColor Green
}
else {
    Write-Host "[i] Library folder not found (already clean)" -ForegroundColor Cyan
}

# Delete Temp folder
if (Test-Path $tempPath) {
    Write-Host "[*] Deleting Temp folder..." -ForegroundColor Yellow
    Remove-Item -Path $tempPath -Recurse -Force
    Write-Host "[OK] Temp folder deleted" -ForegroundColor Green
}
else {
    Write-Host "[i] Temp folder not found" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "[OK] Cache cleared successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Open Unity" -ForegroundColor White
Write-Host "2. Wait for Unity to reimport all assets" -ForegroundColor White
Write-Host "3. Check that compilation errors are resolved" -ForegroundColor White
Write-Host ""
Read-Host "Press Enter to exit"
