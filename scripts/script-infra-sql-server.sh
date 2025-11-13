#!/bin/bash

# Script para criar SQL Server e Database no Azure
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

# Variáveis de ambiente (devem ser configuradas no Azure DevOps)
RESOURCE_GROUP_NAME="${RESOURCE_GROUP_NAME:-rg-minha-api-oracle}"
SQL_SERVER_NAME="${SQL_SERVER_NAME:-sql-minha-api-oracle}"
SQL_DATABASE_NAME="${SQL_DATABASE_NAME:-bd-minha-api-oracle}"
SQL_ADMIN_USER="${SQL_ADMIN_USER:-sqladmin}"
SQL_ADMIN_PASSWORD="${SQL_ADMIN_PASSWORD:-SenhaSegura123!@#}"
LOCATION="${LOCATION:-brazilsouth}"

echo "Criando SQL Server: $SQL_SERVER_NAME"

az sql server create \
    --name $SQL_SERVER_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --location $LOCATION \
    --admin-user $SQL_ADMIN_USER \
    --admin-password $SQL_ADMIN_PASSWORD

if [ $? -ne 0 ]; then
    echo "Erro ao criar SQL Server"
    exit 1
fi

echo "Configurando firewall do SQL Server para permitir serviços do Azure..."

az sql server firewall-rule create \
    --resource-group $RESOURCE_GROUP_NAME \
    --server $SQL_SERVER_NAME \
    --name AllowAzureServices \
    --start-ip-address 0.0.0.0 \
    --end-ip-address 0.0.0.0

echo "Criando SQL Database: $SQL_DATABASE_NAME"

az sql db create \
    --resource-group $RESOURCE_GROUP_NAME \
    --server $SQL_SERVER_NAME \
    --name $SQL_DATABASE_NAME \
    --service-objective S0 \
    --backup-storage-redundancy Local

if [ $? -eq 0 ]; then
    echo "SQL Server e Database criados com sucesso!"
else
    echo "Erro ao criar SQL Database"
    exit 1
fi

