namespace Domain.DomainModels.ValueObjects
{
    internal struct UserNotificationSettings
    {
        public UserNotificationSettings()
        {
            CanReceiveNotification = true;
        }

        public UserNotificationSettings(bool isNotificationOn) : this()
        {
            CanReceiveNotification = isNotificationOn;
        }

        public bool CanReceiveNotification { get; private set; }
    }
}
