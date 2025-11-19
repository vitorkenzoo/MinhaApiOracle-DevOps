# 🔧 Configurar Connection String no Azure - COMANDO RÁPIDO

## ⚠️ PROBLEMA ATUAL
A connection string não está configurada no Azure, causando erro 500.

## ✅ SOLUÇÃO RÁPIDA (Escolha uma opção)

### Opção 1: Via Portal do Azure (Mais Fácil)

1. Acesse: https://portal.azure.com
2. Procure por: **web-minha-api-oracle**
3. Vá em: **Configuration** → **Application settings**
4. Clique: **+ New application setting**
5. Configure:
   ```
   Name: ConnectionStrings__SqlAzureConnection
   Value: Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=SUA_SENHA_AQUI;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
   ```
6. **IMPORTANTE:** Substitua `SUA_SENHA_AQUI` pela senha real do seu SQL Server
7. Clique em **Save**
8. Aguarde ~30 segundos para a aplicação reiniciar

### Opção 2: Via Azure CLI (PowerShell)

```powershell
# Substitua SUA_SENHA_AQUI pela senha real
$senha = "SUA_SENHA_AQUI"
$connectionString = "Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=$senha;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config appsettings set `
    --name web-minha-api-oracle `
    --resource-group rg-minha-api-oracle `
    --settings ConnectionStrings__SqlAzureConnection="$connectionString"
```

### Opção 3: Usar o Script PowerShell

```powershell
.\scripts\config-connection-string.ps1 `
    -ResourceGroupName "rg-minha-api-oracle" `
    -WebAppName "web-minha-api-oracle" `
    -SqlServerName "sql-minha-api-oracle" `
    -SqlDatabaseName "bd-minha-api-oracle" `
    -SqlAdminUser "sqladmin" `
    -SqlAdminPassword "SUA_SENHA_AQUI"
```

## 🔍 Como Descobrir a Senha do SQL Server?

Se você não lembra da senha:
1. No Portal do Azure, vá em **SQL servers** → **sql-minha-api-oracle**
2. Clique em **Reset password** para criar uma nova senha
3. Use essa senha no comando acima

## ✅ Verificar se Funcionou

Após configurar, aguarde ~30 segundos e teste:
- URL: https://web-minha-api-oracle.azurewebsites.net/api/Usuarios
- Deve retornar `[]` (lista vazia) ou dados, **NÃO** mais erro 500

## 📝 Nota

O nome da configuração deve ser exatamente: `ConnectionStrings__SqlAzureConnection` (com dois underscores `__`)

