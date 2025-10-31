# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OmegaFY.Chat.API.WebAPI/OmegaFY.Chat.API.WebAPI.csproj", "src/OmegaFY.Chat.API.WebAPI/"]
COPY ["src/OmegaFY.Chat.API.Application/OmegaFY.Chat.API.Application.csproj", "src/OmegaFY.Chat.API.Application/"]
COPY ["src/OmegaFY.Chat.API.Domain/OmegaFY.Chat.API.Domain.csproj", "src/OmegaFY.Chat.API.Domain/"]
COPY ["src/OmegaFY.Chat.API.Common/OmegaFY.Chat.API.Common.csproj", "src/OmegaFY.Chat.API.Common/"]
COPY ["src/OmegaFY.Chat.API.Infra/OmegaFY.Chat.API.Infra.csproj", "src/OmegaFY.Chat.API.Infra/"]
COPY ["src/OmegaFY.Chat.API.Data.Dapper/OmegaFY.Chat.API.Data.Dapper.csproj", "src/OmegaFY.Chat.API.Data.Dapper/"]
COPY ["src/OmegaFY.Chat.API.Data.EF/OmegaFY.Chat.API.Data.EF.csproj", "src/OmegaFY.Chat.API.Data.EF/"]
RUN dotnet restore "./src/OmegaFY.Chat.API.WebAPI/OmegaFY.Chat.API.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/OmegaFY.Chat.API.WebAPI"
RUN dotnet build "./OmegaFY.Chat.API.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OmegaFY.Chat.API.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OmegaFY.Chat.API.WebAPI.dll"]