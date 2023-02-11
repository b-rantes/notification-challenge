using MongoDB.Driver;

namespace WebApi.DependencyInjection
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
