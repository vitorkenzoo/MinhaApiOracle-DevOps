# 📋 Exemplos de JSON para a API

## ⚠️ **IMPORTANTE:**
- **NÃO envie** objetos de navegação (`usuario`, `curso`) no POST
- **Apenas envie** os IDs (`idUsuario`, `idCurso`) quando necessário
- Os objetos de navegação são preenchidos automaticamente pelo Entity Framework

---

## 👤 **POST /api/Usuarios** - Criar Usuário

### ✅ **JSON Correto:**
```json
{
  "nome": "João Silva",
  "emailUsuario": "joao.silva@email.com",
  "senha": "SenhaSegura123",
  "cpf": "12345678901",
  "cadastro": "2025-11-20T02:17:21.753Z"
}
```

### ❌ **NÃO envie:**
- `idUsuario` (gerado automaticamente)
- `certificados` (crie separadamente depois)

### 📝 **Exemplo Completo:**
```json
{
  "nome": "Maria Santos",
  "emailUsuario": "maria.santos@email.com",
  "senha": "MinhaSenha456",
  "cpf": "98765432100",
  "cadastro": "2025-11-20T10:00:00.000Z"
}
```

---

## 📜 **POST /api/Certificados** - Criar Certificado

### ✅ **JSON Correto:**
```json
{
  "idCertificado": "CERT001",
  "dtEmissao": "2025-11-20T02:17:21.753Z",
  "descricao": "Certificado de conclusão do curso",
  "codigoValidacao": "VAL123456",
  "idUsuario": 1,
  "idCurso": 1
}
```

### ❌ **NÃO envie:**
- `usuario` (objeto de navegação - não envie!)
- `curso` (objeto de navegação - não envie!)

### 📝 **Exemplo Completo:**
```json
{
  "idCertificado": "CERT002",
  "dtEmissao": "2025-11-20T10:00:00.000Z",
  "descricao": "Certificado de .NET Avançado",
  "codigoValidacao": "VAL789012",
  "idUsuario": 2,
  "idCurso": 1
}
```

---

## 📚 **POST /api/Cursos** - Criar Curso

### ✅ **JSON Correto:**
```json
{
  "nomeCurso": "Desenvolvimento .NET Avançado",
  "descricao": "Curso completo de .NET 9.0 com Azure",
  "qtHoras": 80
}
```

### ❌ **NÃO envie:**
- `idCurso` (gerado automaticamente)
- `certificados` (criados separadamente)

---

## 🏢 **POST /api/Empresas** - Criar Empresa

### ✅ **JSON Correto:**
```json
{
  "razaoSocial": "Empresa Tech Ltda",
  "cnpj": "12345678000190",
  "emailEmpresa": "contato@empresatech.com"
}
```

---

## 💼 **POST /api/Vagas** - Criar Vaga

### ✅ **JSON Correto:**
```json
{
  "nomeVaga": "Desenvolvedor .NET Senior",
  "descricaoVaga": "Vaga para desenvolvedor .NET com experiência em Azure",
  "salario": 12000.00,
  "dtPublicacao": "2025-11-20T10:00:00.000Z",
  "idEmpresa": 1
}
```

### ❌ **NÃO envie:**
- `idVaga` (gerado automaticamente)
- `empresa` (objeto de navegação - não envie!)

---

## 🔄 **Fluxo Completo: Criar Usuário + Certificado**

### **Passo 1: Criar Usuário**
```http
POST /api/Usuarios
Content-Type: application/json

{
  "nome": "João Silva",
  "emailUsuario": "joao.silva@email.com",
  "senha": "SenhaSegura123",
  "cpf": "12345678901"
}
```

**Resposta:** O usuário será criado e retornará com `idUsuario: 1` (exemplo)

### **Passo 2: Criar Curso (se ainda não existir)**
```http
POST /api/Cursos
Content-Type: application/json

{
  "nomeCurso": "Desenvolvimento .NET Avançado",
  "descricao": "Curso completo de .NET 9.0",
  "qtHoras": 80
}
```

**Resposta:** O curso será criado e retornará com `idCurso: 1` (exemplo)

### **Passo 3: Criar Certificado**
```http
POST /api/Certificados
Content-Type: application/json

{
  "idCertificado": "CERT001",
  "dtEmissao": "2025-11-20T10:00:00.000Z",
  "descricao": "Certificado de conclusão",
  "codigoValidacao": "VAL123456",
  "idUsuario": 1,
  "idCurso": 1
}
```

---

## 📖 **GET - Consultar Dados**

### **GET /api/Usuarios**
Retorna todos os usuários **COM** certificados (objetos completos)

### **GET /api/Usuarios/1**
Retorna o usuário com ID 1 **COM** certificados (objetos completos)

### **GET /api/Certificados**
Retorna todos os certificados **COM** usuário e curso (objetos completos)

---

## ⚠️ **Regras Importantes:**

1. **POST = Apenas dados básicos** (sem objetos de navegação)
2. **GET = Retorna objetos completos** (com navegação preenchida)
3. **Nunca envie** `usuario`, `curso`, `empresa` como objetos no POST
4. **Use apenas IDs** (`idUsuario`, `idCurso`, `idEmpresa`) quando necessário
5. **Crie relacionamentos separadamente** (ex: criar usuário primeiro, depois certificado)

---

## 🎯 **Resumo do Erro que Você Teve:**

❌ **Errado:**
```json
{
  "certificados": [{
    "usuario": "string"  // ❌ Não pode ser string!
  }]
}
```

✅ **Correto:**
```json
{
  "nome": "João Silva",
  "emailUsuario": "joao@email.com",
  "senha": "Senha123",
  "cpf": "12345678901"
}
```

E depois criar o certificado separadamente:
```json
{
  "idCertificado": "CERT001",
  "idUsuario": 1,  // ✅ Apenas o ID
  "idCurso": 1     // ✅ Apenas o ID
}
```

