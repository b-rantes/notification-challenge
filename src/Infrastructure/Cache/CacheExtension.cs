using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache
{
    internal static class CacheExtension
    {
        public static IServiceCollection AddCachedViews(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICachedUserViewRepository, CachedUserViewRepository>();

            return services;
        }
    }
}
