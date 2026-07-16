#!/usr/bin/env bash
set -e
set -u

SOLUTION_PATH="./CodeYield.slnx"
OUTPUT_DIR="./nupkg"

echo "Cleaning solution..."
dotnet clean "$SOLUTION_PATH" -c Release

echo "Building solution in Release configuration..."
dotnet build "$SOLUTION_PATH" -c Release

echo "Packing all projects..."
dotnet pack "$SOLUTION_PATH" -c Release -o "$OUTPUT_DIR" --no-build

echo "Build and pack completed. Packages are in $OUTPUT_DIR"
ls -1 "$OUTPUT_DIR"/*.nupkg
