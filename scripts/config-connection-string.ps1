# Script para configurar a Connection String no Azure Web App
# RM557245 - Vitor Kenzo Mizumoto
# RM556760 - Adriano Barutti Pessuto

param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,
    
    [Parameter(Mandatory=$true)]
    [string]$WebAppName,
    
    [Parameter(Mandatory=$true)]
    [string]$SqlServerName,
    
    [Parameter(Mandatory=$true)]
    [string]$SqlDatabaseName,
    
    [Parameter(Mandatory=$true)]
    [string]$SqlAdminUser,
    
    [Parameter(Mandatory=$true)]
    [string]$SqlAdminPassword
)

Write-Host "Configurando Connection String no Azure Web App..." -ForegroundColor Green

# Construir connection string
$connectionString = "Server=tcp:$SqlServerName.database.windows.net,1433;Initial Catalog=$SqlDatabaseName;Persist Security Info=False;User ID=$SqlAdminUser;Password=$SqlAdminPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Host "Connection String construída (sem mostrar a senha)" -ForegroundColor Yellow

# Configurar como Application Setting (ConnectionStrings__SqlAzureConnection)
az webapp config appsettings set `
    --name $WebAppName `
    --resource-group $ResourceGroupName `
    --settings ConnectionStrings__SqlAzureConnection="$connectionString" `
    ASPNETCORE_ENVIRONMENT="Production"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Connection String configurada com sucesso!" -ForegroundColor Green
    Write-Host "Web App: $WebAppName" -ForegroundColor Cyan
    Write-Host "Resource Group: $ResourceGroupName" -ForegroundColor Cyan
} else {
    Write-Host "Erro ao configurar Connection String!" -ForegroundColor Red
    exit 1
}

