using Confluent.Kafka;
using Infrastructure.EventProducer;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace WebApi.Consumers.Base
{
    public class BaseProducer : IBaseProducer
    {
        private readonly IProducer<string, string> _producer;
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public BaseProducer(IOptions<KafkaProducerConfiguration> config)
        {
            _producer = new ProducerBuilder<string, string>(config.Value).Build();
        }

        public async Task ProduceAsync(string topic, string key, object message, CancellationToken cancellationToken)
        {
            try
            {
                var eventMessage = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(message, _options),
                    Key = key
                };

                await _producer.ProduceAsync(topic, eventMessage, cancellationToken);
            }
            catch (Exception)
            {

            }
        }
    }
}
