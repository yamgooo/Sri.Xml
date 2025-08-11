using Microsoft.Extensions.DependencyInjection;

namespace Yamgooo.SRI.Xml;

public static class SriXmlServiceCollectionExtensions
{
    public static IServiceCollection AddSriXmlService(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddSingleton<ISriXmlService, SriXmlService>();
        return services;
    }

    public static IServiceCollection AddSriXmlService(this IServiceCollection services, ServiceLifetime lifetime)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<ISriXmlService, SriXmlService>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<ISriXmlService, SriXmlService>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<ISriXmlService, SriXmlService>();
                break;
            default:
                services.AddSingleton<ISriXmlService, SriXmlService>();
                break;
        }
        return services;
    }
}


