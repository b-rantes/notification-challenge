using Application.UseCases.CreateUserNotification.Interface;
using Application.UseCases.CreateUserNotification.Models;
using Application.UseCases.CreateUserNotification.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.CreateUserNotification
{
    public static class CreateUserNotificationUseCaseExtension
    {
        public static IServiceCollection AddCreateUserNotificationUseCase(this IServiceCollection services)
        {
            services
                .AddTransient<ICreateUserNotificationUseCase, CreateUserNotificationUseCase>()
                .AddTransient<IValidator<CreateUserNotificationInput>, CreateUserNotificationInputValidator>();

            return services;
        }
    }
}
