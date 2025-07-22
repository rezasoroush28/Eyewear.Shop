namespace Eyewear.Shop.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
