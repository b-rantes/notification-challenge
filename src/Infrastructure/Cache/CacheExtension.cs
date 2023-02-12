using Infrastructure.Cache.CachedNotificationView;
using Infrastructure.Cache.CachedUserView;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache
{
    internal static class CacheExtension
    {
        private const string RedisConnectionStringPath = "RedisCacheConfiguration:ConnectionString";
        public static IServiceCollection AddCachedViews(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICachedUserViewRepository, CachedUserViewRepository>();
            services.AddTransient<ICachedNotificationViewRepository, CachedNotificationViewRepository>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection(RedisConnectionStringPath).Value;
            });

            return services;
        }
    }
}
