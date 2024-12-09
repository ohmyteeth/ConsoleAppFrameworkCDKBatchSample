FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# docker-entrypoint.shで使う
RUN apt-get update && apt-get install curl -y

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Batch.csproj", "."]
RUN dotnet restore "./Batch.csproj"

COPY . .
RUN dotnet build "./Batch.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Batch.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["/app/docker-entrypoint.sh"]