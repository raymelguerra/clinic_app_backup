#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClinicApp.MSInfrastructure/ClinicApp.MSInfrastructure.csproj", "ClinicApp.MSInfrastructure/"]
COPY ["ClinicApp.Core/ClinicApp.Core.csproj", "ClinicApp.Core/"]
RUN dotnet restore "ClinicApp.MSInfrastructure/ClinicApp.MSInfrastructure.csproj"
COPY . .
WORKDIR "/src/ClinicApp.MSInfrastructure"
RUN dotnet build "ClinicApp.MSInfrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicApp.MSInfrastructure.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicApp.MSInfrastructure.dll"]