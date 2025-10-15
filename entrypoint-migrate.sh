#!/bin/bash
set -e

# Espera PostgreSQL
until pg_isready -h "$POSTGRES_HOST" -p "$POSTGRES_PORT"; do
  echo "⏳ Aguardando PostgreSQL..."
  sleep 2
done

# Aplica migrations
echo "🚀 Aplicando migrações..."
dotnet ef database update --project /src/Moto/MotoApi/MotoApi.csproj --startup-project /src/Moto/MotoApi/MotoApi.csproj

echo "✅ Migrações aplicadas. Container vai encerrar."




