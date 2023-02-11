using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class DomainInstaller
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<INotificationManagerDomainService, NotificationManagerDomainService>();

            return services;
        }
    }
}
