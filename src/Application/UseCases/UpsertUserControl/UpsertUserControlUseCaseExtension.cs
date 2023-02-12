using Application.UseCases.UpsertUserControl.Interface;
using Application.UseCases.UpsertUserControl.Models;
using Application.UseCases.UpsertUserControl.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.UseCases.UpsertUserControl
{
    [ExcludeFromCodeCoverage]
    public static class UpsertUserControlUseCaseExtension
    {
        public static IServiceCollection AddUpsertUserControlUseCase(this IServiceCollection services)
        {
            services
                .AddTransient<IUpsertUserControlUseCase, UpsertUserControlUseCase>()
                .AddTransient<IValidator<UpsertUserControlInput>, UpsertUserControlInputValidator>();

            return services;
        }
    }
}
