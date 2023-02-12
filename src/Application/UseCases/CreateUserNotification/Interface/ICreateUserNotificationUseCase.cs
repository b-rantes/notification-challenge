using Application.UseCases.CreateUserNotification.Models;

namespace Application.UseCases.CreateUserNotification.Interface
{
    public interface ICreateUserNotificationUseCase
    {
        public Task<CreateUserNotificationOutput> CreateUserNotificationAsync(CreateUserNotificationInput input, CancellationToken cancellationToken);
    }
}
