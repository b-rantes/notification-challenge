using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace WebApi.Consumers.Base
{
    public abstract class BaseConsumer<TInput> : BackgroundService
    {
        private readonly IConsumer<string, byte[]> _consumer;
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public abstract string Mailbox { get; }

        public BaseConsumer(IOptions<KafkaConsumerConfig> config)
        {
            _consumer = new ConsumerBuilder<string, byte[]>(config.Value).Build();
        }

        public abstract Task ExecuteAsync(TInput message, CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);

                        var result = JsonSerializer.Deserialize<TInput>(consumeResult.Message.Value, _options);

                        if (result is not null)
                            await ExecuteAsync(result, stoppingToken);
                    }
                    catch (Exception)
                    {

                    }
                }
            }, stoppingToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(Mailbox);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
