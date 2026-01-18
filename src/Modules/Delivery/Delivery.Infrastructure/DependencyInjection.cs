using Delivery.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDeliveryModule(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        return services;
    }
}
