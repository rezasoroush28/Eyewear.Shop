﻿namespace Eyewear.Shop.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? GatewayTransactionId { get; set; }
        public PaymentStatus Status { get; set; }

        public Order Order { get; set; } = null!;
    }

}
