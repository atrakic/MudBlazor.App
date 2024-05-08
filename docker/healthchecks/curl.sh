#!/bin/bash
set -eo pipefail
curl -sf -X 'GET' http://localhost:8000/healthz || exit 1
