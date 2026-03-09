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
        public Task<BasketDto> GetBasketAync(string id);
        //delete
        public Task<bool> DeletebasketAsync (string id);
        //update
        public Task<BasketDto> updateBasketAsync (BasketDto basket, TimeSpan? TimeToLive);

    }
}
