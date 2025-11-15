#!/bin/bash

# Script para deletar todos os recursos do Azure
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

# Variáveis de ambiente (devem ser configuradas no Azure DevOps)
RESOURCE_GROUP_NAME="${RESOURCE_GROUP_NAME:-rg-minha-api-oracle}"
WEB_APP_NAME="${WEB_APP_NAME:-web-minha-api-oracle}"
APP_SERVICE_PLAN_NAME="${APP_SERVICE_PLAN_NAME:-asp-minha-api-oracle}"

echo "=========================================="
echo "Deletando recursos do Azure"
echo "=========================================="
echo ""

# Verificar se está logado no Azure
az account show > /dev/null 2>&1
if [ $? -ne 0 ]; then
    echo "Erro: Você precisa estar logado no Azure CLI"
    echo "Execute: az login"
    exit 1
fi

# Verificar se o Resource Group existe
az group show --name $RESOURCE_GROUP_NAME > /dev/null 2>&1
if [ $? -ne 0 ]; then
    echo "Resource Group '$RESOURCE_GROUP_NAME' não existe. Nada para deletar."
    exit 0
fi

echo "ATENÇÃO: Este script irá deletar TODOS os recursos no Resource Group: $RESOURCE_GROUP_NAME"
echo "Isso inclui:"
echo "  - Web App: $WEB_APP_NAME"
echo "  - App Service Plan: $APP_SERVICE_PLAN_NAME"
echo "  - SQL Server e Database"
echo "  - Todos os outros recursos no Resource Group"
echo ""
read -p "Tem certeza que deseja continuar? (sim/não): " confirm

if [ "$confirm" != "sim" ] && [ "$confirm" != "s" ] && [ "$confirm" != "yes" ] && [ "$confirm" != "y" ]; then
    echo "Operação cancelada."
    exit 0
fi

echo ""
echo "Deletando Resource Group e todos os recursos..."
az group delete \
    --name $RESOURCE_GROUP_NAME \
    --yes \
    --no-wait

if [ $? -eq 0 ]; then
    echo "Resource Group '$RESOURCE_GROUP_NAME' está sendo deletado (operação assíncrona)."
    echo "Você pode verificar o status com: az group show --name $RESOURCE_GROUP_NAME"
else
    echo "Erro ao deletar Resource Group"
    exit 1
fi

echo ""
echo "=========================================="
echo "Recursos marcados para deleção!"
echo "=========================================="
echo "Aguarde alguns minutos para a deleção ser concluída."
echo "Depois, você pode executar o script-infra-complete.sh para recriar tudo."

