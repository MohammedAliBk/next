# -------------------------------
# please first make sure u did
# 
# --Set-ExecutionPolicy Bypass -Scope Process -Force
# .\install-docker.ps1
-----------------------------

Write-Host "Checking WSL..."

wsl --install
wsl --set-default-version 2

$dockerInstaller = "$env:TEMP\DockerDesktopInstaller.exe"

Write-Host "Downloading Docker Desktop..."
Invoke-WebRequest `
  -Uri "https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe" `
  -OutFile $dockerInstaller

Write-Host "Installing Docker Desktop..."
Start-Process $dockerInstaller `
  -Wait `
  -ArgumentList "install", "--quiet"

Write-Host "Starting Docker Desktop..."
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"

Write-Host "Docker installation completed."

