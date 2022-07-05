#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Faluf.Portfolio.API/Faluf.Portfolio.API.csproj", "Faluf.Portfolio.API/"]
COPY ["Faluf.Portfolio.Core/Faluf.Portfolio.Core.csproj", "Faluf.Portfolio.Core/"]
COPY ["Faluf.Portfolio.Infrastructure/Faluf.Portfolio.Infrastructure.csproj", "Faluf.Portfolio.Infrastructure/"]
RUN dotnet restore "Faluf.Portfolio.API/Faluf.Portfolio.API.csproj"
COPY . .
WORKDIR "/src/Faluf.Portfolio.API"
RUN dotnet build "Faluf.Portfolio.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faluf.Portfolio.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faluf.Portfolio.API.dll"]