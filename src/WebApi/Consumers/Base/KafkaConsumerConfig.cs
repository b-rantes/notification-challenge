using Confluent.Kafka;

namespace WebApi.Consumers.Base
{
    public class KafkaConsumerConfig : ConsumerConfig
    {
        public string Mailbox { get; set; }
    }
}
