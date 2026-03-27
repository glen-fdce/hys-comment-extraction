using GetComments.Extensions;
using GetComments.Options;
using GetComments.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GetComments;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices((context, services) =>
        {
            var config = context.Configuration;
            services.AddDatabase(config);
            services.Configure<HysOptions>(config.GetSection(HysOptions.PropertyName));
            var hysOptions = config.GetSection(HysOptions.PropertyName).Get<HysOptions>();
            services.AddHttpClient("HYS", client =>
            {
                client.BaseAddress = new Uri(hysOptions!.BaseUrl);
            });
            services.AddSingleton<BBCCommunicationService>();
            services.AddSingleton<DatabaseService>();
            services.AddSingleton<MainExecution>();
        });

        var app = builder.Build();
        
        var mainExecution = app.Services.GetRequiredService<MainExecution>();
        await mainExecution.RunAsync();
    }
}