# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BibMaMoAPI.sln", "./"]
COPY ["BibMaMoAPI/BibMaMo.Api.csproj", "BibMaMoAPI/"]
COPY ["BibMaMo.Core/BibMaMo.Core.csproj", "BibMaMo.Core/"]
COPY ["BibMaMo.Infrastructure/BibMaMo.Infrastructure.csproj", "BibMaMo.Infrastructure/"]
RUN dotnet restore "BibMaMoAPI/BibMaMo.Api.csproj"
COPY . .
WORKDIR "/src/BibMaMoAPI"
RUN dotnet build "BibMaMo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BibMaMo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BibMaMo.Api.dll"]
