#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Faluf.Portfolio.IdentityServer/Faluf.Portfolio.IdentityServer.csproj", "Faluf.Portfolio.IdentityServer/"]
RUN dotnet restore "Faluf.Portfolio.IdentityServer/Faluf.Portfolio.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Faluf.Portfolio.IdentityServer"
RUN dotnet build "Faluf.Portfolio.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faluf.Portfolio.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faluf.Portfolio.IdentityServer.dll"]