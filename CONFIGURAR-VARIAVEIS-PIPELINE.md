# 🔧 Configurar Variáveis do Banco na Pipeline

## ✅ Vantagens de Usar Variáveis Separadas

Usar variáveis separadas ao invés de uma connection string completa é uma **melhor prática** porque:

- ✅ **Mais seguro** - cada variável pode ser marcada como secreta individualmente
- ✅ **Mais fácil de gerenciar** - pode alterar apenas uma parte (ex: senha) sem reescrever tudo
- ✅ **Mais flexível** - pode reutilizar variáveis em diferentes lugares
- ✅ **Melhor organização** - variáveis claras e descritivas
- ✅ **Menos erros** - menos chance de digitar errado uma connection string longa

## 📋 Variáveis Necessárias

Configure estas variáveis no **Azure DevOps**:

### Variáveis Obrigatórias:

1. **SQL_SERVER_NAME**
   - Tipo: Normal
   - Valor: `sql-minha-api-oracle`
   - Descrição: Nome do SQL Server no Azure

2. **SQL_DATABASE_NAME**
   - Tipo: Normal
   - Valor: `bd-minha-api-oracle`
   - Descrição: Nome do banco de dados

3. **SQL_ADMIN_USER**
   - Tipo: Normal
   - Valor: `sqladmin`
   - Descrição: Usuário administrador do SQL Server

4. **SQL_ADMIN_PASSWORD**
   - Tipo: **SECRETO** ⚠️ (marcar como secreto!)
   - Valor: `SenhaSegura123!@#` (sua senha real)
   - Descrição: Senha do administrador do SQL Server

## 🔐 Como Configurar no Azure DevOps

### Opção 1: Variáveis do Pipeline (Recomendado)

1. Acesse o **Azure DevOps**
2. Vá em **Pipelines** → Selecione seu pipeline
3. Clique em **Edit** (editar)
4. Clique em **Variables** (no canto superior direito)
5. Clique em **+ New variable** para cada variável:

   **Variável 1:**
   - Name: `SQL_SERVER_NAME`
   - Value: `sql-minha-api-oracle`
   - ✅ Keep this value secret: **NÃO** (é público)

   **Variável 2:**
   - Name: `SQL_DATABASE_NAME`
   - Value: `bd-minha-api-oracle`
   - ✅ Keep this value secret: **NÃO**

   **Variável 3:**
   - Name: `SQL_ADMIN_USER`
   - Value: `sqladmin`
   - ✅ Keep this value secret: **NÃO**

   **Variável 4:**
   - Name: `SQL_ADMIN_PASSWORD`
   - Value: `SenhaSegura123!@#` (sua senha real)
   - ✅ Keep this value secret: **SIM** ⚠️ (marcar como secreto!)

6. Clique em **Save** para cada variável

### Opção 2: Variable Groups (Para Múltiplos Pipelines)

1. Acesse o **Azure DevOps**
2. Vá em **Pipelines** → **Library**
3. Clique em **+ Variable group**
4. Nome: `Production-Database-Variables`
5. Adicione as 4 variáveis acima
6. Marque `SQL_ADMIN_PASSWORD` como secreto
7. No pipeline, adicione referência ao Variable Group:

```yaml
variables:
- group: Production-Database-Variables
```

## 🔄 Como o Pipeline Monta a Connection String

O pipeline agora monta a connection string automaticamente:

```powershell
$connectionString = "Server=tcp:$(SQL_SERVER_NAME).database.windows.net,1433;Initial Catalog=$(SQL_DATABASE_NAME);Persist Security Info=False;User ID=$(SQL_ADMIN_USER);Password=$(SQL_ADMIN_PASSWORD);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

**Resultado final:**
```
Server=tcp:sql-minha-api-oracle.database.windows.net,1433;Initial Catalog=bd-minha-api-oracle;Persist Security Info=False;User ID=sqladmin;Password=SenhaSegura123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

## ✅ Vantagens Desta Abordagem

1. **Segurança:** Senha marcada como secreto, não aparece em logs
2. **Manutenção:** Fácil alterar apenas a senha sem mexer no código
3. **Reutilização:** Mesmas variáveis podem ser usadas em outros pipelines
4. **Organização:** Variáveis claras e descritivas
5. **Conformidade:** Atende requisitos de proteção de dados sensíveis

## 🧪 Teste

Após configurar as variáveis:

1. Execute o pipeline
2. Verifique se a Connection String foi configurada corretamente
3. Teste a aplicação: https://web-minha-api-oracle.azurewebsites.net/api/Usuarios

## 📝 Nota Importante

⚠️ **NUNCA** commite senhas ou connection strings completas no código!
- ✅ Use variáveis do Azure DevOps
- ✅ Marque senhas como **SECRETAS**
- ✅ Use Variable Groups para organizar

