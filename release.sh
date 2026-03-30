#!/bin/bash

# Release script for Luma API
# Usage: ./release.sh 1.0.12

if [ -z "$1" ]; then
    echo "Error: Version parameter is required"
    echo "Usage: ./release.sh <version>"
    exit 1
fi

VERSION="$1"
PROJECT_PATH="Luma.API.WebApi"
OUTPUT_DIR="publish"
RELEASES_DIR="releases"
VERSION_FILE="version.json"
ZIP_NAME="luma-api-v${VERSION}.zip"
ZIP_PATH="${RELEASES_DIR}/${ZIP_NAME}"

echo "Building and publishing Luma API v${VERSION}..."

# Update version.json
if command -v jq &> /dev/null; then
    # Use jq if available (preferred method)
    jq --arg version "$VERSION" '.apiVersion = $version' "$VERSION_FILE" > "${VERSION_FILE}.tmp" && mv "${VERSION_FILE}.tmp" "$VERSION_FILE"
else
    # Fallback to sed if jq is not available
    sed -i.bak "s/\"apiVersion\":[[:space:]]*\"[^\"]*\"/\"apiVersion\": \"${VERSION}\"/" "$VERSION_FILE"
    rm -f "${VERSION_FILE}.bak"
fi
echo "Updated $VERSION_FILE to v${VERSION}"

# Clean previous publish
if [ -d "$OUTPUT_DIR" ]; then
    rm -rf "$OUTPUT_DIR"
fi

# Create releases folder if not exists
if [ ! -d "$RELEASES_DIR" ]; then
    mkdir -p "$RELEASES_DIR"
fi

# Publish the project
dotnet publish "$PROJECT_PATH" -c Release -o "$OUTPUT_DIR"

if [ $? -ne 0 ]; then
    echo "Publish failed!"
    exit 1
fi

# Remove old zip if exists
if [ -f "$ZIP_PATH" ]; then
    rm "$ZIP_PATH"
fi

# Create zip
cd "$OUTPUT_DIR"
zip -r "../${ZIP_PATH}" . > /dev/null
cd ..

echo "Release created: $ZIP_PATH"

# Clean up publish folder
rm -rf "$OUTPUT_DIR"
echo "Cleaned up $OUTPUT_DIR folder"
