namespace Domain.ValueObjects
{
    internal struct UserNotificationSettings
    {
        public UserNotificationSettings()
        {
            IsNotificationOn = true;
        }

        public UserNotificationSettings(bool isNotificationOn) : this()
        {
            IsNotificationOn = isNotificationOn;
        }

        public bool IsNotificationOn { get; private set; }
    }
}
