using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Domain
{
    [ExcludeFromCodeCoverage]
    public static class DomainInstaller
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<INotificationManagerDomainService, NotificationManagerDomainService>();
            services.AddTransient<IUserManagerDomainService, UserManagerDomainService>();

            return services;
        }
    }
}
