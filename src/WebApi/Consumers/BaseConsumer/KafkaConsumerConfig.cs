using Confluent.Kafka;

namespace WebApi.Consumers.BaseConsumer
{
    public class KafkaConsumerConfig : ConsumerConfig
    {
        public string Mailbox { get; set; }
    }
}
