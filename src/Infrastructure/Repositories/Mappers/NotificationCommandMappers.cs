using Domain.DomainModels.Entities.NotificationAggregate;
using Domain.Repositories.NotificationRepository.Models;
using Infrastructure.Repositories.DTOs;
using MongoDB.Bson;
using System.Text.Json;

namespace Infrastructure.Repositories.Mappers
{
    public static class NotificationCommandMappers
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static NotificationCommandCollection MapNotificationToNotificationCommandCollection(this Notification notification)
        {
            var document = new NotificationCommandCollection
            {
                NotificationId = notification.NotificationId.ToString(),
                UserOwnerId = notification.UserOwnerId,
                NotificationCreationDate = notification.NotificationCreationDate
            };

            if (BsonDocument.TryParse(JsonSerializer.Serialize(notification.NotificationContent, _options), out var notificationContent))
                document.NotificationContent = notificationContent;

            return document;
        }

        public static NotificationsViewByUserOutput MapNotificationCollectionToViewOutput(this List<NotificationCommandCollection> collection)
        {
            return new NotificationsViewByUserOutput
            {
                UserId = collection.First().UserOwnerId,
                Notifications = collection.Select(x => new NotificationViewItem
                {
                    NotificationContent = x.NotificationContent,
                    NotificationCreationDate = x.NotificationCreationDate,
                    NotificationId = x.NotificationId
                }).ToList(),
            };
        }
    }
}
