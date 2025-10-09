using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nest.JoinField;

namespace Eyewear.Shop.Domain.Entities
{

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PaymentMethod { get; set; }
        public string? TrackingCode { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
    }

}
