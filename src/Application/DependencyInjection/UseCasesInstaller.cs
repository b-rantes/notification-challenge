using Application.UseCases.CreateUserNotification;
using Application.UseCases.FetchUserNotifications;
using Application.UseCases.UpsertUserControl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class UseCasesInstaller
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCreateUserNotificationUseCase()
                .AddFetchUserNotificationsUseCase()
                .AddUpsertUserControlUseCase();

            return services;
        }
    }
}
