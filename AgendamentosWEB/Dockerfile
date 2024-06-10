FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5217

ENV ASPNETCORE_URLS=http://+:5217

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["AgendamentosWEB.csproj", "./"]
RUN dotnet restore "AgendamentosWEB.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AgendamentosWEB.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "AgendamentosWEB.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot /usr/local/webapp/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

