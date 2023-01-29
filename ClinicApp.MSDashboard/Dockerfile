#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClinicApp.MSDashboard/ClinicApp.MSDashboard.csproj", "ClinicApp.MSDashboard/"]
RUN dotnet restore "ClinicApp.MSDashboard/ClinicApp.MSDashboard.csproj"
COPY . .
WORKDIR "/src/ClinicApp.MSDashboard"
RUN dotnet build "ClinicApp.MSDashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicApp.MSDashboard.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicApp.MSDashboard.dll"]