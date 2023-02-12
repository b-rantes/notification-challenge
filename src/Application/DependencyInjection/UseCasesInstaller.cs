using Application.UseCases.CreateUserNotification;
using Application.UseCases.FetchUserNotifications;
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
                .AddFetchUserNotificationsUseCase();

            return services;
        }
    }
}
