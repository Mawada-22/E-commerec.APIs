using AutoMapper;
using Domain.Contarcts;
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
        public async Task<bool> DeletebasketAsync(string id)
        => await _basketRepo.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAync(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return basket is null ? throw new BasketNotFountException(id) :  _mapper.Map<BasketDto>(basket);
       }

        public async Task<BasketDto> updateBasketAsync(BasketDto basket,TimeSpan? TimeToLive)
        {
            var CustmoerBasket = _mapper.Map<CustmoreBasket>(basket);
            var UpdateBasket = await _basketRepo.UpdateBasketAsync(CustmoerBasket, TimeToLive);
            return UpdateBasket is null ? throw new Exception("Can't Update Basket") : _mapper.Map<BasketDto>(UpdateBasket);

        }
    }
}
