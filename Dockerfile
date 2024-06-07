ARG NET_VERSION=8.0

# https://mcr.microsoft.com/product/dotnet/sdk
# https://mcr.microsoft.com/v2/dotnet/sdk/tags/list
FROM mcr.microsoft.com/dotnet/sdk:${NET_VERSION} AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/*.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY src/. .
RUN dotnet publish --no-restore -o /app

# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
FROM mcr.microsoft.com/dotnet/aspnet:${NET_VERSION} as final
LABEL org.opencontainers.image.source="https://github.com/atrakic/MudBlazor.App.git"
WORKDIR /app
COPY --from=build /app .

RUN set -x \
    && apt-get update \
    && apt-get install --no-install-recommends --no-install-suggests -y curl \
    && apt-get remove --purge --auto-remove -y && rm -rf /var/lib/apt/lists/*

USER $APP_UID
ENV TZ="Etc/UTC"
ENTRYPOINT ["./app"]
