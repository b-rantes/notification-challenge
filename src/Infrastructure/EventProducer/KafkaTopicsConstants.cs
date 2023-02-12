namespace Infrastructure.EventProducer
{
    public class KafkaTopicsConstants
    {
        public const string UserNotificationCreatedTopic = "user-notification-created";
        public const string UserOpenedNotificationsTopic = "user-notifications-fetched";
        public const string UserSettingsUpdatedTopic = "user-notificationcontrol-updated";
        public const string CreateUserNotificationTopic = "create-user-notification-command";
    }
}
