using ChangeRequestManagement.Entities.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder)
                .Enrich.WithProperty("SourceContext", null)
                .WriteTo.File("CRMlogs.log", outputTemplate: builder.GetSection("Pattern").Value)
                .WriteTo.Console(outputTemplate: builder.GetSection("Pattern").Value)
                .CreateLogger();

            try
            {
                Log.Information("Starting the application");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<CRMDbContext>();
                    db.Database.Migrate();
                }
                host.Run();
                Log.Information("Application has started.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Application failed to start.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
