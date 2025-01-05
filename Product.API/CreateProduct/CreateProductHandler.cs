using BuildingBlocks.CQRS;
using Product.API.Infrastructure;
using Product.API.Models;

namespace Product.API.CreateProduct
{
	public record CreateProductCommand(string Name , decimal Price) : ICommand<CreateProductResult>;
	public record CreateProductResult(Guid id);
	public class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		private readonly ProductsDbContext _productsDbContext;
		public CreateProductHandler(ProductsDbContext _productsDbContext)
		{
			this._productsDbContext = _productsDbContext;
		}
		public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = new Products
			{
				Id = Guid.NewGuid(),
				Name = request.Name,
				Price = request.Price
			};

			await _productsDbContext.Products.AddAsync(product);
			await _productsDbContext.SaveChangesAsync();

			var response = new CreateProductResult(product.Id);

			return response;

		}
	}
}
