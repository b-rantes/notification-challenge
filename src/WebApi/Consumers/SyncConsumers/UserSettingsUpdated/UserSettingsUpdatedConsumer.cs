using Application.SyncServices.UpdateUserView.Interface;
using Infrastructure.EventProducer;
using Infrastructure.EventProducer.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Consumers.Base;
using WebApi.Consumers.SyncConsumers.UserSettingsUpdated.Mappers;

namespace WebApi.Consumers.SyncConsumers.UserSettingsUpdated
{
    public class UserSettingsUpdatedConsumer : BaseConsumer<UserSettingsUpdatedEventMessage>
    {
        private readonly IUpdateUserViewService _updateUserViewService;
        ILogger<UserSettingsUpdatedConsumer> _logger;

        public UserSettingsUpdatedConsumer(
            IOptions<KafkaConsumerConfig> config,
            IUpdateUserViewService updateUserViewService,
            ILogger<UserSettingsUpdatedConsumer> logger) : base(config)
        {
            _updateUserViewService = updateUserViewService;
            _logger = logger;
        }

        public override string Mailbox => KafkaTopicsConstants.UserSettingsUpdatedTopic;

        public override async Task ExecuteAsync(UserSettingsUpdatedEventMessage message, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{Worker}] consuming message for id: {id}", nameof(UserSettingsUpdatedConsumer), message.UserId);
                
                var input = message.MapMessageToInput();

                await _updateUserViewService.UpdateUserViewAsync(input, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {worker}. Message: {message}",
                    nameof(UserSettingsUpdatedConsumer),
                    JsonSerializer.Serialize(message));
            }
        }
    }
}
