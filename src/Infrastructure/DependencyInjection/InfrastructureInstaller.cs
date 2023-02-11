using Infrastructure.Cache;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureInstaller
    {
        private const string MongoDbConnectionStringPath = "MongoDbConfiguration:ConnectionString";
        private const string MongoDbDatabaseNameStringPath = "MongoDbConfiguration:DatabaseName";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddViewRepositories()
                .AddCommandRepositories()
                .AddCachedViews(configuration)
                .AddMongoDb(configuration);

            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(c =>
            {
                var connectionString = configuration.GetSection(MongoDbConnectionStringPath).Value;
                var databaseName = configuration.GetSection(MongoDbDatabaseNameStringPath).Value;

                var mongoDatabase = new MongoClient(connectionString).GetDatabase(databaseName);
                return mongoDatabase;
            });

            return services;
        }
    }
}
