using Basket.API.Repositories;

namespace Basket.API.Extensions
{
    public static class InjectionService
    {
        public static IServiceCollection InjectCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
    }
}
