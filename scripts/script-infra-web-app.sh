#!/bin/bash

# Script para criar App Service Plan e Web App no Azure
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

# Variáveis de ambiente (devem ser configuradas no Azure DevOps)
RESOURCE_GROUP_NAME="${RESOURCE_GROUP_NAME:-rg-minha-api-oracle}"
APP_SERVICE_PLAN_NAME="${APP_SERVICE_PLAN_NAME:-asp-minha-api-oracle}"
WEB_APP_NAME="${WEB_APP_NAME:-web-minha-api-oracle}"
LOCATION="${LOCATION:-brazilsouth}"
SQL_SERVER_NAME="${SQL_SERVER_NAME:-sql-minha-api-oracle}"
SQL_DATABASE_NAME="${SQL_DATABASE_NAME:-bd-minha-api-oracle}"
SQL_ADMIN_USER="${SQL_ADMIN_USER:-sqladmin}"
SQL_ADMIN_PASSWORD="${SQL_ADMIN_PASSWORD:-SenhaSegura123!@#}"

echo "Criando App Service Plan: $APP_SERVICE_PLAN_NAME"

az appservice plan create \
    --name $APP_SERVICE_PLAN_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --location $LOCATION \
    --sku B1 \
    --is-linux

if [ $? -ne 0 ]; then
    echo "Erro ao criar App Service Plan"
    exit 1
fi

echo "Criando Web App: $WEB_APP_NAME"

# Criar web app e configurar runtime em um único comando
az webapp create \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --plan $APP_SERVICE_PLAN_NAME \
    --runtime "DOTNETCORE:9.0"

if [ $? -ne 0 ]; then
    echo "Erro ao criar Web App"
    echo "Tentando criar sem runtime e configurar depois..."
    
    # Fallback: criar sem runtime e configurar depois
    az webapp create \
        --name $WEB_APP_NAME \
        --resource-group $RESOURCE_GROUP_NAME \
        --plan $APP_SERVICE_PLAN_NAME
    
    if [ $? -ne 0 ]; then
        echo "Erro ao criar Web App"
        exit 1
    fi
    
    echo "Configurando runtime DOTNETCORE|9.0..."
    az webapp config set \
        --name $WEB_APP_NAME \
        --resource-group $RESOURCE_GROUP_NAME \
        --linux-fx-version "DOTNETCORE|9.0"
    
    if [ $? -ne 0 ]; then
        echo "Erro ao configurar runtime"
        exit 1
    fi
fi

echo "Configurando variáveis de ambiente na Web App..."

# Construir connection string
CONNECTION_STRING="Server=tcp:${SQL_SERVER_NAME}.database.windows.net,1433;Initial Catalog=${SQL_DATABASE_NAME};Persist Security Info=False;User ID=${SQL_ADMIN_USER};Password=${SQL_ADMIN_PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config connection-string set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --connection-string-type SQLAzure \
    --settings SqlAzureConnection="$CONNECTION_STRING"

az webapp config appsettings set \
    --name $WEB_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --settings ASPNETCORE_ENVIRONMENT="Production"

echo "Web App criada e configurada com sucesso!"
echo "URL da aplicação: https://${WEB_APP_NAME}.azurewebsites.net"

