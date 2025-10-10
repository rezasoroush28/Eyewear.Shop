namespace Eyewear.Shop.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public ProductVariant ProductVarient { get; set; 
        public Order Order { get; set; } = null!;
    }

}
