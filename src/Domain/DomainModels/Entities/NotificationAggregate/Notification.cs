using Domain.DomainModels.Entities.Abstractions;
using Domain.Helpers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("IntegratedTests")]
namespace Domain.DomainModels.Entities.NotificationAggregate
{
    public sealed class Notification : IAggregateRoot
    {
        public Notification(Guid notificationId, long userOwnerId)
        {
            NotificationId = notificationId;
            UserOwnerId = userOwnerId;
            NotificationCreationDate = DateTime.UtcNow;
        }

        internal Notification(Guid notificationId, long userOwnerId, DateTime notificationCreationDate, object? notificationContent = null)
        {
            NotificationId = notificationId;
            UserOwnerId = userOwnerId;
            NotificationCreationDate = notificationCreationDate;
            NotificationContent = notificationContent;
        }

        public Guid NotificationId { get; set; }
        public long UserOwnerId { get; internal set; }
        public DateTime NotificationCreationDate { get; internal set; }
        public object? NotificationContent { get; set; }

        public void SetNotificationContent(object? notificationContent) => NotificationContent = notificationContent;

        internal EntityValidationResult Validate()
        {
            var validationResult = new EntityValidationResult();
            try
            {
                if (NotificationId == Guid.Empty) throw new ArgumentException($"Invalid {nameof(Notification)} {NotificationId}");
                if (UserOwnerId <= 0) throw new ArgumentException($"Invalid {nameof(UserOwnerId)} {UserOwnerId}");

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
