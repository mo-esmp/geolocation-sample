#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/GeographySample.WebApi/GeographySample.WebApi.csproj", "src/GeographySample.WebApi/"]
COPY ["src/GeographySample.Infrastructure/GeographySample.Infrastructure.csproj", "src/GeographySample.Infrastructure/"]
COPY ["src/GeographySample.Core/GeographySample.Core.csproj", "src/GeographySample.Core/"]
RUN dotnet restore "src/GeographySample.WebApi/GeographySample.WebApi.csproj"
COPY . .
WORKDIR "/src/src/GeographySample.WebApi"
RUN dotnet build "GeographySample.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeographySample.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
RUN chmod +x /wait

CMD /wait && exec dotnet GeographySample.WebApi.dll