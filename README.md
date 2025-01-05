# CQRS-Based API with BuildingBlocks and Product.API

This repository demonstrates a modular and extensible CQRS (Command Query Responsibility Segregation) pattern-based implementation using .NET 7, MediatR, and ICarter for building APIs. The project is divided into two main projects:

- **BuildingBlocks**: A reusable library that defines core abstractions and patterns (e.g., CQRS interfaces).
- **Product.API**: An example implementation of a product API (minimal api) using the abstractions defined in BuildingBlocks.

## Features

- **CQRS Pattern**: Separation of commands and queries for better scalability and maintainability.
- **MediatR Integration**: Simplifies the implementation of handlers for commands and queries.
- **ICarter**: Lightweight, modular routing for ASP.NET Core.
- **EF Core**: Integration with Entity Framework Core for database operations.
- **Clean Architecture**: Separation of concerns between reusable libraries and API implementation.

## Project Structure

### BuildingBlocks

This project contains reusable abstractions for CQRS:

- `ICommand` and `ICommandHandler`: Define commands and their handlers.
- `IQuery` and `IQueryHandler`: Define queries and their handlers.

Example:

```csharp
public interface ICommand : ICommand<Unit> { }
public interface ICommand<out TResponse> : IRequest<TResponse> { }
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull { }
```

### Product.API

This is the main API project that demonstrates the usage of the CQRS abstractions from BuildingBlocks. It includes:

- **Features**: Modular structure with folders like `CreateProduct` for each feature.
- **Infrastructure**: Database context using EF Core.
- **Routing**: Defined using ICarter for modular and clean endpoint definitions.

#### Example: Creating a Product

- **Request and Response Models**:

```csharp
public record CreateProductRequest(string Name, decimal Price);
public record CreateProductResponse(Guid Id);
```

- **Command and Handler**:

```csharp
public record CreateProductCommand(string Name, decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly ProductsDbContext _productsDbContext;

    public CreateProductHandler(ProductsDbContext productsDbContext)
    {
        _productsDbContext = productsDbContext;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Products { Id = Guid.NewGuid(), Name = request.Name, Price = request.Price };
        await _productsDbContext.Products.AddAsync(product, cancellationToken);
        await _productsDbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
```

- **Endpoint**:

```csharp
public class CreateProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"/products/{response.Id}", response);
        });
    }
}
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Entity Framework Core
- MediatR
- ICarter

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   ```
2. Navigate to the solution folder:
   ```bash
   cd your-repo-name
   ```

### Running the Application

1. Build the solution:
   ```bash
   dotnet build
   ```
2. Run the API project:
   ```bash
   dotnet run --project Product.API
   ```
3. The API will be available at `http://localhost:5000` (or another port configured in `launchSettings.json`).

### Example API Requests

#### Create a Product

**POST** `/products`

```json
{
  "name": "Sample Product",
  "price": 99.99
}
```

#### Get All Products

**GET** `/products`


## Notes

- This project is for **learning purposes** and uses an **in-memory database** for simplicity.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

