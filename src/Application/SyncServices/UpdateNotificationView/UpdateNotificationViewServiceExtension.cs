using Application.SyncServices.UpdateNotificationView.Interface;
using Application.SyncServices.UpdateNotificationView.Models;
using Application.SyncServices.UpdateNotificationView.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.SyncServices.UpdateNotificationView
{
    [ExcludeFromCodeCoverage]
    public static class UpdateNotificationViewServiceExtension
    {
        public static IServiceCollection AddUpdateNotificationViewService(this IServiceCollection services)
        {
            services
                .AddTransient<IUpdateNotificationViewService, UpdateNotificationViewService>()
                .AddTransient<IValidator<UpdateNotificationViewInput>, UpdateNotificationViewInputValidator>();

            return services;
        }
    }
}
