namespace WebApi.Consumers.BaseConsumer
{
    public static class BaseConsumerInstaller
    {
        public static IServiceCollection AddConsumer<TWorker>(this IServiceCollection services) where TWorker: BackgroundService
        {
            services.AddHostedService<TWorker>();

            return services;
        }
    }
}
