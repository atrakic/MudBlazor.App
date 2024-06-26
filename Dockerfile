
# https://mcr.microsoft.com/product/dotnet/sdk
# https://mcr.microsoft.com/v2/dotnet/sdk/tags/list
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/*.csproj .
RUN dotnet restore -a $TARGETARCH

# copy and publish app and libraries
COPY src/. .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy
LABEL org.opencontainers.image.description="MudBlazor app demo"

RUN set -x \
    && apt-get update \
    && apt-get install --no-install-recommends --no-install-suggests -y curl \
    && apt-get remove --purge --auto-remove -y && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
WORKDIR /app

COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./app"]
