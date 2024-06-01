#!/bin/bash

set -exo pipefail

networkName=proxy

## Prepare
if docker network ls | grep -q "${networkName}"
then
    docker network create "${networkName}"
fi

## Build
docker compose -f ./init.yml -f ./production.yml pull --ignore-buildable
docker compose -f ./init.yml -f ./production.yml up migrations --remove-orphans

## Deploy
docker compose -f ./init.yml -f ./production.yml up app --remove-orphans --no-color -d
# docker exec nginx-proxy-acme /app/force_renew

## Status
docker exec nginx-proxy-acme /app/cert_status
docker exec nginx-proxy-acme cat /etc/nginx/conf.d/default.conf

docker compose -f init.yml -f production.yml top
docker compose -f init.yml -f production.yml stats --no-stream

#docker exec db \
#    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${MSSQL_SA_PASSWORD}" \
#    -Q "SELECT TOP(5) [MigrationId],[ContextKey],[ProductVersion]
#    FROM [__EFMigrationsHistory]
#    ORDER BY [MigrationId];" -b

#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_activity;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_replication;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_wal_receiver;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_subscription;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_database_conflicts;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_bgwriter;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_database;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_user_tables;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_user_indexes;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_user_functions;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_statements;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_vacuum;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_cluster;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_create_index;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_analyze;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_basebackup;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_copy;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_cluster;"
#docker exec db psql -U postgres -c "SELECT * FROM pg_stat_progress_vacuum;"

## Clean up
docker system prune -f
