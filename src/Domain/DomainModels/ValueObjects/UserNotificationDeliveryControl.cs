using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Domain.DomainModels.ValueObjects
{
    internal struct UserNotificationDeliveryControl
    {
        public UserNotificationDeliveryControl()
        {
            LastOpenedNotificationDate = null;
        }

        public UserNotificationDeliveryControl(DateTime? lastOpenedNotificationDate)
        {
            LastOpenedNotificationDate = lastOpenedNotificationDate;
        }

        public DateTime? LastOpenedNotificationDate { get; private set; } = null;

        private void ValidateLastOpenedNotificationDate()
        {
            if (LastOpenedNotificationDate == DateTime.MinValue)
                throw new ArgumentException($"Invalid {nameof(UserNotificationDeliveryControl)} initialization");
        }

        internal void Validate() => ValidateLastOpenedNotificationDate();
    }
}
