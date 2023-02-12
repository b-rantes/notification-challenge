using Application.SyncServices.UpdateNotificationView;
using Application.SyncServices.UpdateUserView;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class SyncServicesInstaller
    {
        public static IServiceCollection AddSyncServices(this IServiceCollection services)
        {
            services
                .AddUpdateNotificationViewService()
                .AddUpdateUserViewService();

            return services;
        }
    }
}
