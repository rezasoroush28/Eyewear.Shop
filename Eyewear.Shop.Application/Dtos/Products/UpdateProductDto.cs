using Eyewear.Shop.Domain.Entities;

namespace Eyewear.Shop.Application.Dtos.Products
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public float? DiscountAmount { get; set; }
        public DiscountType? DiscountTyp { get; set; }
        public decimal BasePrice { get; set; }
    }
}

