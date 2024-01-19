#!/bin/bash
set -eo pipefail

curl -o /dev/null -sf -X 'GET' \
  'http://localhost:80/healthz' || exit
