# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Moto/MotoApi/MotoApi.csproj", "Moto/MotoApi/"]
RUN dotnet restore "Moto/MotoApi/MotoApi.csproj"
COPY . .
WORKDIR "/src/Moto/MotoApi"
RUN dotnet publish "MotoApi.csproj" -c Release -o /app/publish

# Etapa de execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotoApi.dll"]
