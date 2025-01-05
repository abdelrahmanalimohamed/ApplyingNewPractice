using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure;
using Product.API.Models;

namespace Product.API.GetProducts
{
	public record GetProductQuery() : IQuery<GetProductResult>;
	public record GetProductResult(List<Products> Products);
	public class GetProductsHandler : IQueryHandler<GetProductQuery, GetProductResult>
	{
		private readonly ProductsDbContext productsDbContext;

		public GetProductsHandler(ProductsDbContext productsDbContext)
		{
			this.productsDbContext = productsDbContext;
		}
		public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var results = await productsDbContext.Products.ToListAsync();

			var response = new GetProductResult(results);
			return response;
		}
	}
}