using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IBasketService
    {
        //Get
        public Task<BasketDto> GetBasketAsync(string id);
        //delete
        public Task<bool> DeleteBasketAsync (string id);
        //update
        public Task<BasketDto> UpdateBasketAsync (BasketDto basket, TimeSpan? TimeToLive);

    }
}
