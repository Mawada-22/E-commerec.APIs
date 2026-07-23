using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Specifications;
using ServicesAbstractions;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(IUnitOfWork _unitOfWork, IBasketService _basketService, IMapper _mapper) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDto createOrderDto, string buyerEmail)
        {
            //1- get the basket
            var basket = await _basketService.GetBasketAsync(createOrderDto.BasketId);
            if (basket is null || basket.Items is null || !basket.Items.Any())
                throw new EmptyBasketException(createOrderDto.BasketId);

            //2- get the delivery method
            var deliveryMethod = await _unitOfWork.GetRepo<int, DeliveryMethod>().GetByIdAsync(createOrderDto.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(createOrderDto.DeliveryMethodId);

            //3- re-price every basket line from the Product table - never trust the price sitting in the basket/client
            var productRepo = _unitOfWork.GetRepo<int, Product>();
            var orderItems = new List<OrderItem>();

            foreach (var basketItem in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(basketItem.Id)
                    ?? throw new ProductNotFoundException(basketItem.Id);

                orderItems.Add(new OrderItem
                {
                    Product = new ProductItemOrdered
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    },
                    Price = product.Price,
                    Quantity = basketItem.Quantity
                });
            }

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            //4- create and persist the order
            var order = new Order
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = _mapper.Map<ShippingAddress>(createOrderDto.ShipToAddress),
                DeliveryMethodId = deliveryMethod.Id,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal,
                PaymentIntentId = basket.PaymentIntentId
            };

            var orderRepo = _unitOfWork.GetRepo<int, Order>();
            await orderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            //5- the basket has served its purpose - clear it (after the order is
            //   safely persisted, so a failed save never wipes the customer's cart)
            await _basketService.DeleteBasketAsync(createOrderDto.BasketId);

            return _mapper.Map<OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepo<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _unitOfWork.GetRepo<int, Order>().GetByIdAsync(new OrderSpecifications(id, buyerEmail));
            return order is null ? throw new OrderNotFoundException(id) : _mapper.Map<OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await _unitOfWork.GetRepo<int, Order>().GetAllAsync(new OrderSpecifications(buyerEmail));
            return _mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
        }
    }
}
