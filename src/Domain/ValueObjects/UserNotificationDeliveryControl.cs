using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Domain.ValueObjects
{
    internal struct UserNotificationDeliveryControl
    {
        public UserNotificationDeliveryControl()
        {
        }

        public UserNotificationDeliveryControl(DateTime lastOpenedNotificationDate)
        {
            LastOpenedNotificationDate = lastOpenedNotificationDate;
        }

        public DateTime LastOpenedNotificationDate { get; private set; } = DateTime.MinValue;

        private void ValidateLastOpenedNotificationDate()
        {
            if (LastOpenedNotificationDate == DateTime.MinValue)
                throw new ArgumentException($"Invalid {nameof(UserNotificationDeliveryControl)} initialization");
        }

        internal void Validate() => ValidateLastOpenedNotificationDate();
    }
}
