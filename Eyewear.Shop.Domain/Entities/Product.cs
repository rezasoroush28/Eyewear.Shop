namespace Eyewear.Shop.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    public float? DiscountAmount { get; set; }
    public DiscountType? DiscountTyp { get; set; }
    public decimal BasePrice { get; set; }

    public decimal ApplyDiscount(decimal basePrice)
    {
        if (DiscountAmount == null || DiscountTyp == null)
            return basePrice;

        return DiscountTyp switch
        {
            DiscountType.Percentage => basePrice - (basePrice * (decimal)(DiscountAmount / 100f)),
            DiscountType.FixedAmount => basePrice - (decimal)DiscountAmount,
            _ => basePrice
        };
    }


}

public class ProductAttribute
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
