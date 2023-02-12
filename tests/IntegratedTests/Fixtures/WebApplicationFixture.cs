using Domain;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace IntegratedTests.Fixtures
{
    class WebApplicationFixture : WebApplicationFactory<Program>
    {
        public WebApplicationFixture()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var integratedTestsConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.IntegratedTests.json").Build();

            builder
                .ConfigureAppConfiguration(x =>
                {
                    x.AddConfiguration(integratedTestsConfig);
                })
                .ConfigureTestServices(services =>
                {
                    services
                    .AddDomain()
                    .AddInfrastructure(integratedTestsConfig);
                });
        }
    }
}
