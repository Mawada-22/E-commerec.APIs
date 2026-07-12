using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation
{
    [Authorize]
    public class OrderController(IServiceManger _serviceManger) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManger.OrderService.CreateOrderAsync(createOrderDto, buyerEmail!);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManger.OrderService.GetOrdersForUserAsync(buyerEmail!);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManger.OrderService.GetOrderByIdAsync(id, buyerEmail!);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var result = await _serviceManger.OrderService.GetDeliveryMethodsAsync();
            return Ok(result);
        }
    }
}
