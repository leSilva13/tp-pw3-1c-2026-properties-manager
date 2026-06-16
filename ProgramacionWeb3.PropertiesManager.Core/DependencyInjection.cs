using Microsoft.Extensions.DependencyInjection;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Core.Services;

namespace ProgramacionWeb3.PropertiesManager.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IMasiveEmailService, MasiveEmailService>();

            return services;
        }
    }
}
