using Domain.DomainModels.Entities.Abstractions;
using Domain.DomainModels.ValueObjects;
using Domain.Helpers;

namespace Domain.DomainModels.Entities.UserAggregate
{
    public sealed class User : IAggregateRoot
    {
        internal User()
        {
        }

        public long Id { get; internal set; } = -1;
        public bool CanReceiveNotification => UserNotificationSettings.CanReceiveNotification;
        public DateTime? LastOpenedNotificationDate => UserNotificationDeliveryControl.LastOpenedNotificationDate;

        public void TurnOnNotifications() => SetUserNotificationSettings(new(isNotificationOn: true));
        public void TurnOffNotifications() => SetUserNotificationSettings(new(isNotificationOn: false));
        public void OpenNotification() => SetUserNotificationDeliveryControl(new(lastOpenedNotificationDate: DateTime.UtcNow));

        internal UserNotificationDeliveryControl UserNotificationDeliveryControl { get; set; }
        internal UserNotificationSettings UserNotificationSettings { get; set; }

        internal void SetId(long value) => Id = value;
        internal void SetUserNotificationDeliveryControl(UserNotificationDeliveryControl value) => UserNotificationDeliveryControl = value;
        internal void SetUserNotificationSettings(UserNotificationSettings value) => UserNotificationSettings = value;

        public EntityValidationResult Validate()
        {
            var validationResult = new EntityValidationResult();
            try
            {
                if (Id == default ||
                   Id < 0) throw new ArgumentException($"Invalid {nameof(User)} {nameof(Id)}");
                UserNotificationDeliveryControl.Validate();
                return validationResult;
            }
            catch (Exception ex)
            {
                validationResult.AddErrorMessage(ex.Message);
                return validationResult;
            }
        }
    }
}