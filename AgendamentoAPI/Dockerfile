FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5217


USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["AgendamentoAPI/AgendamentoAPI.csproj", "AgendamentoAPI/"]
RUN dotnet restore "AgendamentoAPI/AgendamentoAPI.csproj"
COPY . .
WORKDIR "/src/AgendamentoAPI"
RUN dotnet build "AgendamentoAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "AgendamentoAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendamentoAPI.dll"]
