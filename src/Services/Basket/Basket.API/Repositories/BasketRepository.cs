using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Basket.API.Repositories
{
    public class BasketRepository: IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if(String.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart?> UpdateBasket(ShoppingCart cart)
        {
            if (cart.UserName != null)
            {
                await _redisCache.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));
            }
            return await GetBasket(cart.UserName);
        }
    }
}
