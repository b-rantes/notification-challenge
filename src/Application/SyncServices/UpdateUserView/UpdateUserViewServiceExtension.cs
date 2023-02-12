using Application.SyncServices.UpdateUserView.Interface;
using Application.SyncServices.UpdateUserView.Models;
using Application.SyncServices.UpdateUserView.Validators;
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
                .AddTransient<IValidator<UpdateUserViewInput>, UpdateUserViewInputValidator>();

            return services;
        }
    }
}
