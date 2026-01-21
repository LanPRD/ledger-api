# Financial Ledger API 💰

API REST para gerenciamento de transações financeiras com garantia de idempotência, construída com .NET 10, Clean Architecture e deploy serverless na AWS.

## 🎯 Sobre o Projeto

Sistema de ledger financeiro que garante consistência de transações através de chaves de idempotência, prevenindo duplicações em cenários de retry e falhas de rede. Ideal para aplicações fintech que exigem alta confiabilidade.

### Principais Funcionalidades

- ✅ Criação de transações (débito/crédito)
- ✅ Garantia de idempotência com constraint UNIQUE
- ✅ Cálculo automático de saldo
- ✅ Validação de saldo insuficiente
- ✅ Transações atômicas com rollback
- ✅ Suporte a múltiplas contas

## 🏗️ Arquitetura

Projeto construído seguindo princípios de **Clean Architecture** e **Domain-Driven Design**:

```
📦 FinancialLedger
├── 📂 src
│   ├── FinancialLedger.Api          # Controllers, Filters, Middleware
│   ├── FinancialLedger.Application  # Use Cases, Validators
│   ├── FinancialLedger.Domain       # Entities, Interfaces
│   ├── FinancialLedger.Infrastructure # EF Core, Repositories, Migrations
│   ├── FinancialLedger.Communication # DTOs, Requests, Responses
│   └── FinancialLedger.Exception    # Custom Exceptions
└── 📂 __tests__
    ├── UseCases.Test                # Testes Unitários
    ├── WebApi.Test                  # Testes de Integração
    └── CommonTestUtilities          # Mocks, Builders
```

## 🛠️ Tecnologias

### Backend

- **.NET 10** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **FluentValidation** - Validação de requests
- **AutoMapper** - Mapeamento de objetos

### Testes

- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions fluentes
- **Moq** - Mocking
- **SQLite InMemory** - Banco para testes de integração

### Cloud & DevOps

- **AWS Lambda** - Serverless compute
- **API Gateway** - HTTP API
- **AWS SAM** - Infrastructure as Code
- **CloudFormation** - Deploy automatizado

## 📊 Diagrama de Fluxo - Idempotência

```
Cliente envia request com IdempotencyKey
           ↓
    Valida Account existe?
           ↓
  Tenta inserir IdempotencyRecord
           ↓
     ┌─────┴─────┐
     │           │
  Success    Conflict (409)
     │           │
     ↓           ↓
 Atualiza    Retorna erro
  Saldo      "já processado"
     │
     ↓
  Cria LedgerEntry
     │
     ↓
 Commit Transaction
     │
     ↓
  Retorna 201 Created
```

## 🚀 Como Rodar

### Pré-requisitos

- .NET 10 SDK
- PostgreSQL
- AWS CLI (para deploy)
- AWS SAM CLI (para deploy)

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/financial-ledger.git
cd financial-ledger
```

### 2. Configure o banco de dados

```bash
# Copie o arquivo de exemplo
cp src/FinancialLedger.Api/appsettings.Example.json src/FinancialLedger.Api/appsettings.Development.json

# Edite a connection string
# "DefaultConnection": "Host=localhost;Database=financialledger;Username=postgres;Password=sua_senha"
```

### 3. Execute as migrations

```bash
cd src/FinancialLedger.Api
dotnet ef database update
```

### 4. Rode a aplicação

```bash
dotnet run
```

## 🧪 Testes

### Rodar todos os testes

```bash
dotnet test
```

### Testes Unitários

```bash
dotnet test __tests__/UseCases.Test
```

### Testes de Integração

```bash
dotnet test __tests__/WebApi.Test
```

### Cobertura

- ✅ **Testes Unitários**: Use Cases com mocks
- ✅ **Testes de Integração**: Fluxo completo com banco SQLite
- ✅ **Cenários cobertos**: Success, validações, idempotência, erros

## 📡 API Endpoints

### Criar Transação

```http
POST /api/account/{accountId}/entries
Content-Type: application/json

{
  "type": "DEBIT",
  "amount": 100.50,
  "description": "Compra no mercado",
  "idempotencyKey": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Responses:**

- `201 Created` - Transação criada com sucesso
- `400 Bad Request` - Validação falhou (saldo insuficiente, valores inválidos)
- `404 Not Found` - Conta não encontrada
- `409 Conflict` - IdempotencyKey já utilizada

### Exemplo de Response

```json
{
  "id": 123,
  "accountId": 1,
  "type": "DEBIT",
  "amount": 100.5,
  "description": "Compra no mercado",
  "createdAt": "2026-01-21T10:30:00Z"
}
```

## ☁️ Deploy na AWS

### Deploy com SAM

```bash
# Build
sam build

# Deploy (primeira vez)
sam deploy --guided

# Deploys seguintes
sam deploy
```

### Configuração

O `template.yaml` define:

- Lambda Function com .NET 10
- API Gateway HTTP API
- Variáveis de ambiente (connection string)
- Políticas IAM necessárias

## 🔒 Segurança

- ✅ Validação de entrada com FluentValidation
- ✅ Transações atômicas (ACID)
- ✅ Exception handling centralizado
- ✅ Connection string via variáveis de ambiente
- ✅ Constraints de banco para integridade

## 📈 Melhorias Futuras

- [ ] Autenticação JWT
- [ ] Rate limiting
- [ ] Cache com Redis
- [ ] Logs estruturados (Serilog)
- [ ] Metrics com CloudWatch
- [ ] CI/CD com GitHub Actions
- [✅] Documentação Swagger/OpenAPI
- [ ] Paginação de transações
- [ ] Filtros e busca avançada
- [ ] Eventos assíncronos (SQS/SNS)

## 📝 Aprendizados

Este projeto me permitiu:

- ✅ Implementar Clean Architecture na prática
- ✅ Trabalhar com idempotência em sistemas distribuídos
- ✅ Criar testes unitários e de integração eficazes
- ✅ Deploy serverless com AWS Lambda
- ✅ Usar EF Core com PostgreSQL
- ✅ Aplicar Domain-Driven Design

## 👨‍💻 Autor

**Allan** - [LinkedIn](https://linkedin.com/in/lanprd) | [GitHub](https://github.com/LanPRD)
