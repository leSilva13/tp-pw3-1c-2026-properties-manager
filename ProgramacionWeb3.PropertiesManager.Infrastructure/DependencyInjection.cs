using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();

            return services;
        }
    }
}
