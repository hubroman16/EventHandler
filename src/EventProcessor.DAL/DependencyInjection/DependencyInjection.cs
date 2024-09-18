using EventProcessor.DAL.Repositories;
using EventProcessor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventProcessor.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.InitRepositories();
    }
    
    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventProcessorRepository, EventProcessorRepository>();
    }
}