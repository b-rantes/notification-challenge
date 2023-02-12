using Application.UseCases.UpsertUserControl.Models;
using Domain.Services.Models;

namespace Application.UseCases.UpsertUserControl.Mapper
{
    public static class UpsertUserControlInputMapper
    {
        public static UpsertUserInput MapInputToUpsertUserInput(this UpsertUserControlInput input)
            => new UpsertUserInput
            {
                Id = input.UserId,
                CanReceiveNotification = input.CanReceiveNotification,
                LastOpenedNotificationDate = input.LastOpenedNotificationDate,
            };
    }
}
