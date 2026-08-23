using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IOrderService
    {
        public Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDto createOrderDto, string buyerEmail);
        public Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail);
        public Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail);
        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
