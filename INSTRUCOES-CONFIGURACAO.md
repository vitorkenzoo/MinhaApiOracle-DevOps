# Instruções para Configurar a Connection String no Azure

## Problema
O erro "Format of the initialization string does not conform to specification starting at index 0" ocorre porque a connection string não está configurada no Azure Web App.

## Solução 1: Configurar via Portal do Azure (Recomendado)

1. Acesse o [Portal do Azure](https://portal.azure.com)
2. Navegue até **App Services** > **web-minha-api-oracle**
3. No menu lateral, vá em **Configuration** > **Application settings**
4. Clique em **+ New application setting**
5. Adicione:
   - **Name:** `ConnectionStrings__SqlAzureConnection`
   - **Value:** `Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=SUA_SENHA_AQUI;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
6. Clique em **Save** e aguarde a aplicação reiniciar

## Solução 2: Configurar via Azure CLI

Execute o script PowerShell:

```powershell
.\scripts\config-connection-string.ps1 `
    -ResourceGroupName "rg-minha-api-oracle" `
    -WebAppName "web-minha-api-oracle" `
    -SqlServerName "sql-minha-api-oracle" `
    -SqlDatabaseName "bd-minha-api-oracle" `
    -SqlAdminUser "sqladmin" `
    -SqlAdminPassword "SUA_SENHA_AQUI"
```

## Solução 3: Configurar via Azure DevOps Pipeline

1. Acesse o Azure DevOps
2. Vá em **Pipelines** > **Library**
3. Crie ou edite uma **Variable Group**
4. Adicione a variável:
   - **Name:** `SQL_CONNECTION_STRING`
   - **Value:** `Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=SUA_SENHA_AQUI;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Marque como secreto (padlock)**
5. No pipeline, adicione a referência ao Variable Group
6. Execute o pipeline novamente

## Verificar se está funcionando

Após configurar, aguarde alguns segundos e teste:
- GET: `https://web-minha-api-oracle.azurewebsites.net/api/Usuarios`
- Deve retornar uma lista (mesmo que vazia) ou erro 200, não mais erro 500

## Nota Importante

⚠️ **Substitua `SUA_SENHA_AQUI` pela senha real do seu SQL Server no Azure!**

