FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SZEW/SZEW.csproj", "SZEW/"]
RUN dotnet restore "./SZEW/SZEW.csproj"
COPY . .
WORKDIR "/src/SZEW"
RUN dotnet build "./SZEW.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SZEW.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Prod
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY ./certs/ /app/certs/

# Kestrel SSL
ENV ASPNETCORE_URLS=https://+:8080
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certs/szew_ssl.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=SZEWToSuperSystemJest

# Set the entry point for the application
ENTRYPOINT ["dotnet", "SZEW.dll"]
