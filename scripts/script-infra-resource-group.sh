#!/bin/bash

# Script para criar Resource Group no Azure
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

# Variáveis de ambiente (devem ser configuradas no Azure DevOps)
RESOURCE_GROUP_NAME="${RESOURCE_GROUP_NAME:-rg-minha-api-oracle}"
LOCATION="${LOCATION:-brazilsouth}"

# Verificar se o Resource Group já existe
az group show --name $RESOURCE_GROUP_NAME > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo "Resource Group '$RESOURCE_GROUP_NAME' já existe."
    echo "Se deseja recriar, execute primeiro o script-infra-delete.sh"
    exit 0
fi

echo "Criando Resource Group: $RESOURCE_GROUP_NAME na região: $LOCATION"

az group create \
    --name $RESOURCE_GROUP_NAME \
    --location $LOCATION

if [ $? -eq 0 ]; then
    echo "Resource Group criado com sucesso!"
else
    echo "Erro ao criar Resource Group"
    exit 1
fi

