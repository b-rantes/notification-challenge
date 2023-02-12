using WebApi.Consumers.BaseConsumer;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand;
using WebApi.Consumers.CommandConsumers.UserNotificationsFetched;
using WebApi.Consumers.SyncConsumers.UserNotificationCreated;
using WebApi.Consumers.SyncConsumers.UserSettingsUpdated;

namespace WebApi.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        private const string KafkaConsumerConfigPath = "KafkaConsumerConfig";

        public static IServiceCollection AddWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConsumerConfig>(x => configuration.GetSection(KafkaConsumerConfigPath).Bind(x));

            services
                .AddConsumer<CreateUserNotificationCommandConsumer>()
                .AddConsumer<UserNotificationCreatedConsumer>()
                .AddConsumer<UserNotificationsFetchedConsumer>()
                .AddConsumer<UserSettingsUpdatedConsumer>();

            return services;
        }
    }
}
