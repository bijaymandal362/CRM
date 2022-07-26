using ChangeRequestManagement.Entities.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChangeRequestManagement.Entities.Dependencies
{
    public static class CRMEntitiesDependencies
    {
        public static IServiceCollection RegisterDatabase(this IServiceCollection services, string connectionString) 
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContextPool<CRMDbContext>((options, config) =>
                {
                    config.UseNpgsql(connectionString, x=> 
                    {
                        x.MigrationsAssembly("ChangeRequestManagement.Entities");
                    });
                    config.UseInternalServiceProvider(options);
                });
            return services;
        }
    }
}
