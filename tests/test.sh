#!/usr/bin/env bash

set -eo pipefail

DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

declare -a opts

opts=(
  -sf
  -X 'GET'
)

curl "${opts[@]}" http://localhost:8080/healthz
