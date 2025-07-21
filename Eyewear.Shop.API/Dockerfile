FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ✅ این‌ها الان دقیقاً داخل build context هستن
COPY ["Eyewear.Shop.API/Eyewear.Shop.API.csproj", "Eyewear.Shop.API/"]
COPY ["Eyewear.Shop.Application/Eyewear.Shop.Application.csproj", "Eyewear.Shop.Application/"]
COPY ["Eyewear.Shop.Infrastructure/Eyewear.Shop.Infrastructure.csproj", "Eyewear.Shop.Infrastructure/"]
COPY ["Eyewear.Shop.Domain/Eyewear.Shop.Domain.csproj", "Eyewear.Shop.Domain/"]

RUN dotnet restore "Eyewear.Shop.API/Eyewear.Shop.API.csproj"

COPY . .

WORKDIR "/src/Eyewear.Shop.API"
RUN dotnet build "Eyewear.Shop.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Eyewear.Shop.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eyewear.Shop.API.dll"]
