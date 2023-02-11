using Infrastructure.Cache.CachedUserView;
using Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Cache
{
    internal static class CacheExtension
    {
        private const string RedisConnectionStringPath = "RedisCacheConfiguration:ConnectionString";
        public static IServiceCollection AddCachedViews(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICachedUserViewRepository, CachedUserViewRepository>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection(RedisConnectionStringPath).Value;
            });

            return services;
        }
    }
}
