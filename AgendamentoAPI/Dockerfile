#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7054
EXPOSE 7054

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AgendamentoAPI/AgendamentoAPI.csproj", "AgendamentoAPI/"]
COPY ["AgendamentosAPI.Shared.Dados/AgendamentosAPI.Shared.Dados.csproj", "AgendamentosAPI.Shared.Dados/"]
COPY ["AgendamentosAPI.Shared.Models/AgendamentosAPI.Shared.Models.csproj", "AgendamentosAPI.Shared.Models/"]
RUN dotnet restore "./AgendamentoAPI/AgendamentoAPI.csproj"
COPY . .
WORKDIR "/src/AgendamentoAPI"
RUN dotnet build "./AgendamentoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AgendamentoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendamentoAPI.dll"]