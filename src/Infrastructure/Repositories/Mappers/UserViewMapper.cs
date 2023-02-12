using Domain.Repositories.UserRepository.Models;
using Infrastructure.Repositories.DTOs;

namespace Infrastructure.Repositories.Mappers
{
    public static class UserViewMapper
    {
        public static UserControlView MapUserCollectionToUserControlView(this UserCommandCollection collection) =>
            new UserControlView
            {
                Id = collection.Id,
                CanReceiveNotification = collection.CanReceiveNotification,
                LastOpenedNotificationDate = collection.LastOpenedNotificationDate,
            };
    }
}
