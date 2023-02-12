using Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EventProducer
{
    public static class KafkaEventProducerExtension
    {
        private const string KafkaProducerConfigPath = "KafkaProducerConfiguration";
        public static IServiceCollection AddKafkaEventProducer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDomainEventsProducer, KafkaEventProducer>();
            services.Configure<KafkaProducerConfiguration>(x => configuration.GetSection(KafkaProducerConfigPath).Bind(x));

            return services;
        }
    }
}
