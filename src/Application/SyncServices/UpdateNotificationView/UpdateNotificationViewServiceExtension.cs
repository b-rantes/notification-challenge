using Application.SyncServices.UpdateNotificationView.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.CreateUserNotification.Validators;
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
                .AddTransient<IValidator<CreateUserNotificationInput>, CreateUserNotificationInputValidator>();

            return services;
        }
    }
}
