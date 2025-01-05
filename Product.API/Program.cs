using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure;

namespace Product.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddCarter();
			builder.Services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
			});

			builder.Services.AddDbContext<ProductsDbContext>(options =>
								options.UseInMemoryDatabase("ProductsDb"));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.MapCarter();

			app.Run();
		}
	}
}
