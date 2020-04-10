#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/GeographySample.DistanceCalculator/GeographySample.DistanceCalculator.csproj", "src/GeographySample.DistanceCalculator/"]
COPY ["src/GeographySample.Core/GeographySample.Core.csproj", "src/GeographySample.Core/"]
RUN dotnet restore "src/GeographySample.DistanceCalculator/GeographySample.DistanceCalculator.csproj"
COPY . .
WORKDIR "/src/src/GeographySample.DistanceCalculator"
RUN dotnet build "GeographySample.DistanceCalculator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeographySample.DistanceCalculator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
RUN chmod +x /wait

CMD /wait && exec dotnet GeographySample.DistanceCalculator.dll