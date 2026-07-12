using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public record CreateOrderDto
    {
        [Required]
        public string BasketId { get; init; }
        public int DeliveryMethodId { get; init; }
        [Required]
        public ShippingAddressDto ShippingAddress { get; init; }
    }
}
