using Domain.Repositories.NotificationRepository;
using Infrastructure.Repositories.CommandRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories
{
    public static class CommandRepositoriesExtension
    {
        public static IServiceCollection AddCommandRepositories(this IServiceCollection services)
        {
            services.AddTransient<INotificationCommandRepository, NotificationCommandRepository>();

            return services;
        }
    }
}
