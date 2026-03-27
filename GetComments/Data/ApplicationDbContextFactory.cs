using GetComments.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GetComments.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<DatabaseOptions>(config.GetSection(DatabaseOptions.PropertyName));
        var sp = services.BuildServiceProvider();

        var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(databaseOptions.ConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options, sp.GetRequiredService<IOptions<DatabaseOptions>>());
    }
}

