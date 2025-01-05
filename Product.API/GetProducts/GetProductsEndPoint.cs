using Product.API.CreateProduct;
using Product.API.Models;

namespace Product.API.GetProducts
{
	//public record GetProductQuery();
	public record GetProductResponse(List<Products> Products);
	public class GetProductsEndPoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/getProducts",
				async (ISender sender) =>
				{
					var result = await sender.Send(new GetProductQuery());
					var response = result.Adapt<GetProductResponse>();
					return Results.Ok(response);
				});
		}
	}
}
