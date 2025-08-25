namespace Eyewear.Shop.Application.Dtos.Products
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
