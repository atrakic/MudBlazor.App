# !/bin/bash
set -e

## https://www.oracle.com/java/technologies/downloads/#jdk22-mac

key="${SONAR_KEY:-}" # sqp_xxx
project="${SONAR_PROJ:-foo}"
url="${SONAR_URL:-http://localhost:9000}"

# dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"${project}" /d:sonar.host.url="${url}" /d:sonar.login="${key}"
dotnet build
dotnet sonarscanner end /d:sonar.login="${key}"
