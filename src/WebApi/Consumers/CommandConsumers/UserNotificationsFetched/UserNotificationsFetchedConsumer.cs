using Application.UseCases.UpsertUserControl.Interface;
using Infrastructure.EventProducer;
using Infrastructure.EventProducer.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Consumers.Base;
using WebApi.Consumers.CommandConsumers.UserNotificationsFetched.Mappers;

namespace WebApi.Consumers.CommandConsumers.UserNotificationsFetched
{
    public class UserNotificationsFetchedConsumer : BaseConsumer<UserOpenedNotificationsEventMessage>
    {
        private readonly IUpsertUserControlUseCase _upsertUserControlUseCase;
        private readonly ILogger<UserNotificationsFetchedConsumer> _logger;

        public UserNotificationsFetchedConsumer(
            IOptions<KafkaConsumerConfig> config,
            IUpsertUserControlUseCase upsertUserControlUseCase,
            ILogger<UserNotificationsFetchedConsumer> logger) : base(config)
        {
            _upsertUserControlUseCase = upsertUserControlUseCase;
            _logger = logger;
        }

        public override string Mailbox => KafkaTopicsConstants.UserOpenedNotificationsTopic;

        public override async Task ExecuteAsync(UserOpenedNotificationsEventMessage message, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{Worker}] consuming message for id: {id}",
                    nameof(UserNotificationsFetchedConsumer),
                    message.UserId);

                var input = message.MapMessageToInput();

                await _upsertUserControlUseCase.UpsertUserSettingsAsync(input, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {worker}. Message: {message}",
                    nameof(UserNotificationsFetchedConsumer),
                    JsonSerializer.Serialize(message));
            }
        }
    }
}
