using MongoDB.Driver;

namespace WebApi.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        private const string MongoDbConfigurationPath = "MongoDbConfiguration:ConnectionString";

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(c =>
            {
                var connectionString = configuration.GetSection(MongoDbConfigurationPath).Value;
                var mongoClient = new MongoClient(connectionString);
                return mongoClient;
            });

            return services;
        }
    }
}
