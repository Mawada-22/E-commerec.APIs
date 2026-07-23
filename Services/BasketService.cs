using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using ServicesAbstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepo _basketRepo,IMapper _mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
        => await _basketRepo.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return basket is null ? throw new BasketNotFoundException(id) :  _mapper.Map<BasketDto>(basket);
       }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket,TimeSpan? TimeToLive)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var UpdateBasket = await _basketRepo.UpdateBasketAsync(customerBasket, TimeToLive);
            return UpdateBasket is null ? throw new Exception("Can't Update Basket") : _mapper.Map<BasketDto>(UpdateBasket);

        }
    }
}
