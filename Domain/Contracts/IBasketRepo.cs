using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepo
    {
        //Get Basket By ID
        public Task<CustomerBasket?> GetBasketAsync(string id);
        //Delete basket
        public Task<bool> DeleteBasketAsync(string id);
        //CreateUpdate Basket
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null);

    }
}
