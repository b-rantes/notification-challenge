using Infrastructure.Cache;
using Infrastructure.EventProducer;
using Infrastructure.Repositories;
using Infrastructure.Repositories.DTOs;
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
                .AddMongoDb(configuration)
                .AddKafkaEventProducer(configuration);

            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(c =>
            {
                var connectionString = configuration.GetSection(MongoDbConnectionStringPath).Value;
                var databaseName = configuration.GetSection(MongoDbDatabaseNameStringPath).Value;

                var mongoDatabase = new MongoClient(connectionString).GetDatabase(databaseName);

                var compoundIndexNotificationCollection = Builders<NotificationCommandCollection>.IndexKeys
                .Ascending(notification => notification.UserOwnerId).Ascending(notification => notification.NotificationCreationDate);

                var uniqueIndexNotificationCollection = Builders<NotificationCommandCollection>.IndexKeys
                .Ascending(notification => notification.NotificationId);

                var notificationCollection = mongoDatabase.GetCollection<NotificationCommandCollection>(CollectionsConstants.NotificationCollectionName);

                notificationCollection.Indexes
                .CreateOneAsync(new CreateIndexModel<NotificationCommandCollection>(compoundIndexNotificationCollection, new CreateIndexOptions
                {
                    Background = true,
                    Unique = false
                }));

                notificationCollection.Indexes
                .CreateOneAsync(new CreateIndexModel<NotificationCommandCollection>(uniqueIndexNotificationCollection, new()
                {
                    Background = true,
                    Unique = true
                }));

                var uniqueUserIdIndex = Builders<UserViewCollection>.IndexKeys
                .Ascending(user => user.Id);

                var userCollection = mongoDatabase.GetCollection<UserViewCollection>(CollectionsConstants.UserCollectionName);

                userCollection.Indexes.CreateOneAsync(new CreateIndexModel<UserViewCollection>(uniqueUserIdIndex, new()
                {
                    Background = true,
                    Unique = true
                }));

                return mongoDatabase;
            });

            return services;
        }
    }
}
