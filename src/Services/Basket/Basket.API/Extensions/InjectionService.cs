using Basket.API.GrpcServices;
using Basket.API.Repositories;
using MassTransit;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API.Extensions
{
    public static class InjectionService
    {
        public static WebApplicationBuilder InjectCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddGrpcClient<DiscountProtoServiceClient>(
                o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

            builder.Services.AddScoped<DiscountGrpcService>();

            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg0) =>
                {
                    cfg0.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });
            builder.Services.AddAutoMapper(typeof(Program));

            return builder;
        }
    }
}
