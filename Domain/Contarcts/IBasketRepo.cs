using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contarcts
{
    public interface IBasketRepo
    {
        //Get Basket By ID
        public Task<CustmoreBasket?> GetBasketAsync(string id);
        //Delete basket
        public Task<bool> DeleteBasketAsync(string id);
        //CreateUpdate Basket
        public Task<CustmoreBasket?> UpdateBasketAsync(CustmoreBasket basket, TimeSpan? TimeToLive = null);

    }
}
