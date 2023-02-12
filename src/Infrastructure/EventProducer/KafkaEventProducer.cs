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
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IProducer<string, string> _producer;

        public KafkaEventProducer(IOptions<KafkaProducerConfiguration> kafkaConfig)
        {
            _producer = new ProducerBuilder<string, string>(kafkaConfig.Value).Build();
        }

        public async Task ProduceNotificationCreatedEvent(Notification notification, User user, CancellationToken cancellationToken)
        {
            try
            {
                var message = new UserNotificationCreatedEventMessage
                {
                    LastOpenedNotificationDate = user.LastOpenedNotificationDate,
                    NotificationCreationDate = notification.NotificationCreationDate,
                    NotificationId = notification.NotificationId,
                    UserOwnerId = notification.UserOwnerId,
                    NotificationContent = notification.NotificationContent
                };

                var notificationCreatedEvent = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(message, _options),
                    Key = user.Id.ToString()
                };

                await _producer.ProduceAsync(KafkaTopicsConstants.UserNotificationCreatedTopic, notificationCreatedEvent, cancellationToken);

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
                var message = new UserOpenedNotificationsEventMessage
                {
                    UserId = user.Id,
                    LastOpenedNotificationDate = user.LastOpenedNotificationDate!.Value
                };

                var userOpenedNotificationsEvent = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(message, _options),
                    Key = message.UserId.ToString()
                };

                await _producer.ProduceAsync(KafkaTopicsConstants.UserOpenedNotificationsTopic, userOpenedNotificationsEvent, cancellationToken);

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ProduceUserSettingsUpdatedEvent(ProduceUserSettingsUpdatedInput userSettingsUpdatedInput, CancellationToken cancellationToken)
        {
            try
            {
                var message = new UserSettingsUpdatedEventMessage
                {
                    UserId = userSettingsUpdatedInput.Id,
                    LastOpenedNotificationDate = userSettingsUpdatedInput.LastOpenedNotificationDate.HasValue ?
                        userSettingsUpdatedInput.LastOpenedNotificationDate.Value : null,
                    CanReceiveNotification = userSettingsUpdatedInput.CanReceiveNotification.HasValue ?
                        userSettingsUpdatedInput.CanReceiveNotification : null
                };

                var userSettingsUpdatedEvent = new Message<string, string>
                {
                    Value = JsonSerializer.Serialize(message, _options),
                    Key = message.UserId.ToString()
                };

                await _producer.ProduceAsync(KafkaTopicsConstants.UserSettingsUpdatedTopic, userSettingsUpdatedEvent, cancellationToken);

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
