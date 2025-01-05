using Microsoft.EntityFrameworkCore;
using Product.API.Models;

namespace Product.API.Infrastructure
{
	public class ProductsDbContext : DbContext
	{
		public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }
		public DbSet<Products> Products { get; set; }
	}
}
