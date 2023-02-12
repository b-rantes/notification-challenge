using Application.UseCases.FetchUserNotifications.Models;

namespace Application.UseCases.FetchUserNotifications.Interface
{
    public interface IFetchUserNotificationsUseCase
    {
        public Task<FetchUserNotificationsOutput> FetchUserNotificationsAsync(FetchUserNotificationsInput input, CancellationToken cancellationToken);
    }
}
