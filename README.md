


# DynamoDbCrudApi

Este projeto é uma API RESTful construída com .NET 7 que realiza operações CRUD (Create, Read, Update, Delete) em uma tabela DynamoDB. A API é configurada para funcionar com o AWS SDK para .NET e utiliza o `DynamoDBContext` para interagir com o DynamoDB.

## Estrutura do Projeto

- **Controllers**: Contém o controlador da API (`ProductsController`).
- **Data**: Contém a configuração do contexto DynamoDB (`DynamoDbContext`).
- **Models**: Contém a classe de modelo (`Product`).
- **Repositories**: Contém o repositório que implementa as operações CRUD (`ProductRepository`).

## Requisitos

- .NET 7 SDK
- Conta AWS com DynamoDB configurado
- Perfil de credenciais AWS configurado no seu ambiente (ex.: via `~/.aws/credentials`)

## Configuração

### Passo 1: Clonar o Repositório

git clone https://github.com/seu-usuario/DynamoDbCrudApi.git
cd DynamoDbCrudApi
```

### Passo 2: Configurar Dependências

Instale os pacotes necessários executando o comando:

```bash
dotnet restore
```

### Passo 3: Configurar Credenciais da AWS

Adicione um arquivo `appsettings.json` na raiz do projeto com as configurações da AWS:

```json
{
  "AWS": {
    "Profile": "default",
    "Region": "us-west-2"
  }
}
```

### Passo 4: Executar a Aplicação

Execute a aplicação com o comando:

```bash
dotnet run
```

A aplicação estará disponível em `http://localhost:5000`.

## Endpoints da API

### GET /api/products

Retorna todos os produtos.

**Resposta:**

```json
[
  {
    "id": "string",
    "name": "string",
    "price": decimal
  }
]
```

### GET /api/products/{id}

Retorna um produto específico pelo ID.

**Resposta:**

```json
{
  "id": "string",
  "name": "string",
  "price": decimal
}
```

### POST /api/products

Cria um novo produto.

**Corpo da Requisição:**

```json
{
  "id": "string",
  "name": "string",
  "price": decimal
}
```

**Resposta:**

```json
{
  "id": "string",
  "name": "string",
  "price": decimal
}
```

### PUT /api/products/{id}

Atualiza um produto existente.

**Corpo da Requisição:**

```json
{
  "id": "string",
  "name": "string",
  "price": decimal
}
```

**Resposta:**

`204 No Content`

### DELETE /api/products/{id}

Exclui um produto pelo ID.

**Resposta:**

`204 No Content`

## Estrutura do Código

### Models/Product.cs

Define a classe `Product` que representa a entidade de produto no DynamoDB.

```csharp
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbCrudApi.Models
{
    [DynamoDBTable("Products")]
    public class Product
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
```

### Data/DynamoDbContext.cs

Configura o contexto do DynamoDB.

```csharp
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Options;

namespace DynamoDbCrudApi.Data
{
    public class DynamoDbContext
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly DynamoDBContext _context;

        public DynamoDbContext(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(_dynamoDbClient);
        }

        public DynamoDBContext Context => _context;
    }
}
```

### Repositories/ProductRepository.cs

Implementa as operações CRUD.

```csharp
using Amazon.DynamoDBv2.DataModel;
using DynamoDbCrudApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDbCrudApi.Repositories
{
    public class ProductRepository
    {
        private readonly DynamoDBContext _context;

        public ProductRepository(Data.DynamoDbContext dbContext)
        {
            _context = dbContext.Context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Product>(conditions).GetRemainingAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Product>(id);
        }

        public async Task SaveAsync(Product product)
        {
            await _context.SaveAsync(product);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync<Product>(id);
        }
    }
}
```

### Controllers/ProductsController.cs

Define os endpoints da API.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDbCrudApi.Repositories;
using DynamoDbCrudApi.Models;

namespace DynamoDbCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await _repository.SaveAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _repository.SaveAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
```

## Contribuições

Sinta-se à vontade para contribuir com este projeto, enviando pull requests ou relatando problemas no repositório.

## Licença

Este projeto está licenciado sob a Licença MIT. Consulte o arquivo LICENSE para obter mais informações.
```

Este `README.md` fornece uma visão geral abrangente do projeto, instruções para configuração, detalhes dos endpoints da API e uma descrição da estrutura do código. Certifique-se de ajustar os links, o nome do repositório e outras informações específicas conforme necessário.
