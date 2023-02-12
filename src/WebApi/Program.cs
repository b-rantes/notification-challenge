using Application.DependencyInjection;
using Domain;
using Infrastructure.DependencyInjection;
using Prometheus;
using Serilog;
using System.Runtime.CompilerServices;
using WebApi.DependencyInjection;

[assembly: InternalsVisibleTo("IntegratedTests")]
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.WriteTo.Console();
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services
        .AddDomain()
        .AddUseCases(configuration)
        .AddInfrastructure(configuration)
        .AddSyncServices()
        .AddWorkers(configuration);
}