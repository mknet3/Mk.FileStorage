#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Mk.FileStorage/Mk.FileStorage.csproj", "FileStorage.Sample/"]
COPY ["Mk.FileStorage.Azure/Mk.FileStorage.Azure.csproj", "FileStorage.Sample/"]
COPY ["Mk.FileStorage.AspNetCore/Mk.FileStorage.AspNetCore.csproj", "FileStorage.Sample/"]
COPY ["Mk.FileStorage.Sample/Mk.FileStorage.Sample.csproj", "FileStorage.Sample/"]
RUN dotnet restore "Mk.FileStorage.Sample/Mk.FileStorage.Sample.csproj"
COPY . .
WORKDIR "/src/FileStorage.Sample"
RUN dotnet build "Mk.FileStorage.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mk.FileStorage.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mk.FileStorage.Sample.dll"]