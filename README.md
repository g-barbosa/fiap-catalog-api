# FIAP Cloud Games - Catalog API

## 📋 Sobre o Projeto

API de Catálogo do sistema **FIAP Cloud Games**, responsável pelo gerenciamento de jogos, bibliotecas de usuários e pedidos. Esta API faz parte de uma arquitetura de microserviços e utiliza mensageria via RabbitMQ para comunicação assíncrona com outros serviços.

### Principais Funcionalidades

- **Gerenciamento de Jogos**: CRUD completo para cadastro e manutenção de jogos no catálogo
- **Bibliotecas de Usuários**: Gerenciamento das bibliotecas pessoais de jogos dos usuários
- **Processamento de Pedidos**: Criação de pedidos e integração com sistema de pagamentos via mensageria
- **Avaliações (MongoDB)**: POST/GET de avaliações por jogo em banco documental
- **Cache (Redis)**: Cache das listagens e detalhes de jogos

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** com as seguintes camadas:

```
src/
├── FiapCloudGames.Catalogs.API/            # Camada de apresentação (Controllers, Configurações)
├── FiapCloudGames.Catalogs.Application/    # Camada de aplicação (Serviços, DTOs)
├── FiapCloudGames.Catalogs.Domain/         # Camada de domínio (Entidades, Interfaces)
└── FiapCloudGames.Catalogs.Infrastructure/ # Camada de infraestrutura (SQL, Redis, Mongo, Mensageria)
```

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework de desenvolvimento
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional (jogos, pedidos, bibliotecas)
- **MongoDB** - NoSQL para avaliações de jogos
- **Redis** - Cache distribuído
- **RabbitMQ** - Mensageria para comunicação assíncrona
- **Swagger/OpenAPI** - Documentação da API
- **Docker** - Containerização
- **Kubernetes** - Orquestração de containers

## ⚙️ Variáveis de Ambiente

### Configurações Gerais

| Variável | Descrição | Valor Padrão |
|----------|-----------|--------------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente de execução (`Development`, `Production`) | `Production` |
| `ASPNETCORE_URLS` | URLs de binding da aplicação | `http://+:8080` |
| `TZ` | Timezone da aplicação | `America/Sao_Paulo` |

### Banco de Dados

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `ConnectionStrings__DefaultConnection` | String de conexão do SQL Server | `Server=sqlserver,1433;Database=FiapCloudGamesDb;User Id=SA;Password=SuaSenha;TrustServerCertificate=True` |
| `ConnectionStrings__Redis` | Endpoint do Redis | `redis:6379` |
| `ConnectionStrings__MongoDb` | Connection string do MongoDB | `mongodb://admin:mongo123@mongodb:27017` |
| `MongoDb__Database` | Nome do database MongoDB | `FiapCloudGamesCatalog` |
| `Cache__JogosTtlSeconds` | TTL do cache de jogos (segundos) | `300` |

### RabbitMQ

| Variável | Descrição | Valor Padrão |
|----------|-----------|--------------|
| `RabbitMq__Host` | Host do servidor RabbitMQ | `rabbitmq` |
| `RabbitMq__Port` | Porta do servidor RabbitMQ | `5672` |
| `RabbitMq__Username` | Usuário de autenticação | `admin` |
| `RabbitMq__Password` | Senha de autenticação | - |

### Logging

| Variável | Descrição | Valor Padrão |
|----------|-----------|--------------|
| `Logging__LogLevel__Default` | Nível de log padrão | `Information` |
| `Logging__LogLevel__Microsoft.AspNetCore` | Nível de log do ASP.NET Core | `Warning` |
| `Logging__LogLevel__Microsoft.EntityFrameworkCore` | Nível de log do EF Core | `Warning` |

## 🚀 Como Executar

### Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose (opcional)
- SQL Server
- Redis
- MongoDB
- RabbitMQ

### Execução Local

```bash
# Restaurar dependências
cd src/FiapCloudGames.Catalogs.API
dotnet restore

# Executar a aplicação
dotnet run
```

### Execução com Docker

```bash
# Build da imagem
docker build -t fiap-catalog-api .

# Executar o container
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="sua-connection-string" \
  -e RabbitMq__Host="rabbitmq" \
  -e RabbitMq__Username="admin" \
  -e RabbitMq__Password="sua-senha" \
  fiap-catalog-api
```

### Execução no Kubernetes

```bash
# Criar namespace
kubectl create namespace fiap-cloud-games

# Aplicar configurações
kubectl apply -f k8s/
```

> **Nota**: Certifique-se de atualizar os valores nos arquivos `k8s/secret.yaml` e `k8s/configmap.yaml` antes de aplicar em produção.

## 📚 Documentação da API

Com o stack via `fiap-orchestration` (`docker compose up`):

- **Swagger (CatalogAPI)**: http://localhost:8082/swagger
- **Health Check**: http://localhost:8082/health

> Se rodar só com `dotnet run` (perfil https), a porta muda (ex.: `7165`). Para validar Redis/Mongo do compose, use sempre a porta **8082**.

## ✅ Como testar Redis e MongoDB (Fase 3)

### 1. Subir o ambiente

```bash
cd ../fiap-orchestration
docker compose up -d --build
```

Aguarde o `catalog-api` ficar healthy e abra http://localhost:8082/swagger.

### 2. Criar um jogo (SQL Server)

No Swagger, execute `POST /api/Jogos`:

```json
{
  "titulo": "Meu Jogo",
  "descricao": "Teste de cache",
  "preco": 99.90
}
```

Guarde o `id` retornado.

### 3. Validar o cache Redis

1. Execute `GET /api/Jogos` **duas vezes** no Swagger (1ª grava no Redis; 2ª deve ler do cache).
2. No terminal, confira as chaves:

```bash
# Liste as chaves
docker exec redis redis-cli KEYS "*"

# Veja o conteúdo (IDistributedCache grava como HASH)
docker exec redis redis-cli HGETALL "fiap-catalog:jogos:all"

# TTL restante (segundos; ~300 = 5 min)
docker exec redis redis-cli TTL "fiap-catalog:jogos:all"
```

**O que esperar**
- Chave `fiap-catalog:jogos:all` (prefixo `fiap-catalog:` do InstanceName)
- Campo `data` com o JSON da lista de jogos
- TTL próximo de `300` e diminuindo

Detalhe por id (após `GET /api/Jogos/{id}`):

```bash
docker exec redis redis-cli KEYS "fiap-catalog:jogos:*"
docker exec redis redis-cli HGETALL "fiap-catalog:jogos:{id-do-jogo}"
```

### 4. Validar avaliações no MongoDB

No Swagger, com o `id` do jogo:

1. `POST /api/Jogos/{id}/avaliacoes`

```json
{
  "usuarioId": "00000000-0000-0000-0000-000000000001",
  "nomeUsuario": "Gabriel",
  "nota": 5,
  "comentario": "Excelente!"
}
```

2. `GET /api/Jogos/{id}/avaliacoes` — deve listar a avaliação criada.

Opcional (conferir no Mongo):

```bash
docker exec mongodb mongosh -u admin -p mongo123 --authenticationDatabase admin --eval "db.getSiblingDB('FiapCloudGamesCatalog').avaliacoes.find().pretty()"
```

## 📁 Estrutura do Kubernetes

```
k8s/
├── configmap.yaml   # Configurações não sensíveis
├── secret.yaml      # Configurações sensíveis (senhas, connection strings)
├── deployment.yaml  # Definição do deployment
└── service.yaml     # Definição do service
```

## 🔒 Segurança

- Em produção, utilize um gerenciador de secrets externo (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault)
- Nunca commite secrets ou senhas reais no repositório
- Utilize variáveis de ambiente para configurações sensíveis