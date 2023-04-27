#!/usr/bin/env bash

set -euo pipefail
shopt -s inherit_errexit

SCRIPT_DIRECTORY=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)

DOTNET_CONFIGURATION="Debug"
DOTNET_FRAMEWORK="net6.0"

rm \
    --recursive \
    --force \
    "${SCRIPT_DIRECTORY}/Core/Microsoft.DataTransfer.Core/bin/${DOTNET_CONFIGURATION}"

dotnet restore
dotnet build \
    "${SCRIPT_DIRECTORY}/CosmosDbDataMigrationTool.sln" \
    --no-restore \
    --configuration "${DOTNET_CONFIGURATION}" \
    --framework "${DOTNET_FRAMEWORK}" \
    --maxcpucount:1

cd "${SCRIPT_DIRECTORY}/Core/Microsoft.DataTransfer.Core/bin/${DOTNET_CONFIGURATION}/${DOTNET_FRAMEWORK}"

# Launch Settings
# https://github.com/Azure/azure-documentdb-datamigrationtool/blob/48b5f42efa6df8b0003d158dc94be02bad110f89/Core/Microsoft.DataTransfer.Core/Properties/launchSettings.json

# Echo to emulate press Enter, else program does not quit automatically.
# https://github.com/Azure/azure-documentdb-datamigrationtool/blob/48b5f42efa6df8b0003d158dc94be02bad110f89/Core/Microsoft.DataTransfer.Core/Program.cs#L50

# TODO: Enable following lines to run the tool.
# echo | dotnet \
#     Microsoft.DataTransfer.Core.dll \
#     --source cosmos \
#     --sink json \
#     --SourceSettingsPath="CosmosSourceSettings.json" \
#     --SinkSettingsPath="JsonSinkSettings.json"

cd -
