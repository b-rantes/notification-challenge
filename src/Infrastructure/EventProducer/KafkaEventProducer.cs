using Confluent.Kafka;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Events;
using Infrastructure.EventProducer.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.EventProducer
{
    public class KafkaEventProducer : IDomainEventsProducer
    {
        private const string NotificationCreatedTopic = "user-notification-created";

        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IProducer<long, string> _producer;

        public KafkaEventProducer(IOptions<KafkaProducerConfiguration> kafkaConfig)
        {
            _producer = new ProducerBuilder<long, string>(kafkaConfig.Value).Build();
        }

        public async Task ProduceNotificationCreatedEvent(Notification notification, User user, CancellationToken cancellationToken)
        {
            try
            {
                var message = new NotificationCreatedEventMessage
                {
                    LastOpenedNotificationDate = user.LastOpenedNotificationDate,
                    NotificationCreationDate = notification.NotificationCreationDate!.Value,
                    NotificationId = notification.NotificationId,
                    UserOwnerId = notification.UserOwnerId,
                    NotificationContent = notification.NotificationContent
                };

                var notificationCreatedEvent = new Message<long, string>
                {
                    Value = JsonSerializer.Serialize(message, _options),
                    Key = user.Id
                };

                await _producer.ProduceAsync(NotificationCreatedTopic, notificationCreatedEvent, cancellationToken);

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ProduceUserOpenedNotificationsEvent(User user, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ProduceUserUpdatedEvent(User user, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
