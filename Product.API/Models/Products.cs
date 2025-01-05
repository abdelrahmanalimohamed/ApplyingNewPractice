using System.ComponentModel.DataAnnotations;

namespace Product.API.Models
{
	public class Products
	{
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}