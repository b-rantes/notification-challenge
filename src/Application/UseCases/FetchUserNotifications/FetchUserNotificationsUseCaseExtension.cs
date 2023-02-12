using Application.UseCases.FetchUserNotifications.Interface;
using Application.UseCases.FetchUserNotifications.Models;
using Application.UseCases.FetchUserNotifications.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.UseCases.FetchUserNotifications
{
    [ExcludeFromCodeCoverage]
    public static class FetchUserNotificationsUseCaseExtension
    {
        public static IServiceCollection AddFetchUserNotificationsUseCase(this IServiceCollection services)
        {
            services
                .AddTransient<IFetchUserNotificationsUseCase, FetchUserNotificationsUseCase>()
                .AddTransient<IValidator<FetchUserNotificationsInput>, FetchUserNotificationsInputValidator>();

            return services;
        }
    }
}
