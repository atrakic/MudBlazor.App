#!/bin/bash

set -exo pipefail

networkName=proxy

## Prepare
if docker network ls | grep -q "${networkName}"
then
    docker network create "${networkName}"
fi

docker compose -f ./init.yml -f ./production.yml pull --ignore-buildable
docker compose -f ./init.yml -f ./production.yml up migrations

## Deploy
docker compose -f ./init.yml -f ./production.yml up app --remove-orphans --no-color -d
# docker exec nginx-proxy-acme /app/force_renew

## Status
docker exec nginx-proxy-acme /app/cert_status
docker exec nginx-proxy-acme cat /etc/nginx/conf.d/default.conf

docker compose -f init.yml -f production.yml top
docker compose -f init.yml -f production.yml stats --no-stream

## Clean up
docker system prune -f
