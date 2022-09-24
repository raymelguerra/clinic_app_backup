#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClinicApp.Contractor/ClinicApp.MSContractor.csproj", "ClinicApp.Contractor/"]
COPY ["ClinicApp.Core/ClinicApp.Core.csproj", "ClinicApp.Core/"]
RUN dotnet restore "ClinicApp.Contractor/ClinicApp.MSContractor.csproj"
COPY . .
WORKDIR "/src/ClinicApp.Contractor"
RUN dotnet build "ClinicApp.MSContractor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicApp.MSContractor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicApp.MSContractor.dll"]