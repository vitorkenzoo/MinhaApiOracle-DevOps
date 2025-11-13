#!/bin/bash

# Script completo para provisionar toda a infraestrutura
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

# Este script executa todos os scripts de infraestrutura em ordem

echo "=========================================="
echo "Provisionamento completo da infraestrutura"
echo "=========================================="
echo ""

# Verificar se está logado no Azure
az account show > /dev/null 2>&1
if [ $? -ne 0 ]; then
    echo "Erro: Você precisa estar logado no Azure CLI"
    echo "Execute: az login"
    exit 1
fi

# Executar scripts em ordem
echo "1. Criando Resource Group..."
bash "$(dirname "$0")/script-infra-resource-group.sh"
if [ $? -ne 0 ]; then
    echo "Erro ao criar Resource Group"
    exit 1
fi

echo ""
echo "2. Criando SQL Server e Database..."
bash "$(dirname "$0")/script-infra-sql-server.sh"
if [ $? -ne 0 ]; then
    echo "Erro ao criar SQL Server"
    exit 1
fi

echo ""
echo "3. Criando Web App..."
bash "$(dirname "$0")/script-infra-web-app.sh"
if [ $? -ne 0 ]; then
    echo "Erro ao criar Web App"
    exit 1
fi

echo ""
echo "=========================================="
echo "Provisionamento concluído com sucesso!"
echo "=========================================="

