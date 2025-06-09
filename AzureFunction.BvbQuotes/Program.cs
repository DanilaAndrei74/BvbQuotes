using AzureFunction.BvbQuotes.Configuration;
using AzureFunction.BvbQuotes.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((builder, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((builder, services) => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        builder.Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
        
        services.AddScoped<WebPageDownloader>();
    })
    .Build();


host.Run();
