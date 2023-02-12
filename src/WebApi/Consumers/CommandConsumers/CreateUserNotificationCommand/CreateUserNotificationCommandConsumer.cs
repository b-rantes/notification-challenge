using Application.UseCases.CreateUserNotification.Interface;
using Infrastructure.EventProducer;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Consumers.BaseConsumer;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Mappers;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Model;

namespace WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand
{
    public class CreateUserNotificationCommandConsumer : BaseConsumer<CreateUserNotificationCommandMessage>
    {
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;
        private readonly ILogger<CreateUserNotificationCommandConsumer> _logger;

        public CreateUserNotificationCommandConsumer(
            IOptions<KafkaConsumerConfig> config,
            ICreateUserNotificationUseCase createUserNotificationUseCase,
            ILogger<CreateUserNotificationCommandConsumer> logger) : base(config)
        {
            _createUserNotificationUseCase = createUserNotificationUseCase;
            _logger = logger;
        }

        public override string Mailbox => KafkaTopicsConstants.CreateUserNotificationTopic;

        public override async Task ExecuteAsync(CreateUserNotificationCommandMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var input = message.MapMessageToInput();

                await _createUserNotificationUseCase.CreateUserNotificationAsync(input, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {worker}. Message: {message}",
                    nameof(CreateUserNotificationCommandConsumer),
                    JsonSerializer.Serialize(message));
            }
        }
    }
}