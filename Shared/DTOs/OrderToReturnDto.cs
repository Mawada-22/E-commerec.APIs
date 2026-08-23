using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public record OrderToReturnDto
    {
        public int Id { get; init; }
        public string BuyerEmail { get; init; }
        public DateTimeOffset OrderDate { get; init; }
        public ShippingAddressDto ShipToAddress { get; init; }
        public string DeliveryMethod { get; init; }
        public decimal DeliveryCost { get; init; }
        public string Status { get; init; }
        public IEnumerable<OrderItemDto> Items { get; init; }
        public decimal Subtotal { get; init; }
        public decimal Total { get; init; }
    }
}
