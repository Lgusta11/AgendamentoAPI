FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5220

ENV ASPNETCORE_URLS=http://+:5220

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["AgendamentosWEB/AgendamentosWEB.csproj", "AgendamentosWEB/"]
RUN dotnet restore "AgendamentosWEB/AgendamentosWEB.csproj"
COPY . .
WORKDIR "/src/AgendamentosWEB"
RUN dotnet build "AgendamentosWEB.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "AgendamentosWEB.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendamentosWEB.dll"]
