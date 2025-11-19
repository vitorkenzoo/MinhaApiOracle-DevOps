# 🔧 Configuração Baseada no Repositório Antigo (Que Funcionava)

## ✅ Mudanças Aplicadas

Baseado no repositório [MinhaApiOracle](https://github.com/vitorkenzoo/MinhaApiOracle) que funcionava, foram aplicadas as seguintes configurações:

### 1. **Program.cs** - Leitura de Connection String
- ✅ Agora lê de **Connection Strings** do Azure (método recomendado)
- ✅ Fallback para Application Settings (compatibilidade)
- ✅ Suporta múltiplos formatos de configuração

### 2. **script-infra-web-app.sh** - Configuração de Infraestrutura
- ✅ Usa `az webapp config connection-string set` (como no repositório antigo)
- ✅ Configura Connection String com tipo `SQLAzure`
- ✅ Nome: `SqlAzureConnection` (sem prefixo ConnectionStrings__)

### 3. **azure-pipeline.yml** - Pipeline CI/CD
- ✅ Configura Connection String via `az webapp config connection-string set`
- ✅ Mantém Application Settings para outras variáveis
- ✅ Usa variável secreta `$(SQL_CONNECTION_STRING)` do Azure DevOps

### 4. **Models** - Mapeamento de Tabelas
- ✅ Todos os modelos têm `[Table("T_...")]` para mapear corretamente:
  - `Usuario` → `T_USUARIOS`
  - `Curso` → `T_CURSO`
  - `Certificado` → `T_CERTIFICADO`
  - `Empresa` → `T_EMPRESA`
  - `Vaga` → `T_VAGA`
  - `LogAuditoria` → `T_LOG_AUDITORIA`

## 📋 Como Configurar no Azure Portal

### Opção 1: Connection Strings (Recomendado - Como no Repositório Antigo)

1. **Azure Portal** → **App Services** → **web-minha-api-oracle**
2. **Configuration** → **Connection strings**
3. Clique em **+ New connection string**
4. Configure:
   - **Name:** `SqlAzureConnection`
   - **Value:** `Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=SenhaSegura123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
   - **Type:** `SQLAzure`
5. Clique em **OK** e **Save**

### Opção 2: Application Settings (Fallback)

1. **Azure Portal** → **App Services** → **web-minha-api-oracle**
2. **Configuration** → **Application settings**
3. Clique em **+ New application setting**
4. Configure:
   - **Name:** `ConnectionStrings__SqlAzureConnection` (2 underscores)
   - **Value:** (mesma connection string acima)

## 🔄 Diferenças do Repositório Antigo

| Aspecto | Repositório Antigo | Repositório Atual |
|---------|-------------------|-------------------|
| Connection String | `az webapp config connection-string set` | ✅ Mesmo método |
| Nome | `SqlAzureConnection` | ✅ Mesmo nome |
| Tipo | `SQLAzure` | ✅ Mesmo tipo |
| Leitura no Código | `GetConnectionString("SqlAzureConnection")` | ✅ Mesmo método |
| Tabelas | Sem prefixo `T_` | ✅ Com prefixo `T_` (requisito atual) |

## ✅ Requisitos da Nova Entrega Mantidos

- ✅ Banco de dados PaaS (Azure SQL)
- ✅ Web App PaaS (Azure App Service)
- ✅ Scripts de infraestrutura (Azure CLI)
- ✅ Pipeline YAML (Build + Release)
- ✅ Testes XUnit publicados
- ✅ Variáveis de ambiente protegidas
- ✅ Tabelas com prefixo `T_` conforme script SQL

## 🧪 Teste

Após configurar, aguarde ~30 segundos e teste:
- URL: https://web-minha-api-oracle.azurewebsites.net/api/Usuarios
- Deve retornar `[]` (lista vazia) ou dados, **NÃO** mais erro 500

