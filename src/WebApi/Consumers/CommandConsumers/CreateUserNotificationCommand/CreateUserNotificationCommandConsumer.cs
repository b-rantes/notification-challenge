using Application.UseCases.CreateUserNotification.Interface;
using Hangfire;
using Infrastructure.EventProducer;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebApi.Consumers.Base;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Mappers;
using WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand.Model;

namespace WebApi.Consumers.CommandConsumers.CreateUserNotificationCommand
{
    public class CreateUserNotificationCommandConsumer : BaseConsumer<CreateUserNotificationCommandMessage>
    {
        private readonly ICreateUserNotificationUseCase _createUserNotificationUseCase;
        private readonly ILogger<CreateUserNotificationCommandConsumer> _logger;
        private readonly IBaseProducer _producer;

        public CreateUserNotificationCommandConsumer(
            IOptions<KafkaConsumerConfig> config,
            ICreateUserNotificationUseCase createUserNotificationUseCase,
            ILogger<CreateUserNotificationCommandConsumer> logger,
            IBaseProducer producer) : base(config)
        {
            _producer = producer;
            _createUserNotificationUseCase = createUserNotificationUseCase;
            _logger = logger;
        }

        public override string Mailbox => KafkaTopicsConstants.CreateUserNotificationTopic;

        public override async Task ExecuteAsync(CreateUserNotificationCommandMessage message, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("[{Worker}] consuming message for id: {id}", nameof(CreateUserNotificationCommandConsumer), message.UserId);

                if (message.ScheduledNotificationUtcDate.HasValue)
                {
                    BackgroundJob.Schedule(() =>
                    ConsumeScheduledNotification(message, cancellationToken), message.ScheduledNotificationUtcDate.Value - DateTime.UtcNow);

                    return;
                }

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

        public void ConsumeScheduledNotification(CreateUserNotificationCommandMessage input, CancellationToken cancellationToken)
        {
            var message = new CreateUserNotificationCommandMessage
            {
                NotificationContent = input.NotificationContent,
                NotificationId = input.NotificationId,
                ScheduledNotificationUtcDate = null,
                UserId = input.UserId,
            };

            _ = _producer.ProduceAsync(Mailbox, message.UserId.ToString(), message, cancellationToken);
        }
    }
}