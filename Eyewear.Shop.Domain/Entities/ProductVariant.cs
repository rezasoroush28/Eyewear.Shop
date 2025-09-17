namespace Eyewear.Shop.Domain.Entities;

public class ProductVariant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal VarientPrice { get; set; }
    public List<ProductAttribute> VarientAttributes { get; set; } = new();
}
