using Domain.Contarcts;
using Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistance.Repostories
{
    public class BasketRepo(IConnectionMultiplexer connectionMultiplexer) : IBasketRepo
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<bool> DeleteBasketAsync(string id)
        => await _database.KeyDeleteAsync(id);

        public async Task<CustmoreBasket?> GetBasketAsync(string id)
        {
            var value = await _database.StringGetAsync(id);
            if (value.IsNullOrEmpty)
                return null;
            return JsonSerializer.Deserialize<CustmoreBasket>(value);  
        }
            

        public async Task<CustmoreBasket?> UpdateBasketAsync(CustmoreBasket basket,TimeSpan? TimeToLive=null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedOrUpdate = await _database.StringSetAsync(basket.Id, jsonBasket, TimeToLive ?? TimeSpan.FromDays(30));
            return IsCreatedOrUpdate ? await GetBasketAsync(basket.Id) : null;
        }
    }
}
