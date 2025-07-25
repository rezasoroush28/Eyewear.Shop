# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Eyewear.Shop.API/Eyewear.Shop.API.csproj", "Eyewear.Shop.API/"]
COPY ["Eyewear.Shop.Application/Eyewear.Shop.Application.csproj", "Eyewear.Shop.Application/"]
COPY ["Eyewear.Shop.Domain/Eyewear.Shop.Domain.csproj", "Eyewear.Shop.Domain/"]
COPY ["Eyewear.Shop.Infrastructure/Eyewear.Shop.Infrastructure.csproj", "Eyewear.Shop.Infrastructure/"]
COPY ["Eyewear.Shop.Persistence/Eyewear.Shop.Persist.csproj", "Eyewear.Shop.Persistence/"]
RUN dotnet restore "./Eyewear.Shop.API/Eyewear.Shop.API.csproj"
COPY . .
WORKDIR "/src/Eyewear.Shop.API"
RUN dotnet build "./Eyewear.Shop.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Eyewear.Shop.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eyewear.Shop.API.dll"]