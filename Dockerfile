FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/*.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY src/. .
RUN dotnet publish --no-restore -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./app"]
