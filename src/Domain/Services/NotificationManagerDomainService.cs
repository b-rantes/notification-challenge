using Domain.Builders;
using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;
using Domain.Events;
using Domain.Exceptions;
using Domain.Repositories.NotificationRepository;
using Domain.Repositories.UserRepository;
using Domain.Services.Interfaces;
using Domain.Services.Mappers;
using Domain.Services.Models;

namespace Domain.Services
{
    public class NotificationManagerDomainService : INotificationManagerDomainService
    {
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly IDomainEventsProducer _domainEventsProducer;
        private readonly INotificationViewRepository _notificationViewRepository;
        private readonly IUserViewRepository _userViewRepository;

        public NotificationManagerDomainService(
            INotificationCommandRepository notificationCommandRepository,
            IDomainEventsProducer domainEventsProducer,
            INotificationViewRepository notificationViewRepository,
            IUserViewRepository userViewRepository)
        {
            _userViewRepository = userViewRepository;
            _notificationViewRepository = notificationViewRepository;
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

        public async Task<UserNotificationsOutput> FetchUserNotificationsAsync(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var notificationsTask = _notificationViewRepository.GetNotificationsByUserId(userId, cancellationToken);
                var userControlViewTask = _userViewRepository.GetUserById(userId, cancellationToken);

                await Task.WhenAll(notificationsTask, userControlViewTask);

                var notifications = await notificationsTask;
                var userControlView = await userControlViewTask;

                var user = UserBuilder.CreateUser()
                    .WithId(userId)
                    .WithNotificationSettings(userControlView.CanReceiveNotification)
                    .WithNotificationDeliveryControl(userControlView.LastOpenedNotificationDate);

                var lastOpenedNotificationDateReference = user.LastOpenedNotificationDate;

                user.OpenNotification();

                _ = _domainEventsProducer.ProduceUserOpenedNotificationsEvent(user, cancellationToken);

                return notifications.MapNotificationsByUserToOutput(lastOpenedNotificationDateReference.HasValue ?
                    lastOpenedNotificationDateReference.Value : DateTime.MinValue);
            }
            catch (Exception ex)
            {
                throw new DomainException(ex.Message);
            }
        }
    }
}
