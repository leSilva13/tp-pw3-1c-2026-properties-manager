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

            services.AddScoped<IBuildingService, BuildingService>();

            services.AddScoped<IUnitService, UnitService>();

            services.AddScoped<IOwnerService, OwnerService>();

            return services;
        }
    }
}
