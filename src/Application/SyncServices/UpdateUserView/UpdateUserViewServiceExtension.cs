using Application.SyncServices.UpdateUserView.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.CreateUserNotification.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.SyncServices.UpdateUserView
{
    [ExcludeFromCodeCoverage]
    public static class UpdateUserViewServiceExtension
    {
        public static IServiceCollection AddUpdateUserViewService(this IServiceCollection services)
        {
            services
                .AddTransient<IUpdateUserViewService, UpdateUserViewService>()
                .AddTransient<IValidator<CreateUserNotificationInput>, CreateUserNotificationInputValidator>();

            return services;
        }
    }
}
