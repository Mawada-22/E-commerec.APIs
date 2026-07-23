using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Specifications;
using ServicesAbstractions;
using Shared.DTOs;
using Stripe;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class PaymentService(
        IBasketRepo _basketRepo,
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IConfiguration _configuration) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Empty string is as bad as missing - Stripe would reject it with a
            // confusing 500; fail fast with an actionable message instead.
            var stripeKey = _configuration["StripeSettings:SecretKey"];
            if (string.IsNullOrWhiteSpace(stripeKey))
                throw new InvalidOperationException(
                    "Stripe is not configured. Set your key with: dotnet user-secrets set \"StripeSettings:SecretKey\" \"sk_test_...\" --project E-commerce.Apis");
            StripeConfiguration.ApiKey = stripeKey;

            //1- load the basket
            var basket = await _basketRepo.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);
            if (basket.Items is null || !basket.Items.Any())
                throw new EmptyBasketException(basketId);

            //2- shipping price from the chosen delivery method (0 until one is chosen)
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.GetRepo<int, DeliveryMethod>()
                    .GetByIdAsync(basket.DeliveryMethodId.Value)
                    ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Price;
            }

            //3- re-price every line from the Product table - never charge the
            //   price sitting in the client's basket
            var productRepo = _unitOfWork.GetRepo<int, Domain.Entities.Product>();
            var items = basket.Items.ToList();
            foreach (var item in items)
            {
                var product = await productRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            basket.Items = items;

            //4- Stripe amounts are in the smallest currency unit (cents/piasters)
            var amount = (long)((items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100);

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var intent = await service.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                });
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // Basket changed since the intent was created - keep the same intent,
                // update the amount so the customer is charged the current total.
                await service.UpdateAsync(basket.PaymentIntentId, new PaymentIntentUpdateOptions
                {
                    Amount = amount
                });
            }

            //5- persist the checkout state back onto the basket
            var updated = await _basketRepo.UpdateBasketAsync(basket)
                ?? throw new Exception("Can't update basket with payment intent.");

            return _mapper.Map<BasketDto>(updated);
        }

        public async Task UpdateOrderPaymentStatusAsync(string paymentIntentId, bool succeeded)
        {
            var orderRepo = _unitOfWork.GetRepo<int, Order>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpecifications(paymentIntentId));
            if (order is null) return; // payment for a basket that never became an order

            order.Status = succeeded ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
