#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UDV-Camp-Test-Task/UDV-Camp-Test-Task.csproj", "UDV-Camp-Test-Task/"]
RUN dotnet restore "./UDV-Camp-Test-Task/UDV-Camp-Test-Task.csproj"
COPY . .
WORKDIR "/src/UDV-Camp-Test-Task"
RUN dotnet build "./UDV-Camp-Test-Task.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UDV-Camp-Test-Task.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UDV-Camp-Test-Task.dll"]