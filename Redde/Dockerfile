FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Redde/Redde.csproj", "Redde/"]
RUN dotnet restore "Redde/Redde.csproj"

COPY . .
WORKDIR "/src/Redde"
RUN dotnet build "Redde.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Redde.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redde.dll"]
