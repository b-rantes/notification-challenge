using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.DomainModels.Entities.UserAggregate;

namespace Domain.Events
{
    public interface IDomainEventsProducer
    {
        public Task ProduceNotificationCreatedEvent(Notification notification, User user, CancellationToken cancellationToken);
        public Task ProduceUserSettingsUpdatedEvent(ProduceUserSettingsUpdatedInput userSettingsUpdatedInput, CancellationToken cancellationToken);
        public Task ProduceUserOpenedNotificationsEvent(User user, CancellationToken cancellationToken);
    }
}
