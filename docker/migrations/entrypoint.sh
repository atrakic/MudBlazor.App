#!/usr/bin/env bash
set -exo pipefail
dotnet ef --version
dotnet ef migrations list --no-build
