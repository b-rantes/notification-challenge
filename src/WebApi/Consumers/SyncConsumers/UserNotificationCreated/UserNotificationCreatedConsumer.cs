using Application.SyncServices.UpdateNotificationView.Interface;
using Infrastructure.EventProducer;
using Infrastructure.EventProducer.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Consumers.BaseConsumer;
using WebApi.Consumers.SyncConsumers.UserNotificationCreated.Mappers;

namespace WebApi.Consumers.SyncConsumers.UserNotificationCreated
{
    public class UserNotificationCreatedConsumer : BaseConsumer<UserNotificationCreatedEventMessage>
    {
        private readonly IUpdateNotificationViewService _updateNotificationViewService;
        ILogger<UserNotificationCreatedConsumer> _logger;

        public UserNotificationCreatedConsumer(
            IOptions<KafkaConsumerConfig> config,
            IUpdateNotificationViewService updateNotificationViewService,
            ILogger<UserNotificationCreatedConsumer> logger) : base(config)
        {
            _updateNotificationViewService = updateNotificationViewService;
            _logger = logger;
        }

        public override string Mailbox => KafkaTopicsConstants.UserNotificationCreatedTopic;

        public override async Task ExecuteAsync(UserNotificationCreatedEventMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var input = message.MapMessageToInput();

                await _updateNotificationViewService.UpdateNotificationViewAsync(input, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {worker}. Message: {message}",
                    nameof(UserNotificationCreatedConsumer),
                    JsonSerializer.Serialize(message));
            }
        }
    }
}
