using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Order : ModelBase<int>
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public ShippingAddress ShippingAddress { get; set; }

        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        //captured from Items at creation time (trusted, server-side prices) rather than recomputed on every read
        public decimal SubTotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Price ?? 0);
    }
}
