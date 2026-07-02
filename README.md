# FIAP Cloud Games - Catalog API

## 📋 Sobre o Projeto

API de Catálogo do sistema **FIAP Cloud Games**, responsável pelo gerenciamento de jogos, bibliotecas de usuários e pedidos. Esta API faz parte de uma arquitetura de microserviços e utiliza mensageria via RabbitMQ para comunicação assíncrona com outros serviços.

### Principais Funcionalidades

- **Gerenciamento de Jogos**: CRUD completo para cadastro e manutenção de jogos no catálogo
- **Bibliotecas de Usuários**: Gerenciamento das bibliotecas pessoais de jogos dos usuários
- **Processamento de Pedidos**: Criação de pedidos e integração com sistema de pagamentos via mensageria

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** com as seguintes camadas:

```
src/
├── FiapCloudGames.Catalogs.API/            # Camada de apresentação (Controllers, Configurações)
├── FiapCloudGames.Catalogs.Application/    # Camada de aplicação (Serviços, DTOs)
├── FiapCloudGames.Catalogs.Domain/         # Camada de domínio (Entidades, Interfaces)
└── FiapCloudGames.Catalogs.Infrastructure/ # Camada de infraestrutura (Repositórios, Mensageria)
```

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework de desenvolvimento
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados relacional
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

Após iniciar a aplicação, acesse a documentação Swagger:

- **Local**: http://localhost:8080/swagger
- **Health Check**: http://localhost:8080/health

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