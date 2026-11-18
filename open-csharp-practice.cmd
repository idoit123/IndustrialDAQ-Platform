@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\open-csharp-practice.ps1"
if errorlevel 1 pause
