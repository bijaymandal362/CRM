using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.BusinessLayer.Services.Implementation;
using ChangeRequestManagement.Entities.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace ChangeRequestManagement.BusinessLayer.Dependencies
{
    public static class CRMBusinessLayerDependencies
    {
        public static IServiceCollection RegisterDatabaseConnection(this IServiceCollection services, string connectionString)
        {
            services.RegisterDatabase(connectionString);
            return services;
        }

        public static IServiceCollection RegisterAllDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IChangeRequestService, ChangeRequestService>();
            services.AddScoped<IChangeRequestDocumentService, ChangeRequestDocumentService>();
            services.AddScoped<IChangeRequestStatusService, ChangeRequestStatusService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}