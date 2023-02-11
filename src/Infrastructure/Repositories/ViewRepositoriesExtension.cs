using Domain.Repositories.UserRepository;
using Infrastructure.Repositories.ViewRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories
{
    public static class ViewRepositoriesExtension
    {
        public static IServiceCollection AddViewRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserViewRepository, UserViewRepository>();

            return services;
        }
    }
}
