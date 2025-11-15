## teste

# Sistema de Gestão de Certificados e Vagas de Emprego

**RM557245** - Vitor Kenzo Mizumoto - 2TDSPF  
**RM556760** - Adriano Barutti Pessuto - 2TDSPF

## Descrição do Projeto

Sistema de gestão desenvolvido em .NET 8.0 para administração de usuários, certificados, cursos, empresas e vagas de emprego. A solução utiliza Azure SQL Database (PaaS) como banco de dados e Azure App Service para hospedagem, seguindo uma arquitetura PaaS pura.

## Arquitetura

- **Backend:** ASP.NET Core Web API (.NET 8.0)
- **Banco de Dados:** Azure SQL Database (PaaS)
- **Hospedagem:** Azure App Service (Web App PaaS)
- **ORM:** Entity Framework Core
- **Testes:** XUnit
- **CI/CD:** Azure DevOps Pipelines (YAML)
- **Infraestrutura:** Provisionada via Azure CLI Scripts

## Estrutura do Banco de Dados

### Tabelas

1. **T_USUARIOS** - Cadastro de usuários do sistema
2. **T_CURSO** - Cursos disponíveis
3. **T_CERTIFICADO** - Certificados emitidos para usuários
4. **T_EMPRESA** - Empresas cadastradas
5. **T_VAGA** - Vagas de emprego publicadas
6. **T_LOG_AUDITORIA** - Logs de auditoria do sistema

## Links Importantes

- **Azure DevOps:** [Link da Organização]
- **Vídeo YouTube:** [Link do Vídeo]

## Configuração de Variáveis de Ambiente

⚠️ **IMPORTANTE:** Dados sensíveis (senhas, connection strings) devem ser configurados como variáveis de ambiente no Azure DevOps ou Azure Key Vault, NUNCA no código fonte.

### Configuração no Azure DevOps:

1. **Pipelines > Library > Variable Groups**
   - Crie um Variable Group chamado `Production-Variables`
   - Adicione: `SQL_CONNECTION_STRING` (marcar como secreto)

2. **Azure Web App > Configuration > Application Settings**
   - Configure `ConnectionStrings:SqlAzureConnection` com a connection string do Azure SQL

## Exemplos de CRUD

### T_USUARIOS

#### CREATE (POST)
```json
POST /api/Usuarios
{
  "nome": "João Silva",
  "emailUsuario": "joao.silva@email.com",
  "senha": "SenhaSegura123",
  "cpf": "12345678901",
  "cadastro": "2025-11-13T10:00:00"
}
```

#### READ (GET)
```json
GET /api/Usuarios
GET /api/Usuarios/1
GET /api/Usuarios/buscar?nome=João
```

#### UPDATE (PUT)
```json
PUT /api/Usuarios/1
{
  "idUsuario": 1,
  "nome": "João Silva Santos",
  "emailUsuario": "joao.santos@email.com",
  "senha": "NovaSenha123",
  "cpf": "12345678901",
  "cadastro": "2025-11-13T10:00:00"
}
```

#### DELETE
```
DELETE /api/Usuarios/1
```

### T_CURSO

#### CREATE (POST)
```json
POST /api/Cursos
{
  "nomeCurso": "Desenvolvimento .NET Avançado",
  "descricao": "Curso completo de .NET 8.0",
  "qtHoras": 80
}
```

#### READ (GET)
```json
GET /api/Cursos
GET /api/Cursos/1
GET /api/Cursos/buscar?nome=.NET
```

#### UPDATE (PUT)
```json
PUT /api/Cursos/1
{
  "idCurso": 1,
  "nomeCurso": "Desenvolvimento .NET 8.0 Avançado",
  "descricao": "Curso completo atualizado",
  "qtHoras": 100
}
```

#### DELETE
```
DELETE /api/Cursos/1
```

### T_CERTIFICADO

#### CREATE (POST)
```json
POST /api/Certificados
{
  "idCertificado": "CERT001",
  "dtEmissao": "2025-11-13T10:00:00",
  "descricao": "Certificado de conclusão",
  "codigoValidacao": "VAL123456",
  "idUsuario": 1,
  "idCurso": 1
}
```

#### READ (GET)
```json
GET /api/Certificados
GET /api/Certificados/CERT001
GET /api/Certificados/usuario/1
```

#### UPDATE (PUT)
```json
PUT /api/Certificados/CERT001
{
  "idCertificado": "CERT001",
  "dtEmissao": "2025-11-13T10:00:00",
  "descricao": "Certificado atualizado",
  "codigoValidacao": "VAL123456",
  "idUsuario": 1,
  "idCurso": 1
}
```

#### DELETE
```
DELETE /api/Certificados/CERT001
```

### T_EMPRESA

#### CREATE (POST)
```json
POST /api/Empresas
{
  "razaoSocial": "Empresa Tech Ltda",
  "cnpj": "12345678000190",
  "emailEmpresa": "contato@empresatech.com"
}
```

#### READ (GET)
```json
GET /api/Empresas
GET /api/Empresas/1
GET /api/Empresas/buscar?razaoSocial=Tech
```

#### UPDATE (PUT)
```json
PUT /api/Empresas/1
{
  "idEmpresa": 1,
  "razaoSocial": "Empresa Tech Solutions Ltda",
  "cnpj": "12345678000190",
  "emailEmpresa": "contato@techsolutions.com"
}
```

#### DELETE
```
DELETE /api/Empresas/1
```

### T_VAGA

#### CREATE (POST)
```json
POST /api/Vagas
{
  "nomeVaga": "Desenvolvedor .NET Senior",
  "descricaoVaga": "Vaga para desenvolvedor .NET com experiência em Azure",
  "salario": 12000.00,
  "dtPublicacao": "2025-11-13T10:00:00",
  "idEmpresa": 1
}
```

#### READ (GET)
```json
GET /api/Vagas
GET /api/Vagas/1
GET /api/Vagas/buscar?nome=.NET
GET /api/Vagas/empresa/1
```

#### UPDATE (PUT)
```json
PUT /api/Vagas/1
{
  "idVaga": 1,
  "nomeVaga": "Desenvolvedor .NET Senior - Remoto",
  "descricaoVaga": "Vaga atualizada com opção remota",
  "salario": 15000.00,
  "dtPublicacao": "2025-11-13T10:00:00",
  "idEmpresa": 1
}
```

#### DELETE
```
DELETE /api/Vagas/1
```

### T_LOG_AUDITORIA

#### CREATE (POST)
```json
POST /api/LogAuditorias
{
  "nomeTabela": "T_USUARIOS",
  "dsOperacao": "I",
  "dataOperacao": "2025-11-13 10:00:00",
  "nmUsuarioDb": "sqladmin"
}
```

#### READ (GET)
```json
GET /api/LogAuditorias
GET /api/LogAuditorias/1
GET /api/LogAuditorias/buscar?tabela=USUARIOS
```

#### UPDATE (PUT)
```json
PUT /api/LogAuditorias/1
{
  "idLog": 1,
  "nomeTabela": "T_USUARIOS",
  "dsOperacao": "U",
  "dataOperacao": "2025-11-13 11:00:00",
  "nmUsuarioDb": "sqladmin"
}
```

#### DELETE
```
DELETE /api/LogAuditorias/1
```

## Scripts de Infraestrutura

Os scripts Azure CLI estão na pasta `scripts/`:

- `script-infra-resource-group.sh` - Cria o Resource Group
- `script-infra-sql-server.sh` - Cria SQL Server e Database
- `script-infra-web-app.sh` - Cria App Service Plan e Web App
- `script-infra-complete.sh` - Executa todos os scripts em sequência
- `script-bd.sql` - Script SQL para criação das tabelas

## Pipeline CI/CD

O arquivo `azure-pipeline.yml` na raiz do projeto configura:

- **Build:** Restore, Build, Test (XUnit), Publish Artifacts
- **Deploy:** Deploy automático para Azure Web App após Build bem-sucedido
- **Variáveis de Ambiente:** Connection string configurada via variáveis do Azure DevOps

## Como Executar Localmente

1. Configure a connection string no `appsettings.Development.json` ou via variável de ambiente:
```bash
$env:ConnectionStrings__SqlAzureConnection="Server=...;Database=...;User Id=...;Password=...;"
```

2. Execute as migrations:
```bash
dotnet ef database update
```

3. Execute a aplicação:
```bash
dotnet run
```

4. Acesse o Swagger em: `http://localhost:8080`

## Testes

Execute os testes com:
```bash
dotnet test
```
