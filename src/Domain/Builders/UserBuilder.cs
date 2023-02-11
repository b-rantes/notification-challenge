using Domain.Entities.UserAggregate;
using Domain.ValueObjects;

namespace Domain.Builders
{
    public static class UserBuilder
    {
        public static User CreateUser() => new User();
        public static User WithId(this User user, long id)
        {
            user.SetId(id);
            return user;
        }

        public static User WithNotificationSettings(this User user, bool isNotificationOn = true)
        {
            user.SetUserNotificationSettings(new(isNotificationOn));
            return user;
        }

        public static User WithNotificationDeliveryControl(this User user, DateTime lastOpenedNotificationDate)
        {
            user.SetUserNotificationDeliveryControl(new UserNotificationDeliveryControl(lastOpenedNotificationDate));
            return user;
        }
    }
}