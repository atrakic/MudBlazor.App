#!/bin/bash

set -exo pipefail

## Prepare
# docker compose -f ./init.yml -f ./production.yml pull --ignore-buildable

## Deploy
docker compose -f ./init.yml -f ./production.yml up --pull always --remove-orphans --no-color -d
# docker exec nginx-proxy-acme /app/force_renew

## Status
docker exec nginx-proxy-acme /app/cert_status
docker exec nginx-proxy-acme cat /etc/nginx/conf.d/default.conf

docker compose -f init.yml -f production.yml top
docker compose -f init.yml -f production.yml stats --no-stream

## Clean up
docker system prune -f
