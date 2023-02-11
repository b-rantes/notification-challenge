using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Events;
using Domain.Exceptions;
using Domain.Repositories.NotificationRepository;
using Domain.Services.Interfaces;

namespace Domain.Services
{
    public class NotificationManagerDomainService : INotificationManagerDomainService
    {
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly IDomainEventsProducer _domainEventsProducer;

        public NotificationManagerDomainService(
            INotificationCommandRepository notificationCommandRepository, IDomainEventsProducer domainEventsProducer)
        {
            _notificationCommandRepository = notificationCommandRepository;
            _domainEventsProducer = domainEventsProducer;
        }

        public async Task CreateUserNotificationAsync(User user, Notification notification, CancellationToken cancellationToken)
        {
            try
            {
                var userValidationResult = user.Validate();

                if (!userValidationResult.IsValid)
                    throw new DomainException(string.Format("Invalid User: {errorMessage}", string.Join(", ", userValidationResult.GetErrors())));

                if (!user.CanReceiveNotification)
                    throw new DomainException(string.Format("User {id} cannot receive notifications", user.Id));

                var operationResult = await _notificationCommandRepository.SaveIdempotentNotificationAsync(notification, cancellationToken);

                if (!operationResult.NotificationSaved)
                    throw new DomainException(operationResult.ErrorMessage);

                await _domainEventsProducer.ProduceNotificationCreatedEvent(notification, user, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DomainException(ex.Message);
            }
        }
    }
}
