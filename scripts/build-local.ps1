# build-local.ps1
<#
.SYNOPSIS
Builds and packs all CodeYield projects.
.DESCRIPTION
Cleans, builds, and creates NuGet packages in ./nupkg.
#>

$ErrorActionPreference = "Stop"

$SolutionPath = "./CodeYield.slnx"
$OutputDir = "./nupkg"

Write-Host "Cleaning solution..." -ForegroundColor Cyan
dotnet clean $SolutionPath -c Release

Write-Host "Building solution in Release configuration..." -ForegroundColor Cyan
dotnet build $SolutionPath -c Release

Write-Host "Packing all projects..." -ForegroundColor Cyan
dotnet pack $SolutionPath -c Release -o $OutputDir --no-build

Write-Host "Packages ready in $OutputDir" -ForegroundColor Green
Get-ChildItem "$OutputDir/*.nupkg" | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor Gray }
