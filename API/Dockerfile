# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0.301@sha256:faa2daf2b72cbe787ee1882d9651fa4ef3e938ee56792b8324516f5a448f3abe AS build

USER root

WORKDIR /src

COPY . .

WORKDIR /src/API.AppHost


RUN dotnet publish API.AppHost.csproj -c Release -o ./publish

RUN chmod -R 755 /src/API.AppHost/bin/Release/net9.0


# Runtime stage
FROM mcr.microsoft.com/dotnet/sdk:9.0.301@sha256:faa2daf2b72cbe787ee1882d9651fa4ef3e938ee56792b8324516f5a448f3abe AS runtime

WORKDIR /app


COPY --from=build /src ./



ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app/API.AppHost/bin/Release/net9.0

ENTRYPOINT ["dotnet", "API.AppHost.dll"]
