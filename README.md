# Desafio Técnico Axia - API de Veículos

API REST desenvolvida em .NET 8 para cadastro e consulta de veículos, seguindo boas práticas de arquitetura em camadas e padrões modernos.

> 🚀 **Quer começar rápido?** Veja o [QUICK_START.md](QUICK_START.md) para instruções passo a passo!

## 🏗️ Arquitetura

A solução está organizada em camadas com responsabilidades bem definidas:

- **Domain**: Entidades, enumeradores e interfaces de repositório
- **Application**: Serviços, Commands/Queries (MediatR), Handlers e Validators (FluentValidation)
- **Infra**: Contexto do Entity Framework, Repositórios e Migrations
- **WebApi**: Controllers, configuração do Swagger e injeção de dependências

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** (Controllers)
- **PostgreSQL 16** - Banco de dados relacional
- **Entity Framework Core 8.0** - ORM
- **MediatR 12.2.0** - CQRS Pattern
- **FluentValidation 11.9.0** - Validação
- **AutoMapper 12.0.1** - Mapeamento de objetos
- **Docker & Docker Compose** - Containerização
- **Kubernetes** - Orquestração de containers
- **Health Checks** - Monitoramento de saúde
- **Swagger/OpenAPI** - Documentação da API
- **xUnit, Moq, FluentAssertions** - Testes unitários

## 📋 Pré-requisitos

### Desenvolvimento Local
- .NET 8 SDK instalado
- Docker Desktop (para PostgreSQL)
- Visual Studio 2022, Visual Studio Code ou Rider (opcional)

### Docker/Kubernetes
- Docker Desktop ou Docker Engine
- Kubernetes cluster (para deploy em K8s)
- kubectl (para deploy em K8s)

## ✅ Verificação Pós-Clone

Antes de executar, verifique se tudo está configurado corretamente:

### Windows (PowerShell):
```powershell
.\VERIFICAR_INSTALACAO.ps1
```

### Linux/Mac (Bash):
```bash
chmod +x VERIFICAR_INSTALACAO.sh
./VERIFICAR_INSTALACAO.sh
```

Este script verifica:
- ✅ .NET 8 SDK instalado
- ✅ Docker e Docker Compose instalados
- ✅ Arquivos essenciais presentes
- ✅ Projeto compila sem erros
- ✅ Testes unitários passam

## 🚀 Como Executar

### Opção 1: Docker Compose (Recomendado) 🐳

1. **Clone o repositório:**
```bash
git clone <repo-url>
cd desafio-tecnico-axia
```

2. **Execute com Docker Compose:**
```bash
docker-compose up --build
```

3. **Aguarde a mensagem "Application started" nos logs**

4. **Teste se a API está funcionando:**

**Windows (PowerShell):**
```powershell
.\TESTAR_API.ps1
```

**Linux/Mac (Bash):**
```bash
chmod +x TESTAR_API.sh
./TESTAR_API.sh
```

Este script testa automaticamente:
- ✅ Health Check
- ✅ Swagger UI
- ✅ GET (listar veículos)
- ✅ POST (criar veículo)
- ✅ GET por ID
- ✅ PUT (atualizar veículo)
- ✅ DELETE (excluir veículo)

4. **Acesse a API nos seguintes endereços:**

   **📚 Swagger UI (Documentação Interativa):**
   ```
   http://localhost:8080/swagger
   ```
   Abra no navegador para testar os endpoints diretamente.

   **🏥 Health Check:**
   ```
   http://localhost:8080/health
   ```
   Verifica se a API está funcionando.

   **📋 Endpoints da API:**
   - `GET http://localhost:8080/api/veiculo` - Listar veículos
   - `GET http://localhost:8080/api/veiculo/{id}` - Obter veículo por ID
   - `POST http://localhost:8080/api/veiculo` - Criar veículo
   - `PUT http://localhost:8080/api/veiculo/{id}` - Atualizar veículo
   - `DELETE http://localhost:8080/api/veiculo/{id}` - Excluir veículo

### 🎯 Exemplo de Teste - Criar Veículo no Swagger

**Passo a passo:**

1. Acesse `http://localhost:8080/swagger` no navegador
2. Expanda o endpoint `POST /api/veiculo`
3. Clique no botão **"Try it out"**
4. No campo **Request body**, cole o seguinte JSON:
```json
{
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "valor": 250000.00
}
```
5. Clique no botão azul **"Execute"**

**Resultado esperado:**

- ✅ **Status Code:** `201 Created`
- ✅ **Response Body:**
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 250000
}
```

> 💡 **Dica:** A forma mais fácil de testar é acessar `http://localhost:8080/swagger` no navegador!

### Opção 2: Desenvolvimento Local 💻

1. **Inicie o PostgreSQL (Docker):**
```bash
docker run --name postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=VeiculosDb -p 5432:5432 -d postgres:16-alpine
```

2. **Restaure dependências:**
```bash
dotnet restore
```

3. **Aplique migrations:**
```bash
dotnet ef database update --project DesafioTecnicoAxia.Infra --startup-project DesafioTecnicoAxia.WebApi
```

4. **Execute a aplicação:**
```bash
dotnet run --project DesafioTecnicoAxia.WebApi/DesafioTecnicoAxia.WebApi.csproj
```

5. **Aguarde a mensagem "Now listening on: https://localhost:XXXX"**

6. **Acesse a API:**
   - **Swagger:** `https://localhost:7XXX/swagger` (a porta será exibida no console)
   - **Health Check:** `https://localhost:7XXX/health`
   - **API Base:** `https://localhost:7XXX/api/veiculo`

### Opção 3: Kubernetes

Veja o guia completo em [DEPLOY.md](DEPLOY.md)

```bash
cd k8s
./deploy.sh
```

## 🧪 Testando a API

### Exemplo de Teste - Criar Veículo

**1. Acesse o Swagger:** `http://localhost:8080/swagger`

**2. Expanda o endpoint `POST /api/veiculo`**

**3. Clique em "Try it out"**

**4. Cole o seguinte JSON no Request body:**
```json
{
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "valor": 250000.00
}
```

**5. Clique em "Execute"**

**6. Resultado esperado:**
- **Status Code:** `201 Created`
- **Response Body:**
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 250000
}
```

> ✅ **Sucesso!** O veículo foi criado e você recebeu um ID único (GUID).

**📸 Exemplo Visual do Teste no Swagger:**

*Captura de tela mostrando o teste bem-sucedido:*
- ✅ Status Code: `201 Created`
- ✅ Request Body com JSON de exemplo
- ✅ Response Body com o veículo criado (incluindo ID gerado)
- ✅ Interface completa do Swagger UI com botões "Try it out" e "Execute"

## 📚 Endpoints da API

### POST /api/veiculo
Cadastra um novo veículo.

**Request Body:**
```json
{
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "valor": 250000.00
}
```

**Response:** 201 Created
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 250000
}
```

> 📸 Veja a seção ["🧪 Testando a API"](#-testando-a-api) acima para ver o exemplo visual completo do teste no Swagger!

### PUT /api/veiculo/{id}
Atualiza um veículo existente.

**Path Parameter:**
- `id` (required, UUID): ID do veículo a ser atualizado

**Request Body:**
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 260000
}
```

**Response:** 200 OK
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 260000
}
```

**📸 Exemplo Visual - Edição de Veículo:**

*Captura de tela mostrando a atualização bem-sucedida:*
- ✅ Status Code: `200 OK`
- ✅ Path Parameter com ID do veículo
- ✅ Request Body com dados atualizados (valor alterado para 260000)
- ✅ Response Body confirmando a atualização

### GET /api/veiculo/{id}
Obtém um veículo específico por Id.

**Path Parameter:**
- `id` (required, UUID): ID do veículo a ser consultado

**Response:** 200 OK
```json
{
  "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
  "descricao": "Carro esportivo",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": null,
  "valor": 260000
}
```

**📸 Exemplo Visual - Busca por ID:**

*Captura de tela mostrando a busca bem-sucedida:*
- ✅ Status Code: `200 OK`
- ✅ Path Parameter com ID do veículo
- ✅ Response Body com os dados completos do veículo

### GET /api/veiculo
Lista todos os veículos cadastrados.

**Response:** 200 OK
```json
[
  {
    "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
    "descricao": "Carro esportivo",
    "marca": 1,
    "modelo": "Mustang GT",
    "opcionais": null,
    "valor": 250000
  }
]
```

**📸 Exemplo Visual - Listagem de Veículos:**

*Captura de tela mostrando a listagem bem-sucedida:*
- ✅ Status Code: `200 OK`
- ✅ Response Body com array de veículos
- ✅ Sem parâmetros necessários (endpoint simples)

### DELETE /api/veiculo/{id}
Remove um veículo.

**Path Parameter:**
- `id` (required, UUID): ID do veículo a ser removido

**Response:** 204 No Content

**📸 Exemplo Visual - Deleção de Veículo:**

*Captura de tela mostrando a deleção bem-sucedida:*
- ✅ Status Code: `204 No Content`
- ✅ Path Parameter com ID do veículo
- ✅ Sem Response Body (conforme padrão HTTP 204)

## 📝 Enumerador Marca

Os valores válidos para o campo `marca` são:

- 1 = Ford
- 2 = Chevrolet
- 3 = Fiat
- 4 = Volkswagen
- 5 = Toyota
- 6 = Honda
- 7 = Nissan
- 8 = Hyundai
- 9 = Renault
- 10 = Peugeot

## ✅ Validações

As validações são aplicadas automaticamente via FluentValidation:

- **Descrição**: Obrigatória, máximo 500 caracteres
- **Marca**: Obrigatória, deve ser um valor válido do enumerador
- **Modelo**: Obrigatório, máximo 200 caracteres
- **Opcionais**: Opcional, máximo 1000 caracteres
- **Valor**: Opcional, se informado deve ser maior que zero

Em caso de erro de validação, a API retorna HTTP 400 (Bad Request) com as mensagens de erro.

## 🧪 Exemplos de Uso

### Exemplo 1: Cadastrar um veículo completo
```bash
curl -X POST "https://localhost:7XXX/api/veiculo" \
  -H "Content-Type: application/json" \
  -d '{
    "descricao": "Carro esportivo em excelente estado",
    "marca": 1,
    "modelo": "Mustang GT",
    "opcionais": "Ar condicionado, Vidros elétricos",
    "valor": 250000.00
  }'
```

### Exemplo 2: Cadastrar um veículo sem campos opcionais
```bash
curl -X POST "https://localhost:7XXX/api/veiculo" \
  -H "Content-Type: application/json" \
  -d '{
    "descricao": "Carro popular",
    "marca": 3,
    "modelo": "Uno"
  }'
```

### Exemplo 3: Listar todos os veículos
```bash
curl -X GET "https://localhost:7XXX/api/veiculo"
```

### Exemplo 4: Obter um veículo por Id
```bash
curl -X GET "https://localhost:7XXX/api/veiculo/3fa85f64-5717-4562-b3fc-2c963f66afa6"
```

### Exemplo 5: Atualizar um veículo
```bash
curl -X PUT "https://localhost:7XXX/api/veiculo/3fa85f64-5717-4562-b3fc-2c963f66afa6" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "descricao": "Carro esportivo - atualizado",
    "marca": 1,
    "modelo": "Mustang GT",
    "valor": 255000.00
  }'
```

### Exemplo 6: Excluir um veículo
```bash
curl -X DELETE "https://localhost:7XXX/api/veiculo/3fa85f64-5717-4562-b3fc-2c963f66afa6"
```

## 📦 Estrutura do Projeto

```
DesafioTecnicoAxia/
├── DesafioTecnicoAxia.Domain/
│   ├── Entidades/
│   │   └── Veiculo.cs
│   ├── Enumeradores/
│   │   └── Marca.cs
│   └── Interfaces/
│       └── IRepository.cs
├── DesafioTecnicoAxia.Application/
│   ├── Commands/
│   │   ├── AdicionarVeiculoCommand.cs
│   │   ├── AtualizarVeiculoCommand.cs
│   │   ├── ExcluirVeiculoCommand.cs
│   │   ├── ListarVeiculosQuery.cs
│   │   └── ObterVeiculoPorIdQuery.cs
│   ├── Handlers/
│   │   ├── AdicionarVeiculoHandler.cs
│   │   ├── AtualizarVeiculoHandler.cs
│   │   ├── ExcluirVeiculoHandler.cs
│   │   ├── ListarVeiculosHandler.cs
│   │   └── ObterVeiculoPorIdHandler.cs
│   ├── Validators/
│   │   ├── AdicionarVeiculoValidator.cs
│   │   ├── AtualizarVeiculoValidator.cs
│   │   ├── ExcluirVeiculoValidator.cs
│   │   └── ObterVeiculoPorIdValidator.cs
│   └── VeiculoService/
│       ├── IVeiculoService.cs
│       └── VeiculoService.cs
├── DesafioTecnicoAxia.Infra/
│   ├── Context/
│   │   └── ApplicationDbContext.cs
│   ├── Migrations/
│   │   ├── 20241216000000_InitialCreate.cs
│   │   └── ApplicationDbContextModelSnapshot.cs
│   └── Repository/
│       ├── IRepository.cs
│       ├── IVeiculoRepository.cs
│       ├── Repository.cs
│       └── VeiculoRepository.cs
└── DesafioTecnicoAxia.WebApi/
    ├── Controllers/
    │   └── VeiculoController.cs
    └── Program.cs
```

## 🔧 Padrões e Boas Práticas Implementadas (Nível Sênior)

### Arquitetura e Design Patterns
- **Clean Architecture**: Separação clara de responsabilidades em camadas
- **SOLID Principles**: Aplicação rigorosa dos princípios SOLID
- **Repository Pattern**: Abstração da camada de acesso a dados
- **Unit of Work Pattern**: Gerenciamento transacional
- **CQRS**: Separação de Commands e Queries usando MediatR
- **DTO Pattern**: Separação entre entidades de domínio e DTOs de API
- **Dependency Injection**: Uso extensivo de DI para desacoplamento
- **Factory Pattern**: Factory methods nas entidades (Veiculo.Create)
- **Base Classes**: Herança e reutilização de código (BaseEntity, BaseHandler)

### Orientação a Objetos (OOP)
- **Encapsulamento**: Propriedades privadas com métodos públicos controlados
- **Herança**: BaseEntity e BaseHandler para reutilização
- **Polimorfismo**: Interfaces e abstrações
- **Abstração**: Interfaces bem definidas (IRepository, IVeiculoService)
- **Imutabilidade**: Entidades com propriedades privadas e métodos de atualização

### Infraestrutura e DevOps
- **Docker**: Containerização completa da aplicação
- **Docker Compose**: Orquestração local com PostgreSQL
- **Kubernetes**: Manifests completos para deploy em produção
- **Health Checks**: Endpoints de saúde (/health, /health/ready, /health/live)
- **PostgreSQL**: Banco de dados relacional robusto
- **Migrations**: Versionamento de schema do banco de dados
- **Environment Variables**: Configuração via variáveis de ambiente

### Qualidade de Código
- **AutoMapper**: Mapeamento automático entre DTOs e entidades
- **FluentValidation**: Validação robusta de entrada
- **Exception Handling Middleware**: Tratamento global de exceções
- **Logging Estruturado**: Logging em todas as camadas críticas
- **Testes Unitários**: Cobertura de Handlers e Validators
- **Testes com FluentAssertions**: Assertions mais legíveis e expressivas

### Tratamento de Erros
- **Middleware Global**: Tratamento centralizado de exceções
- **Custom Exceptions**: Exceções específicas do domínio (NotFoundException)
- **Retornos HTTP Apropriados**: 400, 404, 500 com mensagens claras
- **Swagger**: Documentação automática da API com XML comments

## 🧪 Testes

O projeto inclui testes unitários para validar a lógica de negócio:

```bash
dotnet test
```

**Cobertura de Testes:**
- ✅ Handlers (MediatR)
- ✅ Validators (FluentValidation)
- ✅ Testes de integração (preparados)

## 📝 Observações

- **Banco de Dados**: PostgreSQL 16 (substituiu InMemory para ambiente de produção)
- **Migrations**: Aplicadas automaticamente em desenvolvimento, manual em produção
- **Docker**: Imagem otimizada multi-stage build
- **Kubernetes**: Configurado com 3 réplicas, health checks e auto-scaling ready
- **Health Checks**: Implementados para monitoramento e orquestração

## 🎯 Melhorias de Nível Sênior Implementadas

### Arquitetura
- ✅ Clean Architecture com separação de responsabilidades
- ✅ SOLID principles aplicados rigorosamente
- ✅ DTOs para não expor entidades diretamente
- ✅ AutoMapper para mapeamento
- ✅ Middleware de tratamento de exceções global
- ✅ Unit of Work Pattern
- ✅ Repository Pattern genérico

### Orientação a Objetos
- ✅ BaseEntity com herança
- ✅ BaseHandler com herança
- ✅ Factory Methods (Veiculo.Create)
- ✅ Encapsulamento com propriedades privadas
- ✅ Métodos de atualização controlados (Update)

### Infraestrutura
- ✅ Docker e Docker Compose
- ✅ Kubernetes manifests completos
- ✅ PostgreSQL ao invés de InMemory
- ✅ Health Checks (/health, /health/ready, /health/live)
- ✅ Variáveis de ambiente configuráveis
- ✅ Multi-stage Docker build

### Qualidade
- ✅ Logging estruturado
- ✅ Testes unitários com Moq e FluentAssertions
- ✅ Clean Code
- ✅ Documentação XML nos métodos
- ✅ Retry policies no EF Core


## 🌐 Acesso Rápido (Docker)

Após executar `docker-compose up --build`, acesse:

- **Swagger UI:** http://localhost:8080/swagger
- **Health Check:** http://localhost:8080/health
- **API Base:** http://localhost:8080/api/veiculo

## 👨‍💻 Autor

Desenvolvido como parte do desafio técnico para a posição de Desenvolvedor .NET Sênior.

