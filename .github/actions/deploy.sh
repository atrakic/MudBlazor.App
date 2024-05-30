#!/bin/bash

set -exo pipefail

## Prepare
docker compose -f ./init.yml -f ./production.yml pull --ignore-buildable

## Deploy
docker compose -f ./init.yml -f ./production.yml up --pull always --remove-orphans --no-color -d
# docker compose -f ./init.yml -f ./production.yml exec -T app python manage.py migrate --noinput

## Status
docker exec nginx-proxy-acme /app/cert_status
docker exec nginx-proxy-acme cat /etc/nginx/conf.d/default.conf

## Clean up
docker system prune -f
